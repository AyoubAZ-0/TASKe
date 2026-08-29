using Microsoft.EntityFrameworkCore;
using TASKe.Data;
using TASKe.Models;
using Xunit;

namespace TASKe.Tests.Services
{
    public class AuthServiceTests
    {
        private static MyAppContext CreateInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<MyAppContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new MyAppContext(options);
        }

        [Fact]
        public async Task Register_ValidInput_CreatesAndReturnsUser()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);
            var authService = new AuthService(context);

            var email = "test@example.com";
            var password = "password123";

            var createdUser = await authService.Register(email, password);

            Assert.NotNull(createdUser);
            Assert.Equal(email, createdUser.Email);
            Assert.Equal(password, createdUser.PasswordHash);
            Assert.Equal("User", createdUser.Role);

            var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            Assert.NotNull(dbUser);
            Assert.Equal(createdUser.Id, dbUser.Id);
        }

        [Fact]
        public async Task Register_WithCustomRole_CreatesUserWithSpecifiedRole()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);
            var authService = new AuthService(context);

            var createdUser = await authService.Register("admin@example.com", "adminPass", "Admin");

            Assert.NotNull(createdUser);
            Assert.Equal("Admin", createdUser.Role);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsMatchingUser()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "john@example.com",
                PasswordHash = "secret456",
                Role = "User"
            };
            context.Users.Add(existingUser);
            await context.SaveChangesAsync();

            var authService = new AuthService(context);
            var loggedInUser = await authService.Login("john@example.com", "secret456");

            Assert.NotNull(loggedInUser);
            Assert.Equal(existingUser.Id, loggedInUser.Id);
            Assert.Equal("john@example.com", loggedInUser.Email);
        }

        [Fact]
        public async Task Login_WrongPassword_ReturnsNull()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "john@example.com",
                PasswordHash = "secret456",
                Role = "User"
            };
            context.Users.Add(existingUser);
            await context.SaveChangesAsync();

            var authService = new AuthService(context);
            var result = await authService.Login("john@example.com", "wrongPassword");

            Assert.Null(result);
        }

        [Fact]
        public async Task Login_NonExistentEmail_ReturnsNull()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);
            var authService = new AuthService(context);

            var result = await authService.Login("nonexistent@example.com", "anyPassword");

            Assert.Null(result);
        }
    }
}
