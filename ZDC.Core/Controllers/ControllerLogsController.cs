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
    public class ControllerLogsController : Controller
    {
        private readonly ZdcContext _context;

        public ControllerLogsController(ZdcContext content)
        {
            _context = content;
        }

        [HttpGet("{cid}")]
        public async Task<ActionResult<IList<ControllerLog>>> GetControllerLogs(int cid)
        {
            var user = await _context.Users.FindAsync(cid);
            if (user == null)
                return NotFound($"User {cid} not found");
            var logs = await _context.ControllerLogs
                .OrderBy(x => x.Login)
                .Where(x => x.User == user)
                .ToListAsync();
            return Ok(logs);
        }
    }
}