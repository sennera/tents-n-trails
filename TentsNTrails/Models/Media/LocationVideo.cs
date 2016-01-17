using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TentsNTrails.Models
{
    /// <summary>
    /// Extends Video to include a LocationID field.
    /// </summary>
    public class LocationVideo : Video
    {
        [Required]
        [Display(Name = "Location")]
        public int LocationID { get; set; }
        public virtual Location Location { get; set; }
    }

}