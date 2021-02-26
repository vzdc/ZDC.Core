using System;

namespace ZDC.Core.Models
{
    public class OnlineController : BaseModel
    {
        public string Position { get; set; }
        public string Frequency { get; set; }
        public User User { get; set; }
        public TimeSpan Online { get; set; }
    }
}