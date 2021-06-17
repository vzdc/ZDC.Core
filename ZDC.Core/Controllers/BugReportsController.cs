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
    public class BugReportsController : Controller
    {
        private readonly ZdcContext _context;

        public BugReportsController(ZdcContext content)
        {
            _context = content;
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,AWM")]
        [HttpGet]
        public async Task<ActionResult<IList<BugReport>>> GetBugReports()
        {
            var reports = await _context.BugReports.ToListAsync();
            return Ok(reports);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,AWM")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BugReport>> GetBugReport(int id)
        {
            var report = await _context.BugReports.FindAsync(id);
            return report != null ? Ok(report) : NotFound($"Bug report {id} not found");
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,AWM")]
        [HttpPut]
        public async Task<ActionResult<BugReport>> PutBugReport([FromBody] BugReport data)
        {
            _context.BugReports.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<BugReport>> PostBugReport([FromBody] BugReport data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid bug report");
            await _context.BugReports.AddAsync(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,AWM")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<BugReport>> DeleteBugReport(int id)
        {
            var report = await _context.BugReports.FindAsync(id);
            if (report == null)
                return NotFound($"Bug report {id} not found");
            _context.BugReports.Remove(report);
            await _context.SaveChangesAsync();
            return Ok(report);
        }
    }
}