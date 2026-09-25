namespace CbtLearningPlatform.Client.Curriculum;

// ACTIVE LEARNING STANDARD — mandatory, project-wide (owner decision; root AGENTS.md, ADR-011).
//
// This file is the weekly architecture CONTRACT: metadata and rules only. It never contains CBT
// content, examples or cases — a week declares WHICH learning elements it has (type + where to find
// them in its page), never what they teach. Pure, UI-free and safety-blind by construction:
// Evaluate() takes the declaration and nothing else, so no safety tier can exempt a week.

/// <summary>Structural (learning-architecture) state of a week. NOT a content-approval state — a week's
/// prose stays OWNER APPROVED / LOCKED in either case. There is deliberately no "not applicable" or
/// "exempt" value: safety tiers change the FORM of interaction, never remove it.</summary>
public enum StructuralStatus
{
    /// <summary>All six gates (Mind Map, visual, simulator, retrieval, application feedback, final assessment) pass.</summary>
    Compliant,

    /// <summary>OWNER APPROVED CONTENT / STRUCTURAL ENRICHMENT REQUIRED — content approval stays valid,
    /// prose stays locked, only the learning architecture is reopened for active-learning enrichment.</summary>
    StructuralEnrichmentRequired
}

public enum ActiveLearningGate
{
    MindMapGate,
    VisualLearningModelGate,
    SimulatorInteractiveModelGate,
    RetrievalResponseGate,
    ApplicationFeedbackGate,
    FinalAssessmentGate
}

/// <summary>What structure a visual learning model makes visible (never what it says).</summary>
public enum VisualModelKind
{
    /// <summary>Weekly Mind Map — orientation/memory hierarchy. Counts as the week's visual model ONLY when
    /// it truly represents the week's central learning structure (RepresentsCentralStructure).</summary>
    MindMap,
    Hierarchy,
    Process,
    Sequence,
    Cycle,
    Comparison,
    Timeline,
    ConceptNetwork,
    DecisionModel,
    CaseModel
}

/// <summary>Family of the meaningful active-learning interaction a week offers before its Final Assessment.</summary>
public enum InteractionFamily
{
    InteractiveModel,
    Simulator,
    ClassifyMatch,
    OrderingBuilder,
    PredictCommit,
    BranchingCase,
    RelationshipMapper
}

/// <summary>What the learner does: they must commit a response (or change the model), and only THEN receive
/// feedback, a consequence, a comparison or a reveal.</summary>
public enum LearnerResponseKind
{
    Choose,
    Classify,
    Order,
    Compare,
    Predict,
    ManipulateModel,
    MapRelationships,
    Decide
}

/// <param name="Marker">Where the model lives in the week's page markup — a component tag (e.g. "&lt;HistoricalTimeline"),
/// a registered CSS construct, or a ConceptGraph ComponentId literal. Verified against the page by the gate.</param>
/// <param name="RepresentsCentralStructure">Only meaningful for <see cref="VisualModelKind.MindMap"/>.</param>
public sealed record VisualModelDeclaration(VisualModelKind Kind, string Marker, bool RepresentsCentralStructure = false);

/// <param name="Component">Razor component name without "&lt;", e.g. "ScenarioSimulator".</param>
public sealed record InteractionDeclaration(InteractionFamily Family, string Component);

/// <param name="Component">Razor component name without "&lt;".</param>
public sealed record LearnerResponseDeclaration(LearnerResponseKind Response, string Component);

/// <summary>The presence/type declaration every week makes. See <see cref="ActiveLearningCatalog"/>.</summary>
public sealed record WeekLearningArchitecture(
    int WeekNumber,
    StructuralStatus Status,
    IReadOnlyList<VisualModelDeclaration> VisualModels,
    IReadOnlyList<InteractionDeclaration> Interactions,
    IReadOnlyList<LearnerResponseDeclaration> LearnerResponses,
    bool DeclaresFinalAssessment);

public sealed record ActiveLearningFinding(ActiveLearningGate Gate, string Reason);

