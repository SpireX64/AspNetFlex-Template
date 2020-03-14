using AspNetFlex.Domain.Infrastructure.Exceptions;

namespace AspNetFlex.Domain.Interactions.Users.Exceptions
{
    public class UserAlreadyExistsException : DomainException
    {
        public string Email { get; }

        public UserAlreadyExistsException(string email)
        {
            Email = email;
        }
    }
}