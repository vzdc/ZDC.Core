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
    public class OverflightsController : Controller
    {
        private readonly ZdcContext _context;

        public OverflightsController(ZdcContext content)
        {
            _context = content;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Overflight>>> GetOverflights()
        {
            var overflights = await _context.Overflights.ToListAsync();
            return Ok(overflights);
        }
    }
}