/// <summary>How one learning-layer component may be counted. Empty sets mean "does not qualify" and
/// <paramref name="Reason"/> says why.</summary>
public sealed record ComponentQualification(
    IReadOnlySet<InteractionFamily> Interaction,
    IReadOnlySet<LearnerResponseKind> Response,
    string Reason)
{
    public bool QualifiesAsInteraction => Interaction.Count > 0;
    public bool QualifiesAsResponse => Response.Count > 0;
}

public static class ActiveLearningStandard
{
    private static readonly IReadOnlySet<InteractionFamily> NoInteraction = new HashSet<InteractionFamily>();
    private static readonly IReadOnlySet<LearnerResponseKind> NoResponse = new HashSet<LearnerResponseKind>();

    private static ComponentQualification Excluded(string reason) => new(NoInteraction, NoResponse, reason);

    /// <summary>Components that implement a real stateful, multi-path model. Retrieval-only engines are
    /// deliberately absent even though they are meaningful active learning.</summary>
    public static IReadOnlySet<string> SimulatorComponents { get; } = new HashSet<string>
    {
        "ScenarioSimulator", "CbtChainSimulator", "CaseExaminationSimulator", "StatefulModelSimulator"
    };

    public static IReadOnlySet<string> RetrievalComponents { get; } = new HashSet<string>
    {
        "ScenarioSimulator", "ClassifyMatchCheck", "OrderingBuilder", "PredictReveal", "CaseExaminationSimulator"
    };

    public static IReadOnlySet<string> ApplicationFeedbackComponents { get; } = new HashSet<string>
    {
        "ScenarioSimulator", "CbtChainSimulator", "CaseExaminationSimulator", "StatefulModelSimulator"
    };

