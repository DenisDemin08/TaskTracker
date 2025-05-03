using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities
{
    public class Administrator : User
    {
        public Administrator()
        {
            UserId = UserId;
            Role = UserRole.Administrator;
        }
    }
}
