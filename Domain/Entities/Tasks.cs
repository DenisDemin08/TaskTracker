using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities
{
    /// <summary>
    /// Представляет задачу в системе
    /// </summary>
    public class Tasks
    {
        /// <summary>Уникальный идентификатор задачи</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        /// <summary>Заголовок задачи</summary>
        public required string Title { get; set; }

        /// <summary>Подробное описание задачи</summary>
        public string? TaskDescription { get; set; }

        /// <summary>Текущий статус задачи</summary>
        public TasksStatus TasksStatus { get; set; }

        /// <summary>Приоритет задачи</summary>
        public TasksPriority TasksPriority { get; set; }

        /// <summary>Крайний срок выполнения</summary>
        public DateOnly Deadline { get; set; }

        /// <summary>Идентификатор создателя задачи</summary>
        public int CreatorId { get; set; }

        /// <summary>Идентификатор исполнителя задачи</summary>
        public int? AssigneeId { get; set; }

        /// <summary>Идентификатор связанного проекта</summary>
        public int ProjectId { get; set; }
    }
}