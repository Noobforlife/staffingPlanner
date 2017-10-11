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

		public ActionResult AlterTeacherAllocation(Guid teacherId, List<Guid> offeringId, List<int> hours)
		{
			for (var i = 0; i < offeringId.Count; i++)
			{  
				var db = StaffingPlanContext.GetContext();
				var teacher = db.Teachers.FirstOrDefault(t => t.Id == teacherId);
				var offering = db.CourseOfferings.FirstOrDefault(co => co.Id == offeringId[i]);
				var existingWorkload = db.Workloads
					.FirstOrDefault(w => w.Teacher.Id == teacher.Id && w.Course.Id == offering.Id);
				if (existingWorkload == null)
				{
					if (hours[i] == 0) continue;
					var newWorkload = new TeacherCourseWorkload
					{
						Id = Guid.NewGuid(),
						Course = offering,
						Teacher = teacher,
						Workload = hours[i]
					};
					db.Workloads.Add(newWorkload);
				}
				else
				{
					if (hours[i] == 0)
					{
						db.Workloads.Remove(existingWorkload);
					}
					else
					{
						existingWorkload.Workload = hours[i];
					}
				}
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
            var teacher = db.Teachers.FirstOrDefault(t => t.Id == id);

            //Get the terms, right now we simple use HT17 and VT18
            var fallTerm = db.TermYears.FirstOrDefault(ty => ty.Term == Term.Fall && ty.Year == 2017);
            var springTerm = db.TermYears.FirstOrDefault(ty => ty.Term == Term.Spring && ty.Year == 2018);
            var fallBudget = teacher.GetHourBudget(fallTerm);
            var springBudget = teacher.GetHourBudget(springTerm);

            //Generate final viewmodel
            var teacherModel = GenerateTeacherViewModel(teacher, fallBudget, springBudget);

            return View(teacherModel);
        }

        [ChildActionOnly]
        public PartialViewResult CourseHistory(Guid teacherid)
        {
            var db = StaffingPlanContext.GetContext();
            var courses = db.Workloads.Where(x => x.Teacher.Id == teacherid && x.Course.TermYear.Year < DateTime.Now.Year).ToList();
            
            return PartialView("~/Views/Teacher/_TeacherCourseHistory.cshtml", courses);
        }

		[HttpGet]
		public PartialViewResult CourseList(Guid teacherId)
		{
			var db = StaffingPlanContext.GetContext();

			var teacher = db.Teachers.FirstOrDefault(t => t.Id == teacherId);

			var allOfferings = db.Workloads.Where(t => t.Teacher.Id == teacher.Id).Select(l => l.Course).ToList();
			var pastOfferings = allOfferings.Where(co => co.State == CourseState.Completed).ToList();
			var currentOfferings = allOfferings.Except(pastOfferings).ToList();

			var courses = teacher != null
				? GenerateTeacherCourseList(teacher, currentOfferings)
				: new List<TeacherCourseViewModel>();

			ViewBag.Name = teacher != null 
				? teacher.Name
				: "";

			return PartialView("~/Views/Teacher/_TeacherCourseList.cshtml", courses);
		}

		[HttpGet]
		public PartialViewResult EditableCourseList(Guid teacherId)
		{
			var db = StaffingPlanContext.GetContext();

			var teacher = db.Teachers.FirstOrDefault(t => t.Id == teacherId);

			var allOfferings = db.CourseOfferings;
			var allOfferingsForTeacher = db.Workloads.Where(t => t.Teacher.Id == teacher.Id).Select(l => l.Course).ToList();
			var pastOfferings = allOfferingsForTeacher.Where(co => co.State == CourseState.Completed).ToList();
			var currentOfferings = allOfferingsForTeacher.Except(pastOfferings).ToList();
			var currentOfferingIds = currentOfferings.Select(o => o.Id);

			var otherOfferings = allOfferings
				.Where(co => ((co.TermYear.Year == 2017 && co.TermYear.Term == Term.Fall) ||
				             (co.TermYear.Year == 2018 && co.TermYear.Term == Term.Spring)) &&
							 !currentOfferingIds.Contains(co.Id))
				.ToList();

			var currentCourses = teacher != null
				? GenerateTeacherCourseList(teacher, currentOfferings)
				: new List<TeacherCourseViewModel>();
			var otherCourses = GenerateOtherOfferingsViewModel(otherOfferings);
			var model = new Tuple<List<TeacherCourseViewModel>, List<TeacherCourseViewModel>>(currentCourses, otherCourses);

			ViewBag.Name = teacher != null
				? teacher.Name
				: "";

			return PartialView("~/Views/Teacher/_EditableTeacherCourseList.cshtml", model);
		}

		[HttpPost]
		public void SaveHours(Guid id, int hours)
		{
			
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

        public static DetailedTeacherViewModel GenerateTeacherViewModel(Teacher teacher, HourBudget fallBudget, HourBudget springBudget)
        {
            return new DetailedTeacherViewModel
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Email = teacher.Email,
                Title = teacher.AcademicTitle,

                FallBudget = fallBudget,
                SpringBudget = springBudget,

                FallPeriodWorkload = new TeacherPeriodWorkload(teacher, fallBudget.TermYear),
                SpringPeriodWorkload = new TeacherPeriodWorkload(teacher, springBudget.TermYear)
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
                var fallBudget = teacher.GetHourBudget(fallTerm);
                var springBudget = teacher.GetHourBudget(springTerm);

                var allocatedFall = teacher.GetAllocatedHoursForTerm(fallTerm);
                var allocatedSpring = teacher.GetAllocatedHoursForTerm(springTerm);

		        var totalRemaining = fallBudget.TeachingHours + springBudget.TeachingHours - allocatedFall - allocatedSpring;
		        var allocationWarnings = GenerateAllocationWarning(fallBudget, springBudget, allocatedFall, allocatedSpring);

                output.Add(new SimpleTeacherViewModel
                {
                    Id = teacher.Id,
                    Name = teacher.Name,
                    Title = teacher.AcademicTitle,
                    FallTermAvailability = fallBudget.TermAvailability,
					SpringTermAvailability = springBudget.TermAvailability,
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
		public static Tuple<string, string> GenerateAllocationWarning(HourBudget fallBudget, HourBudget springBudget, int allocatedFall, int allocatedSpring)
		{
			var fallWarning = "";
			var springWarning = "";

			var fallAllocationShouldBe = fallBudget.TeachingHours;
			var springAllocationShouldBe = springBudget.TeachingHours;

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