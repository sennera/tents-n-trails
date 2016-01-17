using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TentsNTrails.Models;

namespace TentsNTrails.Controllers
{
    [Authorize]
    public class EventCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<User> manager;
        
        public EventCommentsController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<User>(new UserStore<User>(db));
        }

        // GET: EventComments
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var eventComments = db.EventComments.Include(e => e.Event);
            return View(eventComments.ToList());
        }

        // GET: EventComments/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventComments eventComments = db.EventComments.Find(id);
            if (eventComments == null)
            {
                return HttpNotFound();
            }
            return View(eventComments);
        }

        // GET: EventComments/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int eventID)
        {
            EventComments comment = new EventComments();
            comment.EventID = eventID;
            comment.Event = db.Events.Find(eventID);
            return View(comment);
        }

        // POST: EventComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "EventCommentID,EventID,Comment")] EventComments eventComments)
        {
            eventComments.Author = manager.FindById(User.Identity.GetUserId());
            eventComments.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.EventComments.Add(eventComments);
                db.SaveChanges();
            }

            //ViewBag.EventID = new SelectList(db.Events, "EventID", "Name", eventComments.EventID);
            return RedirectToAction("Details", "Events", new { id = eventComments.EventID });
        }

        // GET: EventComments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventComments eventComments = db.EventComments.Find(id);
            if (eventComments == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "Name", eventComments.EventID);
            return View(eventComments);
        }

        // POST: EventComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "EventCommentID,Comment,Date,EventID")] EventComments eventComments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventComments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "Name", eventComments.EventID);
            return RedirectToAction("/Details/" + eventComments.EventID, "Events");
            //return View(eventComments);
        }

        // GET: EventComments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventComments eventComments = db.EventComments.Find(id);
            if (eventComments == null)
            {
                return HttpNotFound();
            }
            return View(eventComments);
        }

        // POST: EventComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            EventComments eventComments = db.EventComments.Find(id);
            db.EventComments.Remove(eventComments);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("/Details/" + eventComments.EventID, "Events");
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
