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
            SchoolYear schoolYear = new SchoolYear(DateTime.Today.Year);

			var db = StaffingPlanContext.GetContext();
            var offerings = db.CourseOfferings.Where(c => c.Course != null).ToList();

            List<SimpleCourseViewModel> courses = new List<SimpleCourseViewModel>();
            foreach (var o in offerings) {
                var vm = new SimpleCourseViewModel
                {
                    Code = o.Course.Code,
                    Name = o.Course.Name,
                    Term = o.Term,
                    Credits = o.Credits,
                    AllocatedHours = GetAllocatedHours(o),
                    RemainingHours = GetRemainingHours(o),
                };
                courses.Add(vm);
            }            

            ViewBag.courses = courses;

			return View();
        }

        //public ActionResult CourseDetails(SimpleCourseViewModel course)
        //{
        //    //var schoolYear = "17/18";

        //    var db = StaffingPlanContext.GetContext();

        //    //Temporary (and silly) solution
        //    var course = db.Courses.Where(c => c.Offerings.Select(o => o.Id).Contains(offeringId)).First();
        //    var offering = course.GetOffering(offeringId);

        //    var courseDetails = new DetailedCourseViewModel
        //    {
        //        Name = course.Name,
        //        Code = course.Code,

        //        Credits = offering.Credits,
        //        Term = offering.Term,
        //        Periods = offering.Periods,
        //        TotalHours = offering.Budget,
        //        AllocatedHours = offering.GetAllocatedHours(),
        //        RemainingHours = offering.GetRemainingHours(),
        //        NumStudents = offering.NumStudents,
        //        CourseResponsible = offering.CourseResponsible,
        //        HST = offering.HST,
        //        Teachers = offering.Teachers.ToList()
        //    };

        //    return View(courseDetails);
        //}

        
        public ActionResult CourseDetails()
        {
            return View("CourseModal");
        }

        public static int GetAllocatedHours(CourseOffering offering)
        {
            var db = StaffingPlanContext.GetContext();
            return db.Workloads.Where(w => w.Course.Id.Equals(offering.Id)).Select(w => w.Workload).Sum();
        }

        public static int GetRemainingHours(CourseOffering offering)
        {
            return (offering.Budget - GetAllocatedHours(offering));
        }
    }
}