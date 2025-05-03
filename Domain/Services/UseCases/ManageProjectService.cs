using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.ValueObject.Project;
using TaskTracker.Storage;
using System;
using System.Threading.Tasks;

namespace TaskTracker.Domain.Services.UseCases
{
    public class ManageProjectService : IManageProjectService
    {
        private readonly TaskTrackerRepository _repository;

        public ManageProjectService(TaskTrackerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Project> CreateProjectAsync(ProjectCreateDto dto)
        {
            var team = await _repository.GetTeamByIdAsync(dto.TeamId)
                ?? throw new ArgumentException("Team not found");

            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                AdministratorId = team.ManagerId // Предполагаем, что менеджер команды является администратором проекта
            };

            await _repository.AddProjectAsync(project);
            await _repository.SaveChangesAsync();

            team.ProjectId = project.ProjectId;
            await _repository.UpdateTeamAsync(team);
            await _repository.SaveChangesAsync();

            return project;
        }

        public async Task AssignTeamToProjectAsync(int projectId, int teamId)
        {
            var project = await _repository.GetProjectByIdAsync(projectId)
                ?? throw new ArgumentException("Project not found");

            var team = await _repository.GetTeamByIdAsync(teamId)
                ?? throw new ArgumentException("Team not found");

            team.ProjectId = project.ProjectId;
            await _repository.UpdateTeamAsync(team);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateProjectStatusAsync(int projectId, Project status)
        {
            var project = await _repository.GetProjectByIdAsync(projectId)
                ?? throw new ArgumentException("Project not found");

            project.Name = status.Name;
            project.Description = status.Description;
            project.EndDate = status.EndDate;

            await _repository.UpdateProjectAsync(project);
            await _repository.SaveChangesAsync();
        }
    }
}