using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TentsNTrails.Models;

namespace TentsNTrails.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Notification
        public ActionResult Index()
        {
            // get Notifications only for the current user.
            string currentUserID = User.Identity.GetUserId();
            var notifications = db.Notifications
                .Include(n => n.User)
                .Where(n => n.UserID.Equals(currentUserID))
                .OrderBy(n => n.IsRead)
                .ThenByDescending(n=> n.DateCreated);
            return View(notifications.ToList());
        }

        // GET: Notification/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }




        // renders a list display of a Notification to a user.
        public PartialViewResult Summary(int? id)
        {
            Notification n = null;
            if (id != null) n = db.Notifications.Find(id);
            if (n == null) n = new Notification()
            {
                Description = "Notification failed to load.",
                DateCreated = DateTime.UtcNow,
                User = db.Users.Find(User.Identity.GetUserId()),
            };

            NotificationViewModel viewModel = new NotificationViewModel();
            viewModel.Notification = n;

            if (n is FriendNotification)
            {
                viewModel.PotentialFriend = db.FriendNotifications
                    .Include(f => f.PotentialFriend)
                    .Where(f => f.NotificationID == n.NotificationID)
                    .Single().PotentialFriend;
                viewModel.NotificationType = NotificationType.Friend;
            }
            else
            {
                viewModel.NotificationType = NotificationType.Base;
            }
            System.Diagnostics.Debug.WriteLine("NotificationType: " + viewModel.NotificationType);

            return PartialView(viewModel);
        }

        /// <summary>
        /// Renders a PartialView showing how many unread notifications the current user has.
        /// </summary>
        /// <returns>A PartialView showing the count of unread notifications.</returns>
        public PartialViewResult UnreadCount()
        {
            string userID = User.Identity.GetUserId();
            ViewBag.UnreadCount = db.Notifications.Where(n => n.UserID.Equals(userID) && !n.IsRead).Count();
            return PartialView();
        }


        /// <summary>
        /// Read this FriendNotification, then redirect to /Profile/Index?Username=PotentialFriend.Username.
        /// </summary>
        /// <param name="id">the id of the FriendNotification to read.</param>
        /// <returns>A redirect to /Profile/Index?Username=PotentialFriend.Username.</returns>
        public ActionResult ReadThenViewUserProfile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FriendNotification notification = db.FriendNotifications.Include(n => n.PotentialFriend).Where(n => n.NotificationID == id).Single();
            if (notification == null)
            {
                return HttpNotFound();
            }

            notification.IsRead = true;
            db.Entry(notification).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Profile", new { Username = notification.PotentialFriend.UserName });
        }

        /// <summary>
        /// Read this FriendNotification (and all other ConnectionRequestNotifications), 
        /// then redirect to /Profile/RequestList.
        /// </summary>
        /// <param name="id">the id of the FriendNotification to read.</param>
        /// <returns>A redirect to /Profile/RequestList.</returns>
        public ActionResult ReadThenViewConnectionRequests(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            // read all notifications that are connection requests
            var notifications = db.FriendNotifications
                .Where(n => n.Description.Contains(FriendNotification.REQUEST));
            foreach(Notification n in notifications){
                n.IsRead = true;
                db.Entry(n).State = EntityState.Modified;
            }
            db.SaveChanges();


            return RedirectToAction("RequestList", "Profile");
        }

        /// <summary>
        /// Read this Notification, then redirect to the /Notification/Index.
        /// </summary>
        /// <param name="id">the id of the Notification to read.</param>
        /// <returns>A redirect to /Notification/Index.</returns>
        [HttpPost]
        public ActionResult Read(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }

            //read notification
            notification.IsRead = true;
            db.Entry(notification).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        // POST: Notification/Delete/id
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Notification notification = db.Notifications.Find(id);
            if (notification != null)
            {
                db.Notifications.Remove(notification);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
