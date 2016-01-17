using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TentsNTrails.Models
{
   

    /// <summary>
    /// Used to display images in video in the Media page.
    /// </summary>
    public class LocationMediaViewModel
    {
        public virtual ICollection<LocationImage> Images { get; set; }
        public virtual ICollection<LocationVideo> Videos { get; set; }
    }
    
    /// <summary>
    /// Used to upload or edit a LocationImage.
    /// http://cpratt.co/file-uploads-in-asp-net-mvc-with-view-models/
    /// </summary>
    public class LocationImageUrlViewModel
    {
        // The associated Location
        [Required]
        [Display(Name = "Location")]
        public int LocationID { get; set; }

        // html Title
        [Required]
        public string Title { get; set; }

        // html alt text
        public string AltText { get; set; }

        // The date the user took the photograph (optional)
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Taken (optional)")]
        public DateTime? DateTaken { get; set; }

        // URL to the image
        // Length check from suggestions at
        // http://stackoverflow.com/questions/417142/what-is-the-maximum-length-of-a-url-in-different-browsers
        [Required]
        [StringLength(2000, MinimumLength = 5)]
        public string ImageUrl { get; set; }

        // User
        public virtual User User { get; set; }
    }

    /// <summary>
    /// Used to upload or edit a LocationImage.
    /// http://cpratt.co/file-uploads-in-asp-net-mvc-with-view-models/
    /// </summary>
    public class LocationImageViewModel
    {
        // The associated Location
        [Required]
        [Display(Name = "Location")]
        public int LocationID { get; set; }

        // html Title
        [Required]
        public string Title { get; set; }

        // The date the user took the photograph (optional)
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Taken (optional)")]
        public DateTime? DateTaken { get; set; }

        // Holds the uploaded image file
        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageUpload { get; set; }
    }

    

}