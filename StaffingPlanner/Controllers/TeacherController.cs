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
            //Get all teachers
	        var db = StaffingPlanContext.GetContext();
			var teachersdb = db.Teachers.Where(t => t.Id != null).ToList();

            //Get terms (HT17 and VT18)
            var fallTerm = db.TermYears.Where(ty => ty.Term == Term.Fall && ty.Year == 2017).FirstOrDefault();
            var springTerm = db.TermYears.Where(ty => ty.Term == Term.Spring && ty.Year == 2018).FirstOrDefault();


            //Generate the viewmodel list
            var teachers = GenerateTeacherViewModelList(teachersdb, fallTerm, springTerm);            

			return View(teachers);
        }

        // GET: /Teacher/TeacherDetails/{id}
        public ActionResult TeacherDetails(Guid id)
        {
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

            //Get all courses for which the teacher has assigned hours
            var courses = db.Workloads.Where(t => t.Teacher.Id == teacher.Id).Select(l => l.Course).ToList();
            var courseViewModels = CourseController.GenerateCourseViewModelList(courses);

            //Generate viewmodel
            var teacherModel = GenerateTeacherViewModel(teacher, teacherBugdet, courseViewModels);

            return View(teacherModel);
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

        public static DetailedTeacherViewModel GenerateTeacherViewModel(Teacher teacher, HourBudget teacherBudget, List<SimpleCourseViewModel> teacherCourses)
        {
            return new DetailedTeacherViewModel
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Email = teacher.Email,
                Title = teacher.AcademicTitle,
                RemainingHours = teacherBudget.TotalHours - teacher.AllocatedHours,
                HourBudget = teacherBudget,
                Courses = teacherCourses,
            };
        }

        public static List<SimpleTeacherViewModel> GenerateTeacherViewModelList(List<Teacher> teachersList, TermYear fallTerm, TermYear springTerm)
        {
            //FIX!!! /Simon
            return teachersList.Select(teacher => new SimpleTeacherViewModel
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Title = teacher.AcademicTitle,
                FallTermAvailability = 100,
                SpringTermAvailability = 100,
                AllocatedHoursFall = 1,
                AllocatedHoursSpring = 1,
                TotalRemainingHours = 5
		    })
		    .ToList();
        }



    }
}