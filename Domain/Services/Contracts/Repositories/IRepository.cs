namespace TaskTracker.Domain.Services.Contracts.Repositories
{
    /// <summary>
    /// Базовый интерфейс репозитория для операций CRUD
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>Получить сущность по идентификатору</summary>
        /// <param name="id">Идентификатор сущности</param>
        Task<TEntity?> GetByIdAsync(int id);

        /// <summary>Получить все сущности</summary>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>Добавить новую сущность</summary>
        /// <param name="entity">Добавляемая сущность</param>
        Task AddAsync(TEntity entity);

        /// <summary>Обновить существующую сущность</summary>
        /// <param name="entity">Обновляемая сущность</param>
        Task UpdateAsync(TEntity entity);

        /// <summary>Удалить сущность</summary>
        /// <param name="entity">Удаляемая сущность</param>
        Task DeleteAsync(TEntity entity);
    }
}