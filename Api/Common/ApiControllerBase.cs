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
        
        protected Guid GetLoggedUserId()
        {
            var claim = User.FindFirst(ClaimTypes.PrimarySid);
            if (Guid.TryParse(claim.Value, out var userId))
            {
                return userId;
            }
            throw new Exception("Can't provide user id");
        }

        protected Task<UserModel> GetCurrentUserAsync()
        {
            var currentUserId = GetLoggedUserId();
            return Mediator.Send(new GetUserByIdQuery(currentUserId));
        }

        #region Api responses
        
        protected IActionResult ApiResult<T>(HttpStatusCode status, T data) where T : class
        {
            var apiResponse = new Models.ApiResponse<T>(status) { Data = data };
            return Ok(apiResponse);
        }

        protected IActionResult ApiError(HttpStatusCode status, ApiError error, params object[] args)
        {
            var errorResponse = new ApiResponseError
            {
                Code = error.Code, 
                Message = string.Format(error.Message, args)
            };
            return Ok(new ApiResponseEmpty(status){ Error = errorResponse });
        }

        protected IActionResult ApiEmpty(HttpStatusCode status)
        {
            return Ok(new ApiResponseEmpty(status));
        }
        
        #endregion
    }
}