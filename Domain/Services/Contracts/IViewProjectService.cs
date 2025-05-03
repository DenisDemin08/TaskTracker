using TaskTracker.Domain.Entities;
using TaskTracker.Domain.ValueObject.Project;

namespace TaskTracker.Domain.Services.Contracts
{
    public interface IViewProjectsService
    {
        Task<ProjectDetailsDto> GetProjectDetailsAsync(int projectId);
        Task<List<Project>> GetAllProjectsAsync();
    }
}
