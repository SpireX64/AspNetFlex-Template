using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;

namespace AspNetFlex.Domain.Interactions.Users.Utils
{
    public static class AuthUtils
    {    
        // RFC 5322 99% (https://www.ietf.org/rfc/rfc5322.txt)
        private static readonly Regex EmailRegex = new Regex(
            "^(?:(\"\")(\"\".+?(?<!\\\\)\"\"@)|(([0-9a-z_]((\\.(?!\\.))|[-!#\\$%&'\\*\\+\\/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z_])@))(?:(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$", 
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        
        // at least 8 char length, lower case char, upper case char, number char
        private static readonly Regex StrongPasswordRegex = new Regex(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,}");
        
        private static readonly Regex NameRegex = new Regex(
            @"^\w+(?:(?:[',. -](?:\w| ))?(\w|.| |-)*)*$", 
            RegexOptions.CultureInvariant);

        private const byte RequiredNameMinLength = 3;
        
        public static class Jwt {
            public const string AuthType = "Bearer";
            public const string Issuer = "Dequeue.Estimate";
            public const string Audience = "ApiClient";
            
            public static class ConfigKeys
            {
                public const string Section = "Security";
                public const string SigningKey = "SigningKey";
                public const string LifetimeKey = "Lifetime";
            }
        }

        public static SymmetricSecurityKey GetSymmetricKey(string secret) =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        
        public static bool ValidateEmail(string email) => 
            EmailRegex.IsMatch(email);

        public static string GetMd5Hash(string source)
        {
            using var md5 = MD5.Create();
            var data = md5.ComputeHash(Encoding.UTF8.GetBytes(source));
            
            var hashBuilder = new StringBuilder();
            foreach (var value in data) 
                hashBuilder.Append(value.ToString("x2"));
            return hashBuilder.ToString();
        }

        public static bool CheckPasswordComplexity(string password) => 
            StrongPasswordRegex.IsMatch(password);

        public static bool ValidateName(string name) => 
            name.Length >= RequiredNameMinLength && NameRegex.IsMatch(name);
    }
}