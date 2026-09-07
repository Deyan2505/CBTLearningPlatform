using System.Reflection;
using System.Text.RegularExpressions;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>Week 11 ("Междинни вярвания") — NET-NEW-scope implementation of WEEK_11_SOURCE_AUDIT_v1.md
/// (owner-approved 2026-09-07). Content basis: SRC-041, Глава 13, printed pp. 198-227. This page
/// deliberately does NOT re-teach Week 3's (LOCKED) 3-level hierarchy, Attitude/Rule/Assumption split,
/// or Sally's established core belief/developmental origin — recap + cross-link only. Safety level
/// stays CurriculumSafetyLevel.ProfessionalReviewRequired, so the page is routed and completion-eligible
/// but its public status never becomes Available. Owner-resolved exclusions enforced here: the Downward
/// Arrow Technique is named/cited only (no procedure, no sample questions, no exercise); the book's
/// "Emily"/"Rebecca" secondary figures are generalized, never introduced as named recurring characters;
/// the Dysfunctional Attitude Scale / Personality Belief Questionnaire are excluded entirely.</summary>
public sealed class Week11ContentSliceTests
{
    [Fact]
    public void Week11Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica11"));
    }

    [Fact]
    public void Week11_MetadataIsRoutedButProfessionalReviewRequiredNotAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 11);

        Assert.Equal("/kurs/sedmica-11", week.Route);
        Assert.Equal(CurriculumSafetyLevel.ProfessionalReviewRequired, week.SafetyLevel);
        Assert.Equal(CourseWeekStatus.ProfessionalReviewRequired, week.Status);
        Assert.NotEqual(CourseWeekStatus.Available, week.Status);
    }

    [Fact]
    public void Week11_ObjectivesReflectNetNewOwnership_NotWeek3sGeneralArchitecture()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 11);
        string joined = string.Join(" | ", week.LearningObjectives);

        Assert.Contains("идентифицира и организира", joined);
        Assert.Contains("преценява дали", joined);
        Assert.Contains("категории стратегии за модифициране", joined);

        // The pre-implementation objectives were generic hierarchy-recognition claims already owned
        // by Week 3 — they must not survive the retrofit.
        Assert.DoesNotContain("Разбирате какво е междинно вярване (нагласа/правило)", joined);
        Assert.DoesNotContain("Виждате връзката между междинни вярвания и повтарящи се автоматични мисли", joined);
    }

    [Fact]
    public void Week11Page_HasPageTitleAndFormatBadge()
    {
        string source = ReadPage("Sedmica11.razor");

        Assert.Contains("<PageTitle>Седмица 11: Междинни вярвания", source);
        Assert.Contains("Изисква професионален преглед", source);
    }

    [Fact]
    public void Week11Page_UsesOnlyExistingReusablePatterns()
    {
        string source = ReadPage("Sedmica11.razor");

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
    }

    [Fact]
    public void Week11Page_RecapsWeek3ByCrossLink_DoesNotReRenderItsHierarchyOrTypeTable()
    {
        string source = ReadPage("Sedmica11.razor");

        Assert.Contains("/kurs/sedmica-3", source);

        // Week 3's own hierarchy diagram and Attitude/Rule/Assumption classification table/exercise
        // must not be reproduced here.
        Assert.DoesNotContain("Три вида междинно вярване", source);
        Assert.DoesNotContain("learning-path-diagram", source);
        Assert.DoesNotContain("Изберете кой вид междинно вярване", source);

        // Only one ConceptGraph model exists on this page (the new Week 11 Mind Map, in Preview and
        // Review) — no second, hierarchy-style Concept/Case map was added.
        Assert.Equal(2, Regex.Matches(source, "<ConceptGraph").Count);
        Assert.Equal(2, Regex.Matches(source, @"Model=""@_week11MindMapRender""").Count);
    }

    [Fact]
    public void Week11Page_MindMapPresentInPreviewAndReview_CollapsedByDefaultInReview()
    {
        string source = ReadPage("Sedmica11.razor");

        int previewIndex = source.IndexOf("<h2 id=\"nakratko\"", StringComparison.Ordinal);
        int reviewIndex = source.IndexOf("<h2 id=\"review\"", StringComparison.Ordinal);
        Assert.True(previewIndex >= 0 && reviewIndex > previewIndex);

        string reviewSection = source[reviewIndex..source.IndexOf("<h2 id=\"assessment\"", StringComparison.Ordinal)];

        Assert.Contains("<details class=\"progressive-explanation concept-graph__retrieval-check\">", reviewSection);
        Assert.DoesNotContain("<details class=\"progressive-explanation concept-graph__retrieval-check\" open>", reviewSection);
        Assert.Contains("<ConceptGraph", reviewSection);
    }

    [Fact]
    public void Week11Page_MindMapUsesTheSixSuggestedBranches_NoDeepCognitiveHierarchyNodesReintroduced()
    {
        string source = ReadPage("Sedmica11.razor");

        int mapStart = source.IndexOf("private static MindMapModel BuildWeek11MindMap", StringComparison.Ordinal);
        int mapEnd = source.IndexOf("]);", mapStart, StringComparison.Ordinal);
        Assert.True(mapStart >= 0 && mapEnd > mapStart);
        string mapBlock = source[mapStart..mapEnd];

        string[] expectedBranches =
        [
            "Концептуализация", "Стратегии за справяне", "Идентифициране",
            "Решение дали да се модифицира", "Модифициране", "Сали: промяна във вярването"
        ];
        foreach (string branch in expectedBranches)
        {
            Assert.Contains($"\"{branch}\"", mapBlock);
        }

        // The hierarchy nodes Week 3 already owns must not be reintroduced as Mind Map branches here.
        Assert.DoesNotContain("\"Автоматична мисъл\"", mapBlock);
        Assert.DoesNotContain("\"Основно вярване\"", mapBlock);
    }

    [Fact]
    public void Week11Page_DownwardArrowIsNamedOnly_NoProcedureNoSampleQuestionsNoExercise()
    {
        string source = ReadPage("Sedmica11.razor");

        Assert.Contains("Техниката на низходящата стрела", source);
        Assert.Contains("Бърнс, 1980", source);
        Assert.Contains("без стъпки за прилагане", source);

        // No procedural/sample-question language, and no second ordered list (a steps sequence)
        // beyond the single 3-stage overview list.
        string[] forbiddenProceduralFragments =
        [
            "Какво би означавало", "Какво означава това за теб", "Стъпка 1", "стъпка по стъпка",
            "Какво е толкова лошо", "най-лошата част"
        ];
        foreach (string fragment in forbiddenProceduralFragments)
        {
            Assert.DoesNotContain(fragment, source);
        }

        Assert.Single(Regex.Matches(source, "<ol "));
    }

    [Fact]
    public void Week11Page_ExcludesClinicalQuestionnairesFromLearnerFacingContent()
    {
        // Public markup only — the dev comment legitimately *names* these excluded instruments to
        // document the owner decision; the learner-facing page itself must never mention them.
        string publicMarkup = ReadPublicMarkup("Sedmica11.razor");

        string[] excludedInstrumentTerms =
        [
            "Дисфункционални нагласи", "Скала за дисфункционални", "Личностни убеждения",
            "Dysfunctional Attitude", "Personality Belief Questionnaire"
        ];
        foreach (string term in excludedInstrumentTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }
    }

    [Fact]
    public void Week11Page_DoesNotIntroduceEmilyOrRebeccaAsNamedRecurringFigures()
    {
        // Public markup only — the dev comment legitimately names "Emily"/"Rebecca" to document that
        // they were deliberately generalized; the learner-facing page itself must never name them.
        string publicMarkup = ReadPublicMarkup("Sedmica11.razor");

        string[] bannedNames = ["Емили", "Ребека", "Emily", "Rebecca"];
        foreach (string name in bannedNames)
        {
            Assert.DoesNotContain(name, publicMarkup);
        }
    }

    [Fact]
    public void Week11Page_SallyUsageAddsNoBiographyBeyondChapter13_NoReproducedDialogue()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica11.razor");

        // Section 08's before/after table is the only new Sally-specific content this page adds.
        Assert.Contains("Сали", publicMarkup);
        Assert.Contains("не добавя нова биография", publicMarkup);

        // No reproduced therapist-patient dialogue (a "Терапевт:"/"Пациент:" transcript format).
        Assert.DoesNotContain("Терапевт:", publicMarkup);
        Assert.DoesNotContain("Пациент:", publicMarkup);
    }

    [Fact]
    public void Week11Page_HasNoSelfInputFormsOrSelfAssessmentLanguage()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica11.razor");

        Assert.DoesNotContain("<input", publicMarkup);
        Assert.DoesNotContain("<textarea", publicMarkup);
        Assert.DoesNotContain("<form", publicMarkup);

        string[] selfAssessmentPhrases = ["твоето вярване", "вашето собствено вярване", "оценете себе си", "твоята схема"];
        foreach (string phrase in selfAssessmentPhrases)
        {
            Assert.DoesNotContain(phrase, publicMarkup);
        }

        Assert.Contains("не е инструмент за самооценка", publicMarkup);
    }

    [Fact]
    public void Week11Page_UsesSharedFinalAssessmentWithEightSourceGroundedQuestions()
    {
        string source = ReadPage("Sedmica11.razor");

        Assert.Contains("<FinalAssessment SectionId=\"assessment\" Model=\"_week11FinalAssessment\" />", source);
        Assert.Equal(8, TestPaths.CountFinalAssessmentQuestions(source, "_week11FinalAssessment"));
    }

    [Fact]
    public void Week11Page_WeekCompletionControlIsIndependentOfAssessmentPlacement()
    {
        string source = ReadPage("Sedmica11.razor");

        int assessmentIndex = source.IndexOf("<FinalAssessment", StringComparison.Ordinal);
        int completionIndex = source.IndexOf("<WeekCompletionControl WeekNumber=\"@_week.Number\" />", StringComparison.Ordinal);

        Assert.True(assessmentIndex >= 0 && completionIndex > assessmentIndex);
    }

    [Fact]
    public void Week11Page_SectionNavAnchorsAreRouteSafe()
    {
        string source = ReadPage("Sedmica11.razor");

        string[] anchorIds =
        [
            "nakratko", "vrazka-sedmica-3", "koncept-diagrama", "strategii-za-spravyane",
            "identificirane", "reshenie-za-modifikaciya", "metodi-za-modificirane",
            "sali-predi-sled", "review", "assessment", "izvori"
        ];

        foreach (string id in anchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-11#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
            Assert.Contains($"id=\"{id}\"", source);
        }
    }

    [Fact]
    public void Week11Page_CrossLinksToWeek3AndTheHub_NoDeadLinks()
    {
        string source = ReadPage("Sedmica11.razor");

        Assert.Contains("/kurs/sedmica-3", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Week11Page_HasNoInternalDevelopmentLanguageInItsRenderableMarkup()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica11.razor");

        string[] forbiddenTerms =
        [
            "11_SOURCE_REGISTER.md", "kpt_syllabus.pdf", "citation-grade", "WEEK_11_SOURCE_AUDIT",
            "10_SESSION_LOG.md", "Project OS", "code_artifact.html", "Needs Review", "KU"
        ];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }
    }

    [Fact]
    public void Week11Page_MakesNoFalseAccreditationClaims()
    {
        string source = ReadPage("Sedmica11.razor");

        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week11Page_HasNoDiagnosticOrClinicalScoringContent()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica11.razor");

        string[] forbiddenTerms = ["BDI", "BAI", "BHS", "терапевтичен план", "диагностичен инструмент"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }

        Assert.Contains("не поставя диагноза", publicMarkup);
    }

    [Fact]
    public void Week11Page_HasNoPageLevelOverflowWorkaroundOrInlineStyles()
    {
        string source = ReadPage("Sedmica11.razor");

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
