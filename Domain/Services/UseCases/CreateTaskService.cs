using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;

namespace TaskTracker.Domain.Services.UseCases
{
    /// <summary>
    /// Сервис создания задач
    /// </summary>
    public class CreateTaskService(
        IUnitOfWork unitOfWork,
        IAccessControlService accessControl) : ICreateTaskService
    {
        /// <inheritdoc/>
        public async Task<Tasks> CreateTaskAsync(Tasks task, Administrators initiator)
        {
            if (!await accessControl.ValidateProjectAccessAsync(initiator.AdminId, task.ProjectId))
                throw new UnauthorizedAccessException("Нет доступа к проекту");

            task.CreatorId = initiator.AdminId;
            await unitOfWork.Tasks.AddAsync(task);
            await unitOfWork.SaveChangesAsync();
            return task;
        }

        /// <inheritdoc/>
        public async Task AssignResponsibleAsync(int taskId, Administrators admin)
        {
            var task = await unitOfWork.Tasks.GetByIdAsync(taskId);
                if (task != null)
            {
                if (!await accessControl.ValidateProjectAccessAsync(admin.AdminId, task.ProjectId))
                    throw new UnauthorizedAccessException("Нет доступа к проекту");

                task.AssigneeId = admin.AdminId;
                await unitOfWork.Tasks.UpdateAsync(task);
                await unitOfWork.SaveChangesAsync();
            }
            else
                throw new ArgumentException("Задача не найдена");
        }
    }
}