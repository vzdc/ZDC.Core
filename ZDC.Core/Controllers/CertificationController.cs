using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZDC.Core.Data;
using ZDC.Core.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    public class CertificationController : Controller
    {
        private readonly ZdcContext _context;

        public CertificationController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public Certification GetCertification(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            return user?.Certifications;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCertification(int id, [FromBody] Certification certification)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null) return NotFound();

            user.Certifications = certification;
            user.Certifications.Updated = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}