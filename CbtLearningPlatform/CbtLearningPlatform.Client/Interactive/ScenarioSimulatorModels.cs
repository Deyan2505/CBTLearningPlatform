namespace CbtLearningPlatform.Client.Interactive;

// Data contract for ScenarioSimulator (Week 6 v2 Deep Learning Blueprint §9/§10) — kept as
// plain, JSON-serializable records so the same component can be reused by any future Deep
// Learning Week: the week-specific content lives in the consuming page, never hardcoded here.

public sealed record RecognitionItem(string Excerpt, string CorrectStepLabel, string Explanation, string SourceUnit);

public sealed record MatchingPair(string Excerpt, string StepLabel);

public sealed record OrderingStep(string Text, int CorrectPosition);

public sealed record NextStepChoice(string Label, bool IsCorrect, string Reasoning, string SourceUnit, string BackLinkAnchor, string BackLinkLabel);

public sealed record NextStepScenario(string Situation, IReadOnlyList<NextStepChoice> Choices);

public sealed record BranchOption(string Label, string Consequence, bool IsRecommended, string Reasoning, string SourceUnit, string NextNodeId);

public sealed record BranchNode(string Id, string Narrative, IReadOnlyList<BranchOption> Options, string? Summary);
