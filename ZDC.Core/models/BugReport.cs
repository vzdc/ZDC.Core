using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class BugReport
    {
        [Key] public int Id { get; set; }

        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public BugReportStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public enum BugReportStatus
    {
        Pending,
        UnderInvestigation,
        InProgress,
        Resolved
    }
}