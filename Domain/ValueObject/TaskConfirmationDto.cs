namespace TaskTracker.Domain.ValueObject
{
    public class TaskConfirmationDto
    {
        public required string Status { get; set; }
        public required string RequesterName { get; set; }
        public required string ConfirmerName { get; set; }
        public DateTime RequestDate { get; set; }
        public string? Comment { get; set; }
        public string? Reason { get; set; }
    }
}
