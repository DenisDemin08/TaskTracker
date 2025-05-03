using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities
{
    public class TeamMember: User
    {
        public TeamMember()
        {
            UserId = UserId;
            Role = UserRole.TeamMember;
        }
        public int TeamId { get; set; }
        public TaskPosition Position { get; set; }
    }
}
