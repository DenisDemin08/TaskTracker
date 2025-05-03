namespace TaskTracker.Domain.Services.Contracts
{
    public interface INotificationService
    {
        Task NotifyTeamMemberAsync(int userId, string message);
    }
}
