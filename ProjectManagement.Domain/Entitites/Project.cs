using System.Text.Json.Serialization;
using ProjectManagement.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class Project : AuditableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }

        //Navigation props ef core
        [JsonIgnore]
        public Task Task { get; set; }
    }
}

