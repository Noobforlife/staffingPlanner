using System;
using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;
using StaffingPlanner.Models;
using System.Collections.Generic;

namespace StaffingPlanner.Controllers
{
	public class CourseController : Controller
	{
        private static readonly Random Rnd = new Random();
        //Methods handling returning of View
        #region View methods

        // GET: /Course/Courses
        public ActionResult Courses()
        {
            var db = StaffingPlanContext.GetContext();
			// Are there any course offerings where the Course is null?
            var offerings = db.CourseOfferings.Where(c => c.Course != null).ToList();

            var courses = GenerateCourseViewModelList(offerings);

            return View(courses);
        }

        // GET: /Course/CourseDetails/{id}
        public ActionResult CourseDetails(Guid id)
        {
            var db = StaffingPlanContext.GetContext();
            var offering = db.CourseOfferings.Where(c => c.Id == id).ToList().First();
            var teachers = db.Workloads.Where(w => w.Course.Course.Code == offering.Course.Code).Select(x => x.Teacher).ToList();

            var vm = GenerateCourseDetailViewModel(offering, teachers);

            return View(vm);
        }

        #endregion

        #region Static methods


        //Methods to generate view models
        private static DetailedCourseViewModel GenerateCourseDetailViewModel(CourseOffering offering, List<Teacher> teachers)
        {
            var teacherList = TeacherController.GenerateTeacherViewModelList(teachers);
            var vm = new DetailedCourseViewModel
            {
                Code = offering.Course.Code,
                Name = offering.Course.Name,
                TermYear = offering.TermYear,
                Period = EnumToString.PeriodToString(offering.Periods),
                Credits = offering.Credits,
                CourseResponsible = offering.CourseResponsible,
                HST = offering.HST,
                NumStudents = offering.NumStudents,
                TotalHours = offering.TotalHours,
                AllocatedHours = offering.AllocatedHours,
                RemainingHours = offering.RemainingHours,
                Teachers = teacherList                
            };
            return vm;
        }

        public static List<SimpleCourseViewModel> GenerateCourseViewModelList(List<CourseOffering> offerings)
        {
            return offerings.Select(o => new SimpleCourseViewModel
            {
                Id = o.Id,
                Code = o.Course.Code,
                Name = o.Course.Name,
                TermYear = o.TermYear,
                Period = EnumToString.PeriodToString(o.Periods),
                CourseResponsible = o.CourseResponsible,
                Credits = o.Credits,
                AllocatedHours = o.AllocatedHours,
                RemainingHours = o.RemainingHours,
                State = o.State,
			    Status = o.Status
		    })
		    .ToList();
        }

        public static string GetStatus() {
            var credits = new List<string> {"warning","success","danger"};
            return credits[Rnd.Next(credits.Count)];
        }

        #endregion

    }
}