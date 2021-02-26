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
    public class TrainingTicketsController : Controller
    {
        private readonly ZdcContext _context;

        public TrainingTicketsController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TrainingTicket>> GetTrainingTickets()
        {
            var tickets = _context.TrainingTickets.ToList();

            return Ok(tickets);
        }

        [HttpGet("full")]
        public ActionResult<IEnumerable<TrainingTicket>> GetTrainingTicketsFull()
        {
            var tickets = _context.TrainingTickets
                .Include(x => x.Student)
                .Include(x => x.Trainer)
                .ToList();

            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingTicket>> GetTrainingTicket(int id)
        {
            var ticket = await _context.TrainingTickets.FindAsync(id);

            if (ticket == null)
                return NotFound($"Ticket: {id} not found");

            return Ok(ticket);
        }

        [HttpGet("{id}/full")]
        public ActionResult<TrainingTicket> GetTrainingTicketFull(int id)
        {
            var ticket = _context.TrainingTickets
                .Include(x => x.Student)
                .Include(x => x.Trainer)
                .FirstOrDefault(x => x.Id == id);

            if (ticket == null)
                return NotFound($"Ticket: {id} not found");

            return Ok(ticket);
        }

        [HttpGet("Student/{id}")]
        public async Task<ActionResult<IEnumerable<TrainingTicket>>> GetStudentTrainingTickets(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(await _context.TrainingTickets.Where(x => x.Student == user).ToListAsync());
        }

        [HttpGet("Trainer/{id}")]
        public async Task<ActionResult<IEnumerable<TrainingTicket>>> GetTrainerTrainingTickets(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            var tickets = await _context.TrainingTickets.AnyAsync(x => x.Trainer == user);

            return Ok(tickets);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTrainingTicket(int id, [FromBody] JsonPatchDocument<TrainingTicket> data)
        {
            var ticket = _context.TrainingTickets.FirstOrDefault(x => x.Id == id);

            if (ticket == null)
                return NotFound($"Ticket: {id} not found");

            data.ApplyTo(ticket);

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }

        [HttpPut]
        public async Task<IActionResult> PutTrainingTicket([FromBody] TrainingTicket ticket)
        {
            if (!ModelState.IsValid) return BadRequest(ticket);

            await _context.TrainingTickets.AddAsync(ticket);

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingTicket(int id)
        {
            var ticket = _context.TrainingTickets.FirstOrDefault(x => x.Id == id);

            if (ticket == null)
                return NotFound($"Ticket: {id} not found");

            _context.TrainingTickets.Remove(ticket);

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }
    }
}