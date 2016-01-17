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
    [Authorize(Roles = "Admin")]
    public class RecreationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Recreation
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Recreations.ToList());
        }

        // GET: Recreation/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recreation recreation = db.Recreations.Find(id);
            if (recreation == null)
            {
                return HttpNotFound();
            }
            return View(recreation);
        }

        // GET: Recreation/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // **************************************
        // EDITED (Non-Standard Scaffolded Code)
        // **************************************
        // POST: Recreation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "RecreationID,Label")] Recreation recreation)
        {
            if (ModelState.IsValid)
            {
                // initialize DateTime Stamps
                recreation.DateCreated = DateTime.UtcNow;
                recreation.DateModified = recreation.DateCreated;

                // save changes
                db.Recreations.Add(recreation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(recreation);
        }

        // GET: Recreation/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recreation recreation = db.Recreations.Find(id);
            if (recreation == null)
            {
                return HttpNotFound();
            }
            return View(recreation);
        }

        // **************************************
        // EDITED (Non-Standard Scaffolded Code)
        // **************************************
        // POST: Recreation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "RecreationID,Label,DateCreated")] Recreation recreation)
        {
            if (ModelState.IsValid)
            {
                // update DateModified
                recreation.DateModified = DateTime.UtcNow;

                // save changes
                db.Entry(recreation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recreation);
        }

        // GET: Recreation/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recreation recreation = db.Recreations.Find(id);
            if (recreation == null)
            {
                return HttpNotFound();
            }
            return View(recreation);
        }

        // POST: Recreation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Recreation recreation = db.Recreations.Find(id);
            db.Recreations.Remove(recreation);
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
