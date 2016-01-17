using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    public enum  Flag {
        [Display(Name = "Have Been")]
        HaveBeen,
        [Display(Name = "Want to Go")]
        WantToGo,
        [Display(Name = "Want to Go Again")]
        GoAgain
    }

    public class LocationFlag
    {
        [Key]
        public int FlagID { get; set; }
        //public int UserID { get; set; }
        public int LocationID { get; set; }
        public Flag Flag { get; set; }

        public virtual User User { get; set; }
        public virtual Location Location { get; set; }
    }
}