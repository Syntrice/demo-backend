using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DemoBackend.Database.Entities;
using DemoBackend.Database.Seeders;

namespace DemoBackend.Database;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder SeedDatabase(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSeeding((context, _) =>
        {
            new Seeder().Seed(context);
        });
        optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
           await new Seeder().SeedAsync(context);
        });
        return optionsBuilder;
    }
}