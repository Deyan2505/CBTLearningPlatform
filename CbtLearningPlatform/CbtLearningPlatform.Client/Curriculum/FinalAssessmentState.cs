namespace CbtLearningPlatform.Client.Curriculum;

/// <summary>Pure state machine for the Weekly Final Assessment Standard — select answers (blocked once
/// submitted), submit to lock + score (blocked until every question is answered), retry to reset to a
/// blank test. Kept free of Blazor so it's unit-testable directly; FinalAssessment.razor only renders it.</summary>
public sealed class FinalAssessmentState
{
    private readonly FinalAssessmentModel _model;
    private int?[] _selected;

    public FinalAssessmentState(FinalAssessmentModel model)
    {
        _model = model;
        _selected = new int?[model.Questions.Count];
    }

    public int Total => _model.Questions.Count;
    public bool Submitted { get; private set; }
    public int CorrectCount { get; private set; }
    public int IncorrectCount { get; private set; }
    public int ScorePercent { get; private set; }

    public int? SelectedOption(int questionIndex) => _selected[questionIndex];
    public int AnsweredCount => _selected.Count(s => s.HasValue);
    public bool AllAnswered => AnsweredCount == Total;

    public void SelectAnswer(int questionIndex, int optionIndex)
    {
        if (Submitted) return;
        _selected[questionIndex] = optionIndex;
    }

    /// <returns>False (no-op) when not every question is answered yet.</returns>
    public bool Submit()
    {
        if (!AllAnswered) return false;

        CorrectCount = 0;
        for (int i = 0; i < Total; i++)
        {
            if (_selected[i] == _model.Questions[i].CorrectOptionIndex) CorrectCount++;
        }

        IncorrectCount = Total - CorrectCount;
        ScorePercent = (int)Math.Round(CorrectCount / (double)Total * 100, MidpointRounding.AwayFromZero);
        Submitted = true;
        return true;
    }

    public void Retry()
    {
        _selected = new int?[Total];
        Submitted = false;
        CorrectCount = 0;
        IncorrectCount = 0;
        ScorePercent = 0;
    }

    public bool IsCorrect(int questionIndex) =>
        Submitted && _selected[questionIndex] == _model.Questions[questionIndex].CorrectOptionIndex;

    public string Interpretation => ScorePercent switch
    {
        >= 90 => "Отлично усвояване",
        >= 75 => "Добро усвояване",
        >= 60 => "Нужен е кратък преговор",
        _ => _model.LowScoreInterpretation ?? "Препоръчителен повторен преглед на седмицата"
    };
}
