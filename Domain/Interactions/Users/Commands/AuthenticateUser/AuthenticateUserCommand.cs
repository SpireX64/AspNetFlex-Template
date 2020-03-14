using AspNetFlex.Domain.Interactions.Users.Models;
using MediatR;

namespace AspNetFlex.Domain.Interactions.Users.Commands.AuthenticateUser
{
    public class AuthenticateUserCommand : IRequest<AuthAccessModel>
    {
        public string Email { get; }
        public string Password { get; }

        public AuthenticateUserCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}