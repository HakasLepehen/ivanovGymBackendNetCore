using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ivanovGymBackendNetCore.Domain.Entities;
using ivanovGymBackendNetCore.Domain.Enums;

namespace ivanovGymBackendNetCore.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Email)
            .HasColumnType("citext")
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Password)
            .HasColumnType("varchar")
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .HasColumnName("createdAt")
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(u => u.Roles)
            .HasConversion(
                v => v.Select(r => r.ToString().ToLower()).ToArray(),
                v => v.Select(r => (UserRole)Enum.Parse(typeof(UserRole), r, true)).ToList())
            .HasColumnType("text[]")
            .HasDefaultValueSql("ARRAY['user']::text[]");

        builder.Property(u => u.Token)
            .HasColumnName("token")
            .HasColumnType("varchar")
            .IsRequired(false);

        builder.Property(u => u.ClientFkId)
            .HasColumnName("client_fk_id")
            .HasColumnType("uuid")
            .IsRequired(false);

        builder.HasIndex(u => u.ClientFkId)
            .IsUnique();
    }
}

