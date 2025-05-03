using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.ValueObject.Task
{
    public record TaskCreateDto(
       [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title max length 100 characters")]
        string Title,

       [MaxLength(500, ErrorMessage = "Description max length 500 characters")]
        string Description,

       [Required(ErrorMessage = "Assignee ID is required")]
        int AssigneeId,

       [Required(ErrorMessage = "Project ID is required")]
        int ProjectId,

       DateTime? Deadline,
       TaskPriority Priority = TaskPriority.Medium
   );
}
