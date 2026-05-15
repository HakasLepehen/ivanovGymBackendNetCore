using ivanovGymBackendNetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ivanovGymBackendNetCore.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(u => u.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Email)
            .HasColumnType("citext")
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Roles)
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

