using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoBackend.Database.Entities;

public sealed class RefreshTokenFamily : IEntity
{
    public Guid Id { get; set; }

    public Guid AuthId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }

    // Navigation
    public Auth Auth { get; set; } = null!;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}

internal class RefreshTokenFamilyEntityTypeConfiguration : IEntityTypeConfiguration<RefreshTokenFamily>
{
    public void Configure(EntityTypeBuilder<RefreshTokenFamily> builder)
    {
        builder.HasOne(family => family.Auth)
            .WithMany(auth => auth.RefreshTokenFamilies)
            .HasForeignKey(family => family.AuthId);
    }
}