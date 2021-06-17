using Microsoft.EntityFrameworkCore;
using ZDC.Models;

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
        public DbSet<Dossier> Dossier { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }
        public DbSet<EventPositionPreset> EventPositionPresets { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Hours> Hours { get; set; }
        public DbSet<Loa> Loas { get; set; }
        public DbSet<OnlineController> OnlineControllers { get; set; }
        public DbSet<Ots> Ots { get; set; }
        public DbSet<Overflight> Overflights { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<StaffingRequest> StaffingRequests { get; set; }
        public DbSet<TrainingTicket> TrainingTickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Warning> Warnings { get; set; }
    }
}