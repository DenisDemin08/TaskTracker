using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts.Repositories
{
    /// <summary>
    /// Репозиторий для работы с администраторами
    /// </summary>
    public interface IAdministratorRepository : IRepository<Administrators>
    {
        /// <summary>
        /// Получить администратора по идентификатору пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Сущность администратора или null</returns>
        Task<Administrators?> GetByUserIdAsync(int userId);
    }
}