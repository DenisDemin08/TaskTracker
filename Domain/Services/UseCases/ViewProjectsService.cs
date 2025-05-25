using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts.Repositories;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.ValueObject.Project;

namespace TaskTracker.Domain.Services.UseCases
{
    /// <summary>
    /// Сервис просмотра информации о проектах
    /// </summary>
    public class ViewProjectsService(IUnitOfWork unitOfWork) : IViewProjectsService
    {
        /// <inheritdoc/>
        public async Task<ProjectDetailsDto?> GetProjectDetailsAsync(int projectId)
        {
            var project = await unitOfWork.Projects.GetByIdAsync(projectId);
            if (project == null) return null;

            var teams = await unitOfWork.Teams.GetTeamsByProjectAsync(projectId);
            var tasks = await unitOfWork.Tasks.GetTasksByProjectAsync(projectId);

            return new ProjectDetailsDto
            {
                Name = project.Name,
                Description = project.Description ?? string.Empty,
                StartDate = project.StartDate.ToDateTime(TimeOnly.MinValue),
                CompletionDate = project.EndDate?.ToDateTime(TimeOnly.MinValue),
                Tasks = tasks,
                Teams = teams
            };
        }

        /// <inheritdoc/>
        public async Task<List<Projects>> GetAllProjectsAsync()
        {
            return [.. (await unitOfWork.Projects.GetAllAsync()).OrderByDescending(p => p.StartDate)];
        }
    }
}