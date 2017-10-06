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

            //Get terms (HT17 and VT18)
            var fallTerm = db.TermYears.FirstOrDefault(ty => ty.Term == Term.Fall && ty.Year == 2017);
            var springTerm = db.TermYears.FirstOrDefault(ty => ty.Term == Term.Spring && ty.Year == 2018);

            //Generate the viewmodel list
            var teachers = GenerateTeacherViewModelList(db.Teachers.ToList(), fallTerm, springTerm);            

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
	        var db = StaffingPlanContext.GetContext();
	        var output = new List<SimpleTeacherViewModel>();

	        foreach (var teacher in teachersList)
	        {
				var allocatedFall = db.Workloads.Where(w => w.Teacher.Id == teacher.Id && w.Course.TermYear.Term == fallTerm.Term && w.Course.TermYear.Year == fallTerm.Year).ToList().Sum(w => w.Workload);
		        var allocatedSpring = db.Workloads.Where(w => w.Teacher.Id == teacher.Id && w.Course.TermYear.Term == springTerm.Term && w.Course.TermYear.Year == springTerm.Year).ToList().Sum(w => w.Workload); 
		        var teacherHourBudget = teacher.GetHourBudget(fallTerm, springTerm);
		        var totalRemaining = teacherHourBudget.TeachingHours - allocatedFall - allocatedSpring;

				output.Add(new SimpleTeacherViewModel
				{
					Id = teacher.Id,
					Name = teacher.Name,
					Title = teacher.AcademicTitle,
					FallTermAvailability = teacherHourBudget.FallAvailability,
					SpringTermAvailability = teacherHourBudget.SpringAvailability,
					AllocatedHoursFall = allocatedFall,
					AllocatedHoursSpring = allocatedSpring,
					TotalRemainingHours = totalRemaining
				});

	        }

	        return output;
        }



    }
}