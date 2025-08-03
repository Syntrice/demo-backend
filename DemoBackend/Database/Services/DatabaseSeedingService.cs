using DemoBackend.Authorization;
using DemoBackend.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DemoBackend.Database.Services;

public class DatabaseSeedingService : IDatabaseSeedingService
{
    private readonly List<Author> _authors;
    private readonly List<Book> _books;
    private readonly List<Role> _roles;

    public DatabaseSeedingService()
    {
        _authors =
        [
            new Author { Name = "George Orwell" },
            new Author { Name = "Harper Lee" },
            new Author { Name = "F. Scott Fitzgerald" }
        ];

        _books =
        [
            new Book { Title = "1984", Authors = [_authors[0]] },
            new Book { Title = "To Kill a Mockingbird", Authors = [_authors[1]] },
            new Book { Title = "The Great Gatsby", Authors = [_authors[2]] }
        ];

        _roles =
        [
            new Role
            {
                Name = "User", Description = "Standard user with read-only access",
                Permissions = [Permission.ReadAuthors, Permission.ReadBooks]
            },
            new Role
            {
                Name = "Admin", Description = "Administrator with full access",
                Permissions = Enum.GetValues<Permission>()
            }
        ];
    }

    public void Seed(DbContext context)
    {
        if (context.Database.GetDbConnection() is NpgsqlConnection conn)
        {
            conn.ReloadTypes(); // Reload enums and other types
        }

        if (!context.Set<Author>().Any()) context.AddRange(_authors);
        if (!context.Set<Book>().Any()) context.AddRange(_books);
        if (!context.Set<Role>().Any()) context.AddRange(_roles);
        context.SaveChanges();
    }

    public async Task SeedAsync(DbContext context)
    {
        if (context.Database.GetDbConnection() is NpgsqlConnection conn)
        {
            await conn.ReloadTypesAsync(); // Reload enums and other types
        }

        if (!context.Set<Author>().Any()) context.AddRange(_authors);
        if (!context.Set<Book>().Any()) context.AddRange(_books);
        if (!context.Set<Role>().Any()) context.AddRange(_roles);
        await context.SaveChangesAsync();
    }
}