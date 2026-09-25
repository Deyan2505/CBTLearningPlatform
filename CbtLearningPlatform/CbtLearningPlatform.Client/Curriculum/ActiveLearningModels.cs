namespace CbtLearningPlatform.Client.Curriculum;

// ACTIVE LEARNING TOOLKIT — data contracts. Plain, Blazor-free records: week-specific, source-grounded content is
// supplied BY THE WEEK PAGE (already-approved wording), never authored inside a shared engine. Each activity validates
// itself (Validate) so a week's own test can prove its data is well-formed, and the state machines refuse invalid data.

internal static class ActivityValidation
{
    public static void Require(List<string> issues, bool condition, string message)
    {
        if (!condition) issues.Add(message);
    }

    public static bool Blank(string? value) => string.IsNullOrWhiteSpace(value);

    public static void UniqueIds(List<string> issues, string what, IEnumerable<string> ids)
    {
        string[] all = [.. ids];
        Require(issues, all.All(id => !Blank(id)), $"{what}: every id must be non-blank.");
        Require(issues, all.Distinct(StringComparer.Ordinal).Count() == all.Length, $"{what}: ids must be unique.");
    }
}

// ---------------------------------------------------------------- A. classify / match

public enum ClassifyMatchMode
{
    /// <summary>Items are sorted into shared categories — several items may pick the same option.</summary>
    Classify,

    /// <summary>One-to-one pairing — each option may be used by at most one item.</summary>
    Match
}

public sealed record ClassifyMatchOption(string Id, string Label);

/// <param name="Explanation">Shown AFTER the learner commits — feedback is always explanatory, never a bare verdict.</param>
public sealed record ClassifyMatchItem(string Id, string Prompt, string CorrectOptionId, string Explanation, string? SourceRef = null);

public sealed record ClassifyMatchActivity(
    string Title,
    string Instruction,
    ClassifyMatchMode Mode,
    IReadOnlyList<ClassifyMatchOption> Options,
    IReadOnlyList<ClassifyMatchItem> Items)
{
    public IReadOnlyList<string> Validate()
    {
        List<string> issues = [];
        ActivityValidation.Require(issues, !ActivityValidation.Blank(Title), "Title is required.");
        ActivityValidation.Require(issues, !ActivityValidation.Blank(Instruction), "Instruction is required.");
        ActivityValidation.Require(issues, Options.Count >= 2, "At least two options are required.");
        ActivityValidation.Require(issues, Items.Count >= 1, "At least one item is required.");
        ActivityValidation.UniqueIds(issues, "Options", Options.Select(o => o.Id));
        ActivityValidation.UniqueIds(issues, "Items", Items.Select(i => i.Id));
        ActivityValidation.Require(issues, Options.All(o => !ActivityValidation.Blank(o.Label)), "Every option needs a label.");

        HashSet<string> optionIds = [.. Options.Select(o => o.Id)];
        foreach (ClassifyMatchItem item in Items)
        {
            ActivityValidation.Require(issues, !ActivityValidation.Blank(item.Prompt), $"Item '{item.Id}' needs a prompt.");
            ActivityValidation.Require(issues, !ActivityValidation.Blank(item.Explanation), $"Item '{item.Id}' needs an explanation (feedback is never a bare verdict).");
            ActivityValidation.Require(issues, optionIds.Contains(item.CorrectOptionId), $"Item '{item.Id}' names an unknown correct option '{item.CorrectOptionId}'.");
        }

        if (Mode == ClassifyMatchMode.Match)
        {
            ActivityValidation.Require(issues, Items.Count <= Options.Count, "Match mode needs at least as many options as items.");
            ActivityValidation.Require(issues,
                Items.Select(i => i.CorrectOptionId).Distinct(StringComparer.Ordinal).Count() == Items.Count,
                "Match mode is one-to-one: every item must have a different correct option.");
        }

        return issues;
    }
}

// ---------------------------------------------------------------- B. ordering builder

/// <param name="Explanation">Optional, shown AFTER the learner commits an order.</param>
public sealed record OrderingItem(string Id, string Text, string? Explanation = null);

/// <param name="CorrectSequence">The items in their CORRECT order (list position = correct position).</param>
/// <param name="Feedback">Optional overall explanation shown after a check.</param>
/// <param name="ShuffleSeed">Deterministic starting scramble; never equal to the correct order.</param>
public sealed record OrderingActivity(
    string Title,
    string Instruction,
    IReadOnlyList<OrderingItem> CorrectSequence,
    string? Feedback = null,
    string? SourceRef = null,
    int ShuffleSeed = 0)
{
    public IReadOnlyList<string> Validate()
    {
        List<string> issues = [];
        ActivityValidation.Require(issues, !ActivityValidation.Blank(Title), "Title is required.");
        ActivityValidation.Require(issues, !ActivityValidation.Blank(Instruction), "Instruction is required.");
        ActivityValidation.Require(issues, CorrectSequence.Count >= 2, "At least two items are required to order.");
        ActivityValidation.UniqueIds(issues, "Items", CorrectSequence.Select(i => i.Id));
        ActivityValidation.Require(issues, CorrectSequence.All(i => !ActivityValidation.Blank(i.Text)), "Every item needs text.");
        return issues;
    }
}

