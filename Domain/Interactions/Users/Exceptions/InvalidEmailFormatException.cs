using AspNetFlex.Domain.Infrastructure.Exceptions;

namespace AspNetFlex.Domain.Interactions.Users.Exceptions
{
    public class InvalidEmailFormatException : DomainException
    {
        public string Email { get; }

        public InvalidEmailFormatException(string email)
        {
            Email = email;
        }
    }
}