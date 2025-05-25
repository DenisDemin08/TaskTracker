using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Services.Contracts.Repositories;

namespace TaskTracker.Storage.Repository
{
    /// <summary>
    /// Базовый класс репозитория для операций CRUD
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public class Repository<TEntity>(TaskTrackerdbContext context)
        : IRepository<TEntity> where TEntity : class
    {
        protected readonly TaskTrackerdbContext _context = context;
        protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        /// <inheritdoc/>
        public async Task<TEntity?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _dbSet.ToListAsync();

        /// <inheritdoc/>
        public async Task AddAsync(TEntity entity)
            => await _dbSet.AddAsync(entity);

        /// <inheritdoc/>
        public Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }
    }
}