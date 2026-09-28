using System.Reflection;
using System.Text.RegularExpressions;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>SYSTEMATIC CURRICULUM EXPANSION, Phase B, Batch A — Week 12 ("Основни вярвания и
/// схеми"), second clean-source implementation candidate in the frozen build order (6→12→...).
/// "Concept and Diagram" archetype: a recap-and-link to Week 3's cognitive hierarchy, deepened
/// into the three broad categories the source describes for negative core beliefs. AcademicOnly
/// safety level — zero interaction, no self-assessment, resolves to CourseWeekStatus.AcademicOverview
/// even though routed (see CurriculumLabels.DeriveStatus). Source: SRC-041 (Judith Beck), Глава 14 —
/// confirmed via session log narration, scoped specifically to negative core beliefs' three
/// categories (безпомощност/необичаемост/безполезност), not core beliefs in general.</summary>
public sealed class Week12ContentSliceTests
{
    [Fact]
    public void Week12Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica12"));
    }

    [Fact]
    public void Week12_MetadataIsRoutedButAcademicOverviewNotAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 12);

        Assert.Equal("/kurs/sedmica-12", week.Route);
        Assert.Equal(CourseWeekStatus.AcademicOverview, week.Status);
        Assert.NotEqual(CourseWeekStatus.Available, week.Status);
    }

    [Fact]
    public void Week1Week3Week6Week8Week10_RemainAvailableAfterWeek12WasAdded()
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
    public void RemainingWeeks_StayFullyUnrouted()
    {
        // Weeks 11, 13, 14 and 15 joined the routed set after this test was first written
        // (WEEK_11_SOURCE_AUDIT_v1, WEEK_13_SOURCE_AUDIT_v1, WEEK_14_SOURCE_AUDIT_v1,
        // WEEK_15_SOURCE_AUDIT_v1, all owner-approved implementations) — updated here only to keep
        // this global routing invariant accurate; no Week 12 content/approval changed. Week 15 was
        // the last unrouted week, so the catalog now has zero unrouted weeks.
        int[] routedNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];

        foreach (CourseWeekDefinition week in CourseCatalog.Weeks.Where(w => !routedNumbers.Contains(w.Number)))
        {
            Assert.Null(week.Route);
            Assert.NotEqual(CourseWeekStatus.Available, week.Status);
        }

        Assert.Equal(0, CourseCatalog.Weeks.Count(w => !routedNumbers.Contains(w.Number)));
    }

    [Fact]
    public void Week12Page_HasPageTitleAndFormatBadge()
    {
        string source = ReadPage("Sedmica12.razor");

        Assert.Contains("<PageTitle>Седмица 12: Основни вярвания и схеми", source);
        Assert.Contains("@_week.ToPublicLabel()", source);
    }

    [Fact]
    public void Week12Page_UsesOnlyExistingReusablePatterns_AndPassesTheActiveLearningGate()
    {
        string source = ReadPage("Sedmica12.razor");

        Assert.Contains("<LearningSection", source);
        Assert.Contains("<LearningObjectives", source);
        Assert.Contains("<ProgressiveExplanation", source);
        Assert.Contains("<DisclaimerCallout", source);
        Assert.Contains("<SourceReferences", source);
        Assert.Contains("<OptionalReadingSource", source);
        Assert.Contains("class=\"category-compare\"", source);
        Assert.Contains("class=\"learning-grid learning-grid--balanced\"", source);

        // Active Learning remediation: shared toolkit engines only, in the fixed AcademicThirdPerson mode.
        Assert.Contains("<ConceptGraph", source);
        Assert.Contains("<StatefulModelSimulator", source);
        Assert.Contains("<ClassifyMatchCheck", source);
        Assert.Equal(ActiveLearningSafetyMode.AcademicThirdPerson, ActiveLearningSafety.ModeFor(CourseCatalog.Weeks.Single(w => w.Number == 12).SafetyLevel));

        WeekLearningArchitecture week = ActiveLearningCatalog.For(12);
        Assert.Equal(StructuralStatus.Compliant, week.Status);
        Assert.Empty(ActiveLearningStandard.Evaluate(week));
    }

    [Fact]
    public void Week12Page_MindMapPreviewAndReview_UseTheSameModel_NoWeek3HierarchyNodes()
    {
        string source = ReadPage("Sedmica12.razor");

        Assert.Equal(2, Regex.Matches(source, "<ConceptGraph").Count);
        Assert.Equal(2, Regex.Matches(source, @"Model=""@_week12MindMapRender""").Count);

        int mapStart = source.IndexOf("private static MindMapModel BuildWeek12MindMap", StringComparison.Ordinal);
        int mapEnd = source.IndexOf("]);", mapStart, StringComparison.Ordinal);
        Assert.True(mapStart >= 0 && mapEnd > mapStart);
        string mapBlock = source[mapStart..mapEnd];

        Assert.DoesNotContain("\"Автоматична мисъл\"", mapBlock);
        Assert.DoesNotContain("\"Междинно вярване\"", mapBlock);

        string[] expectedBranches =
        [
            "Схема и основно вярване", "Ранен произход", "Трите категории",
            "Поддържане чрез филтъра", "Развитие на по-адаптивно вярване", "Академична граница"
        ];
        foreach (string branch in expectedBranches)
        {
            Assert.Contains($"\"{branch}\"", mapBlock);
        }
    }

    [Fact]
    public void Week12Page_SimulatorAndRetrieval_AreSeparateLayers_BeforeTheFinalAssessment()
    {
        string source = ReadPage("Sedmica12.razor");

        int kategorii = source.IndexOf("id=\"trite-kategorii\"", StringComparison.Ordinal);
        int simulator = source.IndexOf("<StatefulModelSimulator", StringComparison.Ordinal);
        int razvitie = source.IndexOf("id=\"razvitie-na-novo-vyarvane\"", StringComparison.Ordinal);
        int retrieval = source.IndexOf("<ClassifyMatchCheck", StringComparison.Ordinal);
        int assessment = source.IndexOf("<FinalAssessment", StringComparison.Ordinal);

        Assert.True(simulator > kategorii && simulator < razvitie, "The ambiguity simulator lives in §03.");
        Assert.True(retrieval > razvitie && retrieval < assessment, "The retrieval match follows §05, before the Final Assessment.");
        Assert.NotEqual(simulator, retrieval);
    }

    [Fact]
    public void Week12Page_CategoryAmbiguitySimulator_HasExactlyTwoSourceSupportedBranches_NoWorthlessnessBranch()
    {
        string source = ReadPage("Sedmica12.razor");

        int simulatorStart = source.IndexOf("_week12CategoryAmbiguity = new", StringComparison.Ordinal);
        int simulatorEnd = source.IndexOf("\n    // Retrieval", simulatorStart, StringComparison.Ordinal);
        Assert.True(simulatorStart >= 0 && simulatorEnd > simulatorStart);
        string simulatorBlock = source[simulatorStart..simulatorEnd];

        Assert.Contains("Не съм достатъчно способен, за да постигна", simulatorBlock);
        Assert.Contains("Безпомощност", simulatorBlock);
        Assert.Contains("Не съм достатъчно добър, за да бъда обичан", simulatorBlock);
        Assert.Contains("Необичаемост", simulatorBlock);

        // Exactly two branch choices ("new(" state-transition entries under the initial state) — no third
        // Безполезност branch, since the source never links this specific phrase to that category.
        Assert.DoesNotContain("Безполезност", simulatorBlock);
        Assert.Equal(2, Regex.Matches(simulatorBlock, @"new\(""[a-z]+"", "".*"", (?:""helplessness""|""unlovability"")").Count);

        // Never Sally, never Annie, never the learner's own belief.
        Assert.DoesNotContain("Сали", simulatorBlock);
        Assert.DoesNotContain("Анни", simulatorBlock);
        Assert.DoesNotContain("твоето вярване", simulatorBlock);
    }

    [Fact]
    public void Week12Page_RetrievalMatch_UsesVerbatimSection03Definitions_NeverSection05Phrases()
    {
        string source = ReadPage("Sedmica12.razor");

        // The §03 cards wrap these sentences across markup lines, so compare with whitespace normalized rather than
        // byte-for-byte (the words themselves are identical to each retrieval item's single-line definition).
        string normalized = Regex.Replace(source, @"\s+", " ");
        foreach (string definition in new[]
        {
            "Теми около неефективност — невъзможност да се справиш с нещата, да защитиш себе си или да постигнеш нещо съществено.",
            "Теми, при които ефективността и обичта отстъпват на заден план — усещане, че си лош, недостоен или дори опасен за другите.",
            """Теми около неприемливост, нежеланост или "дефект" в характера — усещане, че не можеш да получиш трайна любов и грижа от другите (не заради постижения или морал, а заради самата си същност)."""
        })
        {
            Assert.Equal(2, Regex.Matches(normalized, Regex.Escape(definition)).Count);
        }

        // Retrieval never classifies the §05 old-belief phrases (that would read as a self-recognition tool).
        string retrievalStart = source[source.IndexOf("_week12CategoryMatch = new", StringComparison.Ordinal)..];
        string retrievalBlock = retrievalStart[..retrievalStart.IndexOf("\n\n    // Weekly Final Assessment", StringComparison.Ordinal)];
        Assert.DoesNotContain("Аз съм (напълно) неслюбим", retrievalBlock);
        Assert.DoesNotContain("Аз съм лош", retrievalBlock);
    }

    [Fact]
    public void Week12Page_ClosingSectionAvoidsWeek3sSingleChildBalancedGridAntiPattern()
    {
        // Week 3's known, deliberately-unfixed defect: the balanced grid's only child is a
        // single <LearningSection>. Week 12 must follow the corrected shape instead (matching
        // Week 6 / the fixed Week 10): the grid's immediate children are plain <div> cards, and
        // OptionalReadingSource sits as a full-width sibling after the grid closes.
        string source = ReadPage("Sedmica12.razor");

        int sectionStart = source.IndexOf("id=\"izvori\"", StringComparison.Ordinal);
        Assert.True(sectionStart >= 0);

        int gridStart = source.IndexOf("class=\"learning-grid learning-grid--balanced\"", sectionStart, StringComparison.Ordinal);
        Assert.True(gridStart >= 0);

        int afterOpenTag = source.IndexOf('>', gridStart) + 1;
        string afterGrid = source[afterOpenTag..].TrimStart();

        Assert.StartsWith("<div>", afterGrid);

        int optionalReadingIndex = source.IndexOf("<OptionalReadingSource", StringComparison.Ordinal);
        Assert.True(optionalReadingIndex > afterOpenTag);
    }

    [Fact]
    public void Week12Page_DescribesTheThreeConfirmedNegativeCoreBeliefCategories()
    {
        string source = ReadPage("Sedmica12.razor");

        Assert.Contains("Безпомощност", source);
        Assert.Contains("Необичаемост", source);
        Assert.Contains("Безполезност", source);
    }

    [Fact]
    public void Week12Page_ScopesTheCategoriesToNegativeCoreBeliefsSpecifically_NoOverclaim()
    {
        // Session log confirms the three categories for NEGATIVE core beliefs specifically —
        // the page must not present them as if every core belief falls into one of these three.
        string source = ReadPage("Sedmica12.razor");

        Assert.Contains("не са задължително отрицателни", source);
        Assert.Contains("<strong>негативните</strong> основни", source);
    }

    [Fact]
    public void Week12Page_RecapsWeek3WithoutReRenderingItsDiagram()
    {
        // Duplication discipline: the 3-level hierarchy is Week 3's own content — Week 12 links
        // to it rather than reproducing the .learning-path-diagram markup.
        string source = ReadPage("Sedmica12.razor");

        Assert.Contains("/kurs/sedmica-3", source);
        Assert.DoesNotContain("learning-path-diagram", source);
    }

    [Fact]
    public void Week12Page_HasExplicitAcademicOnlyNoSelfAssessmentBoundary()
    {
        string source = ReadPage("Sedmica12.razor");

        Assert.Contains("коя категория си ти", source);
        Assert.Contains("академичен обзор", source);
    }

    [Fact]
    public void Week12Page_HasNoInternalDevelopmentLanguageInItsRenderableMarkup()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica12.razor");

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
    public void Week12Page_MakesNoFalseAccreditationClaims()
    {
        string source = ReadPage("Sedmica12.razor");

        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week12Page_HasNoDiagnosticOrClinicalScoringContent()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica12.razor");

        string[] forbiddenTerms = ["BDI", "BAI", "BHS", "терапевтичен план", "диагностичен инструмент"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }

        // The page must instead explicitly deny diagnostic framing, not just avoid the term.
        Assert.Contains("не поставя диагноза", publicMarkup);
    }

    [Fact]
    public void Week12Page_SectionNavAnchorsAreRouteSafe()
    {
        string source = ReadPage("Sedmica12.razor");

        string[] anchorIds =
        [
            "niva", "osnovno-vyarvane", "trite-kategorii", "poddarzhane-na-vyarvaneto",
            "razvitie-na-novo-vyarvane", "akademichen-obzor", "review", "proverka", "izvori"
        ];

        foreach (string id in anchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-12#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
            Assert.Contains($"id=\"{id}\"", source);
        }
    }

    [Fact]
    public void Week12Page_CrossLinksToWeek3AndTheHub_NoDeadLinks()
    {
        string source = ReadPage("Sedmica12.razor");

        Assert.Contains("/kurs/sedmica-3", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Week12Page_DoesNotLinkToWeek11AsIfAvailable()
    {
        // Week 11 has not been implemented yet — not in the frozen build order's next position.
        string source = ReadPage("Sedmica12.razor");

        Assert.DoesNotContain("/kurs/sedmica-11", source);
    }

    [Fact]
    public void KursPage_StartPanelStillListsOnlyTheNineTrulyAvailableWeeks()
    {
        // Week 12 is AcademicOverview, not Available — it must not be added to the start-panel
        // alongside Weeks 1, 2, 3, 5, 6, 7, 8, 9, 10.
        string source = ReadPage("Kurs.razor");

        Assert.Contains("/kurs/sedmica-1", source);
        Assert.Contains("/kurs/sedmica-2", source);
        Assert.Contains("/kurs/sedmica-3", source);
        Assert.Contains("/kurs/sedmica-5", source);
        Assert.Contains("/kurs/sedmica-6", source);
        Assert.Contains("/kurs/sedmica-7", source);
        Assert.Contains("/kurs/sedmica-8", source);
        Assert.Contains("/kurs/sedmica-9", source);
        Assert.Contains("/kurs/sedmica-10", source);
        Assert.Contains("Девет седмици", source);

        int startPanelStart = source.IndexOf("start-panel", StringComparison.Ordinal);
        int startPanelEnd = source.IndexOf("</div>", startPanelStart, StringComparison.Ordinal);
        Assert.DoesNotContain("/kurs/sedmica-12", source[startPanelStart..startPanelEnd]);
    }

    [Fact]
    public void Week12Page_HasNoPageLevelOverflowWorkaround()
    {
        string source = ReadPage("Sedmica12.razor");

        Assert.DoesNotContain("overflow-x: hidden", source);
        Assert.DoesNotContain("overflow-x:hidden", source);
    }

    [Fact]
    public void Week12Page_HasNoInlineStylesOrAbsolutePositioning()
    {
        string source = ReadPage("Sedmica12.razor");

        Assert.DoesNotContain("style=", source);
        Assert.DoesNotContain("position: absolute", source);
        Assert.DoesNotContain("position:absolute", source);
    }

    // ---- WEEK_12_RETROFIT_AUDIT_v1 (owner-approved, conservative AcademicContextOnly retrofit) ----

    [Fact]
    public void Week12Page_ExplainsSchemaVsCoreBeliefDistinction()
    {
        string source = ReadPage("Sedmica12.razor");

        Assert.Contains("Бек разграничава", source);
        Assert.Contains("когнитивна структура", source);
    }

    [Fact]
    public void Week12Page_ExplainsTheAbstractScreenMechanism_WithoutSelfAssessment()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica12.razor");

        Assert.Contains("id=\"poddarzhane-na-vyarvaneto\"", publicMarkup);
        Assert.Contains("филтър", publicMarkup);

        // Abstract explanatory concept only — no invitation to apply it to oneself.
        Assert.DoesNotContain("твоя", publicMarkup);
        Assert.DoesNotContain("твоето вярване", publicMarkup);
        Assert.DoesNotContain("твоята схема", publicMarkup);
    }

    [Fact]
    public void Week12Page_DescribesDevelopingANewBelief_DescriptivelyNotAsSelfTreatmentSteps()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica12.razor");

        Assert.Contains("id=\"razvitie-na-novo-vyarvane\"", publicMarkup);
        Assert.Contains("терапевтичния процес", publicMarkup);
        Assert.Contains("comparison-matrix", publicMarkup);
        Assert.Contains("\"Аз съм безсилен.\"", publicMarkup);
        Assert.Contains("\"Имам контрол над много неща.\"", publicMarkup);

        // Descriptive/academic register only — never a second-person instruction to act.
        string[] selfTreatmentPhrases = ["Направете", "Опитайте", "Запишете", "Проследете вашето"];
        foreach (string phrase in selfTreatmentPhrases)
        {
            Assert.DoesNotContain(phrase, publicMarkup);
        }
    }

    [Fact]
    public void Week12Page_DoesNotReproduceFigure14_1AsASelfRecognitionChecklist()
    {
        // Owner decision: enrich category *themes* (U13), but do NOT reproduce Figure 14.1's
        // full concrete belief-phrase lists (U14) — that would read as a self-recognition tool.
        string publicMarkup = ReadPublicMarkup("Sedmica12.razor");

        string[] figure141OnlyPhrases =
        [
            "Аз съм жертва", "Аз съм извън контрол", "Аз съм хванат в капан",
            "Аз съм токсичен", "Не заслужавам да живея", "предопределен да бъда отхвърлен"
        ];

        foreach (string phrase in figure141OnlyPhrases)
        {
            Assert.DoesNotContain(phrase, publicMarkup);
        }
    }

    [Fact]
    public void Week12Page_ExcludesTheClinicalTechniqueCatalog()
    {
        // WEEK_12_RETROFIT_AUDIT_v1 §0 final accounting keeps 22 KUs Excluded — the entire
        // therapist-led technique/case-demonstration catalog stays off this AcademicContextOnly page.
        string publicMarkup = ReadPublicMarkup("Sedmica12.razor");

        string[] excludedTechniqueTerms =
        [
            "Работен лист за основни вярвания", "екстремни контрасти", "исторически тест",
            "реструктуриране на ранни спомени", "Сали", "Анни"
        ];

        foreach (string term in excludedTechniqueTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }
    }

    [Fact]
    public void Week12Page_HasFiveComprehensionQuestions_NewOnesTestConceptsNotSelfDiagnosis()
    {
        string source = ReadPage("Sedmica12.razor");

        Assert.Equal(5, TestPaths.CountFinalAssessmentQuestions(source, "_week12FinalAssessment"));
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
