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
    public class OnlineControllersController : Controller
    {
        private readonly ZdcContext _context;

        public OnlineControllersController(ZdcContext content)
        {
            _context = content;
        }

        [HttpGet]
        public async Task<ActionResult<IList<OnlineController>>> GetOnlineControllers()
        {
            var onlineControllers = await _context.OnlineControllers.ToListAsync();
            return Ok(onlineControllers);
        }
    }
}