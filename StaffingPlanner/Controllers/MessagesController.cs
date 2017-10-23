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
            if (Globals.SessionUser[Session["UserID"].ToString()].UserRole != Role.DirectorOfStudies)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            var db = StaffingPlanContext.GetContext();
            var model = db.Messages.Where(x => x.DOSonly == true).OrderByDescending(x => x.Datetime).ToList();

            // TODO: Generate view model

            return View("~/Views/Messages/Messages.cshtml", model);
        }
        public ActionResult MessagesTeacher(Guid Id)
        {
            var db = StaffingPlanContext.GetContext();
            var model = db.Messages.Where(x => x.Workload!= null && x.Workload.Teacher.Id == Id).OrderByDescending(x => x.Datetime).ToList();
            var msgs = db.Messages.Where(x => x.Course != null && x.DOSonly == false).ToList();
            var courses = db.Workloads.Where(w => w.Teacher.Id == Id).Select(x => x.Course).ToList();

            foreach (var message in msgs)
            {
                if (courses.Where(c => c.Id == message.Course.Id).Any() && message.Workload == null)
                {
                    model.Add(message);
                }
            }
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
        [HttpPost]
        public ActionResult NewRequest()
        {
            Guid Id = Guid.NewGuid();
            var db = StaffingPlanContext.GetContext();
            var course = db.CourseOfferings.Where(c => c.Id == Id).ToList().FirstOrDefault();
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Datetime = DateTime.Now,
                Body = " Test",
                Course = course,
                Workload = null,
                Seen = false,
                MessageType = MessageType.Request,
                DOSonly=true
            };
            db.Messages.Add(msg);
            db.SaveChanges();
            return null;
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
            var model = db.Messages.Where(x => x.DOSonly == true).OrderByDescending(x => x.Datetime).ToList();

            return PartialView("~/Views/Messages/_NotificationsWindow.cshtml", model);
        }

        [ChildActionOnly]
        public PartialViewResult RenderTeacherNotificationWindow(Guid TeacherId)
        {
            var db = StaffingPlanContext.GetContext();
            var model = db.Messages.Where(x => x.Workload != null && x.Workload.Teacher.Id == TeacherId).OrderByDescending(x => x.Datetime).ToList();
            var msgs = db.Messages.Where(x => x.Course != null && x.DOSonly == false).ToList();
            var workloads = db.Workloads.Where(w => w.Teacher.Id == TeacherId).Select(x => x.Course).ToList();
                        
            foreach (var message in msgs) {
                if (workloads.Where(c => c.Id == message.Course.Id).Any() && message.Workload==null) {
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
                Body = "The director of studies made a change to the details of " + course.Course.Name,
                Course = course,
                Workload = null,
                Seen=false,
                MessageType = MessageType.Notification
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
                Body = "The director of studies changed your time allocation for " + workload.Course.Course.Name + " " + workload.Course.TermYear.ToString(),
                Course= workload.Course,
                Workload = workload,
                Seen = false,
                MessageType = MessageType.Notification
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
                Body = "The director of studies has Removed you from" + workload.Course.Course.Name + " " + workload.Course.TermYear.ToString(),
                Course = workload.Course,
                Workload = workload,
                Seen = false,
                MessageType = MessageType.Notification
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        public static void GenerateTeacherMessageAddition(TeacherCourseWorkload workload, StaffingPlanContext db)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Datetime = DateTime.Now,
                Body = "The director of studies has Added you to the " + workload.Course.Course.Name +" " +workload.Course.TermYear.ToString() + " course",
                Course = workload.Course,
                Workload = workload,
                Seen = false,
                MessageType = MessageType.Notification
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
                Body = "You have a pending approval for " + workload.Course.Course.Name + " " + workload.Course.TermYear.ToString(),
                Course = workload.Course,
                Workload = workload,
                Seen = false
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        public static void GenerateDOSApprovedCourseMessage(CourseOffering course, bool forDOS, StaffingPlanContext db)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Datetime = DateTime.Now,
                Body = course.Course.Name + " for "+ course.TermYear.ToString() + " is now approved by all Teachers",
                Course = course,
                Workload = null,
                Seen = false,
                DOSonly = forDOS,
                MessageType = MessageType.CourseApproval
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        public static void GenerateDOSApprovedWorkloadMessage(TeacherCourseWorkload workload, bool forDOS, StaffingPlanContext db)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Datetime = DateTime.Now,
                Body = workload.Teacher.Name + " has approved workload for " + workload.Course.Course.Name + " " + workload.Course.TermYear.ToString(),
                Course = workload.Course,
                Workload = null,
                Seen = false,
                DOSonly= forDOS,
                MessageType = MessageType.WorkloadApproval
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

        #region helpers
        public static string getIcon(MessageType type) {
            switch (type) {
                case (MessageType.Notification):
                    return "fa fa-bell-o";
                    break;
                case (MessageType.CourseApproval):
                    return "fa fa-check";
                    break;
                case (MessageType.WorkloadApproval):
                    return "fa fa-bell-o";
                    break;
                case (MessageType.Request):
                    return "fa fa-handshake-o";
                    break;
                case (MessageType.Comment):
                    return "fa fa-commenting-o";
                    break;
                case (MessageType.Reminder):
                    return "fa fa-exclamation";
                    break;

            }
            return "fa fa-bell-o";
        }

        #endregion
    }
}