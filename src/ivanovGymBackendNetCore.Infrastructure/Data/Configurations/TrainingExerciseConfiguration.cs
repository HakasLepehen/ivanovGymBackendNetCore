using System;
using System.Collections.Generic;
using System.Text;
using ivanovGymBackendNetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ivanovGymBackendNetCore.Infrastructure.Data.Configurations;

class TrainingExerciseConfiguration : IEntityTypeConfiguration<TrainingExercise>
{
    public void Configure(EntityTypeBuilder<TrainingExercise> builder)
    {
        builder.ToTable("training_exercises");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(t => t.ExecutionNumber)
            .HasColumnName("execution_number")
            .HasColumnType("smallint")
            .HasDefaultValue(0);

        builder.Property(t => t.SetCount)
            .HasColumnType("text")
            .HasColumnName("set_count")
            .HasDefaultValue("");

        builder.Property(t => t.PayloadWeight)
            .HasColumnType("text")
            .HasColumnName("payload_weight")
            .HasDefaultValue("");

        builder.Property(t => t.Comment)
            .HasColumnType("text")
            .HasColumnName("comment")
            .HasDefaultValue("");

        builder.Property(t => t.ExerciseId)
            .HasColumnType("int")
            .HasColumnName("exercise_id")
            .IsRequired();

        builder.Property(c => c.TrainingId)
            .HasColumnType("int")
            .HasColumnName("training_id")
            .IsRequired();

        builder.HasOne(e => e.Exercise)
            .WithOne()
            .HasForeignKey<TrainingExercise>(e => e.ExerciseId)
            .HasPrincipalKey<Exercise>(t => t.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Training)
            .WithOne()
            .HasForeignKey<TrainingExercise>(e => e.TrainingId)
            .HasPrincipalKey<Training>(t => t.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
