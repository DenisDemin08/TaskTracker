namespace TaskTracker.Domain.Services.Contracts.Repositories
{
    /// <summary>
    /// Единица работы для управления транзакциями и репозиториями
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>Репозиторий пользователей</summary>
        IUserRepository Users { get; }

        /// <summary>Репозиторий администраторов</summary>
        IAdministratorRepository Administrator { get; }

        /// <summary>Репозиторий менеджеров</summary>
        IManagerRepository Manager { get; }

        /// <summary>Репозиторий сотрудников</summary>
        IEmployeeRepository Employees { get; }

        /// <summary>Репозиторий проектов</summary>
        IProjectRepository Projects { get; }

        /// <summary>Репозиторий задач</summary>
        ITasksRepository Tasks { get; }

        /// <summary>Репозиторий команд</summary>
        ITeamRepository Teams { get; }

        /// <summary>Сохранить изменения</summary>
        Task SaveChangesAsync();

        /// <summary>Начать транзакцию</summary>
        Task BeginTransactionAsync();

        /// <summary>Зафиксировать транзакцию</summary>
        Task CommitAsync();

        /// <summary>Откатить транзакцию</summary>
        Task RollbackAsync();
    }
}