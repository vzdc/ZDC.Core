using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Core.Models;

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
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpGet("full")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersFull()
        {
            var users = await _context.Users
                .Include(x => x.Certifications)
                .Include(x => x.Loas)
                .Include(x => x.Warnings)
                .Include(x => x.ControllerLogs)
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

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
                .Include(x => x.ControllerLogs)
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

            return Ok(user?.Certifications);
        }

        [HttpGet("{id}/Loas")]
        public ActionResult<IEnumerable<Loa>> GetLoas(int id)
        {
            var user = _context.Users
                .Include(x => x.Loas)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user?.Loas.ToList());
        }

        [HttpGet("{id}/Warnings")]
        public ActionResult<IEnumerable<Warning>> GetWarnings(int id)
        {
            var user = _context.Users
                .Include(x => x.Warnings)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user?.Warnings.ToList());
        }

        [HttpGet("{id}/ControllerLogs")]
        public ActionResult<IEnumerable<ControllerLog>> GetControllerLogs(int id)
        {
            var user = _context.Users
                .Include(x => x.ControllerLogs)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user?.ControllerLogs.ToList());
        }

        [HttpGet("{id}/DossierEntries")]
        public ActionResult<IEnumerable<Dossier>> GetDossierEntries(int id)
        {
            var user = _context.Users
                .Include(x => x.DossierEntries)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user?.DossierEntries.ToList());
        }

        [HttpGet("{id}/Feedback")]
        public ActionResult<IEnumerable<Feedback>> GetFeedback(int id)
        {
            var user = _context.Users
                .Include(x => x.Feedback)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user?.Feedback.ToList());
        }

        [HttpGet("{id}/Role")]
        public async Task<ActionResult<UserRole?>> GetRole(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            return Ok(user?.Role);
        }

        [HttpGet("{id}/TrainingRole")]
        public async Task<ActionResult<TrainingRole?>> GetTrainingRole(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");


            return Ok(user?.TrainingRole);
        }

        [HttpGet("Online")]
        public ActionResult<IEnumerable<OnlineController>> GetOnlineControllers()
        {
            var onlineControllers = _context.OnlineControllers.ToList();

            return Ok(onlineControllers);
        }

        [HttpGet("Online/full")]
        public async Task<ActionResult<IEnumerable<OnlineController>>> GetOnlineControllersFull()
        {
            var onlineControllers = await _context.OnlineControllers
                .Include(x => x.User)
                .ToListAsync();

            return Ok(onlineControllers);
        }

        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            // todo remove this
            if (!ModelState.IsValid)
                return BadRequest();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPost("{id}/Loa")]
        public async Task<IActionResult> PostLoa(int id, [FromBody] Loa loa)
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
        public async Task<IActionResult> PostWarning(int id, [FromBody] Warning warning)
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
        public async Task<IActionResult> PostDossierEntry(int id, [FromBody] Dossier dossier)
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
        public async Task<IActionResult> PostFeedback(int id, [FromBody] Feedback feedback)
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
        public async Task<IActionResult> PostRole(int id, [FromBody] UserRole data)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound($"User: {id} not found");

            user.Role = data;

            await _context.SaveChangesAsync();

            return Ok(data);
        }

        [HttpDelete("{id}/Loas/{loaId}")]
        public async Task<IActionResult> DeleteLoa(int id, int loaId)
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
        public async Task<IActionResult> DeleteWarning(int id, int warningId)
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
        public async Task<IActionResult> DeleteFeedback(int id, int feedbackId)
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
        public async Task<IActionResult> DeleteRole(int id, int roleId)
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