using TaskTracker.Domain.Entities;
namespace TaskTracker.Domain.ValueObject.Project
{
    public class ProjectDetailsDto
    {
        public string? Name { get; set; }
        public required string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public required List<Tasks> Tasks { get; set; }
        public required List<Teams> Teams { get; set; }
    }
}