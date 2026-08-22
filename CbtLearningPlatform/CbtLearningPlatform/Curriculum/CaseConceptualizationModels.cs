namespace CbtLearningPlatform.Curriculum;

public enum CaseLevel { Basic, Intermediate, Challenging }

/// <summary>A fictional, self-invented Case Lab character — never a real patient, never a case reproduced
/// verbatim from the source book. FirstAppearedWeek is metadata only, not a claim about future appearances.</summary>
public sealed record CaseCharacter(
    string Id,
    string Name,
    CaseLevel Level,
    int FirstAppearedWeek);

/// <summary>One week's worth of case-conceptualization detail for a character. Every field is optional by
/// design (COGNITIVE_LEARNING_ARCHITECTURE_v1.md §7/§23) — progressive disclosure means an observation shows
/// only what that week's own content actually establishes, never a value invented to "complete" the model.</summary>
public sealed record CaseObservation(
    string CaseId,
    int WeekNumber,
    string? Situation,
    string? Thought,
    string? Emotion,
    string? Body,
    string? Behavior,
    string? Distortion,
    string? IntermediateBelief,
    string? CoreBelief,
    string? InterventionLink);

public sealed record CaseConceptualizationModel(
    CaseCharacter Character,
    IReadOnlyList<CaseObservation> Observations);

/// <summary>Ирина — approved pilot longitudinal case (Week 6 v2 blueprint §4, owner-approved). Only the Week 6
/// observation exists here; no future belief/distortion/treatment history is invented ahead of a week that
/// actually teaches it.</summary>
public static class CaseCatalog
{
    public static CaseCharacter Irina { get; } = new(
        Id: "irina",
        Name: "Ирина",
        Level: CaseLevel.Intermediate,
        FirstAppearedWeek: 6);

    public static IReadOnlyList<CaseObservation> IrinaObservations { get; } =
    [
        new(
            CaseId: Irina.Id,
            WeekNumber: 6,
            Situation: "Първата терапевтична сесия, моментът на идентифициране на проблеми и цели.",
            Thought: null,
            Emotion: null,
            Body: null,
            Behavior: "\"Искам просто да се справям по-добре.\" — неясна, неизмерима формулировка на цел.",
            Distortion: null,
            IntermediateBelief: null,
            CoreBelief: null,
            InterventionLink: "Терапевтът пита: \"Ако вече се справяше по-добре, какво точно щеше да правиш различно?\" — превръща неясната цел в конкретна, поведенчески измерима формулировка (U25).")
    ];
}
