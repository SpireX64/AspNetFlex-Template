using AspNetFlex.Domain.Infrastructure.Exceptions;

namespace AspNetFlex.Domain.Interactions.Users.Exceptions
{
    public class UserNotFoundByEmailException : DomainException
    {
        public string Email { get; }

        public UserNotFoundByEmailException(string email)
        {
            Email = email;
        }
    }
}