// ---------------------------------------------------------------- C. committed predict -> reveal

/// <param name="Feedback">Optional per-option explanation, shown only if the learner committed to THIS option.</param>
public sealed record PredictRevealOption(string Id, string Label, string? Feedback = null);

/// <param name="ActualOptionId">The real/source outcome. Null when the reveal is an explanation without a single right
/// answer — then only a comparison is shown, never a right/wrong verdict.</param>
/// <param name="Explanation">Never rendered until the learner has committed a prediction.</param>
public sealed record PredictRevealActivity(
    string Title,
    string Scenario,
    string Question,
    IReadOnlyList<PredictRevealOption> Options,
    string? ActualOptionId,
    string Explanation,
    string? SourceRef = null,
    bool AllowRetry = true)
{
    public IReadOnlyList<string> Validate()
    {
        List<string> issues = [];
        ActivityValidation.Require(issues, !ActivityValidation.Blank(Title), "Title is required.");
        ActivityValidation.Require(issues, !ActivityValidation.Blank(Scenario), "Scenario is required.");
        ActivityValidation.Require(issues, !ActivityValidation.Blank(Question), "Question is required.");
        ActivityValidation.Require(issues, !ActivityValidation.Blank(Explanation), "Explanation is required.");
        ActivityValidation.Require(issues, Options.Count >= 2, "At least two options are required.");
        ActivityValidation.UniqueIds(issues, "Options", Options.Select(o => o.Id));
        ActivityValidation.Require(issues, Options.All(o => !ActivityValidation.Blank(o.Label)), "Every option needs a label.");
        ActivityValidation.Require(issues,
            ActualOptionId is null || Options.Any(o => o.Id == ActualOptionId),
            $"ActualOptionId '{ActualOptionId}' is not one of the options.");
        return issues;
    }
}

// ---------------------------------------------------------------- D. case examination model (stateful)

/// <summary>One labelled line of the case as the learner first meets it (situation, thought, emotion, …).</summary>
public sealed record CaseFact(string Label, string Value);

public sealed record ExaminationOption(string Id, string Label);

/// <summary>One examination tool the learner can apply to the case. The learner first PREDICTS what applying it will
/// surface; only after committing does <paramref name="Finding"/> exist and join the board's running state.</summary>
/// <param name="Finding">What this tool actually reveals about the case — the week's approved, source-grounded text.</param>
public sealed record ExaminationTool(
    string Id,
    string Name,
    string Question,
    IReadOnlyList<ExaminationOption> Options,
    string CorrectOptionId,
    string Finding,
    string? SourceRef = null);

/// <summary>A case the learner examines by applying tools to it, watching the board's state grow. Unlike the check-style
/// engines this one has a MODEL the learner manipulates: the case starts in one state and ends in another, and the closing
/// <paramref name="Outcome"/> is withheld until every tool has been applied.</summary>
public sealed record CaseExaminationActivity(
    string Title,
    string Instruction,
    IReadOnlyList<CaseFact> Case,
    IReadOnlyList<ExaminationTool> Tools,
    string OutcomeTitle,
    string Outcome,
    string? SourceRef = null)
{
    public IReadOnlyList<string> Validate()
    {
        List<string> issues = [];
        ActivityValidation.Require(issues, !ActivityValidation.Blank(Title), "Title is required.");
        ActivityValidation.Require(issues, !ActivityValidation.Blank(Instruction), "Instruction is required.");
        ActivityValidation.Require(issues, !ActivityValidation.Blank(OutcomeTitle), "OutcomeTitle is required.");
        ActivityValidation.Require(issues, !ActivityValidation.Blank(Outcome), "Outcome is required (the model must end in a stated state).");
        ActivityValidation.Require(issues, Case.Count >= 1, "The case needs at least one fact.");
        ActivityValidation.Require(issues, Case.All(f => !ActivityValidation.Blank(f.Label) && !ActivityValidation.Blank(f.Value)), "Every case fact needs a label and a value.");
        ActivityValidation.Require(issues, Tools.Count >= 2, "At least two examination tools are required.");
        ActivityValidation.UniqueIds(issues, "Tools", Tools.Select(t => t.Id));

        foreach (ExaminationTool tool in Tools)
        {
            ActivityValidation.Require(issues, !ActivityValidation.Blank(tool.Name), $"Tool '{tool.Id}' needs a name.");
            ActivityValidation.Require(issues, !ActivityValidation.Blank(tool.Question), $"Tool '{tool.Id}' needs a question.");
            ActivityValidation.Require(issues, !ActivityValidation.Blank(tool.Finding), $"Tool '{tool.Id}' needs a finding (the state change it produces).");
            ActivityValidation.Require(issues, tool.Options.Count >= 2, $"Tool '{tool.Id}' needs at least two options.");
            ActivityValidation.UniqueIds(issues, $"Tool '{tool.Id}' options", tool.Options.Select(o => o.Id));
            ActivityValidation.Require(issues, tool.Options.All(o => !ActivityValidation.Blank(o.Label)), $"Tool '{tool.Id}': every option needs a label.");
            ActivityValidation.Require(issues, tool.Options.Any(o => o.Id == tool.CorrectOptionId), $"Tool '{tool.Id}' names an unknown correct option '{tool.CorrectOptionId}'.");
        }

        return issues;
    }
}

