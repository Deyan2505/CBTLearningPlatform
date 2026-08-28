using System.Text.Json;

namespace CbtLearningPlatform.Client.Curriculum;

/// <summary>Learner-local course completion — never CBT content, never sent anywhere. Keyed by
/// <see cref="CourseWeekDefinition.Number"/>, the only stable week identifier. Pure logic only (no
/// browser/JS dependency) so it's directly unit-testable; <see cref="CourseProgressService"/> is the
/// thin localStorage-backed wrapper pages actually use.</summary>
public sealed record CourseProgressSummary(int CompletedCount, int TotalWeeks, int PercentageComplete);

public static class CourseProgressCalculator
{
    /// <summary>Completion eligibility is a real lesson page existing (<see cref="CourseWeekDefinition.Route"/>
    /// is not null) — never <see cref="CourseWeekStatus"/>. Status is a safety/presentation concern
    /// (is this week self-guided, academic-overview-only, pending professional review, ...); lesson
    /// existence is a separate concern. Week 12 is routed but AcademicOverview, and it must still be
    /// completable — while stale or tampered storage naming a week with no page at all must never
    /// inflate the learner's progress, regardless of what status that week happens to carry.</summary>
    public static CourseProgressSummary Summarize(IReadOnlyList<CourseWeekDefinition> weeks, IReadOnlySet<int> completedWeekNumbers)
    {
        HashSet<int> routedNumbers = [.. weeks.Where(w => w.Route is not null).Select(w => w.Number)];
        int completedCount = completedWeekNumbers.Count(routedNumbers.Contains);
        int total = weeks.Count;
        int percentage = total == 0 ? 0 : (int)Math.Round(100.0 * completedCount / total);

        return new CourseProgressSummary(completedCount, total, percentage);
    }

    public static bool IsCounted(IReadOnlyList<CourseWeekDefinition> weeks, int weekNumber) =>
        weeks.Any(w => w.Number == weekNumber && w.Route is not null);
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
