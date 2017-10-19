using System.Collections.Generic;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;
using StaffingPlanner.Models;
using System;
using System.Linq;

namespace StaffingPlanner.Controllers
{
    public class MessagesController : Controller
    {
        #region View Methods
        // GET: Message
        public ActionResult MessagesDos()
        {
            if (Globals.UserRole != Role.DirectorOfStudies)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            var db = StaffingPlanContext.GetContext();
            var model = db.Messages.Where(x => x.DOSonly == true).ToList();

            // TODO: Generate view model

            return View("~/Views/Messages/Messages.cshtml", model);
        }
        public ActionResult MessagesTeacher(Guid TeacherId)
        {
            if (Globals.UserRole != Role.DirectorOfStudies)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            var db = StaffingPlanContext.GetContext();
            var model = db.Messages.Where(x => x.Workload!= null && x.Workload.Teacher.Id == TeacherId).ToList();

            // TODO: Generate view model

            return View("~/Views/Messages/Messages.cshtml", model);
        }

        public ActionResult RedirectToCourse(Guid Id, Guid CourseId)
        {
            var db = StaffingPlanContext.GetContext();
            var msg = db.Messages.Where(m => m.Id == Id).ToList().FirstOrDefault();
            msg.Seen = true;
            db.SaveChanges();
            return RedirectToAction("CourseDetails", "Course", new { id = CourseId});
        }

        public ActionResult RedirectToTeacherProfile(Guid Id, Guid TeacherId)
        {
            var db = StaffingPlanContext.GetContext();
            var msg = db.Messages.Where(m => m.Id == Id).ToList().FirstOrDefault();
            msg.Seen = true;
            db.SaveChanges();
            return RedirectToAction("TeacherDetails", "Teacher", new { id = TeacherId});
        }

        [ChildActionOnly]
        public PartialViewResult RenderDosNotificationWindow()
        {
            var db = StaffingPlanContext.GetContext();
            var model = db.Messages.Where(x => x.DOSonly == true).ToList();

            return PartialView("~/Views/Messages/_NotificationsWindow.cshtml", model);
        }

        [ChildActionOnly]
        public PartialViewResult RenderTeacherNotificationWindow(Guid TeacherId)
        {
            var db = StaffingPlanContext.GetContext();
            var model = db.Messages.Where(x => x.Workload != null && x.Workload.Teacher.Id == TeacherId).ToList();
            var msgs = db.Messages.Where(x => x.Course != null && x.DOSonly == false).ToList();
            var courses = db.Workloads.Where(w => w.Teacher.Id == TeacherId).Select(x => x.Course).ToList();
                        
            foreach (var message in msgs) {
                if (courses.Where(c => c.Id == message.Course.Id).Any()) {
                    model.Add(message);
               }
            }            

            return PartialView("~/Views/Messages/_NotificationsWindow.cshtml", model);
        }

        #endregion

        #region methods to Generate messages
        public static void GenerateTeacherMessage(CourseOffering course, StaffingPlanContext db)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Datetime = DateTime.Now,
                Body = "The director of studies made a change to " + course.Course.Name,
                Course = course,
                Workload = null,
                Seen=false
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        public static void GenerateTeacherMessage(TeacherCourseWorkload workload, StaffingPlanContext db )
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Datetime = DateTime.Now,
                Body = "The director of studies changed your time allocation for " + workload.Course.Course.Name + workload.Course.TermYear.ToString(),
                Course= workload.Course,
                Workload = workload,
                Seen = false
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        public static void GenerateTeacherMessageRemoval(TeacherCourseWorkload workload, StaffingPlanContext db)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Datetime = DateTime.Now,
                Body = "The director of studies has Removed you from" + workload.Course.Course.Name + workload.Course.TermYear.ToString(),
                Course = workload.Course,
                Workload = workload,
                Seen = false
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        public static void GenerateTeacherPendingMessage(TeacherCourseWorkload workload, StaffingPlanContext db)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Datetime = DateTime.Now,
                Body = "You have a pending approval for " + workload.Course.Course.Name + workload.Course.TermYear.ToString(),
                Course = workload.Course,
                Workload = workload,
                Seen = false
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        public static void GenerateDOSApprovedMessage(CourseOffering course, bool forDOS, StaffingPlanContext db)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Datetime = DateTime.Now,
                Body = course.Course.Name + " for "+ course.TermYear.ToString() + " is now approved.",
                Course = course,
                Workload = null,
                Seen = false,
                DOSonly = forDOS
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }
        public static void GenerateDOSApprovedCourseMessage(TeacherCourseWorkload workload, bool forDOS, StaffingPlanContext db)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Datetime = DateTime.Now,
                Body = workload.Teacher.Name + " has approved his workload for " + workload.Course.Course.Name + " " + workload.Course.TermYear.ToString(),
                Course = workload.Course,
                Workload = null,
                Seen = false,
                DOSonly= forDOS
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        public static void GenerateDOSPendingMessage(CourseOffering course, bool forDOS, StaffingPlanContext db)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Datetime = DateTime.Now,
                Body = course.Course.Name + " for " + course.TermYear.ToString() + " is still Pending Approval.",
                Course = course,
                Workload = null,
                Seen = false
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        #endregion 
    }
}