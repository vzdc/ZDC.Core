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
    public class TrainingTicketsController : Controller
    {
        private readonly ZdcContext _context;

        public TrainingTicketsController(ZdcContext content)
        {
            _context = content;
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,EC,FE,ATA,INS,MTR")]
        [HttpGet]
        public async Task<ActionResult<IList<TrainingTicket>>> GetTrainingTickets()
        {
            var tickets = await _context.TrainingTickets
                .Include(x => x.Student)
                .Include(x => x.Trainer)
                .ToListAsync();
            return Ok(tickets);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,EC,FE,ATA,INS,MTR")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingTicket>> GetTrainingTicket(int id)
        {
            var ticket = await _context.TrainingTickets.FindAsync(id);
            return ticket != null ? Ok(ticket) : NotFound($"Training ticket {id} not found");
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,EC,FE,ATA,INS,MTR")]
        [HttpPut]
        public async Task<ActionResult<TrainingTicket>> PutTrainingTicket([FromBody] TrainingTicket data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid training ticket");
            var request = await _context.TrainingTickets.FindAsync(data.Id);
            if (request == null)
                return NotFound($"Training ticket {data.Id} not found");
            var student = await _context.Users.FindAsync(data.StudentId);
            if (student == null)
                return NotFound($"Student {data.StudentId} not found");
            data.Student = student;
            var trainer = await _context.Users.FindAsync(data.TrainerId);
            if (trainer == null)
                return NotFound($"Trainer {data.TrainerId} not found");
            data.Student = student;
            data.Updated = DateTime.UtcNow;
            _context.TrainingTickets.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,EC,FE,ATA,INS,MTR")]
        [HttpPost]
        public async Task<ActionResult<TrainingTicket>> PostTrainingTicket([FromBody] TrainingTicket data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid training ticket");
            var student = await _context.Users.FindAsync(data.StudentId);
            if (student == null)
                return NotFound($"Student {data.StudentId} not found");
            data.Student = student;
            var trainer = await _context.Users.FindAsync(data.TrainerId);
            if (trainer == null)
                return NotFound($"Trainer {data.TrainerId} not found");
            await _context.TrainingTickets.AddAsync(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,ATA")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<TrainingTicket>> DeleteTrainingTicket(int id)
        {
            var ticket = await _context.TrainingTickets.FindAsync(id);
            if (ticket == null)
                return NotFound($"Training ticket {id} not found");
            _context.TrainingTickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return Ok(ticket);
        }
    }
}