using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    /// <summary>
    /// NaturalFeatures are words like Forests, Lakes, Mountains, Rivers, etc
    /// that users can associate with a location.  This allows users to do 
    /// things like search by these words. (see other stories)
    /// </summary>
    public class NaturalFeature
    {
        /// <summary>
        /// The index of the NaturalFeature.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// The Name of the NaturalFeature.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// NavigationProperty to the LocationFeature bridge entity.
        /// (A NaturalFeature can have zero or more Locations, associated by a 
        /// LocationFeature).
        /// </summary>
        public virtual ICollection<LocationFeature> LocationFeatures { get; set; }

        /// <summary>
        /// Create a new NaturalFeature with uninitialized values.
        /// </summary>
        public NaturalFeature() { }

        /// <summary>
        /// Create a new NaturalFeature with the given name.
        /// </summary>
        /// <param name="name">The name for the NaturalFeature.</param>
        public NaturalFeature(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return String.Format("NaturalFeature '{0}'", Name ?? "null");
        }

    }
}