using System;

namespace ivanovGymBackendNetCore.Application.DTOs
{
    public class LastExerciseDto
    {
        public Guid Client { get; set; }
        public string? Repetition { get; set; }
    }
}