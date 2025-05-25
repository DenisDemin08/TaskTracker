namespace TaskTracker.Domain.Entities
{
    /// <summary>
    /// Представляет команду сотрудников
    /// </summary>
    public class Teams
    {
        /// <summary>Уникальный идентификатор команды</summary>
        public int TeamId { get; set; }

        /// <summary>Название команды</summary>
        public required string Name { get; set; }

        /// <summary>Описание команды</summary>
        public string? Description { get; set; }

        /// <summary>Идентификатор связанного проекта</summary>
        public int? ProjectId { get; set; }

        /// <summary>Идентификатор ответственного менеджера</summary>
        public int ManagerId { get; set; }
    }
}