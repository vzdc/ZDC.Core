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
    public class UsersController : Controller
    {
        private readonly ZdcContext _context;

        public UsersController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _context.Users
                .ToList();
        }

        [HttpGet("full")]
        public IEnumerable<User> GetUsersFull()
        {
            return _context.Users
                .Include(x => x.Certifications)
                .Include(x => x.Loas)
                .Include(x => x.Warnings)
                .ToList();
        }

        [HttpGet("{id}")]
        public User GetUser(int id)
        {
            return _context.Users
                .FirstOrDefault(x => x.Id == id);
        }

        [HttpGet("{id}/full")]
        public User GetUserFull(int id)
        {
            return _context.Users
                .Include(x => x.Certifications)
                .Include(x => x.Loas)
                .Include(x => x.Warnings)
                .FirstOrDefault(x => x.Id == id);
        }

        [HttpGet("{id}/Certification")]
        public Certification GetUserCertification(int id)
        {
            var user = _context.Users
                .Include(x => x.Certifications)
                .FirstOrDefault(x => x.Id == id);
            return user?.Certifications;
        }

        [HttpGet("{id}/Loas")]
        public IEnumerable<Loas> GetLoas(int id)
        {
            var user = _context.Users
                .Include(x => x.Loas)
                .FirstOrDefault(x => x.Id == id);
            return user?.Loas.ToList();
        }

        [HttpGet("{id}/Warnings")]
        public IEnumerable<Warnings> GetWarnings(int id)
        {
            var user = _context.Users
                .Include(x => x.Warnings)
                .FirstOrDefault(x => x.Id == id);
            return user?.Warnings.ToList();
        }

        [HttpGet("{id}/Role")]
        public UserRole? GetRole(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            return user?.Role;
        }

        [HttpGet("{id}/TrainingRole")]
        public TrainingRole? GetTrainingRole(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            return user?.TrainingRole;
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] JsonPatchDocument<User> data)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound("User not found");

            data.ApplyTo(user);

            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPatch("{id}/Certification")]
        public async Task<IActionResult> PatchCertification(int id, [FromBody] JsonPatchDocument<Certification> data)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound("User not found");

            data.ApplyTo(user.Certifications);

            await _context.SaveChangesAsync();

            return Ok(user.Certifications);
        }

        [HttpPatch("{id}/Loas/{loaId}")]
        public async Task<IActionResult> PatchLoa(int id, int loaId, [FromBody] JsonPatchDocument<Loas> data)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound("User not found");

            var loa = user.Loas.FirstOrDefault(x => x.Id == loaId);

            if (loa == null)
                return NotFound("Loa not found");

            data.ApplyTo(loa);

            await _context.SaveChangesAsync();

            return Ok(loa);
        }

        [HttpPatch("{id}/Warnings/{warningId}")]
        public async Task<IActionResult> PatchLoa(int id, int warningId, [FromBody] JsonPatchDocument<Warnings> data)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound("User not found");

            var warning = user.Warnings.FirstOrDefault(x => x.Id == warningId);

            if (warning == null)
                return NotFound("Warning not found");

            data.ApplyTo(warning);

            await _context.SaveChangesAsync();

            return Ok(warning);
        }

        [HttpPut]
        public async Task<IActionResult> PutUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPut("{id}/Loa")]
        public async Task<IActionResult> PutLoa(int id, [FromBody] Loas loa)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound("User not found");

            if (user.Loas == null)
                user.Loas = new List<Loas> {loa};
            else
                user.Loas.Add(loa);

            await _context.SaveChangesAsync();

            return Ok(loa);
        }

        [HttpPut("{id}/Warning")]
        public async Task<IActionResult> PutWarning(int id, [FromBody] Warnings warning)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound("User not found");

            if (user.Warnings == null)
                user.Warnings = new List<Warnings> {warning};
            else
                user.Warnings.Add(warning);

            await _context.SaveChangesAsync();

            return Ok(warning);
        }

        [HttpPut("{id}/Roles")]
        public async Task<IActionResult> PutRole(int id, [FromBody] UserRole data)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound("User not found");

            user.Role = data;

            await _context.SaveChangesAsync();

            return Ok(data);
        }

        [HttpDelete("{id}/Loas/{loaId}")]
        public async Task<IActionResult> DeleteLoa(int id, int loaId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound("User not found");

            var loa = user.Loas.FirstOrDefault(x => x.Id == loaId);

            if (loa == null)
                return NotFound("Loa not found");

            user.Loas.Remove(loa);

            await _context.SaveChangesAsync();

            return Ok(loa);
        }

        [HttpDelete("{id}/Warnings/{warningId}")]
        public async Task<IActionResult> DeleteWarning(int id, int warningId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound("User not found");

            var warning = user.Warnings.FirstOrDefault(x => x.Id == warningId);

            if (warning == null)
                return NotFound("Warning not found");

            user.Warnings.Remove(warning);

            await _context.SaveChangesAsync();

            return Ok(warning);
        }

        [HttpDelete("{id}/Roles/{roleId}")]
        public async Task<IActionResult> DeleteRole(int id, int roleId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound("User not found");

            var role = user.Warnings.FirstOrDefault(x => x.Id == roleId);

            if (role == null)
                return NotFound("Role not found");

            user.Warnings.Remove(role);

            await _context.SaveChangesAsync();

            return Ok(role);
        }
    }
}