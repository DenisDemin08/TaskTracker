using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TaskTracker.Storage.Repository
{
    /// <summary>
    /// Реализация репозитория для работы с сотрудниками
    /// </summary>
    public class EmployeeRepository(TaskTrackerdbContext context)
        : Repository<Employees>(context), IEmployeeRepository
    {
        /// <summary>
        /// Получить сотрудников по команде
        /// </summary>
        public async Task<List<Employees>> GetByTeamIdAsync(int teamId)
            => await _dbSet
                .Where(e => e.TeamId == teamId)
                .ToListAsync();
    }
}