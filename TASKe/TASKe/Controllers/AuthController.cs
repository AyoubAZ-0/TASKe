using Microsoft.AspNetCore.Mvc;

namespace TASKe.Controllers
{
    public class AuthDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Role { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _auth;

        public AuthController(AuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthDto request)
        {
            var user = await _auth.Register(request.Email, request.Password, request.Role ?? "User");
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto request)
        {
            var user = await _auth.Login(request.Email, request.Password);
            if (user == null) return Unauthorized();
            return Ok(user);
        }
    }
}
