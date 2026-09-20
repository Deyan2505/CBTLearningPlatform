namespace CbtLearningPlatform.Client.Interactive;

/// <summary>REUSE CONTRACT for ScenarioSimulator (Active Learning Toolkit engine D). The simulator itself is unchanged for
/// Week 6; this contract makes it safe for other weeks to configure only the levels they need:
/// <list type="bullet">
/// <item>Level A — recognition: <c>IdentifyItems</c> (needs <c>StepLabels</c>) and/or <c>MatchingPairs</c>.</item>
/// <item>Level B — application: <c>OrderingSteps</c> and/or <c>NextStepScenario</c>.</item>
/// <item>Level C — reasoning: <c>BranchNodes</c> + <c>BranchStartNodeId</c> (withheld in NoSelfGuidedSimulation mode).</item>
/// </list>
/// A level (or sub-mode) whose data is absent is simply not offered. Data that IS supplied must be well-formed —
/// <see cref="Validate"/> lists every problem, and a week's own test should assert it returns none. No CBT content lives here.</summary>
public static class ScenarioSimulatorContract
{
    public static bool HasIdentify(IReadOnlyList<string> stepLabels, IReadOnlyList<RecognitionItem> identifyItems) =>
        identifyItems.Count > 0 && stepLabels.Count > 0;

    public static bool HasMatching(IReadOnlyList<MatchingPair> matchingPairs) => matchingPairs.Count > 0;

    public static bool HasOrdering(IReadOnlyList<OrderingStep> orderingSteps) => orderingSteps.Count > 0;

    public static bool HasNextStep(NextStepScenario? nextStepScenario) =>
        nextStepScenario is { Choices.Count: > 0 };

    public static bool HasBranching(string branchStartNodeId, IReadOnlyList<BranchNode> branchNodes) =>
        branchNodes.Count > 0 && !string.IsNullOrEmpty(branchStartNodeId);

    public static IReadOnlyList<string> Validate(
        IReadOnlyList<string> stepLabels,
        IReadOnlyList<RecognitionItem> identifyItems,
        IReadOnlyList<MatchingPair> matchingPairs,
        IReadOnlyList<OrderingStep> orderingSteps,
        NextStepScenario? nextStepScenario,
        string branchStartNodeId,
        IReadOnlyList<BranchNode> branchNodes)
    {
        List<string> issues = [];

        if (identifyItems.Count > 0)
        {
            Require(issues, stepLabels.Count >= 2 && stepLabels.Distinct(StringComparer.Ordinal).Count() == stepLabels.Count,
                "Level A identify: StepLabels must contain at least two distinct labels.");
            foreach (RecognitionItem item in identifyItems)
            {
                Require(issues, !string.IsNullOrWhiteSpace(item.Excerpt), "Level A identify: every item needs an excerpt.");
                Require(issues, stepLabels.Contains(item.CorrectStepLabel), $"Level A identify: correct step '{item.CorrectStepLabel}' is not one of StepLabels.");
            }
        }

        if (matchingPairs.Count > 0)
        {
            Require(issues, matchingPairs.Select(p => p.StepLabel).Distinct(StringComparer.Ordinal).Count() >= 2,
                "Level A matching: at least two distinct labels are needed to match against.");
            Require(issues, matchingPairs.All(p => !string.IsNullOrWhiteSpace(p.Excerpt) && !string.IsNullOrWhiteSpace(p.StepLabel)),
                "Level A matching: every pair needs an excerpt and a label.");
        }

        if (orderingSteps.Count > 0)
        {
            Require(issues, orderingSteps.Count >= 2, "Level B ordering: at least two steps are needed.");
            Require(issues, orderingSteps.Select(s => s.CorrectPosition).Order().SequenceEqual(Enumerable.Range(0, orderingSteps.Count)),
                "Level B ordering: CorrectPosition values must be exactly 0..n-1, each once.");
            Require(issues, orderingSteps.All(s => !string.IsNullOrWhiteSpace(s.Text)), "Level B ordering: every step needs text.");
        }

        if (nextStepScenario is not null)
        {
            Require(issues, !string.IsNullOrWhiteSpace(nextStepScenario.Situation), "Level B next-step: the situation is required.");
            Require(issues, nextStepScenario.Choices.Count >= 2, "Level B next-step: at least two choices are needed.");
            Require(issues, nextStepScenario.Choices.Any(c => c.IsCorrect), "Level B next-step: at least one choice must be correct.");
        }

        if (branchNodes.Count > 0 || !string.IsNullOrEmpty(branchStartNodeId))
        {
            HashSet<string> ids = [.. branchNodes.Select(n => n.Id)];
            Require(issues, ids.Count == branchNodes.Count, "Level C branching: node ids must be unique.");
            Require(issues, ids.Contains(branchStartNodeId), $"Level C branching: start node '{branchStartNodeId}' does not exist.");
            foreach (BranchNode node in branchNodes)
            {
                if (node.Options.Count == 0)
                {
                    // A terminal node is shown as an end-of-scenario summary; it may carry no narrative of its own.
                    Require(issues, !string.IsNullOrWhiteSpace(node.Summary), $"Level C branching: terminal node '{node.Id}' needs a summary.");
                }
                else
                {
                    Require(issues, !string.IsNullOrWhiteSpace(node.Narrative), $"Level C branching: node '{node.Id}' needs a narrative.");
                }

                foreach (BranchOption option in node.Options)
                {
                    Require(issues, ids.Contains(option.NextNodeId), $"Level C branching: option '{option.Label}' leads to unknown node '{option.NextNodeId}'.");
                }
            }
        }

        return issues;
    }

    private static void Require(List<string> issues, bool condition, string message)
    {
        if (!condition) issues.Add(message);
    }
}
