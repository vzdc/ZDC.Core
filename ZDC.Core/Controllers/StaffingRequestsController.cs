using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    public class StaffingRequestsController : Controller
    {
        private readonly ZdcContext _context;

        public StaffingRequestsController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IList<StaffingRequest>>> GetStaffingRequests()
        {
            return Ok(await _context.StaffingRequests.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StaffingRequest>> GetStaffingRequest(int id)
        {
            var request = await _context.StaffingRequests.FindAsync(id);

            if (request == null) return NotFound($"Staffing request: {id} not found");

            return Ok(request);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StaffingRequest>> PutStaffingRequest(int id, [FromBody] StaffingRequest data)
        {
            var request = await _context.StaffingRequests.FindAsync(id);

            if (request == null) return NotFound($"Staffing request: {id} not found");

            _context.Entry(request).CurrentValues.SetValues(data);

            await _context.SaveChangesAsync();

            return Ok(request);
        }

        [HttpPost]
        public async Task<ActionResult<StaffingRequest>> PostStaffingRequest([FromBody] StaffingRequest data)
        {
            if (!ModelState.IsValid) return BadRequest(data);

            await _context.StaffingRequests.AddAsync(data);

            await _context.SaveChangesAsync();

            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<StaffingRequest>> DeleteStaffingRequest(int id)
        {
            var request = await _context.StaffingRequests.FindAsync(id);

            if (request == null) return NotFound($"Staffing request: {id} not found");

            _context.StaffingRequests.Remove(request);

            return Ok(request);
        }
    }
}