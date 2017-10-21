using System.Collections.Generic;
using System.Web.Mvc;
using StaffingPlanner.DAL;
using StaffingPlanner.ViewModels;
using StaffingPlanner.Models;
using System;
using System.Linq;

namespace StaffingPlanner.Controllers
{
    public class ApprovalsController : Controller
    {
        // GET: Approvals
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ApproveWorkload(string Id)
        {
            var db = StaffingPlanContext.GetContext();
            var work = db.Workloads.Where(m => m.Id.ToString() == Id).ToList().FirstOrDefault();
            work.IsApproved = true;
            db.SaveChanges();
            ApproveCourse(db, work.Course);
            MessagesController.GenerateDOSApprovedWorkloadMessage(work, true, db);

            return RedirectToAction("TeacherDetails", "Teacher", new { id = work.Teacher.Id });
        }


        public static void ApproveCourse(StaffingPlanContext db, CourseOffering course) {
            if (db.Workloads.Where(x => x.Course.Id == course.Id && x.IsApproved == false).Any() == false) {
                course.IsApproved = true;
                db.SaveChanges();
                MessagesController.GenerateDOSApprovedCourseMessage(course, true, db);
            }            
        }

        public static void Unapprove(StaffingPlanContext db, TeacherCourseWorkload workload)
        {
            workload.IsApproved = false;
            workload.Course.IsApproved = false;
            db.SaveChanges();
        }

    }
}