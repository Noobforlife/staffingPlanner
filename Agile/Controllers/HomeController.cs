using System.Web.Mvc;
using Agile.DAL;

namespace Agile.Controllers
{
	public class HomeController : Controller
	{
		private StaffingPlanContext db = new StaffingPlanContext();

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			var students = db.Courses.ToString();
			//ViewBag.Message = "Your application description page is here.";
			ViewBag.Message = students;

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}