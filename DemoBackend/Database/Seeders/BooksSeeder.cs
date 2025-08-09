using DemoBackend.Database.Entities;
using DemoBackend.Database.Services.Seeding;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Seeders;

public class BooksSeeder : ISeeder
{
    public IReadOnlyCollection<Type> DependsOn => [typeof(AuthorsSeeder)];

    public void Add(DbContext context)
    {
        if (context.Set<Book>().Any()) return;
        var authors = context.Set<Author>().ToList();
        var books = new List<Book>
        {
            new Book { Title = "1984", Authors = new List<Author> { authors[0] } },
            new Book { Title = "To Kill a Mockingbird", Authors = new List<Author> { authors[1] } },
            new Book { Title = "The Great Gatsby", Authors = new List<Author> { authors[2] } }
        };
        context.AddRange(books);
    }
}