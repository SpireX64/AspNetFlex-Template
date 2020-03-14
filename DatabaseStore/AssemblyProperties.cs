using System.Reflection;

namespace AspNetFlex.DatabaseStore
{
    public static class AssemblyProperties
    {
        public static readonly Assembly Assembly = 
            Assembly.GetAssembly(typeof(AssemblyProperties))
            ?? Assembly.GetCallingAssembly();
    }
}