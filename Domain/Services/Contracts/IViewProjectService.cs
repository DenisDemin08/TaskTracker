using TaskTracker.Domain.Entities;
using TaskTracker.Domain.ValueObject.Project;

namespace TaskTracker.Domain.Services.Contracts
{
    /// <summary>
    /// Сервис просмотра информации о проектах
    /// </summary>
    public interface IViewProjectsService
    {
        /// <summary>
        /// Получение детальной информации о проекте
        /// </summary>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <returns>Детали проекта или null</returns>
        Task<ProjectDetailsDto?> GetProjectDetailsAsync(int projectId);

        /// <summary>
        /// Получение списка всех проектов
        /// </summary>
        /// <returns>Список проектов</returns>
        Task<List<Projects>> GetAllProjectsAsync();
    }
}