using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectManagement.Repository;
using ProjectManagement.Web.Models;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace ProjectManagement.Web.Controllers
{
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IRepository<User> _repository;

        public UserController(ILogger<UserController> logger, IRepository<User> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/User")]
        public async Task<IActionResult> Create(UserCreationRequest user)
        {
            _logger.LogInformation("Request to create new user", user.FirstName, user.LastName, user.Email);
            var userEntity = new User { Id = Guid.NewGuid().ToString(), FirstName = user.FirstName, LastName = user.LastName, Email = Email.From(user.Email), Password = user.Password };
            return Ok(_repository.Add(userEntity).Id);
        }

        [HttpPut]
        [Route("api/User")]
        public async Task<IActionResult> Update(UserUpdateRequest user)
        {
            _logger.LogInformation($"Request to update user {user.Id}");
            var userEntity = new User { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = Email.From(user.Email), Password = user.Password };
            if (_repository.Update(userEntity) == null)
            {
                return NotFound($"No entry for user id: {user.Id} found to update");
            }
            return Ok($"Update successful for user with id: {user.Id}");
        }

        [HttpGet]
        [Route("api/User")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("All users requested");
            return Ok(_repository.All());
        }

        [HttpGet]
        [Route("api/User/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogInformation($"Request to get user with id: {id}");
            var user = _repository.Get(id);
            if (user == null)
            {
                return NotFound($"No user found with id {id}");
            }
            return Ok(user);
        }

        [HttpDelete]
        [Route("api/User/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation($"Request to delete user with id: {id}");
            var deletedEntity = _repository.Delete(id);
            if (deletedEntity == null)
            {
                return NotFound($"No user found with id {id}");
            }
            return Ok($"User with id : {id} deleted");
        }
    }
}

