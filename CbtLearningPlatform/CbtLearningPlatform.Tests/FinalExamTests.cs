using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>Course-level final exam (/kurs/finalen-izpit) — 20 questions across all 15 weeks: 4
/// Fundamental + 8 Applied + 8 Integrative. This is the difficulty-audit redesign of the original
/// 15-anchor + 5-integrative set, which let an untrained reader score ~85% purely from mechanical
/// defects (19/20 correct answers at option index 1, only 3 options per item, correct answer usually
/// longest). Alongside the usual structure/navigation/engine-reuse/safety coverage, this file carries
/// dedicated regression tests against those exact defects: exactly 4 options per item (never 3), and
/// the correct-answer index distributed 5/5/5/5 across all four positions (never concentrated).
/// Protects the exam's structure, its navigation entry points, its reuse of the one shared assessment
/// engine (never a parallel quiz implementation), and the safety/product boundaries the owner set: no
/// self-input clinical procedure, no pass/fail threshold, no persistence or certificate, and no
/// correctness revealed before submit.</summary>
public sealed class FinalExamTests
{
    // ---- Route and navigation ----

    [Fact]
    public void FinalExamPage_IsRoutedAtKursFinalenIzpit()
    {
        Assert.Contains("@page \"/kurs/finalen-izpit\"", ReadPage("FinalenIzpit.razor"));
    }

    [Fact]
    public void Sidebar_HasTheFinalExamNavigationItem()
    {
        string source = ReadLayout("MainLayout.razor");

        Assert.Contains("href=\"/kurs/finalen-izpit\"", source);
        Assert.Contains("Краен изпит", source);
    }

    [Fact]
    public void Sidebar_DoesNotListIndividualWeeks()
    {
        // The exam item is one entry in the course-navigation group — it must not have dragged 15
        // week links into the sidebar with it.
        string source = ReadLayout("MainLayout.razor");

        foreach (int week in Enumerable.Range(1, 15))
        {
            Assert.DoesNotContain($"href=\"/kurs/sedmica-{week}\"", source);
        }
    }

    [Fact]
    public void KursPage_LinksToTheFinalExam()
    {
        Assert.Contains("href=\"/kurs/finalen-izpit\"", ReadPage("Kurs.razor"));
    }

    [Fact]
    public void ProgramaPage_LinksToTheFinalExam()
    {
        Assert.Contains("href=\"/kurs/finalen-izpit\"", ReadPage("Programa.razor"));
    }

    [Fact]
    public void KursAndProgramaPages_LinkToTheExamWithoutDuplicatingItsContent()
    {
        foreach (string fileName in new[] { "Kurs.razor", "Programa.razor" })
        {
            string source = ReadPage(fileName);
            Assert.DoesNotContain("<FinalAssessment", source);
            Assert.DoesNotContain("FinalExamCatalog", source);
        }
    }

    // ---- Question-set structure ----

    [Fact]
    public void Exam_HasExactlyTwentyQuestions()
    {
        Assert.Equal(20, FinalExamCatalog.Items.Count);
        Assert.Equal(20, FinalExamCatalog.Model.Questions.Count);
    }

    [Fact]
    public void Exam_HasExactlyFourFundamental_EightApplied_EightIntegrative()
    {
        Assert.Equal(4, FinalExamCatalog.Items.Count(item => item.Level == ExamQuestionLevel.Fundamental));
        Assert.Equal(8, FinalExamCatalog.Items.Count(item => item.Level == ExamQuestionLevel.Applied));
        Assert.Equal(8, FinalExamCatalog.Items.Count(item => item.Level == ExamQuestionLevel.Integrative));
    }

    [Fact]
    public void Exam_EveryIntegrativeItem_SpansAtLeastTwoWeeks()
    {
        // Fundamental/Applied items may legitimately span one week or several (e.g. a Fundamental item
        // contrasting Week 12 and Week 3) — only Integrative carries a hard "at least two weeks" rule.
        foreach (FinalExamItem item in FinalExamCatalog.Items.Where(item => item.Level == ExamQuestionLevel.Integrative))
        {
            Assert.True(item.SourceWeeks.Count >= 2);
        }
    }

