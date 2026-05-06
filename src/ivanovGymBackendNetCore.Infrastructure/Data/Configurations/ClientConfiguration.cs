using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ivanovGymBackendNetCore.Domain.Entities;

namespace ivanovGymBackendNetCore.Infrastructure.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .IsRequired(false);

        builder.Property(c => c.FullName)
            .HasColumnName("fullName")
            .HasColumnType("text")
            .HasDefaultValue("")
            .IsRequired();

        builder.Property(c => c.Age)
            .HasColumnType("text")
            .HasDefaultValue("");

        builder.Property(c => c.Target)
            .HasColumnType("text")
            .HasDefaultValue("");

        builder.Property(c => c.Experience)
            .HasColumnType("text")
            .HasDefaultValue("");

        builder.Property(c => c.Sleep)
            .HasColumnType("text")
            .HasDefaultValue("");

        builder.Property(c => c.Food)
            .HasColumnType("text")
            .HasDefaultValue("");

        builder.Property(c => c.Pharma)
            .HasColumnType("text")
            .HasDefaultValue("");

        builder.Property(c => c.Activity)
            .HasColumnType("text")
            .HasDefaultValue("");

        builder.Property(c => c.Avatar)
            .HasColumnType("text")
            .HasDefaultValue("");

        builder.Property(c => c.Guid)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder.Property(c => c.Limits)
            .HasColumnType("smallint[]");

        builder.Property(c => c.IsActive)
            .HasColumnName("isActive")
            .HasDefaultValue(true)
            .IsRequired();
    }
}
