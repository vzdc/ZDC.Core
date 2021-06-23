using System;
using System.Collections.Generic;
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
    public class StaffingRequestController : Controller
    {
        private readonly ZdcContext _context;
        private readonly NotificationService _notificationService;

        public StaffingRequestController(ZdcContext content, NotificationService notificationService)
        {
            _context = content;
            _notificationService = notificationService;
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,AEC")]
        [HttpGet]
        public async Task<ActionResult<IList<StaffingRequest>>> GetStaffingRequests()
        {
            var requests = await _context.StaffingRequests.ToListAsync();
            return Ok(requests);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,AEC")]
        [HttpGet("{id}")]
        public async Task<ActionResult<StaffingRequest>> GetStaffingRequest(int id)
        {
            var request = await _context.StaffingRequests.FindAsync(id);
            return request != null ? Ok(request) : NotFound($"Staffing request {id} not found");
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,AEC")]
        [HttpPut]
        public async Task<ActionResult<StaffingRequest>> PutStaffingRequest([FromBody] StaffingRequest data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid staffing request");
            var request = await _context.StaffingRequests.FindAsync(data.Id);
            if (request == null)
                return NotFound($"Staffing request {data.Id} not found");
            data.Updated = DateTime.UtcNow;
            _context.StaffingRequests.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<StaffingRequest>> PostStaffingRequest([FromBody] StaffingRequest data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid staffing request");
            await _context.StaffingRequests.AddAsync(data);
            await _context.SaveChangesAsync();
            var roles = new List<string>
            {
                "ATM", "DATM", "TA",
                "WM", "EC", "AEC"
            };
            var link = $"/events/staffingrequest/{data.Id}";
            var title = "New Staffing Request";
            await _notificationService.PushNotification(roles, link, title);
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,AEC")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<StaffingRequest>> DeleteStaffingRequest(int id)
        {
            var request = await _context.StaffingRequests.FindAsync(id);
            if (request == null)
                return NotFound($"Staffing request {id} not found");
            _context.StaffingRequests.Remove(request);
            await _context.SaveChangesAsync();
            return Ok(request);
        }
    }
}