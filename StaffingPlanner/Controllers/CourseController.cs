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
            if (Globals.User == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //Get all course offerings
            var db = StaffingPlanContext.GetContext();
            var offerings = db.CourseOfferings.Where(c => c.Course != null).ToList();

            //Generate course
            var courses = GenerateCourseViewModelList(offerings);

            return View(courses);
        }

        // GET: /Course/CourseDetails/{id}
        public ActionResult CourseDetails(Guid? id)
        {
            if (Globals.User == null)
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
            
            return PartialView("~/Views/Course/_CourseTeacherList.cshtml", CourseTeacherList);
        }

        [ChildActionOnly]
        public PartialViewResult RenderCourseHistory(string courseCode)
        {
            var db = StaffingPlanContext.GetContext();
            var courses = db.CourseOfferings.Where(x => x.Course.Code == courseCode && x.State == CourseState.Completed).ToList();
            return PartialView("~/Views/Course/_CourseHistory.cshtml", courses);
        }

        [HttpGet]
        public PartialViewResult EditCourse(Guid course)
        {
            var db = StaffingPlanContext.GetContext();
            var teachers = db.Teachers.Where(x => x.Id != null).ToList();
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
        public ActionResult SaveCourseChanges(string Responsible, string CourseId)
        {
            var db = StaffingPlanContext.GetContext();
            var teacher = db.Teachers.Where(x => x.Id.ToString() == Responsible).ToList().First();
            var course = db.CourseOfferings.Where(x => x.Id.ToString() == CourseId).ToList().First();
            var vm = GenerateCourseDetailViewModel(course);

            course.CourseResponsible = teacher;
            var t = course;
            db.SaveChanges();

            return RedirectToAction("CourseDetails", "Course", new { id = course.Id });
        }

        [HttpPost]
        public ActionResult SaveTeacherChanges(string Id, string workloadId, string Allocated)
        {
            var db = StaffingPlanContext.GetContext();
            var Workload = db.Workloads.Where(x => x.Id.ToString() == workloadId).ToList().First();

            Workload.Workload = int.Parse(Allocated);
            db.SaveChanges();

            return RedirectToAction("CourseDetails", "Course", new { id = Guid.Parse(Id) });
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
            }
            return RedirectToAction("CourseDetails", "Course", new { id = Guid.Parse(Id) });
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
                RemainingHours = offering.RemainingHours
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
                Status = GetStatus(offering.TotalHours,offering.AllocatedHours),
                State = offering.State
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
			    Status = o.Status
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

            var teachingHours = teacher.GetHourBudget(Globals.CurrentAcademicYear.StartTerm).TeachingHours + teacher.GetHourBudget(Globals.CurrentAcademicYear.EndTerm).TeachingHours;
            var remaining = teachingHours - FallWork - SpringWork;
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
                RemainingHours = remaining,
                CourseState = work.Course.State
            };
            return vm;
        }

        #endregion

        //helpers
        public static string GetStatus() {
            var credits = new List<string> {"warning","success","danger"};
            return credits[Rnd.Next(credits.Count)];
        }

        public static string GetStatus(int TotalHours, int AllocatedHours)
        {
            float percentage = (AllocatedHours/(float)TotalHours)*100;
            if (percentage >= 90)
            {
                return "success";
            }
            else if (percentage >= 55 && percentage < 90)
            {
                return "warning";
            }
            return "danger";
        }               

    }
    
}