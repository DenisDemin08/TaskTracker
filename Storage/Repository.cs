using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Services.Contracts;


namespace TaskTracker.Storage
{
    public class TaskTrackerRepository : IRepository
    {
        private readonly TaskTrackerDbContext _context;

        public TaskTrackerRepository(TaskTrackerDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> GetUserByFullNameAsync(string fullname)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.FullName == fullname);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddUserAsync(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<User> UpdateUserAsync(User updateUser)
        {
            _context.Users.Update(updateUser);
            await _context.SaveChangesAsync();
            return updateUser;
        }

        public async Task<int> RemoveUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return 0;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Project>> GetAdminProjectsAsync(Administrator admin)
        {
            return await _context.Projects
                .Where(p => p.AdministratorId == admin.UserId)
                .ToListAsync();
        }

        public async Task<Project> GetProjectByNameAsync(string name)
        {
            return await _context.Projects
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<List<Team>> GetProjectTeamsAsync(Project project)
        {
            return await _context.Teams
                .Where(t => t.ProjectId == project.ProjectId)
                .ToListAsync();
        }

        public async Task<Team> GetTeamByIdAsync(int teamId)
        {
            return await _context.Teams.FindAsync(teamId);
        }

        public async Task<Project> AddProjectAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<int> RemoveProjectAsync(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null) return 0;

            _context.Projects.Remove(project);
            return await _context.SaveChangesAsync();
        }


        public async Task<List<Tasks>> GetAdminTasksAsync(Administrator admin)
        {
            return await _context.Tasks
                .Where(t => t.CreatorId == admin.UserId)
                .ToListAsync();
        }

        public async Task<List<Tasks>> GetTeamMemberTasksAsync(TeamMember teammember)
        {
            return await _context.Tasks
                .Where(t => t.AssigneeId == teammember.UserId)
                .ToListAsync();
        }

        public async Task<Tasks> AddTasksAsync(Tasks task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Tasks> UpdateTasksAsync(Tasks tasks)
        {
            _context.Tasks.Update(tasks);
            await _context.SaveChangesAsync();
            return tasks;
        }

        public async Task<int> RemoveTasksAsync(int tasksId)
        {
            var task = await _context.Tasks.FindAsync(tasksId);
            if (task == null) return 0;

            _context.Tasks.Remove(task);
            return await _context.SaveChangesAsync();
        }


        public async Task<List<Team>> GetManagerTeamsAsync(Manager manager)
        {
            return await _context.Teams
                .Where(t => t.ManagerId == manager.UserId)
                .ToListAsync();
        }

        public async Task<List<TeamMember>> GetTeamMembersAsync(Team team)
        {
            return await _context.TeamMembers
                .Where(tm => tm.TeamId == team.TeamId)
                .ToListAsync();
        }

        public async Task<Team> AddTeamAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<Team> UpdateTeamAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<int> RemoveTeamAsync(int teamId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null) return 0;

            _context.Teams.Remove(team);
            return await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<Project> GetProjectByIdAsync(int projectId)
        {
            return await _context.Projects.FindAsync(projectId);
        }

        public async Task<Tasks> GetTaskByIdAsync(int taskId)
        {
            return await _context.Tasks.FindAsync(taskId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
