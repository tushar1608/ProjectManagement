using System;

namespace ProjectManagement.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedOn { get; set; }
    }
}

