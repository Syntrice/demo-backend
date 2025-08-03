using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoBackend.Database.Entities;

public sealed class RefreshTokenFamily : IEntity<Guid>
{
    public Guid AuthId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }

    // Navigation
    public UserAccount UserAccount { get; set; } = null!;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public Guid Id { get; set; }
}

internal class
    RefreshTokenFamilyEntityTypeConfiguration : IEntityTypeConfiguration<RefreshTokenFamily>
{
    public void Configure(EntityTypeBuilder<RefreshTokenFamily> builder)
    {
        builder.HasOne(family => family.UserAccount)
            .WithMany(auth => auth.RefreshTokenFamilies)
            .HasForeignKey(family => family.AuthId);
    }
}