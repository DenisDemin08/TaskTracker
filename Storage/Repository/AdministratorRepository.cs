using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TaskTracker.Storage.Repository
{
    /// <summary>
    /// Реализация репозитория для работы с администраторами
    /// </summary>
    public class AdministratorRepository(TaskTrackerdbContext context)
        : Repository<Administrators>(context), IAdministratorRepository
    {
        /// <summary>
        /// Получить администратора по идентификатору пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Найденный администратор или null</returns>
        public async Task<Administrators?> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.User.UserId == userId);
        }
    }
}