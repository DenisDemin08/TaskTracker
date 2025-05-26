using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;
using TaskTracker.Storage.Repository;

namespace TaskTracker.Domain.Services.UseCases
{
    /// <summary>
    /// Сервис для создания задач и управления ответственными
    /// </summary>
    /// <remarks>
    /// Инициализирует новый экземпляр сервиса
    /// </remarks>
    /// <param name="unitOfWork">Единица работы для взаимодействия с хранилищем</param>
    public class CreateTaskService(IUnitOfWork unitOfWork) : ICreateTaskService
    {

        /// <summary>
        /// Создает новую задачу в системе
        /// </summary>
        /// <param name="task">Данные задачи для создания</param>
        /// <param name="initiator">Администратор-инициатор создания</param>
        /// <returns>Созданная задача</returns>
        public async Task<Tasks> CreateTaskAsync(Tasks task, Administrators initiator)
        {
            await unitOfWork.Tasks.AddAsync(task);
            await unitOfWork.SaveChangesAsync();
            return task;
        }

        /// <summary>
        /// Назначает ответственного сотрудника для задачи
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <param name="admin">Администратор, выполняющий назначение</param>
        /// <param name="employeeId">Идентификатор назначаемого сотрудника</param>
        /// <exception cref="KeyNotFoundException">
        /// Выбрасывается если задача или сотрудник не найдены
        /// </exception>
        public async Task AssignResponsibleAsync(int taskId, Administrators admin, int employeeId)
        {
            var task = await unitOfWork.Tasks.GetByIdAsync(taskId);
            if (task != null)
            {
                var employee = await unitOfWork.Employees.GetByIdAsync(employeeId);
                if (employee != null)
                {
                    task.AssigneeId = employeeId;
                    await unitOfWork.SaveChangesAsync();
                }
                else
                    throw new KeyNotFoundException("Сотрудник не найден");
            }
            else
                throw new KeyNotFoundException("Задача не найдена");
        }
    }
}