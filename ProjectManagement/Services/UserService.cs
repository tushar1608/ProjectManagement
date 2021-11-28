using ProjectManagement.Web.Interfaces;
using ProjectManagement.Web.Models;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Exceptions;
using ProjectManager.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Web.Services
{
    public class UserService : IUserService
    {
        private IList<User> Users;

        public UserService()
        {
            this.Users = new List<User>();
        }

        public async Task<string> CreateUser(UserCreationRequest userCreationRequest)
        {
            try
            {
                var user = new User { Id = Guid.NewGuid().ToString(), FirstName = userCreationRequest.FirstName, LastName = userCreationRequest.LastName, Email = Email.From(userCreationRequest.Email), Password = userCreationRequest.Password };
                await System.Threading.Tasks.Task.Run(() => {
                    if (Users.Any(t => t.Email.Equals(user.Email)))
                    {
                        throw new EmailIdTakenException(user.Email);
                    }
                    else
                    {
                        Users.Add(user);
                    }
                });
                return $"User created with id:{user.Id}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<UserInformation>> GetAllUsers()
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var response = new List<UserInformation>();
                foreach (var user in Users)
                {
                    response.Add(new UserInformation { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email });
                }
                return response;
            });
        }

        public async Task<UserInformation> GetUser(string id)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var match = Users.FirstOrDefault(p => p.Id == id);
                if (match == null)
                {
                    return null;
                }
                return new UserInformation { Id = match.Id, FirstName = match.FirstName, LastName = match.LastName, Email = match.Email };
            });
        }

        public async Task<string> UpdateUser(UserUpdateRequest updateUserRequest)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var user = Users.FirstOrDefault(x => x.Id == updateUserRequest.Id);
                if (user != null)
                {
                    user.FirstName = updateUserRequest.FirstName;
                    user.LastName = updateUserRequest.LastName;
                    user.Password = updateUserRequest.Password;
                    user.Email = Email.From(updateUserRequest.Email);
                    return user.Id;
                }
                else
                {
                    return null;
                }
            });
        }

        public async Task<bool> isValidUser(LoginDetails loginDetails)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var user = Users.FirstOrDefault(x => x.Email == loginDetails.Email);
                if (user != null && user.Password == loginDetails.Password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public async Task<bool> DeleteUser(string id)
        {

            return await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    var userToRemove = Users.Single(x => x.Id == id);
                    Users.Remove(userToRemove);
                    return true;
                }
                catch {
                    return false;
                }
                
            });
        }
    }
}

