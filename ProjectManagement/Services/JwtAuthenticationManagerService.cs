using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Web.Interfaces;
using ProjectManagement.Web.Models;

namespace ProjectManagement.Web.Services
{
    public class JwtAuthenticationManagerService: IJwtAuthenticationManagerService
    {
        private readonly IUserService _userService;
        private readonly string _key;

        public JwtAuthenticationManagerService(IUserService userService)
        {
            _userService = userService;
            _key = "AEZAKMIPANZERAEZAKMIPANZER";
        }

        public async Task<string> Authenticate(LoginDetails loginDetails)
        {
            var isValidUser = await _userService.isValidUser(loginDetails);
            if (!isValidUser)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                      new Claim(ClaimTypes.Email, loginDetails.Email)
                  }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
