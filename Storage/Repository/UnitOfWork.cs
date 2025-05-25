using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TaskTracker.Domain.Services.Contracts.Repositories;

namespace TaskTracker.Storage.Repository
{
    public class UnitOfWork(
        TaskTrackerdbContext context,
        IUserRepository userRepository,
        IAdministratorRepository administratorRepository,
        IManagerRepository managerRepository,
        IEmployeeRepository employeeRepository,
        IProjectRepository projectRepository,
        ITasksRepository tasksRepository,
        ITeamRepository teamRepository) : IUnitOfWork
    {
        private IDbContextTransaction? transaction;
        public IUserRepository Users { get; } = userRepository;
        public IProjectRepository Projects { get; } = projectRepository;
        public IAdministratorRepository Administrator { get; } = administratorRepository;
        public IManagerRepository Manager { get; } = managerRepository;
        public IEmployeeRepository Employees { get; } = employeeRepository;
        public ITasksRepository Tasks { get; } = tasksRepository;
        public ITeamRepository Teams { get; } = teamRepository;

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
        public async Task BeginTransactionAsync()
       => transaction = await context.Database.BeginTransactionAsync();
        public async Task CommitAsync()
        {
            if (transaction != null)
            {
                await transaction.CommitAsync();
                transaction = null;
            }
        }
        public async Task RollbackAsync()
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
                transaction = null;
            }
        }
    }
}