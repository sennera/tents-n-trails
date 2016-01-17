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
using TentsNTrails.Controllers;

namespace TentsNTrails.Controllers
{
    
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<User> manager;

        public EventsController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<User>(new UserStore<User>(db));
        }

        // GET: Events
        public ActionResult Index()
        {
            var events = db.Events.Include(e => e.Location).OrderBy(e => e.Date);
            ViewBag.centerLatitude = 39.8282;
            ViewBag.centerLongitude = -98.5795;
            return View(events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            
            if (User.Identity.GetUserId() != null)
                if (events.Participants.Where(e => e.Participant.Id.Equals(manager.FindById(User.Identity.GetUserId()).Id)).Count() == 0)
                {
                    ViewBag.Join = true;
                }
                else
                {
                    ViewBag.Join = false;
                }
            else
            {
                ViewBag.Join = true;
            }

            var commentList = db.EventComments.Where(e => e.EventID == id).ToList();
            if (commentList.Count == 0)
            {
                ViewBag.HasComments = false;
            }
            else
            {
                ViewBag.HasComments = true;
            }

            return View(events);
        }

        [Authorize]
        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Label");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,Name,Description,Date,LocationID")] Events events)
        {
            if (events.Date < DateTime.Now)
            {
                ModelState.AddModelError("Date", "The date must be in the future.");
            }

            if (ModelState.IsValid)
            {
                events.Organizer = manager.FindById(User.Identity.GetUserId());

                db.Events.Add(events);
                db.SaveChanges();

                EventParticipants ep = new EventParticipants();
                ep.EventParticipationID = events.EventID;
                ep.Event = db.Events.Where(e => e.EventID == events.EventID).ToList().First();
                ep.Participant = events.Organizer;
                db.EventParticipants.Add(ep);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Label", events.LocationID);
            return View(events);
        }

        [Authorize]
        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Label", events.LocationID);
            return View(events);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,Name,Description,Date,LocationID")] Events events)
        {
            if (ModelState.IsValid)
            {
                db.Entry(events).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Label", events.LocationID);
            return View(events);
        }

        // GET: Events/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // POST: Events/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Events events = db.Events.Find(id);
            db.Events.Remove(events);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult Summary(int? id, int? imageSize, string redirectAction, string redirectController)
        {
            // find location
            Events events = db.Events
                .Include(l => l.Location)
                .Where(l => l.EventID == id)
                .SingleOrDefault();

            // error checking for null case
            if (events == null) return PartialView(events);

            ViewBag.Size = imageSize ?? 100;
            ViewBag.redirectAction = redirectAction ?? "Index";
            ViewBag.redirectController = redirectController ?? "Location";
            //System.Diagnostics.Debug.WriteLine(String.Format("LocationController.Summary(LocationID: {0}) ViewBag.redirectAction:     {1}", id ?? -1, redirectAction ?? "Index"));
            //System.Diagnostics.Debug.WriteLine(String.Format("LocationController.Summary(LocationID: {0}) ViewBag.redirectController: {1}", id ?? -1, redirectController ?? "Location"));
            // done
            return PartialView(events);
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
