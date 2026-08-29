using Microsoft.AspNetCore.Mvc;

namespace TASKe.Controllers
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
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

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] Guid userId, [FromQuery] string role)
        {
            var tasks = await _service.GetTasks(userId, role);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto request)
        {
            var task = await _service.CreateTask(request.Title, request.Desc, request.UserId);
            return Ok(task);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] Guid userId, [FromQuery] string status)
        {
            var result = await _service.UpdateStatus(id, userId, status);
            if (!result) return BadRequest();
            return Ok();
        }
    }
}
