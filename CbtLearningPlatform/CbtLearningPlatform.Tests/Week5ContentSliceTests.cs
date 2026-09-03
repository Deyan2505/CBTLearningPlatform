using System.Reflection;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>WEEK 5 — DEEP LEARNING MODULE. Source: SRC-041 (Judith Beck), Глава 1 "Какви са
/// основните принципи на лечението?" (printed стр. 6–11) + Глава 2 "Обзор на лечението" — развиване
/// на терапевтичната връзка (printed стр. 17–21, from a full chapter read, pp.17–28). See
/// 00_PROJECT_OS/_blueprints/WEEK_05_SOURCE_AUDIT_v1.md (OWNER APPROVED — FINAL) for the complete
/// 52-KU accounting (30 Included / 10 Deferred / 12 Excluded / 0 Needs Review / 0 Unaccounted).
/// Owner scoping: each of the 10 principles is stated at principle level only; deeper mechanics
/// (conceptualization, automatic-thought work, core beliefs, full session structure) stay
/// cross-linked to Weeks 3/4/6/8/9/10/12, never re-taught. Terminology locks: "терапевтичен съюз"
/// (never "алианс"), "колаборативен емпиризъм" (never Ch.1's own "съвместен емпиризъм"). Chapter 2's
/// "emphasizing the positive" and "homework" threads have no curriculum owner (GAP-014) and are
/// deliberately absent from this page.</summary>
public sealed class Week5ContentSliceTests
{
    private static readonly string[] AnchorIds =
    [
        "karta", "zashto-printsipi", "desette-printsipa", "vremevo-ogranichenie",
        "terapevtichen-suyuz", "satrudnichestvo", "struktura-sluzhi", "poniatiya",
        "primenenie", "proverki", "assessment", "review-map", "izvori"
    ];

