using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities
{
    /// <summary>
    /// Представляет сотрудника в системе
    /// </summary>
    public class Employees
    {
        /// <summary>Уникальный идентификатор сотрудника</summary>
        public int EmployeeId { get; set; }

        /// <summary>Идентификатор связанной команды</summary>
        public int? TeamId { get; set; }

        /// <summary>Связанная сущность пользователя</summary>
        public required Users User { get; set; }

        /// <summary>Должность сотрудника в команде</summary>
        public MemberPosition? Position { get; set; }
    }
}