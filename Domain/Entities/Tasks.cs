using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities
{
    public class Tasks
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = null!;
        public string? TaskDescription { get; set; }
        public Taskstatus TaskStatus { get; set; }
        public TaskPriority TaskPriority { get; set; }
        public DateOnly Deadline { get; set; }

        public int CreatorId { get; set; }
        public int? AssigneeId { get; set; }
        public int ProjectId { get; set; }
    }
}
 