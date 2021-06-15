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
    public class UsersController : Controller
    {
        private readonly ZdcContext _context;

        public UsersController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IList<User>> GetUsers(bool full)
        {
            if (full)
                return Ok(_context.Users
                    .Include(x => x.Certifications)
                    .Include(x => x.Loas)
                    .Include(x => x.Warnings)
                    .Include(x => x.DossierEntries)
                    .Include(x => x.Feedback)
                    .Include(x => x.Hours)
                    .OrderBy(x => x.LastName)
                    .ToList());
            return Ok(_context.Users.OrderBy(x => x.LastName).ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id, bool full = false)
        {
            User user;
            if (full)
            {
                user = _context.Users
                    .Include(x => x.Certifications)
                    .Include(x => x.Loas)
                    .Include(x => x.Warnings)
                    .Include(x => x.DossierEntries)
                    .Include(x => x.Feedback)
                    .Include(x => x.Hours)
                    .FirstOrDefault(x => x.Id == id);

                if (user == null)
                    return NotFound($"User: {id} not found");

                return Ok(user);
            }

            user = _context.Users.Find(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user);
        }

        [HttpGet("Staff")]
        public ActionResult<IList<User>> GetStaff()
        {
            return Ok(_context.Users.Where(x => x.Roles.Any()).ToList());
        }

        [HttpGet("{id}/Certification")]
        public ActionResult<Certification> GetUserCertification(int id)
        {
            var user = _context.Users
                .Include(x => x.Certifications)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user.Certifications);
        }

        [Authorize]
        [HttpGet("{id}/Loas")]
        public ActionResult<IList<Loa>> GetLoas(int id)
        {
            var user = _context.Users
                .Include(x => x.Loas)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user.Loas);
        }

        [Authorize]
        [HttpGet("{id}/Warnings")]
        public ActionResult<IList<Warning>> GetWarnings(int id)
        {
            var user = _context.Users
                .Include(x => x.Warnings)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user.Warnings);
        }

        [Authorize]
        [HttpGet("{id}/DossierEntries")]
        public ActionResult<IList<Dossier>> GetDossierEntries(int id)
        {
            var user = _context.Users
                .Include(x => x.DossierEntries)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user.DossierEntries);
        }

        [Authorize]
        [HttpGet("{id}/Feedback")]
        public ActionResult<IList<Feedback>> GetFeedback(int id)
        {
            var user = _context.Users
                .Include(x => x.Feedback)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user.Feedback);
        }

        [HttpGet("Online")]
        public ActionResult<IList<OnlineController>> GetOnlineControllers(bool full = false)
        {
            if (full)
                return Ok(_context.OnlineControllers
                    .Include(x => x.User).ToList());
            return Ok(_context.OnlineControllers.ToList());
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(int id, [FromBody] User data)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            _context.Entry(user).CurrentValues.SetValues(data);

            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [Authorize]
        [HttpPut("{id}/Loa/{loaId}")]
        public async Task<ActionResult> PutLoa(int id, int loaId, [FromBody] Loa data)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            var loa = user.Loas.FirstOrDefault(x => x.Id == loaId);

            if (loa == null)
                return NotFound($"LOA: {loaId} not found");

            _context.Entry(loa).CurrentValues.SetValues(data);

            await _context.SaveChangesAsync();

            return Ok(loa);
        }

        [Authorize]
        [HttpPut("{id}/Warning/{warningId}")]
        public async Task<ActionResult> PutWarning(int id, int warningId, [FromBody] Warning data)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            var warning = user.Warnings.FirstOrDefault(x => x.Id == warningId);

            if (warning == null)
                return NotFound($"Warning: {warningId} not found");

            _context.Entry(warning).CurrentValues.SetValues(data);

            await _context.SaveChangesAsync();

            return Ok(warning);
        }

        [Authorize]
        [HttpPut("{id}/DossierEntry/{dossierId}")]
        public async Task<ActionResult> PutDossierEntry(int id, int dossierId, [FromBody] Dossier data)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            var dossier = user.DossierEntries.FirstOrDefault(x => x.Id == dossierId);

            if (dossier == null)
                return NotFound($"Dossier entry: {dossierId} not found");

            _context.Entry(dossier).CurrentValues.SetValues(data);

            await _context.SaveChangesAsync();

            return Ok(dossier);
        }

        [Authorize]
        [HttpPut("{id}/Feedback/{feedbackId}")]
        public async Task<ActionResult> PutDossierEntry(int id, int feedbackId, [FromBody] Feedback data)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            var feedback = user.Feedback.FirstOrDefault(x => x.Id == feedbackId);

            if (feedback == null)
                return NotFound($"Feedback entry: {feedbackId} not found");

            _context.Entry(feedback).CurrentValues.SetValues(data);

            await _context.SaveChangesAsync();

            return Ok(feedback);
        }

        [Authorize]
        [HttpPut("{id}/Role")]
        public async Task<ActionResult> PutRole(int id, [FromBody] Role entry)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            var role = await _context.Roles.FindAsync(entry.Id);

            if (role == null)
                return NotFound($"Role: {entry.Name} not found");

            user.Roles.Add(role);

            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [Authorize]
        [HttpPost("{id}/Loa")]
        public async Task<ActionResult> PostLoa(int id, [FromBody] Loa loa)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            if (user.Loas == null)
                user.Loas = new List<Loa> {loa};
            else
                user.Loas.Add(loa);

            await _context.SaveChangesAsync();

            return Ok(loa);
        }

        [Authorize]
        [HttpPost("{id}/Warning")]
        public async Task<ActionResult> PostWarning(int id, [FromBody] Warning warning)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            if (user.Warnings == null)
                user.Warnings = new List<Warning> {warning};
            else
                user.Warnings.Add(warning);

            await _context.SaveChangesAsync();

            return Ok(warning);
        }

        [Authorize]
        [HttpPost("{id}/DossierEntry")]
        public async Task<ActionResult> PostDossierEntry(int id, [FromBody] Dossier dossier)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            if (user.DossierEntries == null)
                user.DossierEntries = new List<Dossier> {dossier};
            else
                user.DossierEntries.Add(dossier);

            await _context.SaveChangesAsync();

            return Ok(dossier);
        }

        [Authorize]
        [HttpPost("{id}/Feedback")]
        public async Task<ActionResult> PostFeedback(int id, [FromBody] Feedback feedback)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            if (user.Feedback == null)
                user.Feedback = new List<Feedback> {feedback};
            else
                user.Feedback.Add(feedback);

            await _context.SaveChangesAsync();

            return Ok(feedback);
        }

        [Authorize]
        [HttpDelete("{id}/Loas/{loaId}")]
        public async Task<ActionResult> DeleteLoa(int id, int loaId)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            var loa = user.Loas.FirstOrDefault(x => x.Id == loaId);

            if (loa == null)
                return NotFound($"Loa: {loaId} not found");

            user.Loas.Remove(loa);

            await _context.SaveChangesAsync();

            return Ok(loa);
        }

        [Authorize]
        [HttpDelete("{id}/Warnings/{warningId}")]
        public async Task<ActionResult> DeleteWarning(int id, int warningId)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            var warning = user.Warnings.FirstOrDefault(x => x.Id == warningId);

            if (warning == null)
                return NotFound($"Warning: {warningId} not found");

            user.Warnings.Remove(warning);

            await _context.SaveChangesAsync();

            return Ok(warning);
        }

        [Authorize]
        [HttpDelete("{id}/Feedback/{feedbackId}")]
        public async Task<ActionResult> DeleteFeedback(int id, int feedbackId)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            var feedback = user.Feedback.FirstOrDefault(x => x.Id == feedbackId);

            if (feedback == null)
                return NotFound($"Feedback: {feedbackId} not found");

            user.Feedback.Remove(feedback);

            await _context.SaveChangesAsync();

            return Ok(feedback);
        }

        [Authorize]
        [HttpDelete("{id}/Roles/{roleId}")]
        public async Task<ActionResult> DeleteRole(int id, int roleId)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            var role = user.Warnings.FirstOrDefault(x => x.Id == roleId);

            if (role == null)
                return NotFound($"Role: {roleId} not found");

            user.Warnings.Remove(role);

            await _context.SaveChangesAsync();

            return Ok(role);
        }
    }
}