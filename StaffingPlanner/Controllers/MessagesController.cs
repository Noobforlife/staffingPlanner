using System.Collections.Generic;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;
using StaffingPlanner.Models;
using System;

namespace StaffingPlanner.Controllers
{
    public class MessagesController : Controller
    {
        #region View Methods
        // GET: Message
        public ActionResult Messages()
        {
            //Removed guid from parameters as it was causing crash when clicking messages icon
            //Bring it back when that is fixed, or perhaps we could use Globals.UserId instead?
			if (Globals.UserRole != Role.DirectorOfStudies)
	        {
		        return RedirectToAction("Index", "Dashboard");
			}

			var model = new List<MessageViewModel>();

			// TODO: Generate view model

            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult RenderNotificationWindow(Guid TeacherId)
        {
            return PartialView("~/Views/Course/_NotificationsWindow.cshtml");
        }

        #endregion

        #region methods to Generate messages
        public static void GenerateTeacherMessage(CourseOffering course, StaffingPlanContext db)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Body = "The director of studies made a change to " + course.Course.Name,
                Course = course,
                Workload = null
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        public static void GenerateTeacherMessage(TeacherCourseWorkload workload, StaffingPlanContext db )
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Body = "The director of studies changed your time allocation for " + workload.Course.Course.Name,
                Course= workload.Course,
                Workload = workload
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        public static void GenerateTeacherPendingMessage(TeacherCourseWorkload workload, StaffingPlanContext db)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Body = "You have a pending approval for " + workload.Course.Course.Name,
                Course = workload.Course,
                Workload = workload
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        public static void GenerateDOSApprovedMessage(CourseOffering course, bool forDOS, StaffingPlanContext db)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Body = course.Course.Name + " for "+ course.TermYear.ToString() + " is now approved.",
                Course = course,
                Workload = null
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        public static void GenerateDOSPendingMessage(CourseOffering course, bool forDOS, StaffingPlanContext db)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                Body = course.Course.Name + " for " + course.TermYear.ToString() + " is still Pending Approval.",
                Course = course,
                Workload = null
            };
            db.Messages.Add(msg);
            db.SaveChanges();
        }

        #endregion 
    }
}