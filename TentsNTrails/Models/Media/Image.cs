using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TentsNTrails.Models
{
    /// <summary>
    /// Store an Image in the database using a string url.
    /// http://cpratt.co/file-uploads-in-asp-net-mvc-with-view-models/
    /// </summary>
    public class Image
    {
        public const string DEFAULT_PROFILE_PICTURE_URL = "~/Content/default-user-image.png";
        public const string NO_IMAGE_AVAILABLE_URL = "~/Content/no-image-available.png";


        [Key]
        public int ImageID { get; set; }

        // html title
        [Required]
        public string Title { get; set; }

        // html alt text
        [Display(Name = "Alternate Text")]
        public string AltText { get; set; }

        // URL to the image
        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name="Image URL")]
        public string ImageUrl { get; set; }

        // The date the user took the photograph (optional)
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Taken")]
        [DisplayFormat(NullDisplayText = "(Not Specified)")]
        public DateTime? DateTaken { get; set; }

        // upload date
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Created")]
        [Editable(false)]
        public DateTime? DateCreated { get; set; }

        // edited date
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Modified")]
        [Editable(false)]
        public DateTime? DateModified { get; set; }
    }

}