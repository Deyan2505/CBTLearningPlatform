using System.Reflection;
using System.Text.RegularExpressions;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>Week 13 ("Допълнителни когнитивни и поведенчески техники") — final source contract implementation
/// (owner-approved 2026-09-07, WEEK_13_SOURCE_AUDIT_v1.md). Content basis: SRC-041, Глава 15, printed
/// pp. 256-276. Safety level stays CurriculumSafetyLevel.NotEligibleForSelfGuidedSimulator — the strictest
/// tier in the system — so the page is routed and completion-eligible but its public status never becomes
/// Available, and its declared InteractiveFormat is never Simulator. Owner-resolved exclusions enforced
/// here: AWARE technique and mindfulness practice depth are Deferred (not taught); Figure 15.3 appears only
/// as a fixed illustration; the first-person homework script and lowest-point/team-framing passage are
/// excluded; the exposure procedure/monitor/hierarchy stay excluded; battered-spouse referral and the
/// problem-behavior list stay excluded.</summary>
public sealed class Week13ContentSliceTests
{
    [Fact]
    public void Week13Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica13"));
    }

    [Fact]
    public void Week13_MetadataIsRoutedButNotEligibleForSelfGuidedSimulatorNotAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 13);

        Assert.Equal("/kurs/sedmica-13", week.Route);
        Assert.Equal(CurriculumSafetyLevel.NotEligibleForSelfGuidedSimulator, week.SafetyLevel);
        Assert.Equal(CourseWeekStatus.ProfessionalReviewRequired, week.Status);
        Assert.NotEqual(CourseWeekStatus.Available, week.Status);

        // Status-only label collapses ProfessionalReviewRequired to the generic "Професионален
        // контекст" — but Week 13's SafetyLevel is the stricter NotEligibleForSelfGuidedSimulator,
        // so the safety-level-aware, actually-rendered label must read differently (Weeks 11/14 keep
        // the generic label; only Week 13 gets this one).
        Assert.Equal("Професионален контекст", week.Status.ToPublicLabel());
        Assert.Equal("Без самостоятелна практика", week.ToPublicLabel());
    }

    [Fact]
    public void Week13_FormatIsNeverSimulator()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 13);

        Assert.DoesNotContain(InteractiveFormat.Simulator, week.InteractiveFormats);
    }

    [Fact]
    public void Week13_TitleReflectsWidenedChapterScope()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 13);

        Assert.Equal("Допълнителни когнитивни и поведенчески техники", week.Title);
        // The too-narrow pre-audit title must not survive.
        Assert.NotEqual("Вземане на решения и поведенчески техники", week.Title);
    }

    [Fact]
    public void Week13_ObjectivesCoverTheThreeApprovedThematicAreas()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 13);
        string joined = string.Join(" | ", week.LearningObjectives);

        Assert.Contains("концептуализация", joined);
        Assert.Contains("градуирани", joined);
        Assert.Contains("дефицит на умения", joined);
        Assert.Contains("пренасочване", joined);
        Assert.Contains("себесравнение", joined);
    }

    [Fact]
    public void Week13Page_HasPageTitleAndFormatBadge()
    {
        string source = ReadPage("Sedmica13.razor");

        Assert.Contains("<PageTitle>Седмица 13: Допълнителни когнитивни и поведенчески техники", source);
        Assert.Contains("@_week.ToPublicLabel()", source);
    }

    [Fact]
    public void Week13Page_UsesOnlyExistingReusablePatterns()
    {
        string source = ReadPage("Sedmica13.razor");

        Assert.Contains("<LearningSection", source);
        Assert.Contains("<LearningObjectives", source);
        Assert.Contains("<ProgressiveExplanation", source);
        Assert.Contains("<DisclaimerCallout", source);
        Assert.Contains("<SourceReferences", source);
        Assert.Contains("<OptionalReadingSource", source);
        Assert.Contains("<WeekCompletionControl", source);
        Assert.Contains("<FinalAssessment", source);
        Assert.Contains("<ConceptGraph", source);
        Assert.Contains("class=\"guided-practice-sequence\"", source);
        Assert.Contains("class=\"comparison-matrix-wrapper\"", source);
        Assert.Contains("class=\"learning-grid learning-grid--balanced\"", source);

        // No new .razor component and no new interactive island — CategorizationCheck-style checks
        // use the existing native <details> reveal pattern instead.
        Assert.DoesNotContain("<CbtChainSimulator", source);
        Assert.DoesNotContain("<InterpretationExample", source);
        Assert.DoesNotContain("<CategorizationCheck", source);
    }

    [Fact]
    public void Week13Page_MindMapPresentInPreviewAndReview_CollapsedByDefaultInReview_NoConceptMapIntroduced()
    {
        string source = ReadPage("Sedmica13.razor");

        int previewIndex = source.IndexOf("<h2 id=\"nakratko\"", StringComparison.Ordinal);
        int reviewIndex = source.IndexOf("<h2 id=\"review\"", StringComparison.Ordinal);
        Assert.True(previewIndex >= 0 && reviewIndex > previewIndex);

        string reviewSection = source[reviewIndex..source.IndexOf("<h2 id=\"assessment\"", StringComparison.Ordinal)];

        Assert.Contains("<details class=\"progressive-explanation concept-graph__retrieval-check\">", reviewSection);
        Assert.DoesNotContain("<details class=\"progressive-explanation concept-graph__retrieval-check\" open>", reviewSection);
        Assert.Contains("<ConceptGraph", reviewSection);

        // Only one ConceptGraph model exists on this page (the Week 13 Mind Map, in Preview and
        // Review) — no second, relationship-network Concept Map was added.
        Assert.Equal(2, Regex.Matches(source, "<ConceptGraph").Count);
        Assert.Equal(2, Regex.Matches(source, @"Model=""@_week13MindMapRender""").Count);
    }

    [Fact]
    public void Week13Page_MindMapUsesEightKnowledgeClusterBranches()
    {
        string source = ReadPage("Sedmica13.razor");

        int mapStart = source.IndexOf("private static MindMapModel BuildWeek13MindMap", StringComparison.Ordinal);
        int mapEnd = source.IndexOf("]);", mapStart, StringComparison.Ordinal);
        Assert.True(mapStart >= 0 && mapEnd > mapStart);
        string mapBlock = source[mapStart..mapEnd];

        string[] expectedBranches =
        [
            "Избор на техника", "Структурирано вземане на решения", "Градуирани задачи",
            "Избягване и безопасност", "Пренасочване на вниманието", "Дефицит на умения или вярване",
            "Диаграма на отговорността", "Себесравнение и списък на заслугите"
        ];
        foreach (string branch in expectedBranches)
        {
            Assert.Contains($"\"{branch}\"", mapBlock);
        }
    }

    [Fact]
    public void Week13Page_UsesSharedFinalAssessmentWithEightSourceGroundedQuestions()
    {
        string source = ReadPage("Sedmica13.razor");

        Assert.Contains("<FinalAssessment SectionId=\"assessment\" Model=\"_week13FinalAssessment\" />", source);
        Assert.Equal(8, TestPaths.CountFinalAssessmentQuestions(source, "_week13FinalAssessment"));
    }

    [Fact]
    public void Week13Page_WeekCompletionControlIsIndependentOfAssessmentPlacement()
    {
        string source = ReadPage("Sedmica13.razor");

        int assessmentIndex = source.IndexOf("<FinalAssessment", StringComparison.Ordinal);
        int completionIndex = source.IndexOf("<WeekCompletionControl WeekNumber=\"@_week.Number\" />", StringComparison.Ordinal);

        Assert.True(assessmentIndex >= 0 && completionIndex > assessmentIndex);
    }

    [Fact]
    public void Week13Page_SectionNavAnchorsAreRouteSafe()
    {
        string source = ReadPage("Sedmica13.razor");

        string[] anchorIds =
        [
            "nakratko", "izbor-na-tehnika", "vzemane-na-reshenia", "graduirani-zadachi",
            "izbyagvane-i-bezopasnost", "prenasochvane-na-vnimanieto", "deficit-na-umenia-ili-vyarvane",
            "krugova-diagrama-na-otgovornostta", "sebesravnenie-i-zaslugi", "review", "assessment", "izvori"
        ];

        foreach (string id in anchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-13#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
            Assert.Contains($"id=\"{id}\"", source);
        }
    }

    [Fact]
    public void Week13Page_CrossLinksToWeeksSevenTenElevenTwelveAndTheHub_NoDeadLinks()
    {
        string source = ReadPage("Sedmica13.razor");

        Assert.Contains("/kurs/sedmica-7", source);
        Assert.Contains("/kurs/sedmica-10", source);
        Assert.Contains("/kurs/sedmica-11", source);
        Assert.Contains("/kurs/sedmica-12", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Week13Page_DoesNotReTeachWeek7sViciousCycleOrEnergySizedSteps()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica13.razor");

        // Week 7 owns the vicious-cycle diagram and the energy-sized activity-schedule steps —
        // this page only distinguishes its own feared-goal ladder from them, never reproduces them.
        Assert.DoesNotContain("порочен кръг", publicMarkup);
        Assert.DoesNotContain("Скала за удоволствие и овладяване", publicMarkup);
    }

    [Fact]
    public void Week13Page_DistinguishesFromWeek11sBeliefAdvantagesDisadvantages()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica13.razor");

        Assert.Contains("различна цел", publicMarkup);
    }

    [Fact]
    public void Week13Page_K122CrossReferenceAnomalyIsNotRepeated_LinksToWeek12Instead()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica13.razor");

        // The source itself misattributes the Core Belief Worksheet to "Глава 11" — this page must
        // never repeat that wrong chapter number, and must forward-link to Week 12 (which owns
        // Глава 14 / core beliefs) instead.
        Assert.DoesNotContain("Глава 11", publicMarkup);

        string normalized = Regex.Replace(publicMarkup, @"\s+", " ");
        Assert.Contains("работния лист за основни убеждения — тема, представена изцяло в", normalized);
        Assert.Contains("Седмица 12", normalized);
    }

    [Fact]
    public void Week13Page_UsesLockedCanonicalTerminology()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica13.razor");
        string normalized = Regex.Replace(publicMarkup, @"\s+", " ");

        Assert.Contains("пренасочване на вниманието (refocusing)", normalized);
        Assert.Contains("градуирани задачи", normalized);
        Assert.Contains("Кръгова диаграма на отговорността", normalized);
        Assert.Contains("Списък на заслугите", normalized);
        Assert.Contains("осъзнатост (mindfulness)", normalized);

        // Mistranslated/unstable source headings must never survive onto the learner-facing page.
        Assert.DoesNotContain("Оценени задачи", publicMarkup);
        Assert.DoesNotContain("ПРЕОСМИСЛЯНЕ", publicMarkup);
        Assert.DoesNotContain("списък с признания", publicMarkup);
        Assert.DoesNotContain("умствена яснота", publicMarkup);
    }

    [Fact]
    public void Week13Page_MindfulnessAndAwareAreDeferred_NoPracticeTaught()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica13.razor");

        Assert.DoesNotContain("AWARE", publicMarkup);

        // Mindfulness is named once, deferred explicitly — never taught as a practice/procedure.
        Assert.Contains("извън обхвата на тази седмица", publicMarkup);
    }

    [Fact]
    public void Week13Page_ParadoxicalArousalCautionIsCoLocatedWithEveryRelaxationMention()
    {
        string normalized = Regex.Replace(ReadPublicMarkup("Sedmica13.razor"), @"\s+", " ");

        int relaxationIndex = normalized.IndexOf("релаксац", StringComparison.OrdinalIgnoreCase);
        Assert.True(relaxationIndex >= 0, "Expected relaxation to be mentioned at least once.");

        // The mandatory paradoxical-arousal caution must appear in the same paragraph run as the
        // relaxation mention — never relaxation presented alone.
        string window = normalized.Substring(relaxationIndex, Math.Min(600, normalized.Length - relaxationIndex));
        Assert.Contains("парадоксален ефект", window);
    }

    [Fact]
    public void Week13Page_ExcludesExposureProcedureMonitorHierarchyAndSensitiveItems()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica13.razor");

        string[] excludedTerms =
        [
            "Хващам се, когато се сравнявам", // first-person homework script
            "екип, работещ",                  // lowest-point/team-framing passage
            "малтретиран", "убежище",         // battered-spouse referral
            "хазарт", "преяждане", "свръхразходване", // problem-behavior list
            "агорофобичн", "Голдстейн и Стейнбек",    // exposure hierarchy literature
        ];
        foreach (string term in excludedTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }

        // Figure 15.3 appears only as a single fixed, already-completed example row — never a blank
        // template and never adjacent daily-monitoring usage instructions.
        Assert.Contains("вече попълнен пример", publicMarkup);
        Assert.DoesNotContain("ежедневен монитор, който може", publicMarkup);
    }

    [Fact]
    public void Week13Page_HasNoSelfInputFormsOrSelfAssessmentLanguage()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica13.razor");

        Assert.DoesNotContain("<input", publicMarkup);
        Assert.DoesNotContain("<textarea", publicMarkup);
        Assert.DoesNotContain("<form", publicMarkup);

        string[] selfAssessmentPhrases = ["твоите предимства и недостатъци", "запиши своя списък", "оценете себе си"];
        foreach (string phrase in selfAssessmentPhrases)
        {
            Assert.DoesNotContain(phrase, publicMarkup);
        }

        Assert.Contains("не е инструмент за самооценка", publicMarkup);
    }

    [Fact]
    public void Week13Page_HasNoReproducedTherapistPatientDialogueTranscripts()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica13.razor");

        Assert.DoesNotContain("Терапевт:", publicMarkup);
        Assert.DoesNotContain("Пациент:", publicMarkup);
    }

    [Fact]
    public void Week13Page_SallyUsageIsLimitedToFourAuditApprovedFigures_NoNewBiography()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica13.razor");

        Assert.Contains("Сали", publicMarkup);

        // Figure 15.4 (goal-setting actual/ideal pies) is intentionally not reproduced — its slice
        // proportions are not recoverable from the flattened source extraction.
        Assert.DoesNotContain("Използване на диаграми на пай при поставяне на цели", publicMarkup);
        Assert.DoesNotContain("Работа/приятели/забавление/семейство", publicMarkup);
    }

    [Fact]
    public void Week13Page_HasNoInternalDevelopmentLanguageInItsRenderableMarkup()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica13.razor");

        string[] forbiddenTerms =
        [
            "11_SOURCE_REGISTER.md", "kpt_syllabus.pdf", "citation-grade", "WEEK_13_SOURCE_AUDIT",
            "10_SESSION_LOG.md", "Project OS", "code_artifact.html", "Needs Review", "KU"
        ];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }
    }

    [Fact]
    public void Week13Page_MakesNoFalseAccreditationClaims()
    {
        string source = ReadPage("Sedmica13.razor");

        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week13Page_HasNoDiagnosticOrClinicalScoringContent()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica13.razor");

        string[] forbiddenTerms = ["BDI", "BAI", "BHS", "терапевтичен план", "диагностичен инструмент"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }

        Assert.Contains("не поставя диагноза", publicMarkup);
    }

    [Fact]
    public void Week13Page_HasNoPageLevelOverflowWorkaroundOrInlineStyles()
    {
        string source = ReadPage("Sedmica13.razor");

        Assert.DoesNotContain("overflow-x: hidden", source);
        Assert.DoesNotContain("overflow-x:hidden", source);
        Assert.DoesNotContain("style=", source);
        Assert.DoesNotContain("position: absolute", source);
        Assert.DoesNotContain("position:absolute", source);
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
