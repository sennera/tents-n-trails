using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TentsNTrails.Models
{

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        // In DB by default:
        //  UserID
        //  UserName
        //  Email

        [Required]
        public DateTime EnrollmentDate { get; set; }

        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }
        public bool Private { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "The {0} cannot be more than {1} characters long.")]
        public string About { get; set; }
        private string profilePictureUrl;
        public string ProfilePictureUrl
        { 
            get
            {
                return profilePictureUrl == null ? Image.DEFAULT_PROFILE_PICTURE_URL : profilePictureUrl;
            }
            set
            {
                profilePictureUrl = value;
            }
        }

        public List<UserRecreation> UserActivities { get; set; }
        public virtual List<Review> UserReviews { get; set; }
        public virtual List<LocationFlag> BeenThereLocations { get; set; }
        public virtual List<LocationFlag> WantToGoLocations { get; set; }
        public virtual List<LocationFlag> GoAgainLocations { get; set; }
        public virtual List<LocationImage> UserLocationImages { get; set; }
        public virtual List<Video> UserLocationVideos { get; set; }
        public virtual List<Notification> Notifications { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        // Calculate this user's total contributions (reviews and media items)
        public virtual int TotalContributions()
        {
            int total = 0;
            if (UserReviews != null)
            {
                total += UserReviews.Count;
            }
            if (UserLocationImages != null)
            {
                total += UserLocationImages.Count;
            }
            if (UserLocationVideos != null)
            {
                total += UserLocationVideos.Count;
            }
            return total;
        }
        // Calculate this user's total reviews
        public virtual int TotalReviews()
        {
            int total = 0;
            if (UserReviews != null)
            {
                total += UserReviews.Count;
            }
            return total;
        }

        // Calculate this user's total media items
        public virtual int TotalMediaItems()
        {
            int total = 0;
            if (UserLocationImages != null)
            {
                total += UserLocationImages.Count;
            }
            if (UserLocationVideos != null)
            {
                total += UserLocationVideos.Count;
            }
            return total;
        }

        // Handles newlines in a string for html markup by replacing each with a <br> tag.
        public string GetDescriptionMarkup()
        {
            if (About != null)
            {
                return About.Replace(Environment.NewLine, "<br />");
            }
            else
            {
                return "";
            }
        }
    }
}