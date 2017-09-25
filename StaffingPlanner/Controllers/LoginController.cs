using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;

namespace StaffingPlanner.Controllers
{
	public class LoginController : Controller
	{
        public ActionResult Login()
        {
        	return View();
        }
    }
}