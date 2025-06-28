using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder SeedDatabase(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSeeding((context, _) =>
        {
            // Call upon the SeedData static class here to seed the database
        });
        optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            // Call upon the SeedData static class here to seed the database
        });
        return optionsBuilder;
    }
}