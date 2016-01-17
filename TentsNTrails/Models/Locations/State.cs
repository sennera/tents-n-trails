using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    
    /// <summary>
    /// Contains helpful data persisting to one of the 50 states of the US.  Used in conjunction wtih
    /// the Location's State property from Reverse-Geocoding API.  This is in a model so that LINQ
    /// and SQL queries can be performed on the States.
    /// </summary>
    public class State
    {
        [Key]
        [MinLength(2), MaxLength(2)]
        [Display(Name = "Abbreviation")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string StateID { get; set; }

        [Display(Name="State")]
        public string Name { get; set; }

        public virtual List<Location> Locations { get; set; }

        /// <summary>
        /// Create an uninitialized State.
        /// </summary>
        public State(){}

        /// <summary>
        /// Create a State with the specified Abbreviation and Name.
        /// </summary>
        /// <param name="abbreviation">The abbreviation of the State.</param>
        /// <param name="name">The name of the State.</param>
        public State(string abbreviation, string name)
        {
            this.StateID = abbreviation;
            this.Name = name;
        }
    }
}