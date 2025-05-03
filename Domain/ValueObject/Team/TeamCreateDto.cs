using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Domain.ValueObject.Team
{
    /// <summary>
    /// DTO для создания команды
    /// </summary>
    public record TeamCreateDto(
        [Required(ErrorMessage = "Team name is required")]
        [MaxLength(50, ErrorMessage = "Team name max length 50 characters")]
        string Name,

        [MaxLength(200, ErrorMessage = "Description max length 200 characters")]
        string? Description
    );
}
