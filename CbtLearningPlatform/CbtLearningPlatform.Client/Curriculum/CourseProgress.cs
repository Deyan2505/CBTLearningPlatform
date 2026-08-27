using System.Text.Json;

namespace CbtLearningPlatform.Client.Curriculum;

/// <summary>Learner-local course completion — never CBT content, never sent anywhere. Keyed by
/// <see cref="CourseWeekDefinition.Number"/>, the only stable week identifier. Pure logic only (no
/// browser/JS dependency) so it's directly unit-testable; <see cref="CourseProgressService"/> is the
/// thin localStorage-backed wrapper pages actually use.</summary>
public sealed record CourseProgressSummary(int CompletedCount, int TotalWeeks, int PercentageComplete);

public static class CourseProgressCalculator
{
    /// <summary>Completed weeks are only ever counted against the total if the week is currently
    /// <see cref="CourseWeekStatus.Available"/> — stale or tampered storage naming an unavailable/future
    /// week must never inflate the learner's progress.</summary>
    public static CourseProgressSummary Summarize(IReadOnlyList<CourseWeekDefinition> weeks, IReadOnlySet<int> completedWeekNumbers)
    {
        HashSet<int> availableNumbers = [.. weeks.Where(w => w.Status == CourseWeekStatus.Available).Select(w => w.Number)];
        int completedCount = completedWeekNumbers.Count(availableNumbers.Contains);
        int total = weeks.Count;
        int percentage = total == 0 ? 0 : (int)Math.Round(100.0 * completedCount / total);

        return new CourseProgressSummary(completedCount, total, percentage);
    }

    public static bool IsCounted(IReadOnlyList<CourseWeekDefinition> weeks, int weekNumber) =>
        weeks.Any(w => w.Number == weekNumber && w.Status == CourseWeekStatus.Available);
}

/// <summary>Serialization for the raw completed-week-number set stored in localStorage. Isolated from
/// JS interop so the round-trip (and its failure modes — missing key, corrupted JSON) is unit-testable.</summary>
public static class CourseProgressStore
{
    public const string StorageKey = "cbt-course-progress";

    public static string Serialize(IReadOnlySet<int> completedWeekNumbers) =>
        JsonSerializer.Serialize(completedWeekNumbers.OrderBy(n => n));

    public static IReadOnlySet<int> Deserialize(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return new HashSet<int>();
        }

        try
        {
            int[]? numbers = JsonSerializer.Deserialize<int[]>(raw);
            return numbers is null ? new HashSet<int>() : [.. numbers];
        }
        catch (JsonException)
        {
            return new HashSet<int>();
        }
    }
}
