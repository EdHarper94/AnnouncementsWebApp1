using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnnouncementsWebApp1.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string Text { get; set; }

        public int AnnouncementId { get; set; }
        public virtual Announcement Announcement { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}