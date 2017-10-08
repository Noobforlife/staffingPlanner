 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;
using StaffingPlanner.Models;

namespace StaffingPlanner.Controllers
{
	public class TeacherController : Controller
	{
        // GET: /Teacher/Teachers
        public ActionResult Teachers()
        {
            if (Globals.User == null) {
                return RedirectToAction("Login", "Account");
            }
	        var db = StaffingPlanContext.GetContext();

            //Get terms (HT17 and VT18)
            var fallTerm = db.TermYears.FirstOrDefault(ty => ty.Term == Term.Fall && ty.Year == 2017);
            var springTerm = db.TermYears.FirstOrDefault(ty => ty.Term == Term.Spring && ty.Year == 2018);

            //Generate the viewmodel list
            var teachers = GenerateTeacherViewModelList(db.Teachers.ToList(), fallTerm, springTerm);            

			return View(teachers);
        }

        // GET: /Teacher/TeacherDetails/{id}
        public ActionResult TeacherDetails(Guid id)
        {
            if (Globals.User == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var db = StaffingPlanContext.GetContext();
            //Get the teacher with the same Id as the parameter id
            var teacher = db.Teachers.First(t => t.Id == id);

            //Get the terms, right now we simple use HT17 and VT18
            var fallTerm = db.TermYears.Where(ty => ty.Term == Term.Fall && ty.Year == 2017).FirstOrDefault();
            var springTerm = db.TermYears.Where(ty => ty.Term == Term.Spring && ty.Year == 2018).FirstOrDefault();
            HourBudget teacherBugdet = teacher.GetHourBudget(fallTerm, springTerm);

            //Get availability for the teacher for the terms above
            int fallAvailability = GetTermAvailability(teacher, fallTerm);
            int springAvailability = GetTermAvailability(teacher, springTerm);

            //Get all offerings for this teacher and divide them into past and current
            List<CourseOffering> allOfferings = db.Workloads.Where(t => t.Teacher.Id == teacher.Id).Select(l => l.Course).ToList();
            var pastOfferings = allOfferings.Where(co => co.State == CourseState.Completed).ToList();
            var currentOfferings = allOfferings.Except(pastOfferings).ToList();

            //Generate viewmodel for both sets of offerings
            var currentCoursesViewModel = GenerateTeacherCourseList(teacher, currentOfferings);
            var pastCoursesViewModel = GenerateTeacherCourseList(teacher, pastOfferings);

            //Generate final viewmodel
            var teacherModel = GenerateTeacherViewModel(teacher, teacherBugdet, currentCoursesViewModel, pastCoursesViewModel);

            return View(teacherModel);
        }

        [ChildActionOnly]
        public PartialViewResult CourseHistory(Guid teacherid)
        {
            var db = StaffingPlanContext.GetContext();
            var courses = db.Workloads.Where(x => x.Teacher.Id == teacherid && x.Course.TermYear.Year < DateTime.Now.Year).ToList();
            
            return PartialView("~/Views/Teacher/_TeacherCourseHistory.cshtml", courses);
        }

        //Helper methods

        public static int GetTermAvailability(Teacher teacher, TermYear termYear)
        {
            var db = StaffingPlanContext.GetContext();
            var teacherAvailability = db.TeacherTermAvailability.Where(tta => tta.Teacher.Id == teacher.Id);
            int termAvailability = teacherAvailability.
                Where(tta => tta.TermYear.Year == termYear.Year && tta.TermYear.Term == termYear.Term)
                .Select(tta => tta.Availability)
                .FirstOrDefault();
            return termAvailability;
        }
       
        //Methods to generate view models

        public static DetailedTeacherViewModel GenerateTeacherViewModel(Teacher teacher, HourBudget teacherBudget,
            List<TeacherCourseViewModel> currentCourseOfferings,
            List<TeacherCourseViewModel> pastCourseOfferings)
        {
            return new DetailedTeacherViewModel
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Email = teacher.Email,
                Title = teacher.AcademicTitle,
                RemainingHours = teacherBudget.TotalHours - teacher.GetAllocatedHoursForTerm(teacherBudget.FallTerm) - teacher.GetAllocatedHoursForTerm(teacherBudget.SpringTerm),
                HourBudget = teacherBudget,
                CurrentCourseOfferings = currentCourseOfferings,
                PastCourseOfferings = pastCourseOfferings
            };
        }

