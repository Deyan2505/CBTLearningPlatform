using CbtLearningPlatform.Client.Curriculum;
using CbtLearningPlatform.Client.Interactive;

namespace CbtLearningPlatform.Tests;

/// <summary>ScenarioSimulator reuse contract (Active Learning Toolkit engine D): Week 6's real data satisfies it (so
/// nothing about Week 6 changes), other weeks may configure only the levels they need, and safety modes bound what is offered.</summary>
public sealed class ScenarioSimulatorReuseTests
{
    // ---- minimal, content-free fixtures: structure only ----
    private static readonly IReadOnlyList<string> Labels = ["s1", "s2", "s3"];
    private static readonly IReadOnlyList<RecognitionItem> Identify = [new("excerpt", "s1", "why", "unit")];
    private static readonly IReadOnlyList<MatchingPair> Matching = [new("e1", "s1"), new("e2", "s2")];
    private static readonly IReadOnlyList<OrderingStep> Ordering = [new("a", 0), new("b", 1), new("c", 2)];
    private static readonly NextStepScenario NextStep = new("situation",
        [new("x", true, "r", "u", "#a", "l"), new("y", false, "r", "u", "#a", "l")]);
    private static readonly IReadOnlyList<BranchNode> Branch =
    [
        new("start", "narrative", [new("go", "consequence", true, "r", "u", "end")], null),
        new("end", "narrative", [], "summary")
    ];

    private static Dictionary<string, object?> Params(
        IReadOnlyList<string>? labels = null,
        IReadOnlyList<RecognitionItem>? identify = null,
        IReadOnlyList<MatchingPair>? matching = null,
        IReadOnlyList<OrderingStep>? ordering = null,
        NextStepScenario? nextStep = null,
        string branchStart = "",
        IReadOnlyList<BranchNode>? branch = null,
        ActiveLearningSafetyMode mode = ActiveLearningSafetyMode.NormalLearning) => new()
    {
        ["Title"] = "T",
        ["Description"] = "D",
        ["StepLabels"] = labels ?? [],
        ["IdentifyItems"] = identify ?? [],
        ["MatchingPairs"] = matching ?? [],
        ["OrderingSteps"] = ordering ?? [],
        ["NextStepScenario"] = nextStep,
        ["BranchStartNodeId"] = branchStart,
        ["BranchNodes"] = branch ?? [],
        ["SafetyMode"] = mode
    };

    [Fact]
    public void Week6RealData_SatisfiesTheReuseContract_SoTheComponentNeverReplacesItWithAnError()
    {
        Week6SimulatorData data = Week6SimulatorData.Load();

        Assert.Empty(ScenarioSimulatorContract.Validate(
            data.StepLabels, data.IdentifyItems, data.MatchingPairs, data.OrderingSteps,
            data.NextStepScenario, data.BranchStartNodeId, data.BranchNodes));
    }

    [Fact]
    public void Contract_FlagsMalformedData()
    {
        IReadOnlyList<string> issues = ScenarioSimulatorContract.Validate(
            Labels,
            [new("excerpt", "not-a-label", "why", "unit")],
            [new("only", "one")],
            [new("a", 0), new("b", 0)],
            new NextStepScenario("s", [new("x", false, "r", "u", "#a", "l")]),
            "missing",
            [new("start", "n", [new("go", "c", true, "r", "u", "nowhere")], null)]);

        Assert.Contains(issues, i => i.Contains("not one of StepLabels"));
        Assert.Contains(issues, i => i.Contains("at least two distinct labels"));
        Assert.Contains(issues, i => i.Contains("exactly 0..n-1"));
        Assert.Contains(issues, i => i.Contains("at least two choices"));
        Assert.Contains(issues, i => i.Contains("at least one choice must be correct"));
        Assert.Contains(issues, i => i.Contains("start node 'missing' does not exist"));
        Assert.Contains(issues, i => i.Contains("unknown node 'nowhere'"));
    }

