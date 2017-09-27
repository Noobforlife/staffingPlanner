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
	        var schoolYear = 17;

	        var db = StaffingPlanContext.GetContext();
			var teachers = db.Teachers.Select(t => new TeacherViewModel
	        {
				Id = t.Id,
				Name = t.Name,
				Title = t.AcademicTitle,
				TotalHours = t.GetTotalHours(),
				FallWork = t.TermEmployment[new TermYear(Term.Fall, schoolYear)],
				SpringWork = t.TermEmployment[new TermYear(Term.Spring, schoolYear)],
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