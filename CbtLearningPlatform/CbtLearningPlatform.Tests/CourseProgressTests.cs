using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>MVP course-progress feature (localStorage-only, no backend). Pure-logic coverage for
/// <see cref="CourseProgressStore"/>/<see cref="CourseProgressCalculator"/> — the actual browser
/// localStorage round-trip and post-refresh behavior are verified separately via a real browser
/// (owner visual review), since xUnit has no DOM/localStorage to exercise.</summary>
public sealed class CourseProgressTests
{
    private static readonly IReadOnlyList<CourseWeekDefinition> Weeks = CourseCatalog.Weeks;

    // ---- Serialization (save/load persistence, refresh-persistence proxy) ----

    [Fact]
    public void Store_SerializeThenDeserialize_RoundTripsExactly()
    {
        HashSet<int> completed = [1, 3, 8];

        string raw = CourseProgressStore.Serialize(completed);
        IReadOnlySet<int> restored = CourseProgressStore.Deserialize(raw);

        Assert.Equal(completed, restored);
    }

    [Fact]
    public void Store_Deserialize_EmptyOrMissingValue_ReturnsEmptySet()
    {
        Assert.Empty(CourseProgressStore.Deserialize(null));
        Assert.Empty(CourseProgressStore.Deserialize(""));
        Assert.Empty(CourseProgressStore.Deserialize("   "));
    }

    [Fact]
    public void Store_Deserialize_CorruptedJson_ReturnsEmptySetInsteadOfThrowing()
    {
        IReadOnlySet<int> restored = CourseProgressStore.Deserialize("{not valid json");

        Assert.Empty(restored);
    }

    // ---- Complete / uncomplete ----

    [Fact]
    public void Calculator_MarkingAWeekComplete_IncreasesCompletedCount()
    {
        CourseProgressSummary before = CourseProgressCalculator.Summarize(Weeks, new HashSet<int>());
        CourseProgressSummary after = CourseProgressCalculator.Summarize(Weeks, new HashSet<int> { 1 });

        Assert.Equal(0, before.CompletedCount);
        Assert.Equal(1, after.CompletedCount);
    }

    [Fact]
    public void Calculator_UndoingCompletion_ReturnsToPreviousCount()
    {
        HashSet<int> completed = [1, 3];
        CourseProgressSummary withBoth = CourseProgressCalculator.Summarize(Weeks, completed);

        completed.Remove(3);
        CourseProgressSummary afterUndo = CourseProgressCalculator.Summarize(Weeks, completed);

        Assert.Equal(2, withBoth.CompletedCount);
        Assert.Equal(1, afterUndo.CompletedCount);
    }

    // ---- Percentage ----

    [Fact]
    public void Calculator_NoWeeksCompleted_ZeroPercent()
    {
        CourseProgressSummary summary = CourseProgressCalculator.Summarize(Weeks, new HashSet<int>());

        Assert.Equal(0, summary.CompletedCount);
        Assert.Equal(15, summary.TotalWeeks);
        Assert.Equal(0, summary.PercentageComplete);
    }

    [Fact]
    public void Calculator_AllFiveAvailableWeeksCompleted_ComputesPercentageOutOfAllFifteen()
    {
        // Percentage is out of the whole 15-week curriculum, not just the 5 currently unlockable
        // weeks — "X/15" is what the owner-approved UI shows.
        HashSet<int> completed = [1, 3, 6, 8, 10];

        CourseProgressSummary summary = CourseProgressCalculator.Summarize(Weeks, completed);

        Assert.Equal(5, summary.CompletedCount);
        Assert.Equal(15, summary.TotalWeeks);
        Assert.Equal(33, summary.PercentageComplete); // round(100 * 5 / 15)
    }

    // ---- Unavailable/future weeks cannot be falsely counted ----

    [Fact]
    public void Calculator_CompletedSetNamingAnUnavailableWeek_DoesNotCountIt()
    {
        // Week 15 is AcademicOverview, not Available, in the real CourseCatalog — stale or
        // tampered localStorage naming it must never inflate the learner's shown progress.
        CourseWeekDefinition week15 = Weeks.Single(w => w.Number == 15);
        Assert.NotEqual(CourseWeekStatus.Available, week15.Status);

        HashSet<int> completed = [1, 15];

        CourseProgressSummary summary = CourseProgressCalculator.Summarize(Weeks, completed);

        Assert.Equal(1, summary.CompletedCount); // only week 1 counts, not the unavailable week 15
    }

