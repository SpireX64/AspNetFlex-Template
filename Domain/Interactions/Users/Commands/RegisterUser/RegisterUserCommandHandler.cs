using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetFlex.Domain.Interactions.Users.Exceptions;
using AspNetFlex.Domain.Interactions.Users.Models;
using AspNetFlex.Domain.Interactions.Users.Repositories;
using AspNetFlex.Domain.Interactions.Users.Utils;
using MediatR;
using NodaTime;
using SpireX.RepositoryProvider.Abstractions;

namespace AspNetFlex.Domain.Interactions.Users.Commands.RegisterUser
{
    internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserModel>
    {
        private readonly IUsersRepository _usersRepository;

        internal IClock Clock { get; set; } = SystemClock.Instance;

        public RegisterUserCommandHandler(IRepositoryProvider provider)
        {
            _usersRepository = provider.GetRepository<IUsersRepository>();
        }
        
        /// <exception cref="InvalidEmailFormatException"></exception>
        /// <exception cref="UserAlreadyExistsException"></exception>
        /// <exception cref="InvalidNameFormatException"></exception>
        /// <exception cref="WeakPasswordException"></exception>
        public async Task<UserModel> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Check is email valid
            if(!AuthUtils.ValidateEmail(request.Email))
                throw new InvalidEmailFormatException(request.Email);
            
            // Check is user already exists
            var isExists = await _usersRepository.IsUserIdentityExists(request.Email);
            if (isExists)
                throw new UserAlreadyExistsException(request.Email);
            
            // Check name
            if (!AuthUtils.ValidateName(request.Name))
                throw new InvalidNameFormatException(request.Name);
            
            // Check password stronger
            if (!AuthUtils.CheckPasswordComplexity(request.Password))
                throw new WeakPasswordException();
            
            // Generate password hash
            var passwordHash = AuthUtils.GetMd5Hash(request.Password);
            
            // User registration instant
            var registrationInstant = Clock.GetCurrentInstant();

            var user = new UserIdentityModel(
                Guid.NewGuid(),
                request.Email,
                request.Name,
                "user",
                passwordHash,
                registrationInstant
            );

            // Register user
            var registrationResult =  await _usersRepository.RegisterUser(user);
            
            return registrationResult;
        }
    }
}