using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Core.Dtos;
using ZDC.Core.Extensions;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly ZdcContext _context;
        private readonly IMapper _mapper;

        public UsersController(ZdcContext content, IMapper mapper)
        {
            _context = content;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IList<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Include(x => x.Certifications)
                .Include(x => x.Roles)
                .Where(x => x.Status != UserStatus.Removed)
                .OrderBy(x => x.LastName).ToListAsync();
            if (await User.IsStaff(_context) || await User.IsTrainingStaff(_context))
                return Ok(_mapper.Map<IList<User>, IList<UserStaffDto>>(users));
            return Ok(_mapper.Map<IList<User>, IList<UserDto>>(users));
        }

        [HttpGet("stats")]
        public async Task<ActionResult<IList<StatsDto>>> GetStats(int year = 0, int month = 0)
        {
            if (year == 0 || month == 0)
            {
                year = DateTime.UtcNow.Year;
                month = DateTime.UtcNow.Month;
            }

            var users = await _context.Users.Where(x => x.Status != UserStatus.Removed).ToListAsync();
            var stats = _mapper.Map<IList<User>, IList<StatsDto>>(users);
            foreach (var entry in stats)
            {
                var hours = await _context.Hours
                    .Where(x => x.User.Id == entry.Id)
                    .FirstOrDefaultAsync(x => x.Year == year && x.Month == month);
                if (hours != null)
                {
                    var time = TimeSpan.FromHours(hours.TotalHours);
                    Console.Write($"{time.Hours}h {time.Minutes}m");
                    entry.Hours = _mapper.Map<Hours, HoursDto>(hours);
                }
            }

            return Ok(stats.OrderByDescending(x => x.Hours?.TotalHours));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(x => x.Certifications)
                .Include(x => x.Roles)
                .Where(x => x.Status != UserStatus.Removed)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return NotFound($"User {id} not found");
            if (await User.IsStaff(_context) || await User.IsTrainingStaff(_context))
                return Ok(user);
            return Ok(_mapper.Map<User, UserDto>(user));
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,EC,EC,FE,ATA,AWM,AEC,AFE,INS,MTR")]
        [HttpPut]
        public async Task<ActionResult<User>> PutUser([FromBody] User data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid user");
            var user = await _context.Users.FindAsync(data.Id);
            if (user == null)
                return NotFound($"User {data.Id} not found");
            _context.Users.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM")]
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] User data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid user");
            var user = await _context.Users.FindAsync(data.Id);
            if (user == null)
                return BadRequest($"User {data.Id} already exists");
            await _context.Users.AddAsync(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        [HttpGet("Roles")]
        public async Task<ActionResult<IList<Role>>> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpPut("{id}/Role/{roleid}")]
        public async Task<ActionResult<User>> PostUserRole(int id, int roleId)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound($"User {id} not found");
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
                return NotFound($"Role {id} not found");
            user.Roles.Add(role);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("{id}/Role/{roleId}")]
        public async Task<ActionResult<User>> DeleteUserRole(int id, int roleId)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound($"User {id} not found");
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
                return NotFound($"Role {id} not found");
            user.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}