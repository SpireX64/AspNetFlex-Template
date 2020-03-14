using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.EnableEndpointRouting = false; })
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddApplicationPart(ApiModule.Assembly)
                .AddControllersAsServices();

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
                options.Configuration = Configuration;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        }
    }
}