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
    public class LocationVideoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<User> manager;

        public LocationVideoController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<User>(new UserStore<User>(db));
        }

        // GET: LocationVideo
        public ActionResult Index()
        {
            var videos = db.LocationVideos.Include(l => l.Location);

            return View(videos.ToList());
        }

        // GET: LocationVideo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationVideo locationVideo = db.LocationVideos.Include(v => v.User).Where(v => v.VideoID == id).Single();
            if (locationVideo == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsAdmin = User.IsInRole("Admin");
            return View(locationVideo);
        }

        // GET: LocationVideo/Create
        [Authorize]
        public ActionResult Create(int locationID)
        {
            //ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Label");

            LocationVideo video = new LocationVideo();
            video.LocationID = locationID;
            video.Location = db.Locations.Find(locationID);
            return View(video);
        }

        // POST: LocationVideo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VideoID,Description,EmbedCode,LocationID")] LocationVideo locationVideo)
        {
            if (ModelState.IsValid)
            {
                User profileUser = manager.FindById(User.Identity.GetUserId());

                locationVideo.User = profileUser;

                db.Videos.Add(locationVideo);
                db.SaveChanges();
                return RedirectToAction("Media", "Location", new { locationID = locationVideo.LocationID});
            }
            locationVideo.Location = db.Locations.Find(locationVideo.LocationID);
            return View(locationVideo);
        }

        // GET: LocationVideo/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationVideo locationVideo = db.LocationVideos.Find(id);
            if (locationVideo == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Label", locationVideo.LocationID);
            return View(locationVideo);
        }

        // POST: LocationVideo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "VideoID,Description,EmbedCode,LocationID")] LocationVideo locationVideo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(locationVideo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Media", "Location", new { locationID = locationVideo.LocationID });
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Label", locationVideo.LocationID);
            return View(locationVideo);
        }

        // GET: LocationVideo/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationVideo locationVideo = db.LocationVideos.Find(id);
            if (locationVideo == null)
            {
                return HttpNotFound();
            }
            return View(locationVideo);
        }

        // POST: LocationVideo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            LocationVideo locationVideo = db.LocationVideos.Find(id);
            db.Videos.Remove(locationVideo);
            db.SaveChanges();
            return RedirectToAction("Media", "Location", new { locationID = locationVideo.LocationID });
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
