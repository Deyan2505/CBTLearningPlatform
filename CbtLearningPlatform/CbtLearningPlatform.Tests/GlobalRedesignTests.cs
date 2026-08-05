using System.Reflection;

namespace CbtLearningPlatform.Tests;

/// <summary>Covers the owner's global two-column redesign — every real route (not just Week 8/
/// the course hub) must use the shared .learning-grid/LearningSection workspace pattern instead
/// of the old flat single .content column. Structural presence only, no pixel assertions.</summary>
public sealed class GlobalRedesignTests
{
    private static readonly string[] AllRoutePageFiles =
    [
        "Home.razor", "Programa.razor", "Kpt.razor", "Kurs.razor", "Sedmica8.razor",
        "Modul1.razor", "Modul1Lesson1.razor",
        "Modul2.razor", "Modul2Lesson1.razor", "Modul2Lesson2.razor", "Modul2Lesson3.razor"
    ];

    [Theory]
    [InlineData("CbtLearningPlatform.Components.Pages.Home")]
    [InlineData("CbtLearningPlatform.Components.Pages.Programa")]
    [InlineData("CbtLearningPlatform.Components.Pages.Kpt")]
    [InlineData("CbtLearningPlatform.Components.Pages.Modul1")]
    [InlineData("CbtLearningPlatform.Components.Pages.Modul2")]
    public void RouteInventory_CoreTypesExistInHostAssembly(string typeName)
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform");

        Assert.NotNull(assembly.GetType(typeName));
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
    public void EveryMainRoute_UsesTheSharedTwoColumnWorkspacePattern(string fileName)
    {
        string source = ReadPage(fileName);

        Assert.Contains("class=\"learning-grid", source);
        Assert.Contains("<LearningSection", source);
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
    public void EveryMainRoute_HasAtLeastOneVisualAnchor(string fileName)
    {
        string source = ReadPage(fileName);

        bool hasVisualAnchor = source.Contains("concept-map")
            || source.Contains("comparison-matrix")
            || source.Contains("<CbtModelDiagram")
            || source.Contains("<CbtChainSimulator")
            || source.Contains("<InterpretationExample")
            || source.Contains("process-steps")
            || source.Contains("<LearningPathVisualization");

        Assert.True(hasVisualAnchor, $"{fileName} has no diagram/matrix/process/simulator visual anchor.");
    }

    [Fact]
    public void HomePage_HasAtLeastTwoTwoColumnRows()
    {
        string source = ReadPage("Home.razor");

        int gridCount = CountOccurrences(source, "class=\"learning-grid");
        Assert.True(gridCount >= 2, $"Expected at least 2 learning-grid rows on Home, found {gridCount}.");
    }

    [Fact]
    public void HomePage_UsesDistinctPrimaryAndSecondaryButtonRoles()
    {
        string source = ReadPage("Home.razor");

        Assert.Contains("btn-violet", source);
        Assert.Contains("btn-blue", source);
    }

    [Fact]
    public void HomePage_HeroIntroIsWiderThanTheProseReadingColumn()
    {
        string source = ReadPage("Home.razor");

        Assert.Contains("class=\"hero-intro\"", source);
    }

    [Theory]
    [InlineData("Modul1.razor")]
    [InlineData("Modul2.razor")]
    public void ModuleOverview_UsesWideNarrowLayoutWithSequenceSidebar(string fileName)
    {
        string source = ReadPage(fileName);

        Assert.Contains("learning-grid--wide-narrow", source);
        Assert.Contains("hub-sidebar", source);
    }

    [Fact]
    public void AtLeastFourDistinctButtonRolesAreUsedAcrossTheApp()
    {
        string[] roles = ["btn-violet", "btn-blue", "btn-example", "btn-neutral"];
        int usedCount = 0;

        foreach (string role in roles)
        {
            bool usedSomewhere = AllRoutePageFiles.Any(f => ReadPage(f).Contains(role))
                || ReadClientComponent("CategorizationCheck.razor").Contains(role)
                || ReadClientComponent("CbtChainSimulator.razor").Contains(role)
                || ReadSharedComponent("ModuleCard.razor").Contains(role);

            if (usedSomewhere) usedCount++;
        }

        Assert.True(usedCount >= 4, $"Expected at least 4 distinct button roles in real use, found {usedCount}.");
    }

    [Fact]
    public void AppCss_ContentColumnIsCentered_NotFlushLeftInAWideContainer()
    {
        string css = ReadCss();

        int contentRuleIndex = css.IndexOf(".content {", StringComparison.Ordinal);
        Assert.True(contentRuleIndex >= 0);

        int ruleEnd = css.IndexOf('}', contentRuleIndex);
        string rule = css[contentRuleIndex..ruleEnd];

        Assert.Contains("margin-inline: auto", rule);
    }

    [Theory]
    [InlineData("Home.razor")]
    [InlineData("Programa.razor")]
    [InlineData("Modul1.razor")]
    [InlineData("Modul1Lesson1.razor")]
    [InlineData("Modul2.razor")]
    [InlineData("Modul2Lesson1.razor")]
    [InlineData("Modul2Lesson2.razor")]
    [InlineData("Modul2Lesson3.razor")]
    public void EveryMainRoute_StillShowsTheDisclaimer(string fileName)
    {
        Assert.Contains("<DisclaimerCallout", ReadPage(fileName));
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
