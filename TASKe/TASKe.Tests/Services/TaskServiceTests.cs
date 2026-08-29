using Microsoft.EntityFrameworkCore;
using TASKe.Data;
using TASKe.Models;
using Xunit;

namespace TASKe.Tests.Services
{
    public class TaskServiceTests
    {
        private static MyAppContext CreateInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<MyAppContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new MyAppContext(options);
        }

        [Fact]
        public async Task CreateTask_ValidInput_CreatesTaskWithNotStartedStatus()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);
            var taskService = new TaskService(context);

            var userId = Guid.NewGuid();
            var title = "Build Frontend";
            var desc = "Complete authentication UI";

            var createdTask = await taskService.CreateTask(title, desc, userId);

            Assert.NotNull(createdTask);
            Assert.NotEqual(Guid.Empty, createdTask.Id);
            Assert.Equal(title, createdTask.Title);
            Assert.Equal(desc, createdTask.Description);
            Assert.Equal(userId, createdTask.AssignedToUserId);
            Assert.Equal("NotStarted", createdTask.Status);

            var dbTask = await context.Tasks.FindAsync(createdTask.Id);
            Assert.NotNull(dbTask);
            Assert.Equal(title, dbTask.Title);
        }

        [Fact]
        public async Task GetTasks_WhenRoleIsAdmin_ReturnsAllTasks()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);

            var user1Id = Guid.NewGuid();
            var user2Id = Guid.NewGuid();

            context.Tasks.AddRange(
                new Taskitem { Id = Guid.NewGuid(), Title = "Task 1", Description = "Desc 1", AssignedToUserId = user1Id, Status = "NotStarted" },
                new Taskitem { Id = Guid.NewGuid(), Title = "Task 2", Description = "Desc 2", AssignedToUserId = user2Id, Status = "Ongoing" }
            );
            await context.SaveChangesAsync();

            var taskService = new TaskService(context);
            var tasks = (await taskService.GetTasks(user1Id, "Admin")).ToList();

            Assert.Equal(2, tasks.Count);
        }

        [Fact]
        public async Task GetTasks_WhenRoleIsUser_ReturnsOnlyAssignedTasks()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);

            var user1Id = Guid.NewGuid();
            var user2Id = Guid.NewGuid();

            context.Tasks.AddRange(
                new Taskitem { Id = Guid.NewGuid(), Title = "User 1 Task", Description = "Desc 1", AssignedToUserId = user1Id, Status = "NotStarted" },
                new Taskitem { Id = Guid.NewGuid(), Title = "User 2 Task", Description = "Desc 2", AssignedToUserId = user2Id, Status = "Ongoing" }
            );
            await context.SaveChangesAsync();

            var taskService = new TaskService(context);
            var tasks = (await taskService.GetTasks(user1Id, "User")).ToList();

            Assert.Single(tasks);
            Assert.Equal("User 1 Task", tasks[0].Title);
            Assert.Equal(user1Id, tasks[0].AssignedToUserId);
        }

        [Fact]
        public async Task UpdateStatus_ValidTransition_NotStartedToOngoing_ReturnsTrue()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);

            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            context.Tasks.Add(new Taskitem
            {
                Id = taskId,
                Title = "Task A",
                Description = "Desc A",
                AssignedToUserId = userId,
                Status = "NotStarted"
            });
            await context.SaveChangesAsync();

            var taskService = new TaskService(context);
            var result = await taskService.UpdateStatus(taskId, userId, "Ongoing");

            Assert.True(result);
            var updatedTask = await context.Tasks.FindAsync(taskId);
            Assert.NotNull(updatedTask);
            Assert.Equal("Ongoing", updatedTask.Status);
        }

        [Fact]
        public async Task UpdateStatus_ValidTransition_OngoingToDone_ReturnsTrue()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);

            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            context.Tasks.Add(new Taskitem
            {
                Id = taskId,
                Title = "Task B",
                Description = "Desc B",
                AssignedToUserId = userId,
                Status = "Ongoing"
            });
            await context.SaveChangesAsync();

            var taskService = new TaskService(context);
            var result = await taskService.UpdateStatus(taskId, userId, "Done");

            Assert.True(result);
            var updatedTask = await context.Tasks.FindAsync(taskId);
            Assert.NotNull(updatedTask);
            Assert.Equal("Done", updatedTask.Status);
        }

        [Fact]
        public async Task UpdateStatus_InvalidTransition_NotStartedToDone_ReturnsFalse()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);

            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            context.Tasks.Add(new Taskitem
            {
                Id = taskId,
                Title = "Task C",
                Description = "Desc C",
                AssignedToUserId = userId,
                Status = "NotStarted"
            });
            await context.SaveChangesAsync();

            var taskService = new TaskService(context);
            var result = await taskService.UpdateStatus(taskId, userId, "Done");

            Assert.False(result);
            var taskInDb = await context.Tasks.FindAsync(taskId);
            Assert.NotNull(taskInDb);
            Assert.Equal("NotStarted", taskInDb.Status);
        }

        [Fact]
        public async Task UpdateStatus_WhenUserNotAssigned_ReturnsFalse()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);

            var assignedUserId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            context.Tasks.Add(new Taskitem
            {
                Id = taskId,
                Title = "Task D",
                Description = "Desc D",
                AssignedToUserId = assignedUserId,
                Status = "NotStarted"
            });
            await context.SaveChangesAsync();

            var taskService = new TaskService(context);
            var result = await taskService.UpdateStatus(taskId, otherUserId, "Ongoing");

            Assert.False(result);
            var taskInDb = await context.Tasks.FindAsync(taskId);
            Assert.NotNull(taskInDb);
            Assert.Equal("NotStarted", taskInDb.Status);
        }

        [Fact]
        public async Task UpdateStatus_WhenTaskDoesNotExist_ReturnsFalse()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateInMemoryDbContext(dbName);
            var taskService = new TaskService(context);

            var result = await taskService.UpdateStatus(Guid.NewGuid(), Guid.NewGuid(), "Ongoing");

            Assert.False(result);
        }
    }
}
