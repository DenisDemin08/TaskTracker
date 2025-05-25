using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Services.Contracts.Repositories;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Storage.Repository
{
    /// <summary>
    /// Реализация репозитория для работы с задачами
    /// </summary>
    public class TasksRepository(TaskTrackerdbContext context)
        : Repository<Tasks>(context), ITasksRepository
    {
        /// <inheritdoc/>
        public async Task<List<Tasks>> GetTasksByAssigneeAsync(int assigneeId)
            => await _dbSet
                .Where(t => t.AssigneeId == assigneeId)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<List<Tasks>> GetHighPriorityTasksAsync()
            => await _dbSet
                .Where(t => t.TasksPriority == TasksPriority.High)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<List<Tasks>> GetTasksByProjectAsync(int projectId)
            => await _dbSet
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
    }
}