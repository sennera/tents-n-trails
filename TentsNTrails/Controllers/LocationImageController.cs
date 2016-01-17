using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TentsNTrails.Models;
using System.Threading.Tasks;
using TentsNTrails.Azure;

namespace TentsNTrails.Controllers
{

    /// <summary>
    /// Handles all Images for Locations.
    /// </summary>
    public class LocationImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<User> manager;
         private PhotoService photoService = new PhotoService();

        public LocationImageController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<User>(new UserStore<User>(db));
        }

        // **************************************
        // EDITED (Non-Standard Scaffolded Code)
        // **************************************
        // GET: LocationImage
        //
        // Shows a grid display of images for the given location.  
        // If locationID has a value, it is limited to that single Location.
        public ActionResult Index(int? locationID)
        {
            IEnumerable<LocationImage> locationImages;
            string cancelAction;
            // handle case for a single location selectedd
            if (locationID.HasValue)
            {
                locationImages = db.LocationImages.Where(i => i.LocationID == locationID);
                ViewBag.Location = db.Locations.Where(i => i.LocationID == locationID).SingleOrDefault();
                cancelAction = "Details/" + locationID;
            }

            // otherwise, show all images
            else
            {
                locationImages = db.LocationImages.ToList();
                cancelAction = "Index";
            }

            ViewBag.CancelAction = cancelAction;
            return View(locationImages);
        }

        // **************************************
        // EDITED (Non-Standard Scaffolded Code)
        // **************************************
        // GET: LocationImage/Details/#
        //

        // fromLocationImageIndex
        // fromLocationDetails


        // Displays an image details vage that displays the image with some metadata.
        // The back link in the view will dynamically redirect based on fromLocationDetails' value.
        public ActionResult Details(int? id, bool fromLocationImageIndex = false, bool fromLocationDetails = false)
        {
            // ensure id is passed
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // check if Image exists
            LocationImage locationImage = db.LocationImages.Find(id);
            if (locationImage == null)
            {
                return HttpNotFound();
            }
 
            // cancel action is used to redirect to the previous page.
            else
            {
                ViewBag.fromLocationImageIndex = fromLocationImageIndex;
                ViewBag.fromLocationDetails = fromLocationDetails;
                ViewBag.IsAdmin = User.IsInRole("Admin");
                return View(locationImage);
            }
        }


        // **************************************
        // EDITED (Non-Standard Scaffolded Code)
        // **************************************
        // GET: LocationImage/Create
        //
        // Shows a new Image Upload Page.  It takes an optional locationID field, which if specified limits the selectlist to a single
        // value, and the subsequent view will redirect to that Location's Details page.  Otherwise, the user can select any Location to
        // associate with the Image and they will be redirected to the Location index page.
        [Authorize]
        public ActionResult CreateByUrl(int locationID)
        {
            // updated to include notes from Monday's meeting           
            ViewBag.PlaceholderUrl = "~/Content/ImagePreview.png";
            ViewBag.LocationLabel = db.Locations.Find(locationID).Label;
            LocationImageUrlViewModel viewModel = new LocationImageUrlViewModel();
            viewModel.LocationID = locationID;

            return View(viewModel);
        }

        // **************************************
        // EDITED (Non-Standard Scaffolded Code)
        // **************************************
        // POST: Recreation/Edit/5
        //
        // Creates a new Image from the data input from the LocationImageViewModel.
        //
        // This is done in two steps: 
        //     1.) Saving the image to the website folder structure,
        //     2.) Saving the LocationImage model with a string url to that image in the database.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateByUrl(LocationImageUrlViewModel model)
        {

            // TODO finish

            // List of allowed image types (for hosting on web)
            var validImageTypes = new string[]
            {
                ".gif",
                ".jpg",
                ".jpeg",
                ".pjpeg", // needed for compatability with some older jpegs
                ".png"
            };


            // 1. check if a valid image was uploaded
            if (model.ImageUrl == null || model.ImageUrl.Length == 0)
            {
                System.Diagnostics.Debug.WriteLine("Image is null or empty");
                ModelState.AddModelError("ImageUpload", "This field is required");
            }

            // 2. check that image is of a valid filetype
            else
            {
                bool hasValidExtension = false;
                foreach (String s in validImageTypes)
                {
                    if (model.ImageUrl.EndsWith(s))
                    {
                        hasValidExtension = true;
                        break;
                    }
                }

                if (!hasValidExtension)
                {
                    System.Diagnostics.Debug.WriteLine("Invalid Image Type");
                    ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
                }
            }

            // 3. create the Image entry to save to the database
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Model state is valid");
                // initialize image model to store in database
                var locationImage = new LocationImage
                {
                    LocationID = model.LocationID,
                    Title = model.Title,
                    AltText = "Image from " + db.Locations.Find(model.LocationID).Label,
                    DateTaken = model.DateTaken,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    ImageUrl = model.ImageUrl,
                    User = manager.FindById(User.Identity.GetUserId())
                };

                // save LocationImage model in database
                db.LocationImages.Add(locationImage);
                db.SaveChanges();
                return RedirectToAction("Media", "Location", new { locationID = model.LocationID });
            }

            // otherwise, go back to create view.
            System.Diagnostics.Debug.WriteLine("Model state is invalid.");

            ViewBag.LocationID = model.LocationID;
            ViewBag.LocationLabel = db.Locations.Find(model.LocationID).Label;
            ViewBag.PlaceholderUrl = "~/Content/ImagePreview.png";
            ViewBag.LocationCount = db.Locations.Count();
            return View(model);
        }


         // GET LocationImage/Upload
        [Authorize]
        public ActionResult Upload(int locationID)
        {         
            ViewBag.PlaceholderUrl = "~/Content/ImagePreview.png";
            ViewBag.LocationLabel = db.Locations.Find(locationID).Label;
            LocationImageViewModel viewModel = new LocationImageViewModel();
            viewModel.LocationID = locationID;
            return View(viewModel);
        }

        // POST: LocationImage/Upload
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(LocationImageViewModel viewModel)
        {
            System.Diagnostics.Debug.WriteLine("----------------------------------------");
            System.Diagnostics.Debug.WriteLine("LocationImageController.Upload()");
            // List of allowed image types (for hosting on web)
            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg", // needed for compatability with some older jpegs
                "image/png"
            };

            // 1. check if file is null, or of zero length
            if (viewModel.ImageUpload == null || viewModel.ImageUpload.ContentLength == 0)
            {
                System.Diagnostics.Trace.WriteLine("Image is null");
                ModelState.AddModelError("ImageUpload", "This field is required.");
            }

            // 2. check if file is null, or of zero length
            else if (viewModel.ImageUpload == null || viewModel.ImageUpload.ContentLength == 0)
            {
                System.Diagnostics.Trace.WriteLine("Uploaded file is empty");
                ModelState.AddModelError("ImageUpload", "Image file cannot be empty.");
            }

            // 3. check that image is of a valid filetype
            else if (!validImageTypes.Contains(viewModel.ImageUpload.ContentType))
            {
                System.Diagnostics.Trace.WriteLine("Invalid Image Type");
                ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
            }

            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is valid!");

                // Upload Image to Blob Storage
                string imageUrl = await photoService.UploadPhotoAsync(viewModel.ImageUpload);
                System.Diagnostics.Debug.WriteLine("Uploaded image to URL: {1}", imageUrl);

                // create 
                var locationImage = new LocationImage
                {
                    LocationID = viewModel.LocationID,
                    Title = viewModel.Title,
                    AltText = "Image from " + db.Locations.Find(viewModel.LocationID).Label,
                    DateTaken = viewModel.DateTaken,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    ImageUrl = imageUrl,
                    User = manager.FindById(User.Identity.GetUserId())
                };

                // save LocationImage model in database
                db.LocationImages.Add(locationImage);
                db.SaveChanges();
                System.Diagnostics.Debug.WriteLine("Saved {0} '{1}' to the database.", locationImage.AltText, locationImage.Title);

                return RedirectToAction("Media", "Location", new { locationID = viewModel.LocationID });
            }

            ViewBag.PlaceholderUrl = "~/Content/ImagePreview.png";
            ViewBag.LocationLabel = db.Locations.Find(viewModel.LocationID).Label;
            return View(viewModel);
        }

        // GET: LocationImage/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationImage locationImage = db.LocationImages.Find(id);
            if (locationImage == null)
            {
                return HttpNotFound();
            }

            ViewBag.PlaceholderUrl = "~/Content/ImagePreview.png";
            return View(locationImage);
        }

        // POST: LocationImage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ImageID,Title,ImageUrl,DateTaken")] LocationImage locationImage)
        {
            if (ModelState.IsValid)
            {
                locationImage.DateModified = DateTime.UtcNow;
                db.Entry(locationImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Media", "Location", new { locationID = locationImage.LocationID });
            }
            return View(locationImage);
        }

        // GET: LocationImage/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationImage locationImage = db.LocationImages.Find(id);
            if (locationImage == null)
            {
                return HttpNotFound();
            }
            return View(locationImage);
        }

        // POST: LocationImage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            LocationImage locationImage = db.LocationImages.Find(id);
            db.Images.Remove(locationImage);
            db.SaveChanges();
            return RedirectToAction("Media", "Location", new { locationID = locationImage.LocationID });
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
