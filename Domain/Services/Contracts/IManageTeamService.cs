using TaskTracker.Domain.Entities;
using TaskTracker.Domain.ValueObject.Team;

namespace TaskTracker.Domain.Services.Contracts
{
    public interface IManageTeamService
    {
        Task<Team> CreateTeamAsync(TeamCreateDto dto);
        Task AddMemberToTeamAsync(int teamId, int userId);
        Task RemoveMemberFromTeamAsync(int teamId, int userId);
        Task DeleteTeamAsync(int teamId);
    }
}
