using GymSystem.DAL.Data.DataSeeding;
using GymSystem.DAL.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.PL
{
    public static class ProgramExtensions
    {
        public static async Task MigrateAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var pendingMigration = dbContext.Database.GetPendingMigrations();

            if (pendingMigration.Any())
            {
                await dbContext.Database.MigrateAsync();
            }
            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbContext, seedFolderPath, logger);
        }
    }
}
