using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Seeders;

public interface ISeeder
{
    void Seed(DbContext context);
    Task SeedAsync(DbContext context);
}