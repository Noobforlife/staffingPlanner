using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;

namespace StaffingPlanner.Controllers
{
	public class CourseListController : Controller
	{
		public ActionResult Courses()
		{
			var schoolYear = "17/18";

			var db = StaffingPlanContext.GetContext();
			var courses = db.Courses.Select(c => new CourseViewModel
			{
				Id = c.GetOffering(schoolYear).Id,
                Name = c.Name,
				Code = c.Code,
				Credits = c.GetOffering(schoolYear).Credits,
				Term = c.GetOffering(schoolYear).Term,
				Periods = c.GetOffering(schoolYear).Periods,
				AllocatedHours = c.GetOffering(schoolYear).GetAllocatedHours(),
				RemainingHours = c.GetOffering(schoolYear).GetRemainingHours()
			});

			return View(courses);
        }

    }
}