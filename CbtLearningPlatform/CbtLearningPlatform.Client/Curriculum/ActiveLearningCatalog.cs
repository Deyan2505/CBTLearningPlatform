namespace CbtLearningPlatform.Client.Curriculum;

/// <summary>Per-week ACTIVE LEARNING STANDARD declarations — presence and type only, never CBT content.
///
/// HONESTY RULE: each entry records what the week's page ACTUALLY contains today, judged by the rules in
/// <see cref="ActiveLearningStandard"/>. Nothing here may be declared to make a week look compliant; the gate
/// (ActiveLearningGateTests) verifies every marker against the page markup and fails if a week's
/// <see cref="StructuralStatus"/> disagrees with its own declarations, in either direction.
///
/// The stricter six-gate owner rule is recorded here honestly. Weeks 5, 7 and 9 include both their retained retrieval
/// activities and the shared stateful simulator; they await owner review. Weeks 2, 6, 8 and 10 also satisfy all six gates.
/// Week 3 now passes all six: the schema-filter simulator, a committed level classification (retrieval) and the Sally belief application.
/// Week 4 passes all six with a fixed third-person case examination, the session sequence and a separate ordering retrieval.
/// Week 11 passes all six with a generic-patient therapist-judgment simulator and a separate technique-matching retrieval.
/// Week 12 passes all six with an unnamed-person category-ambiguity simulator and a separate category-matching retrieval.
/// Week 13 passes all six with a Sally-attribution-order interactive model (NoSelfGuidedSimulation, never labelled
/// "Симулатор") and a separate skill-vs-belief classification retrieval.
/// Week 14 passes all six with a four-way homework-failure-diagnosis interactive model (ProfessionalContext) and a
/// separate Figure 18.1 recovery-shape ordering retrieval.
/// The remaining week (15) remains
/// <see cref="StructuralStatus.StructuralEnrichmentRequired"/> (OWNER APPROVED CONTENT / STRUCTURAL ENRICHMENT REQUIRED):
/// content approval and locked prose remain valid; only the learning architecture is reopened, one controlled batch at a time. When a week's remediation lands, add the new declarations and
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
        // --- Re-evaluated under the six-gate standard ---
        Compliant(3,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week3-mindmap-preview"), RepresentsCentralStructure: false),
                new(VisualModelKind.Hierarchy, Graph("week3-sali-hierarchy")),
                new(VisualModelKind.Process, Flow),
                new(VisualModelKind.Cycle, Loop)
            ],
            interactions:
            [
                new(InteractionFamily.Simulator, "SchemaFilterDemonstration"),
                new(InteractionFamily.Simulator, "StatefulModelSimulator"),
                new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck")
            ],
            responses: [new(LearnerResponseKind.Classify, "ClassifyMatchCheck")]),

        Compliant(6,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week6-mindmap-preview"), RepresentsCentralStructure: false),
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
            visuals: [new(VisualModelKind.MindMap, Graph("week8-mindmap-preview"), RepresentsCentralStructure: false), new(VisualModelKind.Process, Flow)],
            interactions:
            [
                new(InteractionFamily.Simulator, "CbtChainSimulator"),
                new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck")
            ],
            responses: [new(LearnerResponseKind.Classify, "ClassifyMatchCheck")]),

        // --- Earlier batches, honestly re-evaluated under the stricter simulator gate ---
        // Week 1: the evidence simulator is distinct from the timeline and the committed ordering retrieval in section 08.
        Compliant(1,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week1-mindmap-preview"), RepresentsCentralStructure: true),
                new(VisualModelKind.Timeline, "<HistoricalTimeline"),
                new(VisualModelKind.Comparison, ComparisonMatrix)
            ],
            interactions:
            [
                new(InteractionFamily.Simulator, "StatefulModelSimulator"),
                new(InteractionFamily.OrderingBuilder, "OrderingBuilder")
            ],
            responses: [new(LearnerResponseKind.Order, "OrderingBuilder")]),

        // Week 2: the ABC belief-mediation simulator is separate from the committed Beck/Ellis attribution retrieval.
        Compliant(2,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week2-mindmap-preview"), RepresentsCentralStructure: true),
                new(VisualModelKind.Process, Flow),
                new(VisualModelKind.Comparison, ComparisonMatrix),
                new(VisualModelKind.Comparison, CategoryCompare)
            ],
            interactions:
            [
                new(InteractionFamily.Simulator, "StatefulModelSimulator"),
                new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck")
            ],
            responses: [new(LearnerResponseKind.Classify, "ClassifyMatchCheck")]),

        // Week 10: the 10.4 reveal cards are now a committed classification (before the assessment). Section 10.11 also hosts a
        // committed OrderingBuilder retrieval practice; it sits AFTER the assessment, so it is intentionally not declared here —
        // only elements that precede the Final Assessment count toward the gate.
        Compliant(10,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week10-mindmap-preview"), RepresentsCentralStructure: false),
                new(VisualModelKind.Process, Flow),
                new(VisualModelKind.Sequence, Sequence),
                new(VisualModelKind.Comparison, CategoryCompare)
            ],
            interactions:
            [
                new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck"),
                new(InteractionFamily.Simulator, "CaseExaminationSimulator")
            ],
            responses:
            [
                new(LearnerResponseKind.Classify, "ClassifyMatchCheck"),
                new(LearnerResponseKind.ManipulateModel, "CaseExaminationSimulator"),
                new(LearnerResponseKind.Predict, "CaseExaminationSimulator")
            ]),

        // --- Phase 2, Batch 2: gate PASS, AWAITING OWNER VISUAL REVIEW (not locked) ---
        // Week 5: the two-stage collaboration comparison (category-compare) and the Principle 7 tapering sequence stay the visual
        // models; the learner classifies statements against the comparison (ClassifyMatchCheck, 5.5, before the quiz).
        Compliant(5,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week5-mindmap-preview"), RepresentsCentralStructure: false),
                new(VisualModelKind.Comparison, CategoryCompare),
                new(VisualModelKind.Sequence, Sequence)
            ],
            interactions:
            [
                new(InteractionFamily.Simulator, "StatefulModelSimulator"),
                new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck")
            ],
            responses: [new(LearnerResponseKind.Classify, "ClassifyMatchCheck")]),

        // Week 7: the vicious-cycle loop and the Sali behavioural-experiment map stay the visual models; 7.6's two
        // predict-versus-actual scenarios are committed predictions (PredictReveal). 7.11 also hosts a committed OrderingBuilder
        // over the cycle; it follows the Final Assessment, so it is review practice and is intentionally not declared here.
        Compliant(7,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week7-mindmap-preview"), RepresentsCentralStructure: false),
                new(VisualModelKind.Cycle, Loop),
                new(VisualModelKind.ConceptNetwork, Graph("week7-sali-map")),
                new(VisualModelKind.Process, Flow)
            ],
            interactions:
            [
                new(InteractionFamily.Simulator, "StatefulModelSimulator"),
                new(InteractionFamily.PredictCommit, "PredictReveal")
            ],
            responses: [new(LearnerResponseKind.Predict, "PredictReveal")]),

        // Week 9: the six-column thought-record structure and the six-category evaluation sequence stay the visual models; the
        // learner matches six approved distortion examples to their names (ClassifyMatchCheck, 9.8, before the quiz). The fixed,
        // non-fillable Thought Record demonstration is unchanged.
        Compliant(9,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week9-mindmap-preview"), RepresentsCentralStructure: false),
                new(VisualModelKind.Process, Graph("week9-thought-record-structure")),
                new(VisualModelKind.Sequence, Sequence)
            ],
            interactions:
            [
                new(InteractionFamily.Simulator, "StatefulModelSimulator"),
                new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck")
            ],
            responses: [new(LearnerResponseKind.Classify, "ClassifyMatchCheck")]),

        // --- OWNER APPROVED CONTENT / STRUCTURAL ENRICHMENT REQUIRED ---

        // Week 4: lists only + a Mind Map that does not carry the week's central structure.
        // Week 4 (AcademicContextOnly -> fixed third-person case): the six-step session sequence (§04) is the visual; the learner
        // applies the five conceptualization questions to Sally's §08 assessment synthesis (CaseExaminationSimulator), and
        // separately rebuilds the session order (OrderingBuilder, §09, before the quiz).
        Compliant(4,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week4-mindmap-preview"), RepresentsCentralStructure: false),
                new(VisualModelKind.Sequence, Sequence)
            ],
            interactions:
            [
                new(InteractionFamily.Simulator, "CaseExaminationSimulator"),
                new(InteractionFamily.OrderingBuilder, "OrderingBuilder")
            ],
            responses:
            [
                new(LearnerResponseKind.Order, "OrderingBuilder"),
                new(LearnerResponseKind.ManipulateModel, "CaseExaminationSimulator"),
                new(LearnerResponseKind.Predict, "CaseExaminationSimulator")
            ]),

        // Week 11 (ProfessionalReviewRequired -> ProfessionalContext): the existing 3-step identify/decide/modify
        // sequence and the two comparison tables stay the visuals; the learner works the §06 therapist-judgment model
        // (generic unnamed patient, never Sally, never Downward Arrow) and separately matches the seven §07
        // modification-technique names to their definitions before the Final Assessment.
        Compliant(11,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week11-mindmap-preview"), RepresentsCentralStructure: false),
                new(VisualModelKind.Process, Sequence),
                new(VisualModelKind.Comparison, ComparisonMatrix)
            ],
            interactions:
            [
                new(InteractionFamily.Simulator, "StatefulModelSimulator"),
                new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck")
            ],
            responses: [new(LearnerResponseKind.Classify, "ClassifyMatchCheck")]),

        // Week 12 (AcademicContextOnly -> AcademicThirdPerson): the earlier audit's "zero interaction, no Mind Map"
        // decisions are superseded by ADR-011 and the Mind Map mandate (owner decision). The two existing comparisons
        // stay the visuals; the learner works the §03 category-ambiguity model (unnamed person, two source-supported
        // branches only — no Безполезност branch) and separately matches the three category names to their §03
        // definitions before the Final Assessment.
        Compliant(12,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week12-mindmap-preview"), RepresentsCentralStructure: false),
                new(VisualModelKind.Comparison, CategoryCompare),
                new(VisualModelKind.Comparison, ComparisonMatrix)
            ],
            interactions:
            [
                new(InteractionFamily.Simulator, "StatefulModelSimulator"),
                new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck")
            ],
            responses: [new(LearnerResponseKind.Classify, "ClassifyMatchCheck")]),

        // Week 13 (NotEligibleForSelfGuidedSimulator -> NoSelfGuidedSimulation): the earlier audit's "zero simulator, at
        // most one interaction" call is superseded by ADR-011 (owner decision). The Mind Map, ladder sequence and
        // comparisons stay the visuals; the learner works the §08 attribution-order model on Sally's already-approved
        // responsibility pie (two paths that differ only in evaluation order, never in clinical outcome — the model is
        // never labelled "Симулатор" to the learner) and separately classifies §07's own skill-vs-belief statements
        // before the Final Assessment.
        Compliant(13,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week13-mindmap-preview"), RepresentsCentralStructure: false),
                new(VisualModelKind.Sequence, Sequence),
                new(VisualModelKind.Comparison, ComparisonMatrix)
            ],
            interactions:
            [
                new(InteractionFamily.Simulator, "StatefulModelSimulator"),
                new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck")
            ],
            responses: [new(LearnerResponseKind.Classify, "ClassifyMatchCheck")]),

        // Week 14 (ProfessionalReviewRequired -> ProfessionalContext): ADR-011 supersedes the audit's original
        // "no simulator" call (same owner decision already applied to Week 13). The Mind Map, taper sequence and
        // comparisons stay the visuals; the learner works the §04 four-way homework-failure-diagnosis model
        // (verbatim table cells, four distinct clinician-perspective paths, no patient answer, no percentage) and
        // separately orders Figure 18.1's five recovery stages before the Final Assessment.
        Compliant(14,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week14-mindmap-preview"), RepresentsCentralStructure: false),
                new(VisualModelKind.Sequence, Sequence),
                new(VisualModelKind.Comparison, ComparisonMatrix)
            ],
            interactions:
            [
                new(InteractionFamily.Simulator, "StatefulModelSimulator"),
                new(InteractionFamily.OrderingBuilder, "OrderingBuilder")
            ],
            responses: [new(LearnerResponseKind.Order, "OrderingBuilder")]),

        Migration(15,
            visuals:
            [
                new(VisualModelKind.MindMap, Graph("week15-mindmap-preview"), RepresentsCentralStructure: false),
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
    private static WeekLearningArchitecture Migration(
        int week,
        IReadOnlyList<VisualModelDeclaration> visuals,
        IReadOnlyList<InteractionDeclaration>? interactions = null,
        IReadOnlyList<LearnerResponseDeclaration>? responses = null) =>
        new(week, StructuralStatus.StructuralEnrichmentRequired, visuals, interactions ?? [], responses ?? [], DeclaresFinalAssessment: true);
}
