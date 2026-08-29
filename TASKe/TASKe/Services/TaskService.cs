using Microsoft.EntityFrameworkCore;
using TASKe.Data;
using TASKe.Models;

public class TaskService
{
    private readonly MyAppContext _context;

    public TaskService(MyAppContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Taskitem>> GetTasks(Guid userId, string role)
    {
        if (role == "Admin")
        {
            return await _context.Tasks.ToListAsync();
        }

        return await _context.Tasks.Where(t => t.AssignedToUserId == userId).ToListAsync();
    }

    public async Task<Taskitem> CreateTask(string title, string desc, Guid userId)
    {
        var task = new Taskitem
        {
            Title = title,
            Description = desc,
            AssignedToUserId = userId,
            Status = "NotStarted"
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<bool> UpdateStatus(Guid taskId, Guid userId, string newStatus)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null) return false;

        if (task.AssignedToUserId != userId)
            return false;

        if (task.Status == "NotStarted" && newStatus == "Ongoing")
            task.Status = "Ongoing";
        else if (task.Status == "Ongoing" && newStatus == "Done")
            task.Status = "Done";
        else
            return false;

        await _context.SaveChangesAsync();
        return true;
    }
}