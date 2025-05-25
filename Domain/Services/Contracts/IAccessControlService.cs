namespace TaskTracker.Domain.Services.Contracts
{
    /// <summary>
    /// Сервис контроля доступа к ресурсам системы
    /// </summary>
    public interface IAccessControlService
    {
        /// <summary>
        /// Проверка доступа пользователя к проекту
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="projectId">Идентификатор проекта</param>
        /// <returns>Результат проверки доступа</returns>
        Task<bool> ValidateProjectAccessAsync(int userId, int projectId);

        /// <summary>
        /// Проверка принадлежности задачи пользователю
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <returns>Результат проверки владения задачей</returns>
        Task<bool> ValidateTaskOwnershipAsync(int userId, int taskId);

        /// <summary>
        /// Проверка является ли пользователь менеджером команды
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="teamId">Идентификатор команды</param>
        /// <returns>Результат проверки роли менеджера</returns>
        Task<bool> IsTeamManagerAsync(int userId, int teamId);
    }
}