    [Fact]
    public void Week5Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica5"));
    }

    [Fact]
    public void Week5_IsNowRoutedAndAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 5);

        Assert.Equal("/kurs/sedmica-5", week.Route);
        Assert.Equal(CourseWeekStatus.Available, week.Status);
    }

    [Fact]
    public void PreviouslyRoutedWeeks_RemainAvailableAfterWeek5Routing()
    {
        int[] weeksToCheck = [1, 2, 3, 6, 7, 8, 9, 10];

        foreach (int number in weeksToCheck)
        {
            CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == number);
            Assert.Equal(CourseWeekStatus.Available, week.Status);
            Assert.Equal($"/kurs/sedmica-{number}", week.Route);
        }

        // Week 4/12 stay AcademicOverview despite being routed — unaffected by Week 5's routing.
        foreach (int number in new[] { 4, 12 })
        {
            CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == number);
            Assert.Equal(CourseWeekStatus.AcademicOverview, week.Status);
        }
    }

    [Fact]
    public void Week5Page_HasPageTitleAndDeepLearningBadge()
    {
        string source = ReadPage("Sedmica5.razor");

        Assert.Contains("<PageTitle>Седмица 5: Принципи на КПТ и терапевтичен съюз", source);
        Assert.Contains("Дълбочинен модул", source);
    }

    [Fact]
    public void Week5Page_HasAllThirteenSections()
    {
        string source = ReadPage("Sedmica5.razor");

        foreach (string id in AnchorIds)
        {
            Assert.Contains($"id=\"{id}\"", source);
        }

        string[] sectionNumbers = ["5.0", "5.1", "5.2", "5.3", "5.4", "5.5", "5.6", "5.7", "5.8", "5.9", "5.10", "5.11", "5.12"];
        foreach (string number in sectionNumbers)
        {
            Assert.Contains($"{number} ", source);
        }
    }

    [Fact]
    public void Week5Page_SectionNavAnchorsAreRouteSafe()
    {
        string source = ReadPage("Sedmica5.razor");

        foreach (string id in AnchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-5#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
        }
    }

    [Fact]
    public void Week5Page_UsesEstablishedReusablePatterns_ZeroNewComponents()
    {
        string source = ReadPage("Sedmica5.razor");

        Assert.Contains("<LearningSection", source);
        Assert.Contains("<LearningObjectives", source);
        Assert.Contains("<ProgressiveExplanation", source);
        Assert.Contains("<DisclaimerCallout", source);
        Assert.Contains("<SourceReferences", source);
        Assert.Contains("<OptionalReadingSource", source);
        Assert.Contains("<WhatIfBox", source);
        Assert.Contains("<WeekCompletionControl WeekNumber=\"@_week.Number\" />", source);
        Assert.Contains("category-compare", source);
        Assert.Contains("guided-practice-sequence", source);

        // Locked decision: no Mind Map / Concept Map, no simulator, no new component.
        Assert.DoesNotContain("<ConceptGraph", source);
        Assert.DoesNotContain("<ScenarioSimulator", source);
        Assert.DoesNotContain("<SourceArtifact", source);
        Assert.DoesNotContain("<CbtChainSimulator", source);
    }

    [Fact]
    public void Week5Page_HasAllTenPrinciples()
    {
        string source = ReadPage("Sedmica5.razor");

        for (int i = 1; i <= 10; i++)
        {
            Assert.Contains($"Принцип {i} —", source);
        }
    }

    [Fact]
    public void Week5Page_TerminologyLocks_CanonicalTermsOnly()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica5.razor");

        Assert.Contains("терапевтичен съюз", publicMarkup);
        Assert.Contains("колаборативен емпиризъм", publicMarkup);

        // Owner-locked (turn 3): the source's own "алианс" wording and Ch.1's own "съвместен
        // емпиризъм" rendering never surface in learner-facing copy.
        Assert.DoesNotContain("алианс", publicMarkup);
        Assert.DoesNotContain("съвместен емпиризъм", publicMarkup);
    }

    [Fact]
    public void Week5Page_C2K02Citation_HasNoNamedAuthor()
    {
        // The source's own author rendering for the alliance-outcome finding is OCR-uncertain
        // (audit C2-K02) — owner decision #3 (turn 3): finding only, never a named citation here.
        string publicMarkup = ReadPublicMarkup("Sedmica5.razor");

        Assert.DoesNotContain("Голдфрид", publicMarkup);
        Assert.DoesNotContain("Рауе", publicMarkup);
    }

    [Fact]
    public void Week5Page_CrossLinksDeferredTerritory_InsteadOfReteaching()
    {
        string source = ReadPage("Sedmica5.razor");

        // Full conceptualization model → Week 3/4, not re-taught here.
        Assert.Contains("/kurs/sedmica-3", source);
        Assert.Contains("/kurs/sedmica-4", source);
        // Full session-structure breakdown → Week 6.
        Assert.Contains("/kurs/sedmica-6", source);
        // Full automatic-thought/guided-discovery method → Weeks 8-10.
        Assert.Contains("/kurs/sedmica-8", source);
        Assert.Contains("/kurs/sedmica-10", source);
        // Core beliefs → Week 12.
        Assert.Contains("/kurs/sedmica-12", source);
    }

    [Fact]
    public void Week5Page_DoesNotReteachDeferredMechanics()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica5.razor");

        // Sally's canonical case-intro facts (K03, Deferred to Week 3) never appear.
        Assert.DoesNotContain("Сали", publicMarkup);
        // Core-belief specific content (K12a, Deferred to Week 12) never appears here.
        Assert.DoesNotContain("основно вярване", publicMarkup);
        // No reproduced therapist/patient dialogue anywhere on the page.
        Assert.DoesNotContain("TherapisT:", publicMarkup);
        Assert.DoesNotContain("paTienT:", publicMarkup);
    }

    [Fact]
    public void Week5Page_TaperingSequence_IsDescriptiveNotProtocol()
    {
        // Owner decision #6 (turn 3): the Principle 7 tapering sequence must read as descriptive,
        // not a rigid universal protocol.
        string publicMarkup = ReadPublicMarkup("Sedmica5.razor");

        Assert.Contains("не като строг протокол", publicMarkup);
        Assert.Contains("не е фиксирано правило", publicMarkup);
    }

    [Fact]
    public void Week5Page_NoSelfTherapyFraming()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica5.razor");

        Assert.Contains("не е инструкция", publicMarkup);
        Assert.DoesNotContain("твоята връзка", publicMarkup);
        Assert.DoesNotContain("вашата връзка", publicMarkup);
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages");
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
