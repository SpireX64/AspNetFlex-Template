using AspNetFlex.DatabaseStore.Contexts.App;
using Microsoft.EntityFrameworkCore;

namespace AspNetFlex.Data.UnitTests.Utils
{
    public static class DbContextFabric
    {
        public static AppDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new AppDbContext(options);
        }
    }
}