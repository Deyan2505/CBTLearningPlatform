using System.Reflection;
using System.Text.RegularExpressions;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>Week 15 ("Съвременни разширения на КПТ и възстановително-ориентирана терапия") — final
/// source contract implementation (owner-approved 2026-09-08, WEEK_15_SOURCE_AUDIT_v1.md §14).
/// Content basis: SRC-041 (Judith Beck), Гл. 1, printed p.2-3 — the only place in the book touching
/// this week's topic (DBT and ACT named there with citations) — plus SRC-013 (Beck Institute,
/// FULLY REVIEWED) as the primary source for CT-R. SRC-041 has no chapter on this topic; this is the
/// first week without a source-textbook chapter behind it. Safety level stays
/// CurriculumSafetyLevel.AcademicContextOnly / InteractiveFormat.AcademicOnly — routed and
/// completion-eligible, resolving to CourseWeekStatus.AcademicOverview (Weeks 4/12 precedent), never
/// Available. Owner-resolved exclusions enforced here: no "third wave" organizing structure (the label
/// appears once, caveated); no wave-decade ranges; MBCT excluded entirely; the two CT-R belief
/// categories are named with paraphrased, third-person, non-self-rated examples only; Chapter 21's
/// self-application protocol is not used; C15-K05 (therapist development) and GAP-014's remaining
/// half stay unassigned; no Sally, no invented case.</summary>
public sealed class Week15ContentSliceTests
{
    [Fact]
    public void Week15Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica15"));
    }

    [Fact]
    public void Week15_MetadataIsRoutedButAcademicOverviewNotAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 15);

        Assert.Equal("/kurs/sedmica-15", week.Route);
        Assert.Equal(CurriculumSafetyLevel.AcademicContextOnly, week.SafetyLevel);
        Assert.Equal(CourseWeekStatus.AcademicOverview, week.Status);
        Assert.NotEqual(CourseWeekStatus.Available, week.Status);
    }

    [Fact]
    public void Week15_FormatIsAcademicOnlyNeverInteractiveOrSimulator()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 15);

        // AcademicOnly remains the course format after remediation (Weeks 4/12 precedent); only the
        // self-guided "Simulator" label is forbidden — the internal StatefulModelSimulator engine is
        // not a learner-facing format label and does not contradict AcademicOnly.
        Assert.Contains(InteractiveFormat.AcademicOnly, week.InteractiveFormats);
        Assert.DoesNotContain(InteractiveFormat.Simulator, week.InteractiveFormats);
    }

    [Fact]
    public void Week15_TitleDropsTheWaveFraming()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 15);

        Assert.Equal("Съвременни разширения на КПТ и възстановително-ориентирана терапия", week.Title);
        Assert.DoesNotContain("трета вълна", week.Title, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Трета вълна", week.Title);
    }

    [Fact]
    public void Week15_ObjectivesCoverAllSixApprovedThemes()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 15);
        string joined = string.Join(" | ", week.LearningObjectives);

        Assert.Equal(6, week.LearningObjectives.Count);
        Assert.Contains("семейство от сходни подходи", joined);
        Assert.Contains("диалектическата поведенческа терапия", joined);
        Assert.Contains("CT-R", joined);
        Assert.Contains("адаптивния режим", joined);
        Assert.Contains("CBTp", joined);
        Assert.Contains("доказателствата за CT-R", joined);
    }

    [Fact]
    public void Week15Page_HasPageTitleAndFormatBadge()
    {
        string source = ReadPage("Sedmica15.razor");

        Assert.Contains("<PageTitle>Седмица 15: Съвременни разширения на КПТ и възстановително-ориентирана терапия", source);
        Assert.Contains("@_week.ToPublicLabel()", source);
    }

    [Fact]
    public void Week15Page_UsesOnlyExistingReusablePatterns_AndPassesTheActiveLearningGate()
    {
        string source = ReadPage("Sedmica15.razor");

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
        Assert.Contains("class=\"category-compare\"", source);
        Assert.Contains("class=\"learning-grid learning-grid--balanced\"", source);

        // Active Learning remediation: shared toolkit engines only, in the fixed AcademicThirdPerson mode.
        Assert.Contains("<StatefulModelSimulator", source);
        Assert.Contains("<OrderingBuilder", source);
        Assert.Equal(ActiveLearningSafetyMode.AcademicThirdPerson, ActiveLearningSafety.ModeFor(CourseCatalog.Weeks.Single(w => w.Number == 15).SafetyLevel));

        WeekLearningArchitecture week = ActiveLearningCatalog.For(15);
        Assert.Equal(StructuralStatus.Compliant, week.Status);
        Assert.Empty(ActiveLearningStandard.Evaluate(week));
    }

    [Fact]
    public void Week15Page_SimulatorAndRetrieval_AreSeparateLayers_BeforeTheFinalAssessment()
    {
        string source = ReadPage("Sedmica15.razor");

        int comparison = source.IndexOf("id=\"ct-r-sreshtu-cbtp\"", StringComparison.Ordinal);
        int model = source.IndexOf("<StatefulModelSimulator", StringComparison.Ordinal);
        int beliefs = source.IndexOf("id=\"vyarvaniya\"", StringComparison.Ordinal);
        int review = source.IndexOf("id=\"review\"", StringComparison.Ordinal);
        int retrieval = source.IndexOf("<OrderingBuilder", StringComparison.Ordinal);
        int retrievalDetails = source.IndexOf("<details class=\"progressive-explanation concept-graph__retrieval-check\">", StringComparison.Ordinal);
        int assessment = source.IndexOf("<FinalAssessment", StringComparison.Ordinal);

        Assert.True(model > comparison && model < beliefs, "The two-focus model lives in §07, after the comparison table.");
        Assert.True(retrieval > review && retrieval < assessment, "The stage-ordering retrieval sits at the top of §11, before the Final Assessment.");
        Assert.True(retrieval < retrievalDetails, "The retrieval is a separate layer from the Mind Map review reveal, and comes first.");
        Assert.NotEqual(model, retrieval);
    }

    [Fact]
    public void Week15Page_LearnerFacingMarkupNeverSaysSimulator()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");

        Assert.DoesNotContain("Симулатор", publicMarkup);
        Assert.DoesNotContain("Simulator\"", publicMarkup);
        Assert.Contains("Интерактивен модел: два фокуса на работа", publicMarkup);
    }

    [Fact]
    public void Week15Page_FocusModel_CbtpStateUsesOnlyTheExistingSevenOhSevenCellsVerbatim()
    {
        string source = ReadPage("Sedmica15.razor");

        int modelStart = source.IndexOf("_week15FocusModel = new", StringComparison.Ordinal);
        int modelEnd = source.IndexOf("// Retrieval", modelStart, StringComparison.Ordinal);
        Assert.True(modelStart >= 0 && modelEnd > modelStart);
        string modelBlock = source[modelStart..modelEnd];

        int cbtpStart = modelBlock.IndexOf("new(\"cbtp\"", StringComparison.Ordinal);
        int cbtpEnd = modelBlock.IndexOf("new(\"ctr-1\"", StringComparison.Ordinal);
        Assert.True(cbtpStart >= 0 && cbtpEnd > cbtpStart);
        string cbtpState = modelBlock[cbtpStart..cbtpEnd];

        Assert.Contains("Основен фокус върху намаляване на симптомите — за да се редуцира дистресът и да се подобри качеството на живот.", cbtpState);
        Assert.Contains("Симптомите са централната мишена на интервенцията.", cbtpState);

        // Terminal state: exactly two fields (the two source cells) and an empty choices list.
        Assert.Equal(2, Regex.Matches(cbtpState, "new\\(\"focus-\\d\"").Count);
        Assert.Contains("[])", Regex.Replace(cbtpState, @"\s+", ""));

        string[] forbiddenOutcome = ["по-добър", "по-добра", "превъзхожда", "печели", "губи", "%"];
        foreach (string phrase in forbiddenOutcome)
        {
            Assert.DoesNotContain(phrase, cbtpState);
        }
    }

    [Fact]
    public void Week15Page_FocusModel_CtRPathHasExactlyFourStates_VerbatimStageLabels_NoInventedEffectOnStagesOneAndThree()
    {
        string source = ReadPage("Sedmica15.razor");

        int modelStart = source.IndexOf("_week15FocusModel = new", StringComparison.Ordinal);
        int modelEnd = source.IndexOf("// Retrieval", modelStart, StringComparison.Ordinal);
        Assert.True(modelStart >= 0 && modelEnd > modelStart);
        string modelBlock = source[modelStart..modelEnd];

        // Exactly four CT-R states.
        Assert.Equal(4, Regex.Matches(modelBlock, "new\\(\"ctr-\\d\"").Count);

        string[] stageLabels =
        [
            "Достъп и активиране", "Чрез споделена дейност, изграждаща доверие",
            "Развитие", "Чрез лично значими стремежи",
            "Актуализиране", "Чрез позитивно действие в посока на стремежа",
            "Заздравяване — овластяване и устойчивост, докато адаптивният режим стане водещ"
        ];
        foreach (string label in stageLabels)
        {
            Assert.Contains(label, modelBlock);
        }

        int stage1Start = modelBlock.IndexOf("new(\"ctr-1\"", StringComparison.Ordinal);
        int stage1End = modelBlock.IndexOf("new(\"ctr-2\"", StringComparison.Ordinal);
        string stage1 = modelBlock[stage1Start..stage1End];
        Assert.Equal(2, Regex.Matches(stage1, "new\\(\"stage-").Count); // label split across two fields only — no third, invented field.

        int stage3Start = modelBlock.IndexOf("new(\"ctr-3\"", StringComparison.Ordinal);
        int stage3End = modelBlock.IndexOf("new(\"ctr-4\"", StringComparison.Ordinal);
        string stage3 = modelBlock[stage3Start..stage3End];
        Assert.Equal(2, Regex.Matches(stage3, "new\\(\"stage-").Count);

        // Final CT-R endpoint uses only the existing source/page wording — nothing added after it.
        int stage4Start = modelBlock.IndexOf("new(\"ctr-4\"", StringComparison.Ordinal);
        string stage4 = modelBlock[stage4Start..];
        Assert.Contains("докато адаптивният режим стане водещ", stage4);

        // No outcome, no CBTp-vs-CT-R verdict, no RCT result anywhere in the CT-R path.
        string[] forbiddenOutcome = ["по-добър", "по-добра", "превъзхожда", "по-ефективен", "RCT", "подобри участието", "%"];
        foreach (string phrase in forbiddenOutcome)
        {
            Assert.DoesNotContain(phrase, modelBlock);
        }
    }

    [Fact]
    public void Week15Page_RecoveryProcessRetrieval_HasExactlyTheFourSectionSixStageLabelsVerbatim_NotTheBeliefIllustrations()
    {
        string source = ReadPage("Sedmica15.razor");

        int retrievalStart = source.IndexOf("_week15ProcessOrder = new", StringComparison.Ordinal);
        Assert.True(retrievalStart >= 0);
        string retrievalBlock = source[retrievalStart..];

        string[] stages =
        [
            "Достъп и активиране — чрез споделена дейност, изграждаща доверие",
            "Развитие — чрез лично значими стремежи",
            "Актуализиране — чрез позитивно действие в посока на стремежа",
            "Заздравяване — овластяване и устойчивост, докато адаптивният режим стане водещ"
        ];
        foreach (string stage in stages)
        {
            Assert.Contains(stage, retrievalBlock);
        }

        Assert.Equal(4, Regex.Matches(retrievalBlock, "new\\(\"stage-\\d\"").Count);

        // Never the §08 belief illustrations — those stay static (owner-locked, not a classification prompt).
        Assert.DoesNotContain("Дефеатистки вярвания", retrievalBlock);
        Assert.DoesNotContain("Асоциални вярвания", retrievalBlock);
    }

    [Fact]
    public void Week15Page_BeliefIllustrationsRemainStatic_NoNewClassificationPrompt()
    {
        string source = ReadPage("Sedmica15.razor");
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");

        int beliefsStart = source.IndexOf("id=\"vyarvaniya\"", StringComparison.Ordinal);
        int beliefsEnd = source.IndexOf("id=\"dokazatelstva-i-granitsi\"", StringComparison.Ordinal);
        Assert.True(beliefsStart >= 0 && beliefsEnd > beliefsStart);
        string beliefsSection = source[beliefsStart..beliefsEnd];

        // Unchanged owner-locked static presentation (§08): the section still opens on the plain
        // "category-compare" markup and closes at the next heading — no interactive engine appears
        // anywhere between them — and the page's own disclaimer that this is not a self-classification
        // prompt still stands.
        Assert.Contains("class=\"category-compare\"", beliefsSection);
        Assert.Empty(Regex.Matches(beliefsSection, "<(StatefulModelSimulator|OrderingBuilder|ClassifyMatchCheck|PredictReveal)\\b"));
        Assert.Contains("не пита „коя категория си ти", publicMarkup);
    }

    [Fact]
    public void Week15Page_MindMapPresentInPreviewAndReview_CollapsedByDefaultInReview_NoConceptMapIntroduced()
    {
        string source = ReadPage("Sedmica15.razor");

        int previewIndex = source.IndexOf("<h2 id=\"nakratko\"", StringComparison.Ordinal);
        int reviewIndex = source.IndexOf("<h2 id=\"review\"", StringComparison.Ordinal);
        Assert.True(previewIndex >= 0 && reviewIndex > previewIndex);

        string reviewSection = source[reviewIndex..source.IndexOf("<h2 id=\"assessment\"", StringComparison.Ordinal)];

        Assert.Contains("<details class=\"progressive-explanation concept-graph__retrieval-check\">", reviewSection);
        Assert.DoesNotContain("<details class=\"progressive-explanation concept-graph__retrieval-check\" open>", reviewSection);
        Assert.Contains("<ConceptGraph", reviewSection);

        Assert.Equal(2, Regex.Matches(source, "<ConceptGraph").Count);
        Assert.Equal(2, Regex.Matches(source, @"Model=""@_week15MindMapRender""").Count);

        // Mind Map, not Concept Map — this week introduces no relationship-network representation.
        Assert.DoesNotContain("ConceptMapModel", source);
    }

    [Fact]
    public void Week15Page_MindMapUsesTheSixOwnerApprovedTopLevelBranches()
    {
        string source = ReadPage("Sedmica15.razor");

        int mapStart = source.IndexOf("private static MindMapModel BuildWeek15MindMap", StringComparison.Ordinal);
        int mapEnd = source.IndexOf("]);", mapStart, StringComparison.Ordinal);
        Assert.True(mapStart >= 0 && mapEnd > mapStart);
        string mapBlock = source[mapStart..mapEnd];

        string[] expectedTopLevelBranches =
        [
            "КПТ като семейство", "По-късни адаптации", "Възстановително-ориентирана терапия (CT-R)",
            "Адаптивният режим", "Процесът на CT-R", "Доказателства и граници"
        ];
        foreach (string branch in expectedTopLevelBranches)
        {
            Assert.Contains($"\"{branch}\"", mapBlock);
        }

        // Two children nested under the CT-R branch, not top-level siblings.
        Assert.Contains("\"CT-R срещу CBTp\", \"ctr\"", mapBlock);
        Assert.Contains("\"Две вярвания — мотивация и участие\", \"ctr\"", mapBlock);
    }

    [Fact]
    public void Week15Page_UsesSharedFinalAssessmentWithEightSourceGroundedQuestions()
    {
        string source = ReadPage("Sedmica15.razor");

        Assert.Contains("<FinalAssessment SectionId=\"assessment\" Model=\"_week15FinalAssessment\" />", source);
        Assert.Equal(8, TestPaths.CountFinalAssessmentQuestions(source, "_week15FinalAssessment"));
    }

    [Fact]
    public void Week15Page_WeekCompletionControlIsIndependentOfAssessmentPlacement()
    {
        string source = ReadPage("Sedmica15.razor");

        int assessmentIndex = source.IndexOf("<FinalAssessment", StringComparison.Ordinal);
        int completionIndex = source.IndexOf("<WeekCompletionControl WeekNumber=\"@_week.Number\" />", StringComparison.Ordinal);

        Assert.True(assessmentIndex >= 0 && completionIndex > assessmentIndex);
    }

    [Fact]
    public void Week15Page_SectionNavAnchorsAreRouteSafe()
    {
        string source = ReadPage("Sedmica15.razor");

        string[] anchorIds =
        [
            "nakratko", "kpt-kato-semeystvo", "po-kasni-adaptatsii", "ct-r", "adaptiven-rezhim",
            "protses-na-ct-r", "ct-r-sreshtu-cbtp", "vyarvaniya", "dokazatelstva-i-granitsi",
            "granitsite-na-kursa", "review", "assessment", "izvori"
        ];

        foreach (string id in anchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-15#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
            Assert.Contains($"id=\"{id}\"", source);
        }
    }

    [Fact]
    public void Week15Page_CrossLinksToWeeksOneTwoSevenThirteenAndTheHub_NoDeadLinks()
    {
        string source = ReadPage("Sedmica15.razor");

        Assert.Contains("/kurs/sedmica-1", source);
        Assert.Contains("/kurs/sedmica-2", source);
        Assert.Contains("/kurs/sedmica-7", source);
        Assert.Contains("/kurs/sedmica-13", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Week15Page_MbctIsExcludedEntirely()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");

        Assert.DoesNotContain("MBCT", publicMarkup);
        Assert.DoesNotContain("базирана на осъзнатост", publicMarkup);
        Assert.DoesNotContain("основана на осъзнатост", publicMarkup);
    }

    [Fact]
    public void Week15Page_ThirdWaveLabelIsNeverASectionHeadingAndIsExplicitlyCaveated()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");
        string normalized = Regex.Replace(publicMarkup, @"\s+", " ");

        // The label is introduced and caveated exactly once, in prose (§03) — never promoted to a
        // section heading, which would make it the page's organizing structure.
        Assert.DoesNotContain("<h2 id=\"treta-valna\"", publicMarkup);
        foreach (Match headingMatch in Regex.Matches(publicMarkup, @"<h2[^>]*>(.*?)</h2>"))
        {
            Assert.DoesNotContain("трета вълна", headingMatch.Groups[1].Value, StringComparison.OrdinalIgnoreCase);
        }
        Assert.Contains("не използва това понятие като организираща структура", normalized);

        // No decade ranges anywhere — the owner-directed exclusion of C2/C3/C4.
        string[] forbiddenDecadeMarkers = ["1920", "1950-те до 1970", "1980-те", "1990-те"];
        foreach (string marker in forbiddenDecadeMarkers)
        {
            Assert.DoesNotContain(marker, publicMarkup);
        }
    }

    [Fact]
    public void Week15Page_DbtAndActAreNamedOnly_NoMiniLesson()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");

        Assert.Contains("диалектическа поведенческа терапия", publicMarkup);
        Assert.Contains("терапия на приемане и ангажираност", publicMarkup);

        // No technique-level DBT/ACT teaching content should appear.
        Assert.DoesNotContain("диалектически синтез", publicMarkup);
        Assert.DoesNotContain("умения за преживяване на дистрес", publicMarkup);
        Assert.DoesNotContain("шестте процеса на психологическа гъвкавост", publicMarkup);
    }

    [Fact]
    public void Week15Page_CtRBeliefCategoriesAreNamedOnly_NoSelfClassificationPrompt()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");

        Assert.Contains("Дефеатистки вярвания за собствената ефективност", publicMarkup);
        Assert.Contains("Асоциални вярвания", publicMarkup);
        Assert.Contains("не диагностичен въпросник", publicMarkup);

        // The page explicitly disclaims asking this, in a negated sentence (Week 12 precedent) —
        // it must never appear as a bare, unnegated prompt.
        Assert.Contains("не пита „коя категория си ти", publicMarkup);
        Assert.DoesNotContain("Кой тип вярване имате", publicMarkup);
    }

    [Fact]
    public void Week15Page_HasNoSelfInputFormsOrSelfGuidedCtR()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");

        Assert.DoesNotContain("<input", publicMarkup);
        Assert.DoesNotContain("<textarea", publicMarkup);
        Assert.DoesNotContain("<form", publicMarkup);
        Assert.DoesNotContain("type=\"range\"", publicMarkup);

        Assert.Contains("не е инструмент за самооценка", publicMarkup);
        Assert.Contains("изисква супервизирано професионално обучение", publicMarkup);
    }

    [Fact]
    public void Week15Page_HasNoPsychosisOrSevereMentalIllnessSelfAssessment()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");

        // Severe mental illness/psychosis appears only as a population description, never a
        // symptom checklist or a self-recognition prompt.
        Assert.DoesNotContain("Изпитвате ли гласове", publicMarkup);
        Assert.DoesNotContain("Имате ли параноя", publicMarkup);
        Assert.DoesNotContain("Проверете вашите симптоми", publicMarkup);
    }

    [Fact]
    public void Week15Page_DisclaimerCoversNoPsychiatricSelfAssessment()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");

        Assert.Contains("не оценява психично състояние", publicMarkup);
    }

    [Fact]
    public void Week15Page_HasNoChapter21SelfApplicationProtocol()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");

        string[] chapter21Markers =
        [
            "попълнете Работен лист за основни вярвания",
            "Направете по един Запис на мислите на ден",
            "Скала и Ръководство за когнитивна терапия",
            "изберете прост, несложен пациент"
        ];
        foreach (string marker in chapter21Markers)
        {
            Assert.DoesNotContain(marker, publicMarkup);
        }
    }

    [Fact]
    public void Week15Page_TherapistDevelopmentAndGap014RemainUnclaimed()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");

        // C15-K05 (therapists developing their own techniques) is not taught here.
        Assert.DoesNotContain("терапевтите развиват собствени техники", publicMarkup);

        // GAP-014's remaining "highlighting the positive" half is not silently absorbed here.
        Assert.DoesNotContain("подчертаване на положителното", publicMarkup);
    }

    [Fact]
    public void Week15Page_NoSallyOrInventedCase()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");

        Assert.DoesNotContain("Сали", publicMarkup);
        Assert.DoesNotContain("Терапевт:", publicMarkup);
        Assert.DoesNotContain("Пациент:", publicMarkup);
    }

    [Fact]
    public void Week15Page_CtREvidenceStaysNonSuperioritySafe()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");
        string normalized = Regex.Replace(publicMarkup, @"\s+", " ");

        Assert.Contains("може да няма разлика", normalized);
        Assert.Contains("не твърди, че по-новите подходи са по същество по-добри", normalized);
        Assert.Contains("не универсално доказан извод", normalized);
    }

    [Fact]
    public void Week15Page_HasNoInternalDevelopmentLanguageInItsRenderableMarkup()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");

        string[] forbiddenTerms =
        [
            "11_SOURCE_REGISTER.md", "kpt_syllabus.pdf", "citation-grade", "WEEK_15_SOURCE_AUDIT",
            "10_SESSION_LOG.md", "Project OS", "code_artifact.html", "Needs Review", "KU", "GAP-011", "GAP-014"
        ];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }
    }

    [Fact]
    public void Week15Page_MakesNoFalseAccreditationClaims()
    {
        string source = ReadPage("Sedmica15.razor");

        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week15Page_HasNoDiagnosticOrClinicalScoringContent()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica15.razor");

        string[] forbiddenTerms = ["BDI", "BAI", "BHS", "диагностичен инструмент"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }

        Assert.Contains("не поставя диагноза", publicMarkup);
    }

    [Fact]
    public void Week15Page_HasNoPageLevelOverflowWorkaroundOrInlineStyles()
    {
        string source = ReadPage("Sedmica15.razor");

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
