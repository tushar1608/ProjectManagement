

namespace ProjectManagement.Web.Models
{
    public class TaskInformation
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public int Status { get; set; }
        public string AssignedToUserId { get; set; }
        public string Detail { get; set; }
        public string CreatedOn { get; set; }
    }
}

