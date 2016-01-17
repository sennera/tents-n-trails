using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    public class Events
    {
        [Key]
        public int EventID { get; set; }

        [Display(Name = "Event")]
        [Required]
        public String Name { get; set; }

        [Display(Name = "Description")]
        [Required]
        public String Description { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Location")]
        public int LocationID { get; set; }

        public virtual Location Location { get; set; }

        public virtual User Organizer { get; set; }

        public virtual ICollection<EventComments> EventComments { get; set; }

        public virtual ICollection<EventParticipants> Participants { get; set; }
    }
}