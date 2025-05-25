using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Services.Contracts
{

    /// <summary>
    /// Сервис аутентификации и авторизации
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <remarks>
        /// Создает пользователя и соответствующую запись 
        /// в таблице выбранной роли (Administrator/Manager/Employee)
        /// </remarks>
        Task<Users> RegisterAsync(
            string email,
            string password,
            string fullName,
            UserRole role,
            MemberPosition? position);

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <returns>
        /// Кортеж с JWT токеном и временем его истечения
        /// </returns>
        Task <Users> LoginAsync(
            string email,
            string password);
    }
}