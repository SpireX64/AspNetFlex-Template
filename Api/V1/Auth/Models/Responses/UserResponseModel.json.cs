using System;
using Newtonsoft.Json;

namespace AspNetFlex.Api.V1.Auth.Models.Responses
{
    [JsonObject]
    public class UserResponseModel
    {
        [JsonProperty("id", Required = Required.Always)]
        public Guid Id { get; set; }
    
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        
        [JsonProperty("email", Required = Required.Always)]
        public string Email { get; set; }

        [JsonProperty("registration_date", Required = Required.Always)]
        public string RegistrationDate { get; set; }
    }
}