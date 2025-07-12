using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoBackend.Database.Entities;

public sealed class RefreshToken : IEntity
{
    public Guid Id { get; set; }

    public Guid FamilyId { get; set; }
    public Guid? PreviousId { get; set; }
    public Guid? NextId { get; set; }
    public string Hash { get; set; } = null!;
    public DateTime IssuedAt { get; set; }
    public DateTime ExpiresAt { get; set; }

    // Navigation
    public RefreshTokenFamily Family { get; set; } = null!;
    public RefreshToken? Previous { get; set; }
    public RefreshToken? Next { get; set; }
}

internal class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasIndex(t => t.Hash).IsUnique();
        builder.HasOne(token => token.Family)
            .WithMany(family => family.RefreshTokens)
            .HasForeignKey(token => token.FamilyId);
        builder.HasOne(token => token.Previous)
            .WithOne(token => token.Next)
            .HasForeignKey<RefreshToken>(token => token.PreviousId);
        builder.HasOne(token => token.Next)
            .WithOne(token => token.Previous)
            .HasForeignKey<RefreshToken>(token => token.NextId);
    }
}