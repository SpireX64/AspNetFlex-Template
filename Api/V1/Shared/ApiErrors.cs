using AspNetFlex.Api.Common.Errors;
using AspNetFlex.Api.Common.Models;

namespace AspNetFlex.Api.V1.Shared
{
    public static class ApiErrors
    {
        public static readonly ApiError
            Unauthorized = new ApiError(1, "Unauthorized"),
            InvalidEmailFormat = new ApiError(2, "Presented email has invalid format ({0})"),
            UserIdentityNotExists = new ApiError(3, "User not exists"),
            IncorrectPassword = new ApiError(4, "Incorrect password"),
            UserIdentityAlreadyExists = new ApiError(5, "User already exists"),
            InvalidUserNameFormat = new ApiError(6, "Presented user name has invalid format {0}"),
            WeakPassword = new ApiError(7, "Presented weak password");
    }
}