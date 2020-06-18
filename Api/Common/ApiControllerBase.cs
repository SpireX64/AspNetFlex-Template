using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetFlex.Api.Common.Errors;
using AspNetFlex.Api.Common.Models;
using AspNetFlex.Domain.Interactions.Users.Models;
using AspNetFlex.Domain.Interactions.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetFlex.Api.Common
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IMediator Mediator { get; }

        public ApiControllerBase(IMediator mediator)
        {
            Mediator = mediator;
        }
        
        protected Guid? GetLoggedUserId()
        {
            if (User is null) 
                return null;
            
            var claim = User.FindFirst(ClaimTypes.PrimarySid);
            if (Guid.TryParse(claim?.Value, out var userId))
            {
                return userId;
            }

            return null;
        }

        protected Task<UserModel> GetCurrentUserAsync()
        {
            var currentUserId = GetLoggedUserId();
            return currentUserId.HasValue ? 
                Mediator.Send(new GetUserByIdQuery(currentUserId.Value)) : 
                Task.FromResult<UserModel>(null!);
        }

        #region Api responses

        protected IActionResult ApiOk<T>(T data) where T : class => 
            Ok(BuildModelResponse(HttpStatusCode.OK, data));

        protected IActionResult ApiOk() => 
            Ok(new ApiResponseEmpty(HttpStatusCode.OK));

        protected IActionResult ApiBadRequest(ApiError error, params object[] args) => 
            BadRequest(BuildErrorResponse(HttpStatusCode.BadRequest, error, args));

        protected IActionResult ApiNotFound(ApiError error, params object[] args) => 
            NotFound(BuildErrorResponse(HttpStatusCode.NotFound, error, args));

        protected IActionResult ApiUnauthorized(ApiError error, params object[] args) => 
            Unauthorized(BuildErrorResponse(HttpStatusCode.Unauthorized, error, args));

        #endregion
        
        private ApiResponse<T> BuildModelResponse<T>(HttpStatusCode statusCode, T data) where T: class => 
            new ApiResponse<T>(statusCode){ Data = data};

        private ApiResponseEmpty BuildErrorResponse(HttpStatusCode statusCode, ApiError error, params object[] args) =>
            new ApiResponseEmpty(statusCode)
            {
                Error = new ApiResponseError()
                {
                    Code = error.Code,
                    Message = string.Format(error.Message, args)
                }
            };
    }
}