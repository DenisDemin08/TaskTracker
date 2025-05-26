using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.ValueObject
{
    public record ProjectListItemDto(
        int ProjectId,
        string Name,
        DateOnly StartDate,
        DateOnly? EndDate)
    {
        public ProjectListItemDto(Projects project) : this(
            project.ProjectId,
            project.Name ?? string.Empty,
            project.StartDate,
            project.EndDate)
        {
        }
    }
}
