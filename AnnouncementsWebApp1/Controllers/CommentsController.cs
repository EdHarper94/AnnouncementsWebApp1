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
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Index View
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Announcement);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
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
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.AnnouncementId = new SelectList(db.Announcements, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Text,AnnouncementId")] Comment comment)
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
                return RedirectToAction("Index");
            }

            ViewBag.AnnouncementId = new SelectList(db.Announcements, "Id", "Title", comment.AnnouncementId);
            return View(comment);
        }

        /// <summary>
        /// Get comments for an Announcement
        /// </summary>
        /// <param name="id">The comments' Announcement Id</param>
        /// <returns>AnnouncementViewModel</returns>
        public AnnouncementView GetComments(int? id)
        {
            Announcement announcement = db.Announcements.Find(id);
            List<Comment> comments = db.Comments.ToList();
            AnnouncementView av = new AnnouncementView();
            av.Comments = new List<Comment>();
            av.Announcement = announcement;
            foreach (Comment c in comments)
            {
                if (c.AnnouncementId == id)
                {
                    av.Comments.Add(c);
                }
            }
            return av;
        }

        /// <summary>
        /// Ajax Comment creation method.
        /// </summary>
        /// <param name="comment"> Prefix used to be compatible with AnnouncementsViewModel</param>
        /// <returns>_CommentsTable partial view with comments for a perticular announcment</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCommentCreate([Bind(Prefix="Comment",Include = "Id,Text,AnnouncementId")] Comment comment)
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

            //ViewBag.AnnouncementId = new SelectList(db.Announcements, "Id", "Title", comment.AnnouncementId);
            return PartialView("_CommentsTable", GetComments(comment.AnnouncementId));
        }


        /// <summary>
        /// Ajax Create Announcement
        /// </summary>
        /// <param name="announcement"></param>
        /// <returns></returns>

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.AnnouncementId = new SelectList(db.Announcements, "Id", "Title", comment.AnnouncementId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Text,AnnouncementId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AnnouncementId = new SelectList(db.Announcements, "Id", "Title", comment.AnnouncementId);
            return View(comment);
        }

        // GET: Comments/Delete/5
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
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            int redirectId = comment.AnnouncementId;
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "Announcements", new { id = redirectId });
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
