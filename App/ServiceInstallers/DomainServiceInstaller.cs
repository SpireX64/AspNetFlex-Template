using AspNetFlex.Data;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpireX.AspNetCore.Services;
using AssemblyProperties = AspNetFlex.Domain.AssemblyProperties;

namespace AspNetFlex.App.ServiceInstallers
{
    public class DomainServiceInstaller : ServiceInstaller
    {
        public override void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(options =>
            {
                options.AsTransient();
            }, AssemblyProperties.Assembly);

            services.AddSingleton(RepositoryProviderFactory.GetRepositoryProvider);
        }
    }
}