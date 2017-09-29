using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;
using StaffingPlanner.Models;
using System.Collections.ObjectModel;

namespace StaffingPlanner.Controllers
{
	public class TeacherController : Controller
	{
        public ActionResult Teachers()
        {
	        var db = StaffingPlanContext.GetContext();
			var teachersdb = db.Teachers.Where(t => t.Id != null).ToList();
            var teachers = new List<TeacherViewModel>();

            foreach (var t in teachersdb) {
                var tvm = new TeacherViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Title= t.AcademicTitle,
                    TotalHours = GetTotalHoursForTeacher(t),
                    RemainingHours = GetRemainingHoursForTeacher(t)
                };
                teachers.Add(tvm);
            }

            ViewBag.teachers = teachers;

			return View(teachers);
        }

        public ActionResult TeacherDetails(Guid id)
        {
            var db = StaffingPlanContext.GetContext();
            var teacher = db.Teachers.Where(t => t.Id == id).First();

            List<TermYear> terms = db.TermYears.Take(2).ToList();
            var fallLevel = db.TeacherTermAvailability.Where(tta => tta.TermYear.Id.Equals(terms[0].Id)).Select(tta => tta.Availability).First();
            var springLevel = db.TeacherTermAvailability.Where(tta => tta.TermYear.Id.Equals(terms[0].Id)).Select(tta => tta.Availability).First();

            TeacherViewModel teacherModel = new TeacherViewModel
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Title = teacher.AcademicTitle,
                TotalHours = GetTotalHoursForTeacher(teacher),
                RemainingHours = GetRemainingHoursForTeacher(teacher),
                FallWork = 100,
                SpringWork = springLevel              
            };

            return View(teacherModel);
        }

        // Alter to take into account any changes in workload
        public int GetTotalHoursForTeacher(Teacher teacher)
        {
            var year = int.Parse("19" + teacher.PersonalNumber.Substring(0, 2));
            var month = int.Parse(teacher.PersonalNumber.Substring(2, 2));
            var day = int.Parse(teacher.PersonalNumber.Substring(4, 2));
            var birthdate = new DateTime(year, month, day);
            var age = new DateTime().Subtract(birthdate);

            if (age.Days / 365 > 40)
            {
                return 1700;
            }
            if (age.Days / 365 > 30 && age.Days / 365 <= 40)
            {
                return 1735;
            }

            return 1756;
        }

		public static bool WorkloadHasTeacher(Teacher teacher, TeacherCourseWorkload workload)
		{
			return teacher.Equals(workload.Teacher);
		}

        // Alter to take into account how much teaching is to be done, and if there is some decrease in workload
        public int GetRemainingHoursForTeacher(Teacher teacher)
        {
            var db = StaffingPlanContext.GetContext();

			var hours = db.Workloads.Where(w => w.Teacher.Id.Equals(teacher.Id)).ToList().Sum(w => w.Workload);


			return GetTotalHoursForTeacher(teacher) - hours;
        }
    }
}