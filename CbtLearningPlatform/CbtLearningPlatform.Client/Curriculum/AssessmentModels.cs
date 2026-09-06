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

public sealed record FinalAssessmentModel(IReadOnlyList<AssessmentQuestion> Questions);
