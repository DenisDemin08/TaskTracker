using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.ValueObject
{
    public class ProjectDetailsDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public IEnumerable<Task> Tasks { get; set; } = [];
        public IEnumerable<Teams> Teams { get; set; } = [];
    }
}
