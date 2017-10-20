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
            if (Globals.User == null) {
                return RedirectToAction("Login", "Account");
            }
	        var db = StaffingPlanContext.GetContext();

            //Get terms (HT17 and VT18)
            var fallTerm = db.TermYears.FirstOrDefault(ty => ty.Term == Term.Fall && ty.Year == 2017);
            var springTerm = db.TermYears.FirstOrDefault(ty => ty.Term == Term.Spring && ty.Year == 2018);

            //Generate the viewmodel list
            var teachers = GenerateTeacherViewModelList(db.Teachers.ToList(), fallTerm, springTerm);            

			return View(teachers);
        }

        // GET: /Teacher/TeacherDetails/{id}
        public ActionResult TeacherDetails(Guid? id)
        {
            if (Globals.User == null)
            {
                return RedirectToAction("Login", "Account");
            }
			if (id == null)
			{
				return RedirectToAction("Teachers", "Teacher");
			}

            var viewModel = GenerateTeacherViewModel((Guid)id, Globals.CurrentAcademicYear);

            ViewBag.Name = viewModel.Name;
            ViewBag.Firstname = viewModel.Name.Split(' ')[0];

            return View(viewModel);
        }

        #region TeacherDetails Partials

        [HttpGet]
		public PartialViewResult CourseList(Guid teacherId)
		{
			var db = StaffingPlanContext.GetContext();

			var teacher = db.Teachers.FirstOrDefault(t => t.Id == teacherId);
			var courses = new List<TeacherCourseViewModel>();
            if (teacher != null)
			{
				var taughtByTeacher = db.Workloads
					.Where(w => w.Teacher.Id == teacher.Id && w.Course.AcademicYear.Id == Globals.CurrentAcademicYear.Id)					
					.ToList();

                courses = GenerateTeacherCourseList(taughtByTeacher);
            }

			return PartialView("~/Views/Teacher/_CourseListContent.cshtml", courses);
		}

        [ChildActionOnly]
        public PartialViewResult RenderAddTeacherCourse(Guid teacherId)
        {
            var db = StaffingPlanContext.GetContext();
            var courses = db.CourseOfferings.Where(c => c.AcademicYear.Id == Globals.CurrentAcademicYear.Id && c.State != CourseState.Completed).ToList();
            var model = new Tuple<List<CourseOffering>, Guid>(courses, teacherId);

            return PartialView("~/Views/Teacher/_AddTeacherCourse.cshtml", model);
        }

        [ChildActionOnly]
        public PartialViewResult CourseHistory(Guid teacherid)
        {
            var db = StaffingPlanContext.GetContext();
            var courses = db.Workloads.Where(x => x.Teacher.Id == teacherid && x.Course.TermYear.Year < DateTime.Now.Year).ToList();

            var courseHistory = courses.OrderBy(c => c.Course.TermYear.Year)
            .ThenBy(c => c.Course.TermYear.Term)
            .Select(c => new TeacherCourseHistoryViewModel
            {
                CourseId = c.Course.Id,
                CourseName = c.Course.Course.Name,
                TermYear = c.Course.TermYear.ToString(),
                Period = EnumToString.PeriodToString(c.Course.Periods),
                CourseResponsibe = c.Course.CourseResponsible,
                HoursTaught = c.Workload
            }).ToList();


            return PartialView("~/Views/Teacher/_TeacherCourseHistory.cshtml", courseHistory);
        }

		[HttpGet]
		public PartialViewResult TeacherDetailsTop(Guid teacherId)
		{
			var db = StaffingPlanContext.GetContext();
			var teacher = db.Teachers.First(t => t.Id == teacherId);

			var viewModel = GenerateTeacherTopDetails(teacherId, Globals.CurrentAcademicYear);

			ViewBag.Name = teacher.Name;
			ViewBag.Firstname = teacher.Name.Split(' ')[0];

			return PartialView("~/Views/Teacher/_TeacherDetailsTop.cshtml", viewModel);
		}

		[HttpGet]
        public PartialViewResult EditableTeacherDetails(Guid teacherId)
		{
			var db = StaffingPlanContext.GetContext();
			var teacher = db.Teachers.First(t => t.Id == teacherId);

            var viewModel = GenerateTeacherTopDetails(teacherId, Globals.CurrentAcademicYear);

            ViewBag.Name = teacher.Name;
            ViewBag.Firstname = teacher.Name.Split(' ')[0];

            return PartialView("~/Views/Teacher/_TeacherDetailsTopEditable.cshtml", viewModel);
        }

        [HttpGet]
		public PartialViewResult EditableCourseList(Guid teacherId)
		{
			var db = StaffingPlanContext.GetContext();

			var teacher = db.Teachers.First(t => t.Id == teacherId);
			var courses = db.Workloads
					.Where(w => w.Teacher.Id == teacherId && 
						((w.Course.TermYear.Term == Term.Fall && w.Course.TermYear.Year == 2017) ||
					    (w.Course.TermYear.Term == Term.Spring && w.Course.TermYear.Year == 2018)))
					.ToList();

			var offerings = courses.OrderBy(o => o.Course.TermYear.Year)
				.ThenBy(o => o.Course.TermYear.Term)
				.ThenBy(o => o.Course.Periods)
				.Select(o => new TeacherCourseViewModel
				{
					TeacherId = teacher.Id,
					OfferingId = o.Course.Id,
					WorkloadId = o.Id,
					Code = o.Course.Course.Code,
					CourseName = o.Course.TruncatedName,
					TermYear = o.Course.TermYear,
					Period = EnumToString.PeriodToString(o.Course.Periods),
					CourseResponsible = o.Course.CourseResponsible,
					TotalHours = o.Course.TotalHours,
					AllocatedHours = o.Course.AllocatedHours,
					RemainingHours = o.Course.RemainingHours,
					TeacherAssignedHours = teacher.GetAllocatedHoursForOffering(o.Course),
                    CourseState = o.Course.State,
                    CourseStatus = o.Course.Status,
                    IsApproved = o.IsApproved
				})
				.ToList();

            return PartialView("~/Views/Teacher/_CourseListContentEditable.cshtml", offerings);
		}

        #endregion

        #region Updating the database

        [HttpPost]
        public void AlterTeacherAllocation(Guid teacherId, Guid offeringId, Guid workloadId, string hours)
        {
            try
            {
                var numHours = int.Parse(hours);
                var db = StaffingPlanContext.GetContext();
                var teacher = db.Teachers.FirstOrDefault(t => t.Id == teacherId);
                var offering = db.CourseOfferings.FirstOrDefault(o => o.Id == offeringId);
                var existingWorkload = db.Workloads
                    .FirstOrDefault(w => w.Id == workloadId);

                if (existingWorkload == null && numHours > 0)
                {
                    var newWorkLoad = new TeacherCourseWorkload
                    {
                        Id = Guid.NewGuid(),
                        Teacher = teacher,
                        Course = offering,
                        Workload = numHours
                    };
                    db.Workloads.Add(newWorkLoad);
                    db.SaveChanges();
                }
                else if (existingWorkload != null)
                {
                    if (numHours <= 0)
                    {
                        db.Workloads.Remove(existingWorkload);
                        db.SaveChanges();
                        MessagesController.GenerateTeacherMessageRemoval(existingWorkload, db);
                    }
                    else
                    {
                        existingWorkload.Workload = numHours;
                        db.SaveChanges();
                        MessagesController.GenerateTeacherMessage(existingWorkload, db);
                    }
                }
            }
            catch { }
        }

        [HttpPost]
        public ActionResult SaveNewCourse(string Id, string courseId, string Allocated)
        {
            var db = StaffingPlanContext.GetContext();
            var teacher = db.Teachers.Where(t => t.Id.ToString() == Id).ToList().FirstOrDefault();
            var offering = db.CourseOfferings.Where(o => o.Id.ToString() == courseId).ToList().FirstOrDefault();
            var duplicate = db.Workloads.Where(x => x.Course.Id.ToString() == courseId & x.Teacher.Id.ToString() == Id).ToList();

            if (duplicate.Count == 0)
            {
                var teacherworkload = new TeacherCourseWorkload
                {
                    Id = Guid.NewGuid(),
                    Course = offering,
                    Teacher = teacher,
                    Workload = int.Parse(Allocated)
                };

                db.Workloads.Add(teacherworkload);
                db.SaveChanges();
            }
            return RedirectToAction("TeacherDetails", "Teacher", new { id = Guid.Parse(Id) });
        }

        [HttpPost]
        public ActionResult SaveTeacherChanges(Guid teacherId, int fallNonCourseWorkload, int springNonCourseWorkload,
            int teachingShare, int researchShare, int adminShare, int otherShare)
        {
            var currentYear = Globals.CurrentAcademicYear;

            //Update NonCourseHours for both terms
            AlterNonCourseHoursAllocation(teacherId, currentYear.StartTerm, fallNonCourseWorkload);
            AlterNonCourseHoursAllocation(teacherId, currentYear.EndTerm, springNonCourseWorkload);

            //Update task shares
            AlterTeacherResearchShare(teacherId, teachingShare, researchShare, adminShare, otherShare);

            return RedirectToAction("TeacherDetails", "Teacher", new { id = teacherId });
        }

        public void AlterNonCourseHoursAllocation(Guid teacherId, TermYear term, int newHours)
        {
            var db = StaffingPlanContext.GetContext();

            var teacher = db.Teachers.FirstOrDefault(t => t.Id == teacherId);

            var existingNonCourseWorkload = db.NonCourseWorkloads
                .FirstOrDefault(w => w.Teacher.Id == teacher.Id
                && w.TermYear.Year == term.Year
                && w.TermYear.Term == term.Term);

            if (existingNonCourseWorkload == null)
            {
                var newNonCourseWorkload = new NonCourseWorkload
                {
                    Id = Guid.NewGuid(),
                    Teacher = teacher,
                    TermYear = term,
                    Workload = newHours
                };
                db.TermYears.Attach(term);
                db.NonCourseWorkloads.Add(newNonCourseWorkload);
                db.SaveChanges();
            }
            else if (existingNonCourseWorkload != null)
            {
                existingNonCourseWorkload.Workload = newHours;
                db.SaveChanges();
            }

        }

        public void AlterTeacherResearchShare(Guid teacherId, int newTeachingShare, int newResearchShare, int newAdminShare, int newOtherShare)
        {
            var db = StaffingPlanContext.GetContext();

            var teacher = db.Teachers.FirstOrDefault(t => t.Id == teacherId);
            var currentYear = db.AcademicYears.Where(y => y.Id == Globals.CurrentAcademicYear.Id).FirstOrDefault();


            decimal teachingShare = newTeachingShare / 100m;
            decimal researchShare = newResearchShare / 100m;
            decimal adminShare = newAdminShare / 100m;
            decimal otherShare = newOtherShare / 100m;

            var currentShares = db.TeacherTaskShare.Where(tts => tts.Teacher.Id == teacherId 
            && tts.AcademicYear.StartTerm.Term == currentYear.StartTerm.Term
            && tts.AcademicYear.StartTerm.Year == currentYear.StartTerm.Year).FirstOrDefault();

            if (currentShares != null)
            {
                currentShares.TeachingShare = teachingShare;
                currentShares.ResearchShare = researchShare;
                currentShares.AdminShare = adminShare;
                currentShares.OtherShare = otherShare;
            }
            else
            {
                var teacherShares = new TeacherTaskShare
                {
                    Id = Guid.NewGuid(),
                    AcademicYear = currentYear,
                    Teacher = teacher,
                    TeachingShare = teachingShare,
                    ResearchShare = researchShare,
                    AdminShare = adminShare,
                    OtherShare = otherShare
                };
                db.TeacherTaskShare.Add(teacherShares);
            }
            db.SaveChanges();
        }

        #endregion

        #region ViewModel Generation

        public static DetailedTeacherViewModel GenerateTeacherViewModel(Guid teacherId, AcademicYear year)
        {
            var db = StaffingPlanContext.GetContext();
            //Get the teacher with the same Id as the parameter id
            var teacher = db.Teachers.First(t => t.Id == teacherId);

            return new DetailedTeacherViewModel
            {
                Id = teacher.Id,
                Name = teacher.Name
            };
        }

		public static TeacherDetailsTopViewModel GenerateTeacherTopDetails(Guid teacherId, AcademicYear year)
		{
			var db = StaffingPlanContext.GetContext();
			//Get the teacher with the same Id as the parameter id
			var teacher = db.Teachers.First(t => t.Id == teacherId);

			var fallBudget = teacher.GetTermAvailability(year.StartTerm);
			var springBudget = teacher.GetTermAvailability(year.EndTerm);

			return new TeacherDetailsTopViewModel()
			{
				Id = teacher.Id,
				Email = teacher.Email,
				Title = teacher.AcademicTitle,

				FallBudget = fallBudget,
				SpringBudget = springBudget,


				FallWorkload = new TeacherTermWorkload(teacher, fallBudget.TermYear),
				SpringWorkload = new TeacherTermWorkload(teacher, springBudget.TermYear)
			};
		}
            

        public static List<TeacherCourseViewModel> GenerateTeacherCourseList(List<TeacherCourseWorkload> offerings)
        {
            return offerings.OrderBy(o => o.Course.TermYear.Year)
                .ThenBy(o => o.Course.TermYear.Term)
                .ThenBy(o => o.Course.Periods)
                .Select(o => new TeacherCourseViewModel
                {
                    TeacherId = o.Teacher.Id,
                    OfferingId = o.Course.Id,
                    Code = o.Course.Course.Code,
                    CourseName = o.Course.TruncatedName,
                    TermYear = o.Course.TermYear,
                    Period = EnumToString.PeriodToString(o.Course.Periods),
                    CourseResponsible = o.Course.CourseResponsible,
                    TotalHours = o.Course.TotalHours,
                    AllocatedHours = o.Course.AllocatedHours,
                    RemainingHours = o.Course.RemainingHours,
                    TeacherAssignedHours = o.Teacher.GetAllocatedHoursForOffering(o.Course),
                    CourseState = o.Course.State,
                    CourseStatus = o.Course.Status,
                    IsApproved = o.IsApproved,
                    WorkloadId = o.Id
                })
            .ToList();
        }

        public static List<SimpleTeacherViewModel> GenerateTeacherViewModelList(List<Teacher> teachersList, TermYear fallTerm, TermYear springTerm)
        {
	        var output = new List<SimpleTeacherViewModel>();

	        foreach (var teacher in teachersList)
	        {
                var fallBudget = teacher.GetTermAvailability(fallTerm);
                var springBudget = teacher.GetTermAvailability(springTerm);

                var allocatedFall = teacher.GetAllocatedHoursForTerm(fallTerm);
                var allocatedSpring = teacher.GetAllocatedHoursForTerm(springTerm);

		        var totalRemaining = fallBudget.TeachingHours + springBudget.TeachingHours - allocatedFall - allocatedSpring;
		        var allocationWarnings = GenerateAllocationWarning(fallBudget, springBudget, allocatedFall, allocatedSpring);

                output.Add(new SimpleTeacherViewModel
                {
                    Id = teacher.Id,
                    Name = teacher.Name,
                    Title = teacher.AcademicTitle,
                    FallTermEmployment = fallBudget.TermEmployment,
					SpringTermEmployment = springBudget.TermEmployment,
					AllocatedHoursFall = allocatedFall,
					StatusFall = allocationWarnings.Item1,
					AllocatedHoursSpring = allocatedSpring,
					StatusSpring = allocationWarnings.Item2,
					TotalRemainingHours = totalRemaining
				});

	        }

	        return output;
        }

        #endregion

        // Feel free to replace "warning" with "status" if it makes more sense for its intended use
        public static Tuple<string, string> GenerateAllocationWarning(TeacherTermAvailability fallBudget, TeacherTermAvailability springBudget, int allocatedFall, int allocatedSpring)
		{
			var fallWarning = "";
			var springWarning = "";

			var fallAllocationShouldBe = fallBudget.TeachingHours;
			var springAllocationShouldBe = springBudget.TeachingHours;

			// Set the fall warning attribute according to allocation status
			if (allocatedFall >= fallAllocationShouldBe * 1.5)
			{
				fallWarning = "";
			}
			else if (allocatedFall >= fallAllocationShouldBe * 1.1)
			{
				fallWarning = "";
			}

			// Set the spring warning attribute according to allocation status
			if (allocatedSpring >= springAllocationShouldBe * 1.5)
			{
				springWarning = "";
			}
			else if (allocatedSpring >= springAllocationShouldBe * 1.1)
			{
				springWarning = "";
			}

			return new Tuple<string, string>(fallWarning, springWarning);
		}

    }
}