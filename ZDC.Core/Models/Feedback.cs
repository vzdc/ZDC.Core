namespace ZDC.Core.Models
{
    public class Feedback : BaseModel
    {
        public string Name { get; set; }
        public string Callsign { get; set; }
        public string Email { get; set; }
        public User User { get; set; }
        public string Facility { get; set; }
        public ServiceLevel Service { get; set; }
        public string Description { get; set; }
        public FeedbackStatus Status { get; set; }
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