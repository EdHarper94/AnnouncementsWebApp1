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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Announcements
        public ActionResult Index()
        {
            return View();
        }

        // Gets Announcement List
        private IEnumerable<Announcement> GetAnnouncements()
        {
            return db.Announcements.ToList();
        }

        /// <summary>
        /// Builds Announcements Table 
        /// </summary>
        /// <returns> Announcements Table Partial View</returns>
        public ActionResult BuildAnnouncementsTable()
        {
            return PartialView("_AnnouncementsTable", GetAnnouncements());
        }

        // GET: Announcements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            List<Comment> comments = db.Comments.ToList();
            AnnouncementView av = new AnnouncementView();
            av.Comments = new List<Comment>();

            av.Announcement = announcement;
            foreach (Comment c in comments){
                if (c.AnnouncementId == id)
                {
                    av.Comments.Add(c);
                }
            }

            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(av);
        }

        // GET: Announcements/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CreatedDate,Title,Description,Text,IsDeleted")] Announcement announcement)
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
                return RedirectToAction("Index");
            }

            return PartialView(announcement);
        }

        /// <summary>
        /// Ajax Create Announcement
        /// </summary>
        /// <param name="announcement"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "Id,Description,Title,Text")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(
                    x => x.Id == currentUserId
                    );
                announcement.User = currentUser;
                announcement.CreatedDate = DateTime.Now;
                announcement.IsDeleted = false;
                db.Announcements.Add(announcement);
                db.SaveChanges();
            }

            return PartialView("_AnnouncementsTable", GetAnnouncements());
        }


        // GET: Announcements/Edit/5
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

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreatedDate,Title,Description,Text,IsDeleted")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(announcement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(announcement);
        }

        [HttpPost]
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
                announcement.IsDeleted = value;
                db.Entry(announcement).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_AnnouncementsTable", GetAnnouncements());
            }
        }

        // GET: Announcements/Delete/5
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

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
