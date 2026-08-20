using ivanovGymBackendNetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ivanovGymBackendNetCore.Infrastructure.Data.Configurations;

public class ConsultationRequestConfiguration : IEntityTypeConfiguration<ConsultationRequest>
{
    public void Configure(EntityTypeBuilder<ConsultationRequest> builder)
    {
        builder.ToTable("consultation_requests");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasColumnType("text")
            .HasDefaultValue("")
            .IsRequired(true);

        builder.Property(c => c.Phone)
            .HasColumnName("phone")
            .HasColumnType("text")
            .HasDefaultValue("")
            .IsRequired(true);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()");

        builder.Property(c => c.IsCalled)
            .HasColumnName("is_called")
            .HasColumnType("boolean")
            .HasDefaultValue(false);
    }
}
