using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoBackend.Database.Entities;

public sealed class UserProfile : IEntity
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string DisplayName { get; set; } = string.Empty;

    // Navigation
    public UserAccount UserAccount { get; set; } = null!;
}

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasIndex(user => user.DisplayName).IsUnique();
        builder.HasOne(profile => profile.UserAccount).WithOne(account => account.UserProfile)
            .HasForeignKey<UserProfile>(profile => profile.AccountId);
    }
}