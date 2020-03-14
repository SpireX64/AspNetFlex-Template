using System;
using SpireX.RepositoryProvider;
using SpireX.RepositoryProvider.Abstractions;

namespace AspNetFlex.Data
{
    public static class RepositoryProviderFactory
    {
        public static IRepositoryProvider GetRepositoryProvider(IServiceProvider serviceProvider)
        {
            return RepositoryProvider.GetBuilder()
                .AddAssemblySource(AssemblyProperties.Assembly)
                .SetServiceProvider(serviceProvider)
                .AllowTypeCache(true)
                .Build();
        }
    }
}