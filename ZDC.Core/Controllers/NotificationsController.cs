using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Core.Extensions;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : Controller
    {
        private readonly ZdcContext _context;

        public NotificationsController(ZdcContext content)
        {
            _context = content;
        }

        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IList<Notification>>> GetNotifications()
        {
            var user = await User.GetUser(_context);
            var notifications = await _context.Notifications
                .Where(x => x.Status == NotificationStatus.Open)
                .Where(x => x.User == user)
                .ToListAsync();
            return Ok(notifications);
        }
    }
}