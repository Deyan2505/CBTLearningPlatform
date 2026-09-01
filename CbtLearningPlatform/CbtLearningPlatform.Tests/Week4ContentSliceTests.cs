using System.Reflection;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>WEEK_04_SOURCE_AUDIT_v1 (owner-approved) — "Клинична оценка и когнитивна
/// концептуализация," build-from-scratch, sole source SRC-041 Ch.4 ("Сесия по оценка").
/// CurriculumSafetyLevel.AcademicContextOnly — routed but AcademicOverview, not Available, same
/// pattern as Week 12. Three safety-sensitive KUs were resolved by explicit owner decision:
/// RISK1 (suicide/self-harm risk — one high-level sentence, no procedure), TECH3 (excluded
/// outright), DIAG1 (DSM — one general sentence, no criteria). No reproduced therapist–patient
/// dialogue anywhere (Chapter 4's own transcripts are all paraphrased away or omitted).</summary>
public sealed class Week4ContentSliceTests
{
    [Fact]
    public void Week4Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica4"));
    }

    [Fact]
    public void Week4_MetadataIsRoutedButAcademicOverviewNotAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 4);

        Assert.Equal("/kurs/sedmica-4", week.Route);
        Assert.Equal(CourseWeekStatus.AcademicOverview, week.Status);
        Assert.NotEqual(CourseWeekStatus.Available, week.Status);
    }

    [Fact]
    public void Week1Week2Week3_RemainAvailableAfterWeek4WasAdded()
    {
        int[] weeksToCheck = [1, 2, 3];

        foreach (int number in weeksToCheck)
        {
            CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == number);
            Assert.Equal(CourseWeekStatus.Available, week.Status);
            Assert.Equal($"/kurs/sedmica-{number}", week.Route);
        }
    }

    [Fact]
    public void Week4Page_HasPageTitleAndLearningObjectives()
    {
        string source = ReadPage("Sedmica4.razor");

        Assert.Contains("<PageTitle>Седмица 4: Клинична оценка и когнитивна концептуализация", source);
        Assert.Contains("<LearningObjectives", source);
        Assert.Contains("Академичен обзор", source);
    }

    [Fact]
    public void Week4Page_UsesOnlyExistingReusablePatterns_NoNewComponent()
    {
        string source = ReadPage("Sedmica4.razor");

        Assert.Contains("<LearningSection", source);
        Assert.Contains("<LearningObjectives", source);
        Assert.Contains("<ProgressiveExplanation", source);
        Assert.Contains("<DisclaimerCallout", source);
        Assert.Contains("Variant=\"safety\"", source);
        Assert.Contains("<SourceReferences", source);
        Assert.Contains("<OptionalReadingSource", source);
        Assert.Contains("<WeekCompletionControl", source);

        string[] forbiddenNewComponents =
        [
            "<CbtChainSimulator", "<CategorizationCheck", "<InterpretationExample",
            "<ResearchTurnStepper", "<SocraticDialogueExplorer", "<SchemaFilterDemonstration",
            "<ConceptGraph", "<HistoricalTimeline", "<MindMapBranch"
        ];
        foreach (string component in forbiddenNewComponents)
        {
            Assert.DoesNotContain(component, source);
        }
    }

    [Fact]
    public void Week4Page_HasNoSelfInputFieldsOrForms()
    {
        string source = ReadPage("Sedmica4.razor");

        Assert.DoesNotContain("<input", source);
        Assert.DoesNotContain("<form", source);
        Assert.DoesNotContain("<textarea", source);
    }

    [Fact]
    public void Week4Page_HasNoSelfDiagnosticOrSelfAssessmentForm()
    {
        string source = ReadPage("Sedmica4.razor");

        Assert.DoesNotContain("BDI", source);
        Assert.DoesNotContain("BAI", source);
        Assert.DoesNotContain("localStorage", source);
        Assert.DoesNotContain("HttpClient", source);

        Assert.Contains("категории, които специалист обхожда систематично", source);
        Assert.Contains("не формуляр за прилагане върху себе си или друг", source);
    }

    [Fact]
    public void Week4Page_HasNoSuicideRiskProcedure()
    {
        string source = ReadPage("Sedmica4.razor");

        // The one allowed sentence acknowledges the topic exists and names it as a trained
        // professional's responsibility — but teaches no scale, scoring, or decision procedure.
        Assert.Contains("риск от самонараняване или суицид", source);
        Assert.Contains("отговорност изключително на обучен специалист", source);
        Assert.Contains("не преподава скринингови въпроси, скали или процедура", source);

        string[] forbiddenProcedureMarkers =
        [
            "Columbia", "C-SSRS", "скала за суициден риск", "стъпка 1", "точки от", "резултат ≥"
        ];
        foreach (string marker in forbiddenProcedureMarkers)
        {
            Assert.DoesNotContain(marker, source);
        }
    }

    [Fact]
    public void Week4Page_HasNoDsmDiagnosticCriteria()
    {
        string source = ReadPage("Sedmica4.razor");

        // DSM is named exactly once, as a framework qualified professionals may use — never with
        // reproduced criteria, a symptom checklist, or self-diagnosis framing.
        Assert.Contains("DSM", source);
        Assert.Contains("Тази страница не представя", source);
        Assert.Contains("диагностични критерии, списък със симптоми или начин за самодиагностика.", source);

        string[] forbiddenCriteriaMarkers = ["критерий A", "критерий Б", "DSM-5-TR", "296.", "F32", "F33"];
        foreach (string marker in forbiddenCriteriaMarkers)
        {
            Assert.DoesNotContain(marker, source);
        }
    }

    [Fact]
    public void Week4Page_HasNoReproducedTherapistPatientDialogue()
    {
        string source = ReadPage("Sedmica4.razor");

        // Chapter 4's own transcripts always label turns this way — their absence is the direct
        // signal that no dialogue was reproduced (same boundary Week 12 already established).
        Assert.DoesNotContain("Терапевт:", source);
        Assert.DoesNotContain("Пациент:", source);
        Assert.DoesNotContain("паТиент:", source);
    }

    [Fact]
    public void Week4Page_CitesSrc041Chapter4ByNameAndLocator()
    {
        string source = ReadPage("Sedmica4.razor");

        Assert.Contains("Джудит С. Бек", source);
        Assert.Contains("Глава 4", source);
        Assert.Contains("Сесия по оценка", source);
        Assert.Contains("46", source);
        Assert.Contains("58", source);
    }

    [Fact]
    public void Week4Page_HasTheWeek3ConceptualizationBridge()
    {
        string source = ReadPage("Sedmica4.razor");

        Assert.Contains("href=\"/kurs/sedmica-3\"", source);
        Assert.Contains("начална когнитивна концептуализация", source);
        Assert.Contains("Какви са основните вярвания на пациента?", source);
        Assert.Contains("Сали", source);

        // Never re-renders Week 3's own diagram markup.
        Assert.DoesNotContain("concept-map__flow", source);
    }

    [Fact]
    public void Week4Page_SectionNavAnchorsAreRouteSafe()
    {
        string source = ReadPage("Sedmica4.razor");

        string[] anchorIds =
        [
            "nakratko", "zashto-otsenka", "podgotovka", "struktura-na-sesiyata",
            "oblasti-na-otsenkata", "ezhednevie-na-patsienta", "tseli-i-plan",
            "ot-otsenka-kam-kontseptualizatsia", "proverka", "izvori"
        ];

        foreach (string id in anchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-4#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
            Assert.Contains($"id=\"{id}\"", source);
        }
    }

    [Fact]
    public void Week4Page_CrossLinksToWeek3_NoDeadLinks()
    {
        string source = ReadPage("Sedmica4.razor");

        Assert.Contains("href=\"/kurs/sedmica-3\"", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Week4Page_MakesNoFalseAccreditationClaims()
    {
        string source = ReadPage("Sedmica4.razor");

        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит"];
        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week4Page_HasFiveComprehensionQuestions()
    {
        string source = ReadPage("Sedmica4.razor");

        for (int i = 1; i <= 5; i++)
        {
            Assert.Contains($"Въпрос {i}.", source);
        }
        Assert.DoesNotContain("Въпрос 6.", source);
    }

    [Fact]
    public void Week4Page_HasNoInlineStylesOrOverflowWorkaround()
    {
        string source = ReadPage("Sedmica4.razor");

        Assert.DoesNotContain("style=", source);
        Assert.DoesNotContain("overflow-x: hidden", source);
        Assert.DoesNotContain("overflow-x:hidden", source);
    }

    [Fact]
    public void KursPage_ListsWeek4ButNotInTheStartPanel()
    {
        // Week 4 is AcademicOverview, not Available — it must not be added to the start-panel
        // alongside Weeks 1, 2, 3, 6, 7, 8, 9, 10 (same treatment as Week 12).
        string source = ReadPage("Kurs.razor");

        int startPanelStart = source.IndexOf("start-panel", StringComparison.Ordinal);
        int startPanelEnd = source.IndexOf("</div>", startPanelStart, StringComparison.Ordinal);
        Assert.DoesNotContain("/kurs/sedmica-4", source[startPanelStart..startPanelEnd]);
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }
}
