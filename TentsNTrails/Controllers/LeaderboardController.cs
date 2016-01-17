using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TentsNTrails.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using System.Web.Routing;
using TentsNTrails.Controllers;
using System.Xml.Linq;

namespace TentsNTrails.Controllers
{
    public class LeaderboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Leaderboard
        public ActionResult Index()
        {
            
            var allUsers = db.Users.ToList();
            // sort the users by their contributions, but only count it if they have at least 1 contribution
            var topUsers = allUsers
                .Where(u => u.TotalContributions() > 0)
                .OrderByDescending(e => e.TotalContributions())
                .Take(10);

            var topReviewers = allUsers
                .Where(u => u.TotalReviews() > 0)
                .OrderByDescending(e => e.TotalReviews())
                .Take(10);

            var topMediaUploaders = allUsers
                .Where(u => u.TotalMediaItems() > 0)
                .OrderByDescending(e => e.TotalMediaItems())
                .Take(10);

            LeaderboardViewModel viewModel = new LeaderboardViewModel();
            viewModel.TopRanked = topUsers.ToList();
            viewModel.TopReviewers = topReviewers.ToList();
            viewModel.TopMediaUploaders = topMediaUploaders.ToList();

            return View(viewModel);
        }

        // GET: Leaderboard/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
