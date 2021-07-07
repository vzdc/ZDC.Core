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
    public class FeedbackController : Controller
    {
        private readonly ZdcContext _context;
        private readonly NotificationService _notificationService;

        public FeedbackController(ZdcContext content, NotificationService notificationService)
        {
            _context = content;
            _notificationService = notificationService;
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,ATA")]
        [HttpGet]
        public async Task<ActionResult<IList<Feedback>>> GetAllFeedback()
        {
            var feedback = await _context.Feedback
                .Include(x => x.User)
                .OrderBy(x => x.Created).ToListAsync();
            return Ok(feedback);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,ATA")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var feedback = await _context.Feedback
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            return feedback != null ? Ok(feedback) : NotFound($"Feedback {id} not found");
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,ATA")]
        [HttpPut]
        public async Task<ActionResult<Feedback>> PutFeedback([FromBody] Feedback data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid feedback");
            var feedback = await _context.Feedback.FindAsync(data.Id);
            if (feedback != null)
                return NotFound($"Feedback {data.Id} not found");
            data.Updated = DateTimeOffset.UtcNow;
            _context.Feedback.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize]
        [HttpPost]
        public async Task<ActionResult<Feedback>> PostFeedback([FromBody] Feedback data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid feedback");
            var controller = await _context.Users.FindAsync(data.UserId);
            if (controller == null)
                return NotFound($"User {data.UserId} not found");
            data.User = controller;
            await _context.Feedback.AddAsync(data);
            await _context.SaveChangesAsync();
            var roles = new List<string>
            {
                "ATM", "DATM", "TA",
                "WM", "ATA"
            };
            var link = $"/staff/feedback/{data.Id}";
            var title = "New Feedback";
            await _notificationService.PushNotification(roles, link, title);
            return Ok(data);
        }
    }
}