namespace CbtLearningPlatform.Client.Curriculum;

/// <summary>Course Completion Certificate eligibility rule — pure and session-scoped, no persistence.
/// <paramref name="currentExamScore"/> is deliberately nullable and deliberately never read from
/// storage: the course final exam result lives only in the calling page's own component memory
/// (never localStorage, unlike <see cref="CourseProgressStore"/>'s week-completion set), so a reload
/// always yields null here and eligibility is lost — that reset is intentional, not a bug.</summary>
public static class CertificateEligibility
{
    public const int MinimumExamScore = 75;

    public static bool IsEligible(int completedWeeks, int routedWeeks, int? currentExamScore) =>
        completedWeeks == routedWeeks && currentExamScore is >= MinimumExamScore;
}
