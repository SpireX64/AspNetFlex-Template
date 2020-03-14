using System;
using AspNetFlex.Domain.Infrastructure.Exceptions;

namespace AspNetFlex.Domain.Interactions.Users.Exceptions
{
    public class UserNotFoundByIdException : DomainException
    {
        public Guid UserId { get; }

        public UserNotFoundByIdException(Guid userId)
        {
            UserId = userId;
        }
    }
}