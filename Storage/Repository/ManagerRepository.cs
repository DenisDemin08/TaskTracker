using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TaskTracker.Storage.Repository
{
    /// <summary>
    /// Реализация репозитория для работы с менеджерами
    /// </summary>
    public class ManagerRepository(TaskTrackerdbContext context)
        : Repository<Managers>(context), IManagerRepository
    {
        /// <summary>
        /// Получить менеджера по идентификатору пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Найденный менеджер или null</returns>
        public async Task<Managers?> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.User.UserId == userId);
        }
    }
}