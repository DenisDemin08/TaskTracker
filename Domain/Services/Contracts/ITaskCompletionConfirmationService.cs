using TaskTracker.Domain.Entities;
using TaskTracker.Domain.ValueObject;

namespace TaskTracker.Domain.Services.Contracts
{
    /// <summary>
    /// Сервис подтверждения выполнения задач
    /// </summary>
    public interface ITaskCompletionConfirmationService
    {
        /// <summary>
        /// Запрос подтверждения выполнения задачи
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <param name="requester">Сотрудник-инициатор запроса</param>
        Task RequestConfirmationAsync(int taskId, Employees requester);

        /// <summary>
        /// Подтверждение выполнения задачи
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <param name="confirmer">Подтверждающий менеджер</param>
        /// <param name="comment">Комментарий подтверждения</param>
        Task ConfirmTaskCompletionAsync(int taskId, Managers confirmer, string? comment);

        /// <summary>
        /// Отклонение выполнения задачи
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <param name="reviewer">Менеджер-рецензент</param>
        /// <param name="reason">Причина отклонения</param>
        Task RejectTaskCompletionAsync(int taskId, Managers reviewer, string reason);

        /// <summary>
        /// Получение списка ожидающих подтверждения задач
        /// </summary>
        /// <param name="managerId">Идентификатор менеджера</param>
        /// <returns>Список задач на подтверждение</returns>
        Task<List<TaskConfirmationDto>> GetPendingConfirmationsAsync(int managerId);
    }
}