    /// <summary>Every learning-layer component the project ships, classified. A new interactive island must
    /// be added here (the gate fails on an unclassified file in /Interactive) — that is the single place
    /// where a new engine becomes able to satisfy the standard. Qualification is by DESIGN (does the learner
    /// commit or manipulate, and only then get feedback?), never by the name of the week or its safety tier.</summary>
    public static IReadOnlyDictionary<string, ComponentQualification> Components { get; } =
        new Dictionary<string, ComponentQualification>
        {
            ["ScenarioSimulator"] = new(
                new HashSet<InteractionFamily> { InteractionFamily.Simulator, InteractionFamily.ClassifyMatch, InteractionFamily.OrderingBuilder, InteractionFamily.BranchingCase },
                new HashSet<LearnerResponseKind> { LearnerResponseKind.Choose, LearnerResponseKind.Classify, LearnerResponseKind.Order, LearnerResponseKind.MapRelationships, LearnerResponseKind.Decide },
                "Level A matching / B ordering + next-step / C branching: learner commits, then gets a verdict."),
            ["CbtChainSimulator"] = new(
                new HashSet<InteractionFamily> { InteractionFamily.Simulator, InteractionFamily.InteractiveModel },
                new HashSet<LearnerResponseKind> { LearnerResponseKind.ManipulateModel },
                "Learner changes the model's inputs and observes the resulting chain."),
            ["SchemaFilterDemonstration"] = new(
                new HashSet<InteractionFamily> { InteractionFamily.InteractiveModel },
                new HashSet<LearnerResponseKind> { LearnerResponseKind.ManipulateModel },
                "Learner toggles the model's filter and observes how the same data changes."),

            // Active Learning Toolkit engines: the learner commits a response first; feedback exists only afterwards.
            ["ClassifyMatchCheck"] = new(
                new HashSet<InteractionFamily> { InteractionFamily.ClassifyMatch },
                new HashSet<LearnerResponseKind> { LearnerResponseKind.Classify, LearnerResponseKind.Choose, LearnerResponseKind.MapRelationships },
                "Data-driven classify/match: learner commits a choice per item, then checks and receives explanatory verdicts."),
            ["OrderingBuilder"] = new(
                new HashSet<InteractionFamily> { InteractionFamily.OrderingBuilder },
                new HashSet<LearnerResponseKind> { LearnerResponseKind.Order },
                "Data-driven ordering builder: learner builds an order with non-drag controls, then submits a check."),
            ["PredictReveal"] = new(
                new HashSet<InteractionFamily> { InteractionFamily.PredictCommit },
                new HashSet<LearnerResponseKind> { LearnerResponseKind.Predict, LearnerResponseKind.Choose, LearnerResponseKind.Compare },
                "Committed predict → reveal: the explanation and actual outcome do not exist until the learner commits a prediction."),
            ["CaseExaminationSimulator"] = new(
                new HashSet<InteractionFamily> { InteractionFamily.Simulator, InteractionFamily.InteractiveModel },
                new HashSet<LearnerResponseKind> { LearnerResponseKind.ManipulateModel, LearnerResponseKind.Predict, LearnerResponseKind.Decide },
                "Stateful case model: the learner applies examination tools to a case, predicting each tool's finding before it joins the board; the closing outcome exists only once every tool has been applied."),
            ["StatefulModelSimulator"] = new(
                new HashSet<InteractionFamily> { InteractionFamily.Simulator, InteractionFamily.InteractiveModel, InteractionFamily.BranchingCase },
                new HashSet<LearnerResponseKind> { LearnerResponseKind.Choose, LearnerResponseKind.ManipulateModel, LearnerResponseKind.Decide },
                "Closed-choice state machine: a committed choice changes the visible model snapshot, reveals a sourced consequence, and supports multiple meaningful paths."),
            ["ActiveLearningFrame"] = Excluded("Presentational frame (heading, instruction, safety notice) shared by the toolkit engines; not an interaction itself."),

            ["CategorizationCheck"] = Excluded("Reveal-only: the learner classifies mentally and clicks to reveal; no committed response. Superseded for new work by the commit-then-feedback ClassifyMatchCheck."),
            ["InterpretationExample"] = Excluded("Emphasis toggle over two always-visible paths; no decision, no feedback."),
            ["CbtModelDiagram"] = Excluded("Select-to-read step explorer (tabbed reveal)."),
            ["CognitiveHierarchyExplorer"] = Excluded("Select-to-read level explorer (tabbed reveal)."),
            ["ResearchTurnStepper"] = Excluded("Select-to-read step explorer (tabbed reveal)."),
            ["SocraticDialogueExplorer"] = Excluded("Select-to-read step explorer (tabbed reveal)."),

            ["FinalAssessment"] = Excluded("Scored end-of-week check — the standard requires interaction BEFORE it and never counts it as that interaction."),
            ["ProgressiveExplanation"] = Excluded("Plain <details> reveal of an explanation."),
            ["WhatIfBox"] = Excluded("Plain <details> reveal; the question is not committed before the answer shows."),
            ["ConceptGraph"] = Excluded("Mind Map / concept map rendering — expand/collapse is not a learner response."),
            ["MindMapBranch"] = Excluded("Mind Map branch — expand/collapse is not a learner response."),
            ["WeekCompletionControl"] = Excluded("Completion control."),
            ["ThemeToggle"] = Excluded("Site chrome.")
        };

    /// <summary>Registered visual constructs → the model kinds they may declare. Plain lists and bare tables are
    /// deliberately absent: a static list/table is not a visual learning model.</summary>
    public static IReadOnlyDictionary<string, IReadOnlySet<VisualModelKind>> VisualConstructs { get; } =
        new Dictionary<string, IReadOnlySet<VisualModelKind>>
        {
            ["concept-map__flow"] = new HashSet<VisualModelKind> { VisualModelKind.Process, VisualModelKind.Sequence, VisualModelKind.Hierarchy },
            ["cascade-loop"] = new HashSet<VisualModelKind> { VisualModelKind.Cycle },
            ["guided-practice-sequence"] = new HashSet<VisualModelKind> { VisualModelKind.Process, VisualModelKind.Sequence },
            ["decision-branch"] = new HashSet<VisualModelKind> { VisualModelKind.DecisionModel },
            ["category-compare"] = new HashSet<VisualModelKind> { VisualModelKind.Comparison },
            ["comparison-matrix"] = new HashSet<VisualModelKind> { VisualModelKind.Comparison },
            ["learning-path-diagram"] = new HashSet<VisualModelKind> { VisualModelKind.Process },
            ["cbt-diagram"] = new HashSet<VisualModelKind> { VisualModelKind.Process },
            ["<HistoricalTimeline"] = new HashSet<VisualModelKind> { VisualModelKind.Timeline }
        };

