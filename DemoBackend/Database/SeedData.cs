using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DemoBackend.Database.Entities;

namespace DemoBackend.Database;

public static class SeedData
{
    public static readonly List<Book> Books =
    [
        new Book { Id = Guid.NewGuid(), Title = "1984", Author = "George Orwell" },
        new Book { Id = Guid.NewGuid(), Title = "To Kill a Mockingbird", Author = "Harper Lee" },
        new Book { Id = Guid.NewGuid(), Title = "The Great Gatsby", Author = "F. Scott Fitzgerald" }
    ];

    public static readonly List<Author> Authors =
    [
        new Author { Id = Guid.NewGuid(), Name = "George Orwell" },
        new Author { Id = Guid.NewGuid(), Name = "Harper Lee" },
        new Author { Id = Guid.NewGuid(), Name = "F. Scott Fitzgerald" }
    ];
}