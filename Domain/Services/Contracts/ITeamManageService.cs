using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts
{
    /// <summary>
    /// Сервис управления командами
    /// </summary>
    public interface ITeamManageService
    {
        /// <summary>
        /// Создание новой команды
        /// </summary>
        /// <param name="employee">Основатель команды</param>
        /// <returns>Созданная команда</returns>
        Task<Teams> CreateTeamAsync(Employees employee);

        /// <summary>
        /// Добавление участника в команду
        /// </summary>
        /// <param name="teamId">Идентификатор команды</param>
        /// <param name="employeeId">Идентификатор пользователя</param>
        Task AddMemberAsync(int teamId, int employeeId);

        /// <summary>
        /// Удаление участника из команды
        /// </summary>
        /// <param name="teamId">Идентификатор команды</param>
        /// <param name="employeeId">Идентификатор пользователя</param>
        Task RemoveMemberFromTeamAsync(int teamId, int employeeId);

        /// <summary>
        /// Удаление команды
        /// </summary>
        /// <param name="teamId">Идентификатор команды</param>
        Task DeleteTeamAsync(int teamId);

        /// <summary>
        /// Переназначение менеджера команды
        /// </summary>
        /// <param name="teamId">Идентификатор команды</param>
        /// <param name="managerId">Идентификатор нового менеджера</param>
        Task ReassignManagerAsync(int teamId, int managerId);
    }
}