using AspNetFlex.Api.Common.Errors;
using AspNetFlex.Api.Common.Models;

namespace AspNetFlex.Api.V1.Shared
{
    public static class ApiErrors
    {
        public static readonly ApiError
            InvalidEmailFormat = new ApiError(1, "Presented email has invalid format ({0})"),
            UserIdentityNotExists = new ApiError(2, "User not exists"),
            IncorrectPassword = new ApiError(3, "Incorrect password"),
            UserIdentityAlreadyExists = new ApiError(4, "User already exists"),
            InvalidUserNameFormat = new ApiError(5, "Presented user name has invalid format {0}"),
            WeakPassword = new ApiError(6, "Presented weak password");
    }
}