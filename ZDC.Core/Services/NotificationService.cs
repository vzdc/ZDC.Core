using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Services
{
    public class NotificationService
    {
        private readonly ZdcContext _context;

        public NotificationService(ZdcContext context)
        {
            _context = context;
        }

        public async Task PushNotification(IList<string> roles, string link, string title)
        {
            var users = await _context.Users
                .Where(x => x.Roles.Any(x => roles.Contains(x.Name)))
                .ToListAsync();
            foreach (var user in users)
                await _context.Notifications.AddAsync(new Notification
                {
                    User = user,
                    Link = link,
                    Title = title
                });

            await _context.SaveChangesAsync();
        }
    }
}