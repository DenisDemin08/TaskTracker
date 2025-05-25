using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts.Repositories;

namespace TaskTracker.Storage.Repository
{
    /// <summary>
    /// Реализация репозитория для работы с командами
    /// </summary>
    public class TeamRepository(TaskTrackerdbContext context)
        : Repository<Teams>(context), ITeamRepository
    {
        /// <inheritdoc/>
        public async Task<List<Teams>> GetTeamsByProjectAsync(int projectId)
            => await _dbSet
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<List<Teams>> GetTeamsByManagerAsync(int managerId)
            => await _dbSet
                .Where(t => t.ManagerId == managerId)
                .ToListAsync();
    }
}