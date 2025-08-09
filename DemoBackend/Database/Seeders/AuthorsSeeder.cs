using DemoBackend.Database.Entities;
using DemoBackend.Database.Services.Seeding;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Seeders;

public class AuthorsSeeder : ISeeder
{
    public IReadOnlyCollection<Type> DependsOn => [];

    public void Add(DbContext context)
    {
        if (context.Set<Author>().Any()) return;
        var authors = new List<Author>
        {
            new Author { Name = "George Orwell" },
            new Author { Name = "Harper Lee" },
            new Author { Name = "F. Scott Fitzgerald" }
        };
        context.AddRange(authors);
    }
}