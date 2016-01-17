using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    public class ImageViewModel
    {
        // html Title
        [Required]
        public string Title { get; set; }

        // Holds the uploaded image file
        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}