using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Domain.ValueObject.User
{
    /// <summary>
    /// DTO для аутентификации пользователя
    /// </summary>
    public record UserAuthDto(
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        string Email,

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        string Password
    );
}