    [Fact]
    public void Calculator_IsCounted_TrueOnlyForAvailableWeeks()
    {
        Assert.True(CourseProgressCalculator.IsCounted(Weeks, 1));
        Assert.False(CourseProgressCalculator.IsCounted(Weeks, 12)); // AcademicOverview, not Available
        Assert.False(CourseProgressCalculator.IsCounted(Weeks, 15)); // AcademicOverview, not Available
        Assert.False(CourseProgressCalculator.IsCounted(Weeks, 999)); // doesn't exist at all
    }

    // ---- Structural: shared component, not page-specific logic ----

    [Fact]
    public void WeekCompletionControl_HasBothExactLabels_AndDoesNotAutoCompleteOnInit()
    {
        string source = ReadSharedComponent("WeekCompletionControl.razor");

        Assert.Contains("Отбележи като завършена", source);
        Assert.Contains("✓</span> Седмицата е завършена", source);

        // Opening the page must never itself mark a week complete: OnInitializedAsync may only
        // read the stored state (IsWeekCompleteAsync), never write it (SetWeekCompleteAsync).
        int initStart = source.IndexOf("OnInitializedAsync", StringComparison.Ordinal);
        int initEnd = source.IndexOf('}', initStart);
        string initBody = source[initStart..initEnd];

        Assert.Contains("IsWeekCompleteAsync", initBody);
        Assert.DoesNotContain("SetWeekCompleteAsync", initBody);
    }

    [Theory]
    [InlineData("Sedmica1.razor", 1)]
    [InlineData("Sedmica3.razor", 3)]
    [InlineData("Sedmica6.razor", 6)]
    [InlineData("Sedmica8.razor", 8)]
    [InlineData("Sedmica10.razor", 10)]
    public void AvailableWeekPage_UsesTheSharedCompletionControl(string fileName, int weekNumber)
    {
        string source = ReadPage(fileName);

        Assert.Contains("<WeekCompletionControl WeekNumber=\"@_week.Number\" />", source);
        Assert.Contains($"CourseCatalog.Weeks.Single(w => w.Number == {weekNumber})", source);
    }

    [Fact]
    public void Week12Page_IsNotAvailable_SoItDoesNotOfferCompletion()
    {
        // Week 12 is AcademicOverview (routed but not Available) — the completion control is only
        // for currently available weekly lesson pages.
        string source = ReadPage("Sedmica12.razor");

        Assert.DoesNotContain("WeekCompletionControl", source);
    }

    [Fact]
    public void KursPage_ShowsCourseWideProgress_OutOfAllFifteenWeeks()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("<progress class=\"course-progress__bar\"", source);
        Assert.Contains("_summary.CompletedCount", source);
        Assert.Contains("_summary.TotalWeeks", source);
        Assert.Contains("_summary.PercentageComplete", source);
        Assert.Contains("CourseProgressCalculator.Summarize(CourseCatalog.Weeks", source);
    }

    [Fact]
    public void KursPage_MarksCompletedWeeksInTheWeekList_WithoutTouchingTheLockedCourseMap()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("week-list__status--done", source);
        // The locked Course Map (ConceptGraph/MindMapBranch, rendered on /kurs/karta) must stay
        // untouched by this feature — Kurs.razor's own plain week-timeline list is a different,
        // non-locked render path.
        Assert.DoesNotContain("ConceptGraph", source);
    }

    [Fact]
    public void ProgressFeature_HasNoGamificationLanguage()
    {
        string[] sources =
        [
            ReadSharedComponent("WeekCompletionControl.razor"),
            ReadPage("Kurs.razor"),
        ];

        string[] forbiddenTerms = ["points", "streak", "серия", "значка", "badge", "ниво", "level up"];

        foreach (string source in sources)
        {
            foreach (string term in forbiddenTerms)
            {
                Assert.DoesNotContain(term, source, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    private static string ReadSharedComponent(string fileName)
    {
        string sharedDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Shared");
        return File.ReadAllText(Path.Combine(sharedDirectory, fileName));
    }
}
