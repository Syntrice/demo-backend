using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoBackend.Database.Entities;

public class Author : IEntity<Guid>
{
    public string Name { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();
    public Guid Id { get; set; }
}

internal class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
    }
}