using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectManagement.Web.Interfaces;
using ProjectManagement.Web.Models;
using System.Threading.Tasks;

namespace ProjectManagement.Web.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IJwtAuthenticationManagerService _authManagerService;

        public LoginController(ILogger<LoginController> logger, IJwtAuthenticationManagerService authManagerService)
        {
            _logger = logger;
            _authManagerService = authManagerService;
        }

        [HttpPost]
        [Route("api/login")]
        public async Task<IActionResult> login(LoginDetails loginDetails)
        {
            _logger.LogInformation("Request to login");
            var token = await _authManagerService.Authenticate(loginDetails);
            if (token == null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(token);
            }
        }
    }
}

