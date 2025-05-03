using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Domain.ValueObject.Project
{
    /// <summary>
    /// DTO для создания проекта
    /// </summary>
    public record ProjectCreateDto(
        [Required(ErrorMessage = "Project name is required")]
        [MaxLength(100, ErrorMessage = "Project name max length 100 characters")]
        string Name,

        [MaxLength(1000, ErrorMessage = "Description max length 1000 characters")]
        string Description,

        [Required(ErrorMessage = "Team ID is required")]
        int TeamId
    );
}
