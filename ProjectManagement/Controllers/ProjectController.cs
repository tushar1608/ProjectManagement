using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectManagement.Repository;
using ProjectManagement.Web.Models;
using ProjectManager.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ProjectManagement.Web.Controllers
{
    [Authorize]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IRepository<Project> _repository;

        public ProjectController(ILogger<ProjectController> logger, IRepository<Project> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost]
        [Route("api/Project")]
        public async Task<IActionResult> Create(ProjectCreationRequest project)
        {
            _logger.LogInformation("Request to create new project", project.Name);
            var projectEntity = new Project { Id = Guid.NewGuid().ToString(), Detail = project.Detail, Name = project.Name };
            return Ok(_repository.Add(projectEntity));
        }

        [HttpPut]
        [Route("api/Project")]
        public async Task<IActionResult> Update(ProjectUpdateRequest project)
        {
            _logger.LogInformation($"Request to update project {project.Id}");
            var projectEntity = new Project { Id = project.Id, Detail = project.Detail, Name = project.Name };
            if (_repository.Update(projectEntity) == null)
            {
                return NotFound($"No entry for project id: {project.Id} found to update");
            }
            return Ok($"Update successful for project with id: {project.Id}");
        }

        [HttpGet]
        [Route("api/Project")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("All projects requested");
            return Ok(_repository.All());
        }

        [HttpGet]
        [Route("api/Project/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogInformation($"Request to get project with id: {id}");
            var project = _repository.Get(id);
            if (project == null)
            {
                return NotFound($"No project found with id {id}");
            }
            return Ok(project);
        }

        [HttpDelete]
        [Route("api/Project/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation($"Request to delete project with id: {id}");
            var deletedEntity = _repository.Delete(id);
            if (deletedEntity == null)
            {
                return NotFound($"No project found with id {id}");
            }
            return Ok($"project with id: {id} is deleted");
        }
    }
}

