using DemoBackend.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Seeders;

public class AuthorSeeder() : Seeder<Author>([
    new Author { Id = Guid.NewGuid(), Name = "George Orwell" },
    new Author { Id = Guid.NewGuid(), Name = "Harper Lee" },
    new Author { Id = Guid.NewGuid(), Name = "F. Scott Fitzgerald" }
]);