    [Fact]
    public void Exam_AllFifteenWeeksAreRepresentedAtLeastOnce_AndNoWeekDominates()
    {
        Assert.Equal(15, CourseCatalog.Weeks.Count);

        foreach (CourseWeekDefinition week in CourseCatalog.Weeks)
        {
            int occurrences = FinalExamCatalog.Items.Count(item => item.SourceWeeks.Contains(week.Number));
            Assert.InRange(occurrences, 1, 3);
        }
    }

    [Fact]
    public void Exam_EveryReferencedWeekIsARealRoutedWeek()
    {
        int[] routedWeeks = [.. CourseCatalog.Weeks.Where(w => w.Route is not null).Select(w => w.Number)];

        foreach (FinalExamItem item in FinalExamCatalog.Items)
        {
            Assert.All(item.SourceWeeks, week => Assert.Contains(week, routedWeeks));
            Assert.Equal(item.SourceWeeks.Count, item.SourceWeeks.Distinct().Count());
        }
    }

    [Fact]
    public void Exam_HasNoDuplicateQuestionIds()
    {
        string[] ids = [.. FinalExamCatalog.Model.Questions.Select(q => q.Id)];

        Assert.Equal(ids.Length, ids.Distinct().Count());
    }

    [Fact]
    public void Exam_EveryQuestionIsWellFormed()
    {
        foreach (AssessmentQuestion question in FinalExamCatalog.Model.Questions)
        {
            Assert.True(question.Options.Count >= 2);
            Assert.InRange(question.CorrectOptionIndex, 0, question.Options.Count - 1);
            Assert.Equal(question.Options.Count, question.Options.Distinct().Count());
            Assert.False(string.IsNullOrWhiteSpace(question.Text));
            Assert.False(string.IsNullOrWhiteSpace(question.Explanation));
        }
    }

    // ---- Regression protection: the original mechanical defects (owner difficulty audit) ----
    //
    // The original 20-question set let an untrained reader score ~85% without course knowledge, for
    // reasons that had nothing to do with question content: it used 3 options per item (33% guess
    // baseline instead of 25%) and put the correct answer at option index 1 in 19 of 20 items, so
    // "pick the middle option" alone was a winning strategy. These two tests guard the mechanical fix
    // permanently — they must keep passing even if question wording changes in the future.

    [Fact]
    public void Exam_NoThreeOptionItemsRemain_EveryQuestionHasExactlyFourOptions()
    {
        foreach (AssessmentQuestion question in FinalExamCatalog.Model.Questions)
        {
            Assert.Equal(4, question.Options.Count);
        }
    }

    [Fact]
    public void Exam_CorrectAnswerPosition_HasNoConcentration_DistributionIsExactlyFiveAtEachIndex()
    {
        Dictionary<int, int> countsByIndex = FinalExamCatalog.Model.Questions
            .GroupBy(q => q.CorrectOptionIndex)
            .ToDictionary(g => g.Key, g => g.Count());

        Assert.Equal(4, countsByIndex.Count); // all four indices (0-3) actually used
        for (int index = 0; index <= 3; index++)
        {
            Assert.Equal(5, countsByIndex.GetValueOrDefault(index));
        }
    }

    [Fact]
    public void Exam_QuestionsAreIndependentlyAuthored_NotCopiedFromTheWeeklyAssessments()
    {
        // Weekly questions informed coverage only; the exam wording is its own. A verbatim question
        // stem lifted from the week's page would fail here.
        foreach (FinalExamItem item in FinalExamCatalog.Items)
        {
            foreach (int week in item.SourceWeeks)
            {
                Assert.DoesNotContain(item.Question.Text, ReadPage($"Sedmica{week}.razor"));
            }
        }
    }

    [Fact]
    public void Exam_EveryQuestionCarriesReviewLinksToItsSourceWeeks()
    {
        foreach (FinalExamItem item in FinalExamCatalog.Items)
        {
            foreach (int week in item.SourceWeeks)
            {
                Assert.Contains($"href=\"/kurs/sedmica-{week}\"", item.Question.Explanation);
            }
        }
    }

    // ---- Shared engine, not a parallel implementation ----

    [Fact]
    public void ExamPage_UsesTheSharedFinalAssessmentComponent_ExactlyOnce()
    {
        string source = ReadPage("FinalenIzpit.razor");

        Assert.Equal(1, source.Split("<FinalAssessment ").Length - 1);
        Assert.Contains("<FinalAssessment SectionId=\"izpit\" Model=\"FinalExamCatalog.Model\" />", source);
    }

