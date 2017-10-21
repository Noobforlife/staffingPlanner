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

        // POST: Comment/Create
        [HttpPost]
        public void Create(Guid connectedId, string message)
        {
            var db = StaffingPlanContext.GetContext();
            var newComment = new Comment
            {
                Id = Guid.NewGuid(),
                ConnectedTo = connectedId,
                Message = message
            };
            db.Comments.Add(newComment);
        }

        // Post: Comment/Edit/{commentId, message}
        [HttpPost]
        public void Edit(Guid commentId, string newMessage)
        {
            try
            {
                var db = StaffingPlanContext.GetContext();
                var comment = db.Comments.FirstOrDefault(c => c.Id == commentId);
                comment.Message = newMessage;
                db.SaveChanges();
            }
            catch
            {
            }
        }

        // POST: Comment/Delete/{commentId}
        [HttpPost]
        public void Delete(Guid commentId)
        {
            try
            {
                var db = StaffingPlanContext.GetContext();
                var comment = db.Comments.FirstOrDefault(c => c.Id == commentId);
                db.Comments.Remove(comment);
            }
            catch
            {
            }
        }
    }
}
