using System.Reflection;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>WEEK 6 v2 — DEEP LEARNING MODULE, full rebuild per the owner-approved
/// 00_PROJECT_OS/_blueprints/WEEK6_v2_DEEP_LEARNING_BLUEPRINT.md (v1.1). Replaces the v1
/// MVP-depth page. Source: SRC-041 (Judith Beck), Глава 5 — "Структура на ПЪРВАТА
/// терапевтична сесия" — full chapter read from the owner's local PDF, 47 knowledge units,
/// 100% accounted for. Component policy is deliberately relaxed for this week (blueprint
/// §29): WhatIfBox/SourceArtifact/ScenarioSimulator are new, justified reusable components,
/// not a violation of "reuse before creation" — they were added because no existing
/// component could represent this content cleanly, and are designed for reuse by future
/// Deep Learning Weeks.</summary>
public sealed class Week6ContentSliceTests
{
    private static readonly string[] AnchorIds =
    [
        "karta", "struktura", "nachalo", "poniatiya", "deep-dive", "beck-praktika",
        "vizualno", "kray", "case-lab", "simulator", "proverki", "assessment", "review-map", "izvori"
    ];

    [Fact]
    public void Week6Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica6"));
    }

    [Fact]
    public void Week6_MetadataIsUnchanged_RouteAndAvailabilityPreserved()
    {
        // Blueprint §5 migration strategy: KEEP route/technical integration — this is a
        // content-model rebuild, not a curriculum/route change.
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 6);

        Assert.Equal(CourseWeekStatus.Available, week.Status);
        Assert.Equal("/kurs/sedmica-6", week.Route);
    }

    [Fact]
    public void Week1Week3Week8Week10_RemainAvailableAfterWeek6Rebuild()
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
    public void RemainingSixWeeks_StayUnavailable()
    {
        int[] availableNumbers = [1, 2, 3, 5, 6, 7, 8, 9, 10];

        foreach (CourseWeekDefinition week in CourseCatalog.Weeks.Where(w => !availableNumbers.Contains(w.Number)))
        {
            Assert.NotEqual(CourseWeekStatus.Available, week.Status);

            if (week.Number != 4 && week.Number != 11 && week.Number != 12 && week.Number != 13 && week.Number != 14 && week.Number != 15)
            {
                Assert.Null(week.Route);
            }
        }

        Assert.Equal(6, CourseCatalog.Weeks.Count(w => !availableNumbers.Contains(w.Number)));
    }

    [Fact]
    public void Week6Page_HasPageTitleAndDeepLearningBadge()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("<PageTitle>Седмица 6: Структура на терапевтичната сесия", source);
        Assert.Contains("Дълбочинен модул", source);
    }

    [Fact]
    public void Week6Page_HasAllFourteenSections_0Through13()
    {
        string source = ReadPage("Sedmica6.razor");

        foreach (string id in AnchorIds)
        {
            Assert.Contains($"id=\"{id}\"", source);
        }

        for (int i = 0; i <= 13; i++)
        {
            Assert.Contains($"6.{i} ", source);
        }
    }

    [Fact]
    public void Week6Page_SectionNavAnchorsAreRouteSafe()
    {
        string source = ReadPage("Sedmica6.razor");

        foreach (string id in AnchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-6#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
        }
    }

    [Fact]
    public void Week6Page_UsesEstablishedReusablePatterns()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("<LearningSection", source);
        Assert.Contains("<LearningObjectives", source);
        Assert.Contains("<ProgressiveExplanation", source);
        Assert.Contains("<DisclaimerCallout", source);
        Assert.Contains("<SourceReferences", source);
        Assert.Contains("<OptionalReadingSource", source);
        Assert.Contains("class=\"guided-practice-sequence\"", source);
        Assert.Contains("<ConceptGraph", source);
    }

    [Fact]
    public void Week6Page_UsesTheThreeNewJustifiedComponents()
    {
        // Blueprint §29: component policy is deliberately relaxed for this week — these are
        // new, reusable components (justified individually in the blueprint), not a "zero new
        // components" violation.
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("<WhatIfBox", source);
        Assert.Contains("<SourceArtifact", source);
        Assert.Contains("<ScenarioSimulator", source);
    }

    [Fact]
    public void Week6Page_WhatIfBoxIsUsedAtLeastFourTimes()
    {
        string source = ReadPage("Sedmica6.razor");

        int count = CountOccurrences(source, "<WhatIfBox");
        Assert.True(count >= 4, $"Expected at least 4 WhatIfBox instances, found {count}.");
    }

    [Fact]
    public void Week6Page_SourceArtifactIsUsedForBothFigures()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("Списък с домашни задачи на Сали (Фигура 5.1)", source);
        Assert.Contains("Доклад за терапия (Фигура 5.2)", source);
        Assert.Equal(2, CountOccurrences(source, "<SourceArtifact"));
    }

    [Fact]
    public void Week6Page_Section66_UsesClearBulgarianVisualNames_WithWorkingAnchors_NoDeveloperJargon()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("Виж диаграмата „Ситуация → Мисъл → Реакция (пример: обяд с колеги)“ в <a href=\"/kurs/sedmica-6#beck-praktika\">раздел 6.5</a>.", source);
        Assert.Contains("Виж диаграмата с трите легитимни изключения от планираната структура в <a href=\"/kurs/sedmica-6#nachalo\">раздел 6.2</a>.", source);
        Assert.Contains("Виж последователността „Десетте стъпки на първата сесия“ в <a href=\"/kurs/sedmica-6#struktura\">раздел 6.1</a>.", source);

        string publicMarkup = ReadPublicMarkup("Sedmica6.razor");
        Assert.DoesNotContain("branching", publicMarkup);
        Assert.DoesNotContain("process rail", publicMarkup);
        Assert.DoesNotContain("жива диаграма", publicMarkup);
        Assert.DoesNotContain("SourceArtifact)", publicMarkup);
        Assert.DoesNotContain("forward-reference", publicMarkup);
        Assert.DoesNotContain("premine", publicMarkup);
    }

    [Fact]
    public void Week6Page_Figure51_ContainsDateHeadingStandingInstruction_AndTheExactFiveSourceItemsVerbatim()
    {
        string source = ReadPage("Sedmica6.razor");

        int artifactStart = source.IndexOf("<SourceArtifact Title=\"Списък с домашни задачи на Сали (Фигура 5.1)\"", StringComparison.Ordinal);
        int artifactEnd = source.IndexOf("</SourceArtifact>", artifactStart, StringComparison.Ordinal);
        Assert.True(artifactStart >= 0 && artifactEnd > artifactStart);
        string artifact = source[artifactStart..artifactEnd];

        Assert.Contains("<p><strong>22 януари</strong></p>", artifact);
        Assert.Contains("<p><strong>Домашно:</strong></p>", artifact);
        Assert.Contains("Чети този списък два пъти на ден; настрой аларма, за да запомниш.", artifact);

        string[] exactItems =
        [
            "Ако започна да мисля, че съм мързелив и не съм добър, да си припомня, че имам истинска болест, наречена депресия, която ми затруднява да правя неща. Докато лечението започне да действа, моята депресия ще отшуми и нещата ще станат по-лесни.",
            "Прочети списъка с цели и добави други, ако се сетиш за такива.",
            "Когато забележа, че настроението ми се влошава, да се питам: „Какво ми минава през ума в момента?“ и да запиша мислите. Да си припомня, че само защото мисля нещо, не означава непременно, че е вярно.",
            "Да направя планове с Алисон и Джо. Запомни, ако кажат не, вероятно искат да се срещнат с мен, но са твърде заети.",
            "Прочети брошурата „Справяне с депресия“ (по желание)."
        ];
        Assert.Equal(5, System.Text.RegularExpressions.Regex.Matches(artifact, "<li>").Count);
        foreach (string item in exactItems)
        {
            Assert.Contains($"<li>{item}</li>", artifact);
        }
    }

    [Fact]
    public void Week6Page_Figure52_BehavesAsAForm_SourceOnlyInsideArtifact_CommentaryOutside_LocatorIsPage77()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("SourceLabel=\"Възпроизведено по SRC-041, Гл. 5, стр. 77 — собствен дизайн, не факсимиле на оригинала.\"", source);
        Assert.DoesNotContain("стр. 76–77", source);

        int artifactStart = source.IndexOf("<SourceArtifact Title=\"Доклад за терапия (Фигура 5.2)\"", StringComparison.Ordinal);
        int artifactEnd = source.IndexOf("</SourceArtifact>", artifactStart, StringComparison.Ordinal);
        Assert.True(artifactStart >= 0 && artifactEnd > artifactStart);
        string artifact = source[artifactStart..artifactEnd];

        // Exactly five questions, each followed by a visible empty answer area — no platform commentary,
        // rating scales, checkboxes or invented fields inside the source figure itself.
        Assert.Equal(5, System.Text.RegularExpressions.Regex.Matches(artifact, "<dt>").Count);
        Assert.Equal(5, System.Text.RegularExpressions.Regex.Matches(artifact, "class=\"source-artifact__answer-space\" aria-hidden=\"true\"></dd>").Count);
        Assert.Contains("1. Какво обсъдихме днес, което е важно за теб да запомниш?", artifact);
        Assert.Contains("2. Колко много чувстваше, че можеш да се довериш на терапевта си днес?", artifact);
        Assert.Contains("3. Имаше ли нещо, което те притесняваше по време на терапията днес? Ако да, какво беше то?", artifact);
        Assert.Contains("4. Колко домашна работа беше свършил за терапията днес? Колко вероятно е да направиш новата домашна работа?", artifact);
        Assert.Contains("5. Какво искаш да се увериш, че ще обсъдим на следващата сесия?", artifact);
        Assert.DoesNotContain("Проверява", artifact);
        Assert.DoesNotContain("Дава пространство", artifact);
        Assert.DoesNotContain("<input", artifact);
        Assert.DoesNotContain("type=\"checkbox\"", artifact);

        // The platform's own commentary on the five questions now lives AFTER the artifact, as plain prose.
        string afterArtifact = source[(artifactEnd + "</SourceArtifact>".Length)..];
        int nextSectionIndex = afterArtifact.IndexOf("<h2", StringComparison.Ordinal);
        string commentary = nextSectionIndex >= 0 ? afterArtifact[..nextSectionIndex] : afterArtifact;
        Assert.Contains("проверяващ разбирането на пациента", commentary);
        Assert.Contains("проверява алианса директно", commentary);
    }

    [Fact]
    public void Week6Page_TenStepGuidedPracticeSequence_AllStepsPresent()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Equal(10, CountOccurrences(source, "class=\"guided-practice-sequence__step"));

        string[] stepLabels =
        [
            "Задай дневния ред", "Провери настроението", "Получи актуализация", "Обсъди диагнозата",
            "Идентифицирай проблеми и цели", "Обучи за когнитивния модел",
            "Обсъди проблем / поведенческа активация", "Обобщи сесията", "Прегледай домашната работа",
            "Поискай обратна връзка"
        ];

        foreach (string label in stepLabels)
        {
            Assert.Contains(label, source);
        }
    }

    [Fact]
    public void Week6Page_HasARealDecisionBranchDiagram_NotAFlatComparisonGrid()
    {
        // Blueprint §7 finding: the old plan for V4 reused .category-compare (a flat parallel
        // comparison), not an actual root-to-leaves branch. Fixed here with .decision-branch.
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("class=\"decision-branch\"", source);
        Assert.Contains("decision-branch__root", source);
        Assert.Contains("decision-branch__leaves", source);
        Assert.DoesNotContain("class=\"category-compare\"", source);
    }

    [Fact]
    public void Week6Page_HasACustomSvgIllustration_WithAccessibleTitleAndDescription()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("<svg class=\"black-paint-illustration\"", source);
        Assert.Contains("role=\"img\"", source);
        Assert.Contains("<title id=\"paint-title\">", source);
        Assert.Contains("<desc id=\"paint-desc\">", source);
        Assert.DoesNotContain("style=", source);
    }

    [Fact]
    public void Week6Page_FirstSessionPrecision_FramesEverythingAsTheFirstSession()
    {
        // Blueprint §5: v1's own text overgeneralized Ch.5's first-session structure to
        // "every session" — a real source-fidelity defect. The dev comment legitimately
        // names the old overgeneralized phrase to explain why it was fixed (not rendered),
        // so it's checked against the public markup only, matching the established
        // ReadPublicMarkup convention (e.g. Week 6 v1's own BDI/BAI dev-comment handling).
        string publicMarkup = ReadPublicMarkup("Sedmica6.razor");

        Assert.Contains("ПЪРВАТА", publicMarkup);
        Assert.Contains("първата сесия", publicMarkup);
        Assert.Contains("Глава 7", publicMarkup);
        Assert.DoesNotContain("общата форма на стандартна сесия", publicMarkup);
        Assert.DoesNotContain("типична сесия за наблюдател", publicMarkup);
    }

    [Fact]
    public void Week6Page_HasConfirmedDurationClaim()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("45–50 минути", source);
    }

    [Fact]
    public void Week6Page_HasTheThreeDeviationCriteria()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("Риск за пациента или други", source);
        Assert.Contains("Твърде разстроен, за да се фокусира", source);
        Assert.Contains("Риск за терапевтичния алианс", source);
    }

    [Fact]
    public void Week6Page_TerminologyMap_HasElevenTerms()
    {
        string source = ReadPage("Sedmica6.razor");

        string[] terms =
        [
            "Задаване на дневния ред", "Проверка на настроението", "Придобиване на актуализация",
            "Социализиране на пациента", "Психообразование", "Когнитивен модел (Ситуация → Мисъл → Реакция)",
            "Терапевтичен алианс", "Когнитивна триада (\"черна боя\" метафора)",
            "Списък с домашни задачи", "Доклад за терапия", "Поведенческа активация"
        ];

        foreach (string term in terms)
        {
            Assert.Contains(term, source);
        }
    }

    [Fact]
    public void Week6Page_U08U22SafetyContract_ObservationalFramingPresent()
    {
        // Owner-approved (blueprint §4): risk/mood-screening content is INCLUDED with strict
        // third-person/observational framing — unlike v1, BDI-II/BAI are no longer forbidden
        // terms, but they must always appear inside professional-observation language, never
        // as something the reader is invited to apply to themselves.
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("BDI-II", source);
        Assert.Contains("BAI", source);
        Assert.Contains("описание на професионална клинична практика", source);
        Assert.Contains("не начин да прецените собственото си състояние", source);
        Assert.Contains("изцяло третолично", source);
        Assert.Contains("без поле за въвеждане, без оценяване, без автоматична класификация на риска", source);
    }

    [Fact]
    public void Week6Page_HasNoSelfAssessmentInputOrScoring()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.DoesNotContain("<input", source);
        Assert.DoesNotContain("твоят резултат", source);
        Assert.DoesNotContain("вашият резултат", source);
    }

    [Fact]
    public void Week6Page_DistinguishesOverviewFromSelfTherapyInstruction()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica6.razor");

        Assert.Contains("не заменя професионална психологическа или медицинска помощ", publicMarkup);
        Assert.Contains("не обучава за самостоятелно провеждане на терапия", publicMarkup);
    }

    [Fact]
    public void Week6Page_CaseLabHasAllThreeCases()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("Мартин — Основен", source);
        Assert.Contains("Ирина — Среден", source);
        Assert.Contains("Радо — Предизвикателен", source);
        Assert.Contains("одобрен пилотен случай с проследяване във времето", source);
    }

    [Fact]
    public void Week6Page_HasNoLearnerFacingDeveloperJargon()
    {
        // Targeted language-cleanup guard (owner request): these English/mixed-language
        // terms were previously found leaking into learner-facing Week 6 text. Source
        // metadata, component names, and the dev comment block are not learner-facing and
        // are intentionally excluded via ReadPublicMarkup.
        string publicMarkup = ReadPublicMarkup("Sedmica6.razor");

        string[] bannedPhrases =
        [
            "Deep Dive", "Beck in Practice", "Case Lab", "— Basic", "— Intermediate",
            "— Challenging", "risk-свързано", "input поле", "scoring",
            "risk класификация", "self-check", "self-therapy", "professional практика",
            "pilot longitudinal case", "screen reader", "Case Conceptualization Map",
            "Session Structure & Decision Simulator"
        ];

        foreach (string phrase in bannedPhrases)
        {
            Assert.DoesNotContain(phrase, publicMarkup);
        }
    }

    [Fact]
    public void Week6Page_SimulatorIsWiredUpWithAllRequiredData()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("<ScenarioSimulator", source);
        Assert.Contains("StepLabels=\"@_stepLabels\"", source);
        Assert.Contains("IdentifyItems=\"@_identifyItems\"", source);
        Assert.Contains("MatchingPairs=\"@_matchingPairs\"", source);
        Assert.Contains("OrderingSteps=\"@_orderingSteps\"", source);
        Assert.Contains("NextStepScenario=\"@_nextStepScenario\"", source);
        Assert.Contains("BranchStartNodeId=\"@_branchStartId\"", source);
        Assert.Contains("BranchNodes=\"@_branchNodes\"", source);
    }

    [Fact]
    public void Week6Page_BranchScenarioHasSevenNodes_StartTwoDecisionsFourEndings()
    {
        string source = ReadPage("Sedmica6.razor");

        string[] nodeIds = ["\"start\"", "\"ask\"", "\"reassure-end\"", "\"skip-end\"", "\"repair-end\"", "\"insist-end\"", "\"overcorrect-end\""];

        foreach (string id in nodeIds)
        {
            Assert.Contains(id, source);
        }
    }

    [Fact]
    public void Week6Page_FinalAssessmentHasTwentyQuestions()
    {
        string source = ReadPage("Sedmica6.razor");

        // 18 are in the shared scored component; the original Q02 (ordering) and Q08 (identify-the-
        // error) have no lettered options in the source and are kept as separate, unscored static
        // items instead of inventing multiple-choice distractors for a LOCKED week — see the code
        // comment above _week6FinalAssessment. All 20 original questions still exist on the page.
        Assert.Equal(18, TestPaths.CountFinalAssessmentQuestions(source, "_week6FinalAssessment"));
        Assert.Contains("Подредете: Обратна връзка / Задаване на дневния ред / Обобщение / Проверка на настроението", source);
        Assert.Contains("\"Черната боя\" метафората означава, че светът обективно е тъмен.", source);
    }

    [Fact]
    public void Week6Page_AssessmentQuestionsHaveExplanatoryFeedbackWithABackLink()
    {
        string source = ReadPage("Sedmica6.razor");

        int backLinkCount = CountOccurrences(source, "<a href=\"/kurs/sedmica-6#");
        Assert.True(backLinkCount >= 18, $"Expected at least 18 section back-links in assessment feedback, found {backLinkCount}.");

        // The internal KU-id prefix ("Source: Uxx ·") was removed from learner-facing feedback —
        // see LearnerFacingKuIdGuardTests for the authoritative cross-page guard.
        Assert.DoesNotContain("Source: U", source);
    }

    [Fact]
    public void Week6Page_Q19IsAcademicContent_NotPlatformSafetyPolicy()
    {
        // Blueprint §16: the old Q19 tested "observation vs self-assessment" (platform policy).
        // The revised Q19 tests source-grounded CBT knowledge (deviation criteria) instead.
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("AssessmentQuestion.Choice(\"q19\"", source);
        Assert.Contains("Кои основания легитимно оправдават отлагане на планираната стъпка", source);
    }

    [Fact]
    public void Week6Page_HasThirteenLocalKnowledgeChecks()
    {
        string source = ReadPage("Sedmica6.razor");

        for (int i = 1; i <= 13; i++)
        {
            Assert.Contains($"Проверка {i}", source);
        }
    }

    [Fact]
    public void Week6Page_ReviewMapHasCrossWeekConnections()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("/kurs/sedmica-3", source);
        Assert.Contains("/kurs/sedmica-8", source);
        Assert.Contains("/kurs/sedmica-10", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Week6Page_DoesNotLinkToWeek5AsIfAvailable()
    {
        // Week 5 has not been implemented yet (frozen build order position 5, after Week 6/12/7).
        string source = ReadPage("Sedmica6.razor");

        Assert.DoesNotContain("/kurs/sedmica-5", source);
    }

    [Fact]
    public void Week6Page_HasNoInternalDevelopmentLanguageInItsRenderableMarkup()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica6.razor");

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
    public void KursPage_ShowsAllSevenAvailableWeeks()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("/kurs/sedmica-1", source);
        Assert.Contains("/kurs/sedmica-3", source);
        Assert.Contains("/kurs/sedmica-6", source);
        Assert.Contains("/kurs/sedmica-7", source);
        Assert.Contains("/kurs/sedmica-8", source);
        Assert.Contains("/kurs/sedmica-9", source);
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
