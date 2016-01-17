using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    /*
    // ProfilePicture is used for the case of an image representing a User on their profile.
    public class ProfilePicture
    {
        public const string DEFAULT_URL = "~/Content/default-user-image.png";

        [Required]
        public int ProfilePictureID { get; set; }
        public string ImageUrl { get; set; }
        public Boolean IsSelected { get; set; }

        //Get the default profile picture for a given user (with the blank face);
        public static ProfilePicture GetDefault(User user)
        {
            ProfilePicture image = new ProfilePicture();
            /*
            image.User = user;
            image.Title = "Profile Picture";
            image.AltText = user.UserName;
            image.ImageUrl = DEFAULT_URL;
            image.DateCreated = image.DateModified = image.DateTaken = user.EnrollmentDate;
            image.IsSelected = true;
            * /
            return image;
        }
     

    }
    */



    // For uploading a Profile Picture.
    public class ProfilePictureViewModel
    {
        // Holds the uploaded image file
        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}