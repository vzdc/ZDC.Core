using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class OnlineController
    {
        public OnlineController()
        {
            Created = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public string Position { get; set; }
        public string Frequency { get; set; }
        public virtual User User { get; set; }
        public string Online { get; set; }
        public DateTime Created { get; set; }
    }
}