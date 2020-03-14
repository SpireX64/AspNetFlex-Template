using System;
using NodaTime;

namespace AspNetFlex.Domain.Interactions.Users.Models
{
    public class UserIdentityModel
    {
        public Guid Id { get; }
        public string Email { get; }
        public string Role { get; }
        public string PasswordHash { get; }
        public string Name { get; }

        public Instant RegistrationDate { get; set; }

        public UserIdentityModel(Guid id, string email, string name, string role, string passwordHash, Instant registrationDate)
        {
            Id = id;
            Email = email;
            Name = name;
            Role = role;
            PasswordHash = passwordHash;
            RegistrationDate = registrationDate;
        }
    }
}