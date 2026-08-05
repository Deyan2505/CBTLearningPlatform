using System.Reflection;

namespace CbtLearningPlatform.Tests;

public sealed class InteractiveUiTests
{
    [Fact]
    public void DarkThemeIsTheDefault_OnInitialHtmlMarkup()
    {
        string appRazor = ReadHostComponent("App.razor");

        Assert.Contains("data-theme=\"dark\"", appRazor);
    }

    [Fact]
    public void LightThemeRemainsAvailable_AsAnExplicitCssOverride()
    {
        string css = ReadCss();

        Assert.Contains(":root[data-theme=\"light\"]", css);
    }

    [Theory]
    [InlineData("CbtLearningPlatform.Client.Interactive.ThemeToggle")]
    [InlineData("CbtLearningPlatform.Client.Interactive.CbtModelDiagram")]
    [InlineData("CbtLearningPlatform.Client.Interactive.InterpretationExample")]
    [InlineData("CbtLearningPlatform.Client.Interactive.CategorizationCheck")]
    [InlineData("CbtLearningPlatform.Client.Interactive.CbtChainSimulator")]
    public void InteractiveComponent_BelongsToTheClientWebAssemblyAssembly(string typeName)
    {
        // Interactive WebAssembly render mode requires the component to live in the
        // .Client project (the assembly actually downloaded and run in the browser).
        Assembly clientAssembly = Assembly.Load("CbtLearningPlatform.Client");

        Type? type = clientAssembly.GetType(typeName);

        Assert.NotNull(type);
    }

    [Theory]
    [InlineData("Kpt.razor")]
    [InlineData("Modul2Lesson1.razor")]
    public void CbtModelDiagram_IsUsedOnRealPage(string fileName)
    {
        Assert.Contains("<CbtModelDiagram", ReadPage(fileName));
    }

    [Fact]
    public void CbtModelDiagram_DefinesAllFiveModelSteps()
    {
        string source = ReadClientComponent("CbtModelDiagram.razor");

        string[] expectedSteps = ["Ситуация", "Мисъл", "Емоция", "Телесна реакция", "Поведение"];

        foreach (string step in expectedSteps)
        {
            Assert.Contains($"\"{step}\"", source);
        }
    }

    [Fact]
    public void CbtModelDiagram_ProvidesAnAccessibleTextFallback()
    {
        string source = ReadClientComponent("CbtModelDiagram.razor");

        Assert.Contains("<details", source);
        Assert.Contains("<summary>", source);
    }

    [Fact]
    public void InterpretationExample_IsUsedOnARealPage()
    {
        Assert.Contains("<InterpretationExample", ReadPage("Kpt.razor"));
    }

    [Fact]
    public void InterpretationExample_OffersAtLeastTwoInterpretations()
    {
        string source = ReadClientComponent("InterpretationExample.razor");

        Assert.Contains("Сигурно е ядосан/а на мен.", source);
        Assert.Contains("Вероятно не ме е забелязал/а", source);
    }

