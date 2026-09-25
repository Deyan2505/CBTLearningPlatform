namespace CbtLearningPlatform.Client.Curriculum;

// ACTIVE LEARNING TOOLKIT — pure state machines, one per engine. Blazor-free and in-memory only (same pattern as
// FinalAssessmentState): the components only render these, and unit tests exercise every commit/feedback rule directly.
// The single rule they all enforce: NO FEEDBACK EXISTS UNTIL THE LEARNER HAS COMMITTED. Until then the state exposes no
// verdict, no correct answer and no explanation — so a component cannot leak them even by mistake.

// ---------------------------------------------------------------- A. classify / match

public sealed class ClassifyMatchState
{
    private readonly Dictionary<string, string> _selections = new(StringComparer.Ordinal);

    public ClassifyMatchState(ClassifyMatchActivity activity)
    {
        IReadOnlyList<string> issues = activity.Validate();
        if (issues.Count > 0) throw new ArgumentException($"Invalid ClassifyMatchActivity: {string.Join(" ", issues)}", nameof(activity));
        Activity = activity;
    }

    public ClassifyMatchActivity Activity { get; }
    public bool IsChecked { get; private set; }
    public int Total => Activity.Items.Count;
    public int AnsweredCount => _selections.Count;
    public bool AllAnswered => AnsweredCount == Total;

    /// <summary>Match mode only: two items currently point at the same option.</summary>
    public bool HasDuplicateSelections =>
        Activity.Mode == ClassifyMatchMode.Match && _selections.Values.Distinct(StringComparer.Ordinal).Count() < _selections.Count;

    public bool CanCheck => !IsChecked && AllAnswered && !HasDuplicateSelections;

    public string? Selection(string itemId) => _selections.GetValueOrDefault(itemId);

    /// <summary>Records (or changes) the learner's choice. Ignored once checked or for unknown ids.</summary>
    public bool Select(string itemId, string optionId)
    {
        if (IsChecked
            || Activity.Items.All(i => i.Id != itemId)
            || Activity.Options.All(o => o.Id != optionId))
        {
            return false;
        }

        _selections[itemId] = optionId;
        return true;
    }

    public bool Check()
    {
        if (!CanCheck) return false;
        IsChecked = true;
        return true;
    }

    /// <summary>Null until the learner has checked — a verdict never exists before commitment.</summary>
    public bool? IsCorrect(string itemId)
    {
        if (!IsChecked) return null;
        ClassifyMatchItem? item = Activity.Items.FirstOrDefault(i => i.Id == itemId);
        return item is null ? null : Selection(itemId) == item.CorrectOptionId;
    }

    public int CorrectCount => IsChecked ? Activity.Items.Count(i => Selection(i.Id) == i.CorrectOptionId) : 0;

    public void Reset()
    {
        _selections.Clear();
        IsChecked = false;
    }
}

// ---------------------------------------------------------------- B. ordering builder

public sealed class OrderingBuilderState
{
    private List<int> _order;

    public OrderingBuilderState(OrderingActivity activity)
    {
        IReadOnlyList<string> issues = activity.Validate();
        if (issues.Count > 0) throw new ArgumentException($"Invalid OrderingActivity: {string.Join(" ", issues)}", nameof(activity));
        Activity = activity;
        _order = [.. InitialOrder(activity.CorrectSequence.Count, activity.ShuffleSeed)];
    }

    public OrderingActivity Activity { get; }
    public bool IsChecked { get; private set; }
    public bool SolutionRevealed { get; private set; }
    public int Total => Activity.CorrectSequence.Count;

    /// <summary>The learner's CURRENT arrangement (the correct order is never derivable from it before a check).</summary>
    public IReadOnlyList<OrderingItem> CurrentOrder => [.. _order.Select(i => Activity.CorrectSequence[i])];

    /// <summary>Deterministic, never-the-identity starting scramble for two or more items.</summary>
    public static IReadOnlyList<int> InitialOrder(int count, int seed)
    {
        int[] order = [.. Enumerable.Range(0, count)];
        uint state = unchecked((uint)seed * 2654435761u + 12345u);
        for (int i = count - 1; i > 0; i--)
        {
            state = unchecked(state * 1664525u + 1013904223u);
            int j = (int)((state >> 8) % (uint)(i + 1));
            (order[i], order[j]) = (order[j], order[i]);
        }

        if (count >= 2 && order.SequenceEqual(Enumerable.Range(0, count)))
        {
            order = [.. order.Skip(1), order[0]];
        }

        return order;
    }

