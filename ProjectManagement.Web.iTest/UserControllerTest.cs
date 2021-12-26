using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.DataAccess;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.ValueObjects;
using Xunit;

namespace ProjectManagement.Web.iTest
{
    public class UserControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public UserControllerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async System.Threading.Tasks.Task UserControllerOnValidEndpointCallShouldReturnAllUsersInDb()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Users.Add(new User { Id = "1", FirstName = "John", LastName = "Doe", Password = "test", Email = Email.From("john@gmail.com")});
                dbContext.Users.Add(new User { Id = "2", FirstName = "Jane", LastName = "Doe", Password = "test", Email = Email.From("jane@gmail.com") });
                dbContext.SaveChanges();
            }

            // Act
            var response = await _client.GetAsync("/api/User");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.Contains("{\"id\":\"1\",\"firstName\":\"John\",\"lastName\":\"Doe\",\"email\":{\"emailAddress\":\"john@gmail.com\"},\"password\":\"test\"}", responseString);
            Assert.Contains("{\"id\":\"2\",\"firstName\":\"Jane\",\"lastName\":\"Doe\",\"email\":{\"emailAddress\":\"jane@gmail.com\"},\"password\":\"test\"}", responseString);
        }

        [Fact]
        public async System.Threading.Tasks.Task UserControllerOnValidEndpointCallShouldReturnUserById()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Users.Add(new User { Id = "3", FirstName = "John", LastName = "Doe", Password = "test", Email = Email.From("john@gmail.com") });
                dbContext.Users.Add(new User { Id = "4", FirstName = "Jane", LastName = "Doe", Password = "test", Email = Email.From("jane@gmail.com") });
                dbContext.SaveChanges();
            }

            // Act
            var response = await _client.GetAsync("/api/User/4");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.Contains("{\"id\":\"4\",\"firstName\":\"Jane\",\"lastName\":\"Doe\",\"email\":{\"emailAddress\":\"jane@gmail.com\"},\"password\":\"test\"}", responseString);
        }

        [Fact]
        public async System.Threading.Tasks.Task UserControllerOnValidEndpointCallShouldDeleteTaskWhenUserExists()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Users.Add(new User { Id = "5", FirstName = "John", LastName = "Doe", Password = "test", Email = Email.From("john@gmail.com") });
                dbContext.SaveChanges();
            }

            // Act
            var response = await _client.DeleteAsync("/api/User/5");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("User with id : 5 deleted", responseString);
        }

        [Fact]
        public async System.Threading.Tasks.Task UserControllerOnValidEndpointCallShouldCreateUser()
        {
            // Arrange
            var body = new
            {
                firstName = "john",
                lastName = "doe",
                email = "test@test.com",
                password = "test"
            };

            // Act
            var response = await _client.PostAsync("/api/User", ContentHelper.GetStringContent(body));

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseString);
        }

        [Fact]
        public async System.Threading.Tasks.Task UserControllerOnValidEndpointCallShouldUpdateUser()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Users.Add(new User { Id = "7", FirstName = "John", LastName = "Doe", Password = "test", Email = Email.From("john@gmail.com") });
                dbContext.SaveChanges();
            }
            var body = new
            {
                id = "7",
                irstName = "jane",
                lastName = "doe",
                email = "test2@test2.com",
                password = "test"
            };

            // Act
            var response = await _client.PutAsync("/api/User", ContentHelper.GetStringContent(body));

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Update successful for user with id: 7", responseString);
        }
    }
}
