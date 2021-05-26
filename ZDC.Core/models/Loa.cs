using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class Loa
    {
        public Loa()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public string Reason { get; set; }
        public string MoreInfo { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public LoaStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public enum LoaStatus
    {
        Pending,
        MoreInfo,
        Accepted,
        Started,
        Canceled,
        Ended,
        Controlled
    }
}