using System.Text.RegularExpressions;

namespace CbtLearningPlatform.Tests;

/// <summary>Weekly Final Assessment Standard — platform-wide regression coverage. Confirms every
/// currently routed week (1-12) migrated its final assessment to the one shared FinalAssessment
/// component, and that in-lesson formative/retrieval checks were left as the original static reveal
/// interaction — never swept into the scored component.</summary>
public class FinalAssessmentRolloutTests
{
    public static IEnumerable<object[]> RoutedWeeks =>
        new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }.Select(n => new object[] { n });

    [Theory]
    [MemberData(nameof(RoutedWeeks))]
    public void Week_UsesSharedFinalAssessmentComponent_ExactlyOnce(int weekNumber)
    {
        string source = ReadPage($"Sedmica{weekNumber}.razor");

        int usageCount = Regex.Matches(source, $@"<FinalAssessment SectionId=""[\w-]+"" Model=""_week{weekNumber}FinalAssessment""\s*/>").Count;

        Assert.Equal(1, usageCount);
    }

    [Theory]
    [MemberData(nameof(RoutedWeeks))]
    public void Week_FinalAssessmentSectionInstructsSubmittingAll_NotTheOldNonScoredClaim(int weekNumber)
    {
        string source = ReadPage($"Sedmica{weekNumber}.razor");

        Assert.Contains("за да предадете теста и видите резултата", source);
        Assert.DoesNotContain("Не се оценява, не се пази", source);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(9)]
    public void Week_LocalFormativeChecksElsewhereOnThePage_StillUseTheOriginalStaticRevealPattern(int weekNumber)
    {
        // These weeks are known (from the pre-retrofit content survey) to have local/retrieval checks
        // scattered outside the final assessment, in addition to it — e.g. Week 3's ~13 in-lesson
        // checks, or Weeks 6/7/9's "Обобщение на локалните проверки" summary of earlier checks. If the
        // retrofit had accidentally swept those into the scored engine too, this raw reveal markup
        // (which FinalAssessment.razor never emits) would disappear.
        string source = ReadPage($"Sedmica{weekNumber}.razor");

        Assert.Contains("<details class=\"progressive-explanation\">", source);
    }

    [Fact]
    public void SharedFormativeRevealComponent_WasNotTouchedOrCoupledToScoring()
    {
        // ProgressiveExplanation.razor is the pre-existing formative self-check reveal pattern used
        // throughout every lesson. The Weekly Final Assessment Standard added a separate, new
        // component (FinalAssessment.razor) instead of modifying this one, so formative checks keep
        // behaving exactly as before — immediate reveal, never scored, never gated by a submit step.
        string source = ReadComponent("ProgressiveExplanation.razor");

        Assert.DoesNotContain("FinalAssessmentState", source);
        Assert.DoesNotContain("AssessmentQuestion", source);
        Assert.DoesNotContain("Предай теста", source);
    }

    [Fact]
    public void Week4And5_AreNowOwnerApprovedAndLocked_RetrofitPreservedLockStatus()
    {
        // Verify that Weeks 4 and 5 are now marked OWNER APPROVED / LOCKED after owner visual review.
        // The canonical status record is 00_PROJECT_OS/02_CURRENT_STATUS.md (page source files
        // don't carry a page-level lock marker — Week 5's header, for instance, cites its
        // SOURCE_AUDIT doc status, not the page's own status).
        string status = File.ReadAllText(Path.Combine(TestPaths.FindSolutionRoot(), "..", "00_PROJECT_OS", "02_CURRENT_STATUS.md"));

        Assert.Contains("Седмица 4", status);
        Assert.Contains("Седмица 5", status);
        Assert.Contains("OWNER APPROVED / LOCKED", status);
        // Verify the global milestone: all 15/15 weeks are locked
        Assert.Contains("ALL 15 COURSE WEEKS — `OWNER APPROVED / LOCKED`", status);
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    private static string ReadComponent(string fileName)
    {
        string sharedDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Shared");
        return File.ReadAllText(Path.Combine(sharedDirectory, fileName));
    }
}
