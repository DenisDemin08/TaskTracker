using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts.Repositories;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.ValueObject.Project;

namespace TaskTracker.Domain.Services.UseCases
{
    /// <summary>
    /// Сервис просмотра информации о задачах
    /// </summary>
    public class ViewTaskService(IUnitOfWork unitOfWork) : IViewTaskService
    {
        /// <inheritdoc/>
        public async Task<ProjectDetailsDto?> GetTaskDetailsAsync(int taskId)
        {
            var task = await unitOfWork.Tasks.GetByIdAsync(taskId);
            if (task == null) return null;

            return await new ViewProjectsService(unitOfWork)
                .GetProjectDetailsAsync(task.ProjectId);
        }

        /// <inheritdoc/>
        public async Task<List<Tasks>> GetTeamMemberTasksAsync(int userId)
        {
            return await unitOfWork.Tasks.GetTasksByAssigneeAsync(userId);
        }
    }
}