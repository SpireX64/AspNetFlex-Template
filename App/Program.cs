using System;
using System.IO;
using AspNetFlex.App.Boot;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetEscapades.Extensions.Logging.RollingFile;
using SpireX.AspNetCore.Boot;

namespace AspNetFlex.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHost(args).Boot();
        }

        private static WebHostBootstrap CreateHost(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(config =>
            {
                config.UseContentRoot(Directory.GetCurrentDirectory());
                config.UseKestrel();
                config.UseSockets();
                config.UseStartup<Startup>();
                
                LoadEnvironmentConfiguration(config);
                config.ConfigureLogging((ConfigureLogging));
            });

            var hostBoot = WebHostBootstrap.ForHost(builder.Build())
                .UseBootable<DatabaseBootable>()
                .Create();

            return hostBoot;
        }

        private static void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder loggingBuilder)
        {
            var loggingConfig = context.Configuration.GetSection("Logging");
            loggingBuilder.AddConfiguration(loggingConfig);
            loggingBuilder.AddConsole();
            loggingBuilder.AddDebug();
            loggingBuilder.AddFile(options =>
            {
                options.LogDirectory="Logs";
                options.Extension = "log";
                options.Periodicity = PeriodicityOptions.Daily;
                options.IncludeScopes = true;
            });
        }

        private static void LoadEnvironmentConfiguration(IWebHostBuilder hostBuilder)
        {
            const string environmentFile = "Environment.json";
            
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(env))
            {
                env = Environments.Production;
            }

            hostBuilder.ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(AppContext.BaseDirectory + "/Environments")
                    .AddJsonFile(environmentFile)
                    .AddJsonFile($"{env}.{environmentFile}", optional: true);
            });
        }
    }
}