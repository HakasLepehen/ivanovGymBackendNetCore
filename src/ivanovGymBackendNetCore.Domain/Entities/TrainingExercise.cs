using System;
using System.Collections.Generic;
using System.Text;

namespace ivanovGymBackendNetCore.Domain.Entities;

class TrainingExercise
{
    public int Id { get; set; }
    public int TrainingId { get; set; }
    public int ExerciseId { get; set; }
    public byte? ExecutionNumber { get; set; } = 0;
    public string? SetCount { get; set; } = "";
    public string? PayloadWeight { get; set; } = "";
    public string? Comment { get; set; } = "";
}
