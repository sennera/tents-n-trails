using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    public class LocationRecreation
    {
        [Key]
        [Column(Order = 0)]
        public int LocationID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int RecreationID { get; set; }

        public String RecreationLabel { get; set; }

        public bool IsChecked { get; set; }

        public virtual Location Location { get; set; }
        public virtual Recreation Recreation { get; set; }


        /// <summary>
        /// Required default constructor (empty)
        /// </summary>
        public LocationRecreation() { }

        /// <summary>
        /// Create a new LocationRecreation.
        /// </summary>
        /// <param name="locationID">The index of the Location to associate.</param>
        /// <param name="recreationID">The index of the Recreation to associate.</param>
        public LocationRecreation(int locationID, int recreationID)
        {
            this.LocationID = locationID;
            this.RecreationID = recreationID;
        }
    }
}