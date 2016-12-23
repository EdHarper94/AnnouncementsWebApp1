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
    public class AnnouncementsController : Controller
    {
        /**
         * AnnouncementController.cs
         * Handles announcement model operations
         * @see Views/Announcements for associated views
         **/
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns view based on user role
        /// </summary>
        /// <returns>index view</returns>
        [Authorize(Roles = "Lecturer, Student")]
        public ActionResult Index()
        {
            if (User.IsInRole("Lecturer"))
            {
                return View();
            }
            else
            {
                return View("IndexStudent");
            }
        }

        
        /// <summary>
        /// Gets list of announcements depending on role.
        /// </summary>
        /// <returns>list of announcements</returns>
        private IEnumerable<Announcement> GetAnnouncements()
        {
            if (User.IsInRole("Student"))
            {
                return db.Announcements.ToList().Where(x => x.IsPublic == true);
            }
            else
            {
                return db.Announcements.ToList();
            }
        }

        /// <summary>
        /// Builds Announcements Table. Returning different partial view depending on role.
        /// </summary>
        /// <returns> Announcements Table Partial View (student/lecturer)</returns>
        [Authorize(Roles = "Lecturer, Student")]
        public ActionResult BuildAnnouncementsTable()
        {
            if (User.IsInRole("Lecturer"))
            {
                return PartialView("_AnnouncementsTable", GetAnnouncements());
            }
            else
            {
                return PartialView("_StudentAnnouncementsTable", GetAnnouncements());
            }
        }

        /// <summary>
        /// Returns AnnouncementView containing announcements and comments
        /// </summary>
        /// <param name="id">id of announcements</param>
        /// <returns>AnnouncementView</returns>
        [Authorize(Roles = "Lecturer, Student")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            // Check if student then announcement is public
            if (User.IsInRole("Student") && announcement.IsPublic != true)
            {
                return View("NotAuthorised");
            }
            else
            {
                CommentsController cc = new CommentsController();
                AnnouncementView av = new AnnouncementView();
                av.Announcement = announcement;
                cc.GetComments(id);

                av = cc.GetComments(id);
                return View(av);
            }
        }

        /// <summary>
        /// Ajax Create Announcement
        /// </summary>
        /// <param name="announcement">Announcement form</param>
        /// <returns>Announcement table partial view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lecturer")]
        public ActionResult AJAXCreate([Bind(Include = "Description,Title,Text, IsPublic")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(
                    x => x.Id == currentUserId
                    );
                announcement.User = currentUser;
                announcement.CreatedDate = DateTime.Now;
                db.Announcements.Add(announcement);
                db.SaveChanges();
            }

            return PartialView("_AnnouncementsTable", GetAnnouncements());
        }


        /// <summary>
        /// Edit Announcement
        /// </summary>
        /// <param name="id">Announcement of announcement to edit</param>
        /// <returns></returns>
        [Authorize(Roles = "Lecturer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        /// <summary>
        /// Edit announcement Method
        /// </summary>
        /// <param name="announcement">id of Announcement  to Edit</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lecturer")]
        public ActionResult Edit([Bind(Include = "Id,CreatedDate,Title,Description,Text,IsPublic")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(announcement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(announcement);
        }

        /// <summary>
        /// Ajax Edit for Checkboxes
        /// </summary>
        /// <param name="id">Checkbox id</param>
        /// <param name="value">Value of checkbox</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Lecturer")]
        public ActionResult AJAXEdit(int? id, bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            else
            {
                announcement.IsPublic = value;
                db.Entry(announcement).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_AnnouncementsTable", GetAnnouncements());
            }
        }

        /// <summary>
        /// POST
        /// Get Announcement for delete
        /// </summary>
        /// <param name="id">id of announcement to delete</param>
        /// <returns></returns>
        [Authorize(Roles = "Lecturer")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        /// <summary>
        /// POST 
        /// Delete announcement from db
        /// </summary>
        /// <param name="id">Announcement to delete</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lecturer")]
        public ActionResult DeleteConfirmed(int id)
        {
            Announcement announcement = db.Announcements.Find(id);
            db.Announcements.Remove(announcement);
            db.SaveChanges();
            return RedirectToAction("Index");
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
