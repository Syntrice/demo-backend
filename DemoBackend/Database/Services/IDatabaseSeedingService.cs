using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Services;

public interface IDatabaseSeedingService
{
    void Seed(DbContext context);
    Task SeedAsync(DbContext context);
}