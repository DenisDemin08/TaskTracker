using TaskTracker.Domain.Entities;
using TaskTracker.Domain.ValueObject.Project;

namespace TaskTracker.Domain.Services.Contracts
{
    /// <summary>
    /// Сервис просмотра информации о задачах
    /// </summary>
    public interface IViewTaskService
    {
        /// <summary>
        /// Получение детальной информации о задаче
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <returns>Детали задачи или null</returns>
        Task<ProjectDetailsDto?> GetTaskDetailsAsync(int taskId);

        /// <summary>
        /// Получение задач участника команды
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Список задач</returns>
        Task<List<Tasks>> GetTeamMemberTasksAsync(int userId);
    }
}