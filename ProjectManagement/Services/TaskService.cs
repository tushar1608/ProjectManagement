using ProjectManagement.Web.Interfaces;
using ProjectManagement.Web.Models;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManagement.Web.Services
{

    public class TaskService : ITaskService
    {
        private IList<Task> Tasks;

        public TaskService()
        {
            this.Tasks = new List<Task>();
        }

        public async System.Threading.Tasks.Task<string> CreateTask(TaskCreationRequest taskRequest)
        {
            var task = new Task { Id = Guid.NewGuid().ToString(), AssignedToUserId = taskRequest.AssignedToUserId, Detail = taskRequest.Detail, ProjectId = taskRequest.ProjectId, Status = Status.New, CreatedOn = DateTime.Now };
            await System.Threading.Tasks.Task.Run(() =>
            {
                Tasks.Add(task);
            });
            return $"Task created with id:{task.Id}";
        }

        public async System.Threading.Tasks.Task<List<TaskInformation>> GetAllTasks()
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var response = new List<TaskInformation>();
                foreach (var task in Tasks)
                {
                    response.Add(new TaskInformation { Id = task.Id, AssignedToUserId = task.AssignedToUserId, Detail = task.Detail, ProjectId = task.ProjectId, Status = (int)task.Status, CreatedOn = task.CreatedOn.ToString() });
                }
                return response;
            });
        }

        public async System.Threading.Tasks.Task<TaskInformation> GetTask(string id)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var task = Tasks.FirstOrDefault(p => p.Id == id);
                if (task == null)
                {
                    return null;
                }
                return new TaskInformation { Id = task.Id, AssignedToUserId = task.AssignedToUserId, Detail = task.Detail, ProjectId = task.ProjectId, Status = (int)task.Status, CreatedOn = task.CreatedOn.ToString() };
            });
        }

        public async System.Threading.Tasks.Task<string> UpdateTask(TaskUpdateRequest taskUpdateRequest)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var task = Tasks.FirstOrDefault(x => x.Id == taskUpdateRequest.Id);
                if (task != null)
                {
                    task.Detail = taskUpdateRequest.Detail;
                    task.AssignedToUserId = taskUpdateRequest.AssignedToUserId;
                    task.ProjectId = taskUpdateRequest.ProjectId;
                    task.Status = taskUpdateRequest.Status;
                    return task.Id;
                }
                else
                {
                    return null;
                }
            });
        }

        public async System.Threading.Tasks.Task<bool> DeleteTask(string id)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    var TaskToRemove = Tasks.Single(x => x.Id == id);
                    Tasks.Remove(TaskToRemove);
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

