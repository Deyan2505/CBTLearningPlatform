namespace CbtLearningPlatform.Client.Curriculum;

/// <summary>Per-week ACTIVE LEARNING STANDARD declarations — presence and type only, never CBT content.
///
/// HONESTY RULE: each entry records what the week's page ACTUALLY contains today, judged by the rules in
/// <see cref="ActiveLearningStandard"/>. Nothing here may be declared to make a week look compliant; the gate
/// (ActiveLearningGateTests) verifies every marker against the page markup and fails if a week's
/// <see cref="StructuralStatus"/> disagrees with its own declarations, in either direction.
///
/// Weeks 3, 6 and 8 are the owner-confirmed compliant references. Weeks 1, 2 and 10 (Phase 2, Batch 1) pass the gate
/// but are NOT owner-approved or locked: they await the owner's visual review of the interactions in production. The
/// other nine are <see cref="StructuralStatus.StructuralEnrichmentRequired"/> (OWNER APPROVED CONTENT / STRUCTURAL
/// ENRICHMENT REQUIRED): content approval and locked prose remain valid; only the learning architecture is
/// reopened, one controlled batch at a time. When a week's remediation lands, add the new declarations and
/// promote its status to Compliant in the same change — a stale StructuralEnrichmentRequired also fails the gate.
///
/// Existing select-to-read explorers (ResearchTurnStepper, CognitiveHierarchyExplorer, SocraticDialogueExplorer),
/// reveal-only checks (CategorizationCheck, WhatIfBox, ProgressiveExplanation) and Mind Maps are intentionally
/// NOT declared as interactions/responses: they do not qualify (see <see cref="ActiveLearningStandard.Components"/>).</summary>
public static class ActiveLearningCatalog
{
    private const string Loop = "cascade-loop";
    private const string Flow = "concept-map__flow";
    private const string Sequence = "guided-practice-sequence";
    private const string CategoryCompare = "category-compare";
    private const string ComparisonMatrix = "comparison-matrix";

