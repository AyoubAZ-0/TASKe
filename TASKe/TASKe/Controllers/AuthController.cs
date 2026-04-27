using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TASKe.Controllers
{
    public class AuthDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
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
            var user = await _auth.Register(request.Email, request.Password);
            // Optionally set Role if the service supports it
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
