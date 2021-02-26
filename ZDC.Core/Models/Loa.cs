using System;

namespace ZDC.Core.Models
{
    public class Loa : BaseModel
    {
        public string Reason { get; set; }
        public string MoreInfo { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public LoaStatus Status { get; set; }
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