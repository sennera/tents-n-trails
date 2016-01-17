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
    public class LocationFlagController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<User> manager;
            
        public LocationFlagController () 
        {
            manager = new UserManager<User>(new UserStore<User>(db));
        }

        // GET: LocationFlag
        [Authorize]
        public ActionResult Index()
        {
            User currentUser = manager.FindById(User.Identity.GetUserId());
            
            var locationFlags = db.LocationFlags.Include(l => l.Location).
                Where(l => l.User.Id == currentUser.Id).ToList();

            LocationFlagViewModel viewModel = new LocationFlagViewModel();

            if (locationFlags.Count() == 0)
            {
                // they haven't saved any locations
                viewModel.HasSavedLocations = false;
                viewModel.BeenThereLocations = new List<LocationFlag>();
                viewModel.WantToGoLocations = new List<LocationFlag>();
                viewModel.GoAgainLocations = new List<LocationFlag>();
            }
            else
            {
                // Fill the three lists with their respective LocationFlags
                viewModel.BeenThereLocations = locationFlags.Where(l => l.Flag == Flag.HaveBeen).ToList();
                viewModel.WantToGoLocations = locationFlags.Where(l => l.Flag == Flag.WantToGo).ToList();
                viewModel.GoAgainLocations = locationFlags.Where(l => l.Flag == Flag.GoAgain).ToList();
                viewModel.HasSavedLocations = true;
                ViewBag.centerLatitude = 39.8282;
                ViewBag.centerLongitude = -98.5795;
            }

            return View(viewModel);
        }

        // GET: LocationFlag/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Label");
            return View();
        }

        // POST: LocationFlag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "FlagID,LocationID,Flag")] LocationFlag locationFlag)
        {
            if (ModelState.IsValid)
            {
                db.LocationFlags.Add(locationFlag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Label", locationFlag.LocationID);
            return View(locationFlag);
        }

        // GET: LocationFlag/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationFlag locationFlag = db.LocationFlags.Find(id);
            if (locationFlag == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Label", locationFlag.LocationID);
            return View(locationFlag);
        }

        // POST: LocationFlag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "FlagID,LocationID,Flag")] LocationFlag locationFlag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(locationFlag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Label", locationFlag.LocationID);
            return View(locationFlag);
        }

        // GET: LocationFlag/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationFlag locationFlag = db.LocationFlags.Find(id);
            if (locationFlag == null)
            {
                return HttpNotFound();
            }
            return View(locationFlag);
        }

        // POST: LocationFlag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            LocationFlag locationFlag = db.LocationFlags.Find(id);
            db.LocationFlags.Remove(locationFlag);
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
    }
}
