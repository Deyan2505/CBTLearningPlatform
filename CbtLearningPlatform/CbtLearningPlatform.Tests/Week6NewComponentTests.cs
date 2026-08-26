using System.Reflection;

namespace CbtLearningPlatform.Tests;

/// <summary>Component-level tests for the three new reusable components introduced by the
/// Week 6 v2 Deep Learning Blueprint (§8, owner-approved names): WhatIfBox, SourceArtifact
/// (both static SSR, Components/Shared), and ScenarioSimulator (Interactive WebAssembly,
/// Client/Interactive). All three were justified individually as project-wide reusable
/// patterns, not Week-6-specific one-offs — see Week6ContentSliceTests for their in-page
/// usage assertions.</summary>
public sealed class Week6NewComponentTests
{
    [Fact]
    public void WhatIfBox_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Shared.WhatIfBox"));
    }

    [Fact]
    public void WhatIfBox_UsesNativeDetailsSummary_WithAQuestionPrefix()
    {
        string source = ReadSharedComponent("WhatIfBox.razor");

        Assert.Contains("<details class=\"what-if-box\">", source);
        Assert.Contains("<summary>", source);
        Assert.Contains("what-if-box__prefix", source);
        Assert.Contains("Какво ако…", source);
    }

    [Fact]
    public void WhatIfBox_HasRequiredParameters()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");
        Type type = assembly.GetType("CbtLearningPlatform.Client.Components.Shared.WhatIfBox")!;

        Assert.NotNull(type.GetProperty("Question"));
        Assert.NotNull(type.GetProperty("ChildContent"));
    }

    [Fact]
    public void SourceArtifact_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Shared.SourceArtifact"));
    }

    [Fact]
    public void SourceArtifact_IsSemanticFigureNotAnImage()
    {
        string source = ReadSharedComponent("SourceArtifact.razor");

        Assert.Contains("<figure class=\"source-artifact\"", source);
        Assert.Contains("<figcaption>", source);
        Assert.Contains("Възпроизведен учебен артефакт", source);
        Assert.DoesNotContain("<img", source);
    }

    [Fact]
    public void SourceArtifact_HasRequiredParameters_TitleSourceLabelAndContent()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");
        Type type = assembly.GetType("CbtLearningPlatform.Client.Components.Shared.SourceArtifact")!;

        Assert.NotNull(type.GetProperty("Title"));
        Assert.NotNull(type.GetProperty("SourceLabel"));
        Assert.NotNull(type.GetProperty("ChildContent"));
    }

    [Fact]
    public void ScenarioSimulator_BelongsToTheClientWebAssemblyAssembly()
    {
        Assembly clientAssembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(clientAssembly.GetType("CbtLearningPlatform.Client.Interactive.ScenarioSimulator"));
    }

    [Fact]
    public void ScenarioSimulator_IsUsedOnWeek6()
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages");
        string source = File.ReadAllText(Path.Combine(pagesDirectory, "Sedmica6.razor"));

        Assert.Contains("<ScenarioSimulator", source);
    }

    [Fact]
    public void ScenarioSimulator_DoesNotCallTheNetwork()
    {
        string source = ReadClientComponent("ScenarioSimulator.razor");

        Assert.DoesNotContain("HttpClient", source);
        Assert.DoesNotContain("fetch(", source);
    }

    [Fact]
    public void ScenarioSimulator_DoesNotUseBrowserStorage()
    {
        string source = ReadClientComponent("ScenarioSimulator.razor");

        Assert.DoesNotContain("localStorage", source);
        Assert.DoesNotContain("sessionStorage", source);
    }

    [Fact]
    public void ScenarioSimulator_HasNoScoringOrPersonalFreeTextInput()
    {
        string source = ReadClientComponent("ScenarioSimulator.razor");

        Assert.DoesNotContain("<textarea", source);
        Assert.DoesNotContain("type=\"text\"", source);
        string[] forbiddenTerms = ["score", "Score", "диагноза", "диагноз"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void ScenarioSimulator_HasAllThreeLevels()
    {
        string source = ReadClientComponent("ScenarioSimulator.razor");

        Assert.Contains("Ниво A · Разпознаване", source);
        Assert.Contains("Ниво B · Приложение", source);
        Assert.Contains("Ниво C · Разсъждение", source);
    }

    [Fact]
    public void ScenarioSimulator_LevelA_HasIdentifyAndMatchingModes_NotOnlyMultipleChoice()
    {
        string source = ReadClientComponent("ScenarioSimulator.razor");

        Assert.Contains("Разпознай стъпката", source);
        Assert.Contains("Съпоставяне", source);
    }

    [Fact]
    public void ScenarioSimulator_LevelB_HasOrderingWithKeyboardControls_NoDragRequirement()
    {
        string source = ReadClientComponent("ScenarioSimulator.razor");

        Assert.Contains("Подреждане", source);
        Assert.Contains("Премести нагоре", source);
        Assert.Contains("Премести надолу", source);
        Assert.DoesNotContain("draggable", source);
    }

    [Fact]
    public void ScenarioSimulator_LevelC_IsABranchingStateMachine_NotACollapsedSingleQuestion()
    {
        string source = ReadClientComponent("ScenarioSimulator.razor");

        Assert.Contains("BranchNodes", source);
        Assert.Contains("_currentNodeId", source);
        Assert.Contains("Advance", source);
        Assert.Contains("Продължи", source);
        Assert.Contains("Започни отначало", source);
    }

    [Fact]
    public void ScenarioSimulator_FeedbackUsesAriaLive()
    {
        string source = ReadClientComponent("ScenarioSimulator.razor");

        Assert.Contains("aria-live=\"polite\"", source);
    }

    [Fact]
    public void ScenarioSimulator_EveryFeedbackPathCitesASourceUnit()
    {
        string source = ReadClientComponent("ScenarioSimulator.razor");

        Assert.Contains("scenario-simulator__feedback-source", source);
        Assert.Contains("SourceUnit", source);
    }

    [Fact]
    public void ScenarioSimulator_DataModelsAreDefinedAsPlainRecords()
    {
        string source = File.ReadAllText(Path.Combine(
            TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Interactive", "ScenarioSimulatorModels.cs"));

        string[] expectedTypes =
        [
            "RecognitionItem", "MatchingPair", "OrderingStep", "NextStepChoice",
            "NextStepScenario", "BranchOption", "BranchNode"
        ];

        foreach (string type in expectedTypes)
        {
            Assert.Contains($"record {type}(", source);
        }
    }

    private static string ReadSharedComponent(string fileName)
    {
        string sharedDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Shared");
        return File.ReadAllText(Path.Combine(sharedDirectory, fileName));
    }

    private static string ReadClientComponent(string fileName)
    {
        string interactiveDirectory = Path.Combine(
            TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Interactive");
        return File.ReadAllText(Path.Combine(interactiveDirectory, fileName));
    }
}
