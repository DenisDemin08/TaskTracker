using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTracker.Domain.Services.UseCases
{
    public class AccessControlService : IAccessControlService
    {
        private readonly TaskTrackerRepository _repository;

        public AccessControlService(TaskTrackerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ValidateProjectAccessAsync(int userId, int projectId)
        {
            var project = await _repository.GetProjectByIdAsync(projectId);
            if (project == null) return false;

            if (project.AdministratorId == userId)
                return true;

            var projectTeams = await _repository.GetProjectTeamsAsync(project);
            return projectTeams.Any(t => t.ManagerId == userId);
        }

        public async Task<bool> ValidateTaskOwnershipAsync(int userId, int taskId)
        {
            var task = await _repository.GetTaskByIdAsync(taskId);
            if (task == null) return false;

            return task.CreatorId == userId
                || (task.AssigneeId.HasValue && task.AssigneeId.Value == userId);
        }

        public async Task<bool> IsTeamManagerAsync(int userId, int teamId)
        { 
            var manager = new Manager { UserId = userId };
            var managedTeams = await _repository.GetManagerTeamsAsync(manager);

            return managedTeams.Any(t => t.TeamId == teamId);
        }
    }
}