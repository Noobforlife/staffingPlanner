using System;
using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;


namespace StaffingPlanner.Controllers
{
	public class TeacherController : Controller
	{
        public ActionResult Teachers()
        {
	        var schoolYear = "17/18";

	        var db = StaffingPlanContext.GetContext();
			var teachers = db.Teachers.Select(t => new TeacherViewModel
	        {
				Id = t.Id,
				Name = t.Name,
				Title = t.AcademicTitle,
				TotalHours = t.GetTotalHours(),
				FallWork = t.GetContract(schoolYear).FallWork,
				SpringWork = t.GetContract(schoolYear).SpringWork,
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