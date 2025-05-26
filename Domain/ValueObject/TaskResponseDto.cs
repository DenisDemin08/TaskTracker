using TaskTracker.Domain.Enums;
namespace TaskTracker.Domain.ValueObject
{
    public class TaskResponseDto
    {
        public int TaskId { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateOnly Deadline { get; set; }
    }
}
