using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts.Repositories;

namespace TaskTracker.Storage.Repository
{
    /// <summary>
    /// Реализация репозитория для работы с пользователями
    /// </summary>
    public class UserRepository(TaskTrackerdbContext context)
        : Repository<Users>(context), IUserRepository
    {
        /// <inheritdoc/>
        public async Task<Users?> GetByEmailAsync(string email)
            => await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

        /// <inheritdoc/>
        public async Task<Users?> GetByFullNameAsync(string fullName)
            => await _dbSet.FirstOrDefaultAsync(u => u.FullName == fullName);

        /// <inheritdoc/>
        public async Task<List<Administrators>> GetAdministratorsAsync()
            => await _context.Administrators
                .Include(a => a.User)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<List<Managers>> GetManagersAsync()
            => await _context.Managers
                .Include(m => m.User)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<List<Employees>> GetTeamMembersByTeamAsync(int teamId)
            => await _context.Employees
                .Include(e => e.User)
                .Where(e => e.TeamId == teamId)
                .ToListAsync();
    }
}