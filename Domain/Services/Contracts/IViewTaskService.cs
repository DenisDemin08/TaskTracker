using TaskTracker.Domain.ValueObject.Project;
namespace TaskTracker.Domain.Services.Contracts;

public interface IViewTaskService
{
   Task<ProjectDetailsDto> GetTaskDetailsAsync(int taskId);
    Task<List<Task>> GetTeamMemberTasksAsync(int userId);
}


