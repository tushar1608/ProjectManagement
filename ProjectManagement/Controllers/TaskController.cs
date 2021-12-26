using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectManagement.Repository;
using ProjectManagement.Web.Models;
using ProjectManager.Domain.Entities;

namespace ProjectManagement.Web.Controllers
{
    [Authorize]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly IRepository<Task> _repository;

        public TaskController(ILogger<TaskController> logger, IRepository<Task> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost]
        [Route("api/Task")]
        public async System.Threading.Tasks.Task<IActionResult> Create(TaskCreationRequest task)
        {
            _logger.LogInformation("Request to create new task", task.ProjectId, task.AssignedToUserId);
            var taskEntity = new Task {Id = Guid.NewGuid().ToString(), AssignedToUserId = task.AssignedToUserId, Detail = task.Detail, ProjectId = task.ProjectId };
            return Ok(_repository.Add(taskEntity));
        }

        [HttpPut]
        [Route("api/Task")]
        public async System.Threading.Tasks.Task<IActionResult> Update(TaskUpdateRequest task)
        {
            _logger.LogInformation($"Request to update task {task.Id}");
            var taskEntity = new Task { Id = task.Id, AssignedToUserId = task.AssignedToUserId, Detail = task.Detail, ProjectId = task.ProjectId };
            if (_repository.Update(taskEntity) == null)
            {
                return NotFound($"No entry for task id: {task.Id} found to update");
            }
            return Ok($"Update successful for task with id: {task.Id}");
        }

        [HttpGet]
        [Route("api/Task")]
        public async System.Threading.Tasks.Task<IActionResult> GetAll()
        {
            _logger.LogInformation("All tasks requested");
            return Ok(_repository.All());
        }

        [HttpGet]
        [Route("api/Task/{id}")]
        public async System.Threading.Tasks.Task<IActionResult> Get(string id)
        {
            _logger.LogInformation($"Request to get task with id: {id}");
            var task = _repository.Get(id);
            if (task == null)
            {
                return NotFound($"No task found with id {id}");
            }
            return Ok(task);
        }

        [HttpDelete]
        [Route("api/Task/{id}")]
        public async System.Threading.Tasks.Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation($"Request to delete task with id: {id}");
            var deletedEntity = _repository.Delete(id);
            if (deletedEntity == null)
            {
                return NotFound($"No task found with id {id}");
            }
            return Ok($"Task with id: {id} is deleted");
        }
    }
}

