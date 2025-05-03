using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts
{
    public interface ICompletingTaskService
    {
        Task ConfirmTaskCompletionAsync(int taskId, Administrator confirmator, string? comment);
        Task UpdateTaskStatusAsync(int taskId, TaskStatus status);
    }
}
