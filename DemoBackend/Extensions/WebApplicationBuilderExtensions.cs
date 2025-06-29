using DemoBackend.Database;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder SetupApplicationDbContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        if (connectionString == null)
        {
            throw new InvalidOperationException("No connection string configured");
        }

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            var seeder = new DatabaseSeeder();
            options.UseSeeding((context, _) => { seeder.Seed(context); });
            options.UseAsyncSeeding(async (context, _, cancellationToken) => { await seeder.SeedAsync(context); });
        });
        return builder;
    }
}