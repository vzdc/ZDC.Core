using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Core.Extensions;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly ZdcContext _context;

        public EventsController(ZdcContext content)
        {
            _context = content;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Event>>> GetEvents()
        {
            var events = await _context.Events
                .Where(x => x.Open)
                .ToListAsync();
            return Ok(events);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,FE,ATA,AWM,AEC,AFE")]
        [HttpGet("All")]
        public async Task<ActionResult<IList<Event>>> GetAllEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,AEC")]
        [HttpPut]
        public async Task<ActionResult<Event>> PutEvent([FromBody] Event data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid event");
            data.Updated = DateTime.UtcNow;
            _context.Events.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,AEC")]
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent([FromBody] Event data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid event");
            await _context.Events.AddAsync(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,FE,ATA,AWM,AEC,AFE")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Event>> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return NotFound($"Event {id} not found");
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return Ok(@event);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,AEC")]
        [HttpGet("Positions/{id}")]
        public async Task<ActionResult<IList<EventPosition>>> GetEventPositions(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return NotFound($"Event {id} not found");
            return @event.Positions.ToList();
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,AEC")]
        [HttpPost("Positions/{id}")]
        public async Task<ActionResult<EventPosition>> PostEventPosition([FromBody] EventPosition data, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid event position");
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return NotFound($"Event {id} not found");
            @event.Positions.Add(data);
            return data;
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,AEC")]
        [HttpDelete("Positions/{id}/{positionId}")]
        public async Task<ActionResult<EventPosition>> DeleteEventPosition(int id, int positionId)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return NotFound($"Event {id} not found");
            var position = @event.Positions.FirstOrDefault(x => x.Id == positionId);
            if (position == null)
                return NotFound($"Event position {positionId} not found");
            @event.Positions.Remove(position);
            await _context.SaveChangesAsync();
            return Ok(position);
        }

        [HttpGet("Registrations")]
        public async Task<ActionResult<IList<EventRegistration>>> GetEventRegistrations()
        {
            var registrations = await _context.EventRegistrations.ToListAsync();
            return Ok(registrations);
        }

        //[Authorize]
        [HttpGet("Registrations/{id}")]
        public async Task<ActionResult<EventRegistration>> GetEventRegistration(int id)
        {
            var registration = await _context.EventRegistrations.FindAsync(id);
            if (registration == null)
                NotFound($"Event registration {id} not found");
            if (!await User.HasEventRegistration(_context, id) && !await User.IsStaff(_context))
                return Unauthorized($"Cannot view event registration {id}");
            return Ok(registration);
        }

        //[Authorize]
        [HttpPut("Registrations")]
        public async Task<ActionResult<EventRegistration>> PutEventRegistration([FromBody] EventRegistration data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid event registration");
            if (!await User.HasEventRegistration(_context, data.Id) && !await User.IsStaff(_context))
                return Unauthorized($"Cannot view event registration {data.Id}");
            _context.EventRegistrations.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize]
        [HttpPost("Registrations")]
        public async Task<ActionResult<EventRegistration>> PostEventRegistration([FromBody] EventRegistration data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid event registration");
            var @event = await _context.Events.FindAsync(data.EventId);
            if (!@event.Open)
                return Unauthorized($"Cannot register for event {data.EventId}");
            data.Event = @event;
            var user = await _context.Users.FindAsync(data.UserId);
            if (user == null)
                return NotFound($"User {data.UserId} not found");
            data.User = user;
            await _context.EventRegistrations.AddAsync(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize]
        [HttpDelete("Registrations")]
        public async Task<ActionResult<EventRegistration>> DeleteEventRegistration(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid event registration");
            var registration = await _context.EventRegistrations.FindAsync(id);
            if (registration == null)
                return NotFound($"Event registration {id} not found");
            if (!await User.HasEventRegistration(_context, id) && !await User.IsStaff(_context))
                return Unauthorized($"Cannot view event registration {id}");
            _context.EventRegistrations.Remove(registration);
            await _context.SaveChangesAsync();
            return Ok(registration);
        }
    }
}