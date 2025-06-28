using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder SeedDatabase(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSeeding((context, _) =>
        {
            
        });
        optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            
        });
        return optionsBuilder;
    }
}