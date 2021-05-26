using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class StaffingRequest
    {
        [Key] public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Affiliation { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public StaffingRequestStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public enum StaffingRequestStatus
    {
        Pending,
        Accepted,
        Denied
    }
}