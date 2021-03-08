namespace ZDC.Core.Models
{
    public class Hours : BaseModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int UserId { get; set; }
        public double LocalHours { get; set; }
        public double TraconHours { get; set; }
        public double CenterHours { get; set; }
        public double TotalHours => LocalHours + TraconHours + CenterHours;
    }
}