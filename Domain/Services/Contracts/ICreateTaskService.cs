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
        /// Назначить ответственного сотрудника
        /// </summary>
        /// <param name="admin"> администратора</param>
        /// <param name="taskId">ID задачи</param>
        /// <param name="employeeId">ID назначаемого сотрудника</param>
        Task AssignResponsibleAsync(int taskId, Administrators admin, int employeeId);
    }
}