using System;
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
    public class AirportsController : Controller
    {
        private readonly ZdcContext _context;

        public AirportsController(ZdcContext content)
        {
            _context = content;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Airport>>> GetAirports()
        {
            var airports = await _context.Airports.ToListAsync();
            return Ok(airports);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Airport>> GetAirport(int id)
        {
            var airport = await _context.Airports.FindAsync(id);
            return airport != null ? Ok(airport) : NotFound($"Airport {id} not found");
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,AFE")]
        [HttpPut]
        public async Task<ActionResult<Airport>> PutAirport([FromBody] Airport data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid airport");
            data.Updated = DateTime.UtcNow;
            _context.Airports.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,AFE")]
        [HttpPost]
        public async Task<ActionResult<Airport>> PostAirport([FromBody] Airport data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid airport");
            await _context.Airports.AddAsync(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,AFE")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Airport>> DeleteAirport(int id)
        {
            var airport = await _context.Airports.FirstOrDefaultAsync(x => x.Id == id);
            if (airport == null)
                return NotFound($"Airport {id} not found");
            _context.Airports.Remove(airport);
            await _context.SaveChangesAsync();
            return Ok(airport);
        }
    }
}