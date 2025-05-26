using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;
using TaskTracker.Domain.ValueObject;

namespace TaskTracker.Controllers
{
    /// <summary>
    /// Контроллер для управления задачами и их жизненным циклом
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController(
        ICreateTaskService createTaskService,
        ITaskCompletionConfirmationService completionService,
        IViewTaskService viewTaskService,
        IAccessControlService accessControl,
        IUnitOfWork unitOfWork) : ControllerBase
    {
        private async Task<bool> ValidateRole(int userId, UserRole requiredRole)
            => (await unitOfWork.Users.GetByIdAsync(userId))?.Role == requiredRole;

        private static  ObjectResult Forbidden() => new(null) { StatusCode = 403 };
        private static ObjectResult NotFound(string message) => new(new ErrorResponse(message)) { StatusCode = 404 };

        /// <summary>
        /// Создание новой задачи
        /// </summary>
        [HttpPost("{adminId}")]
        [ProducesResponseType(typeof(TaskResponseDto), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 403)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<ObjectResult> CreateTask(
            [FromRoute] int adminId,
            [FromBody] CreateTaskRequest request)
        {
            var adminUser = await unitOfWork.Users.GetByIdAsync(adminId);
            if (adminUser == null) return NotFound("Administrator not found");
            if (adminUser.Role != UserRole.Administrator) return Forbidden();

            if (!await accessControl.ValidateProjectAccessAsync(adminId, request.ProjectId))
                return Forbidden();

            try
            {
                var task = new Tasks
                {
                    Title = request.Title,
                    TaskDescription = request.TaskDescription,
                    TaskStatus = Domain.Enums.TaskStatus.New,
                    TaskPriority = request.TaskPriority,
                    Deadline = request.Deadline,
                    CreatorId = adminId,
                    ProjectId = request.ProjectId
                };

                var admin = new Administrators { AdminId = adminId, User = adminUser };
                var createdTask = await createTaskService.CreateTaskAsync(task, admin);
                return CreatedAtAction(nameof(CreateTask), MapToDto(createdTask));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Назначение ответственного сотрудника
        /// </summary>
        [HttpPatch("{adminId}/tasks/{taskId}/assign")]
        [ProducesResponseType(typeof(TaskResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 403)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<ObjectResult> AssignResponsible(
            [FromRoute] int adminId,
            [FromRoute] int taskId,
            [FromQuery] int employeeId)
        {
            if (!await ValidateRole(adminId, UserRole.Administrator))
                return Forbidden();

            try
            {
                var admin = new Administrators
                {
                    AdminId = adminId,
                    User = await unitOfWork.Users.GetByIdAsync(adminId) ?? throw new KeyNotFoundException()
                };
                await createTaskService.AssignResponsibleAsync(taskId, admin, employeeId);
                return await GetUpdatedTaskResult(taskId);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Запрос подтверждения выполнения задачи (статус изменяется на AwaitingConfirmation)
        /// </summary>
        [HttpPost("{employeeId}/tasks/{taskId}/request-confirmation")]
        [ProducesResponseType(typeof(TaskResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 403)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<ObjectResult> RequestConfirmation(
            [FromRoute] int employeeId,
            [FromRoute] int taskId)
        {
            if (!await ValidateRole(employeeId, UserRole.Employee))
                return Forbidden();
            if (!await accessControl.ValidateTaskOwnershipAsync(employeeId, taskId))
                return Forbidden();

            try
            {
                var employee = new Employees
                {
                    EmployeeId = employeeId,
                    User = await unitOfWork.Users.GetByIdAsync(employeeId) ?? throw new KeyNotFoundException()
                };
                await completionService.RequestConfirmationAsync(taskId, employee);
                return await GetUpdatedTaskResult(taskId);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Подтверждение выполнения задачи (статус изменяется на Completed)
        /// </summary>
        [HttpPost("{managerId}/tasks/{taskId}/confirm")]
        [ProducesResponseType(typeof(TaskResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 403)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<ObjectResult> ConfirmCompletion(
            [FromRoute] int managerId,
            [FromRoute] int taskId,
            [FromBody] ConfirmationRequest? request)
        {
            if (!await ValidateRole(managerId, UserRole.Manager))
                return Forbidden();

            try
            {
                var manager = new Managers
                {
                    ManagerId = managerId,
                    User = await unitOfWork.Users.GetByIdAsync(managerId) ?? throw new KeyNotFoundException()
                };
                await completionService.ConfirmTaskCompletionAsync(taskId, manager, request?.Comment);
                return await GetUpdatedTaskResult(taskId);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Отклонение выполнения задачи (статус изменяется на RequiresRevision)
        /// </summary>
        [HttpPost("{managerId}/tasks/{taskId}/reject")]
        [ProducesResponseType(typeof(TaskResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 403)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<ObjectResult> RejectCompletion(
            [FromRoute] int managerId,
            [FromRoute] int taskId,
            [FromBody] RejectionRequest request)
        {
            if (!await ValidateRole(managerId, UserRole.Manager))
                return Forbidden();

            try
            {
                var manager = new Managers
                {
                    ManagerId = managerId,
                    User = await unitOfWork.Users.GetByIdAsync(managerId) ?? throw new KeyNotFoundException()
                };
                await completionService.RejectTaskCompletionAsync(taskId, manager, request.Reason);
                return await GetUpdatedTaskResult(taskId);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Получение задач сотрудника
        /// </summary>
        [HttpGet("{employeeId}/tasks")]
        [ProducesResponseType(typeof(List<TaskResponseDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 403)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<ObjectResult> GetEmployeeTasks([FromRoute] int employeeId)
        {
            if (!await ValidateRole(employeeId, UserRole.Employee))
                return Forbidden();

            var tasks = await viewTaskService.GetTeamMemberTasksAsync(employeeId);
            return tasks.Count == 0
                ? NotFound("No tasks found")
                : Ok(tasks.Select(MapToDto).ToList());
        }

        /// <summary>
        /// Получение задач на подтверждение
        /// </summary>
        [HttpGet("{managerId}/pending-tasks")]
        [ProducesResponseType(typeof(List<TaskConfirmationDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 403)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<ObjectResult> GetPendingConfirmations([FromRoute] int managerId)
        {
            if (!await ValidateRole(managerId, UserRole.Manager))
                return Forbidden();

            var tasks = await completionService.GetPendingConfirmationsAsync(managerId);
            return tasks.Count == 0
                ? NotFound("No pending tasks")
                : Ok(tasks);
        }

        #region DTO
        public class CreateTaskRequest
        {
            [Required]
            public string Title { get; set; } = null!;

            public string? TaskDescription { get; set; }

            [Required]
            public TaskPriority TaskPriority { get; set; }

            [Required]
            public DateOnly Deadline { get; set; }

            [Required]
            public int ProjectId { get; set; }
        }

        public class RejectionRequest
        {
            [Required]
            public string Reason { get; set; } = null!;
        }

        #endregion

        private static TaskResponseDto MapToDto(Tasks task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "Task entity cannot be null");
            }

            return new TaskResponseDto
            {
                TaskId = task.TaskId,
                Status = task.TaskStatus,
                Title = task.Title,
                Description = task.TaskDescription,
                Deadline = task.Deadline,
            };
        }

        private async Task<ObjectResult> GetUpdatedTaskResult(int taskId)
        {
            var task = await unitOfWork.Tasks.GetByIdAsync(taskId);
            return task == null
                ? NotFound("Task not found after update")
                : Ok(MapToDto(task));
        }
    }
}