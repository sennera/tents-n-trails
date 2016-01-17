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
    public class EventParticipantsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<User> manager;

        public EventParticipantsController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<User>(new UserStore<User>(db));
        }


        // GET: EventParticipants
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.EventParticipants.ToList());
        }

        // GET: EventParticipants/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventParticipants eventParticipants = db.EventParticipants.Find(id);
            if (eventParticipants == null)
            {
                return HttpNotFound();
            }
            return View(eventParticipants);
        }

        // GET: EventParticipants/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return RedirectToAction("Index", "Events", new { area = "" });
        }

        // POST: EventParticipants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(int? id, [Bind(Include = "EventParticipationID")] EventParticipants eventParticipants)
        {
            if (ModelState.IsValid)
            {
                // Check to make sure that the user hasn't tried to join yet
                string userID = manager.FindById(User.Identity.GetUserId()).Id;
                int duplicates = db.EventParticipants.Where(e => e.Participant.Id.Equals(userID)).Where(l => l.Event.LocationID == id).Count();
                if (duplicates == 0)
                {
                    eventParticipants.Participant = manager.FindById(User.Identity.GetUserId());
                    eventParticipants.Event = db.Events.Find(id);
                    db.EventParticipants.Add(eventParticipants);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Details", "Events", new { area = "", id = id});
        }

        // GET: EventParticipants/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventParticipants eventParticipants = db.EventParticipants.Find(id);
            if (eventParticipants == null)
            {
                return HttpNotFound();
            }
            return View(eventParticipants);
        }

        // POST: EventParticipants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "EventParticipationID")] EventParticipants eventParticipants)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventParticipants).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventParticipants);
        }

        // GET: EventParticipants/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventParticipants eventParticipants = db.EventParticipants.Find(id);
            if (eventParticipants == null)
            {
                return HttpNotFound();
            }
            return View(eventParticipants);
        }

        // POST: EventParticipants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var userID = User.Identity.GetUserId();
            EventParticipants eventParticipants = db.EventParticipants.Where(e => e.Event.EventID == id).Where(f => f.Participant.Id == userID).Single();
            db.EventParticipants.Remove(eventParticipants);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("/Details/" + id, "Events");
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
