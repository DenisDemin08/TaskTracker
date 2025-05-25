using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts.Repositories
{
    /// <summary>
    /// Репозиторий для управления проектами
    /// </summary>
    public interface IProjectRepository : IRepository<Projects>
    {
        /// <summary>
        /// Получить проекты по администратору
        /// </summary>
        /// <param name="adminId">Идентификатор администратора</param>
        /// <returns>Список проектов</returns>
        Task<List<Projects>> GetProjectsByAdminAsync(int adminId);

        /// <summary>
        /// Получить активные проекты (с датой окончания в будущем или без даты окончания)
        /// </summary>
        /// <returns>Список активных проектов</returns>
        Task<List<Projects>> GetActiveProjectsAsync();
    }
}