    public bool CanMoveUp(int position) => !IsChecked && position > 0 && position < Total;
    public bool CanMoveDown(int position) => !IsChecked && position >= 0 && position < Total - 1;

    public bool MoveUp(int position)
    {
        if (!CanMoveUp(position)) return false;
        (_order[position - 1], _order[position]) = (_order[position], _order[position - 1]);
        return true;
    }

    public bool MoveDown(int position)
    {
        if (!CanMoveDown(position)) return false;
        (_order[position + 1], _order[position]) = (_order[position], _order[position + 1]);
        return true;
    }

    public bool Check()
    {
        if (IsChecked) return false;
        IsChecked = true;
        return true;
    }

    /// <summary>Null until checked.</summary>
    public bool? IsPlacedCorrectly(int position) =>
        !IsChecked || position < 0 || position >= Total ? null : _order[position] == position;

    public int CorrectPlacementCount => IsChecked ? _order.Select((item, position) => item == position).Count(c => c) : 0;
    public bool IsFullyCorrect => IsChecked && CorrectPlacementCount == Total;

    /// <summary>Available only after a check: shows the correct sequence when the learner is stuck.</summary>
    public bool RevealSolution()
    {
        if (!IsChecked) return false;
        SolutionRevealed = true;
        return true;
    }

    public IReadOnlyList<OrderingItem>? Solution => SolutionRevealed ? Activity.CorrectSequence : null;

    /// <summary>Unlocks for another attempt KEEPING the learner's arrangement, so they can fix it. Verdicts are cleared.</summary>
    public void Retry()
    {
        IsChecked = false;
        SolutionRevealed = false;
    }

    /// <summary>Back to the original scramble.</summary>
    public void Reset()
    {
        _order = [.. InitialOrder(Total, Activity.ShuffleSeed)];
        IsChecked = false;
        SolutionRevealed = false;
    }
}

// ---------------------------------------------------------------- C. committed predict -> reveal

public sealed class PredictRevealState
{
    public PredictRevealState(PredictRevealActivity activity)
    {
        IReadOnlyList<string> issues = activity.Validate();
        if (issues.Count > 0) throw new ArgumentException($"Invalid PredictRevealActivity: {string.Join(" ", issues)}", nameof(activity));
        Activity = activity;
    }

    public PredictRevealActivity Activity { get; }
    public string? Selection { get; private set; }
    public bool IsCommitted { get; private set; }
    public bool CanCommit => !IsCommitted && Selection is not null;

    public bool Select(string optionId)
    {
        if (IsCommitted || Activity.Options.All(o => o.Id != optionId)) return false;
        Selection = optionId;
        return true;
    }

    public bool Commit()
    {
        if (!CanCommit) return false;
        IsCommitted = true;
        return true;
    }

    /// <summary>Null until committed — the explanation and the actual outcome do not exist before that.</summary>
    public PredictRevealOption? Chosen => IsCommitted ? Activity.Options.First(o => o.Id == Selection) : null;

    public PredictRevealOption? Actual =>
        IsCommitted && Activity.ActualOptionId is not null ? Activity.Options.First(o => o.Id == Activity.ActualOptionId) : null;

    /// <summary>Null until committed, and null when the activity has no single right answer (comparison only).</summary>
    public bool? MatchesActual => Actual is null ? null : Chosen!.Id == Actual.Id;

    public string? Explanation => IsCommitted ? Activity.Explanation : null;

    public bool Retry()
    {
        if (!IsCommitted || !Activity.AllowRetry) return false;
        IsCommitted = false;
        Selection = null;
        return true;
    }
}

// ---------------------------------------------------------------- D. case examination model (stateful)

/// <summary>The one engine in the toolkit with a MODEL rather than a question set: the case starts in a stated state and the
/// learner changes it by applying tools. Applying a tool takes two steps — open it, predict what it will surface — and the
/// finding only joins <see cref="Findings"/> once the prediction is committed. The closing outcome does not exist until
/// every tool has been applied, so it cannot leak into the DOM early.</summary>
public sealed class CaseExaminationState
{
    private readonly Dictionary<string, string> _predictions = new(StringComparer.Ordinal);
    private readonly List<string> _applied = [];

    public CaseExaminationState(CaseExaminationActivity activity)
    {
        IReadOnlyList<string> issues = activity.Validate();
        if (issues.Count > 0) throw new ArgumentException($"Invalid CaseExaminationActivity: {string.Join(" ", issues)}", nameof(activity));
        Activity = activity;
    }

    public CaseExaminationActivity Activity { get; }

