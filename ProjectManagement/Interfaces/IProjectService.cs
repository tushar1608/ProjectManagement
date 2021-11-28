using ProjectManagement.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Web.Interfaces
{
    public interface IProjectService
    {
        public Task<string> CreateProject(ProjectCreationRequest project);
        public Task<string> UpdateProject(ProjectUpdateRequest updateProjectRequest);
        public Task<List<ProjectInformation>> GetAllProjects();
        public Task<ProjectInformation> GetProject(string id);
        public Task<bool> DeleteProject(string id);
    }
}