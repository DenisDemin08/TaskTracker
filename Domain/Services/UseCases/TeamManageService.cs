using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;

namespace TaskTracker.Domain.Services.UseCases
{
    /// <summary>
    /// Сервис управления командами
    /// </summary>
    public class TeamManageService(
        IUnitOfWork unitOfWork,
        IAccessControlService accessControl) : ITeamManageService
    {
        /// <inheritdoc/>
        public async Task<Teams> CreateTeamAsync(Employees employee)
        {
            if (employee.User.Role != UserRole.Manager)
                throw new InvalidOperationException("Только менеджеры могут создавать команды");

            var team = new Teams
            {
                ManagerId = employee.EmployeeId,
                Name = $"Команда {DateTime.Now:yyyyMMddHHmm}",
                ProjectId = null
            };

            await unitOfWork.Teams.AddAsync(team);
            await unitOfWork.SaveChangesAsync();
            return team;
        }

        /// <inheritdoc/>
        public async Task AddMemberAsync(int teamId, int userId)
        {
            var team = await unitOfWork.Teams.GetByIdAsync(teamId)
                ?? throw new KeyNotFoundException("Команда не найдена");

            var employee = await unitOfWork.Employees.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("Сотрудник не найден");

            if (!await accessControl.IsTeamManagerAsync(team.ManagerId, teamId))
                throw new UnauthorizedAccessException("Требуются права менеджера команды");

            employee.TeamId = teamId;
            await unitOfWork.Employees.UpdateAsync(employee);
            await unitOfWork.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveMemberFromTeamAsync(int teamId, int userId)
        {
            var employee = await unitOfWork.Employees.GetByIdAsync(userId);
            if (employee?.TeamId == teamId)
            {
                employee.TeamId = null;
                await unitOfWork.Employees.UpdateAsync(employee);
                await unitOfWork.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        public async Task DeleteTeamAsync(int teamId)
        {
            var team = await unitOfWork.Teams.GetByIdAsync(teamId);
            if (team != null)
            {
                await unitOfWork.Teams.DeleteAsync(team);
                await unitOfWork.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        public async Task ReassignManagerAsync(int teamId, int managerId)
        {
            var team = await unitOfWork.Teams.GetByIdAsync(teamId);
                if (team != null)
            {
                var newManager = await unitOfWork.Manager.GetByIdAsync(managerId);
                            if (newManager != null)
                {
                    team.ManagerId = managerId;
                    await unitOfWork.Teams.UpdateAsync(team);
                    await unitOfWork.SaveChangesAsync();
                }
                else
                    throw new InvalidOperationException("Пользователь не является менеджером");
            }
            else
                throw new KeyNotFoundException("Команда не найдена");
        }
    }
}