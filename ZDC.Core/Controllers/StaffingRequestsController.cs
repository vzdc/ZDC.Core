using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class StaffingRequestsController : Controller
    {
        private readonly ZdcContext _context;

        public StaffingRequestsController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IList<StaffingRequest>> GetStaffingRequests()
        {
            return Ok(_context.StaffingRequests.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<StaffingRequest> GetStaffingRequest(int id)
        {
            var request = _context.StaffingRequests.Find(id);

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