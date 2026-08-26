namespace CbtLearningPlatform.Client.Curriculum;

/// <summary>Public-facing status of a week's page/content — never render the enum name directly, always ToPublicLabel().</summary>
public enum CourseWeekStatus
{
    Available,
    InPreparation,
    AcademicOverview,
    ProfessionalReviewRequired
}

/// <summary>Internal curriculum safety classification (23_CLINICAL_SAFETY_BOUNDARIES.md) — not rendered to end users directly.</summary>
public enum CurriculumSafetyLevel
{
    PublicCore,
    PublicWithAdaptation,
    AcademicContextOnly,
    ProfessionalReviewRequired,
    NotEligibleForSelfGuidedSimulator
}

public enum InteractiveFormat
{
    Simulator,
    InteractiveModel,
    Comparison,
    GuidedDemonstration,
    KnowledgeCheck,
    StaticVisualization,
    AcademicOnly
}

public static class CurriculumLabels
{
    public static string ToPublicLabel(this CourseWeekStatus status) => status switch
    {
        CourseWeekStatus.Available => "Налично",
        CourseWeekStatus.InPreparation => "В подготовка",
        CourseWeekStatus.AcademicOverview => "Академичен обзор",
        CourseWeekStatus.ProfessionalReviewRequired => "Изисква професионален преглед",
        _ => throw new ArgumentOutOfRangeException(nameof(status))
    };

    public static string ToStatusModifier(this CourseWeekStatus status) => status switch
    {
        CourseWeekStatus.Available => "available",
        CourseWeekStatus.InPreparation => "in-preparation",
        CourseWeekStatus.AcademicOverview => "academic-overview",
        CourseWeekStatus.ProfessionalReviewRequired => "review-required",
        _ => throw new ArgumentOutOfRangeException(nameof(status))
    };

    public static string ToPublicLabel(this InteractiveFormat format) => format switch
    {
        InteractiveFormat.Simulator => "Симулатор",
        InteractiveFormat.InteractiveModel => "Интерактивен модел",
        InteractiveFormat.Comparison => "Сравнение",
        InteractiveFormat.GuidedDemonstration => "Демонстрация",
        InteractiveFormat.KnowledgeCheck => "Проверка на разбирането",
        InteractiveFormat.StaticVisualization => "Визуализация",
        InteractiveFormat.AcademicOnly => "Академичен преглед",
        _ => throw new ArgumentOutOfRangeException(nameof(format))
    };

    /// <summary>Derives the public Status from the internal SafetyLevel and whether a real route exists —
    /// single source of truth so the two never drift out of sync when a new week is added.</summary>
    public static CourseWeekStatus DeriveStatus(CurriculumSafetyLevel safetyLevel, string? route) => safetyLevel switch
    {
        CurriculumSafetyLevel.AcademicContextOnly => CourseWeekStatus.AcademicOverview,
        CurriculumSafetyLevel.ProfessionalReviewRequired or CurriculumSafetyLevel.NotEligibleForSelfGuidedSimulator
            => CourseWeekStatus.ProfessionalReviewRequired,
        _ => route is not null ? CourseWeekStatus.Available : CourseWeekStatus.InPreparation
    };
}
