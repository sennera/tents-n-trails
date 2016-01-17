using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    public class UserRecreation
    {
        [Key][Column(Order = 0)]
        public String User { get; set; }
        [Key]
        [Column(Order = 1)]
        public int RecreationID { get; set; }

        public String RecreationLabel { get; set; }

        public bool IsChecked { get; set; }

        [ForeignKey("RecreationID")]
        public virtual Recreation Recreation { get; set; }
    }
}