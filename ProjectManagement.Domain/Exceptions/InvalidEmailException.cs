using System;

namespace ProjectManager.Domain.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string email) : base($"Passed email {email} is invalid.")
        {

        }
    }
}