    /// <summary>The tool the learner has opened and is predicting for; null when the board is at rest.</summary>
    public string? ActiveToolId { get; private set; }

    /// <summary>The prediction chosen for the open tool, not yet committed.</summary>
    public string? PendingSelection { get; private set; }

    public int Total => Activity.Tools.Count;
    public int AppliedCount => _applied.Count;
    public bool IsComplete => AppliedCount == Total;
    public bool CanCommit => ActiveToolId is not null && PendingSelection is not null;

    /// <summary>Tools in the order the learner applied them — the board's running state.</summary>
    public IReadOnlyList<ExaminationTool> Findings => [.. _applied.Select(id => Activity.Tools.First(t => t.Id == id))];

    public bool IsApplied(string toolId) => _predictions.ContainsKey(toolId);

    /// <summary>Null until the tool has been applied — there is no verdict to leak before the learner commits.</summary>
    public bool? WasCorrect(string toolId) =>
        _predictions.TryGetValue(toolId, out string? predicted)
            ? predicted == Activity.Tools.First(t => t.Id == toolId).CorrectOptionId
            : null;

    public string? Prediction(string toolId) => _predictions.GetValueOrDefault(toolId);

    /// <summary>The closing state of the model — withheld until every tool has been applied.</summary>
    public string? Outcome => IsComplete ? Activity.Outcome : null;

    public int CorrectPredictionCount => _applied.Count(id => WasCorrect(id) == true);

    public bool Open(string toolId)
    {
        if (IsApplied(toolId) || Activity.Tools.All(t => t.Id != toolId)) return false;
        ActiveToolId = toolId;
        PendingSelection = null;
        return true;
    }

    public bool Select(string optionId)
    {
        if (ActiveToolId is not { } toolId) return false;
        ExaminationTool tool = Activity.Tools.First(t => t.Id == toolId);
        if (tool.Options.All(o => o.Id != optionId)) return false;
        PendingSelection = optionId;
        return true;
    }

    public bool Commit()
    {
        if (!CanCommit) return false;
        _predictions[ActiveToolId!] = PendingSelection!;
        _applied.Add(ActiveToolId!);
        ActiveToolId = null;
        PendingSelection = null;
        return true;
    }

    /// <summary>Backs out of an opened tool without applying it — the model's state is unchanged.</summary>
    public void Close()
    {
        ActiveToolId = null;
        PendingSelection = null;
    }

    public void Reset()
    {
        _predictions.Clear();
        _applied.Clear();
        ActiveToolId = null;
        PendingSelection = null;
    }
}

// ---------------------------------------------------------------- E. stateful model simulator

public sealed record StatefulModelTransition(
    string FromStateId,
    string ChoiceLabel,
    string Consequence,
    string? SourceRef,
    string ToStateId);

/// <summary>Pure in-memory state machine. A pending choice has no consequence; committing atomically
/// changes the visible snapshot and creates feedback.</summary>
public sealed class StatefulModelState
{
    private readonly List<StatefulModelTransition> _history = [];

    public StatefulModelState(StatefulModelActivity activity)
    {
        IReadOnlyList<string> issues = activity.Validate();
        if (issues.Count > 0) throw new ArgumentException($"Invalid StatefulModelActivity: {string.Join(" ", issues)}", nameof(activity));
        Activity = activity;
        CurrentStateId = activity.InitialStateId;
    }

    public StatefulModelActivity Activity { get; }
    public string CurrentStateId { get; private set; }
    public string? PendingChoiceId { get; private set; }
    public StatefulModelNode Current => Activity.States.Single(s => s.Id == CurrentStateId);
    public IReadOnlyList<StatefulModelTransition> History => _history;
    public StatefulModelTransition? LatestTransition => _history.LastOrDefault();
    public bool CanCommit => PendingChoiceId is not null;
    public bool IsTerminal => Current.Choices.Count == 0;

    public bool Select(string choiceId)
    {
        if (Current.Choices.All(c => c.Id != choiceId)) return false;
        PendingChoiceId = choiceId;
        return true;
    }

    public bool Commit()
    {
        if (PendingChoiceId is null) return false;
        StatefulModelNode from = Current;
        StatefulModelChoice choice = from.Choices.Single(c => c.Id == PendingChoiceId);
        _history.Add(new(from.Id, choice.Label, choice.Consequence, choice.SourceRef, choice.NextStateId));
        CurrentStateId = choice.NextStateId;
        PendingChoiceId = null;
        return true;
    }

    public void Reset()
    {
        CurrentStateId = Activity.InitialStateId;
        PendingChoiceId = null;
        _history.Clear();
    }
}
