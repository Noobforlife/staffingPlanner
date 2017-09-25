using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StaffingPlanner.ViewModels;
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
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
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

            var matchingNames = (db.Teachers.Where(t => t.Name.ToLower().Split(' ').Intersect(nameParts).Any()));
            if (matchingNames.Any()) //If there are any teachers who match any name we are sufficiently satisfied
            {
                authResult = true;
            }
            else
            {
                authResult = false;
            }

            switch (authResult)
            {
                case true:
                    Globals.isUserAuthorized = true;
                    
                    //If the matching teacher (in db) is the director of studies than set the user role to match
                    if (matchingNames.First().DirectorOfStudies)
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
                    Globals.isUserAuthorized = false;
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
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