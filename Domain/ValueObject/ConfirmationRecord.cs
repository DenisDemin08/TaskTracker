using TaskTracker.Domain.Enums;
namespace TaskTracker.Domain.ValueObject
{
    public record ConfirmationRecord(
        DateTime Timestamp,
        ConfirmationAction Action,
        string UserName,
        string? Comment
    );
}
