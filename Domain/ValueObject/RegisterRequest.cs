using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.ValueObject
{
    /// <summary>
    /// Модель запроса для регистрации пользователя
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        /// <example>user@example.com</example>
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Пароль (8-100 символов)
        /// </summary>
        /// <example>P@ssw0rd123</example>
        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Пароль должен быть от 8 до 100 символов")]
        public string Password { get; set; } = null!;

        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        /// <example>Иванов Иван Иванович</example>
        [Required(ErrorMessage = "ФИО обязательно")]
        [StringLength(100, ErrorMessage = "ФИО не должно превышать 100 символов")]
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Должность в компании (опционально)
        /// </summary>
        /// <example>Backend</example>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MemberPosition? Position { get; set; }
    }
}
