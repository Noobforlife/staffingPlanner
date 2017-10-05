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
	        var db = StaffingPlanContext.GetContext();
			// Can there be teachers where the Id is null?
			var teachersdb = db.Teachers.Where(t => t.Id != null).ToList();
            var teachers = GenerateTeacherViewModelList(teachersdb);            

			return View(teachers);
        }

        // GET: /Teacher/TeacherDetails/{id}
        public ActionResult TeacherDetails(Guid id)
        {
            var db = StaffingPlanContext.GetContext();
            var teacher = db.Teachers.First(t => t.Id == id);

            var terms = db.TermYears.ToList();
            var fallTerm = terms[0].Term;
	        var fallYear = terms[0].Year; 
            var springTerm = terms[1].Term;
	        var springYear = terms[1].Year;

            var fallWorkload = db.TeacherTermAvailability
				.Where(tta =>
					tta.Teacher.Id == teacher.Id && 
					tta.TermYear.Term == fallTerm && 
					tta.TermYear.Year == fallYear)
				.AsEnumerable()
				.Select(tta => tta.Availability)
				.First();

            var springWorkload = db.TeacherTermAvailability
				.Where(tta => 
					tta.Teacher.Id == teacher.Id &&
					tta.TermYear.Term == springTerm && 
					tta.TermYear.Year == springYear)
				.AsEnumerable()
				.Select(tta => tta.Availability)
				.First();

            var courses = db.Workloads.Where(t => t.Teacher.Id == teacher.Id).Select(l => l.Course).ToList();
            var courseViewModels = CourseController.GenerateCourseViewModelList(courses);

            var teacherModel = GenerateTeacherViewModel(teacher, fallWorkload, springWorkload, courseViewModels);

            return View(teacherModel);
        }

		public static bool WorkloadHasTeacher(Teacher teacher, TeacherCourseWorkload workload)
		{
			return teacher.Equals(workload.Teacher);
		}

        
        public static int GetTotalHoursForCurrentYear(Teacher teacher)
        {
            //Hopefully this hardcoded ugliness is temporary
            var db = StaffingPlanContext.GetContext();
            var terms = db.TermYears.ToList();
            TermYear ht17 = terms[0]; //Let's hope the indexes are correct!
            TermYear vt18 = terms[1];

            //Return the sum
            return teacher.GetHourBudget(ht17).TotalTermHours + teacher.GetHourBudget(vt18).TotalTermHours;
        }


        //Methods to generate view models
        public static List<TeacherViewModel> GenerateTeacherViewModelList(List<Teacher> teachersList)
        {
            return teachersList.Select(teacher => new TeacherViewModel
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Title = teacher.AcademicTitle,
                FallWork = 100,
                SpringWork = 100,
                TotalHours = GetTotalHoursForCurrentYear(teacher),
                RemainingHours = GetTotalHoursForCurrentYear(teacher) - teacher.AllocatedHours,
			    Status = CourseController.GetStatus()
		    })
		    .ToList();
        }

        private static TeacherViewModel GenerateTeacherViewModel(Teacher teacher, int fallAvailability, int springAvailability, List<SimpleCourseViewModel> teacherCourses)
        {
            return new TeacherViewModel
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Email = teacher.Email,
                Title = teacher.AcademicTitle,
                TotalHours = GetTotalHoursForCurrentYear(teacher),
                RemainingHours = GetTotalHoursForCurrentYear(teacher) - teacher.AllocatedHours,
                FallWork = fallAvailability,
                SpringWork = springAvailability,
                Courses = teacherCourses
            };
        }


    }
}