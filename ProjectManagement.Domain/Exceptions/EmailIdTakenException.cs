using System;

namespace ProjectManager.Domain.Exceptions
{
    public class EmailIdTakenException : Exception
    {
        public EmailIdTakenException(string email) : base($"Can't create user as another user with same email id: {email} already exists")
        {

        }
    }
}

