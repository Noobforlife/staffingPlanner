using System;
using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;
using StaffingPlanner.Models;

namespace StaffingPlanner.Controllers
{
	public class CourseController : Controller
	{
		public ActionResult Courses()
		{
            SchoolYear schoolYear = new SchoolYear(DateTime.Today.Year);

			var db = StaffingPlanContext.GetContext();
			var courses = db.Courses.Select(c => new SimpleCourseViewModel
			{
				Id = c.GetOffering(schoolYear).Id,
                Name = c.Name,
				Code = c.Code,
				Credits = c.GetOffering(schoolYear).Credits,
				TermYear = c.GetOffering(schoolYear).TermYear,
				Periods = c.GetOffering(schoolYear).Periods,
				AllocatedHours = c.GetOffering(schoolYear).GetAllocatedHours(),
				RemainingHours = c.GetOffering(schoolYear).GetRemainingHours()
			});

			return View(courses);
        }

        public ActionResult CourseDetails(string code, TermYear termYear)
        {
            var db = StaffingPlanContext.GetContext();

            var course = db.Courses.Where(c => c.Code == code).First();
            var offering = course.GetOffering(termYear);

            var courseDetails = new DetailedCourseViewModel
            {
                Name = course.Name,
                Code = course.Code,

                Credits = offering.Credits,
                TermYear = offering.TermYear,
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