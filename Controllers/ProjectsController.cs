
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.ValueObject;
using TaskTracker.Domain.Services.Contracts.Repositories;
using TaskTracker.Domain.ValueObject.Project;

namespace TaskTracker.Controllers
{
    /// <summary>
    /// Контроллер для управления проектами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProjectsController(
        IProjectManageService projectManageService,
        IViewProjectsService viewProjectsService,
        IUnitOfWork unitOfWork) : ControllerBase
    {
        /// <summary>
        /// Создать новый проект
        /// </summary>
        [HttpPost("{administratorId}")]
        [SwaggerOperation(OperationId = "CreateProject")]
        [ProducesResponseType(typeof(Projects), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProject(
            int administratorId,
            [FromBody] ProjectCreateDto projectDto)
        {
            try
            {
                var admin = await unitOfWork.Administrator.GetByIdAsync(administratorId);
                if (admin == null)
                    return BadRequest(new ErrorResponse("Администратор не найден"));

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
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Получить список всех проектов
        /// </summary>
        [HttpGet]
        [SwaggerOperation(OperationId = "GetAllProjects")]
        [ProducesResponseType(typeof(IEnumerable<ProjectListItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProjects([FromQuery] DateOnly? startDateFrom = null)
        {
            var projects = await viewProjectsService.GetAllProjectsAsync();

            if (startDateFrom.HasValue)
                projects = [.. projects.Where(p => p.StartDate >= startDateFrom.Value)];

            return Ok(projects.Select(p => new ProjectListItemDto(p)));
        }

        /// <summary>
        /// Получить детальную информацию о проекте
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetProjectDetails")]
        [ProducesResponseType(typeof(ProjectDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectDetails(int id)
        {
            var projectDetails = await viewProjectsService.GetProjectDetailsAsync(id);
            return projectDetails == null
                ? NotFound(new ErrorResponse("Проект не найден"))
                : Ok(projectDetails);
        }

        /// <summary>
        /// Обновить дату завершения проекта
        /// </summary>
        [HttpPatch("{id}/status")]
        [SwaggerOperation(OperationId = "UpdateProjectStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProjectStatus(
            int id,
            [FromBody] ProjectStatusUpdateDto statusDto)
        {
            var project = await unitOfWork.Projects.GetByIdAsync(id);
            if (project == null)
                return NotFound(new ErrorResponse("Проект не найден"));

            project.EndDate = statusDto.EndDate;
            await unitOfWork.Projects.UpdateAsync(project);
            await unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Назначить команду на проект
        /// </summary>
        [HttpPost("{id}/teams")]
        [SwaggerOperation(OperationId = "AssignTeamToProject")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignTeamToProject(
            int id,
            [FromBody] int teamId)
        {
            var team = await unitOfWork.Teams.GetByIdAsync(teamId);
            if (team == null)
                return NotFound(new ErrorResponse("Команда не найдена"));

            team.ProjectId = id;
            await unitOfWork.Teams.UpdateAsync(team);
            await unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Получить задачи проекта
        /// </summary>
        [HttpGet("{id}/tasks")]
        [SwaggerOperation(OperationId = "GetProjectTasks")]
        [ProducesResponseType(typeof(IEnumerable<Task>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectTasks(int id)
        {
            var tasks = await unitOfWork.Tasks.GetTasksByProjectAsync(id);
            return Ok(tasks);
        }

        /// <summary>
        /// Получить команды проекта
        /// </summary>
        [HttpGet("{id}/teams")]
        [SwaggerOperation(OperationId = "GetProjectTeams")]
        [ProducesResponseType(typeof(IEnumerable<Teams>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectTeams(int id)
        {
            var teams = await unitOfWork.Teams.GetTeamsByProjectAsync(id);
            return Ok(teams);
        }
    }

    #region DTO Definitions
    public record ProjectCreateDto(
        [Required] string Name,
        [Required] string Description,
        [Required] DateOnly StartDate,
        [Required] DateOnly EndDate);

    #endregion
}