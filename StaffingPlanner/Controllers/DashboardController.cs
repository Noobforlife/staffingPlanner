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

            var fallCourses = GenerateDashViewModelList(fallOfferings);
            var springCourses = GenerateDashViewModelList(springOfferings);

			var courses = new Tuple<List<DashboardViewModel>, List<DashboardViewModel>>(fallCourses, springCourses);

			return View(courses);
        }
                
        //Method to generate list of course offerings for the dashboard view
        private static List<DashboardViewModel> GenerateDashViewModelList(IEnumerable<CourseOffering> courses)
        {
	        return courses.Select(c => new DashboardViewModel
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
	}
}