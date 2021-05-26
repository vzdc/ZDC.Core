using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class Warning
    {
        public Warning()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public WarningReason Reason { get; set; }
        public WarningStatus Status { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public enum WarningReason
    {
        Activity,
        Grp
    }

    public enum WarningStatus
    {
        Warned,
        GotFirstHalf,
        Resolved,
        NeedsRemoval
    }
}