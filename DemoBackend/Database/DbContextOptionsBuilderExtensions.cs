using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DemoBackend.Database;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder SeedDatabase(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSeeding((context, _) =>
        {
            SeedData.SeedBooks(context);
        });
        optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            SeedData.SeedBooks(context);
            await Task.CompletedTask;
        });
        return optionsBuilder;
    }
}
