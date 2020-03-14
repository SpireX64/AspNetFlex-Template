using System;
using System.Reflection;
using AspNetFlex.DatabaseStore.Contexts.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpireX.AspNetCore.Services;

namespace AspNetFlex.App.ServiceInstallers
{
    public class DbContextServiceInstaller : ServiceInstaller
    {
        private const string EnvConnectionString = "DB_CONNECTION_STRING";
        private const string SqliteFilename = "debug.sqlite";
        private const ServiceLifetime DbContextLifetime = ServiceLifetime.Transient;
        private const ServiceLifetime DbOptionsLifetime = ServiceLifetime.Singleton;
        
        public override void Install(IServiceCollection services, IConfiguration configuration)
        {
            var migrationAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            
            var connectionString = configuration?.GetValue<string>(EnvConnectionString);
            
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                if (Environments.Development.Equals(EnvironmentName, StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine("SQLite Database is used!");
                    services.AddDbContext<AppDbContext>(builder =>
                        {
                            builder.UseSqlite("Data Source=" + SqliteFilename, 
                                options => options.MigrationsAssembly(migrationAssemblyName));
                        }, 
                        DbContextLifetime,
                        DbOptionsLifetime);
                }
                else
                    throw new Exception("Environment variable 'DB_CONNECTION_STRING' not provided");
            }
            else
            {
                services.AddDbContext<AppDbContext>(builder => { 
                        builder.UseNpgsql(connectionString, 
                            options => options.MigrationsAssembly(migrationAssemblyName));
                    }, 
                    DbContextLifetime,
                    DbOptionsLifetime);
            }
        }
    }
}