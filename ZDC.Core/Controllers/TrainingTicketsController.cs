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
    public class TrainingTicketsController : Controller
    {
        private readonly ZdcContext _context;

        public TrainingTicketsController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IList<TrainingTicket>>> GetTrainingTickets()
        {
            return Ok(await _context.TrainingTickets.ToListAsync());
        }

        [HttpGet("full")]
        public async Task<ActionResult<IList<TrainingTicket>>> GetTrainingTicketsFull()
        {
            return Ok(await _context.TrainingTickets
                .Include(x => x.Student)
                .Include(x => x.Trainer)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingTicket>> GetTrainingTicket(int id)
        {
            var ticket = await _context.TrainingTickets.FindAsync(id);

            if (ticket == null)
                return NotFound($"Training ticket: {id} not found");

            return Ok(ticket);
        }

        [HttpGet("{id}/full")]
        public async Task<ActionResult<TrainingTicket>> GetTrainingTicketFull(int id)
        {
            var ticket = await _context.TrainingTickets
                .Include(x => x.Student)
                .Include(x => x.Trainer)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (ticket == null)
                return NotFound($"Ticket: {id} not found");

            return Ok(ticket);
        }

        [HttpGet("Student/{id}")]
        public async Task<ActionResult<IList<TrainingTicket>>> GetStudentTrainingTickets(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(await _context.TrainingTickets.Where(x => x.Student == user).ToListAsync());
        }

        [HttpGet("Trainer/{id}")]
        public async Task<ActionResult<IList<TrainingTicket>>> GetTrainerTrainingTickets(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(await _context.TrainingTickets.Where(x => x.Trainer == user).ToListAsync());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutTrainingTicket(int id, [FromBody] TrainingTicket data)
        {
            var ticket = await _context.TrainingTickets.FindAsync(id);

            if (ticket == null)
                return NotFound($"Ticket: {id} not found");

            _context.Entry(ticket).CurrentValues.SetValues(data);

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult> PostTrainingTicket([FromBody] TrainingTicket ticket)
        {
            if (!ModelState.IsValid) return BadRequest(ticket);

            await _context.TrainingTickets.AddAsync(ticket);

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrainingTicket(int id)
        {
            var ticket = await _context.TrainingTickets.FindAsync(id);

            if (ticket == null)
                return NotFound($"Ticket: {id} not found");

            _context.TrainingTickets.Remove(ticket);

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }
    }
}