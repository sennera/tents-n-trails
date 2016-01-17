using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    public class ConnectionRequest
    {
        [Key]
        public int ConnectionRequestID { get; set; }

        [ForeignKey("RequestedUser")]
        public string RequestedUser_Id { get; set; }
        [ForeignKey("Sender")]
        public string Sender_Id { get; set; }

        // each connection is between two users and is directed
        public virtual User RequestedUser { get; set; }
        public virtual User Sender { get; set; }

        /// <summary>
        /// Required, default empty constructor.
        /// </summary>
        public ConnectionRequest() { }

        /// <summary>
        /// Create a new ConnectionRequest with the given users.
        /// </summary>
        /// <param name="sender">The User who requesting a connection.</param>
        /// <param name="requestedUser">The user receiving the request.</param>
        public ConnectionRequest(User sender, User requestedUser)
        {
            this.Sender = sender;
            this.Sender_Id = sender.Id;
            this.RequestedUser = requestedUser;
            this.RequestedUser_Id = requestedUser.Id;
        }
    }
}