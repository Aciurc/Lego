using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OptionsWebSite.Models;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace OptionsWebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserLockController : Controller
    {
        ApplicationUser savedEditUser = null;
        ApplicationDbContext db = new ApplicationDbContext();
        static UserManager<ApplicationUser> um;

        // GET: UserLock
        public ActionResult Index()
        {
            um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            List<ApplicationUser> users = null;
            while (users == null)
            {
                try
                {
                    users = um.Users.ToList();
                }
                catch (Exception e) { }
            }
            return View(users);
        }

        public ActionResult Update(string UserName)
        {
            ApplicationUser user = db.Users.FirstOrDefault(u => u.UserName.Equals(UserName));
            savedEditUser = user;
            return View(user);
        }

        // POST: UserLock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser user)
        {
            savedEditUser.LockoutEnabled = user.LockoutEnabled;
            user = savedEditUser;
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }*/

        [HttpPost]
        public ActionResult Update(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userToEdit = db.Users.FirstOrDefault(u => u.UserName.Equals(user.UserName));
                userToEdit.LockoutEnabled = user.LockoutEnabled;
                db.Entry(userToEdit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}