using System;

namespace ZDC.Core.Models
{
    public class ControllerLog : BaseModel
    {
        public string Position { get; set; }
        public DateTime Login { get; set; }
        public DateTime Logout { get; set; }
        public TimeSpan Duration { get; set; }
        public double Hours => Duration.TotalHours;
    }
}