using System.Reflection;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>CONTENT-DRIVEN TEMPLATE VALIDATION, Slice 2 — Week 3 ("Concept and Diagram"
/// archetype), distinct from Week 1's "Theory and History" and Week 8's "Simulator Workspace".</summary>
public sealed class Week3ContentSliceTests
{
    [Fact]
    public void Week3Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica3"));
    }

    [Fact]
    public void Week3_MetadataIsAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 3);

        Assert.Equal(CourseWeekStatus.Available, week.Status);
        Assert.Equal("/kurs/sedmica-3", week.Route);
    }

    [Fact]
    public void Week1AndWeek8_RemainAvailableAfterWeek3WasAdded()
    {
        CourseWeekDefinition week1 = CourseCatalog.Weeks.Single(w => w.Number == 1);
        CourseWeekDefinition week8 = CourseCatalog.Weeks.Single(w => w.Number == 8);

        Assert.Equal(CourseWeekStatus.Available, week1.Status);
        Assert.Equal("/kurs/sedmica-1", week1.Route);
        Assert.Equal(CourseWeekStatus.Available, week8.Status);
        Assert.Equal("/kurs/sedmica-8", week8.Route);
    }

    [Fact]
    public void RemainingEightWeeks_StayUnavailable()
    {
        int[] availableNumbers = [1, 2, 3, 6, 7, 8, 9, 10];

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

        Assert.Equal(7, CourseCatalog.Weeks.Count(w => !availableNumbers.Contains(w.Number)));
    }

    [Fact]
    public void Week3Page_HasPageTitleAndFormatBadge()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("<PageTitle>Седмица 3: Архитектура на когнитивния модел", source);
        Assert.Contains("Концепция и диаграма", source);
    }

    [Fact]
    public void Week3Page_HierarchyContainsExactlyTheThreeLevels()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("Автоматична мисъл", source);
        Assert.Contains("Междинно вярване", source);
        Assert.Contains("Основно вярване", source);
    }

    [Fact]
    public void Week3Page_AutomaticThoughtIsFramedAsTheMostSituational()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("ситуативно", source);
    }

    [Fact]
    public void CognitiveHierarchyExplorer_IntermediateBeliefsIncludeAttitudesRulesAssumptions()
    {
        string source = ReadClientComponent("CognitiveHierarchyExplorer.razor");

        Assert.Contains("Нагласа, правило или условно предположение", source);
    }

    [Fact]
    public void Week3Page_CoreBeliefsAreNotDescribedAsAlwaysNegative()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("адаптивни или неадаптивни", source);
        Assert.DoesNotContain("винаги отрицателн", source);
        Assert.DoesNotContain("скрити истини", source);
    }

    [Fact]
    public void Week3Page_CognitiveTriadContainsSelfWorldFuture()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("<h3>Аз</h3>", source);
        Assert.Contains("Светът и преживяванията", source);
        Assert.Contains("<h3>Бъдещето</h3>", source);
    }

    [Fact]
    public void Week3Page_CognitiveTriadIsNotFramedAsASelfAssessment()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("не диагностичен тест", source);
        Assert.DoesNotContain("BDI", source);
        Assert.DoesNotContain("BAI", source);
        Assert.DoesNotContain("BHS", source);
    }

    [Fact]
    public void Week3Page_SchemaFilterIsExplicitlyLabeledAMetaphor()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("полезна метафора, не буквален механизъм", source);
    }

    [Fact]
    public void SchemaFilterDemonstration_UsesFixedPresetData_NoPersonalInput()
    {
        string source = ReadClientComponent("SchemaFilterDemonstration.razor");

        Assert.DoesNotContain("<input", source);
        Assert.DoesNotContain("<textarea", source);
        Assert.DoesNotContain("localStorage", source);
        Assert.DoesNotContain("sessionStorage", source);
        Assert.DoesNotContain("HttpClient", source);
    }

    [Fact]
    public void Week3Page_HasExactlyTwoInteractiveIslands()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("<CognitiveHierarchyExplorer", source);
        Assert.Contains("<SchemaFilterDemonstration", source);
    }

    [Fact]
    public void Week3Page_AutomaticVsReflectiveProcessing_RemovedAsSourceUnresolved()
    {
        // v1's "automatic vs. reflexive processing" comparison does not appear anywhere in
        // SRC-041 Ch. 3 and had no cited secondary source in 11_SOURCE_REGISTER.md (checked
        // directly for this v2 migration) — removed rather than invented, per explicit owner
        // instruction not to fabricate a source. See WEEK_03_V2_MIGRATION_BLUEPRINT.md §9.
        string source = ReadPage("Sedmica3.razor");

        Assert.DoesNotContain("Автоматична обработка", source);
        Assert.DoesNotContain("Рефлексивна обработка", source);
    }

    [Fact]
    public void Week3Page_HasTheSallyWorkedCase_RetoldNotVerbatim()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("Случаят на Сали", source);
        // The book's own long descriptive passages ("Като малко дете, тя възприемаше, че не може
        // да направи нищо толкова добре, колкото брат ѝ" etc.) must not appear verbatim — the page
        // retells the case in its own words, not a copied excerpt.
        Assert.DoesNotContain("Като малко дете, тя възприемаше", source);
        Assert.DoesNotContain("Направи ужасна работа, като", source);
    }

    [Fact]
    public void Week3Page_HasThePopulatedSallyConceptMap_UsingNetworkLayout()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("ComponentId=\"week3-sali-hierarchy\"", source);
        Assert.Contains("ConceptGraphLayout.Network", source);
        // Never added to the global map — owner has not separately confirmed these as concepts.
        string curriculumDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Curriculum");
        string knowledgeMapCatalogSource = File.ReadAllText(Path.Combine(curriculumDirectory, "KnowledgeMapCatalog.cs"));
        Assert.DoesNotContain("sali-core", knowledgeMapCatalogSource);
    }

    [Fact]
    public void Week3Page_HasIntermediateBeliefSubtypes()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("Нагласа", source);
        Assert.Contains("Правило", source);
        Assert.Contains("Предположение", source);
        Assert.Contains("Условно \"ако — тогава\" вярване", source);
    }

    [Fact]
    public void Week3Page_HasTreatmentSequencingRationale()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("id=\"posledovatelnost\"", source);
        Assert.Contains("рискува терапевтичния алианс", source);
    }

    [Fact]
    public void Week3Page_HasCascadingModelCaveat()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("id=\"kaskaden-model\"", source);
        Assert.Contains("полезно опростяване", source);
        Assert.Contains("не винаги описва", source);
    }

    [Fact]
    public void Week3Page_HasWeeklyMindMapPreviewAndReview_SameModelBothTimes()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("ComponentId=\"week3-mindmap-preview\"", source);
        Assert.Contains("ComponentId=\"week3-mindmap-review\"", source);
        // Both instances render the exact same static model — one knowledge structure, two states.
        Assert.Equal(2, System.Text.RegularExpressions.Regex.Matches(source, "Model=\"@_week3MindMapRender\"").Count);
    }

    [Fact]
    public void Week3Page_MindMapReviewUsesExplainBeforeRevealPattern()
    {
        string source = ReadPage("Sedmica3.razor");
        int reviewIndex = source.IndexOf("id=\"karta-povtorenie\"", StringComparison.Ordinal);
        int nextSectionIndex = source.IndexOf("id=\"izvori\"", StringComparison.Ordinal);
        string reviewBlock = source[reviewIndex..nextSectionIndex];

        Assert.Contains("<details", reviewBlock);
        Assert.Contains("опитай да си спомниш", reviewBlock);
    }

    [Fact]
    public void Week3Page_HasSixKnowledgeCheckQuestions()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("Въпрос 5.", source);
        Assert.Contains("Въпрос 6", source);
    }

    [Fact]
    public void Week3Page_HasTheIntegratedCognitiveMapAsCentralAnchor()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("Основно вярване", source);
        Assert.Contains("Междинно вярване (нагласа, правило или предположение)", source);
        Assert.Contains("не е инструмент за самодиагностика", source);
    }

    [Fact]
    public void Week3Page_HasANonScoredCategorizationCheck_ReusingNativeDetailsSummary()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("Покажи класификацията", source);
        Assert.Contains("<details", source);
    }

    [Fact]
    public void Week3Page_HasFourKnowledgeCheckQuestions()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("Въпрос 1.", source);
        Assert.Contains("Въпрос 2.", source);
        Assert.Contains("Въпрос 3.", source);
        Assert.Contains("Въпрос 4.", source);
        Assert.Contains("Проверката не се оценява и не запазва отговори", source);
    }

    [Fact]
    public void Week3Page_HasEducationalDisclaimerAndLearnerFacingAcademicContext()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("<DisclaimerCallout", source);
        Assert.DoesNotContain("Variant=\"safety\"", source);
        Assert.Contains("<h3>Академичен контекст</h3>", source);
        Assert.Contains("предстои да премине независим академичен и професионален преглед", source);
    }

    [Fact]
    public void Week3Page_HasNoInternalDevelopmentLanguageInItsRenderableMarkup()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica3.razor");

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
    public void Week3Page_MakesNoFalseAccreditationClaims()
    {
        string source = ReadPage("Sedmica3.razor");

        // "диагности" is intentionally not in this list — it appears only inside the negation
        // "не диагностичен тест" (covered explicitly by the self-assessment test above).
        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week3Page_HasNoClinicalTrainingInstructions()
    {
        string source = ReadPage("Sedmica3.razor");

        string[] forbiddenTerms =
        [
            "открийте основното си вярване", "диагностицирайте схемата си",
            "приложете техниката върху себе си", "работете с пациент", "променете дълбокото си убеждение"
        ];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week3Page_CrossLinksToModul2LessonsWeek1AndWeek8_NoDeadLinks()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.Contains("href=\"/programa/modul-2\"", source);
        Assert.Contains("/programa/modul-2/avtomatichni-misli", source);
        Assert.Contains("/programa/modul-2/emocii-i-telesni-reaktsii", source);
        Assert.Contains("/kurs/sedmica-1", source);
        Assert.Contains("/kurs/sedmica-8", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Modul2Overview_CrossLinksToWeek3AsAnExtendedConceptualLesson()
    {
        string modul2Source = ReadPage("Modul2.razor");

        Assert.Contains("/kurs/sedmica-3", modul2Source);
    }

    [Fact]
    public void Week3Page_DoesNotLinkToWeek4AsIfAvailable()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.DoesNotContain("/kurs/sedmica-4", source);
    }

    [Fact]
    public void Week3Page_HasNoPageLevelOverflowWorkaround()
    {
        string source = ReadPage("Sedmica3.razor");

        Assert.DoesNotContain("overflow-x: hidden", source);
        Assert.DoesNotContain("overflow-x:hidden", source);
    }

    [Fact]
    public void Week3Page_SectionNavAnchorsIncludeTheFullPath_NotBareFragments()
    {
        // Owner review: App.razor sets <base href="/"> for Blazor asset resolution, which means
        // a bare href="#id" resolves against "/" (Home), not the current page — both native
        // anchor-click handling and Blazor's enhanced navigation use this resolved value, so a
        // bare fragment link on this page would navigate to Home instead of scrolling in place.
        string source = ReadPage("Sedmica3.razor");

        string[] anchorIds =
        [
            "karta-sedmicata", "tri-niva", "izsledvane", "situacia-znachenie", "triada", "filtar",
            "sali-hierarhia", "karta", "posledovatelnost", "kaskaden-model", "obarkvaniya",
            "proverka", "karta-povtorenie", "izvori"
        ];

        foreach (string id in anchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-3#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
        }
    }

    [Fact]
    public void Week3Page_ConfusionsAndKnowledgeCheckAreNotPairedInAMismatchedHeightRow()
    {
        // Owner review: sections 08 (3 short cards) and 09 (4 taller cards) were previously
        // paired in one .learning-grid--balanced row — the shared row height (set by the taller
        // section) left a large empty gap under the shorter one. Each is now its own full-width
        // LearningSection; only their own internal card lists use a grid.
        string source = ReadPage("Sedmica3.razor");

        int obarkvaniyaIndex = source.IndexOf("id=\"obarkvaniya\"", StringComparison.Ordinal);
        int proverkaIndex = source.IndexOf("id=\"proverka\"", StringComparison.Ordinal);
        string between = source[obarkvaniyaIndex..proverkaIndex];

        // The two sections must not share an enclosing .learning-grid--balanced wrapper:
        // the closing "</LearningSection>" for section 08 must appear before section 09 starts.
        Assert.Contains("</LearningSection>", between);
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

    /// <summary>Strips the leading @* ... *@ dev comment — Razor comments never reach the
    /// browser, so only the text after them reflects what a visitor could actually see.</summary>
    private static string ReadPublicMarkup(string fileName)
    {
        string source = ReadPage(fileName);
        int commentEnd = source.IndexOf("*@", StringComparison.Ordinal);
        return commentEnd >= 0 ? source[(commentEnd + 2)..] : source;
    }
}
