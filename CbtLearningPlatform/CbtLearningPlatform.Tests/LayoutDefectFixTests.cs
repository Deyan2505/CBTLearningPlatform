namespace CbtLearningPlatform.Tests;

/// <summary>Covers the owner's real-screenshot layout defect round: horizontal overflow,
/// heading focus-ring appearance, stretched grid columns, nested card chrome, module learning
/// paths, calm educational disclaimer, and duplicate role-label/heading pairs. Structural
/// checks only, no pixel assertions.</summary>
public sealed class LayoutDefectFixTests
{
    private static readonly string[] AllPageFiles =
    [
        "Home.razor", "Programa.razor", "Kpt.razor", "Kurs.razor", "Sedmica8.razor",
        "Modul1.razor", "Modul1Lesson1.razor",
        "Modul2.razor", "Modul2Lesson1.razor", "Modul2Lesson2.razor", "Modul2Lesson3.razor"
    ];

    [Fact]
    public void AppCss_DoesNotHideOverflowAsAWorkaround()
    {
        string css = ReadCss();

        Assert.DoesNotContain("overflow-x: hidden", css);
    }

    [Fact]
    public void AppCss_LearningGridChildrenCanShrinkBelowContentSize()
    {
        // Root cause of the page-level horizontal scrollbar: grid items default to
        // min-width:auto (their content's min-content size), so a wide unbreakable child
        // (a table, a flex row) could grow a column past its track and force the whole page
        // wider. min-width:0 lets local overflow-x:auto wrappers (e.g. the comparison-matrix)
        // actually engage instead of pushing the page itself wider.
        string css = ReadCss();

        int ruleIndex = css.IndexOf(".learning-grid > * {", StringComparison.Ordinal);
        Assert.True(ruleIndex >= 0, "Expected a rule constraining every learning-grid child.");

        int ruleEnd = css.IndexOf('}', ruleIndex);
        string rule = css[ruleIndex..ruleEnd];

        Assert.Contains("min-width: 0", rule);
    }

    [Fact]
    public void AppCss_LearningGridChildrenNeverStretchTaller_ThanTheirOwnContent()
    {
        string css = ReadCss();

        int ruleIndex = css.IndexOf(".learning-grid > * {", StringComparison.Ordinal);
        int ruleEnd = css.IndexOf('}', ruleIndex);
        string rule = css[ruleIndex..ruleEnd];

        Assert.Contains("align-self: start", rule);
    }

    [Fact]
    public void AppCss_ProgrammaticHeadingFocusDoesNotUseTheControlStyleRing()
    {
        string css = ReadCss();

        Assert.Contains("h1[tabindex=\"-1\"]:focus,", css);
        Assert.Contains("outline: none;", css);
        Assert.Contains("h1[tabindex=\"-1\"]:focus-visible,", css);
    }

    [Fact]
    public void AppCss_RealKeyboardFocusOnAHeadingStillGetsAVisibleRing()
    {
        string css = ReadCss();

        int ruleIndex = css.IndexOf("h1[tabindex=\"-1\"]:focus-visible,", StringComparison.Ordinal);
        Assert.True(ruleIndex >= 0);

        int ruleEnd = css.IndexOf('}', ruleIndex);
        string rule = css[ruleIndex..ruleEnd];

        Assert.Contains("outline:", rule);
        Assert.DoesNotContain("outline: none", rule);
    }

    [Fact]
    public void ModuleCard_NoLongerRendersItsOwnFullCardBorder()
    {
        // It always sits inside an already-bordered LearningSection now — a second full
        // border was the "card in card" chrome the owner flagged.
        string source = ReadSharedComponent("ModuleCard.razor");

        Assert.DoesNotContain("class=\"card module-card\"", source);
    }

    [Fact]
    public void ModuleCard_StatusTextOnlyShowsForTheDisabledNotYetAvailableCase()
    {
        string source = ReadSharedComponent("ModuleCard.razor");

        int linkBranchIndex = source.IndexOf("if (!string.IsNullOrEmpty(DestinationUrl))", StringComparison.Ordinal);
        int elseBranchIndex = source.IndexOf("else", linkBranchIndex, StringComparison.Ordinal);

        int statusIndex = source.IndexOf("module-card__status", StringComparison.Ordinal);

        Assert.True(statusIndex > elseBranchIndex,
            "The 'Наличен' status text must only render in the disabled (no DestinationUrl) branch, not next to every available lesson's CTA.");
    }

    [Theory]
    [InlineData("Modul1.razor")]
    [InlineData("Modul2.razor")]
    public void ModuleOverview_HasARealSemanticLearningPath(string fileName)
    {
        string source = ReadPage(fileName);

        Assert.Contains("<ol class=\"module-path\">", source);
        Assert.Contains("module-path__marker--number", source);
    }

