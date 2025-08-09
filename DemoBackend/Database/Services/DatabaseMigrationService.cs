using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Services;

public sealed class DatabaseMigrationService(
    IServiceProvider serviceProvider,
    ILogger<DatabaseMigrationService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        logger.LogInformation("Applying migrations");
        await dbContext.Database.MigrateAsync(ct);
        logger.LogInformation("Applying migrations complete");
    }

    public Task StopAsync(CancellationToken _)
    {
        return Task.CompletedTask;
    }
}