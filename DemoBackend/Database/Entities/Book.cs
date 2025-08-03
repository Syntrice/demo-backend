using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoBackend.Database.Entities;

public class Book : IEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public ICollection<Author> Authors { get; set; } = new List<Author>();
    public Guid Id { get; set; }
}

internal class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder
            .HasMany(b => b.Authors)
            .WithMany(a => a.Books)
            .UsingEntity(j => j.ToTable("BookAuthors"));
        builder.Property(book => book.Title).HasMaxLength(100);
    }
}