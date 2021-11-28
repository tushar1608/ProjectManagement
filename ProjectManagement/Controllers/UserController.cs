using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectManagement.Web.Interfaces;
using ProjectManagement.Web.Models;
using System.Threading.Tasks;

namespace ProjectManagement.Web.Controllers
{
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/User")]
        public async Task<IActionResult> Create(UserCreationRequest user)
        {
            _logger.LogInformation("Request to create new user", user.FirstName, user.LastName, user.Email);
            return Ok(await _userService.CreateUser(user));
        }

        [HttpPut]
        [Route("api/User")]
        public async Task<IActionResult> Update(UserUpdateRequest user)
        {
            _logger.LogInformation($"Request to update user {user.Id}");
            if (await _userService.UpdateUser(user) == null)
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
            return Ok(await _userService.GetAllUsers());
        }

        [HttpGet]
        [Route("api/User/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogInformation($"Request to get user with id: {id}");
            var user = await _userService.GetUser(id);
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
            var isDeleteSuccessful = await _userService.DeleteUser(id);
            if (isDeleteSuccessful == false)
            {
                return NotFound($"No user found with id {id}");
            }
            return Ok($"USer with id : {id} deleted");
        }
    }
}

