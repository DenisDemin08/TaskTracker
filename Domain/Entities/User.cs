using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
        {
            public int UserId { get; set; }
            public string Email { get; set; } = null!;
            public string PasswordHash { get; set; } = null!;
            public string FullName { get; set; } = null!;
            public UserRole Role { get; set; }
            
        }
}