using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TentsNTrails.Models
{
    //possible model to adding Recreation data to Locations; need to come up with a variable-length form
    public class RecreationData
    {
        public virtual ICollection<Recreation> Recreations { get; set; }
    }
}