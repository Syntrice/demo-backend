using DemoBackend.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Seeders;

public class BookSeeder() : Seeder<Book>([
    new Book { Id = Guid.NewGuid(), Title = "1984", Author = "George Orwell" },
    new Book { Id = Guid.NewGuid(), Title = "To Kill a Mockingbird", Author = "Harper Lee" },
    new Book { Id = Guid.NewGuid(), Title = "The Great Gatsby", Author = "F. Scott Fitzgerald" }
]);