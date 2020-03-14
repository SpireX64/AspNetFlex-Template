using System;
using NodaTime;

namespace AspNetFlex.Domain.Interactions.Users.Models
{
    public class AuthAccessModel
    {
        public string Token { get; set; } = string.Empty;
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Instant ExpiresAt { get; set; } = Instant.MinValue;
        public string Role { get; set; } = string.Empty;
    }
}