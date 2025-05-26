namespace TaskTracker.Domain.ValueObject
{
    public record TeamCreateDto(string Name, int? ProjectId);
    public record TeamUpdateDto(string Name, int? ProjectId);
    public record TeamResponseDto(int TeamId, string Name, int? ProjectId);
}
