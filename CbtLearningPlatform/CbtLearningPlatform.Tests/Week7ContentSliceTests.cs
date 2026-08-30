using System.Reflection;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>WEEK 7 — DEEP LEARNING MODULE. Source: SRC-041 (Judith Beck), Глава 6 — "Поведенческа
/// активация" — full chapter read from the owner's local PDF, 54 knowledge units, 100% accounted
/// for (see 00_PROJECT_OS/_blueprints/WEEK_07_SOURCE_COVERAGE_AUDIT_v1.md final accounting). Three
/// distinct illustrative patients from the source (Сали — established, extended only with
/// Chapter-6-supported facts; two separate unnamed micro-examples) are kept clearly apart. Zero new
/// components — everything reuses LearningSection/WhatIfBox/ProgressiveExplanation/ConceptGraph and
/// the existing cascade-loop/key-idea-strip CSS patterns.</summary>
public sealed class Week7ContentSliceTests
{
    private static readonly string[] AnchorIds =
    [
        "karta", "znachenie", "porochen-krag", "sali", "postepenni-stapki", "skala",
        "predskazvane", "kredit", "case-lab", "proverki", "assessment", "review-map", "izvori"
    ];

    [Fact]
    public void Week7Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica7"));
    }

    [Fact]
    public void Week7_IsNowRoutedAndAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 7);

        Assert.Equal("/kurs/sedmica-7", week.Route);
        Assert.Equal(CourseWeekStatus.Available, week.Status);
    }

    [Fact]
    public void Week1Week3Week6Week8Week10_RemainAvailableAfterWeek7Routing()
    {
        int[] weeksToCheck = [1, 3, 6, 8, 10];

        foreach (int number in weeksToCheck)
        {
            CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == number);
            Assert.Equal(CourseWeekStatus.Available, week.Status);
            Assert.Equal($"/kurs/sedmica-{number}", week.Route);
        }
    }

    [Fact]
    public void Week7Page_HasPageTitleAndDeepLearningBadge()
    {
        string source = ReadPage("Sedmica7.razor");

        Assert.Contains("<PageTitle>Седмица 7: Поведенческа активация", source);
        Assert.Contains("Дълбочинен модул", source);
    }

    [Fact]
    public void Week7Page_HasAllThirteenSections()
    {
        string source = ReadPage("Sedmica7.razor");

        foreach (string id in AnchorIds)
        {
            Assert.Contains($"id=\"{id}\"", source);
        }

        string[] sectionNumbers = ["7.0", "7.1", "7.2", "7.3", "7.4", "7.5", "7.6", "7.7", "7.8", "7.9", "7.10", "7.11", "7.12"];
        foreach (string number in sectionNumbers)
        {
            Assert.Contains($"{number} ", source);
        }
    }

    [Fact]
    public void Week7Page_SectionNavAnchorsAreRouteSafe()
    {
        string source = ReadPage("Sedmica7.razor");

        foreach (string id in AnchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-7#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
        }
    }

    [Fact]
    public void Week7Page_UsesEstablishedReusablePatterns_ZeroNewComponents()
    {
        string source = ReadPage("Sedmica7.razor");

        Assert.Contains("<LearningSection", source);
        Assert.Contains("<LearningObjectives", source);
        Assert.Contains("<ProgressiveExplanation", source);
        Assert.Contains("<DisclaimerCallout", source);
        Assert.Contains("<SourceReferences", source);
        Assert.Contains("<OptionalReadingSource", source);
        Assert.Contains("<WhatIfBox", source);
        Assert.Contains("<ConceptGraph", source);
        Assert.Contains("<WeekCompletionControl WeekNumber=\"@_week.Number\" />", source);

        // No new component types — Week 7 deliberately reuses only what already exists.
        Assert.DoesNotContain("<ScenarioSimulator", source);
        Assert.DoesNotContain("<SourceArtifact", source);
        Assert.DoesNotContain("<CbtChainSimulator", source);
    }

    [Fact]
    public void Week7Page_HasTheViciousCycleLoopDiagram()
    {
        string source = ReadPage("Sedmica7.razor");

        Assert.Contains("class=\"cascade-loop\"", source);
        Assert.Contains("Бездействие", source);
        Assert.Contains("По-ниско настроение", source);
        Assert.Contains("По-негативно мислене", source);
        Assert.Contains("Още по-силно бездействие", source);
    }

    [Fact]
    public void Week7Page_DistinguishesTheTwoTypesOfInterferingThoughts()
    {
        string source = ReadPage("Sedmica7.razor");

        Assert.Contains("Твърде съм уморен", source);
        Assert.Contains("Няма да ми е приятно", source);
        Assert.Contains("Не мога да го правя толкова добре, колкото преди", source);
    }

    [Fact]
    public void Week7Page_KeepsTheThreeIllustrativePatientsDistinct()
    {
        string source = ReadPage("Sedmica7.razor");

        // 1. Сали — named, established, extended only with Chapter-6-supported facts.
        Assert.Contains("Случаят на Сали", source);
        Assert.Contains("Алисън и Джо", source);

        // 2. Unnamed resistant patient — "wait until I feel better".
        Assert.Contains("трябва да изчакам, докато се почувствам по-добре", source);

        // 3. Unnamed patient — Pleasure/Mastery scale + both predict-vs-actual worked examples.
        Assert.Contains("мача за шампионата", source);
        Assert.Contains("Сценарий 1: среща с приятели", source);
        Assert.Contains("Сценарий 2: тичане през уикенда", source);

        // Never merged into one composite biography or given invented names.
        Assert.DoesNotContain("Сали отиде на мача", source);
        Assert.DoesNotContain("Сали изчака, докато", source);
    }

    [Fact]
    public void Week7Page_Figure63DataIntegrityFix_UsesOnlyTheDialogueVerifiedAnchors()
    {
        // Visual PDF inspection found the printed Figure 6.3 table's Pleasure-column row order
        // directly contradicts the chapter's own continuous, cross-page dialogue. The dialogue
        // (10=football match, 5=dinner with brother, 0=arguing with partner) is used; the
        // contradicted figure-table order and the unverified Mastery-column labels are not.
        string source = ReadPage("Sedmica7.razor");

        Assert.Contains("футболен мач", source);
        Assert.Contains("вечеря с брат", source);
        Assert.Contains("спор с партньора", source);

        // The Mastery-column figure labels have no corroborating dialogue — deliberately excluded.
        Assert.DoesNotContain("Строене на палубата", source);
        Assert.DoesNotContain("Събиране на листа", source);
        Assert.DoesNotContain("Чек с недостатъчно покритие", source);
    }

    [Fact]
    public void Week7Page_PredictRevealCompare_HasBothConfirmedNumericOutcomes()
    {
        string source = ReadPage("Sedmica7.razor");

        // Friends scenario: predicted worse than reality.
        Assert.Contains("0 и 3", source);
        Assert.Contains("Действителни оценки: 3 до 5", source);

        // Weekend-run scenario: predicted better than reality.
        Assert.Contains("по 4 за овладяване", source);
        Assert.Contains("Действителни оценки: по 1", source);
    }

    [Fact]
    public void Week7Page_HasTheSaliCaseMap()
    {
        string source = ReadPage("Sedmica7.razor");

        Assert.Contains("ComponentId=\"week7-sali-map\"", source);
        Assert.Contains("BuildSaliWeek7Map", source);

        // Cross-references Week 3's existing belief hierarchy instead of duplicating it.
        Assert.Contains("/kurs/sedmica-3#sali-hierarhia", source);
    }

    [Fact]
    public void Week7Page_SaliCaseMap_UsesNetworkLayout_NotTheNarrowChainColumn()
    {
        // Desktop visual fix: the default 2-argument ConceptMapAdapter.ToRenderModel call defaults to
        // Chain layout, which renders as a full-width stacked vertical list — the reported defect.
        // Network layout (same engine as Week 3's Sali hierarchy / the global Knowledge Map) must be
        // passed explicitly instead.
        string source = ReadPage("Sedmica7.razor");

        Assert.Contains("ConceptMapAdapter.ToRenderModel(", source);
        Assert.Contains("BuildSaliWeek7Map(), CourseCatalog.Weeks, ConceptGraphLayout.Network, SaliWeek7NetworkLayout", source);
    }

    [Fact]
    public void Week7Page_SaliCaseMap_FollowsTheRequestedLeftToRightReadingOrder()
    {
        string source = ReadPage("Sedmica7.razor");

        // Ситуация(0) → Автоматична мисъл(1) → Емоция(2) → Вероятно поведение(3) — the primary
        // left-to-right row. "Поведенчески експеримент" shares column 3 (row 1): it is an alternative
        // outcome from the SAME source node (thought) as "Вероятно поведение", not sequential to it, and
        // a straight 5th column would force horizontal scroll on every measured viewport (see the code
        // comment for the width math) — sharing the column keeps the map scroll-free while the edge from
        // thought still draws its own clearly-labeled connector.
        Assert.Contains("[\"sali-w7-situation\"] = new(\"\", 0, 0)", source);
        Assert.Contains("[\"sali-w7-thought\"] = new(\"\", 1, 0)", source);
        Assert.Contains("[\"sali-w7-emotion\"] = new(\"\", 2, 0)", source);
        Assert.Contains("[\"sali-w7-behavior\"] = new(\"\", 3, 0)", source);
        Assert.Contains("[\"sali-w7-experiment\"] = new(\"\", 3, 1)", source);

        // The Week 3 cross-reference sits one row below its real source (thought), not in the main row.
        Assert.Contains("[\"sali-w7-beliefs\"] = new(\"\", 1, 1)", source);
    }

    [Fact]
    public void Week7SaliCaseMap_EveryNodeHasANetworkPosition_NoOrphans()
    {
        string source = ReadPage("Sedmica7.razor");
        int dictStart = source.IndexOf("SaliWeek7NetworkLayout = new Dictionary", StringComparison.Ordinal);
        int dictEnd = source.IndexOf("};", dictStart, StringComparison.Ordinal);
        string dictBody = source[dictStart..dictEnd];

        string[] nodeIds =
        [
            "sali-w7-situation", "sali-w7-thought", "sali-w7-emotion",
            "sali-w7-behavior", "sali-w7-experiment", "sali-w7-beliefs"
        ];

        foreach (string nodeId in nodeIds)
        {
            Assert.Contains($"[\"{nodeId}\"]", dictBody);
        }
    }

    [Fact]
    public void Week7Page_FinalAssessmentHasSixteenQuestions()
    {
        string source = ReadPage("Sedmica7.razor");

        for (int i = 1; i <= 16; i++)
        {
            Assert.Contains($"Q{i:D2}", source);
        }
    }

    [Fact]
    public void Week7Page_AssessmentQuestionsHaveSourceCitations()
    {
        string source = ReadPage("Sedmica7.razor");

        int sourceCitationCount = CountOccurrences(source, "Source: U");
        Assert.True(sourceCitationCount >= 14, $"Expected at least 14 source-cited assessment answers, found {sourceCitationCount}.");
    }

    [Fact]
    public void Week7Page_HasFourLocalKnowledgeChecks()
    {
        string source = ReadPage("Sedmica7.razor");

        for (int i = 1; i <= 4; i++)
        {
            Assert.Contains($"Проверка {i}", source);
        }
    }

    [Fact]
    public void Week7Page_HasNoSelfAssessmentInputOrScoring()
    {
        string source = ReadPage("Sedmica7.razor");

        Assert.DoesNotContain("<input", source);
        Assert.DoesNotContain("твоят резултат", source);
        Assert.DoesNotContain("вашият резултат", source);
    }

    [Fact]
    public void Week7Page_DistinguishesOverviewFromSelfTherapyInstruction()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica7.razor");

        Assert.Contains("не заменя професионална психологическа или медицинска помощ", publicMarkup);
        Assert.Contains("не обучава за самостоятелно провеждане на терапия", publicMarkup);
    }

    [Fact]
    public void Week7Page_ReviewMapHasCrossWeekConnections()
    {
        string source = ReadPage("Sedmica7.razor");

        Assert.Contains("/kurs/sedmica-3", source);
        Assert.Contains("/kurs/sedmica-6", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Week7Page_DoesNotLinkToWeekFiveAsIfAvailable()
    {
        string source = ReadPage("Sedmica7.razor");

        Assert.DoesNotContain("/kurs/sedmica-5", source);
    }

    [Fact]
    public void Week7Page_HasNoInternalDevelopmentLanguageInItsRenderableMarkup()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica7.razor");

        string[] forbiddenTerms =
        [
            "11_SOURCE_REGISTER.md", "kpt_syllabus.pdf", "citation-grade",
            "ACADEMIC/CLINICAL REVIEW PENDING", "10_SESSION_LOG.md", "Project OS",
            "code_artifact.html", "14_EXISTING_PROTOTYPE_AUDIT.md", "_source_corpus", "_blueprints"
        ];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }
    }

    [Fact]
    public void Week7Page_MakesNoFalseAccreditationClaims()
    {
        string source = ReadPage("Sedmica7.razor");

        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week7Page_HasNoInlineStylesOrAbsolutePositioning()
    {
        string source = ReadPage("Sedmica7.razor");

        Assert.DoesNotContain("style=", source);
        Assert.DoesNotContain("position: absolute", source);
        Assert.DoesNotContain("position:absolute", source);
    }

    [Fact]
    public void Week7Page_HasPreviewMindMap()
    {
        string source = ReadPage("Sedmica7.razor");

        Assert.Contains("ComponentId=\"week7-mindmap-preview\"", source);
    }

    [Fact]
    public void Week7Page_HasReviewMindMap_GatedByAttemptBeforeReveal()
    {
        string source = ReadPage("Sedmica7.razor");

        Assert.Contains("ComponentId=\"week7-mindmap-review\"", source);

        int summaryIndex = source.IndexOf("опитай да си спомниш, преди да разгънеш", StringComparison.OrdinalIgnoreCase);
        int mapIndex = source.IndexOf("ComponentId=\"week7-mindmap-review\"", StringComparison.Ordinal);

        Assert.True(summaryIndex >= 0 && summaryIndex < mapIndex,
            "Review Mind Map must be preceded by an attempt-before-reveal prompt.");
    }

    [Fact]
    public void Week7Page_PreviewAndReviewMindMap_ShareTheSameUnderlyingModel()
    {
        string source = ReadPage("Sedmica7.razor");

        int bindingCount = CountOccurrences(source, "Model=\"@_week7MindMapRender\"");

        Assert.Equal(2, bindingCount);
    }

    [Fact]
    public void Week7Page_MindMapHasSevenTopLevelClusters_NotForcedToFive()
    {
        // Representation Fit found 7 genuine top-level clusters for this chapter — the audit was
        // explicitly told not to force exactly five if the source didn't support it.
        string source = ReadPage("Sedmica7.razor");

        string[] clusterIds =
        [
            "\"znachenie\"", "\"porochen-krag\"", "\"sali\"", "\"postepenni-stapki\"",
            "\"skala\"", "\"predskazvane\"", "\"kredit\""
        ];

        foreach (string clusterId in clusterIds)
        {
            Assert.Contains(clusterId, source);
        }
    }

    [Fact]
    public void Week7MindMap_ProducesAValidHierarchy_NoDanglingReferencesOrCycles()
    {
        // MindMapAdapter throws on a dangling ParentId or a cycle — building the render model here
        // alone would fail if the page's own static data were malformed.
        string source = ReadPage("Sedmica7.razor");

        Assert.Contains("MindMapAdapter.ToRenderModel(BuildWeek7MindMap())", source);
    }

    [Fact]
    public void KursPage_ListsWeekSevenAsAvailable()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("/kurs/sedmica-7", source);
        Assert.DoesNotContain("Пет седмици", source);
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
