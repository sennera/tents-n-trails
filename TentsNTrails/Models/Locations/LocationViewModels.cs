using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PagedList;
using System.Web.Mvc;

// File holds all ViewModels associated with Location.

namespace TentsNTrails.Models
{
    /// <summary>
    /// Used by Index to display Locations and their associated Recreation Tags.
    /// </summary>
    public class JoinLocationsViewModel
    {
        public IPagedList<Location> LocationA { get; set; }
        public IPagedList<Location> LocationB { get; set; }
    }

    /// <summary>
    /// Designed for use with the Location/Index view.
    /// </summary>
    public class LocationIndexViewModel
    {
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Location> TopRatedLocations { get; set; }
        public virtual ICollection<Location> MostRecentLocations { get; set; }
        public virtual ICollection<Location> PersonalRecommendations { get; set; }
        public virtual ICollection<Location> FriendRecommendations { get; set; }
        public virtual IEnumerable<SelectListItem> Recreations { get; set; }
    }

    /// <summary>
    /// Designed for use with the Location/Browse view.
    /// </summary>
    public class BrowseLocationsViewModel
    {
        //The resulting locations from the filter/search
        public virtual IPagedList<Location> Locations { get; set; }
        public virtual ICollection<Location> AllLocations { get; set; }
        // property used to hold the list of possible recreations shown in the dropdown
        public virtual IEnumerable<SelectListItem> Recreations { get; set; }

        [Display(Name = "Recreation Filter")]
        public virtual int recreationID { get; set; }
        [Display(Name = "Search Query")]
        public virtual String query { get; set; }
        public virtual int? page { get; set; }
    }

    /// <summary>
    /// Used to edit a Location
    /// </summary>
    public class EditLocationViewModel
    {
        [Required]
        public int LocationID { get; set; }

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

        [Required]
        [Display(Name = "Name")]
        public String Label { get; set; }

        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public double Longitude { get; set; }

        [StringLength(250)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public DifficultyRatings Difficulty { get; set; }

        [Required(ErrorMessage = "You must select at least one Recreation type.")]
        public ICollection<string> SelectedRecreations { get; set; }
        public ICollection<string> AllRecreations { get; set; }
 
        public ICollection<string> SelectedFeatures { get; set; }
        public ICollection<string> AllNaturalFeatures { get; set; }
        
        /// <summary>
        /// Required default constructor
        /// </summary>
        public EditLocationViewModel(){ }

        /// <summary>
        /// Initialize the values from the given location.
        /// </summary>
        /// <param name="location"></param>
        public EditLocationViewModel(Location location)
        {
            this.LocationID = location.LocationID;
            this.Label = location.Label;
            this.Latitude = location.Latitude;
            this.Longitude = location.Longitude;
            this.Description = location.Description;
        }
    }

    public class CreateLocationViewModel
    {
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

        [Required]
        [Display(Name = "Name")]
        public String Label { get; set; }

        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public double Longitude { get; set; }

        [StringLength(250)]
        [DataType(DataType.MultilineText)]

        public string Description { get; set; }
        
        [Required]
        public DifficultyRatings Difficulty { get; set; }

        [Required(ErrorMessage = "You must select at least one Recreation type.")]
        public ICollection<string> SelectedRecreations { get; set; }
        public ICollection<string> AllRecreations { get; set; }
        
        public ICollection<string> SelectedFeatures { get; set; }
        public ICollection<string> AllNaturalFeatures { get; set; }

    }

    /// <summary>
    /// Used to abstract away data needed for a map.
    /// </summary>
    public class MapViewModel
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int MinZoom { get; set; }
        public Location Center { get; set; }
        public ICollection<Location> Locations { get; set; }

        /// <summary>
        /// Create a MapViewModel, holding all data needed to render a map using the Google Maps API.
        /// </summary>
        /// <param name="height">the height of the map.</param>
        /// <param name="width">the width of the map.</param>
        /// <param name="minZoom">the minimum zoom on loading.</param>
        /// <param name="center">The center of the map.</param>
        /// <param name="locations">The list of locations to render as markers on this map.</param>
        public MapViewModel(int height, int width, int minZoom, Location center, ICollection<Location> locations)
        {
            this.Height = height;
            this.Width = width;
            this.MinZoom = minZoom;
            this.Center = center;
            this.Locations = locations;
        }

    }

}