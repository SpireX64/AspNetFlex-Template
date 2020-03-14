using AspNetFlex.Domain.Interactions.Users.Models;
using MediatR;

namespace AspNetFlex.Domain.Interactions.Users.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<UserModel>
    {
        public string Email { get; }
        public string Name { get; }
        public string Password { get; }

        public RegisterUserCommand(string email, string name, string password)
        {
            Email = email;
            Name = name.Trim();
            Password = password;
        }
    }
}