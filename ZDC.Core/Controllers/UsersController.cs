using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            return _context.Users;
        }

        [HttpGet("{id}")]
        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        [HttpGet("Certification/{id}")]
        public Certification GetUserCertification(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            return user?.Certifications;
        }

        [HttpGet("Loas/{id}")]
        public IList<Loas> GetLoas(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            return user?.Loas;
        }

        [HttpGet("Warnings/{id}")]
        public IList<Warnings> GetWarnings(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            return user?.Warnings;
        }

        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}