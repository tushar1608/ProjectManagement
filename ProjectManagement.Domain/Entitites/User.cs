using ProjectManager.Domain.ValueObjects;

namespace ProjectManager.Domain.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Email Email { get; set; }
        public string Password { get; set; }
    }
}