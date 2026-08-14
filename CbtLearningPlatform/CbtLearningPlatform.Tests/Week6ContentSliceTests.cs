using System.Reflection;
using CbtLearningPlatform.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>SYSTEMATIC CURRICULUM EXPANSION, Phase B, Batch A — Week 6 ("Структура на
/// терапевтичната сесия"), first implementation candidate. "Guided Practice" archetype (primary)
/// with a "Concept and Diagram" secondary pattern — an informational, third-person overview of
/// what a typical CBT session looks like, not a technique the reader applies to themselves.
/// Source: SRC-041 (Judith Beck), Глава 5 — confirmed via session log narration and GAP-010's
/// resolution (45–50 min duration, 3-part structure). No stage list beyond that confirmed shape
/// is asserted anywhere in this page or these tests.</summary>
public sealed class Week6ContentSliceTests
{
    [Fact]
    public void Week6Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Components.Pages.Sedmica6"));
    }

    [Fact]
    public void Week6_MetadataIsAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 6);

        Assert.Equal(CourseWeekStatus.Available, week.Status);
        Assert.Equal("/kurs/sedmica-6", week.Route);
    }

    [Fact]
    public void Week1Week3Week8Week10_RemainAvailableAfterWeek6WasAdded()
    {
        int[] weeksToCheck = [1, 3, 8, 10];

        foreach (int number in weeksToCheck)
        {
            CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == number);
            Assert.Equal(CourseWeekStatus.Available, week.Status);
            Assert.Equal($"/kurs/sedmica-{number}", week.Route);
        }
    }

    [Fact]
    public void RemainingTenWeeks_StayUnavailable()
    {
        int[] availableNumbers = [1, 3, 6, 8, 10];

        foreach (CourseWeekDefinition week in CourseCatalog.Weeks.Where(w => !availableNumbers.Contains(w.Number)))
        {
            Assert.Null(week.Route);
            Assert.NotEqual(CourseWeekStatus.Available, week.Status);
        }

        Assert.Equal(10, CourseCatalog.Weeks.Count(w => !availableNumbers.Contains(w.Number)));
    }

    [Fact]
    public void Week6Page_HasPageTitleAndFormatBadge()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("<PageTitle>Седмица 6: Структура на терапевтичната сесия", source);
        Assert.Contains("Демонстрация", source);
    }

    [Fact]
    public void Week6Page_HasNoNewInteractiveComponent()
    {
        // Explicit instruction: zero new reusable components / interactive islands for this week.
        string source = ReadPage("Sedmica6.razor");

        Assert.DoesNotContain("@rendermode", source);
    }

    [Fact]
    public void Week6Page_UsesOnlyExistingReusablePatterns()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("<LearningSection", source);
        Assert.Contains("<LearningObjectives", source);
        Assert.Contains("<ProgressiveExplanation", source);
        Assert.Contains("<DisclaimerCallout", source);
        Assert.Contains("<SourceReferences", source);
        Assert.Contains("<OptionalReadingSource", source);
        Assert.Contains("class=\"category-compare\"", source);
        Assert.Contains("class=\"concept-map__side-notes\"", source);
    }

    [Fact]
    public void Week6Page_HasConfirmedDurationClaim_NoInventedStageList()
    {
        string source = ReadPage("Sedmica6.razor");
        string publicMarkup = ReadPublicMarkup("Sedmica6.razor");

        // GAP-010-confirmed, verbatim-adjacent fact — the only precise numeric claim on the page.
        Assert.Contains("45–50 минути", source);

        // No diagnostic-instrument mood-check name from the original prototype (excluded by
        // design — the dev comment explains the exclusion, so it is checked against the public
        // markup only, matching the established ReadPublicMarkup convention).
        Assert.DoesNotContain("BDI", publicMarkup);
        Assert.DoesNotContain("BAI", publicMarkup);

        // No invented technique names beyond the confirmed general 3-part shape.
        Assert.DoesNotContain("поведенчески експерименти", source);
        Assert.DoesNotContain("Agenda Setting", source);
    }

    [Fact]
    public void Week6Page_HasThreePartSessionShape()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("Начало (~5–10 мин.)", source);
        Assert.Contains("Среда (~30–35 мин.)", source);
        Assert.Contains("Край (~5–10 мин.)", source);
    }

    [Fact]
    public void Week6Page_DistinguishesOverviewFromSelfTherapyInstruction()
    {
        // CourseCatalog's own second learning objective for this week.
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("Информационен преглед", source);
        Assert.Contains("Инструкция за самотерапия", source);
        Assert.Contains("не заменя професионална психологическа или медицинска помощ", source);
    }

    [Fact]
    public void Week6Page_HasNoInternalDevelopmentLanguageInItsRenderableMarkup()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica6.razor");

        string[] forbiddenTerms =
        [
            "11_SOURCE_REGISTER.md", "kpt_syllabus.pdf", "citation-grade",
            "ACADEMIC/CLINICAL REVIEW PENDING", "10_SESSION_LOG.md", "Project OS",
            "code_artifact.html", "14_EXISTING_PROTOTYPE_AUDIT.md", "DROP"
        ];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }
    }

    [Fact]
    public void Week6Page_MakesNoFalseAccreditationClaims()
    {
        string source = ReadPage("Sedmica6.razor");

        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week6Page_HasNoDiagnosticContent()
    {
        // Checked against the public markup only — the dev comment legitimately names BDI/BAI
        // to explain why the prototype's mood-check step was excluded (ReadPublicMarkup strips
        // that comment before the assertion, matching the established convention).
        string publicMarkup = ReadPublicMarkup("Sedmica6.razor");

        string[] forbiddenTerms = ["BDI", "BAI", "BHS", "диагности"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }
    }

    [Fact]
    public void Week6Page_SectionNavAnchorsAreRouteSafe()
    {
        string source = ReadPage("Sedmica6.razor");

        string[] anchorIds = ["struktura", "tri-chasti", "pregled-instrukcia", "proverka", "izvori"];

        foreach (string id in anchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-6#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
            Assert.Contains($"id=\"{id}\"", source);
        }
    }

    [Fact]
    public void Week6Page_CrossLinksToWeek8AndWeek10_NoDeadLinks()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("/kurs/sedmica-8", source);
        Assert.Contains("/kurs/sedmica-10", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Week6Page_DoesNotLinkToWeek5AsIfAvailable()
    {
        // Week 5 has not been implemented yet (frozen build order position 5, after Week 6).
        string source = ReadPage("Sedmica6.razor");

        Assert.DoesNotContain("/kurs/sedmica-5", source);
    }

    [Fact]
    public void KursPage_ShowsAllFiveAvailableWeeks()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("/kurs/sedmica-1", source);
        Assert.Contains("/kurs/sedmica-3", source);
        Assert.Contains("/kurs/sedmica-6", source);
        Assert.Contains("/kurs/sedmica-8", source);
        Assert.Contains("/kurs/sedmica-10", source);
    }

    [Fact]
    public void Week6Page_HasNoPageLevelOverflowWorkaround()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.DoesNotContain("overflow-x: hidden", source);
        Assert.DoesNotContain("overflow-x:hidden", source);
    }

    [Fact]
    public void Week6Page_HasNoInlineStylesOrAbsolutePositioning()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.DoesNotContain("style=", source);
        Assert.DoesNotContain("position: absolute", source);
        Assert.DoesNotContain("position:absolute", source);
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    /// <summary>Strips the leading @* ... *@ dev comment — Razor comments never reach the
    /// browser, so only the text after them reflects what a visitor could actually see.</summary>
    private static string ReadPublicMarkup(string fileName)
    {
        string source = ReadPage(fileName);
        int commentEnd = source.IndexOf("*@", StringComparison.Ordinal);
        return commentEnd >= 0 ? source[(commentEnd + 2)..] : source;
    }
}
