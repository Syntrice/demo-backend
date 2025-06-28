using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DemoBackend.Database.Entities;
using DemoBackend.Database.Seeders;

namespace DemoBackend.Database;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder SeedDatabase(this DbContextOptionsBuilder optionsBuilder)
    {
        List<ISeeder> seeders =
        [
            new BookSeeder(),
            new AuthorSeeder(),
        ];

        optionsBuilder.UseSeeding((context, _) =>
        {
            foreach (var seeder in seeders)
            {
                seeder.Seed(context);
            }
        });
        optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(context);
            }
        });
        return optionsBuilder;
    }
}