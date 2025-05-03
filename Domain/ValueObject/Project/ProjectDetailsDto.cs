using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.ValueObject.Project
{
    /// <summary>
    /// DTO для отображения детальной информации о проекте
    /// </summary>
    /// <param name="Id">Идентификатор проекта</param>
    /// <param name="Name">Название проекта</param>
    /// <param name="Description">Описание проекта</param>
    /// <param name="Status">Текущий статус проекта</param>
    /// <param name="StartDate">Дата начала проекта</param>
    /// <param name="Deadline">Планируемая дата завершения</param>
    /// <param name="AssignedTeam">Назначенная команда</param>
    /// <param name="Tasks">Список задач проекта</param>
    
    public record ProjectDetailsDto(
        int Id,
        string Name,
        string Description,
        Domain.Entities.Project Status,
        DateTime StartDate,
        DateTime? Deadline,
        TeamInfo AssignedTeam,
        List<TaskInfo> Tasks

    );


    public record TeamInfo(
        int Id,
        string Name,
        string Manager
    );

    public record TaskInfo(
        int Id,
        string Title,
        Tasks Status,
        string Assignee
    );
}
