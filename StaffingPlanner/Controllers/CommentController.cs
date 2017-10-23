using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaffingPlanner.Models;
using StaffingPlanner.DAL;

namespace StaffingPlanner.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment/Comments/{connectedId}
        [ChildActionOnly]
        public PartialViewResult Comments(Guid connectedId)
        {
            var db = StaffingPlanContext.GetContext();
            var comments = db.Comments.Where(c => c.ConnectedTo == connectedId).ToList();
            if (comments.Any())
            {
                return PartialView("~/Views/Comment/_Comments.cshtml", comments);
            }
            else
            {
                return PartialView("~/Views/Comment/_Comments.cshtml", new List<Comment>());
            }
        }

        [ChildActionOnly]
        public PartialViewResult RenderAddComment(Guid connectedId)
        {
            Comment newComment = new Comment
            {
                Id = Guid.NewGuid(),
                ConnectedTo = connectedId,
                Message = ""
            };
            return PartialView("~/Views/Comment/_AddComment.cshtml", newComment);
        }

        // POST: Comment/Create
        [HttpPost]
        public ActionResult Create(Guid connectedId, string message)
        {
            var db = StaffingPlanContext.GetContext();
            var newComment = new Comment
            {
                Id = Guid.NewGuid(),
                ConnectedTo = connectedId,
                Message = message
            };
            db.Comments.Add(newComment);
            db.SaveChanges();
            return RedirectToAction("TeacherDetails", "Teacher", new { id = connectedId });
        }

        // Post: Comment/Edit/{commentId, message}
        [HttpPost]
        public JsonResult Edit(Guid commentId, string newMessage)
        {
            var db = StaffingPlanContext.GetContext();
            var comment = db.Comments.FirstOrDefault(c => c.Id == commentId);
	        if (comment != null)
	        {
		        comment.Message = newMessage;
		        db.SaveChanges();
		        return Json(new { message = "Success" });
			}

	        return Json(new {message = "No comment with that Id"});
        }

        // POST: Comment/Delete/{commentId}
        [HttpPost]
        public JsonResult Delete(Guid commentId)
        {
            var db = StaffingPlanContext.GetContext();
            var comment = db.Comments.FirstOrDefault(c => c.Id == commentId);
	        if (comment != null)
	        {
		        db.Comments.Remove(comment);
		        db.SaveChanges();
		        return Json(new { message = "Success" });
			}

	        return Json(new {message = "No comment with that Id"});
        }
    }
}
