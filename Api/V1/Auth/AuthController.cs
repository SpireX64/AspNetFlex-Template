using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AspNetFlex.Api.Common;
using AspNetFlex.Api.Common.Models;
using AspNetFlex.Api.V1.Auth.Models;
using AspNetFlex.Api.V1.Auth.Models.Requests;
using AspNetFlex.Api.V1.Auth.Models.Responses;
using AspNetFlex.Api.V1.Shared;
using AspNetFlex.Domain.Interactions.Users.Commands.AuthenticateUser;
using AspNetFlex.Domain.Interactions.Users.Commands.RegisterUser;
using AspNetFlex.Domain.Interactions.Users.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetFlex.Api.V1.Auth
{
    [ApiController]
    [ApiVersion("1")]
    [Route(ApiRouter.Auth.Route)]
    [Produces("application/json")]
    public class AuthController : ApiControllerBase
    {
        public AuthController(IMediator mediator): base(mediator)
        {
        }
        
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Login(
            [FromBody] AuthRequest authRequest,
            CancellationToken cancellationToken)
        {
            var command = new AuthenticateUserCommand(authRequest.Email, authRequest.Password);

            IActionResult response;
            try
            {
                var auth = await Mediator.Send(command, cancellationToken);
                response = ApiOk(auth.AsResponse());
            }
            catch (InvalidEmailFormatException ex)
            {
                response = ApiBadRequest(ApiErrors.InvalidEmailFormat, ex.Email);
            }
            catch (UserNotFoundByEmailException)
            {
                response = ApiBadRequest(ApiErrors.UserIdentityNotExists);
            }
            catch (WrongPasswordException)
            {
                response = ApiBadRequest( ApiErrors.IncorrectPassword);
            }

            return response;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<UserResponseModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseEmpty), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register(
            [FromBody] RegistrationRequest registrationRequest,
            CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(
                registrationRequest.Email,
                registrationRequest.Name,
                registrationRequest.Password);

            try
            {
                var registrationResult = await Mediator.Send(command, cancellationToken);
                return ApiOk(registrationResult.AsResponse());
            }
            catch (InvalidEmailFormatException ex)
            {
                return ApiBadRequest(ApiErrors.InvalidEmailFormat, ex.Email);
            }
            catch (UserAlreadyExistsException)
            {
                return ApiBadRequest(ApiErrors.UserIdentityAlreadyExists);
            }
            catch (InvalidNameFormatException)
            {
                return ApiBadRequest(ApiErrors.InvalidUserNameFormat);
            }
            catch (WeakPasswordException)
            {
                return ApiBadRequest(ApiErrors.WeakPassword);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await GetCurrentUserAsync();
            return user is null 
                ? ApiUnauthorized(ApiErrors.Unauthorized) 
                : ApiOk(user.AsResponse());
        }
    }
}