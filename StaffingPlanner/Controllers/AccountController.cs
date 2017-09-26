using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StaffingPlanner.ViewModels;
using StaffingPlanner.Models;
using StaffingPlanner.DAL;
using System.Collections.Generic;

namespace StaffingPlanner.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            //Don't know if we need this but it was in the ASP Mvc login template
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var db = StaffingPlanContext.GetContext();

            //Define variable for whether the authorization was successful or not
            bool authResult;

            //The user provided name is split since we want to match any name
            string lowerName = model.Name.Trim().ToLower();
            string[] nameParts = lowerName.Split(' ');

            //Find any teachers with matching names
            var matchingTeachers = db.Teachers.Where(t => nameParts.All(np => t.Name.ToLower().Contains(np))).Select(t => new { Name = t.Name, Director = t.DirectorOfStudies });

            if (matchingTeachers.Any()) //If there are any teachers who match any name we are sufficiently satisfied
            {
                authResult = true;
                Globals.user = matchingTeachers.First().Name;
            }
            else
            {
                authResult = false;
            }

            authResult = true;

            switch (authResult)
            {
                case true:                  
                    //If the matching teacher (in db) is the director of studies than set the user role to match
                    if (matchingTeachers.First().Director)
                    {
                        Globals.userRole = Role.DirectorOfStudies;
                    }
                    //Otherwise they are just a teacher
                    else
                    {
                        Globals.userRole = Role.Teacher;
                    }
                    return RedirectToLocal(returnUrl);
                case false:
                default:
                    Globals.userRole = Role.Unauthorized;
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        // GET: /Account/LogOff
        [AllowAnonymous]
        public ActionResult LogOff(string returnUrl)
        {
            Globals.userRole = Role.Unauthorized;
            Globals.user = null;
            return RedirectToAction("Login", "Account");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Dashboard");
        }

    }
}