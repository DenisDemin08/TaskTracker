using TaskTracker.Domain.Entities;
using TaskTracker.Domain.ValueObject.Task;

namespace TaskTracker.Domain.Services.Contracts
{
    public interface ICreateTaskService
	{
        Task<Tasks> CreateTaskAsync(TaskCreateDto dto, Administrator initiator);
        Task AssignResponsibleAsync(int taskId, Manager manager);
    }

}
