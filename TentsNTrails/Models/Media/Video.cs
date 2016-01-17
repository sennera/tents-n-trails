using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TentsNTrails.Models
{
    /// <summary>
    /// Store a video in a database using an embed code.
    /// </summary>
    public class Video
    {
        [Key]
        public int VideoID { get; set; }

        // a multi-line, text description of the video.
        [Required]
        [StringLength(200, MinimumLength=5)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        // Handles newlines in a string for html markup by replacing each with a <br> tag.
        public string GetDescriptionMarkup()
        {
            return Description.Replace(Environment.NewLine, "<br />");
        }

        //**********************************
        // embed tag types:
        //**********************************
        //
        // HTML5:  <video width="320" height="240" controls><source src="movie.mp4" type="video/mp4">HTML 5 Video not supported</video>
        // iFrame: <iframe width="420" height="315" src="https://www.youtube.com/embed/G4Sn91t1V4g"></iframe>
        // object: <object width="420" height="315" data="https://www.youtube.com/v/G4Sn91t1V4g"></object>
        // embed:  <embed width="420" height="315" src="https://www.youtube.com/v/G4Sn91t1V4g"/>
        //
        // <iframe> is recommended overall if at all possible.
        // each of these types work in different ways.  the HTML5 <video> tag links to a literal video, and would work for
        // any videos we host directly on our site.  The other ones will lin
        //
        //20 characters is minimum code length for <embed src="1.mov"/>
        [Required]
        [AllowHtml]
        [Display(Name = "Embed Code")]
        [StringLength(2000, MinimumLength=(20))]
        public string EmbedCode { get; set; }

        public User User { get; set; }

        // if the embed code is from a known website (youtube), 
        //it retrieves an image thumbnail; else, it just returns a the video embed code.
        public string GetThumbnailUrl()
        {
            try
            {
                // if image is from youtube, return the path to youtube thumbnail.
                if (EmbedCode.Contains("youtube.com/embed/"))
                {
                    //try to find the id
                    string videoID = EmbedCode
                        .Split(new string[] { "src" }, StringSplitOptions.None)[1]
                        .Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[3]
                        .Split(new char[]{'\"'}, StringSplitOptions.None)[0];
                    string thumbnailUrl = "http://img.youtube.com/vi/" + videoID + "/0.jpg";

                    Console.WriteLine("Retrieved Youtube VideoID: " + videoID);
                    Console.WriteLine("Created Thumbnail URL: " + thumbnailUrl);

                    return thumbnailUrl;
                }

                else if (EmbedCode.Contains("www.youtube.com/watch?v="))
                {
                    string videoID = EmbedCode.Split(new string[] { "www.youtube.com/watch?v=" }, StringSplitOptions.None)[1];
                    string thumbnailUrl = "http://img.youtube.com/vi/" + videoID + "/0.jpg";
                    return thumbnailUrl;
                }

                // otherwise, just return the embed code.
                else
                {
                    return EmbedCode;
                }
            }
            // if an error happens, just return the EmbedCode.
            catch (NullReferenceException e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Null reference caught"));
                return EmbedCode;
            }
            catch (IndexOutOfRangeException e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Index out of range caught"));
                return EmbedCode;
            }

        }

    }
}