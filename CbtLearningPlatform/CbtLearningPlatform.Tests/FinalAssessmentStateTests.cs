using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>Weekly Final Assessment Standard — behavioral coverage for the shared state machine
/// (FinalAssessmentState) that every routed week's final assessment delegates to. Regression coverage
/// confirming every week actually uses the shared component lives in the per-week ContentSliceTests
/// files (each asserts a &lt;FinalAssessment SectionId="..." Model="..." /&gt; usage) plus
/// FinalAssessmentRolloutTests below.</summary>
public class FinalAssessmentStateTests
{
    private static FinalAssessmentModel MakeModel(int questionCount, int correctOptionIndex = 0) => new(
        Enumerable.Range(0, questionCount)
            .Select(i => AssessmentQuestion.Choice($"q{i}", $"Question {i}", ["A", "B", "C"], correctOptionIndex, $"Explanation {i}"))
            .ToList());

    [Fact]
    public void Submit_BeforeAllQuestionsAnswered_IsRejected()
    {
        FinalAssessmentState state = new(MakeModel(3));
        state.SelectAnswer(0, 0);
        state.SelectAnswer(1, 0);
        // Question 2 left unanswered.

        bool accepted = state.Submit();

        Assert.False(accepted);
        Assert.False(state.Submitted);
    }

    [Fact]
    public void AllAnswered_BecomesTrueOnlyOnceEveryQuestionHasASelection()
    {
        FinalAssessmentState state = new(MakeModel(2));

        Assert.False(state.AllAnswered);
        Assert.Equal(0, state.AnsweredCount);

        state.SelectAnswer(0, 1);
        Assert.False(state.AllAnswered);
        Assert.Equal(1, state.AnsweredCount);

        state.SelectAnswer(1, 2);
        Assert.True(state.AllAnswered);
        Assert.Equal(2, state.AnsweredCount);
    }

    [Fact]
    public void IsCorrect_ReturnsFalseBeforeSubmission_EvenWhenTheSelectedAnswerIsCorrect()
    {
        // Correctness must never be revealed while answering — only after Submit().
        FinalAssessmentState state = new(MakeModel(1, correctOptionIndex: 0));
        state.SelectAnswer(0, 0);

        Assert.False(state.IsCorrect(0));
        Assert.False(state.Submitted);
    }

    [Fact]
    public void SelectAnswer_AfterSubmit_IsIgnored_AnswersAreLocked()
    {
        FinalAssessmentState state = new(MakeModel(1, correctOptionIndex: 0));
        state.SelectAnswer(0, 0);
        state.Submit();

        state.SelectAnswer(0, 1); // attempt to change the locked answer

        Assert.Equal(0, state.SelectedOption(0));
        Assert.True(state.IsCorrect(0));
    }

    [Fact]
    public void Submit_AllCorrect_Scores100AndClassifiesEveryQuestionCorrect()
    {
        FinalAssessmentModel model = MakeModel(4, correctOptionIndex: 0);
        FinalAssessmentState state = new(model);
        for (int i = 0; i < 4; i++) state.SelectAnswer(i, 0);

        Assert.True(state.Submit());

        Assert.Equal(100, state.ScorePercent);
        Assert.Equal(4, state.CorrectCount);
        Assert.Equal(0, state.IncorrectCount);
        Assert.Equal("Отлично усвояване", state.Interpretation);
        for (int i = 0; i < 4; i++) Assert.True(state.IsCorrect(i));
    }

    [Fact]
    public void Submit_AllWrong_Scores0()
    {
        FinalAssessmentModel model = MakeModel(4, correctOptionIndex: 0);
        FinalAssessmentState state = new(model);
        for (int i = 0; i < 4; i++) state.SelectAnswer(i, 1); // every answer wrong

        state.Submit();

        Assert.Equal(0, state.ScorePercent);
        Assert.Equal(0, state.CorrectCount);
        Assert.Equal(4, state.IncorrectCount);
        Assert.Equal("Препоръчителен повторен преглед на седмицата", state.Interpretation);
        for (int i = 0; i < 4; i++) Assert.False(state.IsCorrect(i));
    }

    [Fact]
    public void Submit_PartialScore_ClassifiesEachQuestionIndependently()
    {
        FinalAssessmentModel model = MakeModel(4, correctOptionIndex: 0);
        FinalAssessmentState state = new(model);
        state.SelectAnswer(0, 0); // correct
        state.SelectAnswer(1, 1); // wrong
        state.SelectAnswer(2, 0); // correct
        state.SelectAnswer(3, 1); // wrong

        state.Submit();

        Assert.Equal(50, state.ScorePercent);
        Assert.Equal(2, state.CorrectCount);
        Assert.Equal(2, state.IncorrectCount);
        Assert.True(state.IsCorrect(0));
        Assert.False(state.IsCorrect(1));
        Assert.True(state.IsCorrect(2));
        Assert.False(state.IsCorrect(3));
    }

