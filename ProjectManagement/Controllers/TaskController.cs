using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectManagement.Web.Interfaces;
using ProjectManagement.Web.Models;
using System.Threading.Tasks;

namespace ProjectManagement.Web.Controllers
{
    [Authorize]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        [HttpPost]
        [Route("api/Task")]
        public async Task<IActionResult> Create(TaskCreationRequest task)
        {
            _logger.LogInformation("Request to create new task", task.ProjectId, task.AssignedToUserId);
            return Ok(await _taskService.CreateTask(task));
        }

        [HttpPut]
        [Route("api/Task")]
        public async Task<IActionResult> Update(TaskUpdateRequest task)
        {
            _logger.LogInformation($"Request to update task {task.Id}");
            if (await _taskService.UpdateTask(task) == null)
            {
                return NotFound($"No entry for task id: {task.Id} found to update");
            }
            return Ok($"Update successful for task with id: {task.Id}");
        }

        [HttpGet]
        [Route("api/Task")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("All tasks requested");
            return Ok(await _taskService.GetAllTasks());
        }

        [HttpGet]
        [Route("api/Task/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogInformation($"Request to get task with id: {id}");
            var task = await _taskService.GetTask(id);
            if (task == null)
            {
                return NotFound($"No task found with id {id}");
            }
            return Ok(task);
        }

        [HttpDelete]
        [Route("api/Task/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation($"Request to delete task with id: {id}");
            var isDeleteSuccessful = await _taskService.DeleteTask(id);
            if (isDeleteSuccessful == false)
            {
                return NotFound($"No task found with id {id}");
            }
            return Ok($"Task with id: {id} is deleted");
        }
    }
}

