using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TASKe.Controllers;
using TASKe.Data;
using TASKe.Models;
using Xunit;

namespace TASKe.Tests.Controllers
{
    public class UsersControllerTests
    {
        private static MyAppContext CreateInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<MyAppContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new MyAppContext(options);
        }

        [Fact]
        public async Task GetUsers_ReturnsOnlyUsersWithRoleUser()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);

            context.Users.AddRange(
                new User { Id = Guid.NewGuid(), Email = "user1@test.com", PasswordHash = "hash1", Role = "User" },
                new User { Id = Guid.NewGuid(), Email = "user2@test.com", PasswordHash = "hash2", Role = "User" },
                new User { Id = Guid.NewGuid(), Email = "admin@test.com", PasswordHash = "hashAdmin", Role = "Admin" }
            );
            await context.SaveChangesAsync();

            var controller = new UsersController(context);
            var actionResult = await controller.GetUsers();

            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var usersList = Assert.IsAssignableFrom<System.Collections.IEnumerable>(okResult.Value);

            int count = 0;
            foreach (var item in usersList)
            {
                count++;
            }

            Assert.Equal(2, count);
        }
    }
}
