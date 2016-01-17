using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TentsNTrails.Models;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TentsNTrails.Controllers
{
    public class UsersController : Controller
    {

        private UserManager<User> manager;

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var users = from s in db.Users select s;
            

            int pageSize = 8;
            int pageNumber = (page ?? 1);

            if (searchString ==  "" || searchString == null)
            {
                // Basically return 0 results
                users = users.Where(q => q.PhoneNumber == "9999999999");

                ViewBag.rowCount = users.Count().ToString();

                return View(users.ToPagedList(pageNumber, pageSize));
            }
            else if (!String.IsNullOrEmpty(searchString))
            {
                // Find the users that contain the search string 
                users = users.Where(u => u.UserName.ToLower().Contains(searchString)
                                        || (u.LastName.ToLower().Contains(searchString) && u.Private == false)
                                        || u.FirstName.ToLower().Contains(searchString) && u.Private == false);
            }


            // For each dictionary, the username is the key and the boolean is the value
            ViewBag.IsConnectedDictionary = new Dictionary<string, bool>();
            ViewBag.HasConnectionRequestDictionary = new Dictionary<string, bool>();
            ViewBag.HasSentRequestDictionary = new Dictionary<string, bool>();

            if (User.Identity.IsAuthenticated)
            {
                User currentUser = UserManager.FindById(User.Identity.GetUserId());
                ViewBag.currentUser = currentUser.UserName;

                // Remove current user from the list if they're there.
                users = users.Where(u => u.UserName != currentUser.UserName);
                List<User> userList = users.ToList();

                // Check if the two users are already connected some way
                foreach (var user in userList)
                {
                    // Check for connection
                    var connection = db.Connections.Where(c => (c.User1.UserName == currentUser.UserName && c.User2.UserName == user.UserName) ||
                        (c.User2.UserName == currentUser.UserName && c.User1.UserName == user.UserName));
                    if (connection.Count() > 0)
                    {
                        ViewBag.IsConnectedDictionary.Add(user.UserName, true);
                    }
                    else
                    {
                        ViewBag.IsConnectedDictionary.Add(user.UserName, false);
                    }

                    // Check if the person whose profile we're looking at has sent a ConnectionRequest
                    var requestTo = db.ConnectionRequests.Where(c => (c.RequestedUser.UserName == currentUser.UserName && c.Sender.UserName == user.UserName));
                    if (requestTo.Count() > 0)
                    {
                        ViewBag.HasConnectionRequestDictionary.Add(user.UserName, true);
                        ViewBag.HasSentRequestDictionary.Add(user.UserName, false);
                    }
                    else
                    {
                        var requestFrom = db.ConnectionRequests.Where(c => (c.Sender.UserName == currentUser.UserName && c.RequestedUser.UserName == user.UserName));
                        if (requestFrom.Count() > 0)
                        {
                            ViewBag.HasConnectionRequestDictionary.Add(user.UserName, false);
                            ViewBag.HasSentRequestDictionary.Add(user.UserName, true);
                        }
                        else
                        {
                            ViewBag.HasConnectionRequestDictionary.Add(user.UserName, false);
                            ViewBag.HasSentRequestDictionary.Add(user.UserName, false);
                        }
                    }
                }
            }

            users = users.OrderByDescending( u => u.EnrollmentDate);
            ViewBag.rowCount = users.Count().ToString();


            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: Users
        // Admin Control
        [Authorize(Roles = "Admin")]
        public ActionResult AdminControl(string currentFilter, string searchString, int? page)
        {

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var users = from s in db.Users select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.ToLower().Contains(searchString)
                                       || u.LastName.ToLower().Contains(searchString)
                                       || u.FirstName.ToLower().Contains(searchString));
            }

            users = users.OrderByDescending(u => u.EnrollmentDate);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,EnrollmentDate,FirstName,LastName,Private,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,EnrollmentDate,FirstName,LastName,Private,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Ban/5
        [Authorize(Roles = "Admin")]
        public ActionResult Ban(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Ban/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Ban([Bind(Include = "Id,EnrollmentDate,FirstName,LastName,Private,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                if (UserManager.HasPassword(user.Id))
                {
                    if (user.UserName != "Admin")
                    {
                        UserManager.RemovePasswordAsync(user.Id);
                    }
                }
                else
                {
                    UserManager.AddPassword(user.Id, "Password1!");
                }
                //db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminControl");
            }
            return View(user);
        }

        // GET: Users/MakeAdmin/5
        [Authorize(Roles = "Admin")]
        public ActionResult MakeAdmin(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/MakeAdmin/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult MakeAdmin([Bind(Include = "Id,EnrollmentDate,FirstName,LastName,Private,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                if (!UserManager.IsInRole(user.Id, "Admin"))
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }
                else
                {
                    ViewBag.AdminError = "User is already an Admin";
                }
                //db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminControl");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
