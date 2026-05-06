using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ivanovGymBackendNetCore.Domain.Entities;

namespace ivanovGymBackendNetCore.Infrastructure.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("members");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.Phone)
            .HasMaxLength(20);

        builder.HasIndex(m => m.Email)
            .IsUnique();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt);
    }
}
