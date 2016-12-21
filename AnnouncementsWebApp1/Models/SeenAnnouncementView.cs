using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnnouncementsWebApp1.Models
{
    public class SeenAnnouncementView
    {
        public int Id { get; set; }

        public Announcement Announcement { get; set; }
        public SeenAnnouncement SeenAnnouncement { get; set; }
        public List <SeenAnnouncement> SeenAnnouncements { get; set; }

        [NotMapped]
        public int TotalSeen { get; set; }
        [NotMapped]
        public double PercentageSeen { get; set; }
    }
}