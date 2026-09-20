namespace CbtLearningPlatform.Client.Curriculum;

// ACTIVE LEARNING TOOLKIT — safety modes (root AGENTS.md "Active Learning Standard").
//
// Safety changes the FORM of an activity — its framing, its allowed interaction shapes — and NEVER reduces it to passive
// reading. Every toolkit engine, in every mode, is closed-choice only: no free text, no storage, no server call, no
// diagnosis, no risk prediction, no exposure planning, no self-treatment simulation. That is a property of the engines,
// not a mode setting, so it cannot be switched off by a week.

/// <summary>How an activity must be framed and bounded. Derived from the week's <see cref="CurriculumSafetyLevel"/> via
/// <see cref="ActiveLearningSafety.ModeFor"/> so a week never hand-picks its own leniency.</summary>
public enum ActiveLearningSafetyMode
{
    /// <summary>PublicCore / PublicWithAdaptation: ordinary learning exercise.</summary>
    NormalLearning,

    /// <summary>AcademicContextOnly: fixed third-person examples, a mandatory framing notice; never a self-assessment tool.</summary>
    AcademicThirdPerson,

    /// <summary>ProfessionalReviewRequired: fixed examples from professional practice, a mandatory framing notice; never an
    /// instruction for self-application.</summary>
    ProfessionalContext,

    /// <summary>NotEligibleForSelfGuidedSimulator: strictest — fixed examples, a mandatory notice, and no branching
    /// consequence simulation (ScenarioSimulator Level C is withheld). Classify/match, ordering and committed predict→reveal
    /// on fixed vignettes remain fully available.</summary>
    NoSelfGuidedSimulation
}

/// <param name="DefaultNotice">Framing shown with the activity. Mandatory (cannot be blanked) in every non-Normal mode.</param>
/// <param name="AllowsBranchingScenarios">Whether a consequence-branching scenario (ScenarioSimulator Level C) may be offered.</param>
public sealed record SafetyModeProfile(ActiveLearningSafetyMode Mode, string DefaultNotice, bool AllowsBranchingScenarios);

public static class ActiveLearningSafety
{
    public static SafetyModeProfile Profile(ActiveLearningSafetyMode mode) => mode switch
    {
        ActiveLearningSafetyMode.NormalLearning =>
            new(mode, string.Empty, AllowsBranchingScenarios: true),
        ActiveLearningSafetyMode.AcademicThirdPerson =>
            new(mode, "Упражнението използва фиксирани примери от трето лице. Учебно е — не е инструмент за самооценка.", AllowsBranchingScenarios: true),
        ActiveLearningSafetyMode.ProfessionalContext =>
            new(mode, "Упражнението използва фиксирани примери от професионалната практика. Учебно е — не е инструкция за самостоятелно прилагане.", AllowsBranchingScenarios: true),
        ActiveLearningSafetyMode.NoSelfGuidedSimulation =>
            new(mode, "Упражнението използва фиксирани примери. Учебно е — не е симулация за самостоятелна практика.", AllowsBranchingScenarios: false),
        _ => throw new ArgumentOutOfRangeException(nameof(mode))
    };

    /// <summary>The single mapping from a week's curriculum safety level to its activity safety mode.</summary>
    public static ActiveLearningSafetyMode ModeFor(CurriculumSafetyLevel level) => level switch
    {
        CurriculumSafetyLevel.PublicCore or CurriculumSafetyLevel.PublicWithAdaptation => ActiveLearningSafetyMode.NormalLearning,
        CurriculumSafetyLevel.AcademicContextOnly => ActiveLearningSafetyMode.AcademicThirdPerson,
        CurriculumSafetyLevel.ProfessionalReviewRequired => ActiveLearningSafetyMode.ProfessionalContext,
        CurriculumSafetyLevel.NotEligibleForSelfGuidedSimulator => ActiveLearningSafetyMode.NoSelfGuidedSimulation,
        _ => throw new ArgumentOutOfRangeException(nameof(level))
    };

    /// <summary>The notice to render. A week may supply its own approved wording, but in any non-Normal mode a blank override
    /// falls back to the mode's default: safety framing can never be removed by leaving a parameter empty.</summary>
    public static string ResolveNotice(ActiveLearningSafetyMode mode, string? overrideNotice)
    {
        string trimmed = overrideNotice?.Trim() ?? string.Empty;
        return mode == ActiveLearningSafetyMode.NormalLearning || trimmed.Length > 0
            ? trimmed
            : Profile(mode).DefaultNotice;
    }
}
