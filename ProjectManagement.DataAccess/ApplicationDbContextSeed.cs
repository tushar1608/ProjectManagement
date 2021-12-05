using System.Linq;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.ValueObjects;
using Task = ProjectManager.Domain.Entities.Task;

namespace ProjectManagement.DataAccess
{
    public static class ApplicationDbContextSeed
    {
        public static async System.Threading.Tasks.Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(
                    new User { FirstName = "Tushar", LastName="Sinha", Password = "aezakmi", Id="12", Email=Email.From("tushar@gmail.com")}
                );

                context.Projects.Add(
                    new Project { Id = "1", Name = "Test", Detail= "Details of proj"}
                );

                context.Tasks.Add(
                    new Task { Id = "1", ProjectId="1", Detail="some detail", Status=0, AssignedToUserId="12"}
                    );
                context.Tasks.Add(
                    new Task { Id = "2", ProjectId = "1", Detail = "some detail", Status = 0, AssignedToUserId = "12" }
                    );
                await context.SaveChangesAsync();
            }
        }
    }
}
