using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities
{
    /// <summary>
    /// Представляет администратора системы
    /// </summary>
    public class Administrators
    {
        /// <summary>Уникальный идентификатор администратора</summary>
        public int AdminId { get; set; }

        /// <summary>Связанная сущность пользователя</summary>
        public required Users User { get; set; }
    }
}