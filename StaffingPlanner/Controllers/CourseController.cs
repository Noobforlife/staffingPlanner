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
            var teachers = db.Workloads.Where(w => w.Course.Course.Code == offering.Course.Code).Select(x => x.Teacher).Distinct().ToList();

            //Terms for current year
            var fallTerm = db.TermYears.Where(ty => ty.Term == Term.Fall && ty.Year == 2017).FirstOrDefault();
            var springTerm = db.TermYears.Where(ty => ty.Term == Term.Spring && ty.Year == 2018).FirstOrDefault();

            //Generate viewmodel
            var vm = GenerateCourseDetailViewModel(offering, teachers, fallTerm, springTerm);

            return View(vm);
        }

        [ChildActionOnly]
        public PartialViewResult CourseHistory(Guid courseid)
        {
            var db = StaffingPlanContext.GetContext();
            var courses = db.CourseOfferings.Where(x => x.Id == courseid && x.TermYear.Year < DateTime.Now.Year).ToList();

            return PartialView("~/Views/Course/_CourseHistory.cshtml", courses);
        }

        //Methods to generate view models
                
        private static DetailedCourseViewModel GenerateCourseDetailViewModel(CourseOffering offering, List<Teacher> teachers, TermYear fallTerm, TermYear springTerm)
        {
            var teacherList = GenerateCourseDetailsTeacherList(offering, teachers, fallTerm, springTerm);
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
                RegisteredStudents = offering.RegisteredStudents,
                PassedStudents = offering.PassedStudents,
                TotalHours = offering.TotalHours,
                AllocatedHours = offering.AllocatedHours,
                RemainingHours = offering.RemainingHours,
                Teachers = teacherList                
            };
            return vm;
        }

        public static List<CourseTeacherViewModel> GenerateCourseDetailsTeacherList(CourseOffering offering, List<Teacher> teachers, TermYear fallTerm, TermYear springTerm)
        {
            var output = new List<CourseTeacherViewModel>();
            var db = StaffingPlanContext.GetContext();

            foreach (var teacher in teachers)
            {
                var fallBudget = teacher.GetHourBudget(fallTerm);
                var springBudget = teacher.GetHourBudget(springTerm);

                var allocatedFall = teacher.GetAllocatedHoursForTerm(fallTerm);
                var allocatedSpring = teacher.GetAllocatedHoursForTerm(springTerm);

                var totalRemaining = fallBudget.TeachingHours + springBudget.TeachingHours - allocatedFall - allocatedSpring;
                //var allocationWarnings = TeacherController.GenerateAllocationWarning(fallBudget, springBudget, allocatedFall, allocatedSpring);

                output.Add(new CourseTeacherViewModel
                {
                    Id = teacher.Id,
                    Name = teacher.Name,
                    Title = teacher.AcademicTitle,
                    WorkloadFall = allocatedFall,
                    WorkloadSpring = allocatedSpring,
                    RemainingTeachingHours = totalRemaining,
                    AllocatedCourse = teacher.GetAllocatedHoursForOffering(offering)
                });

            }

            return output;
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