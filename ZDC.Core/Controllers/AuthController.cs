namespace ZDC.Core.Controllers
{
    // [Route("api/[controller]")]
    // public class AuthController : AuthControllerBase
    // {
    //     private readonly ZdcContext _context;
    //     private readonly VatsimServerOptions _vatsimOptions;
    //
    //     public AuthController(IVatsimConnectService vatsimService, IJwtAuthManager jwtAuthManager,
    //         IVatsimAuthenticationService authenticationService, IOptions<VatsimServerOptions> vatsimOptions,
    //         ZdcContext context)
    //         : base(vatsimService, jwtAuthManager, authenticationService)
    //     {
    //         _vatsimOptions = vatsimOptions?.Value ?? throw new ArgumentNullException(nameof(vatsimOptions));
    //         _context = context;
    //     }
    //
    //     /// <summary>
    //     ///     Redirects to VATSIM Connect authorization endpoint
    //     /// </summary>
    //     /// <returns>302 Found</returns>
    //     [HttpGet("login")]
    //     [AllowAnonymous]
    //     [ProducesResponseType(StatusCodes.Status302Found)]
    //     public async Task<IActionResult> RedirectToVatsimConnect()
    //     {
    //         await Task.CompletedTask;
    //         return Redirect(
    //             $"{_vatsimOptions.VatsimTokenRequestOptions.VatsimAuthUri}/oauth/authorize?client_id={_vatsimOptions.VatsimTokenRequestOptions.ClientId}&redirect_uri={_vatsimOptions.VatsimTokenRequestOptions.RedirectUri}&response_type=code&scope=full_name+vatsim_details+email+country");
    //     }
    // }
}