using DemoBackend.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database;

public class ApplicationDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
}