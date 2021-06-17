using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ZDC.Core.Auth;
using ZDC.Core.Data;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ZdcContext _context;

        public AuthController(ZdcContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("token")]
        public async Task<ActionResult<string>> Login(string code)
        {
            var client = new HttpClient();

            var oauth = new Oauth
            {
                ClientId = _config.GetValue<string>("ConnectClientId"),
                ClientSecret = _config.GetValue<string>("ConnectClientSecret"),
                RedirectUri = _config.GetValue<string>("ConnectRedirectUrl"),
                Code = code
            };

            var url = _config.GetValue<string>("ConnectToken");
            var tokenResponse = await client.PostAsJsonAsync(url, oauth);
            var content = await tokenResponse.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            var details = JsonSerializer.Deserialize<TokenResponse>(content);

            if (details == null)
                return StatusCode(500, "Error communicating with VATSIM connect");

            url = _config.GetValue<string>("ConnectUser");
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {details.AccessToken}");
            var userResponse = await client.GetAsync(url);
            content = await userResponse.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            var userDetails = JsonSerializer.Deserialize<UserResponse>(content);

            if (userDetails == null || !userDetails.Data.Oauth.ValidToken.Equals("true"))
                return NotFound("User details invalid");
            var user = _context.Users
                .Include(x => x.Roles)
                .FirstOrDefault(x => x.Id == int.Parse(userDetails.Data.Cid));
            var claims = new List<Claim>
            {
                new("cid", userDetails.Data.Cid),
                new("firstName", userDetails.Data.Personal.FirstName),
                new("lastName", userDetails.Data.Personal.LastName),
                new("rating", userDetails.Data.Vatsim.Rating.Id.ToString()),
                new("isMember", user == null ? false.ToString() : true.ToString())
            };
            if (user?.Roles != null && user.Roles.Any())
            {
                var roles = user.Roles.ToList();
                claims.AddRange(roles.Select(role => new Claim("roles", role.Name)));
            }

            var authSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JwtSecret")));
            var token = new JwtSecurityToken(
                _config.GetValue<string>("JwtIssuer"),
                _config.GetValue<string>("JwtAudience"),
                expires: DateTime.UtcNow.AddMonths(1),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}