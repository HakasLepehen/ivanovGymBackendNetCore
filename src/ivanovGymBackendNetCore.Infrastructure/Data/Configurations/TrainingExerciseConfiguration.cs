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
    }
}
