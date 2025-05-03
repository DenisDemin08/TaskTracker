namespace TaskTracker.Domain.Entities
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? ProjectId { get; set; }
        public int ManagerId { get; set; }
    }
}
