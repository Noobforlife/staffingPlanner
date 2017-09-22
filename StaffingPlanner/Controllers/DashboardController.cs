using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;

namespace StaffingPlanner.Controllers
{
	public class DashboardController : Controller
	{
        //public ActionResult About()
        //{
        //    var students = "";
        //    ViewBag.Message = "Your application description page.";
        //    ViewBag.Students = students;

        //    return View(students);
        //}

        public ActionResult Index()
        {
            return View();
        }

    }
}