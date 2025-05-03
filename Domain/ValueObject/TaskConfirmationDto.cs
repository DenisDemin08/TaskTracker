namespace TaskTracker.Domain.ValueObject
{
    public record TaskConfirmationDto(
         int TaskId,
         string TaskTitle,
         DateTime RequestDate,
         string RequesterName
     );
}
