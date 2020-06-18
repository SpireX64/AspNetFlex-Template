using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SpireX.AspNetCore.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetFlex.App.ServiceInstallers
{
    public class SwaggerServiceInstaller : ServiceInstaller
    {
        private static OpenApiInfo CreateInfoForApi(ApiVersionDescription desc)
        {
            var info = new OpenApiInfo()
            {
                Title = "ASP.NET Core Application",
                Version = desc.ApiVersion.ToString(),
                Contact = new OpenApiContact { Name = "SpireX", Email = "SpireX@outlook.com"}
            };
            
            if (desc.IsDeprecated) 
                info.Description += " (This API version has been deprecated!)";
            
            return info;
        }
        
        public override void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigOptions>();
            services.AddSwaggerGen();
            services.AddSwaggerGenNewtonsoftSupport();
        }

        private class SwaggerConfigOptions : IConfigureOptions<SwaggerGenOptions>
        {
            private readonly IApiVersionDescriptionProvider _provider;

            public SwaggerConfigOptions(IApiVersionDescriptionProvider provider)
            {
                _provider = provider;
            }

            public void Configure(SwaggerGenOptions options)
            {
                options.AddSecurityDefinition("Bearer", BuildSecurityScheme());
                options.AddSecurityRequirement(BuildSecurityRequirement());
                
                foreach (var desc in _provider.ApiVersionDescriptions)
                    options.SwaggerDoc(desc.GroupName, CreateInfoForApi(desc));
            }
            
            private static OpenApiSecurityScheme BuildSecurityScheme() =>
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme.\n\n" +
                                  "Enter 'Bearer' [space] and then your token in the text input below."
                };
            
            private static OpenApiSecurityRequirement BuildSecurityRequirement() => 
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                };
        }
    }
}