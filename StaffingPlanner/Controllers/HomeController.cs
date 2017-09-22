using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;

namespace StaffingPlanner.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			var students = "";
			ViewBag.Message = "Your application description page.";
			ViewBag.Students = students;

			return View(students);
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}