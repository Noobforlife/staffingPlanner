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

            var fallTerm = db.TermYears.Where(ty => ty.Term == Term.Fall && ty.Year == 2017).FirstOrDefault();
            var springTerm = db.TermYears.Where(ty => ty.Term == Term.Spring && ty.Year == 2018).FirstOrDefault();
            var teachers = GenerateTeacherViewModelList(teachersdb, fallTerm, springTerm);            

			return View(teachers);
        }

        // GET: /Teacher/TeacherDetails/{id}
        public ActionResult TeacherDetails(Guid id)
        {
            var db = StaffingPlanContext.GetContext();
            //Get the teacher with the same Id as the parameter id
            var teacher = db.Teachers.First(t => t.Id == id);

            //Gets the terms, right now we simple use HT17 and VT18
            var fallTerm = db.TermYears.Where(ty => ty.Term == Term.Fall && ty.Year == 2017).FirstOrDefault();
            var springTerm = db.TermYears.Where(ty => ty.Term == Term.Spring && ty.Year == 2018).FirstOrDefault();

            //Get availability for the teacher for the terms above
            var teacherAvailability = db.TeacherTermAvailability.Where(tta => tta.Teacher.Id == teacher.Id);
            int fallAvailability = GetTermAvailability(teacher, fallTerm);
            int springAvailability = GetTermAvailability(teacher, springTerm);

            //Gets all courses for which the teacher has assigned hours
            var courses = db.Workloads.Where(t => t.Teacher.Id == teacher.Id).Select(l => l.Course).ToList();
            var courseViewModels = CourseController.GenerateCourseViewModelList(courses);

            //Generate viewmodel
            var teacherModel = GenerateTeacherViewModel(teacher, fallAvailability, springAvailability, courseViewModels);

            //Return viewmodel to the view
            return View(teacherModel);
        }

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

		public static bool WorkloadHasTeacher(Teacher teacher, TeacherCourseWorkload workload)
		{
			return teacher.Equals(workload.Teacher);
		}

        
        public static int GetTotalHoursForCurrentYear(Teacher teacher)
        {
            var db = StaffingPlanContext.GetContext();
            TermYear ht17 = db.TermYears.Where(ty => ty.Term == Term.Fall && ty.Year == 2017).FirstOrDefault();
            TermYear vt18 = db.TermYears.Where(ty => ty.Term == Term.Spring && ty.Year == 2018).FirstOrDefault();

            //Return the sum
            return teacher.GetHourBudget(ht17).TotalTermHours + teacher.GetHourBudget(vt18).TotalTermHours;
        }


        //Methods to generate view models
        public static List<TeacherViewModel> GenerateTeacherViewModelList(List<Teacher> teachersList, TermYear fallTerm, TermYear springTerm)
        {
            return teachersList.Select(teacher => new TeacherViewModel
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Title = teacher.AcademicTitle,
                FallAvailability = GetTermAvailability(teacher, fallTerm),
                SpringAvailability = GetTermAvailability(teacher, springTerm),
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
                FallAvailability = fallAvailability,
                SpringAvailability = springAvailability,
                Courses = teacherCourses
            };
        }


    }
}