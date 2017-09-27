using System;
using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;
using StaffingPlanner.Models;

namespace StaffingPlanner.Controllers
{
	public class TeacherController : Controller
	{
        public ActionResult Teachers()
        {
	        var year = 17;

	        var db = StaffingPlanContext.GetContext();
			var teachers = db.Teachers.Select(t => new TeacherViewModel
	        {
				Id = t.Id,
				Name = t.Name,
				Title = t.AcademicTitle,
				TotalHours = t.GetTotalHours(),
				FallWork = t.TermEmployment[new TermYear(Term.Fall, year)],
				SpringWork = t.TermEmployment[new TermYear(Term.Spring, year + 1)],
				RemainingHours = t.GetRemainingHours()
	        });

			return View(teachers);
        }

        public ActionResult TeacherDetails(Guid id)
        {
            return View();
        }
    }
}