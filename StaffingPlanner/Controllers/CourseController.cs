using System;
using System.Linq;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;
using StaffingPlanner.Models;
using System.Collections.Generic;
using System.Web.Services;

namespace StaffingPlanner.Controllers
{
	public class CourseController : Controller
	{
        private static readonly Random Rnd = new Random();

        #region ViewResult and PartialView Result Methods 
        
        // GET: /Course/Courses
        public ActionResult Courses()
        {
            if (!Globals.SessionUser.ContainsKey(Session["UserID"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            //Get all course offerings
            var db = StaffingPlanContext.GetContext();
            var offerings = db.CourseOfferings.Where(c => c.Course != null && c.AcademicYear.Id == Globals.CurrentAcademicYear.Id).ToList();

            //Generate course
            var courses = GenerateCourseViewModelList(offerings);

            return View(courses);
        }

        // GET: /Course/CourseDetails/{id}
        public ActionResult CourseDetails(Guid? id)
        {
            if (!Globals.SessionUser.ContainsKey(Session["UserID"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
	        if (id == null)
	        {
		        return RedirectToAction("Courses", "Course");
	        }

            var db = StaffingPlanContext.GetContext();
            var offering = db.CourseOfferings.Where(c => c.Id == id).ToList().First();

            //Generate viewmodel
            var vm = GenerateCourseDetailViewModel(offering);

            return View(vm);
        }

        [ChildActionOnly]
        public PartialViewResult RenderTeacherList(Guid Courseid)
        {
            var db = StaffingPlanContext.GetContext();
            var teachers = db.Workloads.Where(w => w.Course.Id == Courseid).Select(x => x.Teacher).ToList();

            var CourseTeacherList = GenerateCourseTeacherViewModelList(teachers, Courseid);

            var offering = db.CourseOfferings.FirstOrDefault(co => co.Id == Courseid);
            ViewBag.CourseName = offering.Course.Name;
            ViewBag.Term = offering.TermYear.ToString();
            
            return PartialView("~/Views/Course/_CourseTeacherList.cshtml", CourseTeacherList);
        }

        [ChildActionOnly]
        public PartialViewResult RenderCourseHistory(string courseCode, Guid CourseId)
        {
            var db = StaffingPlanContext.GetContext();
            var courses = db.CourseOfferings.Where(x => x.Course.Code == courseCode && x.State == CourseState.Completed).ToList();
            var currentCourse = db.CourseOfferings.Where(c => c.Id == CourseId).ToList().FirstOrDefault();
            var model = new Tuple<List<CourseOffering>, CourseOffering>(courses, currentCourse);
            return PartialView("~/Views/Course/_CourseHistory.cshtml", model );
        }

        [HttpGet]
        public PartialViewResult RenderTemplatingModal(string id, string parentid)
        {
            var db = StaffingPlanContext.GetContext();
            var templateWorkloads = db.Workloads.Where(c => c.Course.Id.ToString() == id).ToList();
            var draftWorkloads = db.Workloads.Where(c => c.Course.Id.ToString() == parentid ).ToList();
            var draft = db.CourseOfferings.Where(c => c.Id.ToString() == parentid).ToList().FirstOrDefault();

            var model = new Tuple<List<TeacherCourseWorkload>, List<TeacherCourseWorkload>,CourseOffering>(templateWorkloads, draftWorkloads, draft);

            return PartialView("~/Views/Course/_UseTemplateModal.cshtml", model);
        }

		[HttpGet]
		public PartialViewResult RenderCourseDetailsTop(Guid offeringId)
		{
			var db = StaffingPlanContext.GetContext();
			var offering = db.CourseOfferings.First(c => c.Id == offeringId);

			var viewModel = GenerateCourseDetailViewModel(offering);

			return PartialView("~/Views/Course/_CourseDetailsTop.cshtml", viewModel);
		}

        [HttpGet]
        public PartialViewResult EditCourse(Guid course)
        {
            var db = StaffingPlanContext.GetContext();
            var teachers = db.Teachers.ToList();
            var c = db.CourseOfferings.Where(x => x.Id == course).ToList().First();

            var model = new Tuple<CourseOffering, List<Teacher>>(c, teachers);

            return PartialView("~/Views/Course/_EditCourseDetails.cshtml", model);
        }

        [HttpGet]
        public PartialViewResult EditTeacher(string Id)
        {
            var db = StaffingPlanContext.GetContext();
            var work = db.Workloads.Where(w => w.Id.ToString() == Id).ToList().FirstOrDefault();
            var model = GenerateCourseTeacherViewModel(work.Teacher, work.Course.Id);

            return PartialView("~/Views/Course/_EditCourseTeacher.cshtml", model);
        }

        [ChildActionOnly]
        public PartialViewResult RenderAddCourseTeacher(Guid courseId)
        {
            var db = StaffingPlanContext.GetContext();
            var teachers = db.Teachers.ToList();
            var model = new Tuple<List<Teacher>, Guid>(teachers, courseId);

            return PartialView("~/Views/Course/_AddCourseTeacher.cshtml", model);
        }

        public ActionResult DeleteTeacher(Guid id, Guid workloadId)
        {
            var db = StaffingPlanContext.GetContext();
            var workload = db.Workloads.Where(x => x.Id == workloadId).ToList().FirstOrDefault();
            db.Workloads.Remove(workload);
            db.SaveChanges();

            return RedirectToAction("CourseDetails", "Course", new { id = id });
        }

        public ActionResult CancelChanges(Guid id) {
            return RedirectToAction("CourseDetails", "Course", new { id = id });
        }        

        [HttpPost]
        public ActionResult SaveCourseChanges(string Responsible, string CourseId, string state, string registeredStudents, string passedStudents)
        {
            var db = StaffingPlanContext.GetContext();
            var teacher = db.Teachers.Where(x => x.Id.ToString() == Responsible).ToList().First();
            var course = db.CourseOfferings.Where(x => x.Id.ToString() == CourseId).ToList().First();
            var vm = GenerateCourseDetailViewModel(course);

            var coursestate = matchState(state);
            course.CourseResponsible = teacher;
            course.State = coursestate;
            if (int.Parse(registeredStudents) <= course.NumStudents && int.Parse(passedStudents) <= course.NumStudents) {
                course.PassedStudents = int.Parse(passedStudents);
                course.RegisteredStudents = int.Parse(registeredStudents);
            }

            db.SaveChanges();

            MessagesController.GenerateTeacherMessage(course, db);
            return RedirectToAction("CourseDetails", "Course", new { id = course.Id });
        }

        [HttpPost]
        public ActionResult SaveTeacherChanges(string Id, string workloadId, string Allocated)
        {
            var db = StaffingPlanContext.GetContext();
            var Workload = db.Workloads.Where(x => x.Id.ToString() == workloadId).ToList().First();

            Workload.Workload = int.Parse(Allocated);
            db.SaveChanges();

            if (Workload.IsApproved) { ApprovalsController.Unapprove(db, Workload); }

            MessagesController.GenerateTeacherMessage(Workload,db);
            return RedirectToAction("CourseDetails", "Course", new { id = Guid.Parse(Id) });
        }

		[HttpPost]
		public ActionResult NewSaveTeacherChanges(Guid workloadId, int allocated, bool force=false)
		{
			var db = StaffingPlanContext.GetContext();
			var workload = db.Workloads.First(x => x.Id == workloadId);

			if (force || workload.Workload >= allocated)
			{
				workload.Workload = allocated;
				db.SaveChanges();

				if (workload.IsApproved) { ApprovalsController.Unapprove(db, workload); }

				MessagesController.GenerateTeacherMessage(workload, db);
				var message = force ? "Ok - forced" : "Ok - no problems";
				return Json(new { message });
			}

			var currentYear = db.AcademicYears.First(ay => ay.Id == Globals.CurrentAcademicYear.Id);
			var periodAllocationInformation = TeacherController.GenerateTeacherTopDetails(workload.Teacher.Id, currentYear);
			var diff = allocated - workload.Workload;
			var willOverallocate = diff > GetRemainingAllocatableHoursForDurationOfCourse(workload.Course, periodAllocationInformation);

			if (willOverallocate)
			{
				return Json(new { message = "Overallocation" });
			}

			workload.Workload = allocated;
			db.SaveChanges();

			if (workload.IsApproved) { ApprovalsController.Unapprove(db, workload); }

			MessagesController.GenerateTeacherMessage(workload, db);
			return Json(new { message = "Ok - no problems" });
		}

		[HttpGet]
		[ChildActionOnly]
		public PartialViewResult GetComments(Guid offeringId)
		{
			return PartialView("~/Views/Course/_CommentSection.cshtml", offeringId);
		}

		private static int GetRemainingAllocatableHoursForDurationOfCourse(CourseOffering offering,
			TeacherDetailsTopViewModel allocationInfo)
		{
			var result = 0.0;

			if (offering.TermYear.Term == Term.Fall)
			{
				switch (offering.Periods)
				{
					case Period.P1:
						result = ((double) allocationInfo.FallBudget.TeachingHours / 4) * 0.1 +
						         allocationInfo.FallPeriodBalance.P1Balance;
						break;
					case Period.P2:
						result = ((double)allocationInfo.FallBudget.TeachingHours / 4) * 0.1 +
						         allocationInfo.FallPeriodBalance.P2Balance;
						break;
					case Period.P3:
						result = ((double)allocationInfo.FallBudget.TeachingHours / 4) * 0.1 +
						         allocationInfo.FallPeriodBalance.P3Balance;
						break;
					case Period.P4:
						result = ((double)allocationInfo.FallBudget.TeachingHours / 4) * 0.1 +
						         allocationInfo.FallPeriodBalance.P4Balance;
						break;
					case Period.P1P2:
						result = ((double)allocationInfo.FallBudget.TeachingHours / 2) * 0.1 +
						         allocationInfo.FallPeriodBalance.P1Balance + allocationInfo.FallPeriodBalance.P2Balance;
						break;
					case Period.P3P4:
						result = ((double)allocationInfo.FallBudget.TeachingHours / 2) * 0.1 +
						         allocationInfo.FallPeriodBalance.P3Balance + allocationInfo.FallPeriodBalance.P4Balance;
						break;
					case Period.AllPeriods:
						result = allocationInfo.FallBudget.TeachingHours * 0.1 +
						         allocationInfo.FallPeriodBalance.P1Balance + allocationInfo.FallPeriodBalance.P2Balance +
						         allocationInfo.FallPeriodBalance.P3Balance + allocationInfo.FallPeriodBalance.P4Balance;
						break;
				}
			}
			else
			{
				switch (offering.Periods)
				{
					case Period.P1:
						result = ((double)allocationInfo.SpringBudget.TeachingHours / 4) * 0.1 +
						         allocationInfo.SpringPeriodBalance.P1Balance;
						break;
					case Period.P2:
						result = ((double)allocationInfo.SpringBudget.TeachingHours / 4) * 0.1 +
						         allocationInfo.SpringPeriodBalance.P2Balance;
						break;
					case Period.P3:
						result = ((double)allocationInfo.SpringBudget.TeachingHours / 4) * 0.1 +
						         allocationInfo.SpringPeriodBalance.P3Balance;
						break;
					case Period.P4:
						result = ((double)allocationInfo.SpringBudget.TeachingHours / 4) * 0.1 +
						         allocationInfo.SpringPeriodBalance.P4Balance;
						break;
					case Period.P1P2:
						result = ((double)allocationInfo.SpringBudget.TeachingHours / 2) * 0.1 +
						         allocationInfo.SpringPeriodBalance.P1Balance + allocationInfo.SpringPeriodBalance.P2Balance;
						break;
					case Period.P3P4:
						result = ((double)allocationInfo.SpringBudget.TeachingHours / 2) * 0.1 +
						         allocationInfo.SpringPeriodBalance.P3Balance + allocationInfo.SpringPeriodBalance.P4Balance;
						break;
					case Period.AllPeriods:
						result = allocationInfo.SpringBudget.TeachingHours * 0.1 +
						         allocationInfo.SpringPeriodBalance.P1Balance + allocationInfo.SpringPeriodBalance.P2Balance +
						         allocationInfo.SpringPeriodBalance.P3Balance + allocationInfo.SpringPeriodBalance.P4Balance;
						break;
				}
			}

			return (int)result;
		}

        [HttpPost]
        public ActionResult SaveNewTeacher(string Id, string teacherId, string Allocated)
        {
            var db = StaffingPlanContext.GetContext();
            var teacher = db.Teachers.Where(t => t.Id.ToString() == teacherId).ToList().FirstOrDefault();
            var offering = db.CourseOfferings.Where(o => o.Id.ToString() == Id).ToList().FirstOrDefault();
            var duplicate = db.Workloads.Where(x => x.Course.Id.ToString() == Id & x.Teacher.Id.ToString() == teacherId).ToList();

            if (duplicate.Count == 0) {
                var teacherworkload = new TeacherCourseWorkload
                {
                    Id = Guid.NewGuid(),
                    Course = offering,
                    Teacher = teacher,
                    Workload = int.Parse(Allocated)
                };

                db.Workloads.Add(teacherworkload);
                db.SaveChanges();
                ApprovalsController.Unapprove(db, teacherworkload);
                MessagesController.GenerateTeacherMessageAddition(teacherworkload, db);
            }
            return RedirectToAction("CourseDetails", "Course", new { id = Guid.Parse(Id) });
        }

        [HttpPost]
        public ActionResult Applytemplate(Guid templateid, Guid draftid)
        {
            var db = StaffingPlanContext.GetContext();
            var draft = db.CourseOfferings.Where(w => w.Id == draftid).ToList().FirstOrDefault();
            var draftwork = db.Workloads.Where(w => w.Course.Id == draftid).ToList();
            var templateWork = db.Workloads.Where(w => w.Course.Id == templateid).ToList();
            draftwork.ForEach(x => db.Workloads.Remove(x));
            db.SaveChanges();

            List<TeacherCourseWorkload> workloadList = new List<TeacherCourseWorkload>();
            foreach (var item in templateWork) {
                var work = new TeacherCourseWorkload
                {
                    Id = Guid.NewGuid(),
                    Course = draft,
                    Teacher = item.Teacher,
                    Workload = item.Workload,
                    IsApproved = false
                };
                workloadList.Add(work);
            }
            workloadList.ForEach(w => db.Workloads.Add(w));
            db.SaveChanges();
            return RedirectToAction("CourseDetails", "Course", new { id = draftid });
        }

        #endregion

        #region Methods for generating View Models 

        private static DetailedCourseViewModel GenerateCourseDetailViewModel(CourseOffering offering, List<Teacher> teachers, TermYear fallTerm, TermYear springTerm)
        {
            var teacherList = TeacherController.GenerateTeacherViewModelList(teachers, fallTerm, springTerm);
            var vm = new DetailedCourseViewModel
            {
                Id = offering.Id,
                Code = offering.Course.Code,
                Name = offering.Course.Name,
                TermYear = offering.TermYear,
                Period = EnumToString.PeriodToString(offering.Periods),
                Credits = offering.Credits,
                CourseResponsible = offering.CourseResponsible,
                HST = offering.HST,
                NumStudents = offering.NumStudents,
                RegisteredStudents = offering.RegisteredStudents,
                PassedStudents = offering.PassedStudents,
                TotalHours = offering.TotalHours,
                AllocatedHours = offering.AllocatedHours,
                RemainingHours = offering.RemainingHours,
                IsApproved = offering.IsApproved
            };
            return vm;
        }

        private static DetailedCourseViewModel GenerateCourseDetailViewModel(CourseOffering offering)
        {
           var vm = new DetailedCourseViewModel
            {
                Id = offering.Id,
                Code = offering.Course.Code,
                Name = offering.Course.Name,
                TermYear = offering.TermYear,
                Period = EnumToString.PeriodToString(offering.Periods),
                Credits = offering.Credits,
                CourseResponsible = offering.CourseResponsible,
                HST = offering.HST,
                NumStudents = offering.NumStudents,
                RegisteredStudents = offering.RegisteredStudents,
                PassedStudents = offering.PassedStudents,
                TotalHours = offering.TotalHours,
                AllocatedHours = offering.AllocatedHours,
                RemainingHours = offering.RemainingHours,
                Status = offering.Status,
                State = offering.State,
               IsApproved = offering.IsApproved
           };
            return vm;
        }

        public static List<SimpleCourseViewModel> GenerateCourseViewModelList(List<CourseOffering> offerings)
        {
            return offerings.Select(o => new SimpleCourseViewModel
            {
                Id = o.Id,
                Code = o.Course.Code,
                Name = o.Course.TruncatedName,
                TermYear = o.TermYear,
                Period = EnumToString.PeriodToString(o.Periods),
                CourseResponsible = o.CourseResponsible,
                Credits = o.Credits,
                AllocatedHours = o.AllocatedHours,
                RemainingHours = o.RemainingHours,
                State = o.State,
			    Status = o.Status,
                IsApproved=o.IsApproved
		    })
		    .ToList();
        }

        public static List<CourseTeacherViewModel> GenerateCourseTeacherViewModelList(List<Teacher> teachers, Guid CourseId) {
            List<CourseTeacherViewModel> list = new List<CourseTeacherViewModel>();
            foreach (var teacher in teachers) {
                var vm = GenerateCourseTeacherViewModel(teacher, CourseId);
                list.Add(vm);
            }
            return list;
        }
               
        public static CourseTeacherViewModel GenerateCourseTeacherViewModel(Teacher teacher, Guid CourseId) {

            var db = StaffingPlanContext.GetContext();
            var work = db.Workloads.Where(w => w.Course.Id == CourseId && w.Teacher.Id == teacher.Id).ToList().First();
            var FallWork = db.Workloads.Where(w => w.Teacher.Id == teacher.Id && w.Course.AcademicYear.Id == work.Course.AcademicYear.Id && w.Course.TermYear.Term == Term.Fall).ToList().Select(c => c.Workload).Sum();
            var SpringWork = db.Workloads.Where(w => w.Teacher.Id == teacher.Id && w.Course.AcademicYear.Id == work.Course.AcademicYear.Id && w.Course.TermYear.Term == Term.Spring).ToList().Select(c => c.Workload).Sum();

            var fallAvailability = teacher.GetTermAvailability(Globals.CurrentAcademicYear.StartTerm);
            var springAvailability = teacher.GetTermAvailability(Globals.CurrentAcademicYear.EndTerm);

            var teachingHours = fallAvailability.TeachingHours + springAvailability.TeachingHours;
            var remainingYear = teachingHours - FallWork - SpringWork;
            int remainingTerm;
            if (db.CourseOfferings.FirstOrDefault(co => co.Id == CourseId).TermYear.Term == Term.Fall)
            {
                remainingTerm = fallAvailability.TeachingHours - FallWork;
            }
            else
            {
                remainingTerm = springAvailability.TeachingHours - SpringWork;
            }

            var vm = new CourseTeacherViewModel
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Title = teacher.AcademicTitle,
                WorkloadId = work.Id,
                CourseId = work.Course.Id,
                WorkloadFall = FallWork,
                WorkloadSpring = SpringWork,
                CourseWorkload = work.Workload,
                RemainingHoursForYear = remainingYear,
                RemainingHoursForTerm = remainingTerm,
                CourseState = work.Course.State,
                IsApproved = work.IsApproved
            };
            return vm;
        }

        #endregion

        //helpers
        public static string GetStatus() {
            var credits = new List<string> { "allocation-bad", "allocation-good", "allocation-okay" };
            return credits[Rnd.Next(credits.Count)];
        }

        public static string GetStatus(int TotalHours, int AllocatedHours)
        {
            var percentage = (AllocatedHours/(float)TotalHours)*100;
            if (percentage >= 90)
            {
                return "allocation-good";
            }
            if (percentage >= 55 && percentage < 90)
            {
                return "allocation-okay";
            }
            return "allocation-bad";
        }

        public static CourseState matchState(string state) {
            switch (state) {
                case "Ongoing":
                    return CourseState.Ongoing;
                case "Planned":
                    return CourseState.Planned;
                case "Completed":
                    return CourseState.Completed;
                case "Draft":
                    return CourseState.Draft;
            }
            return CourseState.Planned;
        }
    }    
}