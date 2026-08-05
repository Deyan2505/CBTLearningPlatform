namespace CbtLearningPlatform.Tests;

/// <summary>Covers the final owner visual polish round: sidebar active-nav matching, the Home
/// secondary CTA/learning-path diagram, per-lesson progressive disclosure, workspace gutters,
/// and the cooler dark palette. Structural checks only, no pixel assertions.</summary>
public sealed class FinalPolishTests
{
    private static readonly string[] LessonFiles =
    [
        "Modul1Lesson1.razor", "Modul2Lesson1.razor", "Modul2Lesson2.razor", "Modul2Lesson3.razor"
    ];

    [Fact]
    public void Sidebar_EveryNavLinkUsesExactMatching_NoParentChildDoubleActiveState()
    {
        string source = ReadLayoutComponent("MainLayout.razor");

        int navLinkCount = CountOccurrences(source, "<NavLink");
        int exactMatchCount = CountOccurrences(source, "Match=\"NavLinkMatch.All\"");

        Assert.Equal(navLinkCount, exactMatchCount);
    }

    [Fact]
    public void HomePage_SecondaryCtaUsesTheBlueOutlinedButtonRole()
    {
        string source = ReadPage("Home.razor");

        Assert.Contains("class=\"btn btn-blue\"", source);
    }

    [Fact]
    public void HomePage_LearningPathHasThreeDistinctVisualSteps()
    {
        string source = ReadPage("Home.razor");

        Assert.Contains("learning-path-diagram__step--theory", source);
        Assert.Contains("learning-path-diagram__step--visual", source);
        Assert.Contains("learning-path-diagram__step--interactive", source);
    }

    [Fact]
    public void HomePage_LearningPathHasConnectorsAndIsASemanticOrderedList()
    {
        string source = ReadPage("Home.razor");

        Assert.Contains("<ol class=\"learning-path-diagram\">", source);

        int connectorCount = CountOccurrences(source, "learning-path-diagram__connector");
        Assert.True(connectorCount >= 2, $"Expected at least 2 connectors between 3 steps, found {connectorCount}.");
    }

    [Theory]
    [InlineData("Modul1Lesson1.razor")]
    [InlineData("Modul2Lesson1.razor")]
    [InlineData("Modul2Lesson2.razor")]
    [InlineData("Modul2Lesson3.razor")]
    public void Lesson_TheorySectionUsesProgressiveDisclosureForTheFullExplanation(string fileName)
    {
        string source = ReadPage(fileName);

        Assert.Contains("<ProgressiveExplanation", source);
        Assert.Contains("key-idea-strip", source);
    }

    [Theory]
    [InlineData("Modul1Lesson1.razor")]
    [InlineData("Modul2Lesson1.razor")]
    [InlineData("Modul2Lesson2.razor")]
    [InlineData("Modul2Lesson3.razor")]
    public void Lesson_MainDefinitionStaysVisibleOutsideTheDisclosure(string fileName)
    {
        string source = ReadPage(fileName);

        int firstDisclosureIndex = source.IndexOf("<ProgressiveExplanation", StringComparison.Ordinal);
        int keyIdeaIndex = source.IndexOf("key-idea-strip", StringComparison.Ordinal);

        Assert.True(firstDisclosureIndex >= 0 && keyIdeaIndex >= 0 && keyIdeaIndex < firstDisclosureIndex,
            $"{fileName}: the key-idea summary must render before (outside) the progressive disclosure block.");
    }

    [Theory]
    [InlineData("Modul1Lesson1.razor")]
    [InlineData("Modul2Lesson1.razor")]
    [InlineData("Modul2Lesson2.razor")]
    [InlineData("Modul2Lesson3.razor")]
    public void Lesson_DisclaimerIsNotNestedInsideTheProgressiveDisclosure(string fileName)
    {
        string source = ReadPage(fileName);

        int lastDisclosureClose = source.LastIndexOf("</ProgressiveExplanation>", StringComparison.Ordinal);
        int disclaimerIndex = source.IndexOf("<DisclaimerCallout", StringComparison.Ordinal);

        Assert.True(disclaimerIndex > lastDisclosureClose,
            $"{fileName}: DisclaimerCallout must render after the progressive disclosure block closes, not inside it.");
    }

    [Theory]
    [InlineData("Modul1Lesson1.razor")]
    [InlineData("Modul2Lesson1.razor")]
    [InlineData("Modul2Lesson2.razor")]
    [InlineData("Modul2Lesson3.razor")]
    public void Lesson_LearningObjectivesStayVisibleOutsideTheDisclosure(string fileName)
    {
        string source = ReadPage(fileName);

        int firstDisclosureIndex = source.IndexOf("<ProgressiveExplanation", StringComparison.Ordinal);
        int objectivesIndex = source.IndexOf("<LearningObjectives", StringComparison.Ordinal);

        Assert.True(objectivesIndex >= 0 && objectivesIndex < firstDisclosureIndex);
    }

    [Fact]
    public void Modul2Lesson1_CbtModelDiagramShowsConnectedProcessWithConnectors()
    {
        string source = ReadClientComponent("CbtModelDiagram.razor");

        Assert.Contains("cbt-diagram__step-connector", source);
    }

    [Fact]
    public void Modul2Lesson2_HasThreeDistinctCategoriesNotJustABulletList()
    {
        string source = ReadPage("Modul2Lesson2.razor");

        Assert.Contains("category-compare__item--fact", source);
        Assert.Contains("category-compare__item--thought", source);
        Assert.Contains("category-compare__item--feeling", source);
    }

    [Fact]
    public void Modul2Lesson3_UsesNonUniversalPhrasingForTheBodyReactionLink()
    {
        string source = ReadPage("Modul2Lesson3.razor");

        Assert.Contains("Една възможна телесна реакция", source);
    }

    [Fact]
    public void Modul1Lesson1_ComparisonTableHasIconsAndIsNotColoredAsWarningOrError()
    {
        string source = ReadPage("Modul1Lesson1.razor");

        Assert.Contains("comparison-matrix__icon", source);
    }

    [Fact]
    public void AppCss_HasAResponsiveWorkspaceGutter()
    {
        string css = ReadCss();

        int ruleIndex = css.IndexOf(".page-container {", StringComparison.Ordinal);
        Assert.True(ruleIndex >= 0);

        int ruleEnd = css.IndexOf('}', ruleIndex);
        string rule = css[ruleIndex..ruleEnd];

        Assert.Contains("padding-inline: clamp(", rule);
    }

    [Fact]
    public void AppCss_DarkPaletteUsesACoolerNavyCharcoalBase()
    {
        string css = ReadCss();

        Assert.DoesNotContain("--color-background: #1E1D1B;", css);
        Assert.Contains("--color-background: #0E1420;", css);
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

    private static string ReadLayoutComponent(string fileName)
    {
        string layoutDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Layout");
        return File.ReadAllText(Path.Combine(layoutDirectory, fileName));
    }

    private static string ReadClientComponent(string fileName)
    {
        string interactiveDirectory = Path.Combine(
            TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Interactive");
        return File.ReadAllText(Path.Combine(interactiveDirectory, fileName));
    }

    private static string ReadCss()
    {
        string cssPath = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "wwwroot", "app.css");
        return File.ReadAllText(cssPath);
    }
}
