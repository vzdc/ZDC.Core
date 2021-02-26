namespace ZDC.Core.Models
{
    public class Warning : BaseModel
    {
        public WarningReason Reason { get; set; }
        public WarningStatus Status { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
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