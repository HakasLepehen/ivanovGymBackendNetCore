using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using ivanovGymBackendNetCore.Domain.Entities;

namespace ivanovGymBackendNetCore.Application.DTOs;

public class TrainingExerciseDto
{
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор связанной тренировки
    /// </summary>
    [JsonPropertyName("training_id")]
    public int TrainingId { get; set; }

    /// <summary>
    /// Идентификатор связанного упражнения
    /// </summary>
    [JsonPropertyName("exercise_id")]
    public int ExerciseId { get; set; }

    /// <summary>
    /// Количество повторений внутри подхода
    /// </summary>
    [JsonPropertyName("execution_number")]
    public string? ExecutionNumber { get; set; } = "";

    /// <summary>
    /// Количество подходов
    /// </summary>
    [JsonPropertyName("set_count")]
    public string? SetCount { get; set; } = "";

    /// <summary>
    /// Вес, нагрузка выполняемого упражнения 
    /// </summary>
    [JsonPropertyName("payload_weight")]
    public string? PayloadWeight { get; set; } = "";

    /// <summary>
    /// Комментарий к выполняемому упражнению
    /// </summary>
    [JsonPropertyName("comment")] 
    public string? Comment { get; set; } = "";
}
