using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : Controller
    {
        private readonly ZdcContext _context;

        public SettingsController(ZdcContext content)
        {
            _context = content;
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM")]
        [HttpGet]
        public async Task<ActionResult<Settings>> GetSettings()
        {
            var settings = await _context.Settings.FirstOrDefaultAsync();
            return Ok(settings);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM")]
        [HttpPut]
        public async Task<ActionResult<Settings>> PutSettings([FromBody] Settings data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid settings");
            _context.Settings.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }
    }
}