namespace TaskTracker.Domain.Entities
{
    /// <summary>
    /// Представляет проект в системе
    /// </summary>
    public class Projects
    {
        /// <summary>Уникальный идентификатор проекта</summary>
        public int ProjectId { get; set; }

        /// <summary>Название проекта</summary>
        public string? Name { get; set; }

        /// <summary>Описание проекта</summary>
        public string? Description { get; set; }

        /// <summary>Дата начала проекта</summary>
        public DateOnly StartDate { get; set; }

        /// <summary>Дата окончания проекта (если задана)</summary>
        public DateOnly? EndDate { get; set; }

        /// <summary>Идентификатор ответственного администратора</summary>
        public int AdministratorId { get; set; }
    }
}