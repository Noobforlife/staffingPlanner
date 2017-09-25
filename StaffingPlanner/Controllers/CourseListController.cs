using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;

namespace StaffingPlanner.Controllers
{
	public class CourseListController : Controller
	{
        public ActionResult Courses()
        {
            return View();
        }

    }
}