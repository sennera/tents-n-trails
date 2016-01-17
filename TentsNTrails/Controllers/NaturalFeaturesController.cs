using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TentsNTrails.Models;

namespace TentsNTrails.Controllers
{
    public class NaturalFeaturesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// GET: NaturalFeatures 
        /// </summary>
        /// <returns>The NaturalFeatures/Index view.</returns>
        public ActionResult Index(string query)
        {
            List<NaturalFeature> features;
            if (!String.IsNullOrEmpty(query)) features = searchFor(query);
            else features = db.NaturalFeatures.ToList();
            return View(features);
        }

        /// <summary>
        /// Get a List of NaturalFeatures based on the searchQuery.
        /// </summary>
        /// <param name="query">The query to search for.  Defaults to "" if not set or null.</param>
        /// <returns>A list of all NaturalFeatures with a Name that starts with the search query.</returns>
        public List<NaturalFeature> searchFor(string query = "")
        {
            return db.NaturalFeatures
                .Where(f =>
                    f.Name.ToLower()
                    .StartsWith(query.ToLower())
                )
                .OrderBy(f => f.Name)
                .ToList();
        }

        /// <summary>
        /// <para>GET: NaturalFeatures/Details/5</para>
        /// <para>Shows the Details of a NaturalFeature by listing all associated Locations, with paging.</para>
        /// </summary>
        /// <param name="id">The ID of the NaturalFeature to view.</param>
        /// <param name="pageNumber">The page to view (defaults to 1)</param>
        /// <param name="pageSize">The page size (defaults to 10</param>
        /// <returns>The NaturalFeatures/Details view.</returns>
 
        public ActionResult Details(int? id, int pageNumber = 1, int pageSize = 10 )
        {
            // check parameters
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // check if valid Natural Feature.
            NaturalFeature naturalFeature = db.NaturalFeatures.Find(id);
            if (naturalFeature == null)
            {
                return HttpNotFound();
            }

            // create ViewModel, and populate with values.
            NaturalFeatureDetailsViewModel viewModel = new NaturalFeatureDetailsViewModel();
            viewModel.NaturalFeature = naturalFeature;
            viewModel.AllLocations = naturalFeature.LocationFeatures
                .Select(f => f.Location).ToList();
            viewModel.Locations = viewModel.AllLocations
                .ToPagedList(pageNumber, pageSize);


            // *************************************************
            // calculate center of map display
            // *************************************************            
            Location center = Location.GetLatLongCenter(viewModel.Locations.ToList());
            System.Diagnostics.Debug.WriteLine("centerLatitude:  " + center.Latitude);
            System.Diagnostics.Debug.WriteLine("centerLongitude: " + center.Longitude);
            ViewBag.centerLatitude = center.Latitude;
            ViewBag.centerLongitude = center.Longitude;

            // done!
            return View(viewModel);
        }

        // GET: NaturalFeatures/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: NaturalFeatures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,Name")] NaturalFeature naturalFeature)
        {
            if (ModelState.IsValid)
            {
                db.NaturalFeatures.Add(naturalFeature);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(naturalFeature);
        }

        // GET: NaturalFeatures/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NaturalFeature naturalFeature = db.NaturalFeatures.Find(id);
            if (naturalFeature == null)
            {
                return HttpNotFound();
            }
            return View(naturalFeature);
        }

