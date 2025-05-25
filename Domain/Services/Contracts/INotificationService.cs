using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts
{
    /// <summary>
    /// Сервис уведомлений
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Уведомление исполнителя о назначении задачи
        /// </summary>
        /// <param name="task">Назначенная задача</param>
        /// <param name="assignee">Исполнитель задачи</param>
        Task NotifyAssigneeAsync(Tasks task, Users assignee);

        /// <summary>
        /// Отправка уведомления об изменении задачи
        /// </summary>
        /// <param name="task">Измененная задача</param>
        /// <param name="updateType">Тип изменения</param>
        Task SendTaskUpdateNotificationAsync(Tasks task, string updateType);

        /// <summary>
        /// Отправка подтверждающего уведомления менеджеру
        /// </summary>
        /// <param name="task">Задача для подтверждения</param>
        /// <param name="responsible">Ответственный менеджер</param>
        Task SendConfirmationAlertAsync(Tasks task, Managers responsible);
    }
}