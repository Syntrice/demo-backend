using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoBackend.Database.Entities;

public sealed class UserAccount : IEntity
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int HashSaltSize { get; set; }
    public int HashSize { get; set; }
    public int HashIterations { get; set; }

    public string HashAlgorithm { get; set; } = string.Empty;

    // Navigation
    public UserProfile UserProfile { get; set; } = null!;

    public ICollection<RefreshTokenFamily> RefreshTokenFamilies { get; set; } =
        new List<RefreshTokenFamily>();
}

internal class AuthEntityTypeConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
    }
}