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
            MessagesController.GenerateDOSApprovedCourseMessage(work, true, db);

            return RedirectToAction("TeacherDetails", "Teacher", new { id = work.Teacher.Id });
        }

    }
}