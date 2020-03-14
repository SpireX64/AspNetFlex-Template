using System;
using AspNetFlex.Domain.Interactions.Users.Models;
using MediatR;

namespace AspNetFlex.Domain.Interactions.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserModel>
    {
        public Guid UserId { get; }
        public bool ThrowIfNotExists { get; set; }

        public GetUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}