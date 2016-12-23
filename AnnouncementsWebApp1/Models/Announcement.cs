using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnnouncementsWebApp1.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Text { get; set; }
        public Boolean IsPublic { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}