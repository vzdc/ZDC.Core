using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : Controller
    {
        private readonly ZdcContext _context;

        public AnnouncementsController(ZdcContext content)
        {
            _context = content;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Announcement>>> GetAnnouncements()
        {
            var announcements = await _context.Announcements.ToListAsync();
            return Ok(announcements);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Announcement>> GetAnnouncement(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            return announcement != null ? Ok(announcement) : NotFound($"Announcement {id} not found");
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,EC")]
        [HttpPut]
        public async Task<ActionResult<Announcement>> PutAnnouncement([FromBody] Announcement data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid announcement");
            _context.Announcements.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,EC")]
        [HttpPost]
        public async Task<ActionResult<Announcement>> PostAnnouncement([FromBody] Announcement data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid announcement");
            await _context.Announcements.AddAsync(data);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(data.SubmitterId);
            var announcement = await _context.Announcements.FindAsync(data.Id);
            announcement.Submitter = user;

            await _context.SaveChangesAsync();
            return Ok(announcement);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,EC")]
        [HttpDelete]
        public async Task<ActionResult<Announcement>> DeleteAnnouncement(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
                return NotFound($"Announcement {id} not found");
            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return Ok(announcement);
        }
    }
}