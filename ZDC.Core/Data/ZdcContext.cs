using Microsoft.EntityFrameworkCore;
using ZDC.Core.Models;

namespace ZDC.Core.Data
{
    public class ZdcContext : DbContext
    {
        public ZdcContext(DbContextOptions<ZdcContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}