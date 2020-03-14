using System;

namespace AspNetFlex.Api.V1
{
    public static class ApiRouter
    {
        private const string ApiVersion = "v1";
        private const string BaseUrl = "api/" + ApiVersion;
        
        public static class Auth
        {
            public const string Url = BaseUrl + "/auth";
        }
    }
}