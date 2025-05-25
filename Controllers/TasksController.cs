using Microsoft.AspNetCore.Mvc;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Domain.Services.Contracts.Repositories;
using TaskTracker.Domain.Services.UseCases;
using TaskTracker.Domain.ValueObject;
using TaskTracker.Domain.ValueObject.Project;

namespace TaskTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController(
        ICreateTaskService createTaskService,
        ITaskCompletionConfirmationService completionService,
        IViewTaskService viewTaskService,
        IAccessControlService accessControl,
        IUnitOfWork unitOfWork) : ControllerBase
    {
        #region Вспомогательные методы
        private async Task<bool> ValidateRole(int userId, UserRole requiredRole)
            => (await unitOfWork.Users.GetByIdAsync(userId))?.Role == requiredRole;

        private static ObjectResult Forbidden() => new(null) { StatusCode = 403 };
        private static ObjectResult NotFound(string message) => new(new { Message = message }) { StatusCode = 404 };
        #endregion

        #region Операции администратора
        /// <summary>
        /// Создать задачу (автоматическая установка creatorId = adminId)
        /// </summary>
        [HttpPost("{adminId}")]
        public async Task<ObjectResult> CreateTask(
            [FromRoute] int adminId,
            [FromBody] CreateTaskRequest request)
        {
            var adminUser = await unitOfWork.Users.GetByIdAsync(adminId);
            if (adminUser == null) return NotFound("Администратор не найден");
            if (adminUser.Role != UserRole.Administrator) return Forbidden();

            if (!await accessControl.ValidateProjectAccessAsync(adminId, request.ProjectId))
                return Forbidden();

            try
            {
                var task = new Tasks
                {
                    Title = request.Title,
                    TaskDescription = request.TaskDescription,
                    TasksStatus = TasksStatus.New,
                    TasksPriority = request.TasksPriority,
                    Deadline = request.Deadline,
                    CreatorId = adminId,
                    ProjectId = request.ProjectId
                };

                var admin = new Administrators { AdminId = adminId, User = adminUser };
                var createdTask = await createTaskService.CreateTaskAsync(task, admin);
                return new ObjectResult(createdTask) { StatusCode = 201 };
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Назначить ответственного
        /// </summary>
        [HttpPatch("{adminId}/tasks/{taskId}/assign")]
        public async Task<ObjectResult> AssignResponsible(
            [FromRoute] int adminId,
            [FromRoute] int taskId)
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
                await createTaskService.AssignResponsibleAsync(taskId, admin);
                return new ObjectResult(null) { StatusCode = 204 };
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        #endregion

        #region Операции сотрудника
        /// <summary>
        /// Запросить подтверждение выполнения
        /// </summary>
        [HttpPost("{employeeId}/tasks/{taskId}/request-confirmation")]
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
                return new ObjectResult(null) { StatusCode = 204 };
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Получить свои задачи
        /// </summary>
        [HttpGet("{employeeId}/tasks")]
        public async Task<ObjectResult> GetEmployeeTasks([FromRoute] int employeeId)
        {
            if (!await ValidateRole(employeeId, UserRole.Employee))
                return Forbidden();

            var tasks = await viewTaskService.GetTeamMemberTasksAsync(employeeId);
            return tasks.Count == 0
                ? NotFound("Задачи не найдены")
                : new ObjectResult(tasks) { StatusCode = 200 };
        }
        #endregion

        #region Операции менеджера
        /// <summary>
        /// Подтвердить выполнение
        /// </summary>
        [HttpPost("{managerId}/tasks/{taskId}/confirm")]
        public async Task<ObjectResult> ConfirmCompletion(
            [FromRoute] int managerId,
            [FromRoute] int taskId,
            [FromBody] string? comment)
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
                await completionService.ConfirmTaskCompletionAsync(taskId, manager, comment);
                return new ObjectResult(null) { StatusCode = 204 };
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Получить задачи на подтверждение
        /// </summary>
        [HttpGet("{managerId}/pending-tasks")]
        public async Task<ObjectResult> GetPendingConfirmations([FromRoute] int managerId)
        {
            if (!await ValidateRole(managerId, UserRole.Manager))
                return Forbidden();

            var tasks = await completionService.GetPendingConfirmationsAsync(managerId);
            return tasks.Count == 0
                ? NotFound("Задачи не найдены")
                : new ObjectResult(tasks) { StatusCode = 200 };
        }
        #endregion

        #region Общие операции
        /// <summary>
        /// Получить детали задачи
        /// </summary>
        [HttpGet("{taskId}")]
        public async Task<ObjectResult> GetTaskDetails(int taskId)
        {
            var details = await viewTaskService.GetTaskDetailsAsync(taskId);
            return details == null
                ? NotFound("Задача не найдена")
                : new ObjectResult(details) { StatusCode = 200 };
        }
        #endregion

        #region DTO Classes
        public class CreateTaskRequest
        {
            public required string Title { get; set; }
            public string? TaskDescription { get; set; }
            public TasksPriority TasksPriority { get; set; }
            public DateOnly Deadline { get; set; }
            public int ProjectId { get; set; }
        }
        #endregion
    }
}