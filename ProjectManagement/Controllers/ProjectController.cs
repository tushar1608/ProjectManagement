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
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectService _projectService;

        public ProjectController(ILogger<ProjectController> logger, IProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        [HttpPost]
        [Route("api/Project")]
        public async Task<IActionResult> Create(ProjectCreationRequest project)
        {
            _logger.LogInformation("Request to create new project", project.Name);
            return Ok(await _projectService.CreateProject(project));
        }

        [HttpPut]
        [Route("api/Project")]
        public async Task<IActionResult> Update(ProjectUpdateRequest project)
        {
            _logger.LogInformation($"Request to update project {project.Id}");
            if (await _projectService.UpdateProject(project) == null)
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
            return Ok(await _projectService.GetAllProjects());
        }

        [HttpGet]
        [Route("api/Project/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogInformation($"Request to get project with id: {id}");
            var project = await _projectService.GetProject(id);
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
            var isDeleteSuccessful = await _projectService.DeleteProject(id);
            if (isDeleteSuccessful == false)
            {
                return NotFound($"No project found with id {id}");
            }
            return Ok($"project with id: {id} is deleted");
        }
    }
}

