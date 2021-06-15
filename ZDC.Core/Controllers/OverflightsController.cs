using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    public class OverflightsController : Controller
    {
        private readonly ZdcContext _context;

        public OverflightsController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IList<Overflight>> GetOverflights()
        {
            return Ok(_context.Overflights.OrderBy(x => x.Callsign).ToList());
        }
    }
}