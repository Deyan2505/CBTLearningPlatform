namespace CbtLearningPlatform.Curriculum;

public enum ConceptGraphMode { MindMap, ConceptMap, CaseMap }

/// <summary>Purely presentational — produced fresh by an adapter on every render, never stored or hand-authored
/// as a second copy of the content. ConceptGraph.razor reads only this shape; it never sees MindMapModel/
/// ConceptMapModel/CaseConceptualizationModel directly, so it cannot know what a "core belief" or "Ирина" is
/// (COGNITIVE_LEARNING_ARCHITECTURE_v1.md §8/§9).</summary>
public sealed record GraphRenderNode(
    string Id,
    string Label,
    string? ShortDefinition,
    string? Anchor,
    ConceptState? DisplayState,
    string? ParentId,
    bool IsCrossReference);

public sealed record GraphRenderEdge(
    string FromId,
    string ToId,
    string? Label,
    RelationDirection Direction);

public sealed record GraphRenderModel(
    string Title,
    string ScreenReaderSummary,
    ConceptGraphMode Mode,
    IReadOnlyList<GraphRenderNode> Nodes,
    IReadOnlyList<GraphRenderEdge> Edges);

public static class MindMapAdapter
{
    /// <summary>Validates before mapping — a single-parent tree with a dangling ParentId reference, or a
    /// parent cycle, is a content-authoring mistake, not a valid Mind Map
    /// (COGNITIVE_LEARNING_ARCHITECTURE_v1.md §5). The cycle check matters now that MindMapBranch.razor
    /// genuinely recurses to arbitrary depth — an undetected cycle would recurse forever at render time.</summary>
    public static GraphRenderModel ToRenderModel(MindMapModel model)
    {
        var byId = model.Nodes.ToDictionary(n => n.Id);

        foreach (var node in model.Nodes)
        {
            if (node.ParentId is not null && !byId.ContainsKey(node.ParentId))
            {
                throw new InvalidOperationException($"MindMapNode '{node.Id}' references a ParentId '{node.ParentId}' that does not exist in the model.");
            }
        }

        foreach (var node in model.Nodes)
        {
            var visited = new HashSet<string> { node.Id };
            var current = node;

            while (current.ParentId is not null)
            {
                if (!visited.Add(current.ParentId))
                {
                    throw new InvalidOperationException($"MindMapNode '{node.Id}' sits on a parent cycle through '{current.ParentId}'.");
                }

                current = byId[current.ParentId];
            }
        }

        return new(
            model.Title,
            model.ScreenReaderSummary,
            ConceptGraphMode.MindMap,
            model.Nodes.Select(n => new GraphRenderNode(n.Id, n.Label, n.ShortDefinition, n.Anchor, n.State, n.ParentId, IsCrossReference: false)).ToList(),
            Edges: []);
    }
}

public static class ConceptMapAdapter
{
    public static GraphRenderModel ToRenderModel(ConceptMapModel model, IReadOnlyList<CourseWeekDefinition> weeks) => new(
        model.Title,
        model.ScreenReaderSummary,
        ConceptGraphMode.ConceptMap,
        model.Nodes.Select(n => new GraphRenderNode(
            n.Id,
            n.Label,
            n.Definition,
            n.Anchor,
            ConceptStateResolver.Derive(n.IntroducedWeek, n.RevisitedWeeks, weeks),
            ParentId: null,
            n.IsCrossReference)).ToList(),
        model.Relations.Select(r => new GraphRenderEdge(r.FromId, r.ToId, r.RelationLabel, r.Direction)).ToList());
}

public static class CaseConceptualizationAdapter
{
    /// <summary>Renders one observation as a small chain (Situation → Thought → Emotion → Behavior → ...),
    /// skipping any field that observation didn't record — never fabricating a value to fill a gap.</summary>
    public static GraphRenderModel ToRenderModel(CaseCharacter character, CaseObservation observation)
    {
        var fields = new (string Id, string Label, string? Value)[]
        {
            ("situation", "Ситуация", observation.Situation),
            ("thought", "Автоматична мисъл", observation.Thought),
            ("emotion", "Емоция", observation.Emotion),
            ("body", "Телесна реакция", observation.Body),
            ("behavior", "Поведение", observation.Behavior),
            ("distortion", "Когнитивно изкривяване", observation.Distortion),
            ("intermediate-belief", "Междинно вярване", observation.IntermediateBelief),
            ("core-belief", "Основно вярване", observation.CoreBelief)
        };

        var present = fields.Where(f => !string.IsNullOrWhiteSpace(f.Value)).ToList();

        string NodeId(string fieldId) => $"{character.Id}-{fieldId}-w{observation.WeekNumber}";

        var nodes = present
            .Select(f => new GraphRenderNode(NodeId(f.Id), f.Label, f.Value, Anchor: null, DisplayState: null, ParentId: null, IsCrossReference: false))
            .ToList();

        var edges = new List<GraphRenderEdge>();
        for (var i = 0; i < present.Count - 1; i++)
        {
            edges.Add(new GraphRenderEdge(NodeId(present[i].Id), NodeId(present[i + 1].Id), "поражда", RelationDirection.Directed));
        }

        if (!string.IsNullOrWhiteSpace(observation.InterventionLink))
        {
            var interventionId = NodeId("intervention");
            nodes.Add(new GraphRenderNode(interventionId, "Терапевтична интервенция", observation.InterventionLink, Anchor: null, DisplayState: null, ParentId: null, IsCrossReference: true));
            if (present.Count > 0)
            {
                edges.Add(new GraphRenderEdge(NodeId(present[^1].Id), interventionId, "адресирано чрез", RelationDirection.Directed));
            }
        }

        return new GraphRenderModel(
            $"Case Conceptualization Map — {character.Name}",
            $"Как когнитивният модел се проявява в наблюдаваната от {character.Name} ситуация през Седмица {observation.WeekNumber} — само вече показаните в урока елементи.",
            ConceptGraphMode.CaseMap,
            nodes,
            edges);
    }
}
