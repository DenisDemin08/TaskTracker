using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts.Repositories
{
    /// <summary>
    /// Репозиторий для управления командами
    /// </summary>
    public interface ITeamRepository : IRepository<Teams>
    {
        /// <summary>
        /// Получить команды по проекту
        /// </summary>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <returns>Список команд</returns>
        Task<List<Teams>> GetTeamsByProjectAsync(int projectId);

        /// <summary>
        /// Получить команды по менеджеру
        /// </summary>
        /// <param name="managerId">Идентификатор менеджера</param>
        /// <returns>Список команд</returns>
        Task<List<Teams>> GetTeamsByManagerAsync(int managerId);
    }
}