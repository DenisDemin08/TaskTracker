using TaskTracker.Domain.Entities;
using TaskTracker.Domain.ValueObject.Project;

namespace TaskTracker.Domain.Services.Contracts
{
    public interface IManageProjectService
    {
       Task<Project> CreateProjectAsync(ProjectCreateDto dto);
        Task AssignTeamToProjectAsync(int projectId, int teamId);
        Task UpdateProjectStatusAsync(int projectId, Project status);
    }
}
