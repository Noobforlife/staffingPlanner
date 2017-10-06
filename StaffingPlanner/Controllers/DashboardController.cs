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
	        var db = StaffingPlanContext.GetContext();
	        var fallOfferings = db.CourseOfferings
				.Where(c => c.TermYear.Term == Term.Fall)
				.OrderBy(c => c.Periods)
				.ToList();
	        var springOfferings = db.CourseOfferings
				.Where(c => c.TermYear.Term == Term.Spring)
				.OrderBy(c => c.Periods)
				.ToList();

	        var dos = Globals.UserRole == Role.DirectorOfStudies;

			var fallCourses = GenerateDashViewModelList(fallOfferings, dos);
            var springCourses = GenerateDashViewModelList(springOfferings, dos);

	        var model = new DashboardViewModel
	        {
				DoS = dos,
				FallCourses = fallCourses,
				SpringCourses = springCourses
	        };

			return View(model);
        }
                
        //Method to generate list of course offerings for the dashboard view
        private static List<DashboardCourseViewModel> GenerateDashViewModelList(IEnumerable<CourseOffering> courses, bool dos)
        {
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