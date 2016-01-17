using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Xml.Linq;

namespace TentsNTrails.Models
{
    /**
     * Location stores all Location-related data in a model.  For now I decided to try out
     * using the DbGeography datatype as it is designed to handle geo-coordinates.
     * 
     * See the following link for helpful information on how and why I chose this datatype:
     * http://weblog.west-wind.com/posts/2012/Jun/21/Basic-Spatial-Data-with-SQL-Server-and-Entity-Framework-50
     */
    public class Location
    {
        public static readonly Location CENTER = new Location("Center of the United States", 39.8282, -98.5795);

        // for difficulty rating below
        public enum DifficultyRatings
        {
            [Display(Name = "Easy")]
            Easy,
            [Display(Name = "Medium")]
            Medium,
            [Display(Name = "Hard")]
            Hard,
            [Display(Name = "Varies")]
            Varies,
            [Display(Name = "NA")]
            NA
        }

        [Key]
        public int LocationID { get; set; }

        [Display(Name = "Name")]
        [Required]
        public String Label { get; set; }

        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Required]
        [Range(-180,180)]
        public double Longitude { get; set; }

        // date created
        [DataType(DataType.DateTime)]
        [Display(Name = "Created")]
        [Editable(false)]
        public DateTime? DateCreated { get; set; }

        // date modified
        [DataType(DataType.DateTime)]
        [Display(Name = "Modified")]
        [Editable(false)]
        public DateTime? DateModified { get; set; }

        // difficulty rating
        [DisplayFormat(NullDisplayText = "No Rating")]
        public DifficultyRatings Difficulty { get; set; }

        // description of the location
        [DataType(DataType.MultilineText)]
        [StringLength(250)]
        public string Description { get; set; }

        //public int StateID { get; set; }
        
