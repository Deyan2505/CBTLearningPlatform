using System.Reflection;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>End-to-end stress test of one learner's full course journey: week 1 through week 15's own
/// Weekly Final Assessment, then the course-level final exam, then the Course Completion Certificate —
/// exercised together through the real engine (FinalAssessmentState), the real per-week question banks
/// (fetched by reflection, the same technique Week1ContentSliceTests already uses for
/// CbtLearningPlatform.Client types — each Sedmica{N}.razor keeps its own _week{N}FinalAssessment as a
/// private static field), the real localStorage-shaped round trip (CourseProgressStore), and the real
/// CertificateEligibility rule. Every other test file in this project covers exactly one of these units
/// in isolation; this one walks the full path a real learner takes, start to finish, in one run.</summary>
public sealed class StudentJourneyStressTests
{
    [Fact]
    public void FullJourney_AllWeeksPassed_ThenFinalExamPassed_UnlocksTheCertificate()
    {
        IReadOnlySet<int> completed = new HashSet<int>();

        foreach (CourseWeekDefinition week in CourseCatalog.Weeks.Where(w => w.Route is not null).OrderBy(w => w.Number))
        {
            Assert.False(completed.Contains(week.Number));

            int weeklyScore = TakeAssessment(GetWeekFinalAssessmentModel(week.Number), answerCorrectly: true);
            Assert.Equal(100, weeklyScore);

            // Round-trip through the real serialize/deserialize pair, exactly as a page reload would.
            completed = CourseProgressStore.Deserialize(
                CourseProgressStore.Serialize(new HashSet<int>(completed) { week.Number }));

            CourseProgressSummary progress = CourseProgressCalculator.Summarize(CourseCatalog.Weeks, completed);
            Assert.False(CertificateEligibility.IsEligible(progress.CompletedCount, progress.TotalWeeks, currentExamScore: null));
        }

        CourseProgressSummary finalProgress = CourseProgressCalculator.Summarize(CourseCatalog.Weeks, completed);
        // Current catalog state: all 15 weeks are routed (see CourseCatalog.cs), so finishing every
        // routed week finishes the whole course. If a future week ships without a page, this equality
        // is the signal that the journey can no longer reach 100% and needs updating alongside it.
        Assert.Equal(finalProgress.TotalWeeks, finalProgress.CompletedCount);

        int examScore = TakeAssessment(FinalExamCatalog.Model, answerCorrectly: true);
        Assert.Equal(100, examScore);

        Assert.True(CertificateEligibility.IsEligible(finalProgress.CompletedCount, finalProgress.TotalWeeks, examScore));
    }

    [Fact]
    public void FullJourney_OneWeekLeftIncomplete_CertificateStaysLockedEvenWithAPerfectExam()
    {
        HashSet<int> completed = [.. CourseCatalog.Weeks.Where(w => w.Route is not null).Select(w => w.Number)];
        completed.Remove(completed.Max()); // a learner who skipped exactly one week

        CourseProgressSummary progress = CourseProgressCalculator.Summarize(CourseCatalog.Weeks, completed);
        int examScore = TakeAssessment(FinalExamCatalog.Model, answerCorrectly: true);

        Assert.Equal(100, examScore);
        Assert.False(CertificateEligibility.IsEligible(progress.CompletedCount, progress.TotalWeeks, examScore));
    }

    [Fact]
    public void FullJourney_FailedFinalExamFirst_RetryReachesTheCertificate()
    {
        HashSet<int> completed = [.. CourseCatalog.Weeks.Where(w => w.Route is not null).Select(w => w.Number)];
        CourseProgressSummary progress = CourseProgressCalculator.Summarize(CourseCatalog.Weeks, completed);

        FinalAssessmentState exam = new(FinalExamCatalog.Model);
        AnswerAll(exam, FinalExamCatalog.Model, answerCorrectly: false);
        exam.Submit();

        Assert.Equal(0, exam.ScorePercent);
        Assert.False(CertificateEligibility.IsEligible(progress.CompletedCount, progress.TotalWeeks, exam.ScorePercent));

        exam.Retry();
        AnswerAll(exam, FinalExamCatalog.Model, answerCorrectly: true);
        exam.Submit();

        Assert.Equal(100, exam.ScorePercent);
        Assert.True(CertificateEligibility.IsEligible(progress.CompletedCount, progress.TotalWeeks, exam.ScorePercent));
    }

