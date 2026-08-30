using System.Reflection;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>CONTENT-DRIVEN TEMPLATE VALIDATION, Slice 1 — Week 1 ("Theory and History" archetype),
/// distinct from Week 8's ("Simulator Workspace") coverage in CurriculumHubTests.cs.</summary>
public sealed class Week1ContentSliceTests
{
    [Fact]
    public void Week1Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica1"));
    }

    [Fact]
    public void Week1_MetadataIsAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 1);

        Assert.Equal(CourseWeekStatus.Available, week.Status);
        Assert.Equal("/kurs/sedmica-1", week.Route);
    }

    [Fact]
    public void Week8_RemainsAvailableAfterWeek1WasAdded()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 8);

        Assert.Equal(CourseWeekStatus.Available, week.Status);
        Assert.Equal("/kurs/sedmica-8", week.Route);
    }

    [Fact]
    public void RemainingWeeks_StayUnavailable()
    {
        int[] availableNumbers = [1, 3, 6, 7, 8, 9, 10];
        int[] stillUnavailable = [.. CourseCatalog.Weeks
            .Where(w => !availableNumbers.Contains(w.Number))
            .Select(w => w.Number)];

        Assert.Equal(8, stillUnavailable.Length);

        foreach (CourseWeekDefinition week in CourseCatalog.Weeks.Where(w => !availableNumbers.Contains(w.Number)))
        {
            Assert.NotEqual(CourseWeekStatus.Available, week.Status);

            // Week 12 is AcademicContextOnly: it later gained a real, routed AcademicOverview
            // page without becoming Available — every other non-available week still has no route.
            if (week.Number != 12)
            {
                Assert.Null(week.Route);
            }
        }
    }

    [Fact]
    public void Week1Page_HasPageTitleAndLearningObjectives()
    {
        string source = ReadPage("Sedmica1.razor");

        Assert.Contains("<PageTitle>", source);
        Assert.Contains("<LearningObjectives", source);
    }

    [Fact]
    public void Week1Page_UsesHistoricalTimelineAsASemanticOrderedList()
    {
        string source = ReadPage("Sedmica1.razor");
        string timelineComponentSource = ReadHostComponent("HistoricalTimeline.razor");

        Assert.Contains("<HistoricalTimeline", source);
        Assert.Contains("<ol", timelineComponentSource);
    }

    [Fact]
    public void Week1Page_HistoricalTimelineIncludesTheCoreMilestones()
    {
        string source = ReadPage("Sedmica1.razor");

        Assert.Contains("психоаналитичната традиция", source);
        Assert.Contains("не потвърждават", source);
        Assert.Contains("1979", source);
    }

    [Fact]
    public void Week1Page_HasTheResearchTurnStepperAsItsOnlyInteractiveIsland()
    {
        string source = ReadPage("Sedmica1.razor");

        Assert.Contains("<ResearchTurnStepper", source);
        Assert.DoesNotContain("<CbtChainSimulator", source);
        Assert.DoesNotContain("<CategorizationCheck", source);
        Assert.DoesNotContain("<InterpretationExample", source);
    }

    [Fact]
    public void ResearchTurnStepper_HasNoScoringStorageOrDiagnosticLanguage()
    {
        string source = ReadClientComponent("ResearchTurnStepper.razor");

        string[] forbiddenTerms = ["localStorage", "sessionStorage", "fetch(", "HttpClient", "%", "диагноз", "точки"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week1Page_HasAutomaticThoughtPreview_ReconciledDefinition_NotDuplicatingWeek8Simulator()
    {
        string source = ReadPage("Sedmica1.razor");

        // Reconciled wording — automatic thoughts are not framed as "lacking objective validity".
        Assert.Contains("могат да бъдат точни, неточни", source);
        Assert.DoesNotContain("лишени от обективна валидност", source);
        Assert.Contains("Седмица 8", source);
    }

    [Fact]
    public void Week1Page_HasThe1979MilestoneAsADirectPrimaryCitation()
    {
        string source = ReadPage("Sedmica1.razor");

        Assert.Contains("Beck, A.T., Rush, A.J., Shaw, B.F.", source);
        Assert.Contains("Cognitive Therapy for Depression", source);
    }

    [Fact]
    public void Week1Page_HasANonScoredKnowledgeCheckUsingNativeDetailsSummary_NotANewQuizEngine()
    {
        string source = ReadPage("Sedmica1.razor");

        Assert.Contains("<details", source);
        Assert.Contains("<summary>", source);
        Assert.DoesNotContain("%", source);
        Assert.DoesNotContain("успешен студент", source);
        Assert.DoesNotContain("неуспешен студент", source);
    }

    [Fact]
    public void Week1Page_HasEducationalDisclaimerAndAcademicContextDisclosure()
    {
        string source = ReadPage("Sedmica1.razor");

        Assert.Contains("<DisclaimerCallout", source);
        Assert.DoesNotContain("Variant=\"safety\"", source);
        Assert.Contains("<ProgressiveExplanation SummaryLabel=\"Покажи академичния контекст\"", source);
    }

    [Fact]
    public void Week1Page_TitleIsShort_AndDoesNotRepeatHistoricalTransitionPhraseTwice()
    {
        string source = ReadPage("Sedmica1.razor");

        Assert.Contains("<PageTitle>Седмица 1: Как се ражда когнитивната терапия", source);

        // The old long CourseCatalog title must be gone everywhere, not just on this page.
        Assert.DoesNotContain("Въведение в когнитивната терапия и исторически преход", source);
        string catalogSource = ReadHostFile("Curriculum/CourseCatalog.cs");
        Assert.DoesNotContain("Въведение в когнитивната терапия и исторически преход", catalogSource);
    }

    [Fact]
    public void Week1Page_ShowsAContentFormatBadge_NotAnAvailabilityStatusPill()
    {
        string source = ReadPage("Sedmica1.razor");

        Assert.Contains("week-list__status--format", source);
        Assert.DoesNotContain("week-list__status--@_week.Status.ToStatusModifier()", source);
    }

    [Fact]
    public void Week1Page_HasNoInternalDevelopmentLanguageInItsRenderableMarkup()
    {
        // Scoped to everything after the top @* ... *@ dev comment — Razor comments never
        // reach the browser, so file references there are fine; the rendered body must not
        // leak them (Project OS file names, internal QA-status jargon, uppercase EN status).
        string publicMarkup = ReadPublicMarkup("Sedmica1.razor");

        string[] forbiddenTerms =
        [
            "11_SOURCE_REGISTER.md", "kpt_syllabus.pdf", "citation-grade",
            "ACADEMIC/CLINICAL REVIEW PENDING", "10_SESSION_LOG.md", "Project OS"
        ];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }
    }

    [Fact]
    public void Week1Page_PublicReviewStatusIsPlainBulgarian_NotUppercaseEnglish()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica1.razor");

        Assert.Contains("предстои да премине независим академичен и професионален преглед", publicMarkup);
    }

    [Fact]
    public void ResearchTurnStepper_HasFourSemanticStepsAndNoTrailingConnector()
    {
        string source = ReadClientComponent("ResearchTurnStepper.razor");

        Assert.Contains("Първоначална хипотеза", source);
        Assert.Contains("Наблюдение и проверка", source);
        Assert.Contains("Резултатът не потвърждава очакването", source);
        Assert.Contains("Преформулиране", source);

        // Connector only renders "if (index < Steps.Count - 1)" — no arrow after the last step.
        Assert.Contains("index < Steps.Count - 1", source);
    }

    [Fact]
    public void ResearchTurnStepper_UsesALocalResponsiveLayoutVariant_NotTheSharedCbtDiagramFlexWrap()
    {
        string componentSource = ReadClientComponent("ResearchTurnStepper.razor");
        string cssSource = ReadCss();

        Assert.Contains("research-turn-stepper", componentSource);
        Assert.Contains(".research-turn-stepper .cbt-diagram__steps", cssSource);
        Assert.Contains("grid-template-columns: 1fr auto 1fr auto 1fr auto 1fr", cssSource);

        // CbtModelDiagram's own shared layout must stay untouched by this fix.
        string cbtModelDiagramSource = ReadClientComponent("CbtModelDiagram.razor");
        Assert.DoesNotContain("research-turn-stepper", cbtModelDiagramSource);
    }

    [Fact]
    public void HistoricalTimeline_HasACompactDensityVariant_CourseHubTimelineStaysUntouched()
    {
        string componentSource = ReadHostComponent("HistoricalTimeline.razor");
        string cssSource = ReadCss();
        string week1Source = ReadPage("Sedmica1.razor");
        string kursSource = ReadPage("Kurs.razor");

        Assert.Contains("Compact", componentSource);
        Assert.Contains(".week-timeline--compact", cssSource);
        Assert.Contains("Compact=\"true\"", week1Source);
        Assert.DoesNotContain("Compact", kursSource);
    }

    [Fact]
    public void MainLayout_HasContextualSidebarStateForKursAndProgramaSections()
    {
        string layoutSource = ReadLayoutComponent("MainLayout.razor");
        string cssSource = ReadCss();

        Assert.Contains("IsSectionContext(\"kurs\")", layoutSource);
        Assert.Contains("IsSectionContext(\"programa\")", layoutSource);
        Assert.Contains(".app-sidebar__nav a.is-context", cssSource);

        // Exact routes must never also carry the weak context class on the same element —
        // NavLink's own Match="NavLinkMatch.All" already guarantees exactly 1 strong .active.
        Assert.Contains("Match=\"NavLinkMatch.All\" class=\"@(IsSectionContext(\"kurs\")", layoutSource);
    }

    [Fact]
    public void Week1Page_KnowledgeCheckInstructionIsUpdated_StillNonScoredNativeDetails()
    {
        string source = ReadPage("Sedmica1.razor");

        Assert.Contains("Проверката не се оценява и не запазва отговори", source);
        Assert.DoesNotContain("нерезултатна проверка", source);
    }

    [Fact]
    public void Week1Page_Section09HasThreeDistinctLearnerFacingSubblocks()
    {
        string source = ReadPage("Sedmica1.razor");

        Assert.Contains("<h3>Какво да запомните</h3>", source);
        Assert.Contains("<h3>Академичен контекст</h3>", source);
        Assert.Contains("<h3>Източници и следващи стъпки</h3>", source);
    }

    [Fact]
    public void Week1Page_MakesNoFalseAccreditationOrCreditClaims()
    {
        string source = ReadPage("Sedmica1.razor");

        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит", "Академичен съвет"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week1Page_HasNoDiagnosticOrSelfAssessmentContent()
    {
        string source = ReadPage("Sedmica1.razor");

        string[] forbiddenTerms = ["BDI", "BAI", "диагности", "localStorage", "HttpClient"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week1Page_CrossLinksToKptModul1AndWeek8_NoDeadLinks()
    {
        string source = ReadPage("Sedmica1.razor");

        Assert.Contains("href=\"/kpt\"", source);
        Assert.Contains("href=\"/programa/modul-1\"", source);
        Assert.Contains("href=\"/kurs/sedmica-8\"", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Modul1Overview_CrossLinksToWeek1AsAnExtendedLesson_WithoutDuplicatingLesson1()
    {
        string modul1Source = ReadPage("Modul1.razor");

        Assert.Contains("/kurs/sedmica-1", modul1Source);

        // Modul1Lesson1 remains the module's own required lesson — unchanged in role.
        Assert.Contains("/programa/modul-1/kakvo-e-kpt", modul1Source);
    }

    [Fact]
    public void KursPage_ShowsWeek1AndWeek8AsAvailable_TheRestAsNotAvailable()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("/kurs/sedmica-1", source);
        Assert.Contains("/kurs/sedmica-8", source);
        // Week 3 is covered separately in Week3ContentSliceTests.cs.
    }

    [Fact]
    public void Week1Page_HasSourceReferencesWithThePrimaryAndAcademicCitations()
    {
        string source = ReadPage("Sedmica1.razor");

        Assert.Contains("<SourceReferences", source);
    }

    [Fact]
    public void Week1Page_HasNoPageLevelOverflowWorkaround()
    {
        string source = ReadPage("Sedmica1.razor");

        Assert.DoesNotContain("overflow-x: hidden", source);
        Assert.DoesNotContain("overflow-x:hidden", source);
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    private static string ReadHostComponent(string fileName)
    {
        string sharedDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Shared");
        return File.ReadAllText(Path.Combine(sharedDirectory, fileName));
    }

    private static string ReadClientComponent(string fileName)
    {
        string interactiveDirectory = Path.Combine(
            TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Interactive");
        return File.ReadAllText(Path.Combine(interactiveDirectory, fileName));
    }

    private static string ReadLayoutComponent(string fileName)
    {
        string layoutDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Layout");
        return File.ReadAllText(Path.Combine(layoutDirectory, fileName));
    }

    private static string ReadHostFile(string relativePath)
    {
        string projectDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client");
        return File.ReadAllText(Path.Combine(projectDirectory, relativePath));
    }

    private static string ReadCss()
    {
        string cssPath = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "wwwroot", "app.css");
        return File.ReadAllText(cssPath);
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