        [Display(Name = "Recreation Tags")]
        public virtual ICollection<LocationRecreation> RecOptions { get; set; }
        public virtual ICollection<Recreation> Recreations { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<LocationFlag> LocationFlags { get; set; }
        public virtual ICollection<LocationImage> Images { get; set; }
        public virtual ICollection<LocationVideo> Videos { get; set; }

        [ForeignKey("State")]
        public string StateID { get; set; }
        public virtual State State { get; set; }

        /// <summary>
        /// NavigationProperty to the LocationFeature bridge entity.
        /// (A Location can have zero or more NaturalFeatures, associated via LocationFeatures).
        /// </summary>
        public virtual ICollection<LocationFeature> LocationFeatures { get; set; }

        // a convenience preview image url.
        public virtual String PreviewImageURL
        { 
            get
            {
                LocationImage image = Images.FirstOrDefault();
                return image == null ? Image.NO_IMAGE_AVAILABLE_URL : image.ImageUrl;          
            }
        }

        // calculate the overall rating of this Location, based off of its Reviews.
        public virtual double Rating() {
            if (Reviews != null && Reviews.Count != 0)
            {
                double up = 0;
                for (int i = 0; i < Reviews.Count; i++)
                {
                    up = Reviews.Count(item => item.Rating);
                }
                return up / Reviews.Count;
            }
            else
            {
                return 0;
            }
        }

        // get the number of upvotes for this Location from its associated Reviews.
        public virtual int UpVotes()
        {
            if (Reviews != null && Reviews.Count != 0)
            {
                int up = 0;
                for (int i = 0; i < Reviews.Count; i++)
                {
                    up = Reviews.Count(item => item.Rating);
                }
                return up;
            }
            else
            {
                return 0;
            }
        }

        // get the number of downvotes for this Location from its associated Reviews.
        public virtual int DownVotes()
        {
            if (Reviews != null && Reviews.Count != 0)
            {
                int up = 0;
                for (int i = 0; i < Reviews.Count; i++)
                {
                    up = Reviews.Count(item => item.Rating);
                }
                return Reviews.Count - up;
            }
            else
            {
                return 0;
            }
        }


        // Handles newlines in a string for html markup by replacing each with a <br> tag.
        public string GetDescriptionMarkup()
        {
            if (Description != null)
            {
                return Description.Replace(Environment.NewLine, "<br />");
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Used for review index page, to give only a preview of the comment
        /// </summary>
        /// <returns>A shortened description that </returns>
        public string GetDescriptionPreview(int length = 50)
        {
            if (Description == null) return "";
            if (Description.Length < length) return Description.Replace(Environment.NewLine, " ");
            else return Description.Substring(0, length - 3).Replace(Environment.NewLine, " ") + "...";
        }

        /// <summary>
        /// The base URI for the Google Geocoding API.
        /// </summary>
        public const string GEOCODE_BASE_URI = "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false";    

        /// <summary>
        /// Retrieve the State for the specified Location using Google's Reverse Geocoding API.
        /// Use this to set the value of a Location initially.  Use Location's State property 
        /// to access information about a Location's State during regular use.
        /// </summary>
        /// <returns>The Abbreviation of the US State of this Location.</returns>
        public static string ReverseGeocodeState(Location location)
        {
            string state = null;
            string requestUri = string.Format(GEOCODE_BASE_URI, location.Latitude.ToString(), location.Longitude.ToString());
            using (WebClient wc = new WebClient())
            {
                string result = wc.DownloadString(requestUri);
                var xmlElm = XElement.Parse(result);
                var status = (from elm in xmlElm.Descendants()
                              where elm.Name == "status"
                              select elm).FirstOrDefault();
                if (status.Value.ToLower() == "ok")
                {
                    var res = xmlElm.Descendants("address_component")
                        .Where(x => (string)x.Element("type") == "administrative_area_level_1"
                        )
                        .Select(x => (string)x.Element("short_name")).FirstOrDefault();

                    state = res.ToString();
                }
            }
            return state;
        }

        /// <summary>
        /// Retrieve the Country for the specified Location using Google's Reverse Geocoding API.
        /// Use this to check the value of a Location initially.  If not in the USA, we cannot add it.
        /// </summary>
        /// <returns>The coutry name of this Location.</returns>
        public static string ReverseGeocodeCountry(double latitute, double longitude)
        {
            string country = null;
            string requestUri = string.Format(GEOCODE_BASE_URI, latitute.ToString(), longitude.ToString());
            using (WebClient wc = new WebClient())
            {
                string result = wc.DownloadString(requestUri);
                var xmlElm = XElement.Parse(result);
                var status = (from elm in xmlElm.Descendants()
                              where elm.Name == "status"
                              select elm).FirstOrDefault();

                if (status.Value.ToLower().Equals("ok"))
                {
                    var res = xmlElm.Descendants("address_component")
                        .Where(x => (string)x.Element("type") == "country")
                        .Select(x => (string)x.Element("short_name")).FirstOrDefault();


                    country = res.ToString();
                }
                else
                {
                    country = null;
                }
            }
            return country;
        }

        /// <summary>
        /// Convenience method to calculate the centerpoint of latlong coordinates using trigonometry
        /// (solution used from http://stackoverflow.com/questions/6671183/calculate-the-center-point-of-multiple-latitude-longitude-coordinate-pairs)
        /// </summary>
        /// <param name="locations">The list of locations with latlong data to average.</param>
        /// <returns>A temp location containing the averaged data.</returns>
        public static Location GetLatLongCenter(ICollection<Location> locations)
        {
            int count = locations.Count;

            // return center of US if no locations.
            if (count == 0)
            {
                // for now, use this: the below logic doesn't seem to be too accurate.  it is close, though ...
                return Location.CENTER;
            }

            double x = 0;
            double y = 0;
            double z = 0;

            foreach (Location l in locations)
            {
                // convert to radians
                double latitude = l.Latitude * Math.PI / 180;
                double longitude = l.Longitude * Math.PI / 180;

                // convert to cartesian coordinates
                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
            }

            // average cartesian coordinates
            x /= count;
            y /= count;
            z /= count;

            // convert back to latitude/longitude
            double hypoteneuse = Math.Sqrt(x * x + y * y);
            double centerLatitude = Math.Atan2(z, hypoteneuse) * 180 / Math.PI;
            double centerLongitude = Math.Atan2(y, x) * 180 / Math.PI;

            // store results in a temporary location.
            return new Location()
            {
                Latitude = centerLatitude,
                Longitude = centerLongitude
            };
        }

        public Location() { }

        public Location(string label, double latitude, double longitude)
        {
            this.Label = label;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public override int GetHashCode()
        {
            return this.LocationID;
        }

        public override bool Equals(object obj)
        {
            if (obj is Location)
                return ((Location)obj).LocationID == this.LocationID;
            return false;
        }
    }
}