using ProjectManagement.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Web.Interfaces
{
    public interface IUserService
    {
        public Task<string> CreateUser(UserCreationRequest user);
        public Task<string> UpdateUser(UserUpdateRequest updateUSerRequest);
        public Task<List<UserInformation>> GetAllUsers();
        public Task<UserInformation> GetUser(string id);
        public Task<bool> DeleteUser(string id);
        public Task<bool> isValidUser(LoginDetails loginDetails);
    }
}

