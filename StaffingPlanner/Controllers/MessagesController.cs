using System.Web.Mvc;
using StaffingPlanner.DAL;

namespace StaffingPlanner.Controllers
{
    public class MessagesController : Controller
    {
        // GET: Message
        public ActionResult Index()
        {
	        if (Globals.UserRole != Role.DirectorOfStudies)
	        {
		        try
		        {
			        Response.Redirect(Request.UrlReferrer.ToString());
		        }
		        catch
		        {
			        return RedirectToAction("Index", "Dashboard");
		        }
	        }
            return View();
        }
    }
}