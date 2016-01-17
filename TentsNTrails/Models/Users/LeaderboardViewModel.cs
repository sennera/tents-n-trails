using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    public class LeaderboardViewModel
    {
        public virtual ICollection<User> TopRanked { get; set; }
        public virtual ICollection<User> TopReviewers { get; set; }
        public virtual ICollection<User> TopMediaUploaders { get; set; }
    }
}