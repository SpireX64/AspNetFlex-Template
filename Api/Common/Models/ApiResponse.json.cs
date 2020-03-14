using System.Net;
using Newtonsoft.Json;

namespace AspNetFlex.Api.Common.Models
{
    [JsonObject]
    public class ApiResponse<T> where T : class
    {
        [JsonProperty("status_code", Required = Required.Always)]
        public int StatusCode { get; set; }

        [JsonProperty("status", Required = Required.Always)]
        public string StatusName { get; set; }
        
        [JsonProperty("data", Required = Required.AllowNull)]
        public T? Data { get; set; }
        
        [JsonProperty("error", Required = Required.AllowNull)]
        public ApiResponseError Error { get; set; }

        public ApiResponse(HttpStatusCode httpStatusCode)
        {
            StatusCode = (int) httpStatusCode;
            StatusName = httpStatusCode.ToString();
        }
    }
}