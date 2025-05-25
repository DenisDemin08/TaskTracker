using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts.Repositories
{
    /// <summary>
    /// Репозиторий для управления задачами
    /// </summary>
    public interface ITasksRepository : IRepository<Tasks>
    {
        /// <summary>
        /// Получить задачи по исполнителю
        /// </summary>
        /// <param name="assigneeId">Идентификатор исполнителя</param>
        /// <returns>Список задач</returns>
        Task<List<Tasks>> GetTasksByAssigneeAsync(int assigneeId);

        /// <summary>
        /// Получить задачи с высоким приоритетом
        /// </summary>
        /// <returns>Список высокоприоритетных задач</returns>
        Task<List<Tasks>> GetHighPriorityTasksAsync();

        /// <summary>
        /// Получить задачи по проекту
        /// </summary>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <returns>Список задач проекта</returns>
        Task<List<Tasks>> GetTasksByProjectAsync(int projectId);
    }
}