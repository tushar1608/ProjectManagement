using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;

namespace ProjectManagement.DataAccess.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.Detail)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.Status)
                .IsRequired();
        }
    }
}
