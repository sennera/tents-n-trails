using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    public class EventParticipants
    {
        [Key]
        public int EventParticipationID { get; set; }

        public virtual Events Event { get; set; }

        public virtual User Participant { get; set; }
    }
}