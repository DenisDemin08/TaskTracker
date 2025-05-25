// TaskCompletionConfirmationService.cs
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;
using TaskTracker.Domain.ValueObject;

namespace TaskTracker.Domain.Services.UseCases
{
    /// <summary>
    /// Сервис подтверждения выполнения задач
    /// </summary>
    public class TaskCompletionConfirmationService(
        IUnitOfWork unitOfWork,
        IAccessControlService accessControl) : ITaskCompletionConfirmationService
    {
        /// <inheritdoc/>
        public async Task RequestConfirmationAsync(int taskId, Employees requester)
        {
            var task = await unitOfWork.Tasks.GetByIdAsync(taskId)
                ?? throw new KeyNotFoundException("Задача не найдена");

            if (!await accessControl.ValidateTaskOwnershipAsync(requester.User.UserId, taskId))
                throw new UnauthorizedAccessException("Нет доступа к задаче");

            task.TasksStatus = TasksStatus.PendingConfirmation;
            await unitOfWork.Tasks.UpdateAsync(task);
            await unitOfWork.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task ConfirmTaskCompletionAsync(int taskId, Managers confirmer, string? comment)
        {
            var task = await unitOfWork.Tasks.GetByIdAsync(taskId)
                ?? throw new KeyNotFoundException("Задача не найдена");

            var project = await unitOfWork.Projects.GetByIdAsync(task.ProjectId)
                ?? throw new KeyNotFoundException("Проект не найден");

            var teams = await unitOfWork.Teams.GetTeamsByProjectAsync(project.ProjectId);
            var isManager = teams?.Any(t => t.ManagerId == confirmer.ManagerId) ?? false;

            if (!isManager)
                throw new UnauthorizedAccessException("Требуются права менеджера команды");

            task.TasksStatus = TasksStatus.Completed;
            await unitOfWork.Tasks.UpdateAsync(task);
            await unitOfWork.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task RejectTaskCompletionAsync(int taskId, Managers reviewer, string reason)
        {
            var task = await unitOfWork.Tasks.GetByIdAsync(taskId)
                ?? throw new KeyNotFoundException("Задача не найдена");

            var project = await unitOfWork.Projects.GetByIdAsync(task.ProjectId)
                ?? throw new KeyNotFoundException("Проект не найден");

            var teams = await unitOfWork.Teams.GetTeamsByProjectAsync(project.ProjectId);
            var isManager = teams?.Any(t => t.ManagerId == reviewer.ManagerId) ?? false;

            if (!isManager)
                throw new UnauthorizedAccessException("Требуются права менеджера команды");

            task.TasksStatus = TasksStatus.NeedsRevision;
            await unitOfWork.Tasks.UpdateAsync(task);
            await unitOfWork.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<List<TaskConfirmationDto>> GetPendingConfirmationsAsync(int managerId)
        {
            var teams = await unitOfWork.Teams.GetTeamsByManagerAsync(managerId);
            var teamIds = teams?.Select(t => t.TeamId).ToList() ?? [];

            var allTasks = await unitOfWork.Tasks.GetAllAsync();
            var pendingTasks = allTasks?
                .Where(t => t.TasksStatus == TasksStatus.PendingConfirmation)
                .ToList() ?? [];

            var result = new List<TaskConfirmationDto>();

            foreach (var task in pendingTasks)
            {

                var project = await unitOfWork.Projects.GetByIdAsync(task.ProjectId);
                if (project == null) continue;

                var projectTeams = await unitOfWork.Teams.GetTeamsByProjectAsync(project.ProjectId);
                if (projectTeams == null || !projectTeams.Any(t => teamIds.Contains(t.TeamId))) continue;

                var creator = await unitOfWork.Users.GetByIdAsync(task.CreatorId);
                var confirmer = await unitOfWork.Users.GetByIdAsync(managerId);

                result.Add(new TaskConfirmationDto
                {
                    Status = task.TasksStatus.ToString(),
                    RequesterName = creator?.FullName ?? "Неизвестный",
                    ConfirmerName = confirmer?.FullName ?? "Менеджер команды",
                    RequestDate = DateTime.UtcNow,
                    Comment = null,
                    Reason = null
                });
            }

            return result;
        }
    }
}