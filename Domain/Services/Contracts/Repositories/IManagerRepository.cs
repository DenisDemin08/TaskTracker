using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts.Repositories
{
    /// <summary>
    /// Репозиторий для работы с менеджерами
    /// </summary>
    public interface IManagerRepository : IRepository<Managers>
    {
        /// <summary>
        /// Получить менеджера по идентификатору пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Сущность менеджера или null</returns>
        Task<Managers?> GetByUserIdAsync(int userId);
    }
}