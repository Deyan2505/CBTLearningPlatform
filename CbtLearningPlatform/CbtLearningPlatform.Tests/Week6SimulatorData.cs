using System.Reflection;
using CbtLearningPlatform.Client.Components.Pages;
using CbtLearningPlatform.Client.Interactive;

namespace CbtLearningPlatform.Tests;

/// <summary>The REAL data Week 6 hands to ScenarioSimulator, read straight from the locked Sedmica6 page (private fields, after
/// OnInitialized) — never copied, so the regression tests below always pin what the page really renders.</summary>
internal sealed record Week6SimulatorData(
    IReadOnlyList<string> StepLabels,
    IReadOnlyList<RecognitionItem> IdentifyItems,
    IReadOnlyList<MatchingPair> MatchingPairs,
    IReadOnlyList<OrderingStep> OrderingSteps,
    NextStepScenario NextStepScenario,
    string BranchStartNodeId,
    IReadOnlyList<BranchNode> BranchNodes)
{
    private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;

    public static Week6SimulatorData Load()
    {
        Sedmica6 page = new();
        typeof(Sedmica6).GetMethod("OnInitialized", Private)?.Invoke(page, null);

        T Field<T>(string name) =>
            (T)(typeof(Sedmica6).GetField(name, Private)?.GetValue(page)
                ?? throw new InvalidOperationException($"Sedmica6.{name} not found or null."));

        return new Week6SimulatorData(
            Field<IReadOnlyList<string>>("_stepLabels"),
            Field<IReadOnlyList<RecognitionItem>>("_identifyItems"),
            Field<IReadOnlyList<MatchingPair>>("_matchingPairs"),
            Field<IReadOnlyList<OrderingStep>>("_orderingSteps"),
            Field<NextStepScenario>("_nextStepScenario"),
            Field<string>("_branchStartId"),
            Field<IReadOnlyList<BranchNode>>("_branchNodes"));
    }

    public Dictionary<string, object?> AsParameters(string title = "Session Structure & Decision Simulator", string description = "Week 6 golden") => new()
    {
        ["Title"] = title,
        ["Description"] = description,
        ["StepLabels"] = StepLabels,
        ["IdentifyItems"] = IdentifyItems,
        ["MatchingPairs"] = MatchingPairs,
        ["OrderingSteps"] = OrderingSteps,
        ["NextStepScenario"] = NextStepScenario,
        ["BranchStartNodeId"] = BranchStartNodeId,
        ["BranchNodes"] = BranchNodes
    };
}
