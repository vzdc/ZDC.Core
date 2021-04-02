using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    public class AnnouncementsController : Controller
    {
        private readonly ZdcContext _context;

        public AnnouncementsController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Announcement>>> GetAnnouncements()
        {
            return Ok(await _context.Announcements.ToListAsync());
        }

        [HttpGet("full")]
        public async Task<ActionResult<IList<Announcement>>> GetAnnouncementsFull()
        {
            return Ok(await _context.Announcements
                .Include(x => x.User)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Announcement>> GetAnnouncement(int id)
        {
            return Ok(await _context.Announcements.FindAsync(id));
        }

        [HttpGet("{id}/full")]
        public async Task<ActionResult<Announcement>> GetAnnouncementFull(int id)
        {
            return Ok(await _context.Announcements
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAnnouncement(int id, [FromBody] Announcement data)
        {
            var announcement = await _context.Announcements.FindAsync(id);

            if (announcement == null) return NotFound($"Announcement: {id} not found");

            _context.Entry(announcement).CurrentValues.SetValues(data);

            await _context.SaveChangesAsync();

            return Ok(announcement);
        }

        [HttpPost]
        public async Task<ActionResult> PostAnnouncement([FromBody] Announcement announcement)
        {
            if (!ModelState.IsValid) return BadRequest(announcement);

            await _context.Announcements.AddAsync(announcement);

            await _context.SaveChangesAsync();

            return Ok(announcement);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnnouncement(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null) return NotFound($"Announcement: {id} not found");

            _context.Announcements.Remove(announcement);

            await _context.SaveChangesAsync();

            return Ok(announcement);
        }
    }
}