    [Fact]
    public void InterpretationExample_HasNoScoringOrCorrectWrongLanguage()
    {
        string source = ReadClientComponent("InterpretationExample.razor");

        string[] forbiddenTerms = ["грешен", "грешна", "правилен отговор", "score", "Score"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void InterpretationExample_UsesAriaLiveForTheDynamicExplanation()
    {
        string source = ReadClientComponent("InterpretationExample.razor");

        Assert.Contains("aria-live", source);
    }

    [Fact]
    public void HostProgram_DefinesNoCustomServerEndpoints()
    {
        // Guards the "no server round-trip, no logging of interactions" requirement —
        // the interactive components must stay purely client-side.
        string programCs = ReadHostFile("Program.cs");

        Assert.DoesNotContain("MapPost", programCs);
        Assert.DoesNotContain("MapGet(\"/", programCs);
    }

    [Theory]
    [InlineData("ThemeToggle.razor")]
    [InlineData("CbtModelDiagram.razor")]
    [InlineData("InterpretationExample.razor")]
    [InlineData("CategorizationCheck.razor")]
    [InlineData("CbtChainSimulator.razor")]
    public void InteractiveComponent_DoesNotCallTheNetwork(string fileName)
    {
        string source = ReadClientComponent(fileName);

        Assert.DoesNotContain("HttpClient", source);
        Assert.DoesNotContain("fetch(", source);
    }

    [Theory]
    [InlineData("CbtModelDiagram.razor")]
    [InlineData("InterpretationExample.razor")]
    [InlineData("CategorizationCheck.razor")]
    [InlineData("CbtChainSimulator.razor")]
    public void InteractiveComponent_DoesNotUseBrowserStorage(string fileName)
    {
        string source = ReadClientComponent(fileName);

        Assert.DoesNotContain("localStorage", source);
        Assert.DoesNotContain("sessionStorage", source);
    }

    [Fact]
    public void InterpretationExample_BranchDiagramShowsBothPathsWithBodyReaction()
    {
        // Owner visual rejection round 2: both paths must render simultaneously (not
        // click-to-reveal), and each path includes a body reaction, not just thought/emotion/behavior.
        string source = ReadClientComponent("InterpretationExample.razor");

        Assert.Contains("branch-diagram", source);
        Assert.Contains("BodyReaction", source);
        Assert.DoesNotContain("_selected", source);
    }

    [Fact]
    public void InterpretationExample_EmphasisTogglesWithoutHidingEitherPath()
    {
        string source = ReadClientComponent("InterpretationExample.razor");

        Assert.Contains("is-emphasized", source);
        Assert.Contains("ToggleEmphasis", source);
    }

    [Fact]
    public void CbtChainSimulator_IsUsedOnTheRepresentativeWeekPage()
    {
        Assert.Contains("<CbtChainSimulator", ReadPage("Sedmica8.razor"));
    }

    [Fact]
    public void CbtChainSimulator_HasAtLeastThreeSituationOptions()
    {
        string source = ReadClientComponent("CbtChainSimulator.razor");

        string[] situations =
        [
            "Съобщение без отговор", "Грешка при нова задача",
            "Промяна на уговорка в последния момент", "Закъснение поради трафик"
        ];

        int presentCount = situations.Count(situation => source.Contains(situation));
        Assert.True(presentCount >= 3, $"Expected at least 3 situation options, found {presentCount}.");
    }

    [Fact]
    public void CbtChainSimulator_ProducesEmotionBodyReactionAndBehaviorOutputs()
    {
        string source = ReadClientComponent("CbtChainSimulator.razor");

        Assert.Contains("BodyReaction", source);
        Assert.Contains("Поведение", source);
        Assert.Contains("тревога", source);
    }

    [Fact]
    public void CbtChainSimulator_HasAResetControl()
    {
        string source = ReadClientComponent("CbtChainSimulator.razor");

        Assert.Contains("Нулирай", source);
        Assert.Contains("Reset", source);
    }

    [Fact]
    public void CbtChainSimulator_HasNoScoringDiagnosisOrPersonalFreeText()
    {
        string source = ReadClientComponent("CbtChainSimulator.razor");

        string[] forbiddenTerms = ["грешен", "грешна", "правилен отговор", "score", "Score", "диагноза", "диагноз"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }

        Assert.DoesNotContain("<textarea", source);
        Assert.DoesNotContain("type=\"text\"", source);
    }

    [Fact]
    public void CbtChainSimulator_UsesAriaLiveForTheLiveOutput()
    {
        Assert.Contains("aria-live", ReadClientComponent("CbtChainSimulator.razor"));
    }

    [Fact]
    public void CategorizationCheck_IsUsedOnTheRepresentativeWeekPage()
    {
        Assert.Contains("<CategorizationCheck", ReadPage("Sedmica8.razor"));
    }

    [Fact]
    public void CategorizationCheck_OffersAllFourModelCategories()
    {
        string source = ReadClientComponent("CategorizationCheck.razor");

        foreach (string category in new[] { "Мисъл", "Емоция", "Телесна реакция", "Поведение" })
        {
            Assert.Contains($"\"{category}\"", source);
        }
    }

    [Fact]
    public void CategorizationCheck_HasNoScoringOrCorrectWrongLanguage()
    {
        string source = ReadClientComponent("CategorizationCheck.razor");

        string[] forbiddenTerms = ["грешен", "грешна", "правилен отговор", "score", "Score"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void CategorizationCheck_UsesAriaLiveForTheDynamicExplanation()
    {
        Assert.Contains("aria-live", ReadClientComponent("CategorizationCheck.razor"));
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    private static string ReadHostComponent(string fileName)
    {
        string componentsDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components");
        return File.ReadAllText(Path.Combine(componentsDirectory, fileName));
    }

    private static string ReadHostFile(string fileName)
    {
        string projectDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform");
        return File.ReadAllText(Path.Combine(projectDirectory, fileName));
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
