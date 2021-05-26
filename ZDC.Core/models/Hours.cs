using System;
using System.ComponentModel.DataAnnotations;

namespace ZDC.Core.Models
{
    public class Hours
    {
        public Hours()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        [Key] public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double LocalHours { get; set; }
        public double TraconHours { get; set; }
        public double CenterHours { get; set; }
        public double TotalHours => LocalHours + TraconHours + CenterHours;

        public string LocalHoursString
        {
            get
            {
                var time = TimeSpan.FromHours(LocalHours);
                return $"{time.Hours}h {time.Minutes}m";
            }
        }

        public string TraconHoursString
        {
            get
            {
                var time = TimeSpan.FromHours(TraconHours);
                return $"{time.Hours}h {time.Minutes}m";
            }
        }

        public string CenterHoursString
        {
            get
            {
                var time = TimeSpan.FromHours(CenterHours);
                return $"{time.Hours}h {time.Minutes}m";
            }
        }

        public string TotalHoursString
        {
            get
            {
                var time = TimeSpan.FromHours(TotalHours);
                return $"{time.Hours}h {time.Minutes}m";
            }
        }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}