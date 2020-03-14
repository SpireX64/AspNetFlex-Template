using System.Net;
using AspNetFlex.Api.V1.Auth.Models.Responses;
using AspNetFlex.Domain.Interactions.Users.Models;

namespace AspNetFlex.Api.V1.Auth.Models
{
    public static class UsersMapper
    {
        public static AuthResponseModel AsResponse(this AuthAccessModel auth) =>
            new AuthResponseModel
            {
                UserId = auth.Id,
                Token = auth.Token,
                Email = auth.Email,
                Name = auth.Name,
                Expires = auth.ExpiresAt.ToString()
            };

        public static UserResponseModel AsResponse(this UserModel user) =>
            new UserResponseModel
            {
                Name = user.Name,
                Email = user.Email,
                RegistrationDate = user.RegistrationDate.ToString()
            };
    }
}