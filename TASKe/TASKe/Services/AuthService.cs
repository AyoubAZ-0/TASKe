using Microsoft.EntityFrameworkCore;
using TASKe.Data;
using TASKe.Models;

public class AuthService
{
    private readonly MyAppContext _context;

    public AuthService(MyAppContext context)
    {
        _context = context;
    }

    public async Task<User> Register(string email, string password, string role = "User")
    {
        var user = new User
        {
            Email = email,
            PasswordHash = password,
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> Login(string email, string password)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);
    }
}