using System.Linq;
using System.Threading.Tasks;
using AspNetFlex.DatabaseStore.Contexts.App;
using Microsoft.EntityFrameworkCore;
using SpireX.AspNetCore.Boot;

namespace AspNetFlex.App.Boot
{
    /**
     * Checks database connection and performs migrations
     */
    
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DatabaseBootable : Bootable
    {
        private readonly AppDbContext _dbContext;
        public override BootKey Key { get; } = AppBootKey.DatabaseBootableKey;

        public DatabaseBootable(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public override Task Boot()
        {
            return _dbContext.Database.GetPendingMigrations().Any() 
                ? _dbContext.Database.MigrateAsync() 
                : Task.CompletedTask;
        }

    }
}