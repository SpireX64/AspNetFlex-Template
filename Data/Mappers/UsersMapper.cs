using AspNetFlex.DatabaseStore.Contexts.App.Entities;
using AspNetFlex.Domain.Interactions.Users.Models;
using NodaTime;

namespace AspNetFlex.Data.Mappers
{
    public static class UsersMapper
    {
        public static UserIdentityModel ToDomainIdentityModel(this UserDbEntity dbEntity)
        {
            var regDate = Instant.FromUnixTimeMilliseconds(dbEntity.RegistrationDate);
            return new UserIdentityModel(
                dbEntity.Id,
                dbEntity.Email,
                dbEntity.Name,
                dbEntity.Role,
                dbEntity.PasswordHash,
                regDate);
        }

        public static UserModel ToDomainModel(this UserDbEntity dbEntity)
        {
            var regDate = Instant.FromUnixTimeMilliseconds(dbEntity.RegistrationDate);
            return new UserModel
            {
                Id = dbEntity.Id,
                Email = dbEntity.Email,
                Name = dbEntity.Name,
                RegistrationDate = regDate
            };
        }

        public static UserDbEntity ToDbEntity(this UserIdentityModel userIdentity) =>
            new UserDbEntity()
            {
                Id = userIdentity.Id,
                Email = userIdentity.Email,
                Name = userIdentity.Name,
                Role = userIdentity.Role,
                RegistrationDate = userIdentity.RegistrationDate.ToUnixTimeMilliseconds(),
                PasswordHash = userIdentity.PasswordHash
            };
    }
}