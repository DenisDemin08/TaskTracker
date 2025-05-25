using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts
{
    /// <summary>
    /// Сервис создания и управления задачами
    /// </summary>
    public interface ICreateTaskService
    {
        /// <summary>
        /// Создание новой задачи
        /// </summary>
        /// <param name="task">Данные задачи</param>
        /// <param name="initiator">Администратор-инициатор</param>
        /// <returns>Созданная задача</returns>
        Task<Tasks> CreateTaskAsync(Tasks task, Administrators initiator);

        /// <summary>
        /// Назначение ответственного за задачу
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <param name="admin">Ответственный администратор</param>
        Task AssignResponsibleAsync(int taskId, Administrators admin);
    }
}