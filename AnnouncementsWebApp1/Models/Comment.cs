using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnouncementsWebApp1.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }

        public int AnnouncementId { get; set; }
        public virtual Announcement Announcement { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}