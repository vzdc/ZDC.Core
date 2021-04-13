using System;
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
    public class UsersController : Controller
    {
        private readonly ZdcContext _context;

        public UsersController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IList<User>> GetUsers()
        {
            return Ok(_context.Users.OrderBy(x => x.LastName).ToList());
        }

        [HttpGet("full")]
        public ActionResult<IList<User>> GetUsersFull()
        {
            return Ok(_context.Users
                .Include(x => x.Certifications)
                .Include(x => x.Loas)
                .Include(x => x.Warnings)
                .Include(x => x.DossierEntries)
                .Include(x => x.Feedback)
                .OrderBy(x => x.LastName)
                .ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user);
        }

        [HttpGet("{id}/full")]
        public ActionResult<User> GetUserFull(int id)
        {
            var user = _context.Users
                .Include(x => x.Certifications)
                .Include(x => x.Loas)
                .Include(x => x.Warnings)
                .Include(x => x.DossierEntries)
                .Include(x => x.Feedback)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user);
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

        [HttpGet("{id}/Role")]
        public ActionResult<UserRole> GetRole(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user.Role);
        }

        [HttpGet("{id}/TrainingRole")]
        public ActionResult<TrainingRole> GetTrainingRole(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user.TrainingRole);
        }

        [HttpGet("Online")]
        public ActionResult<IList<OnlineController>> GetOnlineControllers()
        {
            return Ok(_context.OnlineControllers.ToList());
        }

        [HttpGet("Online/full")]
        public ActionResult<IList<OnlineController>> GetOnlineControllersFull()
        {
            return Ok(_context.OnlineControllers
                .Include(x => x.User)
                .ToList());
        }

        [HttpGet("top")]
        public ActionResult<IList<Hours>> GetTopControllers()
        {
            return Ok(_context.Hours.ToList().Where(x => x.Month == DateTime.UtcNow.Month)
                .Where(x => x.Year == DateTime.UtcNow.Year).OrderByDescending(x => x.TotalHours).Take(3).ToList());
        }

        [HttpGet("top/full")]
        public ActionResult<IList<Hours>> GetTopControllersFull()
        {
            return Ok(_context.Hours.Include(x => x.User).ToList().Where(x => x.Month == DateTime.UtcNow.Month)
                .Where(x => x.Year == DateTime.UtcNow.Year).OrderByDescending(x => x.TotalHours).Take(3).ToList());
        }

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

        [HttpPut("{id}/Role")]
        public async Task<ActionResult> PutRole(int id, [FromBody] UserRole role)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            user.Role = role;

            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPut("{id}/TrainingRole")]
        public async Task<ActionResult> PutTrainingRole(int id, [FromBody] TrainingRole role)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            user.TrainingRole = role;

            await _context.SaveChangesAsync();

            return Ok(user);
        }

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

        [HttpPost("{id}/Roles")]
        public async Task<ActionResult> PostRole(int id, [FromBody] UserRole data)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            user.Role = data;

            await _context.SaveChangesAsync();

            return Ok(data);
        }

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