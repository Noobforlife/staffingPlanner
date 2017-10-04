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

        //Methods to generate view models
        public static List<TeacherViewModel> GenerateTeacherViewModelList(List<Teacher> teachersList)
        {
	        return teachersList.Select(t => new TeacherViewModel
		    {
			    Id = t.Id,
			    Name = t.Name,
			    Title = t.AcademicTitle,
			    FallWork = 100,
			    SpringWork = 100,
			    TotalHours = t.TotalHours,
			    RemainingHours = t.RemainingHours,
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
                TotalHours = teacher.TotalHours,
                RemainingHours = teacher.RemainingHours,
                FallWork = fallAvailability,
                SpringWork = springAvailability,
                Courses = teacherCourses
            };
        }

    }
}