    /// <summary>A non-Mind-Map ConceptGraph is declared by its ComponentId literal.</summary>
    public const string ConceptGraphIdPrefix = "ComponentId=\"";

    private static readonly IReadOnlySet<VisualModelKind> ConceptGraphKinds = new HashSet<VisualModelKind>
    {
        VisualModelKind.Hierarchy, VisualModelKind.ConceptNetwork, VisualModelKind.CaseModel,
        VisualModelKind.Process, VisualModelKind.Sequence
    };

    private const string MindMapMarker = "mindmap";

    public static IReadOnlyList<ActiveLearningFinding> Evaluate(WeekLearningArchitecture week)
    {
        var findings = new List<ActiveLearningFinding>();

        // MindMapGate
        if (!week.VisualModels.Any(v => v.Kind == VisualModelKind.MindMap && v.Marker.Contains(MindMapMarker, StringComparison.OrdinalIgnoreCase)))
        {
            findings.Add(new(ActiveLearningGate.MindMapGate, "No Weekly Mind Map declared (Preview + Review rendered from one semantic MindMapModel)."));
        }

        // VisualLearningModelGate
        List<string> visualRejections = [];
        bool hasVisual = false;
        foreach (VisualModelDeclaration visual in week.VisualModels)
        {
            string? rejection = VisualRejection(visual);
            if (rejection is null) hasVisual = true; else visualRejections.Add(rejection);
        }
        if (!hasVisual)
        {
            findings.Add(new(ActiveLearningGate.VisualLearningModelGate, Join("No qualifying visual learning model declared.", visualRejections)));
        }

        // SimulatorInteractiveModelGate — retrieval practice can never satisfy this gate.
        List<string> simulatorRejections = [];
        bool hasSimulator = false;
        foreach (InteractionDeclaration interaction in week.Interactions)
        {
            string? rejection = InteractionRejection(interaction);
            if (rejection is not null) simulatorRejections.Add(rejection);
            else if (interaction.Family == InteractionFamily.Simulator && SimulatorComponents.Contains(interaction.Component)) hasSimulator = true;
            else simulatorRejections.Add($"'{interaction.Component}' is not a qualifying stateful simulator declaration.");
        }
        if (!hasSimulator)
        {
            findings.Add(new(ActiveLearningGate.SimulatorInteractiveModelGate, Join("No qualifying simulator / stateful interactive model declared.", simulatorRejections)));
        }

        // RetrievalResponseGate
        List<string> responseRejections = [];
        bool hasRetrieval = false;
        foreach (LearnerResponseDeclaration response in week.LearnerResponses)
        {
            string? rejection = ResponseRejection(response);
            if (rejection is not null) responseRejections.Add(rejection);
            else if (RetrievalComponents.Contains(response.Component)) hasRetrieval = true;
            else responseRejections.Add($"'{response.Component}' changes a model but is not retrieval practice.");
        }
        if (!hasRetrieval)
        {
            findings.Add(new(ActiveLearningGate.RetrievalResponseGate, Join("No qualifying retrieval-practice / committed learner response declared.", responseRejections)));
        }

        // ApplicationFeedbackGate
        bool hasApplicationFeedback = week.Interactions.Any(i =>
            InteractionRejection(i) is null && ApplicationFeedbackComponents.Contains(i.Component));
        if (!hasApplicationFeedback)
        {
            findings.Add(new(ActiveLearningGate.ApplicationFeedbackGate, "No qualifying application + feedback interaction declared."));
        }

        // FinalAssessmentGate
        if (!week.DeclaresFinalAssessment)
        {
            findings.Add(new(ActiveLearningGate.FinalAssessmentGate, "No Final Assessment declared."));
        }

        return findings;
    }

