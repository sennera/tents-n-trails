using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TentsNTrails.Models;

namespace TentsNTrails.DAL
{
    public class ApplicationDBInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        /*protected override void Seed(ApplicationDbContext context)
        {
            var UserManager = new UserManager<User>(new UserStore<User>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string name = "Admin";
            string password = "Password1!";
            string email = "superuser@gmail.com";
            string firstname = "Super";
            string lastname = "User";


            // Create Admin Role if it does not already exist
            if (!RoleManager.RoleExists(name))
            {
                var roleresult = RoleManager.Create(new IdentityRole(name));
            }

            // Create User=Admin with password=Password1!
            var user = new User();
            user.UserName = name;
            user.FirstName = firstname;
            user.Email = email;
            user.LastName = lastname;
            user.EnrollmentDate = System.DateTime.Now;
            var adminresult = UserManager.Create(user, password);

            // Add User Admin to Role Admin
            if (adminresult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, name);
            }

            base.Seed(context);
        }*/
    }
}