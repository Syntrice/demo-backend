using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Services.Seeding;

public interface IDatabaseSeedingService
{
    void Seed(DbContext context);
    Task SeedAsync(DbContext context);
}