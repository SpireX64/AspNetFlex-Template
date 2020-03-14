using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Data.UnitTests")]

namespace AspNetFlex.Data
{
    public static class AssemblyProperties
    {
        public static readonly Assembly Assembly = 
            Assembly.GetAssembly(typeof(AssemblyProperties))
            ?? Assembly.GetCallingAssembly();
    }
}