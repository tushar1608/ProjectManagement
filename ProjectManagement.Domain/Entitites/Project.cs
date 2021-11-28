using ProjectManagement.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class Project : AuditableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
    }
}

