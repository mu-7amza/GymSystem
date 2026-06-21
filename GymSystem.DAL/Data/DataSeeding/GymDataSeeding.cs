using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymSystem.DAL.Data.DataSeeding
{
    public static class GymDataSeeding
    {
        public static async Task SeedAsync(GymDbContext _context , string seedfolderPath , ILogger logger , CancellationToken ct = default)
        {
            try
            {
                if(!await _context.Plans.AnyAsync())
                {
                    var plans = await LoadDataFromjsonFileAsync<Plan>(seedfolderPath,"plans.json");
                    if (plans.Any())
                    {
                       await _context.Plans.AddRangeAsync(plans,ct);

                    }

                    if (_context.ChangeTracker.HasChanges())
                        await _context.SaveChangesAsync(ct);
                    else
                        logger.LogInformation("plan already seeded");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "seed data Failed");
                throw;
            }
        }

        public static async Task<List<T>> LoadDataFromjsonFileAsync<T>(string seedfolderPath, string fileName)
        {
            var filePath = Path.Combine(seedfolderPath, fileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Seed data file not found , FilePath : {filePath}");
            var data = await File.ReadAllTextAsync(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var plans = JsonSerializer.Deserialize<List<T>>(data, options);
            return plans ?? [];
        }

    }
}
