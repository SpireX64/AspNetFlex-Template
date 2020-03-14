using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace AspNetFlex.Domain.UnitTests.Utils.Factories
{
    public static class ConfigurationFactory
    {
        public static IConfiguration FromDictionary(IDictionary<string, string> dictionary) =>
            new ConfigurationBuilder()
                .AddInMemoryCollection(dictionary)
                .Build();
    }
}