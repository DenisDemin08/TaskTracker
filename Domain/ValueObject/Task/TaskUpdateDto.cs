using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.ValueObject.Task
{
    /// <summary>
    /// DTO для обновления задачи
    /// </summary>
    public record TaskUpdateDto(
        int TaskId,
        string? Title,
        string? Description,
        int? AssigneeId,
        DateTime? Deadline,
        TaskPriority? Priority,
        TaskStatus? Status
    );
}
