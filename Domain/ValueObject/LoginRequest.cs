using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Domain.ValueObject
{
    /// <summary>
    /// Модель запроса для аутентификации
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        /// <example>user@example.com</example>
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        /// <example>P@ssw0rd123</example>
        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; } = null!;
    }
}
