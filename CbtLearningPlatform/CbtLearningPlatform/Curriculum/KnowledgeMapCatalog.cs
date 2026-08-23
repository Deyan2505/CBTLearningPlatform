namespace CbtLearningPlatform.Curriculum;

/// <summary>Global CBT Knowledge Map — "как са свързани знанията по КПТ?", COGNITIVE_LEARNING_ARCHITECTURE_v1.md
/// Phase 5. Every node/relation below is grounded in real, already-routed page content, verified by
/// reading the actual section anchors before writing this file (§9/§10 of the Phase 5 authorization:
/// "не изграждай карта от общите си знания по КПТ" / "провери действителния CourseCatalog, не
/// приемай списък от паметта"). No global CBT taxonomy is invented — this is a small, honest v1
/// covering only Weeks 3, 6, 8, 10, 12 (the routed weeks that actually name these concepts), not a
/// speculative map of the full future curriculum.
///
/// Source grounding per node (re-verified directly in each page's own markup, not from memory):
///  - Ситуация / Автоматична мисъл / Поведение — Sedmica3.razor #situacia-znachenie (a distinct
///    3/4-step chain) and #tri-niva (the automatic-thought/intermediate/core hierarchy diagram).
///  - Емоция / Телесна реакция — first appear as SEPARATE labeled nodes only in Sedmica8.razor
///    #karta-na-temata (Week 3's own chain merges them into one "Емоционална и телесна реакция"
///    node, so Week 8 — not Week 3 — is where each becomes its own concept).
///  - Когнитивен модел (the umbrella S-T-E-Body-Behavior chain as a named concept) — Sedmica3.razor
///    #situacia-znachenie, revisited by name in Sedmica6.razor's terminology card and Sedmica8.razor.
///  - Междинно вярване / Основно вярване (Схема) — Sedmica3.razor #tri-niva; Основно вярване
///    revisited in Sedmica12.razor #osnovno-vyarvane.
///  - Терапевтичен алианс — Sedmica6.razor #poniatiya (terminology card).
///  - Сократически въпрос — Sedmica10.razor #izsledvane.
///
/// This is deliberately a small, honest graph (10 nodes, 12 relations) rather than a large
/// speculative one (§11: "по-добре малка вярна карта, отколкото голяма измислена"). No Upcoming
/// nodes in this v1 — every concept here is already taught by a routed week; nothing forward-leaks.</summary>
public static class KnowledgeMapCatalog
{
    public static ConceptMapModel Build() => new(
        "Карта на знанието по КПТ",
        "Как основните понятия от вече изучените седмици се свързват помежду си — не всяка седмица, само реално потвърдените връзки.",
        [
            new("situation", "Ситуация", null, 3, [6, 8], "/kurs/sedmica-3#situacia-znachenie"),
            new("automatic-thought", "Автоматична мисъл", null, 3, [6, 8], "/kurs/sedmica-3#tri-niva"),
            new("emotion", "Емоция", null, 8, [], "/kurs/sedmica-8#karta-na-temata"),
            new("body-reaction", "Телесна реакция", null, 8, [], "/kurs/sedmica-8#karta-na-temata"),
            new("behavior", "Поведение", null, 3, [6, 8], "/kurs/sedmica-3#situacia-znachenie"),
            new("cognitive-model", "Когнитивен модел", null, 3, [6, 8], "/kurs/sedmica-3#situacia-znachenie", IsCrossReference: true),
            new("intermediate-belief", "Междинно вярване", null, 3, [], "/kurs/sedmica-3#tri-niva", IsCrossReference: true),
            new("core-belief", "Основно вярване / Схема", null, 3, [12], "/kurs/sedmica-3#tri-niva", IsCrossReference: true),
            new("therapeutic-alliance", "Терапевтичен алианс", null, 6, [], "/kurs/sedmica-6#poniatiya", IsCrossReference: true),
            new("socratic-question", "Сократически въпрос", null, 10, [], "/kurs/sedmica-10#izsledvane", IsCrossReference: true)
        ],
        [
            new("situation", "automatic-thought", RelationType.LeadsTo, "поражда"),
            new("automatic-thought", "emotion", RelationType.LeadsTo, "поражда"),
            new("emotion", "body-reaction", RelationType.Supports, "съпътства се от"),
            new("body-reaction", "behavior", RelationType.LeadsTo, "оформя"),
            new("situation", "cognitive-model", RelationType.IsPartOf, "част е от"),
            new("automatic-thought", "cognitive-model", RelationType.IsPartOf, "част е от"),
            new("emotion", "cognitive-model", RelationType.IsPartOf, "част е от"),
            new("body-reaction", "cognitive-model", RelationType.IsPartOf, "част е от"),
            new("behavior", "cognitive-model", RelationType.IsPartOf, "част е от"),
            new("automatic-thought", "intermediate-belief", RelationType.Precedes, "задълбочено в"),
            new("intermediate-belief", "core-belief", RelationType.Precedes, "задълбочено в"),
            new("automatic-thought", "socratic-question", RelationType.Supports, "изследва се чрез")
        ]);

    /// <summary>Presentation-only spatial layout for the Network renderer — NOT a CBT taxonomy, and not a
    /// field on ConceptNode. Column/Row are small grid coordinates (not pixels) the renderer turns into exact
    /// SSR-computed positions, so relation lines can be drawn as real curves without any client-side DOM
    /// measurement. Three cluster groups, derived from which of the 12 relations above already connect these
    /// concepts to each other:
    ///  - "Когнитивна верига" spans two columns: column 0 is the five-step sequential chain (situation →
    ///    automatic-thought → emotion → body-reaction → behavior, rows 0-4); column 1 holds cognitive-model
    ///    alone (row 2, vertically centered) as the single node all five IsPartOf relations converge into — a
    ///    hub, not a sixth chain step, so it sits beside the chain rather than stacked below it.
    ///  - "Вярвания" (column 2): intermediate-belief (row 0) → core-belief (row 1), the only two concepts
    ///    connected to each other by the Precedes relations.
    ///  - "Терапевтичен процес" (column 3): therapeutic-alliance (row 0, no relations at all) and
    ///    socratic-question (row 1, exactly one relation, from automatic-thought) — two outliers with no
    ///    relation to each other, grouped here because neither belongs to either dense sub-graph.
    /// automatic-thought (column 0, row 1) is the only node whose relations reach outside its own column pair —
    /// to intermediate-belief and to socratic-question — which is exactly why the renderer draws those two as
    /// long, arcing cross-cluster curves rather than short in-column ones.</summary>
    public static IReadOnlyDictionary<string, ConceptNetworkPosition> NetworkLayout { get; } = new Dictionary<string, ConceptNetworkPosition>
    {
        ["situation"] = new("Когнитивна верига", 0, 0),
        ["automatic-thought"] = new("Когнитивна верига", 0, 1),
        ["emotion"] = new("Когнитивна верига", 0, 2),
        ["body-reaction"] = new("Когнитивна верига", 0, 3),
        ["behavior"] = new("Когнитивна верига", 0, 4),
        ["cognitive-model"] = new("Когнитивна верига", 1, 2),
        ["intermediate-belief"] = new("Вярвания", 2, 0),
        ["core-belief"] = new("Вярвания", 2, 1),
        ["therapeutic-alliance"] = new("Терапевтичен процес", 3, 0),
        ["socratic-question"] = new("Терапевтичен процес", 3, 1)
    };
}
