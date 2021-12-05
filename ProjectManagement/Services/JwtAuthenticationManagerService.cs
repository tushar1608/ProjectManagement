using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Repository;
using ProjectManagement.Web.Interfaces;
using ProjectManagement.Web.Models;
using ProjectManager.Domain.Entities;

namespace ProjectManagement.Web.Services
{
    public class JwtAuthenticationManagerService: IJwtAuthenticationManagerService
    {
        private readonly IRepository<User> _repository;
        private readonly string _key;

        public JwtAuthenticationManagerService(IRepository<User> repository)
        {
            _repository = repository;
            _key = "AEZAKMIPANZERAEZAKMIPANZER";
        }

        public async Task<string> Authenticate(LoginDetails loginDetails)
        {
            var isValidUser = _repository.All()
                .Any(t => t.Email == loginDetails.Email && t.Password == loginDetails.Password);

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
