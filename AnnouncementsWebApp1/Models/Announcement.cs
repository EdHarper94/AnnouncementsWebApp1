using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnouncementsWebApp1.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public Boolean IsDeleted { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}