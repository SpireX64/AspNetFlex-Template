using AspNetFlex.Domain.Interactions.Users.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SpireX.AspNetCore.Services;

namespace AspNetFlex.App.ServiceInstallers
{
    public class AuthenticationServiceInstaller : ServiceInstaller
    {
        public override void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => ConfigureJwtBearer(options, 
                    configuration.GetSection(AuthUtils.Jwt.ConfigKeys.Section)));
        }

        private void ConfigureJwtBearer(JwtBearerOptions options, IConfiguration configuration)
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                
                ValidateIssuer = true,
                ValidIssuer = AuthUtils.Jwt.Issuer,
                
                ValidateAudience = true,
                ValidAudience = AuthUtils.Jwt.Audience,
                
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = AuthUtils.GetSymmetricKey(configuration[AuthUtils.Jwt.ConfigKeys.SigningKey]),
                
            };
        }
    }
}