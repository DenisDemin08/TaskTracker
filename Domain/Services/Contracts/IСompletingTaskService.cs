using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Services.Contracts
{
    /// <summary>
    /// Сервис завершения задач
    /// </summary>
    public interface ICompletingTaskService
    {
        /// <summary>
        /// Подтверждение завершения задачи
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <param name="confirmator">Подтверждающий администратор</param>
        Task ConfirmTaskCompletionAsync(int taskId, Administrators confirmator);

        /// <summary>
        /// Обновление статуса задачи
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <param name="status">Новый статус задачи</param>
        Task UpdateTaskStatusAsync(int taskId, Enums.TaskStatus status);
    }
}