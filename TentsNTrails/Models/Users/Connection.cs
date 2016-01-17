using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    public class Connection
    {
        [Key]
        public int ConnectionID { get; set; }

        [ForeignKey("User1")]
        public string User1_Id { get; set; }
        [ForeignKey("User2")]
        public string User2_Id { get; set; }

        // each connection is between two users
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }

        /// <summary>
        /// Required, default empty constructor.
        /// </summary>
        public Connection() { }

        /// <summary>
        /// Create a new Connection with the given users.
        /// </summary>
        /// <param name="user1">The first User.</param>
        /// <param name="user2">The second User.</param>
        public Connection(User user1, User user2)
        {
            this.User1 = user1;
            this.User1_Id = user1.Id;
            this.User2 = user2;
            this.User2_Id = user2.Id;
        }
    }
}