        public static List<TeacherCourseViewModel> GenerateTeacherCourseList(Teacher teacher, List<CourseOffering> offerings)
        {
            var db = StaffingPlanContext.GetContext();

            return offerings.OrderBy(o => o.TermYear.Year)
                .ThenBy(o => o.TermYear.Term)
                .ThenBy(o => o.Periods)
                .Select(o => new TeacherCourseViewModel
            {
                Id = o.Id,
                Code = o.Course.Code,
                CourseName = o.Course.TruncatedName,
                TermYear = o.TermYear,
                Period = EnumToString.PeriodToString(o.Periods),
                CourseResponsible = o.CourseResponsible,
                TotalHours = o.TotalHours,
                AllocatedHours = o.AllocatedHours,
                RemainingHours = o.RemainingHours,
                TeacherAssignedHours = teacher.GetAllocatedHoursForOffering(o)
            })
            .ToList();
        }

        public static List<SimpleTeacherViewModel> GenerateTeacherViewModelList(List<Teacher> teachersList, TermYear fallTerm, TermYear springTerm)
        {
            //FIX!!! /Simon
	        var db = StaffingPlanContext.GetContext();
	        var output = new List<SimpleTeacherViewModel>();

	        foreach (var teacher in teachersList)
	        {
				var allocatedFall = db.Workloads.Where(w => w.Teacher.Id == teacher.Id && w.Course.TermYear.Term == fallTerm.Term && w.Course.TermYear.Year == fallTerm.Year).ToList().Sum(w => w.Workload);
		        var allocatedSpring = db.Workloads.Where(w => w.Teacher.Id == teacher.Id && w.Course.TermYear.Term == springTerm.Term && w.Course.TermYear.Year == springTerm.Year).ToList().Sum(w => w.Workload); 
		        var teacherHourBudget = teacher.GetHourBudget(fallTerm, springTerm);
		        var totalRemaining = teacherHourBudget.TeachingHours - allocatedFall - allocatedSpring;
		        var allocationWarnings = GenerateAllocationWarning(teacherHourBudget, allocatedFall, allocatedSpring);

				output.Add(new SimpleTeacherViewModel
				{
					Id = teacher.Id,
					Name = teacher.Name,
					Title = teacher.AcademicTitle,
					FallTermAvailability = teacherHourBudget.FallAvailability,
					SpringTermAvailability = teacherHourBudget.SpringAvailability,
					AllocatedHoursFall = allocatedFall,
					StatusFall = allocationWarnings.Item1,
					AllocatedHoursSpring = allocatedSpring,
					StatusSpring = allocationWarnings.Item2,
					TotalRemainingHours = totalRemaining
				});

	        }

	        return output;
        }

		// Feel free to replace "warning" with "status" if it makes more sense for its intended use
		private static Tuple<string, string> GenerateAllocationWarning(HourBudget budget, int allocatedFall, int allocatedSpring)
		{
			var fallWarning = "";
			var springWarning = "";

			var fallAllocationShouldBe = budget.TeachingHours / 2 * budget.FallAvailability;
			var springAllocationShouldBe = budget.TeachingHours / 2 * budget.SpringAvailability;

			// Set the fall warning attribute according to allocation status
			if (allocatedFall >= fallAllocationShouldBe * 1.5)
			{
				fallWarning = "";
			}
			else if (allocatedFall >= fallAllocationShouldBe * 1.1)
			{
				fallWarning = "";
			}

			// Set the spring warning attribute according to allocation status
			if (allocatedSpring >= springAllocationShouldBe * 1.5)
			{
				springWarning = "";
			}
			else if (allocatedSpring >= springAllocationShouldBe * 1.1)
			{
				springWarning = "";
			}

			return new Tuple<string, string>(fallWarning, springWarning);
		}
    }
}