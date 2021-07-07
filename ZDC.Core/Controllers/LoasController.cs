using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Core.Services;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoasController : Controller
    {
        private readonly ZdcContext _context;
        private readonly NotificationService _notificationService;

        public LoasController(ZdcContext content, NotificationService notificationService)
        {
            _context = content;
            _notificationService = notificationService;
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,FE,ATA,AWM,AEC,AFE")]
        [HttpGet]
        public async Task<ActionResult<IList<Loa>>> GetLoas()
        {
            var loas = await _context.Loas
                .Include(x => x.User)
                .OrderBy(x => x.Created).ToListAsync();
            return Ok(loas);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,FE,ATA,AWM,AEC,AFE")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Loa>> GetLoa(int id)
        {
            var loa = await _context.Loas
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            return loa != null ? Ok(loa) : NotFound($"Loa {id} not found");
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,FE,ATA,AWM,AEC,AFE")]
        [HttpPut]
        public async Task<ActionResult<Loa>> PutLoa([FromBody] Loa data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid LOA");
            var loa = await _context.Loas.FindAsync(data.Id);
            if (loa != null)
                return NotFound($"LOA {data.Id} not found");
            data.Updated = DateTimeOffset.UtcNow;
            _context.Loas.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,FE,ATA,AWM,AEC,AFE")]
        [HttpPost]
        public async Task<ActionResult<Loa>> PostLoa([FromBody] Loa data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid LOA");
            var user = await _context.Users.FindAsync(data.UserId);
            if (user == null)
                return NotFound($"User {data.UserId} not found");
            data.User = user;
            await _context.Loas.AddAsync(data);
            await _context.SaveChangesAsync();
            var roles = new List<string>
            {
                "ATM", "DATM", "TA",
                "WM"
            };
            var link = $"/staff/loa/{data.Id}";
            var title = "New LOA";
            await _notificationService.PushNotification(roles, link, title);
            return Ok(data);
        }
    }
}