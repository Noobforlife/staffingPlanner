using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;

namespace StaffingPlanner.Controllers
{
	public class TeacherListController : Controller
	{
        public ActionResult Teachers()
        {
            return View();
        }

    }
}