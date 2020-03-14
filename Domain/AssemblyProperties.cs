using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Domain.UnitTests")]

namespace AspNetFlex.Domain
{
    public static class AssemblyProperties
    {
        public static readonly Assembly Assembly = 
            Assembly.GetAssembly(typeof(AssemblyProperties)) 
            ?? Assembly.GetExecutingAssembly();
    }
}