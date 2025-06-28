using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DemoBackend.Database.Entities;

namespace DemoBackend.Database;

public static class SeedData
{
    // Create seed data for books, which should correspond to Book entity,
    // So they should have a string Title and string Author fields
    public static readonly List<Book> Books = [
        new Book { Id = Guid.NewGuid(), Title = "1984", Author = "George Orwell" },
        new Book { Id = Guid.NewGuid(), Title = "To Kill a Mockingbird", Author = "Harper Lee" },
        new Book { Id = Guid.NewGuid(), Title = "The Great Gatsby", Author = "F. Scott Fitzgerald" }
    ];
}
