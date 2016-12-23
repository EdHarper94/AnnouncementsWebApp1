using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AnnouncementsWebApp1.Models;
using Microsoft.AspNet.Identity;

namespace AnnouncementsWebApp1.Controllers
{
    /**
        * CommentsController.cs 
        * Handles Comment Model operations.
        * @see Views/Comments for assoiated views.
        * */
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Validates current user owns the passed comment
        /// </summary>
        /// <param name="comment"> passed comment to be validated</param>
        /// <returns> true/false depending on validation result</returns>
        public bool ValidateOwnership(Comment comment)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(
                x => x.Id == currentUserId
                );
            System.Diagnostics.Debug.WriteLine(comment.User);
            System.Diagnostics.Debug.WriteLine(currentUser);
            if (comment.User == currentUser)
            {

                return true;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get an idividual comments details and passes it to view
        /// </summary>
        /// <param name="id">the comment details to fetch</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            // Validate comment ownership
            if ((ValidateOwnership(comment)))
            {
                return View(comment);
            }
            else
            {
                return View("NotAuthorised");
            }
        }

        /// <summary>
        /// Get all comments for an announcement
        /// </summary>
        /// <param name="id">The id of the Announcement</param>
        /// <returns>See AnnouncementViewModel</returns>
        public AnnouncementView GetComments(int? id)
        {
            Announcement announcement = db.Announcements.Find(id);
            AnnouncementView av = new AnnouncementView();
            av.Comments = db.Comments.ToList().Where(x=> x.AnnouncementId == announcement.Id);
            av.Announcement = announcement;
            return av;
        }

        /// <summary>
        /// Ajax Comment creation.
        /// </summary>
        /// <param name="comment"> Prefix used to be compatible with AnnouncementsViewModel</param>
        /// <returns>_CommentsTable - partialview using AnnounceViewModel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCommentCreate([Bind(Prefix="Comment",Include = "Text,AnnouncementId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(
                    x => x.Id == currentUserId
                    );
                comment.User = currentUser;
                comment.Date = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
            }

            return PartialView("_CommentsTable", GetComments(comment.AnnouncementId));
        }

        /// <summary>
        /// gets comment to edit
        /// </summary>
        /// <param name="id"> the comment to edit</param>
        /// <returns>the view to edit the comment</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            int redirectId = comment.AnnouncementId;
            if (comment == null)
            {
                return HttpNotFound();
            }
            // Comment ownership validation
            if ((ValidateOwnership(comment)))
            {
                return View(comment);
            }
            else
            {
                return View("NotAuthorised");
            }
        }

        /// <summary>
        /// Edit comment in db.
        /// </summary>
        /// <param name="comment">the comment sent from view</param>
        /// <returns>redirects back to previous announcements details</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Text,AnnouncementId")] Comment comment)
        {

            int redirectId = comment.AnnouncementId;
            if (ModelState.IsValid) { 
                
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Announcements", new { id = redirectId });
            }
            else
            {
                return RedirectToAction("Details", "Announcements", new { id = redirectId });
            }
        }

        /// <summary>
        /// Gets the comment to delete
        /// </summary>
        /// <param name="id">id of comment to delete</param>
        /// <returns>redirects back to previous announcements details page</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);

            if (comment == null)
            {
                return HttpNotFound();
            }
            int redirectId = comment.AnnouncementId;
            // Ownership check
            if ((ValidateOwnership(comment))|| (User.IsInRole("Lecturer")))
            {
                return View(comment);
            }
            else
            {
                return RedirectToAction("Details", "Announcements", new { id = redirectId });
            }
        }

        /// <summary>
        /// Peforms delete in db.
        /// </summary>
        /// <param name="id">id of the comment to delete</param>
        /// <returns>redirects back to previous announcements details page<</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            int redirectId = comment.AnnouncementId;
            if ((ValidateOwnership(comment)) || (User.IsInRole("Lecturer")))
            {
                
                db.Comments.Remove(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "Announcements", new { id = redirectId });
            }
            else
            {
                return RedirectToAction("Details", "Announcements", new { id = redirectId });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
