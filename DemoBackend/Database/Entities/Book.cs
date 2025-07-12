using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoBackend.Database.Entities;

public class Book : IEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public ICollection<Author> Authors { get; set; } = new List<Author>();
}

internal class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder
            .HasMany(b => b.Authors)
            .WithMany(a => a.Books)
            .UsingEntity(j => j.ToTable("BookAuthors"));
    }
}