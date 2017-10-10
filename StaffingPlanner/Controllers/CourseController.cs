using System;
using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;
using StaffingPlanner.Models;
using System.Collections.Generic;
using System.Web.Services;

namespace StaffingPlanner.Controllers
{
	public class CourseController : Controller
	{
        private static readonly Random Rnd = new Random();

        #region ViewResult and PartialView Result Methods 
        
        // GET: /Course/Courses
        public ActionResult Courses()
        {
            if (Globals.User == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //Get all course offerings
            var db = StaffingPlanContext.GetContext();
            var offerings = db.CourseOfferings.Where(c => c.Course != null).ToList();

            //Generate course
            var courses = GenerateCourseViewModelList(offerings);

            return View(courses);
        }

        // GET: /Course/CourseDetails/{id}
        public ActionResult CourseDetails(Guid id)
        {
            if (Globals.User == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //Get the matching course offering and all teacher who have assigned hours to the offering
            var db = StaffingPlanContext.GetContext();
            var offering = db.CourseOfferings.Where(c => c.Id == id).ToList().First();
            var teachers = db.Workloads.Where(w => w.Course.Course.Code == offering.Course.Code).Select(x => x.Teacher).ToList();

            //Terms for current year
            var fallTerm = db.TermYears.Where(ty => ty.Term == Term.Fall && ty.Year == 2017).FirstOrDefault();
            var springTerm = db.TermYears.Where(ty => ty.Term == Term.Spring && ty.Year == 2018).FirstOrDefault();

            //Generate viewmodel
            var vm = GenerateCourseDetailViewModel(offering, teachers, fallTerm, springTerm);

            return View(vm);
        }

        [ChildActionOnly]
        public PartialViewResult RenderTeacherList(Guid Courseid)
        {
            var db = StaffingPlanContext.GetContext();
            var teachers = db.Workloads.Where(w => w.Course.Id == Courseid).Select(x => x.Teacher).ToList();

            //Terms for current year
            var fallTerm = db.TermYears.Where(ty => ty.Term == Term.Fall && ty.Year == 2017).FirstOrDefault();
            var springTerm = db.TermYears.Where(ty => ty.Term == Term.Spring && ty.Year == 2018).FirstOrDefault();

            var teacherList = TeacherController.GenerateTeacherViewModelList(teachers, fallTerm, springTerm);

            return PartialView("~/Views/Course/_CourseTeacherList.cshtml", teacherList);
        }

        [ChildActionOnly]
        public PartialViewResult RenderCourseHistory(Guid courseid)
        {
            var db = StaffingPlanContext.GetContext();
            var courses = db.CourseOfferings.Where(x => x.Id == courseid && x.TermYear.Year < DateTime.Now.Year).ToList();

            return PartialView("~/Views/Course/_CourseHistory.cshtml", courses);
        }

        [HttpGet]
        public PartialViewResult EditCourse(Guid course)
        {
            var db = StaffingPlanContext.GetContext();
            var teachers = db.Teachers.Where(x => x.Id != null).ToList();
            var c = db.CourseOfferings.Where(x => x.Id == course).ToList().First();

            var model = new Tuple<CourseOffering, List<Teacher>>(c, teachers);

            return PartialView("~/Views/Course/_EditCourseDetails.cshtml", model);

        }

        [HttpPost]
        public ActionResult SaveChanges(string Responsible, string CourseId)
        {
            var db = StaffingPlanContext.GetContext();
            var teacher = db.Teachers.Where(x => x.Id.ToString() == Responsible).ToList().First();
            var course = db.CourseOfferings.Where(x => x.Id.ToString() == CourseId).ToList().First();
            var vm = GenerateCourseDetailViewModel(course);

            course.CourseResponsible = teacher;
            var t = course;
            db.SaveChanges();

            return RedirectToAction("CourseDetails", "Course", new { id = course.Id });
        }

        #endregion

        #region Methods for generating View Models 

        private static DetailedCourseViewModel GenerateCourseDetailViewModel(CourseOffering offering, List<Teacher> teachers, TermYear fallTerm, TermYear springTerm)
        {
            var teacherList = TeacherController.GenerateTeacherViewModelList(teachers, fallTerm, springTerm);
            var vm = new DetailedCourseViewModel
            {
                Id = offering.Id,
                Code = offering.Course.Code,
                Name = offering.Course.Name,
                TermYear = offering.TermYear,
                Period = EnumToString.PeriodToString(offering.Periods),
                Credits = offering.Credits,
                CourseResponsible = offering.CourseResponsible,
                HST = offering.HST,
                NumStudents = offering.NumStudents,
                RegisteredStudents = offering.RegisteredStudents,
                PassedStudents = offering.PassedStudents,
                TotalHours = offering.TotalHours,
                AllocatedHours = offering.AllocatedHours,
                RemainingHours = offering.RemainingHours
            };
            return vm;
        }

        private static DetailedCourseViewModel GenerateCourseDetailViewModel(CourseOffering offering)
        {
           var vm = new DetailedCourseViewModel
            {
                Id = offering.Id,
                Code = offering.Course.Code,
                Name = offering.Course.Name,
                TermYear = offering.TermYear,
                Period = EnumToString.PeriodToString(offering.Periods),
                Credits = offering.Credits,
                CourseResponsible = offering.CourseResponsible,
                HST = offering.HST,
                NumStudents = offering.NumStudents,
                RegisteredStudents = offering.RegisteredStudents,
                PassedStudents = offering.PassedStudents,
                TotalHours = offering.TotalHours,
                AllocatedHours = offering.AllocatedHours,
                RemainingHours = offering.RemainingHours
            };
            return vm;
        }

        public static List<SimpleCourseViewModel> GenerateCourseViewModelList(List<CourseOffering> offerings)
        {
            return offerings.Select(o => new SimpleCourseViewModel
            {
                Id = o.Id,
                Code = o.Course.Code,
                Name = o.Course.TruncatedName,
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

        #endregion

        //helpers
        public static string GetStatus() {
            var credits = new List<string> {"warning","success","danger"};
            return credits[Rnd.Next(credits.Count)];
        }

        public static string GetStatus(int TotalHours, int AllocatedHours)
        {
            float percentage = (AllocatedHours/(float)TotalHours)*100;
            if (percentage >= 90)
            {
                return "success";
            }
            else if (percentage >= 55 && percentage < 90)
            {
                return "warning";
            }
            return "danger";
        }               

    }
    
}