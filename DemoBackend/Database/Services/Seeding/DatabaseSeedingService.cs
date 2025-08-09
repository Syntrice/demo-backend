using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DemoBackend.Database.Services.Seeding;

public class DatabaseSeedingService(
    IEnumerable<ISeeder> seeders,
    ILogger<DatabaseSeedingService> logger)
    : IDatabaseSeedingService
{
    private readonly IReadOnlyCollection<ISeeder>
        _sortedSeeders = TopologicalSort(seeders.ToList());

    public void Seed(DbContext context)
    {
        if (context.Database.GetDbConnection() is NpgsqlConnection conn)
            foreach (var seeder in _sortedSeeders)
            {
                conn.ReloadTypes(); // Reload enums and other types - this is needed for NPGSQL seeding 
                var seederName = seeder.GetType().Name.Replace("Seeder", "");
                logger.LogInformation("Seeding {Name}", seederName);
                seeder.Add(context);
                seeder.Seed(context);
            }

        logger.LogInformation("Seeding complete");
    }

    public async Task SeedAsync(DbContext context)
    {
        if (context.Database.GetDbConnection() is NpgsqlConnection conn)
            foreach (var seeder in _sortedSeeders)
            {
                await conn
                    .ReloadTypesAsync(); // Reload enums and other types - this is needed for NPGSQL seeding 
                var seederName = seeder.GetType().Name.Replace("Seeder", "");
                logger.LogInformation("Seeding {Name}", seederName);
                seeder.Add(context);
                await seeder.SeedAsync(context);
            }

        logger.LogInformation("Seeding complete");
    }

    private static List<ISeeder> TopologicalSort(List<ISeeder> seeders)
    {
        var sorted = new List<ISeeder>();
        var visited = new HashSet<ISeeder>();

        foreach (var seeder in seeders)
        {
            Visit(seeder);
        }

        return sorted;

        void Visit(ISeeder seeder)
        {
            if (visited.Add(seeder))
            {
                foreach (var dependency in seeder.DependsOn)
                {
                    var dependentSeeder = seeders.FirstOrDefault(s => s.GetType() == dependency);
                    if (dependentSeeder != null)
                    {
                        Visit(dependentSeeder);
                    }
                }

                sorted.Add(seeder);
            }
            else if (!sorted.Contains(seeder))
            {
                throw new InvalidOperationException("Cyclic dependency detected in seeders.");
            }
        }
    }
}