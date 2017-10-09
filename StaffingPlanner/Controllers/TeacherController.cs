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

		public ActionResult AlterTeacherAllocation(Guid teacherId, Guid offeringId, int hours)
		{
			var db = StaffingPlanContext.GetContext();
			var teacher = db.Teachers.FirstOrDefault(t => t.Id == teacherId);
			var offering = db.CourseOfferings.FirstOrDefault(co => co.Id == offeringId);
			var existingWorkload = db.Workloads
				.FirstOrDefault(w => w.Teacher.Id == teacher.Id && w.Course.Id == offering.Id);
			if (existingWorkload == null)
			{
				var newWorkload = new TeacherCourseWorkload
				{
					Id = Guid.NewGuid(),
					Course = offering,
					Teacher = teacher,
					Workload = hours
				};
				db.Workloads.Add(newWorkload);
			}
			else
			{
				existingWorkload.Workload = hours;
			}

			return TeacherDetails(teacherId);
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
            var fallTerm = db.TermYears.FirstOrDefault(ty => ty.Term == Term.Fall && ty.Year == 2017);
            var springTerm = db.TermYears.FirstOrDefault(ty => ty.Term == Term.Spring && ty.Year == 2018);
            var teacherBugdet = teacher.GetHourBudget(fallTerm, springTerm);

            //Get all offerings for this teacher and divide them into past and current
            var allOfferings = db.Workloads.Where(t => t.Teacher.Id == teacher.Id).Select(l => l.Course).ToList();
            var pastOfferings = allOfferings.Where(co => co.State == CourseState.Completed).ToList();
            var currentOfferings = allOfferings.Except(pastOfferings).ToList();

			// Get all current offerings that this teacher is not part of
	        var otherOfferings = allOfferings
		        .Where(co => (co.TermYear.Year == fallTerm.Year && co.TermYear.Term == fallTerm.Term) || 
					(co.TermYear.Year == springTerm.Year && co.TermYear.Term == springTerm.Term))
		        .Except(currentOfferings)
				.ToList();

            //Generate viewmodel for both sets of offerings
            var currentCoursesViewModel = GenerateTeacherCourseList(teacher, currentOfferings);
            var pastCoursesViewModel = GenerateTeacherCourseList(teacher, pastOfferings);
	        var otherCoursesViewModel = GenerateOtherOfferingsViewModel(otherOfferings);

            //Generate final viewmodel
            var teacherModel = GenerateTeacherViewModel(teacher, teacherBugdet, currentCoursesViewModel, pastCoursesViewModel, otherCoursesViewModel);

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
            var termAvailability = teacherAvailability.
                Where(tta => tta.TermYear.Year == termYear.Year && tta.TermYear.Term == termYear.Term)
                .Select(tta => tta.Availability)
                .FirstOrDefault();
            return termAvailability;
        }
       
        //Methods to generate view models

        public static DetailedTeacherViewModel GenerateTeacherViewModel(Teacher teacher, HourBudget teacherBudget,
            List<TeacherCourseViewModel> currentCourseOfferings,
            List<TeacherCourseViewModel> pastCourseOfferings,
			List<TeacherCourseViewModel> otherOfferings)
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
                PastCourseOfferings = pastCourseOfferings,
				OtherCourseOfferings = otherOfferings,
                FallPeriodWorkload = new TeacherPeriodWorkload(teacher, teacherBudget.FallTerm),
                SpringPeriodWorkload = new TeacherPeriodWorkload(teacher, teacherBudget.SpringTerm)
            };
        }

		public static List<TeacherCourseViewModel> GenerateOtherOfferingsViewModel(List<CourseOffering> offerings)
		{
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
					TeacherAssignedHours = 0
				})
				.ToList();
		}

        public static List<TeacherCourseViewModel> GenerateTeacherCourseList(Teacher teacher, List<CourseOffering> offerings)
        {
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
	        var output = new List<SimpleTeacherViewModel>();

	        foreach (var teacher in teachersList)
	        {
                var allocatedFall = teacher.GetAllocatedHoursForTerm(fallTerm);
                var allocatedSpring = teacher.GetAllocatedHoursForTerm(springTerm);
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