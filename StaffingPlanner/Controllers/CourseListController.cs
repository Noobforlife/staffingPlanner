using System;
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
			var courses = db.Courses.Select(c => new CourseViewModels
				{
					Id = c.Id,
					Name = c.Name,
					Code = c.Code,
					Credits = c.GetEdition(schoolYear).Credits,
					Term = c.GetEdition(schoolYear).Term,
					Period = c.GetEdition(schoolYear).Period,
					AllocatedHours = c.GetEdition(schoolYear).GetAllocatedHours(),
					RemainingHours = c.GetEdition(schoolYear).GetRemainingHours()
			});

			return View(courses);
        }

    }
}