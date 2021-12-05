using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectManagement.DataAccess.Interfaces;
using ProjectManagement.Domain.Common;
using ProjectManager.Domain.Entities;
using Task = ProjectManager.Domain.Entities.Task;

namespace ProjectManagement.DataAccess
{
    public class ApplicationDbContext: DbContext, IApplicationDbContext
    {
        private readonly IDateTime _dateTime;
        public ApplicationDbContext(DbContextOptions options, IDateTime dateTime): base(options)
        {
            _dateTime = dateTime;
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTime.Now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
            builder.Entity<Project>().ToTable("Project");
            builder.Entity<Task>().ToTable("Task");
            builder.Entity<User>().ToTable("User");
        }
    }
}
