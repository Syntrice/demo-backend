using DemoBackend.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }  
}