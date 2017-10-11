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

		[HttpPost]
		public void AlterTeacherAllocation(Guid teacherId, Guid offeringId, Guid workloadId, string hours)
		{
			try
			{
				var numHours = int.Parse(hours);
				var db = StaffingPlanContext.GetContext();
				var teacher = db.Teachers.FirstOrDefault(t => t.Id == teacherId);
				var offering = db.CourseOfferings.FirstOrDefault(o => o.Id == offeringId);
				var existingWorkload = db.Workloads
					.FirstOrDefault(w => w.Id == workloadId);

				if (existingWorkload == null && numHours > 0)
				{
					var newWorkLoad = new TeacherCourseWorkload
					{
						Id = Guid.NewGuid(),
						Teacher = teacher,
						Course = offering,
						Workload = numHours
					};
					db.Workloads.Add(newWorkLoad);
					db.SaveChanges();
				}
				else if (existingWorkload != null)
				{
					if (numHours <= 0)
					{
						db.Workloads.Remove(existingWorkload);
						db.SaveChanges();
					}
					else
					{
						existingWorkload.Workload = numHours;
						db.SaveChanges();
					}
				}
			}
			catch { }
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
			var courses = new List<TeacherCourseViewModel>();
			var name = "";
			if (teacher != null)
			{
				var taughtByTeacher = db.Workloads
					.Where(w => w.Teacher.Id == teacher.Id &&
					             ((w.Course.TermYear.Term == Term.Fall && w.Course.TermYear.Year == 2017) ||
					              (w.Course.TermYear.Term == Term.Spring && w.Course.TermYear.Year == 2018)))
					.Select(w => w.Course.Id)
					.ToList();

				var offerings = db.CourseOfferings
					.Where(co => ((co.TermYear.Term == Term.Fall && co.TermYear.Year == 2017) ||
						(co.TermYear.Term == Term.Spring && co.TermYear.Year == 2018)) &&
						taughtByTeacher.Contains(co.Id))
					.ToList();

				courses = GenerateTeacherCourseList(teacher, offerings);
				name = teacher.Name;
			}

			ViewBag.Name = name;

			return PartialView("~/Views/Teacher/_TeacherCourseList.cshtml", courses);
		}

		[HttpGet]
		public PartialViewResult EditableCourseList(Guid teacherId)
		{
			var db = StaffingPlanContext.GetContext();

			var teacher = db.Teachers.First(t => t.Id == teacherId);
			var otherCourses = new List<TeacherCourseViewModel>();
			var name = teacher.Name;
			var courses = db.Workloads
					.Where(w => w.Teacher.Id == teacherId && 
						((w.Course.TermYear.Term == Term.Fall && w.Course.TermYear.Year == 2017) ||
					    (w.Course.TermYear.Term == Term.Spring && w.Course.TermYear.Year == 2018)))
					.ToList();

			var offerings = courses.OrderBy(o => o.Course.TermYear.Year)
				.ThenBy(o => o.Course.TermYear.Term)
				.ThenBy(o => o.Course.Periods)
				.Select(o => new TeacherCourseViewModel
				{
					TeacherId = teacher.Id,
					OfferingId = o.Course.Id,
					WorkloadId = o.Id,
					Code = o.Course.Course.Code,
					CourseName = o.Course.TruncatedName,
					TermYear = o.Course.TermYear,
					Period = EnumToString.PeriodToString(o.Course.Periods),
					CourseResponsible = o.Course.CourseResponsible,
					TotalHours = o.Course.TotalHours,
					AllocatedHours = o.Course.AllocatedHours,
					RemainingHours = o.Course.RemainingHours,
					TeacherAssignedHours = teacher.GetAllocatedHoursForOffering(o.Course)
				})
				.ToList();

			ViewBag.Name = name;

			return PartialView("~/Views/Teacher/_EditableTeacherCourseList.cshtml", offerings);
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

        public static List<TeacherCourseViewModel> GenerateTeacherCourseList(Teacher teacher, List<CourseOffering> offerings)
        {
            return offerings.OrderBy(o => o.TermYear.Year)
                .ThenBy(o => o.TermYear.Term)
                .ThenBy(o => o.Periods)
                .Select(o => new TeacherCourseViewModel
            {
				TeacherId = teacher.Id,
                OfferingId = o.Id,
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