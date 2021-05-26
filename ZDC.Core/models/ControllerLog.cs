using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class ControllerLog
    {
        public ControllerLog()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public string Position { get; set; }
        public string Frequency { get; set; }
        public virtual User User { get; set; }
        public DateTime Login { get; set; }
        public DateTime Logout { get; set; }
        public double Duration { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}