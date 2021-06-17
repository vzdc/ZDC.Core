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
    [ApiController]
    public class DossierController : Controller
    {
        private readonly ZdcContext _context;

        public DossierController(ZdcContext content)
        {
            _context = content;
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,FE,ATA,AWM,AEC,AFE,INS,MTR")]
        [HttpGet("{cid}")]
        public async Task<ActionResult<IList<Dossier>>> GetDossierEntries(int cid)
        {
            var user = await _context.Users.FindAsync(cid);
            if (user == null)
                return NotFound($"User {cid} not found");
            var dossier = await _context.Dossier
                .Include(x => x.Submitter)
                .Where(x => x.User == user)
                .ToListAsync();
            return Ok(dossier);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,FE,ATA,AWM,AEC,AFE,INS,MTR")]
        [HttpPost]
        public async Task<ActionResult<Dossier>> PostDossierEntry([FromBody] Dossier data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid dossier entry");
            await _context.Dossier.AddAsync(data);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(data.UserId);
            var submitter = await _context.Users.FindAsync(data.SubmitterId);
            var dossier = await _context.Dossier.FindAsync(data.Id);
            dossier.User = user;
            dossier.Submitter = submitter;
            await _context.SaveChangesAsync();

            return Ok(dossier);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Dossier>> DeleteDossierEntry(int id)
        {
            var dossier = await _context.Dossier.FindAsync(id);
            if (dossier == null)
                return NotFound($"Dossier entry {id} not found");
            _context.Dossier.Remove(dossier);
            await _context.SaveChangesAsync();
            return Ok(dossier);
        }
    }
}