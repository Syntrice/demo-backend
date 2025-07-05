using DemoBackend.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Services;

public class DatabaseSeedingService : IDatabaseSeedingService
{
    private readonly List<Author> _authors;
    private readonly List<Book> _books;

    public DatabaseSeedingService()
    {
        _authors =
        [
            new Author { Id = Guid.NewGuid(), Name = "George Orwell" },
            new Author { Id = Guid.NewGuid(), Name = "Harper Lee" },
            new Author { Id = Guid.NewGuid(), Name = "F. Scott Fitzgerald" }
        ];

        _books =
        [
            new Book { Id = Guid.NewGuid(), Title = "1984", Authors = [_authors[0]] },
            new Book { Id = Guid.NewGuid(), Title = "To Kill a Mockingbird", Authors = [_authors[1]] },
            new Book { Id = Guid.NewGuid(), Title = "The Great Gatsby", Authors = [_authors[2]] }
        ];
    }

    public void Seed(DbContext context)
    {
        if (context.Set<Author>().Any()) return;
        context.AddRange(_authors);
        if (context.Set<Book>().Any()) return;
        context.AddRange(_books);
        context.SaveChanges();
    }

    public async Task SeedAsync(DbContext context)
    {
        if (context.Set<Author>().Any()) return;
        context.AddRange(_authors);
        if (context.Set<Book>().Any()) return;
        context.AddRange(_books);
        await context.SaveChangesAsync();
    }
}