    [Fact]
    public void ExamPage_HasNoLocalAssessmentImplementation()
    {
        // Scanned below the page's own @* … *@ header comment, which legitimately names the shared
        // engine it reuses — the point is that the page implements none of it.
        string source = PageBody("FinalenIzpit.razor");

        foreach (string marker in new[]
                 {
                     "FinalAssessmentState", "new FinalAssessmentModel", "AssessmentQuestion.",
                     "<button", "@onclick", "@onchange", "CorrectOptionIndex", "@code"
                 })
        {
            Assert.DoesNotContain(marker, source);
        }
    }

    [Fact]
    public void SharedAssessmentEngine_WasNotForkedOrSpecialCasedForTheExam()
    {
        string component = ReadInteractive("FinalAssessment.razor");
        string state = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Curriculum", "FinalAssessmentState.cs");

        Assert.DoesNotContain("FinalExamCatalog", component);
        Assert.DoesNotContain("FinalExamCatalog", File.ReadAllText(state));
    }

    // ---- Behavior: scoring, retry, no reveal before submit ----

    [Fact]
    public void Exam_ScoreIsZeroToHundred_AcrossAllCorrectAllWrongAndMixed()
    {
        FinalAssessmentState allCorrect = Answer(correct: 20);
        Assert.Equal(100, allCorrect.ScorePercent);
        Assert.Equal(20, allCorrect.CorrectCount);
        Assert.Equal(0, allCorrect.IncorrectCount);

        FinalAssessmentState allWrong = Answer(correct: 0);
        Assert.Equal(0, allWrong.ScorePercent);
        Assert.Equal(0, allWrong.CorrectCount);
        Assert.Equal(20, allWrong.IncorrectCount);

        FinalAssessmentState mixed = Answer(correct: 13);
        Assert.Equal(65, mixed.ScorePercent);
        Assert.Equal(13, mixed.CorrectCount);
        Assert.Equal(7, mixed.IncorrectCount);
        Assert.InRange(mixed.ScorePercent, 0, 100);
    }

    [Fact]
    public void Exam_CannotBeSubmittedUntilAllTwentyAreAnswered()
    {
        FinalAssessmentState state = new(FinalExamCatalog.Model);

        for (int i = 0; i < 19; i++)
        {
            state.SelectAnswer(i, 0);
            Assert.False(state.AllAnswered);
            Assert.False(state.Submit());
            Assert.False(state.Submitted);
        }

        state.SelectAnswer(19, 0);
        Assert.True(state.AllAnswered);
        Assert.True(state.Submit());
    }

    [Fact]
    public void Exam_RevealsNoCorrectnessBeforeSubmit()
    {
        FinalAssessmentState state = new(FinalExamCatalog.Model);

        for (int i = 0; i < FinalExamCatalog.Model.Questions.Count; i++)
        {
            state.SelectAnswer(i, FinalExamCatalog.Model.Questions[i].CorrectOptionIndex);
            Assert.False(state.IsCorrect(i));
        }

        Assert.False(state.Submitted);
        Assert.Equal(0, state.ScorePercent);
        Assert.Equal(0, state.CorrectCount);

        // The page itself must not print an answer key or explanation outside the component either.
        string source = ReadPage("FinalenIzpit.razor");
        Assert.DoesNotContain("Верен отговор", source);
    }

    [Fact]
    public void Exam_BelowSixtyPercent_UsesCourseLevelLowScoreWording_NotTheWeeklyOne()
    {
        FinalAssessmentState state = Answer(correct: 5); // 25% -> below 60

        Assert.Equal("Препоръчителен повторен преглед на материала от курса", state.Interpretation);
        Assert.DoesNotContain("седмицата", state.Interpretation);
    }

    [Fact]
    public void Exam_OtherScoreBands_MatchTheSharedEngineUnchanged()
    {
        Assert.Equal("Отлично усвояване", Answer(correct: 20).Interpretation);
        Assert.Equal("Нужен е кратък преговор", Answer(correct: 13).Interpretation); // 65%
    }

