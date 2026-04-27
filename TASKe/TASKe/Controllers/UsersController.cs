using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TASKe.Data;

namespace TASKe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyAppContext _context;

        public UsersController(MyAppContext context)
        {
            _context = context;
        }

        [HttpGet("assignable")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Where(u => u.Role == "User")
                .Select(u => new
                {
                    u.Id,
                    u.Email
                })
                .ToListAsync();

            return Ok(users);
        }

    }
}
