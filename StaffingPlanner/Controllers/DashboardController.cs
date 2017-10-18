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
        public ActionResult Index(int year = 2017)
        {
            if (Globals.User == null)
            {
                return RedirectToAction("Login", "Account");
            }

	        var db = StaffingPlanContext.GetContext();

	        var currentYear = year == 2017 
				? Globals.CurrentAcademicYear 
				: db.AcademicYears.FirstOrDefault(ay => ay.StartTerm.Term == Term.Fall && ay.StartTerm.Year == year);

			if (currentYear == null)
	        {
		        currentYear = Globals.CurrentAcademicYear;
	        }

            //Get all offerings for fall and spring
  	        var fallOfferings = db.CourseOfferings
				.Where(co => co.TermYear.Term == Term.Fall && co.AcademicYear.Id == currentYear.Id)
				.OrderBy(co => co.Periods)
				.ToList();
	        var springOfferings = db.CourseOfferings
				.Where(co => co.TermYear.Term == Term.Spring && co.AcademicYear.Id == currentYear.Id)
				.OrderBy(co => co.Periods)
				.ToList();

            //Bool indicating whethert the user is a director of studies
	        var directorOfStudies = Globals.UserRole == Role.DirectorOfStudies;

            //Generate viewmodels for both fall and spring courses
			var fallCourses = GenerateDashViewModelList(fallOfferings, directorOfStudies);
            var springCourses = GenerateDashViewModelList(springOfferings, directorOfStudies);

            //Create model that contains all necessary information
	        var model = new DashboardViewModel
	        {
				DoS = directorOfStudies,
				Year = currentYear.StartTerm.Year,
				TopPanel = GenerateTopPanelViewModel(),
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
				        Status = GetColorForOffering(c),
				        CourseResponsible = c.CourseResponsible,
                        IsApproved = c.IsApproved
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
					State = c.State,
			        CourseResponsible = c.CourseResponsible,
		        })
		        .ToList();

	        return courseList;
        }

		private static TopPanelViewModel GenerateTopPanelViewModel()
		{
			var db = StaffingPlanContext.GetContext();
			var teachers = db.Teachers.ToList();
			var workloads = db.Workloads.ToList();
			var fallTerm = db.TermYears.FirstOrDefault(ty => ty.Term == Term.Fall && ty.Year == 2017);
			var springTerm = db.TermYears.FirstOrDefault(ty => ty.Term == Term.Spring && ty.Year == 2018);

			var model = new TopPanelViewModel();
			
			foreach (var teacher in teachers)
			{
				var teachingHours = teacher.GetTermAvailability(fallTerm).TeachingHours + teacher.GetTermAvailability(springTerm).TeachingHours;
				model.TotalRemaining += teachingHours;

				switch (teacher.AcademicTitle)
				{
					case AcademicTitle.Professor:
						model.ProfessorRemaining += teachingHours;
						break;
					case AcademicTitle.Lektor:
						model.LektorRemaining += teachingHours;
						break;
					case AcademicTitle.Adjunkt:
						model.AdjunktRemaining += teachingHours;
						break;
					case AcademicTitle.Amanuens:
						model.AmanuensRemaining += teachingHours;
						break;
					case AcademicTitle.Doktorand:
						model.DoktorandRemaining += teachingHours;
						break;
					case AcademicTitle.Assistent:
						model.AssistentRemaining += teachingHours;
						break;
				}
			}
			foreach (var workload in workloads)
			{
				model.TotalRemaining -= workload.Workload;
				switch (workload.Teacher.AcademicTitle)
				{
					case AcademicTitle.Professor:
						model.ProfessorRemaining -= workload.Workload;
						break;
					case AcademicTitle.Lektor:
						model.LektorRemaining -= workload.Workload;
						break;
					case AcademicTitle.Adjunkt:
						model.AdjunktRemaining -= workload.Workload;
						break;
					case AcademicTitle.Amanuens:
						model.AmanuensRemaining -= workload.Workload;
						break;
					case AcademicTitle.Doktorand:
						model.DoktorandRemaining -= workload.Workload;
						break;
					case AcademicTitle.Assistent:
						model.AssistentRemaining -= workload.Workload;
						break;
				}
			}

			return model;
		}

		private static string GetColorForOffering(CourseOffering offering)
		{
			if (offering.State == CourseState.Draft)
			{
				return "progress-bar-" + CourseController.GetStatus();
			}
			if (offering.State == CourseState.Completed)
			{
				return "background-grey";
			}

			return "progress-bar-info";
		}
	}
}