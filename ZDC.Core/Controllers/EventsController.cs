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
    public class EventsController : Controller
    {
        private readonly ZdcContext _context;

        public EventsController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IList<Event>> GetEvents()
        {
            return Ok(_context.Events.ToList());
        }

        [HttpGet("full")]
        public ActionResult<IList<Event>> GetEventsFull()
        {
            return Ok(_context.Events
                .Include(x => x.Registrations)
                .ToList());
        }

        [HttpGet("{id}/Registrations")]
        public ActionResult<IList<EventRegistration>> GetEventRegistrations(int id)
        {
            var @event = _context.Events.Find(id);

            if (@event == null) return NotFound($"Event: {id} not found");

            return Ok(@event.Registrations);
        }

        [HttpGet("{id}/Registrations/{registrationId}")]
        public ActionResult<EventRegistration> GetEventRegistration(int id, int registrationId)
        {
            var @event = _context.Events.Find(id);

            if (@event == null) return NotFound($"Event: {id} not found");

            var registration = @event.Registrations.FirstOrDefault(x => x.Id == registrationId);

            if (registration == null) return NotFound($"Event registration: {registrationId} not found");

            return registration;
        }

        [HttpGet("{id}/Positions")]
        public ActionResult<IList<EventPosition>> GetEventPositions(int id)
        {
            var @event = _context.Events.Find(id);

            if (@event == null) return NotFound($"Event: {id} not found");

            return Ok(@event.Positions);
        }

        [HttpGet("{id}/Positions/{positionId}")]
        public ActionResult<EventPosition> GetEventPosition(int id, int positionId)
        {
            var @event = _context.Events.Find(id);

            if (@event == null) return NotFound($"Event: {id} not found");

            var position = @event.Positions.FirstOrDefault(x => x.Id == positionId);

            if (position == null) return NotFound($"Event position: {positionId} not found");

            return position;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutEvent(int id, [FromBody] Event data)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
                return NotFound($"Event: {id} not found");

            _context.Entry(@event).CurrentValues.SetValues(data);

            await _context.SaveChangesAsync();

            return Ok(@event);
        }

        [HttpPut("{id}/Registration/{registrationId}")]
        public async Task<ActionResult> PutEventRegistration(int id, int registrationId,
            [FromBody] EventRegistration data)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
                return NotFound($"Event: {id} not found");

            var registration = @event.Registrations.FirstOrDefault(x => x.Id == registrationId);

            if (registration == null)
                return NotFound($"Event registration: {registrationId} not found");

            _context.Entry(registration).CurrentValues.SetValues(data);

            await _context.SaveChangesAsync();

            return Ok(registration);
        }

        [HttpPost]
        public async Task<ActionResult> PostEvent([FromBody] Event @event)
        {
            if (!ModelState.IsValid)
                return BadRequest(@event);

            await _context.AddAsync(@event);

            await _context.SaveChangesAsync();

            return Ok(@event);
        }

        [HttpPost("{id}/Registration")]
        public async Task<IActionResult> PostEventRegistration(int id, [FromBody] EventRegistration registration)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
                return NotFound($"Event: {id} not found");

            if (!ModelState.IsValid)
                return BadRequest(registration);

            @event.Registrations.Add(registration);

            await _context.SaveChangesAsync();

            return Ok(registration);
        }

        [HttpPut("{id}/Position")]
        public async Task<ActionResult> PostEventPosition(int id, [FromBody] EventPosition position)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
                return NotFound($"Event: {id} not found");

            if (!ModelState.IsValid)
                return BadRequest(position);

            @event.Positions.Add(position);

            await _context.SaveChangesAsync();

            return Ok(position);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
                return NotFound($"Event: {id} not found");

            _context.Events.Remove(@event);

            await _context.SaveChangesAsync();

            return Ok(@event);
        }

        [HttpDelete("{id}/Registration/{registrationId}")]
        public async Task<ActionResult> DeleteEventRegistration(int id, int registrationId)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
                return NotFound($"Event: {id} not found");

            var registration = @event.Registrations.FirstOrDefault(x => x.Id == registrationId);

            if (registration == null)
                return NotFound($"Registration: {registrationId} not found");

            @event.Registrations.Remove(registration);

            await _context.SaveChangesAsync();

            return Ok(registration);
        }

        [HttpDelete("{id}/Position/{positionId}")]
        public async Task<ActionResult> DeleteEventPosition(int id, int positionId)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
                return NotFound($"Event: {id} not found");

            var position = @event.Positions.FirstOrDefault(x => x.Id == positionId);

            if (position == null)
                return NotFound($"Position: {positionId} not found");

            @event.Positions.Remove(position);

            await _context.SaveChangesAsync();

            return Ok(position);
        }
    }
}