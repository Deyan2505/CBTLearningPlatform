namespace CbtLearningPlatform.Client.Curriculum;

/// <summary>Single-answer question for the shared Weekly Final Assessment Standard. True/false is just a
/// 2-option instance of the same shape (see <see cref="TrueFalse"/>) — no separate question-kind concept,
/// since scoring and rendering are identical either way.</summary>
public sealed record AssessmentQuestion(
    string Id,
    string Text,
    IReadOnlyList<string> Options,
    int CorrectOptionIndex,
    string Explanation)
{
    public static AssessmentQuestion Choice(string id, string text, IReadOnlyList<string> options, int correctOptionIndex, string explanation) =>
        new(id, text, options, correctOptionIndex, explanation);

    public static AssessmentQuestion TrueFalse(string id, string text, bool correctIsTrue, string explanation) =>
        new(id, text, ["Вярно", "Невярно"], correctIsTrue ? 0 : 1, explanation);
}

/// <summary><paramref name="LowScoreInterpretation"/> overrides the shared engine's default
/// below-60% interpretation text (FinalAssessmentState.Interpretation), which otherwise reads
/// "...на седмицата" — correct for every per-week assessment but wrong for a course-wide one (e.g.
/// FinalExamCatalog's "...на материала от курса"). Defaults to null so every existing weekly model
/// (all constructed as `new(questions)`) is completely unaffected; only a model that explicitly sets
/// this gets different wording. Score bands, thresholds, and every other tier are untouched.</summary>
public sealed record FinalAssessmentModel(IReadOnlyList<AssessmentQuestion> Questions, string? LowScoreInterpretation = null);
