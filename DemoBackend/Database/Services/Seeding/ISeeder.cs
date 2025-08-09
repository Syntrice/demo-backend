using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Services.Seeding;

public interface ISeeder
{
    IReadOnlyCollection<Type> DependsOn { get; }
    void Add(DbContext context);

    void Seed(DbContext context)
    {
        context.SaveChanges();
    }

    async Task SeedAsync(DbContext context)
    {
        await context.SaveChangesAsync();
    }
}