using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    /// <summary>
    /// This is a pure bridge entity between a Location and a NaturalFeature.
    /// </summary>
    public class LocationFeature
    {
        /// <summary>
        /// Index of the associated Location.
        /// </summary>
        [Key,  ForeignKey("Location"), Column(Order = 0)]
        public int LocationID { get; set; }

        /// <summary>
        /// Index of the associated NaturalFeature.
        /// </summary>
        [Key, ForeignKey("NaturalFeature"), Column(Order = 1)]
        public int NaturalFeatureID { get; set; }

        /// <summary>
        /// The Location referenced by this LocationFeature.
        /// </summary>
        public virtual Location Location { get; set; }

        /// <summary>
        /// The NaturalFeature referenced by this LocationFeature.
        /// </summary>
        public virtual NaturalFeature NaturalFeature { get; set; }



        /// <summary>
        /// Create a new LocationFeature with uninitialized fields.
        /// </summary>
        public LocationFeature() { }

        /// <summary>
        /// Create a new LocationFeature.
        /// </summary>
        /// <param name="locationID">The index of the Location to associate.</param>
        /// <param name="naturalFeatureID">The index of the NaturalFeature to associate.</param>
        public LocationFeature(int locationID, int naturalFeatureID)
        {
            this.LocationID = locationID;
            this.NaturalFeatureID = naturalFeatureID;
        }
    }
}