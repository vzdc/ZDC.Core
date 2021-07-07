using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Core.Dtos;
using ZDC.Core.Extensions;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : Controller
    {
        private readonly ZdcContext _context;
        private readonly IMapper _mapper;

        public AnnouncementsController(ZdcContext content, IMapper mapper)
        {
            _context = content;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Announcement>>> GetAnnouncements()
        {
            var announcements = await _context.Announcements
                .Include(x => x.Submitter)
                .OrderBy(x => x.Updated).ToListAsync();
            if (!await User.IsStaff(_context))
                return Ok(_mapper.Map<IList<Announcement>, IList<AnnouncementDto>>(announcements));
            return Ok(announcements);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,EC")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Announcement>> GetAnnouncement(int id)
        {
            var announcement = await _context.Announcements
                .Include(x => x.Submitter)
                .FirstOrDefaultAsync(x => x.Id == id);
            return announcement != null ? Ok(announcement) : NotFound($"Announcement {id} not found");
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,EC")]
        [HttpPut]
        public async Task<ActionResult<Announcement>> PutAnnouncement([FromBody] Announcement data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid announcement");
            var announcement = await _context.Announcements.FindAsync(data.Id);
            if (announcement != null)
                return NotFound($"Announcement {data.Id} not found");
            data.Updated = DateTimeOffset.UtcNow;
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
            var user = await _context.Users.FindAsync(data.SubmitterId);
            if (user == null)
                return NotFound($"User {data.SubmitterId} not found");
            data.Submitter = user;
            await _context.Announcements.AddAsync(data);
            await _context.SaveChangesAsync();
            return Ok(data);
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