using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Services.Contracts.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Storage.Repository
{
    /// <summary>
    /// Реализация репозитория для работы с проектами
    /// </summary>
    public class ProjectRepository(TaskTrackerdbContext context)
        : Repository<Projects>(context), IProjectRepository
    {
        /// <summary>
        /// Получить проекты по администратору
        /// </summary>
        public async Task<List<Projects>> GetProjectsByAdminAsync(int adminId)
            => await _dbSet
                .Where(p => p.AdministratorId == adminId)
                .ToListAsync();

        /// <summary>
        /// Получить активные проекты
        /// </summary>
        public async Task<List<Projects>> GetActiveProjectsAsync()
            => await _dbSet
                .Where(p => p.EndDate == null || p.EndDate > DateOnly.FromDateTime(DateTime.Now))
                .ToListAsync();
    }
}