    /// <summary>Migration honesty in both directions: Compliant requires zero findings; StructuralEnrichmentRequired
    /// requires at least one (otherwise the status is stale and the week must be promoted by a conscious decision).</summary>
    public static string? CheckStatus(WeekLearningArchitecture week)
    {
        IReadOnlyList<ActiveLearningFinding> findings = Evaluate(week);
        return week.Status switch
        {
            StructuralStatus.Compliant when findings.Count > 0 =>
                $"Week {week.WeekNumber} is marked Compliant but fails: {string.Join(" | ", findings.Select(f => $"{f.Gate}: {f.Reason}"))}",
            StructuralStatus.StructuralEnrichmentRequired when findings.Count == 0 =>
                $"Week {week.WeekNumber} is marked StructuralEnrichmentRequired but every gate passes — promote it to Compliant.",
            _ => null
        };
    }

    public static string ToProjectLabel(this StructuralStatus status) => status switch
    {
        StructuralStatus.Compliant => "STRUCTURALLY COMPLIANT (ACTIVE LEARNING GATE — PASS)",
        StructuralStatus.StructuralEnrichmentRequired => "OWNER APPROVED CONTENT / STRUCTURAL ENRICHMENT REQUIRED",
        _ => throw new ArgumentOutOfRangeException(nameof(status))
    };

    private static string? VisualRejection(VisualModelDeclaration visual)
    {
        bool isMindMapMarker = visual.Marker.Contains(MindMapMarker, StringComparison.OrdinalIgnoreCase);

        if (visual.Kind == VisualModelKind.MindMap)
        {
            if (!isMindMapMarker)
            {
                return $"'{visual.Marker}' is declared as a Mind Map but is not a Mind Map marker.";
            }
            return visual.RepresentsCentralStructure
                ? null
                : $"Mind Map '{visual.Marker}' does not represent the week's central learning structure, so it cannot be the week's visual learning model on its own.";
        }

        if (isMindMapMarker)
        {
            return $"'{visual.Marker}' is a Mind Map and must be declared as {VisualModelKind.MindMap}.";
        }

        if (visual.Marker.StartsWith(ConceptGraphIdPrefix, StringComparison.Ordinal))
        {
            return ConceptGraphKinds.Contains(visual.Kind)
                ? null
                : $"A ConceptGraph cannot be declared as {visual.Kind}.";
        }

        if (!VisualConstructs.TryGetValue(visual.Marker, out IReadOnlySet<VisualModelKind>? allowed))
        {
            return $"'{visual.Marker}' is not a registered visual construct (plain lists and bare tables are not a visual learning model).";
        }

        return allowed.Contains(visual.Kind)
            ? null
            : $"'{visual.Marker}' cannot be declared as {visual.Kind}.";
    }

    private static string? InteractionRejection(InteractionDeclaration interaction)
    {
        if (!Components.TryGetValue(interaction.Component, out ComponentQualification? qualification))
        {
            return $"'{interaction.Component}' is not a classified learning-layer component.";
        }
        if (!qualification.QualifiesAsInteraction)
        {
            return $"'{interaction.Component}' does not qualify as an active-learning interaction: {qualification.Reason}";
        }
        return qualification.Interaction.Contains(interaction.Family)
            ? null
            : $"'{interaction.Component}' does not offer the {interaction.Family} family.";
    }

    private static string? ResponseRejection(LearnerResponseDeclaration response)
    {
        if (!Components.TryGetValue(response.Component, out ComponentQualification? qualification))
        {
            return $"'{response.Component}' is not a classified learning-layer component.";
        }
        if (!qualification.QualifiesAsResponse)
        {
            return $"'{response.Component}' does not qualify as a learner response: {qualification.Reason}";
        }
        return qualification.Response.Contains(response.Response)
            ? null
            : $"'{response.Component}' does not support a {response.Response} response.";
    }

    private static string Join(string headline, List<string> rejections) =>
        rejections.Count == 0 ? headline : $"{headline} Rejected: {string.Join(" ", rejections)}";
}
