using ivanovGymBackendNetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ivanovGymBackendNetCore.Infrastructure.Data.Configurations;

public class TrainingConfiguration : IEntityTypeConfiguration<Training>
{
    public void Configure(EntityTypeBuilder<Training> builder)
    {
        builder.ToTable("trainings");

        builder.HasKey(t => t.Id);

        builder.Property(c => c.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(c => c.PlannedDate)
            .HasColumnName("planned_date")
            .HasColumnType("timestamptz")
            .IsRequired(true);

        builder.Property(c => c.IsCompleted)
            .HasColumnName("is_completed")
            .HasColumnType("boolean")
            .IsRequired();

        builder.Property(e => e.ClientGuid)
            .HasColumnName("client_guid")
            .HasColumnType("uuid")
            .IsRequired();

        builder.HasOne(e => e.Client)
            .WithMany()
            .HasForeignKey(e => e.ClientGuid)
            .HasPrincipalKey(c => c.Guid)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
