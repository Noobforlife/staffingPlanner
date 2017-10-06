using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.Models;
using StaffingPlanner.ViewModels;

namespace StaffingPlanner.Controllers
{
	public class DashboardController : Controller
	{
        // GET: /Dashboard/Index
        public ActionResult Index()
        {
            //Get all offerings for fall and spring
            //Todo: Get only current courses, not everything in database
	        var db = StaffingPlanContext.GetContext();
	        var fallOfferings = db.CourseOfferings
				.Where(co => co.TermYear.Term == Term.Fall)
				.OrderBy(co => co.Periods)
				.ToList();
	        var springOfferings = db.CourseOfferings
				.Where(co => co.TermYear.Term == Term.Spring)
				.OrderBy(co => co.Periods)
				.ToList();

            //Bool indicating whethert the user is a director of studies
	        bool directorOfStudies = Globals.UserRole == Role.DirectorOfStudies;

            //Generate viewmodels for both fall and spring courses
			var fallCourses = GenerateDashViewModelList(fallOfferings, directorOfStudies);
            var springCourses = GenerateDashViewModelList(springOfferings, directorOfStudies);

            //Create model that contains all necessary information
	        var model = new DashboardViewModel
	        {
				DoS = directorOfStudies,
				FallCourses = fallCourses,
				SpringCourses = springCourses
	        };

			return View(model);
        }
                
        //Method to generate list of course offerings for the dashboard view
        private static List<DashboardCourseViewModel> GenerateDashViewModelList(IEnumerable<CourseOffering> courses, bool dos)
        {
            //If the user is director of studies they can see all courses
	        if (dos)
	        {
		        return courses.Select(c => new DashboardCourseViewModel
			        {
				        Id = c.Id,
				        Code = c.Course.Code,
				        Name = c.Course.Name,
				        Periods = c.Periods,
				        Status = c.Status,
				        CourseResponsible = c.CourseResponsible,
			        })
			        .ToList();
	        }

            //If the user is a teacher, get the courses that they are assigned to
	        var db = StaffingPlanContext.GetContext();
	        var teacher = db.Teachers.First(t => t.Id == Globals.UserId);
	        var courseIds = db.Workloads.Where(w => w.Teacher.Id.Equals(teacher.Id)).Select(w => w.Course.Id);

	        var courseList = courses
		        .Where(c => courseIds.Contains(c.Id) || c.CourseResponsible.Id == teacher.Id)
		        .Select(c => new DashboardCourseViewModel
		        {
			        Id = c.Id,
			        Code = c.Course.Code,
			        Name = c.Course.Name,
			        Periods = c.Periods,
			        Status = c.CourseResponsible.Id == teacher.Id ? "info" : "success",
			        CourseResponsible = c.CourseResponsible,
		        })
		        .ToList();

	        return courseList;
        }
	}
}