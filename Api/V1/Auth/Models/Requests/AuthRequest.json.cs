using Newtonsoft.Json;

namespace AspNetFlex.Api.V1.Auth.Models.Requests
{
    [JsonObject]
    public class AuthRequest
    {
        [JsonProperty("email", Required = Required.Always)]
        public string Email { get; set; }
        
        [JsonProperty("password", Required = Required.Always)]
        public string Password { get; set; }
    }
}