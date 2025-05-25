using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts.Repositories
{
    /// <summary>
    /// Репозиторий для работы с пользователями
    /// </summary>
    public interface IUserRepository : IRepository<Users>
    {
        /// <summary>
        /// Найти пользователя по email
        /// </summary>
        /// <param name="email">Электронная почта</param>
        /// <returns>Найденный пользователь или null</returns>
        Task<Users?> GetByEmailAsync(string email);

        /// <summary>
        /// Найти пользователя по полному имени
        /// </summary>
        /// <param name="fullName">Полное имя</param>
        /// <returns>Найденный пользователь или null</returns>
        Task<Users?> GetByFullNameAsync(string fullName);

        /// <summary>
        /// Получить всех администраторов
        /// </summary>
        /// <returns>Список администраторов</returns>
        Task<List<Administrators>> GetAdministratorsAsync();

        /// <summary>
        /// Получить всех менеджеров
        /// </summary>
        /// <returns>Список менеджеров</returns>
        Task<List<Managers>> GetManagersAsync();

        /// <summary>
        /// Получить членов команды
        /// </summary>
        /// <param name="teamId">Идентификатор команды</param>
        /// <returns>Список сотрудников</returns>
        Task<List<Employees>> GetTeamMembersByTeamAsync(int teamId);
    }
}