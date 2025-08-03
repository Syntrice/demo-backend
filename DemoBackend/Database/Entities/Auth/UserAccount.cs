using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoBackend.Database.Entities.Auth;

public sealed class UserAccount : IEntity<Guid>
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int HashSaltSize { get; set; }
    public int HashSize { get; set; }
    public int HashIterations { get; set; }
    public string HashAlgorithm { get; set; } = string.Empty;
    public UserProfile UserProfile { get; set; } = null!;
    public ICollection<Role> Roles { get; set; } = new List<Role>();

    public ICollection<RefreshTokenFamily> RefreshTokenFamilies { get; set; } =
        new List<RefreshTokenFamily>();

    public Guid Id { get; set; }
}

internal class AuthEntityTypeConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        // Configure many-to-many with Roles
        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.ToTable("UserRoles"));
        builder.Property(u => u.Email).HasMaxLength(254);
        builder.Property(u => u.PasswordHash).HasMaxLength(300);
        builder.Property(u => u.HashAlgorithm).HasMaxLength(50);
    }
}