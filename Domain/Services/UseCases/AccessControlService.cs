using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;

namespace TaskTracker.Domain.Services.UseCases
{
    /// <summary>
    /// Сервис контроля доступа к ресурсам системы
    /// </summary>
    public class AccessControlService(IUnitOfWork unitOfWork) : IAccessControlService
    {
        /// <inheritdoc/>
        public async Task<bool> ValidateProjectAccessAsync(int userId, int projectId)
        {
            var project = await unitOfWork.Projects.GetByIdAsync(projectId);
            if (project == null) return false;

            return project.AdministratorId == userId ||
                   await IsUserInProjectTeamAsync(userId, projectId);
        }

        /// <inheritdoc/>
        public async Task<bool> ValidateTaskOwnershipAsync(int userId, int taskId)
        {
            var task = await unitOfWork.Tasks.GetByIdAsync(taskId);
            return task != null && (task.CreatorId == userId || task.AssigneeId == userId);
        }

        /// <inheritdoc/>
        public async Task<bool> IsTeamManagerAsync(int userId, int teamId)
        {
            var team = await unitOfWork.Teams.GetByIdAsync(teamId);
            return team != null && team.ManagerId == userId;
        }

        private async Task<bool> IsUserInProjectTeamAsync(int userId, int projectId)
        {
            var teams = await unitOfWork.Teams.GetTeamsByProjectAsync(projectId);
            foreach (var team in teams)
            {
                var employees = await unitOfWork.Employees.GetByTeamIdAsync(team.TeamId);
                if (employees.Any(e => e.EmployeeId == userId)) return true;
            }
            return false;
        }
    }
}