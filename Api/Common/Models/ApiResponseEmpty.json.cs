using System.Net;

namespace AspNetFlex.Api.Common.Models
{
    public class ApiResponseEmpty : ApiResponse<object>
    {
        public ApiResponseEmpty(HttpStatusCode httpStatusCode) : base(httpStatusCode)
        {
            Data = null;
        }
    }
}