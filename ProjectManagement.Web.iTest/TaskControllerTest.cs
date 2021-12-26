using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.DataAccess;
using ProjectManager.Domain.Enums;
using Xunit;

namespace ProjectManagement.Web.iTest
{
    public class TaskControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public TaskControllerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task TaskControllerOnValidEndpointCallShouldReturnAllTasksInDb()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Tasks.Add(new ProjectManager.Domain.Entities.Task { Id = "1", Detail = "detail1", Status = Status.New, AssignedToUserId = "1", ProjectId = "1" });
                dbContext.Tasks.Add(new ProjectManager.Domain.Entities.Task { Id = "2", Detail = "detail2", Status = Status.New, AssignedToUserId = "2", ProjectId = "2" });
                dbContext.SaveChanges();
            }

            // Act
            var response = await _client.GetAsync("/api/Task");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.Contains("{\"id\":\"1\",\"projectId\":\"1\",\"status\":1,\"assignedToUserId\":\"1\",\"detail\":\"detail1\",\"createdOn\":\"0001-01-01T00:00:00\"}", responseString);
            Assert.Contains("{\"id\":\"2\",\"projectId\":\"2\",\"status\":1,\"assignedToUserId\":\"2\",\"detail\":\"detail2\",\"createdOn\":\"0001-01-01T00:00:00\"}", responseString);
        }

        [Fact]
        public async Task TaskControllerOnValidEndpointCallShouldReturnTaskById()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Tasks.Add(new ProjectManager.Domain.Entities.Task { Id = "3", Detail = "detail1", Status = Status.New, AssignedToUserId = "1", ProjectId = "1" });
                dbContext.Tasks.Add(new ProjectManager.Domain.Entities.Task { Id = "4", Detail = "detail2", Status = Status.New, AssignedToUserId = "2", ProjectId = "2" });
                dbContext.SaveChanges();
            }

            // Act
            var response = await _client.GetAsync("/api/Task/3");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.Contains("{\"id\":\"3\",\"projectId\":\"1\",\"status\":1,\"assignedToUserId\":\"1\",\"detail\":\"detail1\",\"createdOn\":\"0001-01-01T00:00:00\"}", responseString);
        }

        [Fact]
        public async Task TaskControllerOnValidEndpointCallShouldDeleteTaskWhenTaskExists()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Tasks.Add(new ProjectManager.Domain.Entities.Task { Id = "5", Detail = "detail1", Status = Status.New, AssignedToUserId = "1", ProjectId = "1" });
                dbContext.Tasks.Add(new ProjectManager.Domain.Entities.Task { Id = "6", Detail = "detail2", Status = Status.New, AssignedToUserId = "2", ProjectId = "2" });
                dbContext.SaveChanges();
            }

            // Act
            var response = await _client.DeleteAsync("/api/Task/5");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("Task with id: 5 is deleted", responseString);
        }

        [Fact]
        public async Task TaskControllerOnValidEndpointCallShouldCreateTask()
        {
            // Arrange
            var body = new
            {
                projectId = "1",
                assignedToUserId = "1",
                detail = "detail5"
            };

            // Act
            var response = await _client.PostAsync("/api/Task", ContentHelper.GetStringContent(body));

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("\"projectId\":\"1\",\"status\":0,\"assignedToUserId\":\"1\",\"detail\":\"detail5\",\"createdOn\":\"0001-01-01T00:00:00\"}", responseString);
        }

        [Fact]
        public async Task TaskControllerOnValidEndpointCallShouldUpdateTask()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Tasks.Add(new ProjectManager.Domain.Entities.Task { Id = "7", Detail = "detail1", Status = Status.New, AssignedToUserId = "1", ProjectId = "1" });
                dbContext.SaveChanges();
            }
            var body = new
            {
                id = "7",
                status = 1,
                projectId = "1",
                assignedToUserId = "1",
                detail = "detail5"
            };

            // Act
            var response = await _client.PutAsync("/api/Task", ContentHelper.GetStringContent(body));

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Update successful for task with id: 7", responseString);
        }

    }
}
