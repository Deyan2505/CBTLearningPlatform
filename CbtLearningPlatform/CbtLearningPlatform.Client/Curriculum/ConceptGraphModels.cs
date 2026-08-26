namespace CbtLearningPlatform.Client.Curriculum;

/// <summary>Curriculum-lifecycle state of a concept — never learner-progress data (COGNITIVE_LEARNING_ARCHITECTURE_v1.md
/// §12). The platform has no persistent progress engine, so this must never read as "the learner has mastered this."</summary>
public enum ConceptState
{
    /// <summary>Catalogued, but its introducing week has no route yet — shown muted, orientation-only, never with a full definition.</summary>
    Upcoming,
    /// <summary>Its introducing week is routed; this is the first time the curriculum teaches it.</summary>
    Introduced,
    /// <summary>Introduced earlier, and at least one later routed week revisits/applies it again.</summary>
    Revisited
}

public enum RelationDirection { Directed, Bidirectional }

/// <summary>Minimal controlled vocabulary — every value is grounded in a real relationship already present in the
/// curriculum text (architecture v1.1 §11), not an abstractly pre-designed taxonomy.</summary>
public enum RelationType
{
    /// <summary>"поражда" — Ситуация → Автоматична мисъл → Реакция.</summary>
    LeadsTo,
    /// <summary>"разграничава се от" — напр. "актуализация" срещу "проверка на настроението".</summary>
    DiffersFrom,
    /// <summary>"част е от" — напр. "Проверка на настроението" е част от "Начало на сесията".</summary>
    IsPartOf,
    /// <summary>"предхожда" — forward-reference към бъдеща/друга седмица.</summary>
    Precedes,
    /// <summary>"укрепва" — напр. обратната връзка укрепва терапевтичния алианс.</summary>
    Supports,
    /// <summary>"пример е на" — конкретен сценарий, илюстриращ по-общ концепт.</summary>
    IsExampleOf
}

/// <summary>Single-parent hierarchy node — orientation/memory structure (Weekly Mind Map). Not for cross-links;
/// those belong in ConceptMapModel. ParentId is null only for the root.</summary>
public sealed record MindMapNode(
    string Id,
    string Label,
    string? ParentId,
    string? ShortDefinition,
    string? Anchor,
    ConceptState State);

public sealed record MindMapModel(
    string Title,
    string ScreenReaderSummary,
    IReadOnlyList<MindMapNode> Nodes);

/// <summary>Multi-parent network node — relationships between concepts (Concept Map). IsCrossReference marks a
/// node that this particular map only points at (another week already teaches/will teach it), not one this
/// page elaborates — kept separate from ConceptState, which describes the concept's own curriculum lifecycle,
/// not how this specific map chooses to display it.</summary>
public sealed record ConceptNode(
    string Id,
    string Label,
    string? Definition,
    int? IntroducedWeek,
    IReadOnlyList<int> RevisitedWeeks,
    string? Anchor,
    bool IsCrossReference = false);

public sealed record ConceptRelation(
    string FromId,
    string ToId,
    RelationType RelationType,
    string RelationLabel,
    RelationDirection Direction = RelationDirection.Directed);

public sealed record ConceptMapModel(
    string Title,
    string ScreenReaderSummary,
    IReadOnlyList<ConceptNode> Nodes,
    IReadOnlyList<ConceptRelation> Relations);

/// <summary>Derives ConceptState from which weeks are actually routed — mirrors CurriculumLabels.DeriveStatus()
/// so concept state and week status can never drift out of sync via two independently-maintained rules.</summary>
public static class ConceptStateResolver
{
    public static ConceptState Derive(int? introducedWeek, IReadOnlyList<int> revisitedWeeks, IReadOnlyList<CourseWeekDefinition> weeks)
    {
        bool IsRouted(int number) => weeks.Any(w => w.Number == number && w.Route is not null);

        if (introducedWeek is null || !IsRouted(introducedWeek.Value))
        {
            return ConceptState.Upcoming;
        }

        return revisitedWeeks.Any(IsRouted) ? ConceptState.Revisited : ConceptState.Introduced;
    }
}
