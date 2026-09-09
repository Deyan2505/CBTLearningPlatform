using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>Protects the learner-facing safety-classification labels (owner terminology fix,
/// 2026-09-09): "Изисква професионален преглед" read as an editorial/review workflow status, which
/// misrepresented OWNER APPROVED / LOCKED weeks (11/13/14) as unfinished. Internal enums,
/// DeriveStatus, and routing/eligibility logic are unchanged — only display text moved. Week 4 is
/// deliberately excluded (still not owner-approved; out of scope for this pass) and keeps the old
/// "Академичен обзор" wording on its own page.</summary>
public sealed class CurriculumSafetyLabelTests
{
    [Fact]
    public void StatusLabel_AcademicOverview_IsAcademicContext()
    {
        Assert.Equal("Академичен контекст", CourseWeekStatus.AcademicOverview.ToPublicLabel());
    }

    [Fact]
    public void StatusLabel_ProfessionalReviewRequired_IsProfessionalContext_NotAReviewWorkflowClaim()
    {
        Assert.Equal("Професионален контекст", CourseWeekStatus.ProfessionalReviewRequired.ToPublicLabel());
        Assert.DoesNotContain("преглед", CourseWeekStatus.ProfessionalReviewRequired.ToPublicLabel());
    }

    [Theory]
    [InlineData(11, "Професионален контекст")] // SafetyLevel.ProfessionalReviewRequired
    [InlineData(13, "Без самостоятелна практика")] // SafetyLevel.NotEligibleForSelfGuidedSimulator — stricter tier, distinct label
    [InlineData(14, "Професионален контекст")] // SafetyLevel.ProfessionalReviewRequired
    [InlineData(12, "Академичен контекст")] // SafetyLevel.AcademicContextOnly
    [InlineData(15, "Академичен контекст")] // SafetyLevel.AcademicContextOnly
    public void WeekLevelLabel_MatchesExpectedSafetyClassification(int weekNumber, string expectedLabel)
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == weekNumber);

        Assert.Equal(expectedLabel, week.ToPublicLabel());
    }

    [Fact]
    public void WeekLevelLabel_Week13Differs_FromWeek11And14_DespiteSameCourseWeekStatus()
    {
        // All three resolve to the same CourseWeekStatus.ProfessionalReviewRequired (eligibility is
        // unaffected), but Week 13's stricter SafetyLevel must read differently to a learner.
        CourseWeekDefinition week11 = CourseCatalog.Weeks.Single(w => w.Number == 11);
        CourseWeekDefinition week13 = CourseCatalog.Weeks.Single(w => w.Number == 13);
        CourseWeekDefinition week14 = CourseCatalog.Weeks.Single(w => w.Number == 14);

        Assert.Equal(CourseWeekStatus.ProfessionalReviewRequired, week11.Status);
        Assert.Equal(CourseWeekStatus.ProfessionalReviewRequired, week13.Status);
        Assert.Equal(CourseWeekStatus.ProfessionalReviewRequired, week14.Status);

        Assert.Equal(week11.ToPublicLabel(), week14.ToPublicLabel());
        Assert.NotEqual(week11.ToPublicLabel(), week13.ToPublicLabel());
    }

    [Fact]
    public void OwnerApprovedLockedWeeks_NeverShowTheOldReviewWorkflowWording()
    {
        // Weeks 11, 13, 14 are OWNER APPROVED / LOCKED — their safety classification must never read
        // like an unfinished/awaiting-review editorial status.
        foreach (string fileName in new[] { "Sedmica11.razor", "Sedmica13.razor", "Sedmica14.razor" })
        {
            string source = ReadPage(fileName);
            Assert.DoesNotContain("Изисква професионален преглед", source);
        }
    }

    [Theory]
    [InlineData("Kurs.razor")]
    [InlineData("Programa.razor")]
    [InlineData("Sedmica11.razor")]
    [InlineData("Sedmica12.razor")]
    [InlineData("Sedmica13.razor")]
    [InlineData("Sedmica14.razor")]
    [InlineData("Sedmica15.razor")]
    public void PublicSurface_UsesCanonicalLabelHelper_NotAHardcodedDuplicate(string fileName)
    {
        string source = ReadPage(fileName);

        Assert.DoesNotContain("Изисква професионален преглед", source);
        Assert.DoesNotContain("Академичен обзор", source);
    }

    [Fact]
    public void KursPage_SidebarLegend_ListsAllThreeDistinctRestrictedLabels()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("<dt>Академичен контекст</dt>", source);
        Assert.Contains("<dt>Професионален контекст</dt>", source);
        Assert.Contains("<dt>Без самостоятелна практика</dt>", source);
        Assert.DoesNotContain("Изисква професионален преглед", source);
        Assert.DoesNotContain("Академичен обзор", source);
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }
}
