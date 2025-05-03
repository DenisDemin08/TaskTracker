using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Domain.ValueObject.Project
{
    /// <summary>
    /// DTO для назначения команды на проект
    /// </summary>
    public record ProjectAssignmentDto(
        [Required(ErrorMessage = "Project ID is required")]
        int ProjectId,

        [Required(ErrorMessage = "Team ID is required")]
        int TeamId
    );
}