        // POST: NaturalFeatures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,Name")] NaturalFeature naturalFeature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(naturalFeature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(naturalFeature);
        }

        // GET: NaturalFeatures/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NaturalFeature naturalFeature = db.NaturalFeatures.Find(id);
            if (naturalFeature == null)
            {
                return HttpNotFound();
            }
            return View(naturalFeature);
        }

        /// <summary>
        /// My Test form for testing my multi-select list.
        /// </summary>
        /// <param name="tags">The tag string list.</param>
        /// <returns>The view.</returns>
        //[ValidateAntiForgeryToken]
        public ActionResult TestForm(ICollection<string> Recreations, ICollection<string> NaturalFeatures)
        {
            System.Diagnostics.Debug.WriteLine("TestForm()");
            System.Diagnostics.Debug.WriteLine("Recreations     = " + GetArrayFormattedString(Recreations));
            System.Diagnostics.Debug.WriteLine("NaturalFeatures = " + GetArrayFormattedString(NaturalFeatures));
            FormExampleViewModel viewModel = new FormExampleViewModel();
            viewModel.Recreations = Recreations  ?? new List<string>();
            viewModel.NaturalFeatures = NaturalFeatures ?? new List<string>();
            return View(viewModel);
        }

        /// <summary>
        /// An example of how to search one list with an input field and do cool stuff with it.
        /// </summary>
        /// <param name="NaturalFeatureStrings">Any strings the user has searched for.</param>
        /// <returns>A view.</returns>
        public ActionResult SearchForm(ICollection<string> NaturalFeatureStrings)
        {
            System.Diagnostics.Debug.WriteLine("SearchForm()");
            System.Diagnostics.Debug.WriteLine("NaturalFeatures = " + GetArrayFormattedString(NaturalFeatureStrings));
            SearchFormViewModel viewModel = new SearchFormViewModel();
            viewModel.NaturalFeatureStrings = NaturalFeatureStrings ?? new List<string>();
            viewModel.NaturalFeatures = db.NaturalFeatures.ToList();
            return View(viewModel);
        }

        /// <summary>
        /// An example of how to search one list with an input field and do cool stuff with it.
        /// </summary>
        /// <param name="NaturalFeatureStrings">Any strings the user has searched for.</param>
        /// <returns>A view.</returns>
        public ActionResult CombinedExample(ICollection<string> tags)
        {
            System.Diagnostics.Debug.WriteLine("CombinedExample()");
            System.Diagnostics.Debug.WriteLine("tags = " + GetArrayFormattedString(tags));
            //SearchFormViewModel viewModel = new SearchFormViewModel();
            //viewModel.NaturalFeatureStrings = NaturalFeatureStrings ?? new List<string>();
            //viewModel.NaturalFeatures = db.NaturalFeatures.ToList();
            return View();
        }

        /// <summary>
        /// Edit the NaturalFeatures for a single Location.
        /// </summary>
        /// <param name="locationID">The index of the Location to edit.</param>
        /// <returns>the Location/EditFor view.</returns>
        public ActionResult EditFor(int? id)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            System.Diagnostics.Debug.WriteLine("get EditFor(" + id + ")");

            Location location = db.Locations.Find(id);

            /*
            Location location = db.Locations
                .Include(l => l.LocationFeatures)
                .SingleOrDefault(l => l.LocationID == id);
            */

            if (location == null)
            {
                return HttpNotFound();
            }
            EditNaturalFeaturesViewModel viewModel = new EditNaturalFeaturesViewModel();
            viewModel.LocationID = location.LocationID;
            viewModel.LocationLabel = location.Label;
            viewModel.SelectedFeatures = location.LocationFeatures
                .Select(lf => lf.NaturalFeature)
                .Select(nf => nf.Name)
                .ToList();
            viewModel.AllNaturalFeatures = db.NaturalFeatures
                .Select(nf => nf.Name)
                .ToList();
            return View(viewModel);
        }

        /// <summary>
        /// HttpPost NaturalFeatures/EditFor/5
        /// </summary>
        /// <param name="viewModel">The model to validate.</param>
        /// <returns>A redirect to location details if successful, else back to EditFor.</returns>
        [HttpPost]
        public ActionResult EditFor([Bind(Include = "LocationID, LocationLabel, AllNaturalFeatures, SelectedFeatures")] EditNaturalFeaturesViewModel viewModel)
        {
            System.Diagnostics.Debug.WriteLine("post EditFor(viewModel)");
            System.Diagnostics.Debug.WriteLine("SelectedFeatures = " + GetArrayFormattedString(viewModel.SelectedFeatures));

            // check for no SelectedFeatures.
            if (viewModel.SelectedFeatures == null) viewModel.SelectedFeatures = new List<string>();

            int locationID = viewModel.LocationID;

            // save all changes to the db.
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is valid.");
                var location = db.Locations.Find(locationID);

                // 1) - Set aside all previous location features for comparison.
                var associated = db.LocationFeatures
                    .Include(lf => lf.NaturalFeature)
                    .Where(lf => lf.LocationID == locationID)
                    .ToList();

                // ****************************************************
                // 2) - add any new NaturalFeatures.
                // ****************************************************
                int newNfCount = 0;
                foreach (string name in viewModel.SelectedFeatures)
                {
                    var match = db.NaturalFeatures.SingleOrDefault(nf => nf.Name.Equals(name));
                    if (match == null)
                    { 
                        db.NaturalFeatures.Add(new NaturalFeature(name));
                        newNfCount++;
                    }
                }
                db.SaveChanges();
                System.Diagnostics.Debug.WriteLine(String.Format("successfully added {0} new features.", newNfCount));


                // get all features to iterate over.
                var selected = db.NaturalFeatures
                    .Where(nf => viewModel.SelectedFeatures.Contains(nf.Name))
                    .ToList();

                
                // ****************************************************
                // 3) - remove any LocationFeatures that are currently 
                //      associated, but aren't selected.
                // ****************************************************
                int removedLfcount = 0;
                foreach (LocationFeature item in associated){
                    var result = selected.SingleOrDefault(nf => nf.ID == item.NaturalFeatureID);
                    if (result == null)
                    {
                        System.Diagnostics.Debug.WriteLine(String.Format("'{0}' is associated with '{1}', but needs to be removed.", item.NaturalFeature.Name, viewModel.LocationLabel));
                        db.LocationFeatures.Remove(item);
                        removedLfcount++;
                    }
                }
                db.SaveChanges();
                System.Diagnostics.Debug.WriteLine(String.Format("successfully removed {0} new features from {1}.", removedLfcount, viewModel.LocationLabel));

                /*
                 * this should be obsolete code
                // re-query the db to fetch the 
                associated = db.LocationFeatures
                    .Include(lf => lf.NaturalFeature)
                    .Where(lf => lf.LocationID == locationID)
                    .ToList();
                 */

                // ****************************************************
                // 4) - add any LocationFeatures that are currently 
                //      selected, but aren't associated.
                // ****************************************************
                int addedLfCount = 0;
                foreach (NaturalFeature item in selected)
                {
                    var result = associated.SingleOrDefault(lf => lf.NaturalFeatureID == item.ID);
                    if (result == null)
                    {
                        System.Diagnostics.Debug.WriteLine(String.Format("'{0}' is not associated with '{1}', but needs to be added.", item.Name, viewModel.LocationLabel));
                        db.LocationFeatures.Add(new LocationFeature(locationID, item.ID));
                        addedLfCount++;
                    }
                }
                db.SaveChanges();
                System.Diagnostics.Debug.WriteLine(String.Format("successfully added {0} new features.", addedLfCount, viewModel.LocationLabel));

                return RedirectToAction("Details", "Location", new { id = locationID });
            }

            System.Diagnostics.Debug.WriteLine("ModelState is INVALID.");
            // reload AllNaturalFeatures from the db.
            viewModel.AllNaturalFeatures = db.NaturalFeatures
                .Select(nf => nf.Name)
                .ToList();
            return View(viewModel);
        }



        /// <summary>
        /// Helper method to format a collection of strings into a human-readable string,
        /// in an array-literal format (i.e. '{"hello", "world"}'.
        /// </summary>
        /// <param name="strings">The collection of strings to format.</param>
        /// <returns>A formatted string.</returns>
        public string GetArrayFormattedString(ICollection<string> strings)
        {
            // case 1 - null
            if (strings == null) return "null";            

            int length = strings.Count();

            // case 2 - empty
            if (length == 0) return "{ }";

            // case 3 - general
            string[] array = strings.ToArray();
            StringBuilder sb = new StringBuilder();
            sb.Append('{');
            for (int i = 0; i < length; i++)
            {
                sb.Append(String.Format("\"{0}\"{1}", array[i], i + 1 < length ? ", " : ""));
            }
            sb.Append('}');
            return sb.ToString();
        }

        // POST: NaturalFeatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            NaturalFeature naturalFeature = db.NaturalFeatures.Find(id);
            db.NaturalFeatures.Remove(naturalFeature);
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
