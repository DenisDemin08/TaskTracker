using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities
{
    /// <summary>
    /// Представляет пользователя системы
    /// </summary>
    [Table("Users")]
    [Index(nameof(Email), IsUnique = true)]
    public class Users
    {
        /// <summary>Уникальный идентификатор пользователя</summary>
        public int UserId { get; set; }

        /// <summary>Электронная почта (уникальная)</summary>
        public required string Email { get; set; }

        /// <summary>Хэш пароля</summary>
        public required string PasswordHash { get; set; }

        /// <summary>Полное имя пользователя</summary>
        public required string FullName { get; set; }

        /// <summary>Роль в системе</summary>
        public UserRole Role { get; set; }
    }
}