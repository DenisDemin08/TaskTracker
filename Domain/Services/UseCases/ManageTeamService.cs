using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.ValueObject.Team;
using TaskTracker.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Services.UseCases
{
    public class ManageTeamService : IManageTeamService
    {
        private readonly TaskTrackerRepository _repository;
        private readonly IAccessControlService _accessControl;

        public ManageTeamService(
            TaskTrackerRepository repository,
            IAccessControlService accessControl)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _accessControl = accessControl ?? throw new ArgumentNullException(nameof(accessControl));
        }

        public async Task<Team> CreateTeamAsync(TeamCreateDto dto)
        {
            var team = new Team
            {
                Name = dto.Name,
                Description = dto.Description,
                ManagerId = 0 // Временное значение, требуется явное назначение менеджера
            };

            await _repository.AddTeamAsync(team);
            await _repository.SaveChangesAsync();
            return team;
        }

        public async Task AddMemberToTeamAsync(int teamId, int userId)
        {
            var team = await _repository.GetTeamByIdAsync(teamId)
                ?? throw new ArgumentException("Team not found");

            var user = await _repository.GetUserByIdAsync(userId)
                ?? throw new ArgumentException("User not found");

            var existingMember = await _repository.GetTeamMemberAsync(teamId, userId);
            if (existingMember != null)
                throw new InvalidOperationException("User already in team");

            var teamMember = new TeamMember
            {
                TeamId = teamId,
                UserId = userId,
                Position = TaskPosition.Programmer
            };

            await _repository.AddTeamMemberAsync(teamMember);
            await _repository.SaveChangesAsync();
        }

        public async Task RemoveMemberFromTeamAsync(int teamId, int userId)
        {
            var member = await _repository.GetTeamMemberAsync(teamId, userId)
                ?? throw new ArgumentException("Member not found in team");

            await _repository.RemoveTeamMemberAsync(member);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteTeamAsync(int teamId)
        {
            var team = await _repository.GetTeamByIdAsync(teamId)
                ?? throw new ArgumentException("Team not found");

            var members = await _repository.GetTeamMembersAsync(new Team { TeamId = teamId });
            foreach (var member in members)
            {
                await _repository.RemoveTeamMemberAsync(member);
            }

            await _repository.RemoveTeamAsync(teamId);
            await _repository.SaveChangesAsync();
        }
    }
}