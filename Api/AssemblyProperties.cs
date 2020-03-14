using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Api.UnitTests")]

namespace AspNetFlex.Api
{
    public static class AssemblyProperties
    {
        public static readonly Assembly Assembly = Assembly.GetAssembly(typeof(AssemblyProperties));
    }
}