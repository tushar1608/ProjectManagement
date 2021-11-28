namespace ProjectManagement.Web.Models
{
    public class TaskCreationRequest
    {
        public string ProjectId { get; set; }
        public string AssignedToUserId { get; set; }
        public string Detail { get; set; }
    }
}