    [Fact]
    public void AWeekCanConfigureOnlyOrdering_AndTheOtherLevelsAreNotOffered()
    {
        string html = ComponentRender.Html<ScenarioSimulator>(Params(ordering: Ordering));

        Assert.Contains("scenario-simulator__order-list", html);
        Assert.DoesNotContain("scenario-simulator__level-tabs", html); // a single level needs no level switcher
        Assert.DoesNotContain("scenario-simulator__mode-tabs", html);  // a single mode needs no mode switcher
        Assert.DoesNotContain("scenario-simulator__config-error", html);
        Assert.DoesNotContain("scenario-simulator__matching-grid", html);
    }

    [Fact]
    public void AWeekCanConfigureOnlyMatching_AndOnlyBranching()
    {
        string matching = ComponentRender.Html<ScenarioSimulator>(Params(matching: Matching));
        Assert.Contains("scenario-simulator__matching-grid", matching);
        Assert.DoesNotContain("scenario-simulator__level-tabs", matching);

        string branching = ComponentRender.Html<ScenarioSimulator>(Params(branchStart: "start", branch: Branch));
        Assert.Contains("narrative", branching);
        Assert.DoesNotContain("scenario-simulator__level-tabs", branching);
    }

    [Fact]
    public void WithSeveralLevelsConfigured_OnlyTheAvailableLevelTabsAppear()
    {
        string html = ComponentRender.Html<ScenarioSimulator>(Params(ordering: Ordering, nextStep: NextStep, matching: Matching));

        Assert.Contains("Ниво A · Разпознаване", html);
        Assert.Contains("Ниво B · Приложение", html);
        Assert.DoesNotContain("Ниво C · Разсъждение", html);
    }

    [Fact]
    public void NoSelfGuidedSimulation_WithholdsBranchingButKeepsMatchingAndOrdering()
    {
        string html = ComponentRender.Html<ScenarioSimulator>(Params(
            matching: Matching, ordering: Ordering, branchStart: "start", branch: Branch,
            mode: ActiveLearningSafetyMode.NoSelfGuidedSimulation));

        Assert.DoesNotContain("Ниво C", html);
        Assert.Contains("Ниво A", html);
        Assert.Contains("Ниво B", html);
        Assert.Contains("active-learning__notice", html);
        Assert.Contains(ActiveLearningSafety.Profile(ActiveLearningSafetyMode.NoSelfGuidedSimulation).DefaultNotice, HtmlText.Decode(html));
    }

    [Fact]
    public void NoSelfGuidedSimulation_WithOnlyBranchingConfigured_ShowsAnUnavailableMessageInsteadOfASimulation()
    {
        string html = ComponentRender.Html<ScenarioSimulator>(Params(
            branchStart: "start", branch: Branch, mode: ActiveLearningSafetyMode.NoSelfGuidedSimulation));

        Assert.Contains("scenario-simulator__config-error", html);
        Assert.DoesNotContain("narrative", html);
    }

    [Theory]
    [InlineData(ActiveLearningSafetyMode.AcademicThirdPerson)]
    [InlineData(ActiveLearningSafetyMode.ProfessionalContext)]
    public void RestrictedModes_ShowAMandatoryNotice_ButKeepBranchingOnFixedVignettes(ActiveLearningSafetyMode mode)
    {
        string html = ComponentRender.Html<ScenarioSimulator>(Params(
            ordering: Ordering, branchStart: "start", branch: Branch, mode: mode));

        Assert.Contains(ActiveLearningSafety.Profile(mode).DefaultNotice, HtmlText.Decode(html));
        Assert.Contains("Ниво C · Разсъждение", html);
    }

    [Fact]
    public void NormalMode_AddsNothingToTheMarkup()
    {
        string html = ComponentRender.Html<ScenarioSimulator>(Params(ordering: Ordering));

        Assert.DoesNotContain("active-learning__notice", html);
    }

    [Fact]
    public void MalformedData_RendersAnUnavailableMessage_NotAnUnhandledException()
    {
        string html = ComponentRender.Html<ScenarioSimulator>(Params(ordering: [new("a", 0), new("b", 0)]));

        Assert.Contains("scenario-simulator__config-error", html);
        Assert.DoesNotContain("scenario-simulator__order-list", html);
    }
}
