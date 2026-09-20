using System.Text.RegularExpressions;
using CbtLearningPlatform.Client.Interactive;

namespace CbtLearningPlatform.Tests;

/// <summary>Week 6 is a locked regression target. This pins the initial markup ScenarioSimulator renders with Week 6's REAL
/// data (read from the page itself). The golden was captured from the component BEFORE the active-learning toolkit refactor,
/// so a green run proves the reuse-contract refactor left Week 6's rendering structurally identical (same elements, attributes,
/// classes and text; only insignificant inter-tag whitespace is normalised, because the refactor re-indented markup).
/// Interaction behaviour was baselined separately in a real browser. Regenerate ONLY on a deliberate, owner-approved Week 6
/// change: set UPDATE_GOLDEN=1.</summary>
public sealed class ScenarioSimulatorRegressionTests
{
    private static string GoldenPath => Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Tests", "Golden", "ScenarioSimulator.week6.initial.html");

    private static string Normalize(string html) =>
        Regex.Replace(Regex.Replace(html, @">\s+<", "><"), @"\s+", " ").Trim();

    [Fact]
    public void Week6Simulator_InitialMarkup_MatchesTheGoldenCapturedBeforeTheToolkitRefactor()
    {
        string html = ComponentRender.Html<ScenarioSimulator>(Week6SimulatorData.Load().AsParameters());

        if (Environment.GetEnvironmentVariable("UPDATE_GOLDEN") == "1")
        {
            Directory.CreateDirectory(Path.GetDirectoryName(GoldenPath)!);
            File.WriteAllText(GoldenPath, html);
        }

        Assert.True(File.Exists(GoldenPath), "Golden missing — run once with UPDATE_GOLDEN=1 against the unmodified component.");
        Assert.Equal(Normalize(File.ReadAllText(GoldenPath)), Normalize(html));
    }

    [Fact]
    public void Week6Simulator_InNormalMode_ShowsAllThreeLevelsAndNoToolkitNotice()
    {
        string html = ComponentRender.Html<ScenarioSimulator>(Week6SimulatorData.Load().AsParameters());

        Assert.Contains("Ниво A · Разпознаване", html);
        Assert.Contains("Ниво B · Приложение", html);
        Assert.Contains("Ниво C · Разсъждение", html);
        Assert.DoesNotContain("active-learning__notice", html);
        Assert.DoesNotContain("scenario-simulator__config-error", html);
    }
}
