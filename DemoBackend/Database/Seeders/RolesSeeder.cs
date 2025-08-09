using DemoBackend.Authorization;
using DemoBackend.Database.Entities.Auth;
using DemoBackend.Database.Services.Seeding;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Seeders;

public class RolesSeeder : ISeeder
{
    public IReadOnlyCollection<Type> DependsOn => [];

    public void Add(DbContext context)
    {
        if (context.Set<Role>().Any()) return;
        var roles = new List<Role>
        {
            new Role
            {
                Name = "User", Description = "Standard user with read-only access",
                Permissions = [Permission.ReadAuthors, Permission.ReadBooks],
                IsDefault = true
            },
            new Role
            {
                Name = "Admin", Description = "Administrator with full access",
                Permissions = Enum.GetValues<Permission>()
            }
        };
        context.AddRange(roles);
    }
}