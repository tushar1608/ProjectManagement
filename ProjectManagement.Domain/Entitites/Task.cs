
using System.Text.Json.Serialization;
using ProjectManagement.Domain.Common;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Domain.Entities
{
    public class Task : AuditableEntity
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public Status Status { get; set; }
        public string AssignedToUserId { get; set; }
        public string Detail { get; set; }

        //Navigation property ef core
        [JsonIgnore]
        public Project Project { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}

