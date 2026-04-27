using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TASKe.Models;

namespace TASKe.Controllers
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public Guid UserId { get; set; }
    }

    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _service;

        public TaskController(TaskService service)
        {
            _service = service;
        }

        // Get tasks based on user role
        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] Guid userId, [FromQuery] string role)
        {
            var tasks = await _service.GetTasks(userId, role);
            return Ok(tasks);
        }

        // Admin assigns task
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto request)
        {
            var task = await _service.CreateTask(request.Title, request.Desc, request.UserId);
            return Ok(task);
        }

        // User updates status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] Guid userId, [FromQuery] string status)
        {
            var result = await _service.UpdateStatus(id, userId, status);
            if (!result) return BadRequest();
            return Ok();
        }

    }
}
