using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;

namespace ProjectManagement.DataAccess.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Project> Projects { get; set; }
        DbSet<Task> Tasks { get; set; }
        DbSet<User> Users { get; set; }
    }
}
