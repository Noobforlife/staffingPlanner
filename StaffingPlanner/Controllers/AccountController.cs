using System;
using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.ViewModels;
using StaffingPlanner.Models;
using StaffingPlanner.DAL;

namespace StaffingPlanner.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Session["UserID"] != null &&
                Globals.SessionUser.ContainsKey(Session["UserID"] as string))
            {
                return RedirectToAction("Index", "Dashboard");
            }

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

            //Define variable for whether the authorization was successful or not
            string newSessionUserId = Guid.NewGuid().ToString();
	        var authResult = LoginUser(model.Name, newSessionUserId);      

            switch (authResult)
            {
                case true:
                    Session["UserID"] = newSessionUserId;
                    return RedirectToLocal(returnUrl);
                default:
                    
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        // GET: /Account/LogOff
        [AllowAnonymous]
        public ActionResult LogOff(string returnUrl)
        {
            Globals.SessionUser.Remove(Session["UserID"] as string);
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

        private static TermYear getCurrentTerm(AcademicYear year) {
            if (DateTime.Now.Month >= 1 && DateTime.Now.Month <= 6) {
                return year.EndTerm;
            }
            return year.StartTerm;
        }

	    protected static bool LoginUser(string input, string sessionUserId)
	    {
		    var db = StaffingPlanContext.GetContext();

		    //The user provided name is split since we want to match any name
		    var lowerName = input.Trim().ToLower();
		    var nameParts = lowerName.Split(' ');

		    //Find any teachers with matching names
		    var matchingTeacher = db.Teachers.Where(t => nameParts.All(np => t.Name.ToLower().Contains(np))).Select(t => new { t.Name, t.Id, Director = t.DirectorOfStudies }).FirstOrDefault();
		    var academicYear = db.AcademicYears.AsNoTracking().Where(y => y.StartTerm.Year == 2017).ToList().FirstOrDefault();
		    if (matchingTeacher != null) //If there are any teachers who match any name we are sufficiently satisfied
		    {
                
                Role userRole = matchingTeacher.Director ? Role.DirectorOfStudies : Role.Teacher;
                User newUser = new User(matchingTeacher.Name, matchingTeacher.Id, userRole);

                Globals.SessionUser.Add(sessionUserId, newUser);
			    Globals.CurrentAcademicYear = academicYear;
			    Globals.CurrentTerm = getCurrentTerm(academicYear);

			    //If the matching teacher (in db) is the director of studies than set the user role to match
                
				return true;
		    }
			return false;
	    }
    }
}