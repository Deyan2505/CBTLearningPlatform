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
    public void Calculator_AllSixRoutedWeeksCompleted_ComputesPercentageOutOfAllFifteen()
    {
        // Percentage is out of the whole 15-week curriculum, not just the currently routed
        // weeks — "X/15" is what the owner-approved UI shows.
        HashSet<int> completed = [1, 3, 6, 8, 10, 12];

        CourseProgressSummary summary = CourseProgressCalculator.Summarize(Weeks, completed);

        Assert.Equal(6, summary.CompletedCount);
        Assert.Equal(15, summary.TotalWeeks);
        Assert.Equal(40, summary.PercentageComplete); // round(100 * 6 / 15)
    }

    // ---- Route-less weeks cannot be falsely counted; status alone never decides eligibility ----

    [Fact]
    public void Calculator_RoutedAcademicOverviewWeek12_CanBeCompleted()
    {
        // Week 12 has a real lesson page (/kurs/sedmica-12) but is intentionally AcademicOverview,
        // not Available — course safety/presentation status and lesson existence are separate
        // concerns. A real page must be completable regardless of its status.
        CourseWeekDefinition week12 = Weeks.Single(w => w.Number == 12);
        Assert.NotNull(week12.Route);
        Assert.NotEqual(CourseWeekStatus.Available, week12.Status);

        CourseProgressSummary summary = CourseProgressCalculator.Summarize(Weeks, new HashSet<int> { 12 });

        Assert.Equal(1, summary.CompletedCount);
    }

    [Fact]
    public void Calculator_CompletedSetNamingARouteLessWeek_DoesNotCountIt()
    {
        // Week 15 has no lesson page at all (Route is null) in the real CourseCatalog — stale or
        // tampered localStorage naming it must never inflate the learner's shown progress, no
        // matter what CourseWeekStatus it happens to carry.
        CourseWeekDefinition week15 = Weeks.Single(w => w.Number == 15);
        Assert.Null(week15.Route);

        HashSet<int> completed = [1, 15];

        CourseProgressSummary summary = CourseProgressCalculator.Summarize(Weeks, completed);

        Assert.Equal(1, summary.CompletedCount); // only week 1 counts, not the route-less week 15
    }

    [Fact]
    public void Calculator_StatusAloneNeverDeterminesEligibility_OnlyRouteDoes()
    {
        // Synthetic weeks, isolated from the real CourseCatalog, to prove the rule precisely:
        // Available-but-unrouted must NOT count, and routed-but-not-Available (e.g. Week 12's real
        // shape) MUST count. If eligibility were ever re-tied to Status, one of these would flip.
        CourseWeekDefinition availableNoRoute = new(
            101, "Test", "Available, no route", "", CourseWeekStatus.Available,
            CurriculumSafetyLevel.PublicCore, null, [], []);
        CourseWeekDefinition academicOverviewRouted = new(
            102, "Test", "AcademicOverview, routed", "", CourseWeekStatus.AcademicOverview,
            CurriculumSafetyLevel.AcademicContextOnly, "/kurs/sedmica-102", [], []);
        CourseWeekDefinition[] syntheticWeeks = [availableNoRoute, academicOverviewRouted];

        Assert.False(CourseProgressCalculator.IsCounted(syntheticWeeks, 101));
        Assert.True(CourseProgressCalculator.IsCounted(syntheticWeeks, 102));
    }

    [Fact]
    public void Calculator_IsCounted_TrueOnlyForRoutedWeeks()
    {
        Assert.True(CourseProgressCalculator.IsCounted(Weeks, 1));
        Assert.True(CourseProgressCalculator.IsCounted(Weeks, 12)); // AcademicOverview, but routed
        Assert.False(CourseProgressCalculator.IsCounted(Weeks, 15)); // no Route at all
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
    [InlineData("Sedmica12.razor", 12)] // routed but AcademicOverview — still completable, route decides
    public void RoutedWeekPage_UsesTheSharedCompletionControl(string fileName, int weekNumber)
    {
        string source = ReadPage(fileName);

        Assert.Contains("<WeekCompletionControl WeekNumber=\"@_week.Number\" />", source);
        Assert.Contains($"CourseCatalog.Weeks.Single(w => w.Number == {weekNumber})", source);
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
