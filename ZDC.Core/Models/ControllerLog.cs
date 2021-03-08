using System;

namespace ZDC.Core.Models
{
    public class ControllerLog : BaseModel
    {
        public string Position { get; set; }
        public string Frequency { get; set; }
        public User User { get; set; }
        public DateTime Login { get; set; }
        public DateTime Logout { get; set; }
        public double Duration { get; set; }
    }
}