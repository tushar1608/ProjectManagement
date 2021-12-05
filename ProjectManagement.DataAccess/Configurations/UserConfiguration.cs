
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;

namespace ProjectManagement.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasAlternateKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.Password)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.OwnsOne(t => t.Email, f => {
                f.Property(p => p.EmailAddress)
                .HasMaxLength(50)
                .IsRequired();
            });

            builder.HasMany(t => t.Tasks)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.AssignedToUserId);
        }
    }
}
