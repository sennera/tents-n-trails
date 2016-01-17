using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TentsNTrails.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }
        [Required]
        public int LocationID { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime ReviewDate { get; set; }
        // The rating: true = good/liked/thumbs up, false = bad/disliked/thumbs down
        public bool Rating { get; set; }
        public string Comment { get; set; }

        private int CommentPreviewLength = 75;
        // used for review index page, to give only a preview of the comment
        public string GetCommentPreview()
        {
            if (Comment == null) return "";
            if (Comment.Length < CommentPreviewLength) return Comment;
            else return Comment.Substring(0, CommentPreviewLength - 3) + "...";            
        }

        [ForeignKey("User")]
        public string User_Id { get; set; }

        public virtual User User { get; set; }
        public virtual Location Location { get; set; }
        
    }
}