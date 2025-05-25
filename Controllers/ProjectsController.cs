using Microsoft.AspNetCore.Mvc;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;
using TaskTracker.Domain.ValueObject.Project;

namespace TaskTracker.Controllers
{
    /// <summary>
    /// Контроллер для управления проектами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController(
        IProjectManageService projectManageService,
        IViewProjectsService viewProjectsService,
        IUnitOfWork unitOfWork) : ControllerBase
    {
        /// <summary>
        /// Создать новый проект
        /// </summary>
        [HttpPost("{administratorId}")]
        public async Task<IActionResult> CreateProject(int administratorId, [FromBody] ProjectCreateDto projectDto)
        {
            try
            {
                var admin = await unitOfWork.Administrator.GetByIdAsync(administratorId);
                if (admin == null)
                {
                    return BadRequest(new
                    {
                        error = "Admin Not Found",
                        message = "Указанный администратор не существует"
                    });
                }

                var project = new Projects
                {
                    Name = projectDto.Name,
                    Description = projectDto.Description,
                    StartDate = projectDto.StartDate,
                    EndDate = projectDto.EndDate,
                    AdministratorId = administratorId
                };

                var createdProject = await projectManageService.CreateProjectAsync(project);
                return CreatedAtAction(
                    nameof(GetProjectDetails),
                    new { id = createdProject.ProjectId },
                    createdProject
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    error = "Validation Error",
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Получить список всех проектов
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await viewProjectsService.GetAllProjectsAsync();
            return Ok(projects);
        }

        /// <summary>
        /// Получить детальную информацию о проекте
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectDetails(int id)
        {
            try
            {
                var projectDetails = await viewProjectsService.GetProjectDetailsAsync(id);
                return projectDetails == null
                    ? NotFound(new { error = "Not Found", message = "Проект не найден" })
                    : Ok(projectDetails);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    error = "Invalid Request",
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Обновить статус проекта
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateProjectStatus(int id, [FromBody] ProjectStatusUpdateDto statusDto)
        {
            try
            {
                var project = await unitOfWork.Projects.GetByIdAsync(id);
                if (project == null)
                {
                    return NotFound(new
                    {
                        error = "Project Not Found",
                        message = "Проект не найден"
                    });
                }

                project.EndDate = statusDto.EndDate;
                await unitOfWork.Projects.UpdateAsync(project);
                await unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    error = "Validation Error",
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Назначить команду на проект
        /// </summary>
        [HttpPost("{id}/teams")]
        public async Task<IActionResult> AssignTeamToProject(int id, [FromBody] int teamId)
        {
            try
            {
                var team = await unitOfWork.Teams.GetByIdAsync(teamId);
                if (team == null)
                {
                    return NotFound(new
                    {
                        error = "Team Not Found",
                        message = "Команда не найдена"
                    });
                }

                team.ProjectId = id;
                await unitOfWork.Teams.UpdateAsync(team);
                await unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    error = "Invalid Request",
                    message = ex.Message
                });
            }
        }
    }

    /// <summary>
    /// DTO для создания проекта
    /// </summary>
    public record ProjectCreateDto(
        string Name,
        string Description,
        DateOnly StartDate,
        DateOnly EndDate);

    /// <summary>
    /// DTO для обновления статуса проекта
    /// </summary>
    public record ProjectStatusUpdateDto(DateOnly? EndDate);
}