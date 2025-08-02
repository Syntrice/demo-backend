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
        await dbContext.Database.MigrateAsync(cancellationToken: ct);
    }

    public Task StopAsync(CancellationToken _) => Task.CompletedTask;
}