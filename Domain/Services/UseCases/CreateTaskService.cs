using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.ValueObject.Task;
using TaskTracker.Storage;

namespace TaskTracker.Domain.Services.UseCases
{
    public class CreateTaskService : ICreateTaskService
    {
        private readonly TaskTrackerRepository _taskRepository;
        private readonly TaskTrackerRepository _userRepository;
        private readonly TaskTrackerRepository _projectRepository;

        public CreateTaskService(
            TaskTrackerRepository taskRepository,
            TaskTrackerRepository userRepository,
            TaskTrackerRepository projectRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<Tasks> CreateTaskAsync(TaskCreateDto dto, Administrator initiator)
        {
            if (dto.Deadline == null)
                throw new ArgumentException("Deadline is required.");

            var assignee = await _userRepository.GetUserByIdAsync(dto.AssigneeId)
                ?? throw new ArgumentException("Assignee not found.");

            var project = await _projectRepository.GetProjectByIdAsync(dto.ProjectId)
                ?? throw new ArgumentException("Project not found.");

            var task = new Tasks
            {
                Title = dto.Title,
                TaskDescription = dto.Description,
                TaskStatus = Taskstatus.ToDo,
                TaskPriority = dto.Priority,
                Deadline = DateOnly.FromDateTime(dto.Deadline.Value),
                CreatorId = initiator.UserId,
                AssigneeId = dto.AssigneeId,
                ProjectId = dto.ProjectId
            };

            await _taskRepository.AddTasksAsync(task);
            await _taskRepository.SaveChangesAsync();

            return task;
        }

        public async Task AssignResponsibleAsync(int taskId, Manager manager)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId)
                ?? throw new ArgumentException($"Task with ID {taskId} not found.");

            task.AssigneeId = manager.UserId;

            await _taskRepository.UpdateTasksAsync(task);
            await _taskRepository.SaveChangesAsync();
        }
    }
}