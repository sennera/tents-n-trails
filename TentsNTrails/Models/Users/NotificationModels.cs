using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    /// <summary>
    /// Notification is the base class for all Notifications.  This will be designed so that we can subclass this notification type and
    /// add new fields/data as needed, so that there can be a Friend Request Notification, a Message Notification, etc.
    /// </summary>
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateRead { get; set; }

        // Convenience Property for checking and assigning a  if the notification is read by assigning values to DateRead.
        public bool IsRead
        {
            // IsRead returns false if DateRead is null.  Otherwise, it is true.
            get
            {
                return DateRead != null;
            }

            // If value is "true" and DateRead is null, then DateRead is assigned to UtcNow.
            // otherwise, if value is "false", then DateRead is assigned back to null. 
            set
            {
                if (value && DateRead == null) DateRead = DateTime.UtcNow;
                else if (!value) DateRead = null;
            }
        }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual User User { get; set; }
    }

    // ****************************************************************************************************************
    // FRIEND_NOTIFICATION
    // ****************************************************************************************************************
    /// <summary>
    /// FriendNotification has an additional field, a potential friend.  There are also static builder methods for 
    /// creating the three FriendNotificationTypes: a connection request, confirm, and deny.
    /// </summary>
    public class FriendNotification : Notification
    {
        // the three message types.
        public static String REQUEST = "You have a new connection request from ";
        public static String CONFIRM = " has confirmed your connection request.";
        public static String DENY    = " has denied your connection request.";

        // the friend who the notification is about.
        [ForeignKey("PotentialFriend")]
        public string PotentialFriend_Id { get; set; }
        public User PotentialFriend { get; set; }
        
        /// <summary>
        /// Create a new FriendNotification for the targetUser about a potentialFriend.
        /// </summary>
        /// <param name="targetUser">The user who will see the notification.</param>
        /// <param name="potentialFriend">The user who the notification is about.</param>
        /// <param name="description">The string description of this notification.</param>
        public static FriendNotification MakeFriendNotification(User targetUser, User potentialFriend, string description)
        {
            return new FriendNotification()
            {
                Description = description,
                DateCreated = DateTime.UtcNow,
                DateRead = null,
                User = targetUser,
                UserID = targetUser.Id,
                PotentialFriend = potentialFriend,
                PotentialFriend_Id = potentialFriend.Id,
            };
        }

        /// <summary>
        /// Convenience method for creating a Notification for a connection request.
        /// </summary>
        /// <param name="target">The user who will see the notification.</param>
        /// <param name="potential">The user who the notification is about.</param>
        /// <returns>A new FriendNotification for the target user which says "You have a new connection request from 'potential.UserName'</returns>
        public static FriendNotification CreateRequestNotification(User target, User potential)
        {
            return MakeFriendNotification(target, potential, REQUEST);
        }

        /// <summary>
        /// Convenience method for creating a Notification about a confirmed connection request.
        /// </summary>
        /// <param name="target">The user who will see the notification.</param>
        /// <param name="potential">The user who the notification is about.</param>
        /// <returns>A new FriendNotification for the target user which says "'potential.UserName' has confirmed your connection request."</returns>
        public static FriendNotification CreateConfirmNotification(User target, User potential)
        {
            return MakeFriendNotification(target, potential, CONFIRM);
        }

        /// <summary>
        /// Convenience method for creating a Notification about a denied connection request.
        /// </summary>
        /// <param name="target">The user who will see the notification.</param>
        /// <param name="potential">The user who the notification is about.</param>
        /// <returns>A new FriendNotification for the target user which says "'potential.UserName' has denied your connection request."</returns>
        public static FriendNotification CreateDenyNotification(User target, User potential)
        {
            return MakeFriendNotification(target, potential, DENY);
        }

    }

    public enum NotificationType
    {
        Base, Friend
    }

    public class NotificationViewModel
    {
        public Notification Notification { get; set; }
        public NotificationType NotificationType { get; set; }
        public User PotentialFriend { get; set; }
    }

}