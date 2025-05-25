using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;

namespace TaskTracker.Domain.Services.UseCases
{
    /// <summary>
    /// Сервис управления проектами
    /// </summary>
    public class ProjectManageService(IUnitOfWork unitOfWork) : IProjectManageService
    {
        /// <summary>
        /// Создает новый проект
        /// </summary>
        public async Task<Projects> CreateProjectAsync(Projects project)
        {
            await unitOfWork.Projects.AddAsync(project);
            await unitOfWork.SaveChangesAsync();
            return project;
        }

        /// <summary>
        /// Назначает команду на проект
        /// </summary>
        public async Task AssignTeamAsync(int teamId, int projectId)
        {
            var team = await unitOfWork.Teams.GetByIdAsync(teamId);
            if (team != null)
            {
                team.ProjectId = projectId;
                await unitOfWork.Teams.UpdateAsync(team);
                await unitOfWork.SaveChangesAsync();
            }
            else
                throw new ArgumentException("Команда не найдена");
        }

        /// <summary>
        /// Обновляет статус проекта
        /// </summary>
        public async Task UpdateProjectStatusAsync(int projectId, Projects status)
        {
            var project = await unitOfWork.Projects.GetByIdAsync(projectId);
            if (project != null)
            {
                project.EndDate = status.EndDate;
                await unitOfWork.Projects.UpdateAsync(project);
                await unitOfWork.SaveChangesAsync();
            }
            else
                throw new ArgumentException("Проект не найден");
        }

        /// <summary>
        /// Добавляет задачи в проект
        /// </summary>
        public async Task AddTasksAsync(IEnumerable<int> taskIds, int projectId)
        {
            foreach (var taskId in taskIds)
            {
                var task = await unitOfWork.Tasks.GetByIdAsync(taskId);
                if (task != null) task.ProjectId = projectId;
            }
            await unitOfWork.SaveChangesAsync();
        }
    }
}