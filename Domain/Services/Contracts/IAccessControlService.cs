namespace TaskTracker.Domain.Services.Contracts
{
    public interface IAccessControlService
    {
        Task<bool> ValidateProjectAccessAsync(int userId, int projectId);
        Task<bool> ValidateTaskOwnershipAsync(int userId, int taskId);
        Task<bool> IsTeamManagerAsync(int userId, int teamId);
    }
}
