using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    public class BugReportsController : Controller
    {
        private readonly ZdcContext _context;

        public BugReportsController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IList<BugReport>>> GetBugReports()
        {
            return Ok(await _context.BugReports.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BugReport>> GetBugReport(int id)
        {
            var report = await _context.BugReports.FindAsync(id);

            if (report == null) return NotFound($"Bug report: {id} not found");

            return Ok(report);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BugReport>> PutBugReport(int id, [FromBody] BugReport data)
        {
            var report = await _context.BugReports.FindAsync(id);

            if (report == null) return NotFound($"Bug report: {id} not found");

            _context.Entry(report).CurrentValues.SetValues(data);

            await _context.SaveChangesAsync();

            return Ok(report);
        }

        [HttpPost]
        public async Task<ActionResult<BugReport>> PostBugReport([FromBody] BugReport data)
        {
            if (!ModelState.IsValid) return BadRequest(data);

            await _context.BugReports.AddAsync(data);

            await _context.SaveChangesAsync();

            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BugReport>> DeleteBugReport(int id)
        {
            var report = await _context.BugReports.FindAsync(id);

            if (report == null) return NotFound($"Bug report: {id} not found");

            _context.BugReports.Remove(report);

            return Ok(report);
        }
    }
}