    [Fact]
    public void Exam_RetryResetsTheWholeExam()
    {
        FinalAssessmentState state = Answer(correct: 20);
        Assert.True(state.Submitted);

        state.Retry();

        Assert.False(state.Submitted);
        Assert.Equal(0, state.ScorePercent);
        Assert.Equal(0, state.CorrectCount);
        Assert.Equal(0, state.IncorrectCount);
        Assert.Equal(0, state.AnsweredCount);
        Assert.All(Enumerable.Range(0, state.Total), i => Assert.Null(state.SelectedOption(i)));
    }

    // ---- Safety and product boundaries ----

    [Fact]
    public void ExamPage_HasNoSelfInputClinicalProcedureOrForm()
    {
        string source = ReadPage("FinalenIzpit.razor");

        foreach (string marker in new[] { "<input", "<form", "<textarea", "localStorage", "HttpClient" })
        {
            Assert.DoesNotContain(marker, source);
        }
    }

    [Fact]
    public void Exam_ContentCarriesNoClinicalSelfAssessmentOrDiagnosticProcedure()
    {
        string examContent = string.Join(
            "\n",
            FinalExamCatalog.Model.Questions.Select(q => $"{q.Text}\n{string.Join("\n", q.Options)}\n{q.Explanation}"));
        string all = $"{ReadPage("FinalenIzpit.razor")}\n{examContent}";

        foreach (string marker in new[]
                 {
                     "самодиагно", "диагностицирайте", "оценете себе си", "вашата диагноза",
                     "коя категория сте", "вашето основно вярване", "BDI", "BAI", "C-SSRS",
                     "DSM-5-TR", "критерий A", "скала за суициден риск", "суициден", "самонараняване",
                     "експозиция стъпка", "протокол за прилагане"
                 })
        {
            Assert.DoesNotContain(marker, all, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void ExamPage_HasNoPassFailThresholdCertificateOrTimer()
    {
        string source = ReadPage("FinalenIzpit.razor");

        foreach (string marker in new[]
                 {
                     "издържал", "преминал", "успешно завършил", "минимален резултат", "праг за",
                     "точки за преминаване", "таймер", "оставащо време", "точки опит", "ниво "
                 })
        {
            Assert.DoesNotContain(marker, source, StringComparison.OrdinalIgnoreCase);
        }

        // The educational boundary is stated explicitly instead.
        Assert.Contains("не професионална квалификация", source);
        Assert.Contains("не е сертификат", source);
    }

    [Fact]
    public void ExamPage_StatesItsCoverageAndRepeatability()
    {
        string source = ReadPage("FinalenIzpit.razor");

        Assert.Contains("20 въпроса", source);
        Assert.Contains("15 седмици", source);
        Assert.Contains("от 0 до 100", source);
        Assert.Contains("интегративни", source);
    }

    [Fact]
    public void ExamPage_DoesNotAlterWeeklyAssessmentsOrCourseProgress()
    {
        string source = ReadPage("FinalenIzpit.razor");

        Assert.DoesNotContain("WeekCompletionControl", source);
        Assert.DoesNotContain("CourseProgressService", source);
    }

    private static FinalAssessmentState Answer(int correct)
    {
        FinalAssessmentState state = new(FinalExamCatalog.Model);

        for (int i = 0; i < state.Total; i++)
        {
            AssessmentQuestion question = FinalExamCatalog.Model.Questions[i];
            int wrong = (question.CorrectOptionIndex + 1) % question.Options.Count;
            state.SelectAnswer(i, i < correct ? question.CorrectOptionIndex : wrong);
        }

        state.Submit();
        return state;
    }

    /// <summary>A Razor page's content with its leading <c>@* … *@</c> authoring comment removed.</summary>
    private static string PageBody(string fileName)
    {
        string source = ReadPage(fileName);
        int end = source.IndexOf("*@", StringComparison.Ordinal);
        return source.StartsWith("@*", StringComparison.Ordinal) && end > 0 ? source[(end + 2)..] : source;
    }

    private static string ReadPage(string fileName) =>
        File.ReadAllText(Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages", fileName));

    private static string ReadLayout(string fileName) =>
        File.ReadAllText(Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Layout", fileName));

    private static string ReadInteractive(string fileName) =>
        File.ReadAllText(Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Interactive", fileName));
}
