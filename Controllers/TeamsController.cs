using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController(
        ITeamManageService teamManageService,
        IAccessControlService accessControlService,
        IUnitOfWork unitOfWork) : ControllerBase
    {
        /// <summary>
        /// Создать новую команду
        /// </summary>
        [HttpPost("{managerId}")]
        public async Task<IActionResult> CreateTeam(
            int managerId,
            [FromBody] TeamCreateDto teamDto)
        {
            var (manager, error) = await ValidateManager(managerId);
            if (error != null) return error;

            try
            {
                var team = new Teams
                {
                    ManagerId = managerId,
                    Name = teamDto.Name,
                    ProjectId = teamDto.ProjectId
                };

                await unitOfWork.Teams.AddAsync(team);
                await unitOfWork.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetTeamDetails),
                    new { id = team.TeamId },
                    new TeamResponseDto(team.TeamId, team.Name, team.ProjectId)
                );
            }
            catch (DbUpdateException ex)
            {
                return HandleDatabaseError(ex);
            }
        }

        /// <summary>
        /// Получить информацию о команде
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamDetails(int id)
        {
            var team = await unitOfWork.Teams.GetByIdAsync(id);
            return team == null
                ? NotFound(new { error = "Команда не найдена" })
                : Ok(new TeamResponseDto(team.TeamId, team.Name, team.ProjectId));
        }

        /// <summary>
        /// Обновить информацию о команде
        /// </summary>
        [HttpPut("{teamId}/manager/{managerId}")]
        public async Task<IActionResult> UpdateTeam(
            int teamId,
            int managerId,
            [FromBody] TeamUpdateDto dto)
        {
            var (_, error) = await ValidateManager(managerId);
            if (error != null) return error;

            var team = await unitOfWork.Teams.GetByIdAsync(teamId);
            if (team == null)
                return NotFound(new { error = "Команда не найдена" });

            if (!await accessControlService.IsTeamManagerAsync(managerId, teamId))
                return ForbidWithMessage("У вас нет прав на управление этой командой");

            try
            {
                team.Name = dto.Name;
                team.ProjectId = dto.ProjectId;
                await unitOfWork.Teams.UpdateAsync(team);
                await unitOfWork.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return HandleDatabaseError(ex);
            }
        }

        /// <summary>
        /// Добавить участника в команду
        /// </summary>
        [HttpPost("{teamId}/members/{memberId}/manager/{managerId}")]
        public async Task<IActionResult> AddMember(
            int teamId,
            int memberId,
            int managerId)
        {
            var (_, error) = await ValidateManager(managerId);
            if (error != null) return error;

            if (!await accessControlService.IsTeamManagerAsync(managerId, teamId))
                return ForbidWithMessage("У вас нет прав на управление этой командой");

            try
            {
                await teamManageService.AddMemberAsync(teamId, memberId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                return HandleDatabaseError(ex);
            }
        }

        /// <summary>
        /// Удалить участника из команды
        /// </summary>
        [HttpDelete("{teamId}/members/{memberId}/manager/{managerId}")]
        public async Task<IActionResult> RemoveMember(
            int teamId,
            int memberId,
            int managerId)
        {
            var (_, error) = await ValidateManager(managerId);
            if (error != null) return error;

            if (!await accessControlService.IsTeamManagerAsync(managerId, teamId))
                return ForbidWithMessage("У вас нет прав на управление этой командой");

            try
            {
                await teamManageService.RemoveMemberFromTeamAsync(teamId, memberId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                return HandleDatabaseError(ex);
            }
        }

        /// <summary>
        /// Удалить команду
        /// </summary>
        [HttpDelete("{teamId}/manager/{managerId}")]
        public async Task<IActionResult> DeleteTeam(int teamId, int managerId)
        {
            var (_, error) = await ValidateManager(managerId);
            if (error != null) return error;

            if (!await accessControlService.IsTeamManagerAsync(managerId, teamId))
                return ForbidWithMessage("У вас нет прав на управление этой командой");

            try
            {
                await teamManageService.DeleteTeamAsync(teamId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                return HandleDatabaseError(ex);
            }
        }

        private async Task<(Managers? manager, IActionResult? error)> ValidateManager(int managerId)
        {
            var manager = await unitOfWork.Manager.GetByIdAsync(managerId);
            if (manager == null)
                return (null, NotFound(new { error = "Менеджер не найден" }));

            return (manager, null);
        }

        private static ObjectResult ForbidWithMessage(string message)
        {
            return new ObjectResult(new { error = "Доступ запрещен", message })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        private static ObjectResult HandleDatabaseError(DbUpdateException ex)
        {
            return new ObjectResult(new
            {
                error = "Ошибка базы данных",
                message = ex.InnerException?.Message ?? ex.Message
            });
        }
    }

    public record TeamCreateDto(string Name, int? ProjectId);
    public record TeamUpdateDto(string Name, int? ProjectId);
    public record TeamResponseDto(int TeamId, string Name, int? ProjectId);
}