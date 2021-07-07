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
    public class OtsController : Controller
    {
        private readonly ZdcContext _context;
        private readonly NotificationService _notificationService;

        public OtsController(ZdcContext content, NotificationService notificationService)
        {
            _context = content;
            _notificationService = notificationService;
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,EC,ATA,INS")]
        [HttpGet]
        public async Task<ActionResult<IList<Ots>>> GetAllOts()
        {
            var ots = await _context.Ots.OrderBy(x => x.Created)
                .Include(x => x.User)
                .Include(x => x.RecommendedBy)
                .Include(x => x.Instructor)
                .ToListAsync();
            return Ok(ots);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,EC,ATA,INS")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Ots>> GetOts(int id)
        {
            var ots = await _context.Ots.OrderBy(x => x.Created)
                .Include(x => x.User)
                .Include(x => x.RecommendedBy)
                .Include(x => x.Instructor)
                .FirstOrDefaultAsync(x => x.Id == id);
            return ots != null ? Ok(ots) : NotFound($"OTS {id} not found");
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,EC,ATA,INS")]
        [HttpPut]
        public async Task<ActionResult<Ots>> PutOts([FromBody] Ots data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid OTS");
            var ots = await _context.Ots.FindAsync(data.Id);
            if (ots != null)
                return NotFound($"OTS {data.Id} not found");
            var recommendedBy = await _context.Users.FindAsync(data.RecommendedById);
            if (recommendedBy == null)
                return NotFound($"Recommender {data.RecommendedById} not found");
            data.RecommendedBy = recommendedBy;
            data.Updated = DateTimeOffset.UtcNow;
            _context.Ots.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,EC,ATA,INS")]
        [HttpPost]
        public async Task<ActionResult<Ots>> PostOts([FromBody] Ots data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid OTS");
            var user = await _context.Users.FindAsync(data.UserId);
            if (user == null)
                return NotFound($"User {data.UserId} not found");
            data.User = user;
            var recommendedBy = await _context.Users.FindAsync(data.RecommendedById);
            if (recommendedBy == null)
                return NotFound($"Recommender {data.RecommendedById} not found");
            data.RecommendedBy = recommendedBy;
            var instructor = await _context.Users.FindAsync(data.InstructorId);
            if (instructor == null)
                return NotFound($"Instructor {data.InstructorId} not found");
            data.Instructor = instructor;
            await _context.AddAsync(data);
            await _context.SaveChangesAsync();
            var roles = new List<string>
            {
                "ATM", "DATM", "TA",
                "WM", "ATA"
            };
            var link = $"/training/ots/{data.Id}";
            var title = "New OTS";
            await _notificationService.PushNotification(roles, link, title);
            return Ok(data);
        }
    }
}