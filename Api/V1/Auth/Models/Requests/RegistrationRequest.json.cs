using Newtonsoft.Json;

namespace AspNetFlex.Api.V1.Auth.Models.Requests
{
    [JsonObject]
    public class RegistrationRequest
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        
        [JsonProperty("email", Required = Required.Always)]
        public string Email { get; set; }
        
        [JsonProperty("password", Required = Required.Always)]
        public string Password { get; set; }
    }
}