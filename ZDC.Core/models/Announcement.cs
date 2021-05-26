using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class Announcement
    {
        public Announcement()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public virtual User User { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}