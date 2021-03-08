using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ZDC.Core.Data;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ZdcContext _context;

        public AuthController(ZdcContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("Login/{code}")]
        public async Task<ActionResult> Login(string code)
        {
            var data = new Dictionary<string, string>
            {
                {"grant_type", "authorization_code"},
                {"client_id", _config.GetValue<string>("ConnectClientId")},
                {"client_secret", _config.GetValue<string>("ConnectClientSecret")},
                {"redirect_url", _config.GetValue<string>("ConnectRedirectUrl")},
                {"code", code}
            };

            var content = new FormUrlEncodedContent(data);

            using var httpClient = new HttpClient();
            using var response = await httpClient.PostAsync("https://auth.vatsim.net/oauth/token/", content);

            if (!response.IsSuccessStatusCode)
                return Unauthorized($"Invalid login: {code}");

            return Ok(response);
        }
    }
}