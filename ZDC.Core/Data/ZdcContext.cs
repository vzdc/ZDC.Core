using Microsoft.EntityFrameworkCore;
using ZDC.Core.Models;

namespace ZDC.Core.Data
{
    public class ZdcContext : DbContext
    {
        public ZdcContext(DbContextOptions<ZdcContext> options) : base(options)
        {
        }

        public DbSet<Airport> Airports { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<BugReport> BugReports { get; set; }
        public DbSet<ControllerLog> ControllerLogs { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<LastUpdated> LastUpdated { get; set; }
        public DbSet<Metar> Metar { get; set; }
        public DbSet<OnlineController> OnlineControllers { get; set; }
        public DbSet<Overflight> Overflights { get; set; }
        public DbSet<StaffingRequest> StaffingRequests { get; set; }
        public DbSet<TrainingTicket> TrainingTickets { get; set; }
        public DbSet<User> Users { get; set; }
    }
}