using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoBackend.Database.Entities;

public sealed class User : IEntity
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public Auth Auth { get; set; } = null!;
}

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(user => user.Email).IsUnique();
        builder.HasIndex(user => user.Username).IsUnique();
    }
}