    [Fact]
    public void FullJourney_FinalExamScoreAtTheDocumentedBoundary_MatchesEligibility()
    {
        // FinalExamCatalog has exactly 20 questions, so 15/20 correct lands exactly on
        // CertificateEligibility.MinimumExamScore — the real boundary a real learner would hit, not a
        // hand-picked integer fed straight into the pure function (already covered separately by
        // CertificateEligibilityTests).
        Assert.Equal(20, FinalExamCatalog.Model.Questions.Count);

        HashSet<int> completed = [.. CourseCatalog.Weeks.Where(w => w.Route is not null).Select(w => w.Number)];
        CourseProgressSummary progress = CourseProgressCalculator.Summarize(CourseCatalog.Weeks, completed);

        FinalAssessmentState exam = new(FinalExamCatalog.Model);
        AnswerFirstNCorrectly(exam, FinalExamCatalog.Model, correctCount: 15);
        exam.Submit();

        Assert.Equal(CertificateEligibility.MinimumExamScore, exam.ScorePercent);
        Assert.True(CertificateEligibility.IsEligible(progress.CompletedCount, progress.TotalWeeks, exam.ScorePercent));

        exam.Retry();
        AnswerFirstNCorrectly(exam, FinalExamCatalog.Model, correctCount: 14);
        exam.Submit();

        Assert.Equal(70, exam.ScorePercent);
        Assert.False(CertificateEligibility.IsEligible(progress.CompletedCount, progress.TotalWeeks, exam.ScorePercent));
    }

    [Fact]
    public void FullJourney_CorruptedProgressMidCourse_NeverSilentlyGrantsTheCertificate()
    {
        // A mid-course learner has genuinely finished 10 weeks, then the stored value gets corrupted
        // (browser/extension/manual tampering) — the storage layer must reset, not crash or inflate.
        HashSet<int> completed = [.. Enumerable.Range(1, 10)];
        string corrupted = CourseProgressStore.Serialize(completed)[..5]; // truncated, invalid JSON

        IReadOnlySet<int> recovered = CourseProgressStore.Deserialize(corrupted);

        Assert.Empty(recovered);
        CourseProgressSummary progress = CourseProgressCalculator.Summarize(CourseCatalog.Weeks, recovered);
        Assert.False(CertificateEligibility.IsEligible(progress.CompletedCount, progress.TotalWeeks, currentExamScore: 100));
    }

    private static int TakeAssessment(FinalAssessmentModel model, bool answerCorrectly)
    {
        FinalAssessmentState state = new(model);
        AnswerAll(state, model, answerCorrectly);
        Assert.True(state.Submit());
        return state.ScorePercent;
    }

    private static void AnswerAll(FinalAssessmentState state, FinalAssessmentModel model, bool answerCorrectly)
    {
        for (int i = 0; i < model.Questions.Count; i++)
        {
            AssessmentQuestion question = model.Questions[i];
            state.SelectAnswer(i, answerCorrectly ? question.CorrectOptionIndex : WrongOptionIndex(question));
        }
    }

    private static void AnswerFirstNCorrectly(FinalAssessmentState state, FinalAssessmentModel model, int correctCount)
    {
        for (int i = 0; i < model.Questions.Count; i++)
        {
            AssessmentQuestion question = model.Questions[i];
            state.SelectAnswer(i, i < correctCount ? question.CorrectOptionIndex : WrongOptionIndex(question));
        }
    }

    private static int WrongOptionIndex(AssessmentQuestion question) =>
        Enumerable.Range(0, question.Options.Count).First(i => i != question.CorrectOptionIndex);

    private static FinalAssessmentModel GetWeekFinalAssessmentModel(int weekNumber)
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");
        Type pageType = assembly.GetType($"CbtLearningPlatform.Client.Components.Pages.Sedmica{weekNumber}")
            ?? throw new InvalidOperationException($"Sedmica{weekNumber} page type not found.");
        FieldInfo field = pageType.GetField($"_week{weekNumber}FinalAssessment", BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException($"_week{weekNumber}FinalAssessment field not found on Sedmica{weekNumber}.");
        return (FinalAssessmentModel)field.GetValue(null)!;
    }
}
