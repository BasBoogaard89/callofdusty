namespace Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    IUserService userService,
    IMapper mapper) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        var dto = mapper.Map<RegisterDto>(req);

        var data = await userService.Register(dto);

        return Ok(data);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var dto = mapper.Map<LoginDto>(req);

        var data = await userService.Login(dto);

        Response.Cookies.Append("cod_token", data, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(15)
        });

        return Ok(new { success = true });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("cod_token");
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            email = User.Identity?.Name,
            roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value)
        });
    }
}

