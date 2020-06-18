using System;

namespace AspNetFlex.Api.V1
{
    public static class ApiRouter
    {
        private const string BaseUrl = "api/v{version:apiVersion}";
        
        public static class Auth
        {
            public const string Route = BaseUrl + "/auth";
        }
    }
}