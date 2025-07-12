using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoBackend.Database.Entities;

public class Author : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();
}

internal class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
    }
}