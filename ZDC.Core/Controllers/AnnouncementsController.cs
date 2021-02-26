using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Core.Models;

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
        public IEnumerable<Announcement> GetAnnouncements()
        {
            return _context.Announcements.ToList();
        }

        [HttpGet("full")]
        public IEnumerable<Announcement> GetAnnouncementsFull()
        {
            return _context.Announcements
                .Include(x => x.User)
                .ToList();
        }

        [HttpGet("{id}")]
        public Announcement GetAnnouncement(int id)
        {
            return _context.Announcements.Find(id);
        }

        [HttpGet("{id}/full")]
        public Announcement GetAnnouncementFull(int id)
        {
            return _context.Announcements
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAnnouncement(int id, [FromBody] JsonPatchDocument<Announcement> data)
        {
            var announcement = await _context.Announcements.FindAsync(id);

            if (announcement == null) return NotFound(id);

            data.ApplyTo(announcement);

            await _context.SaveChangesAsync();

            return Ok(announcement);
        }

        [HttpPut]
        public async Task<IActionResult> PutAnnouncement([FromBody] Announcement announcement)
        {
            if (!ModelState.IsValid) return BadRequest(announcement);

            await _context.Announcements.AddAsync(announcement);

            await _context.SaveChangesAsync();

            return Ok(announcement);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null) return NotFound(id);

            _context.Announcements.Remove(announcement);

            await _context.SaveChangesAsync();

            return Ok(announcement);
        }
    }
}