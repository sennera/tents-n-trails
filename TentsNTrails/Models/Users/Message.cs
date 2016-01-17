using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TentsNTrails.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        // Message body
        [Required]
        [Display(Name = "Message")]
        [StringLength(1000, ErrorMessage = "The {0} cannot be more than {2} characters long.")]
        [DataType(DataType.MultilineText)]
        public string MessageText { get; set; }

        // Time the message was sent
        [DataType(DataType.DateTime)]
        [Required]
        [Display(Name = "Time Sent")]
        public DateTime TimeSent { get; set; }

        // Ture if has been read, false elsewise
        public bool IsRead { get; set; }

        // True if deleted by sender
        public bool DeletedBySender { get; set; }

        // True if deleted by recipient
        public bool DeletedByRecipient { get; set; }

        public virtual User ToUser { get; set; }
        public virtual User FromUser { get; set; }


        // Handles newlines in a string for html markup by replacing each with a <br> tag.
        public string GetMessageMarkup()
        {
            if (MessageText != null)
            {
                return MessageText.Trim().Replace(Environment.NewLine, "<br />");
            }
            else
            {
                return "";
            }
        }

        public string GetFormattedDate()
        {
            if (TimeSent.Date == DateTime.Now.Date)
            {
                return TimeSent.ToString("h:mm tt");
            }
            else
            {
                return TimeSent.ToString("MM/dd/yy");
            }
         }
    }
}