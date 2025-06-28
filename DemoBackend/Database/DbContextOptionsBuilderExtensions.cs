using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DemoBackend.Database;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder SeedDatabase(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSeeding((context, _) =>
        {
            context.AddRange(SeedData.Books);
            context.SaveChanges();
        });
        optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            context.AddRange(SeedData.Books);
            await context.SaveChangesAsync(cancellationToken);
        });
        return optionsBuilder;
    }
}