using ProjectManager.Domain.Enums;

namespace ProjectManagement.Web.Models
{
    public class TaskUpdateRequest
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public Status Status { get; set; }
        public string AssignedToUserId { get; set; }
        public string Detail { get; set; }
    }
}

