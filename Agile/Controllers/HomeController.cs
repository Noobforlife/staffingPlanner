using System.Linq;
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
			var students = db.Courses.ToList();

			return View(students);
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}