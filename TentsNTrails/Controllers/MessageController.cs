using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using TentsNTrails.Models;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;


namespace TentsNTrails.Controllers
{
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<User> manager;

        public MessageController()
        {
            manager = new UserManager<User>(new UserStore<User>(db));
        }

        // GET: Message
        [Authorize]
        public ActionResult Index(ProfileMessageId? message)
        {
            User currentUser = manager.FindById(User.Identity.GetUserId());
            var messages = db.Messages.Where(m => m.ToUser.UserName == currentUser.UserName && !m.DeletedByRecipient)
                .OrderBy(m => m.IsRead)
                .ThenByDescending(m => m.TimeSent);

            ViewBag.TotalCount = messages.Count();
            ViewBag.NewCount = messages.Where(m => !m.IsRead).Count();

            ViewBag.SuccessMessage =
                message == ProfileMessageId.SentMessage ? "Your message has been sent."
                : "";

            return View(messages.ToList());
        }

        // GET: Sent Messages
        [Authorize]
        public ActionResult SentMessages(ProfileMessageId? message)
        {
            User currentUser = manager.FindById(User.Identity.GetUserId());
            var messages = db.Messages.Where(m => m.FromUser.UserName == currentUser.UserName && !m.DeletedBySender)
                .OrderBy(m => m.IsRead)
                .ThenByDescending(m => m.TimeSent);

            ViewBag.TotalCount = messages.Count();

            return View(messages.ToList());
        }

        // GET: Message/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Message/Create
        [Authorize]
        public ActionResult Create(string username, bool profile)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.User = manager.FindByName(username);
            if (ViewBag.User == null)
            {
                return HttpNotFound();
            }

            ViewBag.CameFromProfile = profile;

            return View();
        }

        // POST: Message/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageText")] Message message, string username, bool profile)
        {
            if (ModelState.IsValid)
            {
                if (username == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                message.ToUser = manager.FindByName(username);
                if (message.ToUser == null)
                {
                    return HttpNotFound();
                }

                message.TimeSent = DateTime.Now;
                message.IsRead = false;
                message.FromUser = manager.FindById(User.Identity.GetUserId());

                db.Messages.Add(message);
                db.SaveChanges();
                if (profile)
                {
                    // The user came from the profile page
                    return RedirectToAction("Index", "Profile", new { username = username, Message = ProfileMessageId.SentMessage });
                }
                else
                {
                    // The user came from somewhere else and will be sent back to the Messages page by default
                    return RedirectToAction("Index", "Message", new { message = ProfileMessageId.SentMessage });
                }
            }

            return View(message);
        }

        // GET: Message/MarkAsRead/5
        // This GET method should never be used but it's here just in case it's called
        [Authorize]
        public ActionResult MarkAsRead()
        {
            return RedirectToAction("Index");
        }

        // POST: Message/MarkAsRead/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult MarkAsRead([Bind(Include = "MessageID,MessageText,TimeSent,IsRead")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.IsRead = true;
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Message/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Message/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "MessageID,MessageText,TimeSent,IsRead")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Message/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Message/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = manager.FindById(User.Identity.GetUserId());
            Message message = db.Messages.Find(id);

            if (user.Id == message.FromUser.Id)
            {
                message.DeletedBySender = true;
            }
            else if (user.Id == message.ToUser.Id)
            {
                message.DeletedByRecipient = true;
            }

            if (message.DeletedByRecipient && message.DeletedBySender)
            {
                db.Messages.Remove(message);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// Renders a PartialView showing how many unread messages the current user has.
        /// </summary>
        /// <returns>A PartialView showing the count of unread messages.</returns>
        public PartialViewResult UnreadCount()
        {
            string userID = User.Identity.GetUserId();
            ViewBag.UnreadCount = db.Messages.Where(m => m.ToUser.Id.Equals(userID) && !m.IsRead).Count();
            return PartialView();
        }
    }
}
