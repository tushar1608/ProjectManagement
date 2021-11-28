using ProjectManagement.Web.Interfaces;
using ProjectManagement.Web.Models;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Web.Services
{
    public class ProjectService : IProjectService
    {
        private IList<Project> Projects;

        public ProjectService()
        {
            this.Projects = new List<Project>();
        }

        public async Task<string> CreateProject(ProjectCreationRequest projectCreationRequest)
        {
            var project = new Project { Id = Guid.NewGuid().ToString(), Detail = projectCreationRequest.Detail, Name = projectCreationRequest.Name, CreatedOn = DateTime.Now };
            await System.Threading.Tasks.Task.Run(() => {

                Projects.Add(project);

            });
            return $"Project created with id: {project.Id}";
        }

        public async Task<List<ProjectInformation>> GetAllProjects()
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var response = new List<ProjectInformation>();
                foreach (var project in Projects)
                {
                    response.Add(new ProjectInformation { Id = project.Id, Detail = project.Detail, Name = project.Name, CreatedOn = project.CreatedOn.ToString() });
                }
                return response;
            });
        }

        public async Task<ProjectInformation> GetProject(string id)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var match = Projects.FirstOrDefault(p => p.Id == id);
                if (match == null)
                {
                    return null;
                }
                return new ProjectInformation { Id = match.Id, Detail = match.Detail, Name = match.Name, CreatedOn = match.CreatedOn.ToString() };
            });
        }

        public async Task<string> UpdateProject(ProjectUpdateRequest updateProjectRequest)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var project = Projects.FirstOrDefault(x => x.Id == updateProjectRequest.Id);
                if (project != null)
                {
                    project.Name = updateProjectRequest.Name;
                    project.Detail = updateProjectRequest.Detail;
                    return project.Id;
                }
                else
                {
                    return null;
                }
            });
        }

        public async Task<bool> DeleteProject(string id)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    var ProjectToRemove = Projects.Single(x => x.Id == id);
                    Projects.Remove(ProjectToRemove);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}


