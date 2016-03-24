using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OptionsWebSite.Models;
namespace OptionsWebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        public ActionResult Index()
        {
            var roles = context.Roles.ToList();
            return View(roles);
        }
        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }
        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                context.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully !";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(string RoleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ManageUserRoles()
        {
            // prepopulate roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            var userlist = context.Users.Select(c => new SelectListItem { Value = c.UserName.ToString(), Text = c.UserName.ToString() });
            ViewBag.Roles = list;
            ViewBag.UserList = userlist;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var idResult = _userManager.AddToRole(user.Id, RoleName);
            ViewBag.ResultMessage = "Role created successfully !";
            // prepopulate roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            var userlist = context.Users.Select(c => new SelectListItem { Value = c.UserName.ToString(), Text = c.UserName.ToString() });
            ViewBag.Roles = list;
            ViewBag.UserList = userlist;
            return View("ManageUserRoles");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                ViewBag.RolesForThisUser = this._userManager.GetRoles(user.Id);
                // prepopulate roles for the view dropdown
                var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                var userlist = context.Users.Select(c => new SelectListItem { Value = c.UserName.ToString(), Text = c.UserName.ToString() });
                ViewBag.Roles = list;
                ViewBag.UserList = userlist;
            }
            return View("ManageUserRoles");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            ViewBag.ResultMessage = "Invalid Value!";
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                if (_userManager.IsInRole(user.Id, RoleName))
                {
                    if (user.UserName == "A00111111")
                    {
                        ViewBag.ResultMessage = "Not allowed";
                    }
                    else {
                        _userManager.RemoveFromRole(user.Id, RoleName);
                        ViewBag.ResultMessage = "Role removed from this user successfully !";
                    }
                }
                else
                {
                    ViewBag.ResultMessage = "This user doesn't belong to selected role.";
                }
            }
            // prepopulate roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            var userlist = context.Users.Select(c => new SelectListItem { Value = c.UserName.ToString(), Text = c.UserName.ToString() });
            ViewBag.Roles = list;
            ViewBag.UserList = userlist;
            return View("ManageUserRoles");
        }
    }
}