    [Theory]
    [InlineData(9, 12, 75)]   // exact
    [InlineData(1, 3, 33)]    // 33.33 -> rounds down
    [InlineData(2, 3, 67)]    // 66.67 -> rounds up
    [InlineData(1, 8, 13)]    // 12.5  -> half rounds away from zero, not banker's rounding to 12
    public void Score_RoundsCorrectPercentageToNearestWholeNumber(int correct, int total, int expectedScore)
    {
        FinalAssessmentModel model = MakeModel(total, correctOptionIndex: 0);
        FinalAssessmentState state = new(model);
        for (int i = 0; i < total; i++) state.SelectAnswer(i, i < correct ? 0 : 1);

        state.Submit();

        Assert.Equal(expectedScore, state.ScorePercent);
    }

    [Theory]
    [InlineData(90, "Отлично усвояване")]
    [InlineData(75, "Добро усвояване")]
    [InlineData(60, "Нужен е кратък преговор")]
    [InlineData(59, "Препоръчителен повторен преглед на седмицата")]
    public void Interpretation_MatchesScoreTier(int correctOutOf100, string expected)
    {
        FinalAssessmentModel model = MakeModel(100, correctOptionIndex: 0);
        FinalAssessmentState state = new(model);
        for (int i = 0; i < 100; i++) state.SelectAnswer(i, i < correctOutOf100 ? 0 : 1);

        state.Submit();

        Assert.Equal(expected, state.Interpretation);
    }

    // ---- LowScoreInterpretation override (course-level exam vs. per-week default) ----

    [Fact]
    public void Interpretation_BelowSixty_DefaultsToTheWeeklyWording_WhenModelSetsNoOverride()
    {
        // Every existing weekly model is built as `new(questions)` — LowScoreInterpretation defaults
        // to null — so this must remain the exact pre-existing behavior with zero opt-in required.
        FinalAssessmentModel model = MakeModel(4, correctOptionIndex: 0);
        FinalAssessmentState state = new(model);
        for (int i = 0; i < 4; i++) state.SelectAnswer(i, 1); // all wrong -> < 60%

        state.Submit();

        Assert.Equal("Препоръчителен повторен преглед на седмицата", state.Interpretation);
    }

    [Fact]
    public void Interpretation_BelowSixty_UsesModelOverride_WhenModelSuppliesOne()
    {
        FinalAssessmentModel model = MakeModel(4, correctOptionIndex: 0) with
        {
            LowScoreInterpretation = "Препоръчителен повторен преглед на материала от курса"
        };
        FinalAssessmentState state = new(model);
        for (int i = 0; i < 4; i++) state.SelectAnswer(i, 1); // all wrong -> < 60%

        state.Submit();

        Assert.Equal("Препоръчителен повторен преглед на материала от курса", state.Interpretation);
    }

    [Fact]
    public void Interpretation_AtOrAboveSixty_IgnoresTheOverride_RegardlessOfModel()
    {
        // The override only ever applies to the below-60 tier — it must not leak into or otherwise
        // change any other score band.
        FinalAssessmentModel model = MakeModel(100, correctOptionIndex: 0) with
        {
            LowScoreInterpretation = "Препоръчителен повторен преглед на материала от курса"
        };
        FinalAssessmentState state = new(model);

        foreach ((int correctOutOf100, string expected) in new[]
                 {
                     (90, "Отлично усвояване"),
                     (75, "Добро усвояване"),
                     (60, "Нужен е кратък преговор")
                 })
        {
            for (int i = 0; i < 100; i++) state.SelectAnswer(i, i < correctOutOf100 ? 0 : 1);
            state.Submit();
            Assert.Equal(expected, state.Interpretation);
            state.Retry();
        }
    }

    [Fact]
    public void Retry_ClearsAnswersScoreAndSubmittedState()
    {
        FinalAssessmentModel model = MakeModel(3, correctOptionIndex: 0);
        FinalAssessmentState state = new(model);
        state.SelectAnswer(0, 0);
        state.SelectAnswer(1, 1);
        state.SelectAnswer(2, 0);
        state.Submit();

        state.Retry();

        Assert.False(state.Submitted);
        Assert.Equal(0, state.ScorePercent);
        Assert.Equal(0, state.CorrectCount);
        Assert.Equal(0, state.IncorrectCount);
        Assert.Equal(0, state.AnsweredCount);
        Assert.False(state.AllAnswered);
        for (int i = 0; i < 3; i++) Assert.Null(state.SelectedOption(i));
    }

    [Fact]
    public void Retry_ThenAnsweringAndSubmittingAgain_ProducesAFreshScore()
    {
        FinalAssessmentModel model = MakeModel(2, correctOptionIndex: 0);
        FinalAssessmentState state = new(model);
        state.SelectAnswer(0, 1);
        state.SelectAnswer(1, 1);
        state.Submit(); // 0/100

        state.Retry();
        state.SelectAnswer(0, 0);
        state.SelectAnswer(1, 0);
        state.Submit(); // 100/100 this time

        Assert.Equal(100, state.ScorePercent);
        Assert.Equal(2, state.CorrectCount);
    }
}
