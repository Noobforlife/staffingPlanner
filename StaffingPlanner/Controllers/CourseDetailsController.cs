using System;
using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;

namespace StaffingPlanner.Controllers
{
	public class CourseDetailsController : Controller
	{
		public ActionResult Course()
		{
			var schoolYear = "17/18";
			var courseID = Guid.NewGuid();

			var db = StaffingPlanContext.GetContext();
			var c = db.Courses.First(course => course.Id == courseID);
			var ce = c.GetEdition(schoolYear);

			var courseDetails = new DetailedCourseViewModel
			{
				Id = c.Id,
				Name = c.Name,
				Code = c.Code,
				Credits = ce.Credits,
				Term = ce.Term,
				Periods = ce.Periods,
				TotalHours = ce.Budget,
				AllocatedHours = ce.GetAllocatedHours(),
				RemainingHours = ce.GetRemainingHours(),
				NumStudents = ce.NumStudents,
				CourseResponsible = ce.CourseResponsible,
				HST = ce.HST,
				Teachers = ce.Teachers.ToList()
			};

			return View(courseDetails);
		}
	}
}