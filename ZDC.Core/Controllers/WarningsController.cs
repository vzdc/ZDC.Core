using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarningsController : Controller
    {
        private readonly ZdcContext _context;

        public WarningsController(ZdcContext content)
        {
            _context = content;
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<IList<Warning>>> GetUserWarnings(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound($"User {userId} not found");
            var warnings = await _context.Warnings
                .Where(x => x.User == user).ToListAsync();
            return Ok(warnings);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM")]
        [HttpPut]
        public async Task<ActionResult<Warning>> PutWarning([FromBody] Warning data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid warning");
            var user = await _context.Users.FindAsync(data.UserId);
            if (user == null)
                return NotFound($"User {data.UserId} not found");
            data.User = user;
            _context.Warnings.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM")]
        [HttpPost]
        public async Task<ActionResult<Warning>> PostWarning([FromBody] Warning data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid warning");
            var user = await _context.Users.FindAsync(data.UserId);
            if (user == null)
                return NotFound($"User {data.UserId} not found");
            data.User = user;
            await _context.Warnings.AddAsync(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Warning>> DeleteWarning(int id)
        {
            var warning = await _context.Warnings.FindAsync(id);
            if (warning == null)
                return NotFound($"Warning {id} not found");
            _context.Warnings.Remove(warning);
            await _context.SaveChangesAsync();
            return Ok(warning);
        }
    }
}