using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;

namespace TaskTracker.Domain.Services.UseCases
{
    /// <summary>
    /// Сервис завершения задач
    /// </summary>
    public class CompletingTaskService(
        IUnitOfWork unitOfWork,
        IAccessControlService accessControl) : ICompletingTaskService
    {
        /// <inheritdoc/>
        public async Task ConfirmTaskCompletionAsync(int taskId, Administrators confirmator)
        {
            var task = await unitOfWork.Tasks.GetByIdAsync(taskId);
                if (task != null)
            {
                if (!await accessControl.ValidateProjectAccessAsync(confirmator.AdminId, task.ProjectId))
                    throw new UnauthorizedAccessException("Доступ запрещен");

                task.TasksStatus = TasksStatus.Completed;
                await unitOfWork.Tasks.UpdateAsync(task);
                await unitOfWork.SaveChangesAsync();
            }
            else
                throw new ArgumentException("Задача не найдена");
        }

        /// <inheritdoc/>
        public async Task UpdateTaskStatusAsync(int taskId, TasksStatus status)
        {
            var task = await unitOfWork.Tasks.GetByIdAsync(taskId);
                if (task != null)
            {
                task.TasksStatus = status;
                await unitOfWork.Tasks.UpdateAsync(task);
                await unitOfWork.SaveChangesAsync();
            }
            else
                throw new ArgumentException("Задача не найдена");
        }
    }
}