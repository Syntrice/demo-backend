using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database;

public static class DatabaseExtensions
{
    public static async Task<WebApplication> EnsureDatabaseCreatedAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        return app;
    }

    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            var seeder = new DatabaseSeeder();
            options.UseSeeding((context, _) => { seeder.Seed(context); });
            options.UseAsyncSeeding(async (context, _, cancellationToken) => { await seeder.SeedAsync(context); });
        });
        return services;
    }
}