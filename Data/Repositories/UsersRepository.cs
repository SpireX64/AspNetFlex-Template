using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetFlex.Data.Mappers;
using AspNetFlex.DatabaseStore.Contexts.App;
using AspNetFlex.Domain.Interactions.Users.Models;
using AspNetFlex.Domain.Interactions.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AspNetFlex.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _dbContext;

        public UsersRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<UserIdentityModel?> GetUserIdentity(string email)
        {
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var user = await _dbContext.Users
                .Where(e => e.Email == email)
                .FirstOrDefaultAsync();

            return user?.ToDomainIdentityModel();
        }

        public async Task<bool> IsUserIdentityExists(string email)
        {
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var user = await _dbContext.Users.FirstOrDefaultAsync(e => e.Email == email);
            return user != null;
        }

        public async Task<UserModel> RegisterUser(UserIdentityModel user)
        {
            var userDbEntity = user.ToDbEntity();
            _dbContext.Users.Add(userDbEntity);
            await _dbContext.SaveChangesAsync();
            return userDbEntity.ToDomainModel();
        }

        public async Task<UserModel?> Get(Guid userId)
        {
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var userDbEntity = await _dbContext.Users.FindAsync(userId);
            return userDbEntity?.ToDomainModel();
        }
    }
}