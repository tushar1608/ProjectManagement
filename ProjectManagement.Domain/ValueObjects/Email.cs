
using ProjectManager.Domain.Common;
using ProjectManager.Domain.Exceptions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProjectManager.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        static Email()
        {

        }

        private Email()
        {

        }

        private Email(string email)
        {
            EmailAddress = email;
        }

        public static Email From(string email)
        {
            var emailAddress = new Email(email);
            if (!IsValidEmail(emailAddress.EmailAddress))
            {
                throw new InvalidEmailException(emailAddress.EmailAddress);
            }
            return emailAddress;
        }

        private static bool IsValidEmail(string email)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(email);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static implicit operator string(Email email)
        {
            return email.ToString();
        }

        public static explicit operator Email(string email)
        {
            return From(email);
        }

        public override string ToString()
        {
            return EmailAddress;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmailAddress;
        }

        public string EmailAddress { get; private set; }
    }
}

