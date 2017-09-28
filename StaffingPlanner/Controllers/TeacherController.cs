using System;
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
            Collection<TeacherViewModel> teachers = new Collection<TeacherViewModel>();

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

			return View();
        }

        public ActionResult TeacherDetails(Guid id)
        {
            return View();
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

        // Alter to take into account how much teaching is to be done, and if there is some decrease in workload
        public int GetRemainingHoursForTeacher(Teacher teacher)
        {
            var db = StaffingPlanContext.GetContext();
            var wk = db.Workloads;
            var hours = db.Workloads.Where(w => w.Teacher.Name == teacher.Name).Select(w => w.Workload).Sum();

            return GetTotalHoursForTeacher(teacher) - hours;
        }
    }
}