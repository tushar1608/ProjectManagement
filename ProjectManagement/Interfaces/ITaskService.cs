using ProjectManagement.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Web.Interfaces
{
    public interface ITaskService
    {
        public Task<string> CreateTask(TaskCreationRequest task);
        public Task<string> UpdateTask(TaskUpdateRequest updateUserRequest);
        public Task<List<TaskInformation>> GetAllTasks();
        public Task<bool> DeleteTask(string id);
        public Task<TaskInformation> GetTask(string id);
    }
}

