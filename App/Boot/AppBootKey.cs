using SpireX.AspNetCore.Boot;

namespace AspNetFlex.App.Boot
{
    public static class AppBootKey
    {
        public static readonly BootKey DatabaseBootableKey = new BootKey(nameof(DatabaseBootable)); 
    }
}