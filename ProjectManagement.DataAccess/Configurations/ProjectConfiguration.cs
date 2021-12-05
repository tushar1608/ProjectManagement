using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;

namespace ProjectManagement.DataAccess.Configurations
{
    public class ProjectConfiguration: IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {

            builder.Property(t => t.Id)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Detail)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(t => t.CreatedOn)
                .IsRequired();

            builder.HasOne(a => a.Task)
                .WithOne(b => b.Project)
                .HasForeignKey<Task>(b => b.ProjectId);
        }
    }
}
