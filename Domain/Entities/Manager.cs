using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities
{
    public class Manager : User
    {
        public Manager()
        {
            UserId = UserId;
            Role = UserRole.Manager;
        }

    }
}
