using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TentsNTrails.Models
{
    /// <summary>
    /// I am using the inheritance TPH (Table Per Heirarchy) model from the MVC Tutorial 11.
    /// So, there is only one table in the Database (Images), but multiple models to be used
    /// by the site (a controller for LocationImage, and later a controller for ProfileImage).
    /// 
    /// Note that most of the controller's create and edit views will use a LocationImageViewModel
    /// to make things simpler.
    /// </summary>
    public class LocationImage : Image
    {
        public const string UPLOAD_DIRECTORY = "~/Uploads/Images/Locations/";

        [Display(Name = "Location")]        
        public int LocationID { get; set; }
        public virtual Location Location { get; set; }

        // each image is associated with a user
        public virtual User User { get; set; }
    }

}