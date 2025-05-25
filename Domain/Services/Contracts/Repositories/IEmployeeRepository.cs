using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts.Repositories
{
    /// <summary>
    /// Репозиторий для работы с членами команд
    /// </summary>
    public interface IEmployeeRepository : IRepository<Employees>
    {
        /// <summary>
        /// Получить сотрудников по команде
        /// </summary>
        /// <param name="teamId">Идентификатор команды</param>
        /// <returns>Список сотрудников</returns>
        Task<List<Employees>> GetByTeamIdAsync(int teamId);
    }
}