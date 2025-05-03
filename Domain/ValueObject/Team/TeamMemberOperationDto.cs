using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Domain.ValueObject.Team
{
    /// <summary>
    /// DTO для операций с участниками команды
    /// </summary>
    public record TeamMemberOperationDto(
        [Required(ErrorMessage = "User ID is required")]
        int UserId,

        [Required(ErrorMessage = "Team ID is required")]
        int TeamId
    );
}
