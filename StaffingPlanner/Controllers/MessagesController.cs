using System.Collections.Generic;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;

namespace StaffingPlanner.Controllers
{
    public class MessagesController : Controller
    {
        // GET: Message
        public ActionResult Messages()
        {
			if (Globals.UserRole != Role.DirectorOfStudies)
	        {
		        return RedirectToAction("Index", "Dashboard");
			}

			var model = new List<MessageViewModel>();

			// TODO: Generate view model

            return View(model);
        }
    }
}