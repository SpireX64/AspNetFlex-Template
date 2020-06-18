using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpireX.AspNetCore.Services;
using ApiModule = AspNetFlex.Api.AssemblyProperties;

namespace AspNetFlex.App
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Config { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(options =>
                {
                    options.EnableEndpointRouting = false;
                }).SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddApplicationPart(ApiModule.Assembly)
                .AddControllersAsServices()
                .AddApiExplorer();
            
            services.AddApiVersioning(options => options.ReportApiVersions = true);
            services.AddVersionedApiExplorer(options =>
            {
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(cors =>
                {
                    cors.AllowAnyOrigin();
                    cors.AllowAnyMethod();
                    cors.AllowAnyHeader();
                    cors.Build();
                });
            });
            
            services.AddServicesInstaller(options =>
            {
                options.Environment = Environment.EnvironmentName;
                options.Configuration = Config;
            });
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{desc.GroupName}/swagger.json",
                        desc.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}