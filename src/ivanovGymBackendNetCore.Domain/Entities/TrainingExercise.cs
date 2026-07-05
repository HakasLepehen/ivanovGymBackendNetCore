using System;
using System.Collections.Generic;
using System.Text;

namespace ivanovGymBackendNetCore.Domain.Entities;

public class TrainingExercise
{
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор связанной тренировки
    /// </summary>
    public int TrainingId { get; set; }

    /// <summary>
    /// Идентификатор связанного упражнения
    /// </summary>
    public int ExerciseId { get; set; }

    /// <summary>
    /// Количество повторений внутри подхода
    /// </summary>
    public byte? ExecutionNumber { get; set; } = 0;

    /// <summary>
    /// Количество подходов
    /// </summary>
    public string? SetCount { get; set; } = "";

    /// <summary>
    /// Вес, нагрузка выполняемого упражнения 
    /// </summary>
    public string? PayloadWeight { get; set; } = "";

    /// <summary>
    /// Комментарий к выполняемому упражнению
    /// </summary>
    public string? Comment { get; set; } = "";

    /// <summary>
    /// Связанное упражнение
    /// </summary>
    public Exercise Exercise { get; set; }

    /// <summary>
    /// Связанная тренировка
    /// </summary>
    public Training Training { get; set; }
}
