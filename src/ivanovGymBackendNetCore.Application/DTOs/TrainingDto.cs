namespace ivanovGymBackendNetCore.Application.DTOs;

// <summary>
// ДТО тренировки
// </summary>
public class TrainingDto
{
    // <summary>
    // Идентификатор тренировки
    // </summary>
    public int? Id { get; set; }

    // <summary>
    // Идентификатор клиента, к которому привязана тренировка
    // </summary>
    public Guid ClientGuid { get; set; }

    // <summary>
    // Дата запланированной тренировки
    // </summary>
    public DateTime PlannedDate { get; set; }

    // <summary>
    // Завершена ли тренировка
    // </summary>
    public bool IsCompleted { get; set; } = false;

    // <summary>
    // Список упражнений привязанных к 
    // </summary>
    public List<TrainingExerciseDto>? Exercises { get; set; } = new List<TrainingExerciseDto>();
}
