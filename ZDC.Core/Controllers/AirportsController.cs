using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ZDC.Core.Data;
using ZDC.Core.Models;

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
        public IEnumerable<Airport> GetAirports()
        {
            return _context.Airports.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Airport>> GetAirport(int id)
        {
            return await _context.Airports.FindAsync(id);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAirport(int id, [FromBody] JsonPatchDocument<Airport> data)
        {
            var airport = await _context.Airports.FindAsync(id);

            if (airport == null)
                return NotFound($"Airport: {id} not found");

            data.ApplyTo(airport);

            await _context.SaveChangesAsync();

            return Ok(airport);
        }

        [HttpPut]
        public async Task<IActionResult> PutAirport([FromBody] Airport airport)
        {
            if (!ModelState.IsValid) return BadRequest(airport);

            await _context.Airports.AddAsync(airport);

            await _context.SaveChangesAsync();

            return Ok(airport);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirport(int id)
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