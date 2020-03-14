using System;
using System.Threading.Tasks;
using AspNetFlex.Domain.Interactions.Users.Models;
using SpireX.RepositoryProvider.Abstractions;

namespace AspNetFlex.Domain.Interactions.Users.Repositories
{
    public interface IUsersRepository : IRepository
    {
        Task<UserIdentityModel?> GetUserIdentity(string email);
        Task<bool> IsUserIdentityExists(string email);
        Task<UserModel> RegisterUser(UserIdentityModel user);
        Task<UserModel?> Get(Guid userId);
    }
}