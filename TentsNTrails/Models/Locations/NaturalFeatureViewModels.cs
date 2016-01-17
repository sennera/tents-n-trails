using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{

    /// <summary>
    /// Used for the NaturalFeatures/Details view.
    /// </summary>
    public class NaturalFeatureDetailsViewModel
    {
        /// <summary>
        /// The NaturalFeature to see the details of.
        /// </summary>
        public NaturalFeature NaturalFeature { get; set; }

        /// <summary>
        /// The Locations associated with this NaturalFeature.
        /// </summary>
        public List<Location> AllLocations { get; set; }
        public IPagedList<Location> Locations { get; set; }

    }

    /// <summary>
    /// Used as a model for the FormExample view.
    /// </summary>
    public class FormExampleViewModel
    {
        public ICollection<String> Recreations { get; set; }
        public ICollection<String> NaturalFeatures { get; set; }
    }

    /// <summary>
    /// Used as a model for the SearchForm view.
    /// </summary>
    public class SearchFormViewModel
    {
        public ICollection<String> NaturalFeatureStrings { get; set; }
        public ICollection<NaturalFeature> NaturalFeatures { get; set; }
    }

    /// <summary>
    /// Used to Edit a single location's natural features.
    /// </summary>
    public class EditNaturalFeaturesViewModel
    {
        [Required]
        public int LocationID { get; set; }
        [Required]
        public string LocationLabel { get; set; }
        public Location Location { get; set; }
        public ICollection<string> AllNaturalFeatures { get; set; }
        public ICollection<string> SelectedFeatures { get; set; }
    }
}