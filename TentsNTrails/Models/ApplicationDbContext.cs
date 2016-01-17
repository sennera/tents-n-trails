using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace TentsNTrails.Models
{
    /// <summary>
    /// <para>
    /// Used to contain a convenient reference for all connection strings in web.config.  
    /// Assign DEFAULT the current string to be used.
    /// </para>
    /// <para>
    /// I have references to these multiple values so I can conveniently run migrations on the Azure databases.
    /// The Connection Strings are set in web.config transform files (select the > button next to web.config in
    /// the Solution Explorer to see the Config Transforms).  These only make a difference when publishing.
    /// </para>
    /// <para>
    /// Note: When running Locally, ensure that it is ALWAYS running on TentsNTrailsDB, unless you have a good
    /// reason to change it (like running migrations on a remote database manually with Visual Studio).
    /// </para>
    /// </summary>
    public static class ConnectionStrings
    {
        public const string TENTS_N_TRAILS_DB = "TentsNTrailsDB";
        public const string LOCAL_DB = "LocalDB";
        public const string AZURE_RELEASE = "AzureRelease";
        public const string AZURE_NIGHTLY = "AzureNightly";
        public const string CURRENT = ConnectionStrings.TENTS_N_TRAILS_DB;
        //public const string CURRENT = ConnectionStrings.AZURE_RELEASE;
    }

    /// <summary>
    /// I separated out the ApplicationDbContext from the IdentityModels class file,
    /// so it is easier to read.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext() : base(ConnectionStrings.CURRENT, throwIfV1Schema: false) { }

        // accessible DbSets
        public DbSet<Connection> Connections { get; set; }
        public DbSet<ConnectionRequest> ConnectionRequests { get; set; }

        public DbSet<Events> Events { get; set; }
        public DbSet<EventParticipants> EventParticipants { get; set; }
        public DbSet<EventComments> EventComments { get; set; }

        public DbSet<Image> Images { get; set; }
        public DbSet<LocationImage> LocationImages { get; set; }
        
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationRecreation> LocationRecreations { get; set; }
        public DbSet<LocationFlag> LocationFlags { get; set; }

        public DbSet<LocationFeature> LocationFeatures { get; set; }
        public DbSet<NaturalFeature> NaturalFeatures { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<FriendNotification> FriendNotifications { get; set; }

        public DbSet<Message> Messages { get; set; }
        
        public DbSet<Recreation> Recreations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<State> States { get; set; }
        
        public DbSet<UserRecreation> UserRecreations { get; set; }
        
        public DbSet<Video> Videos { get; set; }
        public DbSet<LocationVideo> LocationVideos { get; set; }
        
        /// <summary>
        /// Used by Startup.Auth.cs to initialize a DbContext for Identity Authentication.
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}