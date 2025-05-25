using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts
{
    /// <summary>
    /// Сервис управления проектами
    /// </summary>
    public interface IProjectManageService
    {
        /// <summary>
        /// Создание нового проекта
        /// </summary>
        /// <param name="project">Данные проекта</param>
        /// <returns>Созданный проект</returns>
        Task<Projects> CreateProjectAsync(Projects project);

        /// <summary>
        /// Назначение команды на проект
        /// </summary>
        /// <param name="teamId">Идентификатор команды</param>
        /// <param name="projectId">Идентификатор проекта</param>
        Task AssignTeamAsync(int teamId, int projectId);

        /// <summary>
        /// Обновление статуса проекта
        /// </summary>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <param name="status">Новый статус проекта</param>
        Task UpdateProjectStatusAsync(int projectId, Projects status);

        /// <summary>
        /// Добавление задач в проект
        /// </summary>
        /// <param name="taskIds">Список идентификаторов задач</param>
        /// <param name="projectId">Идентификатор проекта</param>
        Task AddTasksAsync(IEnumerable<int> taskIds, int projectId);
    }
}