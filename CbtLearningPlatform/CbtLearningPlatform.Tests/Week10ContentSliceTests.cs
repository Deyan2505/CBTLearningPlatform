using System.Reflection;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>CONTENT-DRIVEN TEMPLATE VALIDATION, Slice 3 — Week 10 ("Guided Practice"
/// archetype), distinct from Week 1's "Theory and History", Week 3's "Concept and Diagram",
/// and Week 8's "Simulator Workspace".</summary>
public sealed class Week10ContentSliceTests
{
    [Fact]
    public void Week10Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica10"));
    }

    [Fact]
    public void Week10_MetadataIsAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 10);

        Assert.Equal(CourseWeekStatus.Available, week.Status);
        Assert.Equal("/kurs/sedmica-10", week.Route);
    }

    [Fact]
    public void Week1Week3Week8_RemainAvailableAfterWeek10WasAdded()
    {
        int[] weeksToCheck = [1, 3, 8];

        foreach (int number in weeksToCheck)
        {
            CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == number);
            Assert.Equal(CourseWeekStatus.Available, week.Status);
            Assert.Equal($"/kurs/sedmica-{number}", week.Route);
        }
    }

    [Fact]
    public void RemainingEightWeeks_StayUnavailable()
    {
        int[] availableNumbers = [1, 2, 3, 6, 7, 8, 9, 10];

        foreach (CourseWeekDefinition week in CourseCatalog.Weeks.Where(w => !availableNumbers.Contains(w.Number)))
        {
            Assert.NotEqual(CourseWeekStatus.Available, week.Status);

            // Weeks 4 and 12 are AcademicContextOnly: they later gained a real, routed
            // AcademicOverview page without becoming Available — every other non-available week
            // still has no route.
            if (week.Number != 4 && week.Number != 12)
            {
                Assert.Null(week.Route);
            }
        }

        Assert.Equal(7, CourseCatalog.Weeks.Count(w => !availableNumbers.Contains(w.Number)));
    }

    [Fact]
    public void Week10Page_HasPageTitleAndFormatBadge()
    {
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains("<PageTitle>Седмица 10: Сократически въпроси и съвместно изследване", source);
        Assert.Contains("Насочено упражнение", source);
    }

    [Fact]
    public void Week10Page_HasAllSixEvaluationQuestionCategories()
    {
        // Retrofit migration: the old "four families" compression is replaced by SRC-041 Chapter
        // 11's real six categories (Figure 11.1), matching what Week 9's recap already promises.
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains(">Доказателства<", source);
        Assert.Contains(">Алтернативно обяснение<", source);
        Assert.Contains(">Декатастрофизиране<", source);
        Assert.Contains(">Ефект от вярването<", source);
        Assert.Contains(">Дистанциране<", source);
        Assert.Contains(">Решаване на проблема<", source);
    }

    [Fact]
    public void Week10Page_SixCategoriesHaveTheSourcesFullSubQuestions()
    {
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains("Какви са доказателствата, които подкрепят тази идея?", source);
        Assert.Contains("Какви са доказателствата срещу тази идея?", source);
        Assert.Contains("Има ли алтернативно обяснение или гледна точка?", source);
        Assert.Contains("Какво е най-лошото, което може да се случи?", source);
        Assert.Contains("Какъв е най-реалистичният резултат?", source);
        Assert.Contains("Какъв е ефектът от това, че вярвам в автоматичната мисъл?", source);
        Assert.Contains("Какво трябва да направя?", source);
    }

    [Fact]
    public void Week10Page_HasExactlyOneInteractiveIsland()
    {
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains("<SocraticDialogueExplorer", source);
        Assert.DoesNotContain("<CbtChainSimulator", source);
        Assert.DoesNotContain("<CognitiveHierarchyExplorer", source);
        Assert.DoesNotContain("<SchemaFilterDemonstration", source);
        Assert.DoesNotContain("<ResearchTurnStepper", source);
    }

    [Fact]
    public void SocraticDialogueExplorer_UsesAFixedScenario_NoFreeTextOrPersonalDataInput()
    {
        // The retold percentages (belief 90%→20%, matching the source's own re-rating) are static
        // narrative text describing the fixed case, not a live scoring control — an interactive
        // input/slider/textarea is what this test actually guards against.
        string source = ReadClientComponent("SocraticDialogueExplorer.razor");

        Assert.DoesNotContain("<input", source);
        Assert.DoesNotContain("<textarea", source);
        Assert.DoesNotContain("localStorage", source);
        Assert.DoesNotContain("sessionStorage", source);
        Assert.DoesNotContain("HttpClient", source);
    }

    [Fact]
    public void SocraticDialogueExplorer_UsesSaliAndKarenSourceGroundedCase_NotTheOldInventedScenario()
    {
        string source = ReadClientComponent("SocraticDialogueExplorer.razor");

        Assert.Contains("Сали", source);
        Assert.Contains("Карен", source);
        Assert.Contains("Тя всъщност не се интересува какво ще ми се случи", source);
        Assert.DoesNotContain("Изпращате подготвен материал", source);
        Assert.DoesNotContain("Сигурно материалът е лош", source);
    }

    [Fact]
    public void SocraticDialogueExplorer_HasAllSixCategorySteps_InSourceOrder()
    {
        string source = ReadClientComponent("SocraticDialogueExplorer.razor");

        string[] stepsInOrder =
        [
            "Доказателства", "Алтернативно обяснение", "Декатастрофизиране",
            "Ефект от вярването", "Дистанциране", "Решаване на проблема"
        ];

        int lastIndex = -1;
        foreach (string step in stepsInOrder)
        {
            int index = source.IndexOf($"new(\"{step}\"", StringComparison.Ordinal);
            Assert.True(index > lastIndex, $"'{step}' must appear after the previous step in declaration order.");
            lastIndex = index;
        }
    }

    [Fact]
    public void Week10Page_LeadingQuestionComparisonHasAllFourExamples()
    {
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains("Насочващ въпрос", source);
        Assert.Contains("Изследващ въпрос", source);
        Assert.Contains("Съвет, представен като въпрос", source);
        Assert.Contains("Въпрос за алтернативно обяснение", source);
    }

    [Fact]
    public void Week10Page_HasFactAssumptionConclusionDistinction()
    {
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains(">Факт<", source);
        Assert.Contains(">Предположение<", source);
        Assert.Contains(">Заключение<", source);
        Assert.Contains("вярна част, възможна част и непотвърдена част", source);
    }

    [Fact]
    public void Week10Page_DecatastrophizingDoesNotDenyTheProblem()
    {
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains("не твърди, че няма проблем", source);
    }

    [Fact]
    public void Week10Page_AdaptiveResponseIsNotForcedPositivity()
    {
        // Terminology migration: canonical term is now "адаптивен отговор" (first occurrence reads
        // "адаптивен (балансиран) отговор" for continuity), per the owner's decision — the concept
        // itself (not forced positivity) is unchanged.
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains("Адаптивен (балансиран) отговор", source);
        Assert.Contains("Принудително положително", source);
        Assert.Contains("правдоподобен и съвместим с фактите, а не просто по-приятен", source);
    }

    [Fact]
    public void Week10Page_HasSixKnowledgeCheckQuestions()
    {
        string source = ReadPage("Sedmica10.razor");

        for (int i = 1; i <= 6; i++)
        {
            Assert.Contains($"Въпрос {i}", source);
        }
        Assert.Contains("Проверката не се оценява и не запазва отговори", source);
    }

    [Fact]
    public void Week10Page_HasEducationalDisclaimerAndLearnerFacingAcademicContext()
    {
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains("<DisclaimerCallout", source);
        Assert.DoesNotContain("Variant=\"safety\"", source);
        Assert.Contains("<h3>Академичен контекст</h3>", source);
        Assert.Contains("предстои да премине независим академичен и професионален преглед", source);
    }

    [Fact]
    public void Week10Page_HasNoInternalDevelopmentLanguageInItsRenderableMarkup()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica10.razor");

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
    public void Week10Page_MakesNoFalseAccreditationClaims()
    {
        string source = ReadPage("Sedmica10.razor");

        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week10Page_HasNoDiagnosticContent()
    {
        string source = ReadPage("Sedmica10.razor");

        string[] forbiddenTerms = ["BDI", "BAI", "BHS", "диагности"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week10Page_HasNoClinicalTrainingLanguage()
    {
        string source = ReadPage("Sedmica10.razor");

        string[] forbiddenTerms =
        [
            "водене на мислите на пациента", "овладяване на терапевтична техника",
            "когнитивно преструктуриране на пациента", "убедете човека, че мисълта му е грешна"
        ];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week10Page_SectionNavAnchorsAreRouteSafe()
    {
        string source = ReadPage("Sedmica10.razor");

        string[] anchorIds =
        [
            "karta", "izsledvane", "kategorii", "sluchay-sali-karen", "prikrit-savet",
            "fakti-zakliucheniya", "dekatastrofizirane", "adaptiven-otgovor", "flow",
            "samostoyatelno", "proverka", "review-map", "izvori"
        ];

        foreach (string id in anchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-10#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
            Assert.Contains($"id=\"{id}\"", source);
        }
    }

    [Fact]
    public void Week10Page_CrossLinksToWeek3Week8Week9AndKurs_NoDeadLinks()
    {
        // Retrofit: the old Module 2 lesson links (pre-CourseCatalog era) are replaced by real
        // routed weeks — Week 9 specifically, to preserve the boundary (distortions/Thought Record
        // stay there) instead of duplicating.
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains("/kurs/sedmica-3", source);
        Assert.Contains("/kurs/sedmica-8", source);
        Assert.Contains("/kurs/sedmica-9", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Week10Page_DoesNotLinkToWeek11AsIfAvailable()
    {
        string source = ReadPage("Sedmica10.razor");

        Assert.DoesNotContain("/kurs/sedmica-11", source);
    }

    [Fact]
    public void Week8Page_CrossLinksToWeek10_WithoutDuplicatingTheSimulator()
    {
        string week8Source = ReadPage("Sedmica8.razor");

        Assert.Contains("/kurs/sedmica-10", week8Source);

        // The simulator itself remains unique to Week 8 — not duplicated on Week 10.
        Assert.Contains("<CbtChainSimulator", week8Source);
        Assert.DoesNotContain("<SocraticDialogueExplorer", week8Source);
    }

    [Fact]
    public void KursPage_ShowsAllFourAvailableWeeks()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("/kurs/sedmica-1", source);
        Assert.Contains("/kurs/sedmica-3", source);
        Assert.Contains("/kurs/sedmica-8", source);
        Assert.Contains("/kurs/sedmica-10", source);
    }

    [Fact]
    public void Week10Page_HasNoPageLevelOverflowWorkaround()
    {
        string source = ReadPage("Sedmica10.razor");

        Assert.DoesNotContain("overflow-x: hidden", source);
        Assert.DoesNotContain("overflow-x:hidden", source);
    }

    [Fact]
    public void Week10Page_H1HasNoPageSpecificFocusOverride()
    {
        // Owner review: h1 must rely solely on the shared, global heading-focus contract in
        // app.css (verified separately in LayoutDefectFixTests) — no inline style/class here
        // that could reintroduce a control-style ring just for this page.
        string source = ReadPage("Sedmica10.razor");

        Assert.DoesNotContain("<h1 style=", source);
        Assert.DoesNotContain("<h1 class=", source);
        Assert.Matches(new System.Text.RegularExpressions.Regex("<h1>"), source);
    }

    [Fact]
    public void Week10Page_Section01UsesTheDedicatedProcessLayout_NotTheDefaultNarrowFlow()
    {
        // Owner review: the shared .concept-map__flow (narrow ~26rem column, correct for its
        // many other 2-5-step uses) wasted the full workspace width for this page's 6-step
        // process. Scoped modifier only — other .concept-map__flow uses must stay untouched.
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains("class=\"concept-map__flow concept-map__flow--process\"", source);

        string cssSource = ReadCss();
        Assert.Contains(".concept-map__flow--process", cssSource);
    }

    [Fact]
    public void Week10Page_Section01ProcessHasExactlySixStepsInDomOrder_NoTrailingConnector()
    {
        string source = ReadPage("Sedmica10.razor");

        int processStart = source.IndexOf("concept-map__flow--process", StringComparison.Ordinal);
        int processEnd = source.IndexOf("</ol>", processStart, StringComparison.Ordinal);
        string process = source[processStart..processEnd];

        string[] stepsInOrder =
        [
            "Мисъл или интерпретация", "Изясняване на значението", "Разглеждане на фактите",
            "Алтернативни обяснения", "Последствия и перспектива", "По-адаптивен отговор"
        ];

        int lastIndex = -1;
        foreach (string step in stepsInOrder)
        {
            int index = process.IndexOf(step, StringComparison.Ordinal);
            Assert.True(index > lastIndex, $"'{step}' must appear after the previous step in DOM order.");
            lastIndex = index;
        }

        // The last node's text must be the final content in the list — no connector li after it.
        int lastStepIndex = process.LastIndexOf("По-адаптивен отговор", StringComparison.Ordinal);
        string afterLastStep = process[lastStepIndex..];
        Assert.DoesNotContain("<li", afterLastStep);
    }

    [Fact]
    public void ConceptMapFlow_ProcessModifierIsScoped_OtherPagesStillUseThePlainFlow()
    {
        // The new CSS variant must not change the default .concept-map__flow behavior used on
        // Week 1/3/8 and the Module 2 lessons.
        Assert.DoesNotContain("concept-map__flow--process", ReadPage("Sedmica1.razor"));
        Assert.DoesNotContain("concept-map__flow--process", ReadPage("Sedmica3.razor"));
        Assert.DoesNotContain("concept-map__flow--process", ReadPage("Sedmica8.razor"));
    }

    [Fact]
    public void Week10Page_Section10IsFullWidth_NotASingleChildInABalancedGrid()
    {
        // Owner review: same structural anti-pattern already found and fixed on Week 3 —
        // a lone LearningSection as the only child of .learning-grid--balanced leaves the
        // right half of the workspace empty. Section 10 must be a full-width sibling now.
        string source = ReadPage("Sedmica10.razor");

        // Walk back from Section 10's own heading to the <LearningSection> tag that opens it,
        // then check its immediate ancestor is not a learning-grid--balanced wrapper (which
        // would make it that grid's sole child — the exact anti-pattern found on Week 3).
        int izvoriHeadingIndex = source.IndexOf("id=\"izvori\"", StringComparison.Ordinal);
        int sectionTagStart = source.LastIndexOf("<LearningSection", izvoriHeadingIndex, StringComparison.Ordinal);
        string immediatelyBefore = source[..sectionTagStart].TrimEnd();

        Assert.False(
            immediatelyBefore.EndsWith("<div class=\"learning-grid learning-grid--balanced\">", StringComparison.Ordinal),
            "Section 10's LearningSection must not be the sole child of a learning-grid--balanced wrapper.");
    }

    [Fact]
    public void Week10Page_ReviewMapSectionHasFourSemanticInternalSubblocks()
    {
        // Retrofit: the old single "Section 10" is now split into "Карта за повторение" (review +
        // takeaways/context/disclaimer/forward-back links) and a separate "Източници" section
        // (SourceReferences + OptionalReadingSource), matching Week 7/9's established pattern.
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains("<h3>Какво да запомните</h3>", source);
        Assert.Contains("<h3>Академичен контекст</h3>", source);
        Assert.Contains("<h3>Връзки напред и назад</h3>", source);
        Assert.Contains("<DisclaimerCallout", source);
    }

    [Fact]
    public void Week10Page_HasADistinctSourcesSection()
    {
        string source = ReadPage("Sedmica10.razor");

        int izvoriHeadingIndex = source.IndexOf("id=\"izvori\"", StringComparison.Ordinal);
        Assert.True(izvoriHeadingIndex >= 0, "Expected a distinct id=\"izvori\" heading.");

        int sectionTagStart = source.LastIndexOf("<LearningSection", izvoriHeadingIndex, StringComparison.Ordinal);
        int sectionTagEnd = source.IndexOf("</LearningSection>", izvoriHeadingIndex, StringComparison.Ordinal);
        string izvoriSection = source[sectionTagStart..sectionTagEnd];

        Assert.Contains("<SourceReferences", izvoriSection);
        Assert.Contains("<OptionalReadingSource", izvoriSection);
        Assert.Contains("<WeekCompletionControl WeekNumber=\"@_week.Number\" />", izvoriSection);
    }

    [Fact]
    public void Week10Page_HasNoFixedHeightNegativeMarginOrAbsolutePositioningWorkaround()
    {
        string source = ReadPage("Sedmica10.razor");

        Assert.DoesNotContain("style=", source);
        Assert.DoesNotContain("position: absolute", source);
        Assert.DoesNotContain("position:absolute", source);
    }

    [Fact]
    public void Week10Page_Section08UsesTheDedicatedProcessRail_NotTheSharedConceptMapFlow()
    {
        // Owner review: Section 08 read as "just a list" because it reused the same card+arrow
        // .concept-map__flow visual language as every other flow diagram on the site. It now
        // gets its own distinct dot-and-rail pattern, scoped to this section only.
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains("class=\"guided-practice-sequence\"", source);

        int flowStart = source.IndexOf("id=\"flow\"", StringComparison.Ordinal);
        int flowEnd = source.IndexOf("</LearningSection>", flowStart, StringComparison.Ordinal);
        string section08 = source[flowStart..flowEnd];

        Assert.DoesNotContain("concept-map__flow", section08);
        Assert.DoesNotContain("concept-map__node", section08);
        Assert.DoesNotContain("concept-map__connector", section08);

        string cssSource = ReadCss();
        Assert.Contains(".guided-practice-sequence", cssSource);
    }

    [Fact]
    public void Week10Page_Section08PreservesAllSixStepsVerbatimInDomOrder()
    {
        string source = ReadPage("Sedmica10.razor");

        int flowStart = source.IndexOf("id=\"flow\"", StringComparison.Ordinal);
        int flowEnd = source.IndexOf("</ol>", flowStart, StringComparison.Ordinal);
        string section08 = source[flowStart..flowEnd];

        string[] stepsInOrder =
        [
            "Спри", "Назови мисълта", "Раздели фактите от заключенията", "Разгледай алтернативи",
            "Оцени вероятността и последствията", "Формулирай правдоподобен отговор"
        ];

        int lastIndex = -1;
        foreach (string step in stepsInOrder)
        {
            int index = section08.IndexOf(step, StringComparison.Ordinal);
            Assert.True(index > lastIndex, $"'{step}' must appear after the previous step in DOM order.");
            lastIndex = index;
        }

        Assert.Contains("Това е образователна демонстрация, не универсална терапевтична процедура.", source);
    }

    [Fact]
    public void Week10Page_Section08HasExactlySixListItems_ConnectorsAreNotListItems()
    {
        // Semantic contract: Section 08 is a six-step process, so its <ol> must hold exactly
        // six <li> — one per step. Connectors are decorative, not steps, and must never be a
        // sibling <li> of their own; they live inside the step <li> they trail.
        string source = ReadPage("Sedmica10.razor");

        int flowStart = source.IndexOf("id=\"flow\"", StringComparison.Ordinal);
        int flowEnd = source.IndexOf("</ol>", flowStart, StringComparison.Ordinal);
        string section08 = source[flowStart..flowEnd];

        int liCount = System.Text.RegularExpressions.Regex.Matches(section08, "<li").Count;
        Assert.Equal(6, liCount);

        Assert.DoesNotContain("<li class=\"guided-practice-sequence__connector", section08);
        Assert.Contains("class=\"guided-practice-sequence__step", section08);
    }

    [Fact]
    public void Week10Page_Section08ConnectorsArePresentationOnly_NoneAfterTheFinalStep()
    {
        string source = ReadPage("Sedmica10.razor");

        int flowStart = source.IndexOf("id=\"flow\"", StringComparison.Ordinal);
        int flowEnd = source.IndexOf("</ol>", flowStart, StringComparison.Ordinal);
        string section08 = source[flowStart..flowEnd];

        // Connectors are aria-hidden <span>s nested inside their step's <li>, never their own
        // list item and never announced to a screen reader.
        int connectorCount = System.Text.RegularExpressions.Regex.Matches(
            section08, "<span class=\"guided-practice-sequence__connector\" aria-hidden=\"true\">").Count;
        Assert.Equal(5, connectorCount);
        Assert.DoesNotContain("role=\"listitem\"", section08);

        int finalStepStart = section08.IndexOf("guided-practice-sequence__step--final", StringComparison.Ordinal);
        string finalStep = section08[finalStepStart..];
        Assert.DoesNotContain("guided-practice-sequence__connector", finalStep);
    }

    [Fact]
    public void Week10Page_Section08FinalStepModifierRemains()
    {
        string source = ReadPage("Sedmica10.razor");

        Assert.Contains("guided-practice-sequence__step guided-practice-sequence__step--final", source);
    }

    [Fact]
    public void Week10Page_Section08GroupsNumberAndLabelAsOneUnit()
    {
        // Owner review: number and label must never sit at opposite ends of a shared grid
        // cell — they belong inside one physically grouped element per step.
        string source = ReadPage("Sedmica10.razor");

        int flowStart = source.IndexOf("id=\"flow\"", StringComparison.Ordinal);
        int flowEnd = source.IndexOf("</ol>", flowStart, StringComparison.Ordinal);
        string section08 = source[flowStart..flowEnd];

        int unitCount = System.Text.RegularExpressions.Regex.Matches(
            section08, "<span class=\"guided-practice-sequence__unit\">").Count;
        Assert.Equal(6, unitCount);

        // Each unit's number must immediately precede its own label, inside the same unit.
        System.Text.RegularExpressions.MatchCollection units = System.Text.RegularExpressions.Regex.Matches(
            section08,
            "<span class=\"guided-practice-sequence__unit\">\\s*" +
            "<span class=\"guided-practice-sequence__number\"[^>]*>\\d\\d</span>\\s*" +
            "<span class=\"guided-practice-sequence__label\">[^<]+</span>\\s*</span>");
        Assert.Equal(6, units.Count);
    }

    [Fact]
    public void GuidedPracticeSequence_HasExactlyTwoResponsiveModes_NoMultiRowGridRemains()
    {
        // Owner review: the rejected 3x2 grid (and the boustrophedon before it) is gone —
        // exactly one vertical mode (base) and one horizontal mode (wide container), nothing
        // in between, no CSS `order`, no absolute positioning, no per-nth-child row hacks.
        string cssSource = ReadCss();

        int sectionStart = cssSource.IndexOf(".guided-practice-sequence {", StringComparison.Ordinal);
        int sectionEnd = cssSource.IndexOf(
            "Comparison matrix (static table)", sectionStart, StringComparison.Ordinal);
        string block = cssSource[sectionStart..sectionEnd];

        Assert.DoesNotContain("grid-template-columns: repeat(3", block);
        Assert.DoesNotContain(":nth-child(3)", block);
        Assert.DoesNotContain(":nth-child(6)", block);
        Assert.DoesNotContain("display: none", block);
        // Bare "order:" would also false-positive-match inside "border:" — check for the
        // property declaration specifically (newline + indent immediately before it).
        Assert.DoesNotContain("\n    order:", block);
        Assert.DoesNotContain("\n        order:", block);
        Assert.DoesNotContain("position: absolute", block);

        // The content-bearing containers (step, unit, label) stay auto-sized — only the small
        // decorative number badge and connector line get a fixed, non-content-clipping size.
        Assert.DoesNotContain(".guided-practice-sequence__step {\n    height:", block);
        Assert.DoesNotContain(".guided-practice-sequence__unit {\n    height:", block);
        Assert.DoesNotContain(".guided-practice-sequence__label {\n    height:", block);

        // Vertical fallback (default/base rule).
        Assert.Contains(".guided-practice-sequence__step {\n    display: flex;\n    flex-direction: column;", block);

        // Horizontal wide mode exists, sized against the container (not the raw viewport).
        Assert.Contains("@container (min-width: 1100px)", block);
        Assert.Contains("@supports not (container-type: inline-size)", block);
        Assert.Contains("@media (min-width: 1100px)", block);
    }

    [Fact]
    public void GuidedPracticeSequence_IsScopedToWeek10Section08Only()
    {
        Assert.DoesNotContain("guided-practice-sequence", ReadPage("Sedmica1.razor"));
        Assert.DoesNotContain("guided-practice-sequence", ReadPage("Sedmica3.razor"));
        Assert.DoesNotContain("guided-practice-sequence", ReadPage("Sedmica8.razor"));

        // Section 01's own process diagram must still use its own dedicated pattern, not this one.
        string source = ReadPage("Sedmica10.razor");
        int izsledvaneStart = source.IndexOf("id=\"izsledvane\"", StringComparison.Ordinal);
        int izsledvaneEnd = source.IndexOf("</ol>", izsledvaneStart, StringComparison.Ordinal);
        Assert.DoesNotContain("guided-practice-sequence", source[izsledvaneStart..izsledvaneEnd]);
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    private static string ReadClientComponent(string fileName)
    {
        string interactiveDirectory = Path.Combine(
            TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Interactive");
        return File.ReadAllText(Path.Combine(interactiveDirectory, fileName));
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
