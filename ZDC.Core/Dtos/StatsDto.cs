using ZDC.Models;

namespace ZDC.Core.Dtos
{
    public class StatsDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Initials { get; set; }
        public UserRating UserRating { get; set; }
        public string RatingLong { get; set; }
        public UserStatus Status { get; set; }
        public HoursDto Hours { get; set; }
    }

    public class HoursDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public double LocalHours { get; set; }
        public double TraconHours { get; set; }
        public double CenterHours { get; set; }
        public double TotalHours { get; set; }
        public string LocalHoursString { get; set; }
        public string TraconHoursString { get; set; }
        public string CenterHoursString { get; set; }
        public string TotalHoursString { get; set; }
    }
}