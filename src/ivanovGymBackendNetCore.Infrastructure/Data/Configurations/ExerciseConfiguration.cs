using ivanovGymBackendNetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ivanovGymBackendNetCore.Infrastructure.Data.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("exercises");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(e => e.Name)
            .IsRequired(true)
            .HasMaxLength(255);

        builder.Property(e => e.Url)
            .HasColumnName("url")
            .HasColumnType("text")
            .HasDefaultValue("")
            .IsRequired(false);

        builder.Property(e => e.Comment)
            .HasColumnName("comment")
            .HasColumnType("text")
            .HasDefaultValue("")
            .IsRequired(false);

        builder.Property(e => e.MuscleGroup)
            .HasColumnName("muscle_group")
            .HasColumnType("int")
            .HasDefaultValue(null)
            .IsRequired(false);
    }
}