    [Fact]
    public void Module1_LearningPathDistinguishesLinkFromStatusFromNextStep()
    {
        string source = ReadPage("Modul1.razor");

        Assert.Contains("module-path__marker--number", source);
        Assert.Contains("module-path__marker--status", source);
        Assert.Contains("module-path__marker--next", source);
        // The status-only node must not look like a clickable/button element.
        Assert.Contains("<span class=\"module-path__status\">", source);
    }

    [Fact]
    public void Module2_LearningPathHasThreeRealLessonNodes()
    {
        string source = ReadPage("Modul2.razor");

        int nodeCount = CountOccurrences(source, "module-path__marker--number");
        Assert.Equal(3, nodeCount);

        Assert.Contains("/programa/modul-2/situacia-misal-emocia-povedenie", source);
        Assert.Contains("/programa/modul-2/avtomatichni-misli", source);
        Assert.Contains("/programa/modul-2/emocii-i-telesni-reaktsii", source);
    }

    [Theory]
    [InlineData("Modul1.razor")]
    [InlineData("Modul2.razor")]
    public void ModuleOverview_HasAConceptMapDistinctFromTheLearningPath(string fileName)
    {
        string source = ReadPage(fileName);

        Assert.Contains("concept-map__flow", source);
    }

    [Fact]
    public void DisclaimerCallout_EducationalVariantIsCalm_NotAFullNestedPanel()
    {
        string css = ReadCss();

        int ruleIndex = css.IndexOf(".callout--educational {", StringComparison.Ordinal);
        Assert.True(ruleIndex >= 0);

        int ruleEnd = css.IndexOf('}', ruleIndex);
        string rule = css[ruleIndex..ruleEnd];

        Assert.Contains("border: none", rule);
        Assert.Contains("var(--accent-academic", rule);
    }

    [Theory]
    [InlineData("Modul1.razor", "Уроци")]
    [InlineData("Modul2.razor", "Уроци")]
    [InlineData("Modul1.razor", "Последователност")]
    [InlineData("Modul2.razor", "Последователност")]
    [InlineData("Modul1Lesson1.razor", "Проверка")]
    [InlineData("Modul2Lesson1.razor", "Проверка")]
    [InlineData("Modul2Lesson2.razor", "Проверка")]
    [InlineData("Modul2Lesson3.razor", "Проверка")]
    [InlineData("Modul2Lesson1.razor", "Обобщение")]
    [InlineData("Modul2Lesson2.razor", "Обобщение")]
    [InlineData("Modul2Lesson3.razor", "Обобщение")]
    [InlineData("Kpt.razor", "Следваща стъпка")]
    [InlineData("Modul2Lesson1.razor", "Следваща стъпка")]
    [InlineData("Modul2Lesson2.razor", "Следваща стъпка")]
    [InlineData("Modul2Lesson3.razor", "Следваща стъпка")]
    [InlineData("Programa.razor", "Как се използва")]
    [InlineData("Programa.razor", "Видове съдържание")]
    [InlineData("Sedmica8.razor", "Проверка")]
    public void RoleLabel_NoLongerRepeatsTheAdjacentHeadingWordForWord(string fileName, string retiredLabel)
    {
        string source = ReadPage(fileName);

        Assert.DoesNotContain($"RoleLabel=\"{retiredLabel}\"", source);
        Assert.DoesNotContain($"section-role-label--theory\">{retiredLabel}<", source);
    }

    [Theory]
    [InlineData("Home.razor")]
    [InlineData("Programa.razor")]
    [InlineData("Kpt.razor")]
    [InlineData("Modul1.razor")]
    [InlineData("Modul1Lesson1.razor")]
    [InlineData("Modul2.razor")]
    [InlineData("Modul2Lesson1.razor")]
    [InlineData("Modul2Lesson2.razor")]
    [InlineData("Modul2Lesson3.razor")]
    public void EveryMainPage_StillShowsTheDisclaimer(string fileName)
    {
        Assert.Contains("<DisclaimerCallout", ReadPage(fileName));
    }

    [Fact]
    public void AppCss_SidebarIsWideEnoughForLabelsToFitOnOneLine()
    {
        string css = ReadCss();

        int ruleIndex = css.IndexOf(".app-sidebar {", StringComparison.Ordinal);
        int ruleEnd = css.IndexOf('}', ruleIndex);
        string rule = css[ruleIndex..ruleEnd];

        Assert.Contains("width: 276px", rule);
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
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    private static string ReadSharedComponent(string fileName)
    {
        string sharedDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Shared");
        return File.ReadAllText(Path.Combine(sharedDirectory, fileName));
    }

    private static string ReadCss()
    {
        string cssPath = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "wwwroot", "app.css");
        return File.ReadAllText(cssPath);
    }
}
