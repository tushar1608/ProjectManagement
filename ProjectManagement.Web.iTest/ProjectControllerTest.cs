using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.DataAccess;
using ProjectManager.Domain.Entities;
using Xunit;

namespace ProjectManagement.Web.iTest
{
    public class ProjectControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public ProjectControllerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async System.Threading.Tasks.Task ProjectControllerOnValidEndpointCallShouldReturnAllProjectsInDb()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Projects.Add(new Project { Id = "1", Detail = "detail1", Name = "Test Proj1" });
                dbContext.Projects.Add(new Project { Id = "2", Detail = "detail2", Name = "Test Proj2" });
                dbContext.SaveChanges();
            }

            // Act
            var response = await _client.GetAsync("/api/Project");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.Contains("{\"id\":\"1\",\"name\":\"Test Proj1\",\"detail\":\"detail1\",\"createdOn\":\"0001-01-01T00:00:00\"}", responseString);
            Assert.Contains("{\"id\":\"2\",\"name\":\"Test Proj2\",\"detail\":\"detail2\",\"createdOn\":\"0001-01-01T00:00:00\"}", responseString);
        }

        [Fact]
        public async System.Threading.Tasks.Task ProjectControllerOnValidEndpointCallShouldReturnProjectById()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Projects.Add(new Project { Id = "3", Detail = "detail1", Name = "Test Proj1" });
                dbContext.Projects.Add(new Project { Id = "4", Detail = "detail2", Name = "Test Proj2" });
                dbContext.SaveChanges();
            }

            // Act
            var response = await _client.GetAsync("/api/Project/4");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.Contains("{\"id\":\"4\",\"name\":\"Test Proj2\",\"detail\":\"detail2\",\"createdOn\":\"0001-01-01T00:00:00\"}", responseString);
        }

        [Fact]
        public async System.Threading.Tasks.Task ProjectControllerOnValidEndpointCallShouldDeleteTaskWhenProjectExists()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Projects.Add(new Project { Id = "5", Detail = "detail2", Name = "Test Proj2" });
                dbContext.SaveChanges();
            }

            // Act
            var response = await _client.DeleteAsync("/api/Project/5");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("project with id: 5 is deleted", responseString);
        }

        [Fact]
        public async System.Threading.Tasks.Task ProjectControllerOnValidEndpointCallShouldCreateProject()
        {
            // Arrange
            var body = new
            {
                name = "test",
                detail = "detail"
            };

            // Act
            var response = await _client.PostAsync("/api/Project", ContentHelper.GetStringContent(body));

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("\"name\":\"test\",\"detail\":\"detail\",\"createdOn\":\"0001-01-01T00:00:00\"}", responseString);
        }

        [Fact]
        public async System.Threading.Tasks.Task ProjectControllerOnValidEndpointCallShouldUpdateProject()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Projects.Add(new Project { Id = "7", Detail = "detail2", Name = "Test Proj2" });
                dbContext.SaveChanges();
            };
            var body = new
            {
                id= "7",
                name = "test",
                detail = "detail"
            };

            // Act
            var response = await _client.PutAsync("/api/Project", ContentHelper.GetStringContent(body));

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Update successful for project with id: 7", responseString);
        }
    }
}
