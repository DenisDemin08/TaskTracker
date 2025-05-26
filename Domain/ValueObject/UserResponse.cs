using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.ValueObject
{
    /// <summary>
    /// Модель ответа с данными пользователя
    /// </summary>
    public class UserResponse(Users user)
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        /// <example>123</example>
        public int UserId { get; } = user.UserId;

        /// <summary>
        /// Электронная почта
        /// </summary>
        /// <example>user@example.com</example>
        public string Email { get; } = user.Email;

        /// <summary>
        /// Полное имя
        /// </summary>
        /// <example>Иванов Иван Иванович</example>
        public string FullName { get; } = user.FullName;

        /// <summary>
        /// Роль пользователя
        /// </summary>
        /// <example>Developer</example>
        public UserRole Role { get; } = user.Role;
    }
}
