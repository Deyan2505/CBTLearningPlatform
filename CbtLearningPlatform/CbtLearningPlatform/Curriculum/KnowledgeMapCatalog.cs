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

    /// <summary>Presentation-only spatial grouping for the Network layout — NOT a CBT taxonomy, and not a
    /// field on ConceptNode. Derived purely from which of the 12 relations above already link these concepts
    /// densely to each other: situation/automatic-thought/emotion/body-reaction/behavior/cognitive-model are
    /// all connected to one another (the LeadsTo chain plus the 5 IsPartOf edges into cognitive-model);
    /// intermediate-belief/core-belief are connected only to each other and to automatic-thought;
    /// therapeutic-alliance has no relations at all and socratic-question has exactly one (from
    /// automatic-thought) — those two sit apart from the dense chain. automatic-thought itself is the single
    /// bridge node with edges reaching all three groups, which is exactly why it renders with the most visible
    /// outgoing chips rather than needing a separate "hub" designation.</summary>
    public static IReadOnlyDictionary<string, string> Clusters { get; } = new Dictionary<string, string>
    {
        ["situation"] = "Когнитивна верига",
        ["automatic-thought"] = "Когнитивна верига",
        ["emotion"] = "Когнитивна верига",
        ["body-reaction"] = "Когнитивна верига",
        ["behavior"] = "Когнитивна верига",
        ["cognitive-model"] = "Когнитивна верига",
        ["intermediate-belief"] = "Вярвания",
        ["core-belief"] = "Вярвания",
        ["therapeutic-alliance"] = "Терапевтичен процес",
        ["socratic-question"] = "Терапевтичен процес"
    };
}
