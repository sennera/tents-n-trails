using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    /// <summary>
    /// Used to Edit a single location's natural features.
    /// </summary>
    public class EditRecreationsViewModel
    {
        [Required]
        public int LocationID { get; set; }
        [Required]
        public string LocationLabel { get; set; }
        public Location Location { get; set; }
        public ICollection<string> AllRecreations { get; set; }
        [Required(ErrorMessage = "You must select at least one Recreation type.")]
        public ICollection<string> SelectedRecreations { get; set; }
    }
}