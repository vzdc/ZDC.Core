using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    public class AirportsController : Controller
    {
        private readonly ZdcContext _context;

        public AirportsController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IList<Airport>> GetAirports()
        {
            return Ok(_context.Airports
                .Include(x => x.Metar).ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Airport> GetAirport(int id)
        {
            return Ok(_context.Airports
                .Include(x => x.Metar)
                .FirstOrDefault(x => x.Id == id));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAirport(int id, [FromBody] Airport data)
        {
            var airport = await _context.Airports.FindAsync(id);

            if (airport == null)
                return NotFound($"Airport: {id} not found");

            _context.Entry(airport).CurrentValues.SetValues(data);

            await _context.SaveChangesAsync();

            return Ok(airport);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> PostAirport([FromBody] Airport airport)
        {
            if (!ModelState.IsValid) return BadRequest(airport);

            await _context.Airports.AddAsync(airport);

            await _context.SaveChangesAsync();

            return Ok(airport);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAirport(int id)
        {
            var airport = await _context.Airports.FindAsync(id);

            if (airport == null)
                return NotFound($"Airport: {id} not found");

            _context.Airports.Remove(airport);

            await _context.SaveChangesAsync();

            return Ok(airport);
        }
    }
}