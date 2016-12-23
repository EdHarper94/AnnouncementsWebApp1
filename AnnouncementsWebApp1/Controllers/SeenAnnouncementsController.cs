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
using Microsoft.AspNet.Identity.EntityFramework;

namespace AnnouncementsWebApp1.Controllers
{
    public class SeenAnnouncementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        //GET SeenAnnouncements
        public SeenAnnouncementView GetSeenAnnouncements(int? id)
        {
            Announcement announcement = db.Announcements.Find(id);
            List<SeenAnnouncement> seenAnns = db.SeenAnnouncements.ToList();
            SeenAnnouncementView sv = new SeenAnnouncementView();
            sv.SeenAnnouncements = new List<SeenAnnouncement>();
            sv.Announcement = announcement;

            sv.TotalSeen = 0;
            foreach (SeenAnnouncement sa in seenAnns)
            {
                if (sa.AnnouncementId == id)
                {
                    sv.TotalSeen++;
                    sv.SeenAnnouncements.Add(sa);
                }
            }
            IdentityRole myRole = db.Roles.First(r => r.Name == "Student");
            sv.TotalUsers = db.Set<IdentityUserRole>().Count(r => r.RoleId == myRole.Id);
            List<ApplicationUser> us = db.Users.ToList();

            sv.PercentageSeen = Math.Round(100f * ((float)sv.TotalSeen / (float)sv.TotalUsers));
            return sv;
        }

        // GET: SeenAnnouncements/Details/5
        [Authorize(Roles = "Lecturer")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            SeenAnnouncementView sv = GetSeenAnnouncements(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(sv);
        }

        /// <summary>
        /// Adds user and announcementID to SeenAnnouncements table
        /// </summary>
        /// <param name="seenAnnouncement">Seen Announcement for user</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public void MarkSeenAJAX([Bind(Include = "Id,AnnouncementId")] SeenAnnouncement seenAnnouncement)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(
                    x => x.Id == currentUserId
                    );
                // Check to see if user is in DB for current announcement
                bool checkuser = db.SeenAnnouncements.Any(x => x.User.Id.Equals(currentUser.Id) && x.AnnouncementId == seenAnnouncement.AnnouncementId);
                if (!checkuser)
                {

                    seenAnnouncement.User = currentUser;
                    db.SeenAnnouncements.Add(seenAnnouncement);
                    db.SaveChanges();
                }
                
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
