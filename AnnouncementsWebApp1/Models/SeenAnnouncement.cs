using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnouncementsWebApp1.Models
{
    public class SeenAnnouncement
    {
        public int Id { get; set; }

        public int AnnouncementId { get; set; }
        public virtual Announcement Announcement { get; set; }

        public virtual ApplicationUser User { get; set; }

    }
}