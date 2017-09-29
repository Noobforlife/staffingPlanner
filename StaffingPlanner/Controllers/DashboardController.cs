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
        public ActionResult Index()
        {
	        var db = StaffingPlanContext.GetContext();
	        var fallCourses = db.CourseOfferings.Where(c => c.TermYear.Term == Term.Fall)
		        .Select(c => new DashboardViewModel
		        {
			        Code = c.Course.Code,
			        Name = c.Course.Name,
			        Periods = c.Periods
		        }).ToList();
	        var springCourses = db.CourseOfferings.Where(c => c.TermYear.Term == Term.Spring)
				.Select(c => new DashboardViewModel
		        {
			        Code = c.Course.Code,
			        Name = c.Course.Name,
			        Periods = c.Periods
		        }).ToList();

			var courses = new Tuple<List<DashboardViewModel>, List<DashboardViewModel>>(fallCourses, springCourses);

			return View(courses);
        }
	}
}