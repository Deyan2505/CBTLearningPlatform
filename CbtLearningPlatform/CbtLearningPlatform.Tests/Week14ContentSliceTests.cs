using System.Reflection;
using System.Text.RegularExpressions;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>Week 14 ("Домашна работа, прекратяване и превенция на рецидив") — final source contract
/// implementation (owner-approved 2026-09-07, WEEK_14_SOURCE_AUDIT_v1.md). Content basis: SRC-041,
/// Глава 17 (printed pp. 294-315) + Глава 18 (printed pp. 316-331). Safety level stays
/// CurriculumSafetyLevel.ProfessionalReviewRequired — the page is routed and completion-eligible but
/// its public status never becomes Available, and its declared InteractiveFormat is never
/// StaticVisualization. Owner-resolved exclusions enforced here: Figures 18.3/18.5 are descriptive
/// only (never a usable form/checklist); the 0-100% question and sub-90% fallback have no self-input
/// control; the four-way diagnosis is clinician-perspective, not learner self-assessment; covert
/// rehearsal, role play, BDI, reproduced dialogue, induced imagery, and the exposure hierarchy
/// procedure all stay excluded; Sally's first-person setback coping card (Figure 18.4) is excluded.</summary>
public sealed class Week14ContentSliceTests
{
    [Fact]
    public void Week14Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica14"));
    }

    [Fact]
    public void Week14_MetadataIsRoutedButProfessionalReviewRequiredNotAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 14);

        Assert.Equal("/kurs/sedmica-14", week.Route);
        Assert.Equal(CurriculumSafetyLevel.ProfessionalReviewRequired, week.SafetyLevel);
        Assert.Equal(CourseWeekStatus.ProfessionalReviewRequired, week.Status);
        Assert.NotEqual(CourseWeekStatus.Available, week.Status);
        Assert.Equal("Изисква професионален преглед", week.Status.ToPublicLabel());
    }

    [Fact]
    public void Week14_FormatIsInteractiveModelNeverStaticVisualization()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 14);

        Assert.Contains(InteractiveFormat.InteractiveModel, week.InteractiveFormats);
        Assert.DoesNotContain(InteractiveFormat.StaticVisualization, week.InteractiveFormats);
        Assert.DoesNotContain(InteractiveFormat.Simulator, week.InteractiveFormats);
    }

    [Fact]
    public void Week14_TitleUsesPrekratyavaneNeverPriklyuchvane()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 14);

        Assert.Equal("Домашна работа, прекратяване и превенция на рецидив", week.Title);
        Assert.DoesNotContain("приключване", week.Title);
    }

    [Fact]
    public void Week14_ObjectivesCoverAllSixApprovedThemes()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 14);
        string joined = string.Join(" | ", week.LearningObjectives);

        Assert.Equal(6, week.LearningObjectives.Count);
        Assert.Contains("проектира", joined);
        Assert.Contains("вероятността за изпълнение", joined);
        Assert.Contains("неизпълнението", joined);
        Assert.Contains("прегледа на домашното", joined);
        Assert.Contains("възстановяването", joined);
        Assert.Contains("превенция на рецидив", joined);
    }

    [Fact]
    public void Week14Page_HasPageTitleAndFormatBadge()
    {
        string source = ReadPage("Sedmica14.razor");

        Assert.Contains("<PageTitle>Седмица 14: Домашна работа, прекратяване и превенция на рецидив", source);
        Assert.Contains("Изисква професионален преглед", source);
    }

    [Fact]
    public void Week14Page_UsesOnlyExistingReusablePatterns()
    {
        string source = ReadPage("Sedmica14.razor");

        Assert.Contains("<LearningSection", source);
        Assert.Contains("<LearningObjectives", source);
        Assert.Contains("<ProgressiveExplanation", source);
        Assert.Contains("<DisclaimerCallout", source);
        Assert.Contains("<SourceReferences", source);
        Assert.Contains("<OptionalReadingSource", source);
        Assert.Contains("<WeekCompletionControl", source);
        Assert.Contains("<FinalAssessment", source);
        Assert.Contains("<ConceptGraph", source);
        Assert.Contains("<SourceArtifact", source);
        Assert.Contains("class=\"guided-practice-sequence\"", source);
        Assert.Contains("class=\"comparison-matrix-wrapper\"", source);
        Assert.Contains("class=\"learning-grid learning-grid--balanced\"", source);

        // No new .razor component and no new interactive island.
        Assert.DoesNotContain("<CbtChainSimulator", source);
        Assert.DoesNotContain("<InterpretationExample", source);
        Assert.DoesNotContain("<CategorizationCheck", source);
    }

    [Fact]
    public void Week14Page_MindMapPresentInPreviewAndReview_CollapsedByDefaultInReview_NoConceptMapIntroduced()
    {
        string source = ReadPage("Sedmica14.razor");

        int previewIndex = source.IndexOf("<h2 id=\"nakratko\"", StringComparison.Ordinal);
        int reviewIndex = source.IndexOf("<h2 id=\"review\"", StringComparison.Ordinal);
        Assert.True(previewIndex >= 0 && reviewIndex > previewIndex);

        string reviewSection = source[reviewIndex..source.IndexOf("<h2 id=\"assessment\"", StringComparison.Ordinal)];

        Assert.Contains("<details class=\"progressive-explanation concept-graph__retrieval-check\">", reviewSection);
        Assert.DoesNotContain("<details class=\"progressive-explanation concept-graph__retrieval-check\" open>", reviewSection);
        Assert.Contains("<ConceptGraph", reviewSection);

        Assert.Equal(2, Regex.Matches(source, "<ConceptGraph").Count);
        Assert.Equal(2, Regex.Matches(source, @"Model=""@_week14MindMapRender""").Count);
    }

    [Fact]
    public void Week14Page_MindMapUsesEightKnowledgeClusterBranches()
    {
        string source = ReadPage("Sedmica14.razor");

        int mapStart = source.IndexOf("private static MindMapModel BuildWeek14MindMap", StringComparison.Ordinal);
        int mapEnd = source.IndexOf("]);", mapStart, StringComparison.Ordinal);
        Assert.True(mapStart >= 0 && mapEnd > mapStart);
        string mapBlock = source[mapStart..mapEnd];

        string[] expectedBranches =
        [
            "Домашна работа като инструмент", "Увеличаване на изпълнението", "Диагностика на неизпълнение",
            "Преглед на домашното", "Форма на възстановяването", "Приписване на напредъка",
            "Намаляване и прекратяване", "Поддържане след терапията"
        ];
        foreach (string branch in expectedBranches)
        {
            Assert.Contains($"\"{branch}\"", mapBlock);
        }
    }

    [Fact]
    public void Week14Page_UsesSharedFinalAssessmentWithEightSourceGroundedQuestions()
    {
        string source = ReadPage("Sedmica14.razor");

        Assert.Contains("<FinalAssessment SectionId=\"assessment\" Model=\"_week14FinalAssessment\" />", source);
        Assert.Equal(8, TestPaths.CountFinalAssessmentQuestions(source, "_week14FinalAssessment"));
    }

    [Fact]
    public void Week14Page_WeekCompletionControlIsIndependentOfAssessmentPlacement()
    {
        string source = ReadPage("Sedmica14.razor");

        int assessmentIndex = source.IndexOf("<FinalAssessment", StringComparison.Ordinal);
        int completionIndex = source.IndexOf("<WeekCompletionControl WeekNumber=\"@_week.Number\" />", StringComparison.Ordinal);

        Assert.True(assessmentIndex >= 0 && completionIndex > assessmentIndex);
    }

    [Fact]
    public void Week14Page_SectionNavAnchorsAreRouteSafe()
    {
        string source = ReadPage("Sedmica14.razor");

        string[] anchorIds =
        [
            "nakratko", "domashna-rabota-kato-instrument", "uvelichavane-na-izpalnenieto",
            "diagnostika-na-neizpalnenie", "pregled-na-domashnata", "forma-na-vazstanovyavaneto",
            "pripisvane-na-napreduka", "namalyavane-i-prekratyavane", "podderzhane-sled-terapiyata",
            "review", "assessment", "izvori"
        ];

        foreach (string id in anchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-14#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
            Assert.Contains($"id=\"{id}\"", source);
        }
    }

    [Fact]
    public void Week14Page_CrossLinksToWeeksSixSevenNineElevenThirteenAndTheHub_NoDeadLinks()
    {
        string source = ReadPage("Sedmica14.razor");

        Assert.Contains("/kurs/sedmica-6", source);
        Assert.Contains("/kurs/sedmica-7", source);
        Assert.Contains("/kurs/sedmica-9", source);
        Assert.Contains("/kurs/sedmica-11", source);
        Assert.Contains("/kurs/sedmica-13", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Week14Page_DoesNotReproduceWeek6sHomeworkListArtifact()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica14.razor");

        // Week 6 owns the dated homework-list session artifact (Figure 5.1) — this page only
        // cross-links to it, never reproduces the list itself.
        Assert.DoesNotContain("Списък с домашни задачи на Сали", publicMarkup);
    }

    [Fact]
    public void Week14Page_UsesLockedCanonicalTerminology()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica14.razor");

        Assert.Contains("прекратяване", publicMarkup);
        Assert.Contains("поддържащ", publicMarkup); // "поддържаща сесия" / "поддържащи сесии"
        Assert.Contains("списък на заслугите", publicMarkup);

        // Competing translation variants must never survive onto the learner-facing page.
        Assert.DoesNotContain("бустер сесия", publicMarkup);
        Assert.DoesNotContain("сесия за усилване", publicMarkup);
        Assert.DoesNotContain("сесия за подсилване", publicMarkup);
        Assert.DoesNotContain("сесия за повишаване на мотивацията", publicMarkup);
        Assert.DoesNotContain("списък с кредити", publicMarkup);
    }

    [Fact]
    public void Week14Page_ParadoxicalArousalCautionIsCoLocatedWithEveryRelaxationMention()
    {
        string normalized = Regex.Replace(ReadPublicMarkup("Sedmica14.razor"), @"\s+", " ");

        int relaxationIndex = normalized.IndexOf("релаксац", StringComparison.OrdinalIgnoreCase);
        Assert.True(relaxationIndex >= 0, "Expected relaxation to be mentioned at least once.");

        string window = normalized.Substring(relaxationIndex, Math.Min(400, normalized.Length - relaxationIndex));
        Assert.Contains("парадоксален ефект", window);
    }

    [Fact]
    public void Week14Page_FiguresAreDescriptiveOnly_NoUsableSelfTherapyOrBoosterForm()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica14.razor");

        // Figure 18.3/18.5 are academic description only — never a fillable, first-person checklist.
        Assert.DoesNotContain("<input", publicMarkup);
        Assert.DoesNotContain("<textarea", publicMarkup);
        Assert.DoesNotContain("<form", publicMarkup);
        Assert.DoesNotContain("Моят план за самотерапия", publicMarkup);
        Assert.DoesNotContain("Моят план за поддържаща сесия", publicMarkup);
    }

    [Fact]
    public void Week14Page_HasNoPercentageOrSelfRatingInputForAdherence()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica14.razor");

        Assert.DoesNotContain("type=\"range\"", publicMarkup);
        Assert.DoesNotContain("type=\"number\"", publicMarkup);
        Assert.DoesNotContain("Оценете вероятността", publicMarkup);
        Assert.DoesNotContain("Вашата увереност", publicMarkup);
    }

    [Fact]
    public void Week14Page_FourWayDiagnosisIsClinicianPerspective_NotLearnerSelfAssessment()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica14.razor");

        Assert.Contains("клиничен инструмент на терапевта", publicMarkup);
        Assert.Contains("не тест за самооценка на", publicMarkup);
    }

    [Fact]
    public void Week14Page_DiagnosisTableCellsCarryDataLabelsForMobileStack_ContentUnchanged()
    {
        // Owner production-review fix: below 480px this table reflows into stacked cards via CSS
        // (app.css), driven by these data-label attributes — same <table>, same rows/cells/order,
        // no wording change. Guards against the labels silently drifting from the visible column
        // headers or being dropped from a future edit.
        string publicMarkup = ReadPublicMarkup("Sedmica14.razor");

        Assert.Equal(4, Regex.Matches(publicMarkup, Regex.Escape("data-label=\"Как се разпознава\"")).Count);
        Assert.Equal(4, Regex.Matches(publicMarkup, Regex.Escape("data-label=\"Отговор на терапевта\"")).Count);
    }

    [Fact]
    public void Week14Page_ExcludesCovertRehearsalRolePlayBdiAndExposureHierarchyProcedure()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica14.razor");

        string[] excludedTerms =
        [
            "тайна репетиция", "скрито репетиране",              // covert rehearsal
            "интелектуално-емоционална ролева игра",              // role play mechanics
            "BDI", "Инвентар за депресия",                        // clinical scoring instrument
            "Хващам се, когато",                                  // Sally's first-person coping card (Fig 18.4)
        ];
        foreach (string term in excludedTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }

        // Avoided-task hierarchy is named once in the maintenance inventory, never taught as a procedure.
        Assert.Contains("йерархии на избягвани задачи", publicMarkup);
        Assert.DoesNotContain("стъпка по стъпка изградете йерархия", publicMarkup);
    }

    [Fact]
    public void Week14Page_HasNoSelfInputFormsOrSelfAssessmentLanguage()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica14.razor");

        Assert.DoesNotContain("<input", publicMarkup);
        Assert.DoesNotContain("<textarea", publicMarkup);
        Assert.DoesNotContain("<form", publicMarkup);

        Assert.Contains("не е инструмент за самооценка", publicMarkup);
    }

    [Fact]
    public void Week14Page_HasNoReproducedTherapistPatientDialogueTranscripts()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica14.razor");

        Assert.DoesNotContain("Терапевт:", publicMarkup);
        Assert.DoesNotContain("Пациент:", publicMarkup);
    }

    [Fact]
    public void Week14Page_SallyUsageIsLimitedToFigure182_NoNewBiography()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica14.razor");

        Assert.Contains("Сали", publicMarkup);
        Assert.Contains("Ще имам повече възможности да използвам и усъвършенствам инструментите си.", publicMarkup);

        // No invented dialogue or biography beyond Figure 18.2's advantages/disadvantages table.
        Assert.DoesNotContain("Терапевт:", publicMarkup);
    }

    [Fact]
    public void Week14Page_HasNoInternalDevelopmentLanguageInItsRenderableMarkup()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica14.razor");

        string[] forbiddenTerms =
        [
            "11_SOURCE_REGISTER.md", "kpt_syllabus.pdf", "citation-grade", "WEEK_14_SOURCE_AUDIT",
            "10_SESSION_LOG.md", "Project OS", "code_artifact.html", "Needs Review", "KU", "GAP-014"
        ];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }
    }

    [Fact]
    public void Week14Page_MakesNoFalseAccreditationClaims()
    {
        string source = ReadPage("Sedmica14.razor");

        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week14Page_HasNoDiagnosticOrClinicalScoringContent()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica14.razor");

        string[] forbiddenTerms = ["BDI", "BAI", "BHS", "терапевтичен план", "диагностичен инструмент"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }

        Assert.Contains("не поставя диагноза", publicMarkup);
    }

    [Fact]
    public void Week14Page_HasNoPageLevelOverflowWorkaroundOrInlineStyles()
    {
        string source = ReadPage("Sedmica14.razor");

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
