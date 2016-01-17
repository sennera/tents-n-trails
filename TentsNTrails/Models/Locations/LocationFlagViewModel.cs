using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    public class LocationFlagViewModel
    {
        public virtual ICollection<LocationFlag> BeenThereLocations { get; set; }
        public virtual ICollection<LocationFlag> WantToGoLocations { get; set; }
        public virtual ICollection<LocationFlag> GoAgainLocations { get; set; }
        public bool HasSavedLocations { get; set; }
    }
}