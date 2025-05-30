﻿using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;
using TaskTracker.Domain.ValueObject;

namespace TaskTracker.Domain.Services.UseCases
{
    /// <summary>
    /// Сервис управления подтверждением выполнения задач
    /// </summary>
    public class TaskCompletionConfirmationService(
        IUnitOfWork unitOfWork,
        IAccessControlService accessControl) : ITaskCompletionConfirmationService
    {
        public async Task RequestConfirmationAsync(int taskId, Employees requester)
        {
            var task = await unitOfWork.Tasks.GetByIdAsync(taskId)
                ?? throw new KeyNotFoundException("Task not found");

            if (!await accessControl.ValidateTaskOwnershipAsync(requester.User.UserId, taskId))
                throw new UnauthorizedAccessException("Access denied");

            task.TaskStatus = Domain.Enums.TaskStatus.PendingConfirmation;
            await unitOfWork.Tasks.UpdateAsync(task);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ConfirmTaskCompletionAsync(int taskId, Managers confirmer, string? comment)
        {
            var task = await unitOfWork.Tasks.GetByIdAsync(taskId)
                ?? throw new KeyNotFoundException("Task not found");

            var project = await unitOfWork.Projects.GetByIdAsync(task.ProjectId)
                ?? throw new KeyNotFoundException("Project not found");

            var teams = await unitOfWork.Teams.GetTeamsByProjectAsync(project.ProjectId);
            if (teams?.Any(t => t.ManagerId == confirmer.ManagerId) != true)
                throw new UnauthorizedAccessException("Manager rights required");

            task.TaskStatus = Domain.Enums.TaskStatus.Completed;
            await unitOfWork.Tasks.UpdateAsync(task);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RejectTaskCompletionAsync(int taskId, Managers reviewer, string reason)
        {
            var task = await unitOfWork.Tasks.GetByIdAsync(taskId)
                ?? throw new KeyNotFoundException("Task not found");

            var project = await unitOfWork.Projects.GetByIdAsync(task.ProjectId)
                ?? throw new KeyNotFoundException("Project not found");

            var teams = await unitOfWork.Teams.GetTeamsByProjectAsync(project.ProjectId);
            if (teams?.Any(t => t.ManagerId == reviewer.ManagerId) != true)
                throw new UnauthorizedAccessException("Manager rights required");

            task.TaskStatus = Domain.Enums.TaskStatus.NeedsRevision;
            await unitOfWork.Tasks.UpdateAsync(task);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<TaskConfirmationDto>> GetPendingConfirmationsAsync(int managerId)
        {
            var teams = await unitOfWork.Teams.GetTeamsByManagerAsync(managerId);
            var teamIds = teams?.Select(t => t.TeamId).ToList() ?? [];

            var allTasks = await unitOfWork.Tasks.GetAllAsync();
            var pendingTasks = allTasks?
                .Where(t => t.TaskStatus == Domain.Enums.TaskStatus.PendingConfirmation)
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
                    Status = task.TaskStatus.ToString(),
                    RequesterName = creator?.FullName ?? "Unknown",
                    ConfirmerName = confirmer?.FullName ?? "Team Manager",
                    RequestDate = DateTime.UtcNow,
                    Comment = null,
                    Reason = null
                });
            }

            return result;
        }
    }
}