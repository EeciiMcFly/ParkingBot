namespace YogaBot.Models.Poll;

public class Poll
{
    public string? Answer { get; init; }
    public string[]? Options { get; init; }
    public bool IsAnonymous { get; init; } = false;
    public bool AllowsMultipleAnswers { get; init; } = false;
    public DateTime CloseDate { get; init; }
}