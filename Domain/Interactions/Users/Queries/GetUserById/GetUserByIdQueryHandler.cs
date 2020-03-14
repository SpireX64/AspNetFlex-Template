using System.Threading;
using System.Threading.Tasks;
using AspNetFlex.Domain.Interactions.Users.Exceptions;
using AspNetFlex.Domain.Interactions.Users.Models;
using AspNetFlex.Domain.Interactions.Users.Repositories;
using MediatR;
using SpireX.RepositoryProvider.Abstractions;

// Mediator doesn't support types nullability, ignore it
#pragma warning disable 8613

namespace AspNetFlex.Domain.Interactions.Users.Queries.GetUserById
{
    internal class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserModel>
    {
        private readonly IUsersRepository _usersRepository;

        public GetUserByIdQueryHandler(IRepositoryProvider provider)
        {
            _usersRepository = provider.GetRepository<IUsersRepository>();
        }

        public async Task<UserModel?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.Get(request.UserId);
            if (user is null && request.ThrowIfNotExists)
                throw new UserNotFoundByIdException(request.UserId);
            return user;
        }
    }
}