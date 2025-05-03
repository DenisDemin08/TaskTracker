using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Services.Contracts
{
    /// <summary>
    /// Интерфейс репозитория для работы с данными в TaskTracker
    /// </summary>
    /// <remarks>
    /// Предоставляет методы для управления пользователями, проектами, задачами и командами
    /// </remarks>
    public interface IRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        /// <summary>
        /// Получает пользователя по полному имени
        /// </summary>
        /// <param name="fullname">Полное имя пользователя</param>
        /// <returns>Объект пользователя</returns>
        Task<User> GetUserByFullNameAsync(string fullname);

        /// <summary>
        /// Получает пользователя по email
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <returns>Объект пользователя</returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Добавляет нового пользователя
        /// </summary>
        /// <param name="newUser">Объект нового пользователя</param>
        /// <returns>Добавленный пользователь</returns>
        Task<User> AddUserAsync(User newUser);

        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        /// <param name="updateUser">Объект обновляемого пользователя</param>
        /// <returns>Обновленный пользователь</returns>
        Task<User> UpdateUserAsync(User updateUser);

        /// <summary>
        /// Удаляет пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Количество удаленных записей</returns>
        Task<int> RemoveUserAsync(int userId);

        /// <summary>
        /// Получает проекты администратора
        /// </summary>
        /// <param name="admin">Объект администратора</param>
        /// <returns>Список проектов</returns>
        Task<List<Project>> GetAdminProjectsAsync(Administrator admin);

        /// <summary>
        /// Получает задачи администратора
        /// </summary>
        /// <param name="admin">Объект администратора</param>
        /// <returns>Список задач</returns>
        Task<List<Tasks>> GetAdminTasksAsync(Administrator admin);

        /// <summary>
        /// Получает команды менеджера
        /// </summary>
        /// <param name="manager">Объект менеджера</param>
        /// <returns>Список команд</returns>
        Task<List<Team>> GetManagerTeamsAsync(Manager manager);

        /// <summary>
        /// Получает задачи члена команды
        /// </summary>
        /// <param name="teammember">Объект члена команды</param>
        /// <returns>Список задач</returns>
        Task<List<Tasks>> GetTeamMemberTasksAsync(TeamMember teammember);

        Task<Project> GetProjectByIdAsync(int projectId);
        /// <summary>
        /// Получает проект по названию
        /// </summary>
        /// <param name="name">Название проекта</param>
        /// <returns>Объект проекта</returns>
        Task<Project> GetProjectByNameAsync(string name);

        /// <summary>
        /// Получает команды проекта
        /// </summary>
        /// <param name="project">Объект проекта</param>
        /// <returns>Список команд проекта</returns>
        Task<List<Team>> GetProjectTeamsAsync(Project project);

        /// <summary>
        /// Добавляет новый проект
        /// </summary>
        /// <param name="project">Объект проекта</param>
        /// <returns>Добавленный проект</returns>
        Task<Project> AddProjectAsync(Project project);

        /// <summary>
        /// Обновляет данные проекта
        /// </summary>
        /// <param name="project">Объект проекта</param>
        /// <returns>Обновленный проект</returns>
        Task<Project> UpdateProjectAsync(Project project);

        /// <summary>
        /// Удаляет проект
        /// </summary>
        /// <param name="projectId">ID проекта</param>
        /// <returns>Количество удаленных записей</returns>
        Task<int> RemoveProjectAsync(int projectId);

        Task<Tasks> GetTaskByIdAsync(int taskId);
        /// <summary>
        /// Добавляет новую задачу
        /// </summary>
        /// <param name="tasks">Объект задачи</param>
        /// <returns>Добавленная задача</returns>
        Task<Tasks> AddTasksAsync(Tasks tasks);

        /// <summary>
        /// Обновляет данные задачи
        /// </summary>
        /// <param name="tasks">Объект задачи</param>
        /// <returns>Обновленная задача</returns>
        Task<Tasks> UpdateTasksAsync(Tasks tasks);

        /// <summary>
        /// Удаляет задачу
        /// </summary>
        /// <param name="tasksId">ID задачи</param>
        /// <returns>Количество удаленных записей</returns>
        Task<int> RemoveTasksAsync(int tasksId);

        /// <summary>
        /// Получает участников команды
        /// </summary>
        /// <param name="team">Объект команды</param>
        /// <returns>Список участников команды</returns>
        Task<List<TeamMember>> GetTeamMembersAsync(Team team);

        Task<Team> GetTeamByIdAsync(int teamId);

        /// <summary>
        /// Добавляет новую команду
        /// </summary>
        /// <param name="team">Объект команды</param>
        /// <returns>Добавленная команда</returns>
        Task<Team> AddTeamAsync(Team team);

        /// <summary>
        /// Обновляет данные команды
        /// </summary>
        /// <param name="team">Объект команды</param>
        /// <returns>Обновленная команда</returns>
        Task<Team> UpdateTeamAsync(Team team);

        /// <summary>
        /// Удаляет команду
        /// </summary>
        /// <param name="teamId">ID команды</param>
        /// <returns>Количество удаленных записей</returns>
        Task<int> RemoveTeamAsync(int teamId);
        Task SaveChangesAsync();

        Task AddTeamMemberAsync(TeamMember member);
        Task RemoveTeamMemberAsync(TeamMember member);
    }
}