using System;
using NodaTime;

namespace AspNetFlex.Domain.Interactions.Users.Models
{
    public class UserModel
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Instant RegistrationDate { get; set; } = Instant.MinValue;
    }
}