using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        public ActionResult<IList<Announcement>> GetAnnouncements(bool full = false)
        {
            if (full)
                return Ok(_context.Announcements
                    .Include(x => x.User).ToList());
            return Ok(_context.Announcements.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Announcement> GetAnnouncement(int id, bool full = false)
        {
            Announcement announcement;
            if (full)
            {
                announcement = _context.Announcements
                    .Include(x => x.User)
                    .FirstOrDefault(x => x.Id == id);
                if (announcement == null)
                    return NotFound($"Announcement: {id} not found");
                return Ok(announcement);
            }

            announcement = _context.Announcements.Find(id);
            if (announcement == null)
                return NotFound($"Announcement: {id} not found");
            return Ok(announcement);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAnnouncement(int id, [FromBody] Announcement data)
        {
            var announcement = await _context.Announcements.FindAsync(id);

            if (announcement == null) return NotFound($"Announcement: {id} not found");

            _context.Entry(announcement).CurrentValues.SetValues(data);

            await _context.SaveChangesAsync();

            return Ok(announcement);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> PostAnnouncement([FromBody] Announcement announcement)
        {
            if (!ModelState.IsValid) return BadRequest(announcement);

            await _context.Announcements.AddAsync(announcement);

            await _context.SaveChangesAsync();

            return Ok(announcement);
        }

        [Authorize]
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