    public static IReadOnlyList<WeekLearningArchitecture> Weeks { get; } =
    [
        // --- Structurally compliant references ---
        Compliant(3,
            visuals:
            [
                new(VisualModelKind.Hierarchy, Graph("week3-sali-hierarchy")),
                new(VisualModelKind.Process, Flow),
                new(VisualModelKind.Cycle, Loop)
            ],
            interactions: [new(InteractionFamily.InteractiveModel, "SchemaFilterDemonstration")],
            responses: [new(LearnerResponseKind.ManipulateModel, "SchemaFilterDemonstration")]),

        Compliant(6,
            visuals:
            [
                new(VisualModelKind.DecisionModel, "decision-branch"),
                new(VisualModelKind.ConceptNetwork, Graph("week6-concept-map")),
                new(VisualModelKind.CaseModel, Graph("irina-case-map")),
                new(VisualModelKind.Sequence, Sequence)
            ],
            interactions: [new(InteractionFamily.Simulator, "ScenarioSimulator")],
            responses:
            [
                new(LearnerResponseKind.Classify, "ScenarioSimulator"),
                new(LearnerResponseKind.Order, "ScenarioSimulator"),
                new(LearnerResponseKind.Decide, "ScenarioSimulator")
            ]),

        Compliant(8,
            visuals: [new(VisualModelKind.Process, Flow)],
            interactions: [new(InteractionFamily.Simulator, "CbtChainSimulator")],
            responses: [new(LearnerResponseKind.ManipulateModel, "CbtChainSimulator")]),

        // --- Phase 2, Batch 1: gate PASS, AWAITING OWNER VISUAL REVIEW (not locked) ---
        // Week 1: the timeline stays the visual model; the learner rebuilds it (OrderingBuilder, section 08, before the quiz).
        Compliant(1,
            visuals: [new(VisualModelKind.Timeline, "<HistoricalTimeline"), new(VisualModelKind.Comparison, ComparisonMatrix)],
            interactions: [new(InteractionFamily.OrderingBuilder, "OrderingBuilder")],
            responses: [new(LearnerResponseKind.Order, "OrderingBuilder")]),

        // Week 2: the two schools as side-by-side process chains + a committed Beck/Ellis attribution over the comparison table.
        Compliant(2,
            visuals:
            [
                new(VisualModelKind.Process, Flow),
                new(VisualModelKind.Comparison, ComparisonMatrix),
                new(VisualModelKind.Comparison, CategoryCompare)
            ],
            interactions: [new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck")],
            responses: [new(LearnerResponseKind.Classify, "ClassifyMatchCheck")]),

        // Week 10: the 10.4 reveal cards are now a committed classification (before the assessment). Section 10.11 also hosts a
        // committed OrderingBuilder retrieval practice; it sits AFTER the assessment, so it is intentionally not declared here —
        // only elements that precede the Final Assessment count toward the gate.
        Compliant(10,
            visuals:
            [
                new(VisualModelKind.Process, Flow),
                new(VisualModelKind.Sequence, Sequence),
                new(VisualModelKind.Comparison, CategoryCompare)
            ],
            interactions: [new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck")],
            responses: [new(LearnerResponseKind.Classify, "ClassifyMatchCheck")]),

        // --- OWNER APPROVED CONTENT / STRUCTURAL ENRICHMENT REQUIRED ---

        // Week 4: lists only + a Mind Map that does not carry the week's central structure.
        Migration(4,
            visuals: [new(VisualModelKind.MindMap, Graph("week4-mindmap-preview"), RepresentsCentralStructure: false)]),

        Migration(5,
            visuals: [new(VisualModelKind.Comparison, CategoryCompare), new(VisualModelKind.Sequence, Sequence)]),

        Migration(7,
            visuals:
            [
                new(VisualModelKind.Cycle, Loop),
                new(VisualModelKind.ConceptNetwork, Graph("week7-sali-map")),
                new(VisualModelKind.Process, Flow)
            ]),

        Migration(9,
            visuals: [new(VisualModelKind.Process, Graph("week9-thought-record-structure")), new(VisualModelKind.Sequence, Sequence)]),

        Migration(11,
            visuals: [new(VisualModelKind.Process, Sequence), new(VisualModelKind.Comparison, ComparisonMatrix)]),

        Migration(12,
            visuals: [new(VisualModelKind.Comparison, CategoryCompare), new(VisualModelKind.Comparison, ComparisonMatrix)]),

        Migration(13,
            visuals: [new(VisualModelKind.Sequence, Sequence), new(VisualModelKind.Comparison, ComparisonMatrix)]),

        Migration(14,
            visuals: [new(VisualModelKind.Sequence, Sequence), new(VisualModelKind.Comparison, ComparisonMatrix)]),

        Migration(15,
            visuals:
            [
                new(VisualModelKind.Process, Sequence),
                new(VisualModelKind.Comparison, CategoryCompare),
                new(VisualModelKind.Comparison, ComparisonMatrix)
            ])
    ];

    public static WeekLearningArchitecture For(int weekNumber) => Weeks.Single(w => w.WeekNumber == weekNumber);

    private static string Graph(string componentId) => $"{ActiveLearningStandard.ConceptGraphIdPrefix}{componentId}\"";

    private static WeekLearningArchitecture Compliant(
        int week,
        IReadOnlyList<VisualModelDeclaration> visuals,
        IReadOnlyList<InteractionDeclaration> interactions,
        IReadOnlyList<LearnerResponseDeclaration> responses) =>
        new(week, StructuralStatus.Compliant, visuals, interactions, responses, DeclaresFinalAssessment: true);

    // Interaction and response lists are empty on purpose: no qualifying element exists yet. They are filled
    // in by the week's controlled remediation, together with the promotion to Compliant.
    private static WeekLearningArchitecture Migration(int week, IReadOnlyList<VisualModelDeclaration> visuals) =>
        new(week, StructuralStatus.StructuralEnrichmentRequired, visuals, [], [], DeclaresFinalAssessment: true);
}
