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

/// <summary>Unique DOM ids for toolkit instances (radio-group names, aria-labelledby) so several engines can share a page.
/// A week may pass its own stable ComponentId instead.</summary>
internal static class ActiveLearningIds
{
    private static int _next;

    public static string Next(string kind) => $"al-{kind}-{Interlocked.Increment(ref _next)}";
}
