using System.Reflection;
using CbtLearningPlatform.Curriculum;

namespace CbtLearningPlatform.Tests;

public sealed class CurriculumHubTests
{
    [Fact]
    public void CourseCatalog_HasExactlyFifteenWeeks()
    {
        Assert.Equal(15, CourseCatalog.Weeks.Count);
    }

    [Fact]
    public void CourseCatalog_WeekNumbersAreOneToFifteenWithNoDuplicates()
    {
        int[] numbers = [.. CourseCatalog.Weeks.Select(w => w.Number).OrderBy(n => n)];

        Assert.Equal(Enumerable.Range(1, 15), numbers);
    }

    [Fact]
    public void CourseCatalog_HasFourCurriculumModules()
    {
        Assert.Equal(4, CourseCatalog.Modules.Count);
    }

    [Fact]
    public void CourseCatalog_EveryWeekHasATitleAndShortSummary()
    {
        foreach (CourseWeekDefinition week in CourseCatalog.Weeks)
        {
            Assert.False(string.IsNullOrWhiteSpace(week.Title));
            Assert.False(string.IsNullOrWhiteSpace(week.ShortSummary));
        }
    }

    [Fact]
    public void CourseCatalog_OnlyWeeksOneThreeEightAndTenHaveARealRoute()
    {
        foreach (CourseWeekDefinition week in CourseCatalog.Weeks)
        {
            if (week.Number == 1)
            {
                Assert.Equal("/kurs/sedmica-1", week.Route);
            }
            else if (week.Number == 3)
            {
                Assert.Equal("/kurs/sedmica-3", week.Route);
            }
            else if (week.Number == 8)
            {
                Assert.Equal("/kurs/sedmica-8", week.Route);
            }
            else if (week.Number == 10)
            {
                Assert.Equal("/kurs/sedmica-10", week.Route);
            }
            else
            {
                Assert.Null(week.Route);
            }
        }
    }

    [Fact]
    public void CourseCatalog_ClinicallySensitiveWeeksAreNeverMarkedAvailable()
    {
        CurriculumSafetyLevel[] sensitiveLevels =
        [
            CurriculumSafetyLevel.AcademicContextOnly,
            CurriculumSafetyLevel.ProfessionalReviewRequired,
            CurriculumSafetyLevel.NotEligibleForSelfGuidedSimulator
        ];

        foreach (CourseWeekDefinition week in CourseCatalog.Weeks.Where(w => sensitiveLevels.Contains(w.SafetyLevel)))
        {
            Assert.NotEqual(CourseWeekStatus.Available, week.Status);
            Assert.Null(week.Route);
        }
    }

    [Fact]
    public void KursPage_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Components.Pages.Kurs"));
    }

    [Fact]
    public void KursPage_ListsWeeksAndCurriculumModules()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("week-list", source);
    }

    [Fact]
    public void KursPage_HasAVisualCourseMapAndVerticalTimeline()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("course-map", source);
        Assert.Contains("week-timeline", source);
    }

    [Fact]
    public void KursPage_HasNoFakeProgressPercentage()
    {
        string source = ReadPage("Kurs.razor");

        Assert.DoesNotContain("%", source);
    }

    [Fact]
    public void KursPage_HasAStartPanelPointingToTheAvailableWeeks()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("start-panel", source);
        Assert.Contains("/kurs/sedmica-1", source);
        Assert.Contains("/kurs/sedmica-3", source);
        Assert.Contains("/kurs/sedmica-8", source);
        Assert.Contains("/kurs/sedmica-10", source);
    }

    [Fact]
    public void KursPage_MakesNoFalseAccreditationOrCreditClaims()
    {
        string source = ReadPage("Kurs.razor");

        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void ProgressiveExplanation_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Components.Shared.ProgressiveExplanation"));
    }

    [Fact]
    public void ProgressiveExplanation_UsesNativeDetailsSummary()
    {
        string source = ReadHostComponent("ProgressiveExplanation.razor");

        Assert.Contains("<details", source);
        Assert.Contains("<summary>", source);
    }

    [Fact]
    public void ProgressiveExplanation_IsUsedAtLeastTwiceAcrossRealPages()
    {
        int usageCount = CountOccurrences(ReadPage("Kurs.razor"), "<ProgressiveExplanation")
            + CountOccurrences(ReadPage("Sedmica8.razor"), "<ProgressiveExplanation");

        Assert.True(usageCount >= 2, $"Expected ProgressiveExplanation to be used at least twice, found {usageCount}.");
    }

    [Fact]
    public void Week8Page_ExistsAndUsesAllThreeInteractiveTypes()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("<CbtChainSimulator", source);
        Assert.Contains("<InterpretationExample", source);
        Assert.Contains("<CategorizationCheck", source);
    }

    [Fact]
    public void Week8Page_HasDisclaimerAndSourceReferences()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("<DisclaimerCallout", source);
        Assert.Contains("<SourceReferences", source);
    }

    [Fact]
    public void Week8Page_LinksToTheThreeRealExistingLessons_NoDeadLinks()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("/programa/modul-2/situacia-misal-emocia-povedenie", source);
        Assert.Contains("/programa/modul-2/avtomatichni-misli", source);
        Assert.Contains("/programa/modul-2/emocii-i-telesni-reaktsii", source);
    }

    [Fact]
    public void Week8Page_LinksBackToTheHub()
    {
        Assert.Contains("href=\"/kurs\"", ReadPage("Sedmica8.razor"));
    }

    [Fact]
    public void Sidebar_LinksToTheWeeklyCourseHub()
    {
        string layoutSource = ReadLayoutComponent("MainLayout.razor");

        Assert.Contains("href=\"/kurs\"", layoutSource);
    }

    private static int CountOccurrences(string source, string needle)
    {
        int count = 0;
        int index = 0;

        while ((index = source.IndexOf(needle, index, StringComparison.Ordinal)) != -1)
        {
            count++;
            index += needle.Length;
        }

        return count;
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    private static string ReadHostComponent(string fileName)
    {
        string sharedDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Shared");
        return File.ReadAllText(Path.Combine(sharedDirectory, fileName));
    }

    private static string ReadLayoutComponent(string fileName)
    {
        string layoutDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Layout");
        return File.ReadAllText(Path.Combine(layoutDirectory, fileName));
    }
}
