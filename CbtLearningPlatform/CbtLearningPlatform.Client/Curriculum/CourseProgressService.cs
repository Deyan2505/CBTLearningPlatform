using Microsoft.JSInterop;

namespace CbtLearningPlatform.Client.Curriculum;

/// <summary>Learner-local course completion, stored in the browser's own localStorage — no backend, no
/// account, no sync. Native `localStorage.getItem`/`setItem` calls (no wrapper JS file needed); actual
/// parsing/scoring lives in the pure, testable <see cref="CourseProgressStore"/>/<see cref="CourseProgressCalculator"/>.</summary>
public sealed class CourseProgressService(IJSRuntime js)
{
    public async Task<IReadOnlySet<int>> GetCompletedWeekNumbersAsync()
    {
        string? raw = await js.InvokeAsync<string?>("localStorage.getItem", CourseProgressStore.StorageKey);
        return CourseProgressStore.Deserialize(raw);
    }

    public async Task<bool> IsWeekCompleteAsync(int weekNumber) =>
        (await GetCompletedWeekNumbersAsync()).Contains(weekNumber);

    public async Task SetWeekCompleteAsync(int weekNumber, bool isComplete)
    {
        HashSet<int> completed = [.. await GetCompletedWeekNumbersAsync()];

        if (isComplete)
        {
            completed.Add(weekNumber);
        }
        else
        {
            completed.Remove(weekNumber);
        }

        await js.InvokeVoidAsync("localStorage.setItem", CourseProgressStore.StorageKey, CourseProgressStore.Serialize(completed));
    }

    public async Task<CourseProgressSummary> GetSummaryAsync(IReadOnlyList<CourseWeekDefinition> weeks) =>
        CourseProgressCalculator.Summarize(weeks, await GetCompletedWeekNumbersAsync());
}
