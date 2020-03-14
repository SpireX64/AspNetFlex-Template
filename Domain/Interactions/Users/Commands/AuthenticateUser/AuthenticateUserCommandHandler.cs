using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AspNetFlex.Domain.Infrastructure.Utils;
using AspNetFlex.Domain.Interactions.Users.Exceptions;
using AspNetFlex.Domain.Interactions.Users.Models;
using AspNetFlex.Domain.Interactions.Users.Repositories;
using AspNetFlex.Domain.Interactions.Users.Utils;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NodaTime;
using SpireX.RepositoryProvider.Abstractions;

namespace AspNetFlex.Domain.Interactions.Users.Commands.AuthenticateUser
{
    internal class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, AuthAccessModel>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IConfiguration _configuration;

        public IClock Clock { get; set; } = SystemClock.Instance;
        
        public AuthenticateUserCommandHandler(IRepositoryProvider provider, IConfiguration configuration)
        {
            _usersRepository = provider.GetRepository<IUsersRepository>();
            _configuration = configuration.GetSection(AuthUtils.Jwt.ConfigKeys.Section);
        }
        
        /// <exception cref="UserNotFoundByEmailException">Provided email is invalid</exception>
        /// <exception cref="WrongPasswordException">User with provided email not found</exception>
        /// <exception cref="InvalidEmailFormatException">Provided password is not correct</exception>
        public async Task<AuthAccessModel> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate email
            if (string.IsNullOrWhiteSpace(request.Email) || !AuthUtils.ValidateEmail(request.Email))
                throw new InvalidEmailFormatException(request.Email);
            
            // Find user identity
            var userIdentity = await _usersRepository.GetUserIdentity(request.Email);
            if (userIdentity is null)
                throw new UserNotFoundByEmailException(request.Email);
            
            // Check password
            if (!ValidatePassword(request.Password, userIdentity))
                throw new WrongPasswordException();

            // Generate token
            var instantNow = Clock.GetCurrentInstant();
            var tokenExpiresAt = instantNow.Plus(GetJwtLifetime());
            var claims = GetClaimsIdentity(userIdentity);
            var token = GenerateJwtSecurityToken(claims, instantNow, tokenExpiresAt);

            // Build access model
            var authAccess = new AuthAccessModel
            {
                Id = userIdentity.Id,
                Email = userIdentity.Email,
                Name = userIdentity.Name,
                Role = userIdentity.Role,
                Token = token,
                ExpiresAt = tokenExpiresAt
            };

            return authAccess;
        }

        private bool ValidatePassword(string providedPassword, UserIdentityModel identity)
        {
            var hash = AuthUtils.GetMd5Hash(providedPassword);
            return StringComparer.OrdinalIgnoreCase.Compare(hash, identity.PasswordHash) == 0;
        }

        private ClaimsIdentity GetClaimsIdentity(UserIdentityModel userIdentity)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, userIdentity.Id.ToString()), 
                new Claim(ClaimTypes.NameIdentifier, userIdentity.Email)
            };
            return new ClaimsIdentity(claims, AuthUtils.Jwt.AuthType);
        }

        private SymmetricSecurityKey GetSecurityKey()
        {
            var secret = _configuration[AuthUtils.Jwt.ConfigKeys.SigningKey];
            return AuthUtils.GetSymmetricKey(secret);
        }

        private Duration GetJwtLifetime()
        {
            var lifetimeString = _configuration[AuthUtils.Jwt.ConfigKeys.LifetimeKey];
            return DurationUtils.FromString(lifetimeString);
        }

        private string GenerateJwtSecurityToken(ClaimsIdentity identityClaims, Instant instantNow, Instant expiresAt)
        {
            var signingCredentials = new SigningCredentials(
                GetSecurityKey(),
                SecurityAlgorithms.HmacSha256);
            
            var jwt = new JwtSecurityToken(
                AuthUtils.Jwt.Issuer,
                AuthUtils.Jwt.Audience,
                identityClaims.Claims,
                instantNow.ToDateTimeUtc(),
                expiresAt.ToDateTimeUtc(),
                signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}