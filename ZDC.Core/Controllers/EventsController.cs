using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Core.Models;

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
        public IEnumerable<Event> GetEvents()
        {
            return _context.Events.ToList();
        }

        [HttpGet("full")]
        public IEnumerable<Event> GetEventsFull()
        {
            return _context.Events
                .Include(x => x.Registrations)
                .ToList();
        }

        [HttpGet("{id}/Registrations")]
        public IEnumerable<EventRegistration> GetEventRegistrations(int id)
        {
            var @event = _context.Events.FirstOrDefault(x => x.Id == id);

            return @event?.Registrations;
        }

        [HttpGet("{id}/Registrations/{registrationId}")]
        public EventRegistration GetEventRegistration(int id, int registrationId)
        {
            var @event = _context.Events.FirstOrDefault(x => x.Id == id);

            return @event?.Registrations.FirstOrDefault(x => x.Id == registrationId);
        }

        [HttpGet("{id}/Positions")]
        public IEnumerable<EventPosition> GetEventPositions(int id)
        {
            var @event = _context.Events.FirstOrDefault(x => x.Id == id);

            return @event?.Positions;
        }

        [HttpGet("{id}/Positions/{positionId}")]
        public EventPosition GetEventPosition(int id, int positionId)
        {
            var @event = _context.Events.FirstOrDefault(x => x.Id == id);

            return @event?.Positions.FirstOrDefault(x => x.Id == positionId);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEvent(int id, [FromBody] JsonPatchDocument<Event> data)
        {
            var @event = _context.Events.FirstOrDefault(x => x.Id == id);

            if (@event == null)
                return NotFound($"Event: {id} not found");

            data.ApplyTo(@event);

            await _context.SaveChangesAsync();

            return Ok(@event);
        }

        [HttpPatch("{id}/Registration/{registrationId}")]
        public async Task<IActionResult> PatchEventRegistration(int id, int registrationId,
            [FromBody] JsonPatchDocument<EventRegistration> data)
        {
            var @event = _context.Events.FirstOrDefault(x => x.Id == id);

            if (@event == null)
                return NotFound($"Event: {id} not found");

            var registration = @event.Registrations.FirstOrDefault(x => x.Id == registrationId);

            if (registration == null)
                return NotFound($"Registration: {registrationId} not found");

            data.ApplyTo(registration);

            await _context.SaveChangesAsync();

            return Ok(registration);
        }

        [HttpPut]
        public async Task<IActionResult> PutEvent([FromBody] Event @event)
        {
            if (!ModelState.IsValid)
                return BadRequest(@event);

            await _context.AddAsync(@event);

            await _context.SaveChangesAsync();

            return Ok(@event);
        }

        [HttpPut("{id}/Registration")]
        public async Task<IActionResult> PutEventRegistration(int id, [FromBody] EventRegistration registration)
        {
            var @event = _context.Events.FirstOrDefault(x => x.Id == id);

            if (@event == null)
                return NotFound($"Event: {id} not found");

            if (!ModelState.IsValid)
                return BadRequest(registration);

            @event.Registrations.Add(registration);

            await _context.SaveChangesAsync();

            return Ok(registration);
        }

        [HttpPut("{id}/Position")]
        public async Task<IActionResult> PutEventPosition(int id, [FromBody] EventPosition position)
        {
            var @event = _context.Events.FirstOrDefault(x => x.Id == id);

            if (@event == null)
                return NotFound($"Event: {id} not found");

            if (!ModelState.IsValid)
                return BadRequest(position);

            @event.Positions.Add(position);

            await _context.SaveChangesAsync();

            return Ok(position);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = _context.Events.FirstOrDefault(x => x.Id == id);

            if (@event == null)
                return NotFound($"Event: {id} not found");

            _context.Events.Remove(@event);

            await _context.SaveChangesAsync();

            return Ok(@event);
        }

        [HttpDelete("{id}/Registration/{registrationId}")]
        public async Task<IActionResult> DeleteEventRegistration(int id, int registrationId)
        {
            var @event = _context.Events.FirstOrDefault(x => x.Id == id);

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
        public async Task<IActionResult> DeleteEventPosition(int id, int positionId)
        {
            var @event = _context.Events.FirstOrDefault(x => x.Id == id);

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