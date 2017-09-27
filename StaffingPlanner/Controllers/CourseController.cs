using System;
using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;

namespace StaffingPlanner.Controllers
{
	public class CourseController : Controller
	{
		public ActionResult Courses()
		{
			var schoolYear = "17/18";

			var db = StaffingPlanContext.GetContext();
			var courses = db.Courses.Select(c => new SimpleCourseViewModel
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

        public ActionResult CourseDetails(Guid offeringId)
        {
            //var schoolYear = "17/18";

            var db = StaffingPlanContext.GetContext();

            //Temporary (and silly) solution
            var course = db.Courses.Where(c => c.Offerings.Select(o => o.Id).Contains(offeringId)).First();
            var offering = course.GetOffering(offeringId);
            //var course = db.Courses.First(cc => cc.Id == courseID);
            //var offering = course.GetOffering(schoolYear);

            var courseDetails = new DetailedCourseViewModel
            {
                Name = course.Name,
                Code = course.Code,

                Credits = offering.Credits,
                Term = offering.Term,
                Periods = offering.Periods,
                TotalHours = offering.Budget,
                AllocatedHours = offering.GetAllocatedHours(),
                RemainingHours = offering.GetRemainingHours(),
                NumStudents = offering.NumStudents,
                CourseResponsible = offering.CourseResponsible,
                HST = offering.HST,
                Teachers = offering.Teachers.ToList()
            };

            return View(courseDetails);
        }

    }
}