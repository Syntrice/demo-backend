using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoBackend.Database.Entities;

public sealed class Auth : IEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public string PasswordHash { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public User User { get; set; } = default!;
    public ICollection<RefreshTokenFamily> RefreshTokenFamilies { get; set; } = new List<RefreshTokenFamily>();
}

internal class AuthEntityTypeConfiguration : IEntityTypeConfiguration<Auth>
{
    public void Configure(EntityTypeBuilder<Auth> builder)
    {
        builder.HasOne(auth => auth.User)
            .WithOne(user => user.Auth)
            .HasForeignKey<Auth>(auth => auth.UserId);
    }
}