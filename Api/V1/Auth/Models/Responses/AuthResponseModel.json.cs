using System;
using Newtonsoft.Json;

namespace AspNetFlex.Api.V1.Auth.Models.Responses
{
    [JsonObject]
    public class AuthResponseModel
    {
        [JsonProperty("id", Required = Required.Always)]
        public Guid UserId { get; set; }
        
        [JsonProperty("token", Required = Required.Always)]
        public string Token { get; set; }
        
        [JsonProperty("email", Required = Required.Always)]
        public string Email { get; set; }
        
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        
        [JsonProperty("expires", Required = Required.Always)]
        public string Expires { get; set; }

    }
}