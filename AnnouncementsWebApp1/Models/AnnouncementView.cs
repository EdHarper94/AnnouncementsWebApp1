using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnouncementsWebApp1.Models
{
    public class AnnouncementView
    {
        public Announcement Announcement { get; set; }
        public Comment Comment { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}