using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class Feedback
    {
        public Feedback()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string Callsign { get; set; }
        public string Email { get; set; }
        public virtual User User { get; set; }
        public string Facility { get; set; }
        public ServiceLevel Service { get; set; }
        public string Description { get; set; }
        public FeedbackStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public enum ServiceLevel
    {
        Unsatisfactory,
        Poor,
        Fair,
        Good,
        Excellent
    }

    public enum FeedbackStatus
    {
        Pending,
        Contacted,
        Accepted,
        Denied
    }
}