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
        //View methods
        public ActionResult Teachers()
        {
	        var db = StaffingPlanContext.GetContext();
			var teachersdb = db.Teachers.Where(t => t.Id != null).ToList();

            var teachers = GenerateTeacherViewModelList(teachersdb);            

			return View(teachers);
        }

        public ActionResult TeacherDetails(Guid id)
        {
            var db = StaffingPlanContext.GetContext();
            var teacher = db.Teachers.Where(t => t.Id == id).First();

            List<TermYear> terms = db.TermYears.ToList();
            TermYear fallTerm = terms[0];
            TermYear springTerm = terms[1];

            //int fallWorkload = db.TeacherTermAvailability.Where(tta => tta.TermYear == fallTerm).AsEnumerable().Select(tta => tta.Availability).First();
            //int springWorkload = db.TeacherTermAvailability.Where(tta => tta.TermYear == springTerm).AsEnumerable().Select(tta => tta.Availability).First();
            //This doesn't work, causes the following exception:
            //System.NotSupportedException: 'Unable to create a constant value of type 'StaffingPlanner.Models.TermYear'. Only primitive types or enumeration types are supported in this context.'

            var courses = db.Workloads.Where(t => t.Teacher.Id == teacher.Id).Select(l => l.Course).ToList();
            var courseViewModels = CourseController.GenerateCourseViewModelList(courses);

            var teacherModel = GenerateTeacherViewModel(teacher, 100, 100, courseViewModels);

            return View(teacherModel);
        }

        // Alter to take into account any changes in workload
        public static int GetTotalHoursForTeacher(Teacher teacher)
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
        public static int GetRemainingHoursForTeacher(Teacher teacher)
        {
            var db = StaffingPlanContext.GetContext();

			var hours = db.Workloads.Where(w => w.Teacher.Id.Equals(teacher.Id)).ToList().Sum(w => w.Workload);

			return GetTotalHoursForTeacher(teacher) - hours;
        }

        //Methods to generate view models
        public static List<TeacherViewModel> GenerateTeacherViewModelList(List<Teacher> teachersList)
        {
            var teachers = new List<TeacherViewModel>();
            foreach (var t in teachersList)
            {
                var tvm = new TeacherViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Title = t.AcademicTitle,
                    TotalHours = GetTotalHoursForTeacher(t),
                    RemainingHours = GetRemainingHoursForTeacher(t),
                    Status= CourseController.GetStatus()
                };
                teachers.Add(tvm);
            }
            return teachers;
        }

        private static TeacherViewModel GenerateTeacherViewModel(Teacher teacher, int fallAvailability, int springAvailability, List<SimpleCourseViewModel> teacherCourses)
        {
            TeacherViewModel teacherModel = new TeacherViewModel
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Email = teacher.Email,
                Title = teacher.AcademicTitle,
                TotalHours = GetTotalHoursForTeacher(teacher),
                RemainingHours = GetRemainingHoursForTeacher(teacher),
                FallWork = fallAvailability,
                SpringWork = springAvailability,
                Courses = teacherCourses
            };

            return teacherModel;
        }

    }
}