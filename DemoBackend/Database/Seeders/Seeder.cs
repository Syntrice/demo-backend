using DemoBackend.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Seeders;

public class Seeder<TEntity>(List<TEntity> seedData) : ISeeder
    where TEntity : class, IEntity
{
    public Seeder() : this([])
    {
    }

    public void Seed(DbContext context)
    {
        if (!context.Set<TEntity>().Any())
        {
            context.AddRange(seedData);
            context.SaveChanges();
        }
    }

    public async Task SeedAsync(DbContext context)
    {
        if (!context.Set<TEntity>().Any())
        {
            context.AddRange(seedData);
            await context.SaveChangesAsync();
        }
    }
}