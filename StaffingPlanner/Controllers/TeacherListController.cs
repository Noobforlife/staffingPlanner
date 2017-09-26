using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;
using WebGrease.Css.Extensions;

namespace StaffingPlanner.Controllers
{
	public class TeacherListController : Controller
	{
        public ActionResult Teachers()
        {
	        var schoolYear = "17/18";

	        var db = StaffingPlanContext.GetContext();
			var teachers = db.Teachers.Select(t => new TeacherViewModel
	        {
				Id = t.Id,
				Name = t.Name,
				Title = t.AcademicTitle,
				TotalHours = t.GetTotalHours(),
				FallWork = t.GetContract(schoolYear).FallWork,
				SpringWork = t.GetContract(schoolYear).SpringWork,
				RemainingHours = t.GetRemainingHours()
	        });

			return View(teachers);
        }
    }
}