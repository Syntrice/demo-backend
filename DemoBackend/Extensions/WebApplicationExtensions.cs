using DemoBackend.Database;

namespace DemoBackend;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> DropAndCreateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        return app;
    }
}