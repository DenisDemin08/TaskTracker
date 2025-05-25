using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities
{
    /// <summary>
    /// Представляет менеджера проектов
    /// </summary>
    public class Managers
    {
        /// <summary>Уникальный идентификатор менеджера</summary>
        public int ManagerId { get; set; }

        /// <summary>Связанная сущность пользователя</summary>
        public required Users User { get; set; }
    }
}