// ---------------------------------------------------------------- E. stateful model simulator

/// <summary>One visible slot in a model snapshot. Domain labels and values are supplied by the week.</summary>
public sealed record StatefulModelField(string Id, string Label, string Value);

/// <summary>A committed choice transitions the model. Consequence and source stay hidden until commit.</summary>
public sealed record StatefulModelChoice(
    string Id,
    string Label,
    string NextStateId,
    string Consequence,
    string? SourceRef = null);

/// <summary>A complete visible snapshot of the model at one point in an exploratory path.</summary>
public sealed record StatefulModelNode(
    string Id,
    string Phase,
    string Title,
    string Description,
    IReadOnlyList<StatefulModelField> Fields,
    IReadOnlyList<StatefulModelChoice> Choices);

/// <summary>Content-free contract for a closed-choice, multi-state simulator.</summary>
public sealed record StatefulModelActivity(
    string Title,
    string Instruction,
    string InitialStateId,
    IReadOnlyList<StatefulModelNode> States)
{
    public IReadOnlyList<string> Validate()
    {
        List<string> issues = [];
        ActivityValidation.Require(issues, !ActivityValidation.Blank(Title), "Title is required.");
        ActivityValidation.Require(issues, !ActivityValidation.Blank(Instruction), "Instruction is required.");
        ActivityValidation.Require(issues, States.Count >= 3, "At least three model states are required.");
        ActivityValidation.UniqueIds(issues, "States", States.Select(s => s.Id));

        HashSet<string> stateIds = [.. States.Select(s => s.Id)];
        ActivityValidation.Require(issues, stateIds.Contains(InitialStateId), $"Initial state '{InitialStateId}' does not exist.");
        foreach (StatefulModelNode state in States)
        {
            ActivityValidation.Require(issues, !ActivityValidation.Blank(state.Phase), $"State '{state.Id}' needs a phase.");
            ActivityValidation.Require(issues, !ActivityValidation.Blank(state.Title), $"State '{state.Id}' needs a title.");
            ActivityValidation.Require(issues, !ActivityValidation.Blank(state.Description), $"State '{state.Id}' needs a description.");
            ActivityValidation.Require(issues, state.Fields.Count >= 2, $"State '{state.Id}' needs at least two visible model fields.");
            ActivityValidation.UniqueIds(issues, $"State '{state.Id}' fields", state.Fields.Select(f => f.Id));
            ActivityValidation.Require(issues, state.Fields.All(f => !ActivityValidation.Blank(f.Label) && !ActivityValidation.Blank(f.Value)),
                $"State '{state.Id}': every field needs a label and value.");
            ActivityValidation.UniqueIds(issues, $"State '{state.Id}' choices", state.Choices.Select(c => c.Id));
            foreach (StatefulModelChoice choice in state.Choices)
            {
                ActivityValidation.Require(issues, !ActivityValidation.Blank(choice.Label), $"Choice '{choice.Id}' needs a label.");
                ActivityValidation.Require(issues, !ActivityValidation.Blank(choice.Consequence), $"Choice '{choice.Id}' needs a consequence.");
                ActivityValidation.Require(issues, stateIds.Contains(choice.NextStateId),
                    $"Choice '{choice.Id}' leads to unknown state '{choice.NextStateId}'.");
            }
        }

        StatefulModelNode? initial = States.FirstOrDefault(s => s.Id == InitialStateId);
        ActivityValidation.Require(issues,
            initial is not null && initial.Choices.Select(c => c.NextStateId).Distinct(StringComparer.Ordinal).Count() >= 2,
            "The initial state must offer at least two meaningful paths to different states.");
        ActivityValidation.Require(issues, States.Any(s => s.Choices.Count == 0), "At least one terminal state is required.");
        return issues;
    }
}

/// <summary>Unique DOM ids for toolkit instances (radio-group names, aria-labelledby) so several engines can share a page.
/// A week may pass its own stable ComponentId instead.</summary>
internal static class ActiveLearningIds
{
    private static int _next;

    public static string Next(string kind) => $"al-{kind}-{Interlocked.Increment(ref _next)}";
}
