using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>Active Learning Toolkit — pure state machines, data validation and safety modes. The rule under test everywhere:
/// no feedback of any kind exists until the learner has committed. Fixtures are neutral placeholders, never CBT content.</summary>
public sealed class ActiveLearningToolkitStateTests
{
    // ---------------------------------------------------------------- fixtures

    private static ClassifyMatchActivity Classify(ClassifyMatchMode mode = ClassifyMatchMode.Classify) => new(
        "T", "I", mode,
        [new("o1", "One"), new("o2", "Two"), new("o3", "Three")],
        [
            new("a", "Item A", "o1", "Because A."),
            new("b", "Item B", "o2", "Because B."),
            new("c", "Item C", mode == ClassifyMatchMode.Match ? "o3" : "o1", "Because C.")
        ]);

    private static OrderingActivity Ordering(int count = 4, int seed = 0) => new(
        "T", "I",
        [.. Enumerable.Range(0, count).Select(i => new OrderingItem($"i{i}", $"Step {i}", $"Why {i}"))],
        Feedback: "Overall", ShuffleSeed: seed);

    private static PredictRevealActivity Predict(string? actual = "right", bool allowRetry = true) => new(
        "T", "Scenario", "Question?",
        [new("right", "Right", "Fb right"), new("wrong", "Wrong", "Fb wrong")],
        actual, "The explanation.", AllowRetry: allowRetry);

    // ---------------------------------------------------------------- A. classify / match

    [Fact]
    public void Classify_NoVerdictExistsBeforeTheLearnerChecks()
    {
        var state = new ClassifyMatchState(Classify());
        state.Select("a", "o1");

        Assert.Null(state.IsCorrect("a"));
        Assert.Equal(0, state.CorrectCount);
        Assert.False(state.IsChecked);
    }

    [Fact]
    public void Classify_CannotBeCheckedUntilEveryItemIsAnswered()
    {
        var state = new ClassifyMatchState(Classify());
        state.Select("a", "o1");
        state.Select("b", "o2");

        Assert.False(state.CanCheck);
        Assert.False(state.Check());

        state.Select("c", "o1");
        Assert.True(state.CanCheck);
        Assert.True(state.Check());
    }

    [Fact]
    public void Classify_CheckProducesPerItemVerdicts_AndAChoiceCanBeChangedBeforeChecking()
    {
        var state = new ClassifyMatchState(Classify());
        state.Select("a", "o2");
        state.Select("a", "o1"); // changed their mind
        state.Select("b", "o1");
        state.Select("c", "o1");
        state.Check();

        Assert.True(state.IsCorrect("a"));
        Assert.False(state.IsCorrect("b"));
        Assert.True(state.IsCorrect("c"));
        Assert.Equal(2, state.CorrectCount);
    }

    [Fact]
    public void Classify_IsLockedAfterChecking_UntilReset()
    {
        var state = new ClassifyMatchState(Classify());
        foreach (string id in new[] { "a", "b", "c" }) state.Select(id, "o1");
        state.Check();

        Assert.False(state.Select("a", "o2"));
        Assert.Equal("o1", state.Selection("a"));

        state.Reset();

        Assert.False(state.IsChecked);
        Assert.Equal(0, state.AnsweredCount);
        Assert.Null(state.IsCorrect("a"));
        Assert.True(state.Select("a", "o2"));
    }

    [Fact]
    public void Classify_IgnoresUnknownItemAndOptionIds()
    {
        var state = new ClassifyMatchState(Classify());

        Assert.False(state.Select("nope", "o1"));
        Assert.False(state.Select("a", "nope"));
        Assert.Equal(0, state.AnsweredCount);
    }

    [Fact]
    public void Match_IsOneToOne_DuplicateChoicesBlockTheCheck()
    {
        var state = new ClassifyMatchState(Classify(ClassifyMatchMode.Match));
        state.Select("a", "o1");
        state.Select("b", "o1"); // duplicate
        state.Select("c", "o3");

        Assert.True(state.HasDuplicateSelections);
        Assert.False(state.CanCheck);

        state.Select("b", "o2");
        Assert.False(state.HasDuplicateSelections);
        Assert.True(state.CanCheck);
    }

    [Fact]
    public void Classify_Mode_AllowsTheSameOptionForSeveralItems()
    {
        var state = new ClassifyMatchState(Classify());
        foreach (string id in new[] { "a", "b", "c" }) state.Select(id, "o1");

        Assert.False(state.HasDuplicateSelections);
        Assert.True(state.CanCheck);
    }

    [Fact]
    public void Classify_ValidationRejectsMalformedData_AndTheStateRefusesIt()
    {
        var bad = new ClassifyMatchActivity("", "I", ClassifyMatchMode.Match,
            [new("o1", "One"), new("o1", "Dup")],
            [new("a", "A", "missing", ""), new("a", "B", "o1", "x")]);

        IReadOnlyList<string> issues = bad.Validate();

        Assert.Contains(issues, i => i.Contains("Title"));
        Assert.Contains(issues, i => i.Contains("ids must be unique"));
        Assert.Contains(issues, i => i.Contains("unknown correct option"));
        Assert.Contains(issues, i => i.Contains("needs an explanation"));
        Assert.Throws<ArgumentException>(() => new ClassifyMatchState(bad));
    }

    [Fact]
    public void Match_ValidationRequiresDistinctCorrectOptions()
    {
        ClassifyMatchActivity notOneToOne = Classify() with { Mode = ClassifyMatchMode.Match };

        Assert.Contains(notOneToOne.Validate(), i => i.Contains("one-to-one"));
    }

    // ---------------------------------------------------------------- B. ordering builder

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(7)]
    public void Ordering_TheStartingScramble_IsAPermutation_NeverTheCorrectOrder_AndDeterministic(int count)
    {
        for (int seed = 0; seed < 60; seed++)
        {
            IReadOnlyList<int> order = OrderingBuilderState.InitialOrder(count, seed);

            Assert.Equal(Enumerable.Range(0, count), order.Order());
            Assert.False(order.SequenceEqual(Enumerable.Range(0, count)), $"seed {seed} produced the correct order");
            Assert.Equal(order, OrderingBuilderState.InitialOrder(count, seed));
        }
    }

    [Fact]
    public void Ordering_NoVerdictAndNoSolutionExistBeforeTheCheck()
    {
        var state = new OrderingBuilderState(Ordering());

        Assert.Null(state.IsPlacedCorrectly(0));
        Assert.Equal(0, state.CorrectPlacementCount);
        Assert.False(state.IsFullyCorrect);
        Assert.Null(state.Solution);
        Assert.False(state.RevealSolution()); // cannot peek without committing
        Assert.Null(state.Solution);
    }

    [Fact]
    public void Ordering_MovesAreBoundedAtTheEdges_AndSwapNeighbours()
    {
        var state = new OrderingBuilderState(Ordering());
        string first = state.CurrentOrder[0].Id;
        string second = state.CurrentOrder[1].Id;

        Assert.False(state.MoveUp(0));
        Assert.False(state.MoveDown(state.Total - 1));
        Assert.True(state.MoveDown(0));

        Assert.Equal(second, state.CurrentOrder[0].Id);
        Assert.Equal(first, state.CurrentOrder[1].Id);
    }

    [Fact]
    public void Ordering_CheckLocksTheArrangement_AndReportsPerPositionVerdicts()
    {
        var state = new OrderingBuilderState(Ordering());
        state.Check();

        Assert.False(state.CanMoveUp(1));
        Assert.False(state.CanMoveDown(0));
        Assert.False(state.MoveDown(0));
        Assert.NotNull(state.IsPlacedCorrectly(0));
        Assert.InRange(state.CorrectPlacementCount, 0, state.Total - 1); // the scramble is never already right
        Assert.False(state.IsFullyCorrect);
    }

    private static void Solve(OrderingBuilderState state)
    {
        for (int target = 0; target < state.Total; target++)
        {
            int at = state.CurrentOrder.ToList().FindIndex(i => i.Id == $"i{target}");
            while (at > target) { state.MoveUp(at); at--; }
        }
    }

    [Fact]
    public void Ordering_ArrangingTheCorrectSequenceIsFullyCorrect()
    {
        var state = new OrderingBuilderState(Ordering(5, seed: 9));
        Solve(state);
        state.Check();

        Assert.True(state.IsFullyCorrect);
        Assert.Equal(5, state.CorrectPlacementCount);
    }

    [Fact]
    public void Ordering_RetryKeepsTheArrangement_ResetRestoresTheScramble()
    {
        var state = new OrderingBuilderState(Ordering(4, seed: 2));
        string[] initial = [.. state.CurrentOrder.Select(i => i.Id)];
        state.MoveDown(0);
        string[] moved = [.. state.CurrentOrder.Select(i => i.Id)];
        state.Check();

        state.Retry();
        Assert.False(state.IsChecked);
        Assert.Equal(moved, state.CurrentOrder.Select(i => i.Id));
        Assert.True(state.MoveDown(1)); // unlocked again

        state.Check();
        state.Reset();
        Assert.False(state.IsChecked);
        Assert.Equal(initial, state.CurrentOrder.Select(i => i.Id));
    }

    [Fact]
    public void Ordering_TheSolutionCanOnlyBeRevealedAfterACheck()
    {
        var state = new OrderingBuilderState(Ordering());
        state.Check();

        Assert.True(state.RevealSolution());
        Assert.Equal(state.Activity.CorrectSequence, state.Solution);

        state.Retry();
        Assert.Null(state.Solution);
    }

    [Fact]
    public void Ordering_ValidationRequiresTwoOrMoreUniqueItems()
    {
        var bad = new OrderingActivity("T", "I", [new("x", "Only")]);
        var dup = new OrderingActivity("T", "I", [new("x", "A"), new("x", "B")]);

        Assert.Contains(bad.Validate(), i => i.Contains("At least two"));
        Assert.Contains(dup.Validate(), i => i.Contains("unique"));
        Assert.Throws<ArgumentException>(() => new OrderingBuilderState(bad));
    }

    // ---------------------------------------------------------------- C. committed predict -> reveal

    [Fact]
    public void Predict_NothingIsExposedBeforeTheLearnerCommits()
    {
        var state = new PredictRevealState(Predict());
        state.Select("wrong");

        Assert.False(state.IsCommitted);
        Assert.Null(state.Chosen);
        Assert.Null(state.Actual);
        Assert.Null(state.MatchesActual);
        Assert.Null(state.Explanation);
    }

    [Fact]
    public void Predict_CannotCommitWithoutAPrediction_AndAfterCommitTheChoiceIsLocked()
    {
        var state = new PredictRevealState(Predict());

        Assert.False(state.CanCommit);
        Assert.False(state.Commit());

        state.Select("wrong");
        Assert.True(state.Commit());
        Assert.False(state.Select("right"));
        Assert.Equal("wrong", state.Chosen!.Id);
    }

    [Fact]
    public void Predict_RevealComparesThePredictionWithTheActualOutcome()
    {
        var wrong = new PredictRevealState(Predict());
        wrong.Select("wrong"); wrong.Commit();
        var right = new PredictRevealState(Predict());
        right.Select("right"); right.Commit();

        Assert.False(wrong.MatchesActual);
        Assert.Equal("right", wrong.Actual!.Id);
        Assert.True(right.MatchesActual);
        Assert.Equal("The explanation.", wrong.Explanation);
    }

    [Fact]
    public void Predict_WithoutASingleRightAnswer_OnlyComparesAndNeverJudges()
    {
        var state = new PredictRevealState(Predict(actual: null));
        state.Select("wrong"); state.Commit();

        Assert.Null(state.Actual);
        Assert.Null(state.MatchesActual);
        Assert.NotNull(state.Chosen);
    }

    [Fact]
    public void Predict_RetryResetsToUnansweredOnlyWhereTheActivityAllowsIt()
    {
        var allowed = new PredictRevealState(Predict());
        allowed.Select("right"); allowed.Commit();
        Assert.True(allowed.Retry());
        Assert.Null(allowed.Selection);
        Assert.False(allowed.IsCommitted);

        var refused = new PredictRevealState(Predict(allowRetry: false));
        refused.Select("right"); refused.Commit();
        Assert.False(refused.Retry());
        Assert.True(refused.IsCommitted);
    }

    [Fact]
    public void Predict_ValidationRejectsAnUnknownActualOption()
    {
        var bad = Predict(actual: "ghost");

        Assert.Contains(bad.Validate(), i => i.Contains("not one of the options"));
        Assert.Throws<ArgumentException>(() => new PredictRevealState(bad));
    }

    // ---------------------------------------------------------------- safety modes

    [Fact]
    public void SafetyModes_EverySafetyLevelMapsToOneMode()
    {
        Assert.Equal(ActiveLearningSafetyMode.NormalLearning, ActiveLearningSafety.ModeFor(CurriculumSafetyLevel.PublicCore));
        Assert.Equal(ActiveLearningSafetyMode.NormalLearning, ActiveLearningSafety.ModeFor(CurriculumSafetyLevel.PublicWithAdaptation));
        Assert.Equal(ActiveLearningSafetyMode.AcademicThirdPerson, ActiveLearningSafety.ModeFor(CurriculumSafetyLevel.AcademicContextOnly));
        Assert.Equal(ActiveLearningSafetyMode.ProfessionalContext, ActiveLearningSafety.ModeFor(CurriculumSafetyLevel.ProfessionalReviewRequired));
        Assert.Equal(ActiveLearningSafetyMode.NoSelfGuidedSimulation, ActiveLearningSafety.ModeFor(CurriculumSafetyLevel.NotEligibleForSelfGuidedSimulator));

        foreach (CurriculumSafetyLevel level in Enum.GetValues<CurriculumSafetyLevel>())
        {
            _ = ActiveLearningSafety.ModeFor(level); // throws if a new level is added without a mode
        }
    }

    [Fact]
    public void SafetyModes_TheCourseCatalogWeeks_MapToTheirIntendedModes()
    {
        // Derived from the catalog, never hand-picked per week.
        Assert.Equal(ActiveLearningSafetyMode.AcademicThirdPerson, ActiveLearningSafety.ModeFor(CourseCatalog.Weeks.Single(w => w.Number == 4).SafetyLevel));
        Assert.Equal(ActiveLearningSafetyMode.ProfessionalContext, ActiveLearningSafety.ModeFor(CourseCatalog.Weeks.Single(w => w.Number == 11).SafetyLevel));
        Assert.Equal(ActiveLearningSafetyMode.NoSelfGuidedSimulation, ActiveLearningSafety.ModeFor(CourseCatalog.Weeks.Single(w => w.Number == 13).SafetyLevel));
    }

    [Fact]
    public void SafetyModes_EveryRestrictedModeHasAMandatoryNotice_AndOnlyTheStrictestWithholdsBranching()
    {
        foreach (ActiveLearningSafetyMode mode in Enum.GetValues<ActiveLearningSafetyMode>())
        {
            SafetyModeProfile profile = ActiveLearningSafety.Profile(mode);

            Assert.Equal(mode, profile.Mode);
            Assert.Equal(mode != ActiveLearningSafetyMode.NormalLearning, profile.DefaultNotice.Length > 0);
            Assert.Equal(mode != ActiveLearningSafetyMode.NoSelfGuidedSimulation, profile.AllowsBranchingScenarios);
        }
    }

    [Theory]
    [InlineData(ActiveLearningSafetyMode.AcademicThirdPerson)]
    [InlineData(ActiveLearningSafetyMode.ProfessionalContext)]
    [InlineData(ActiveLearningSafetyMode.NoSelfGuidedSimulation)]
    public void SafetyModes_ABlankOverrideCanNeverRemoveTheNotice(ActiveLearningSafetyMode mode)
    {
        string defaultNotice = ActiveLearningSafety.Profile(mode).DefaultNotice;

        Assert.Equal(defaultNotice, ActiveLearningSafety.ResolveNotice(mode, null));
        Assert.Equal(defaultNotice, ActiveLearningSafety.ResolveNotice(mode, ""));
        Assert.Equal(defaultNotice, ActiveLearningSafety.ResolveNotice(mode, "   "));
        Assert.Equal("Approved", ActiveLearningSafety.ResolveNotice(mode, "  Approved  "));
    }

    [Fact]
    public void SafetyModes_NormalLearning_HasNoNoticeUnlessTheWeekSuppliesOne()
    {
        Assert.Equal("", ActiveLearningSafety.ResolveNotice(ActiveLearningSafetyMode.NormalLearning, null));
        Assert.Equal("Custom", ActiveLearningSafety.ResolveNotice(ActiveLearningSafetyMode.NormalLearning, "Custom"));
    }

    // ---------------------------------------------------------------- integration with the Active Learning Standard

    [Fact]
    public void ToolkitEngines_QualifyAsInteractionAndResponse_WithinTheirOwnFamiliesOnly()
    {
        WeekLearningArchitecture Week(InteractionDeclaration interaction, LearnerResponseDeclaration response, bool addSimulator = false) =>
            new(99, StructuralStatus.StructuralEnrichmentRequired,
                [
                    new(VisualModelKind.Sequence, "guided-practice-sequence"),
                    new(VisualModelKind.MindMap, "ComponentId=\"week99-mindmap-preview\"")
                ],
                addSimulator ? [new(InteractionFamily.Simulator, "StatefulModelSimulator"), interaction] : [interaction], [response], true);

        Assert.Empty(ActiveLearningStandard.Evaluate(Week(
            new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck"), new(LearnerResponseKind.Classify, "ClassifyMatchCheck"), addSimulator: true)));
        Assert.Empty(ActiveLearningStandard.Evaluate(Week(
            new(InteractionFamily.OrderingBuilder, "OrderingBuilder"), new(LearnerResponseKind.Order, "OrderingBuilder"), addSimulator: true)));
        Assert.Empty(ActiveLearningStandard.Evaluate(Week(
            new(InteractionFamily.PredictCommit, "PredictReveal"), new(LearnerResponseKind.Predict, "PredictReveal"), addSimulator: true)));
        Assert.Empty(ActiveLearningStandard.Evaluate(Week(
            new(InteractionFamily.Simulator, "CaseExaminationSimulator"), new(LearnerResponseKind.ManipulateModel, "CaseExaminationSimulator"))));

        // a classify engine does not order, and an ordering engine does not predict
        Assert.NotEmpty(ActiveLearningStandard.Evaluate(Week(
            new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck"), new(LearnerResponseKind.Order, "ClassifyMatchCheck"))));
        Assert.NotEmpty(ActiveLearningStandard.Evaluate(Week(
            new(InteractionFamily.OrderingBuilder, "OrderingBuilder"), new(LearnerResponseKind.Predict, "OrderingBuilder"))));
        Assert.NotEmpty(ActiveLearningStandard.Evaluate(Week(
            new(InteractionFamily.OrderingBuilder, "CaseExaminationSimulator"), new(LearnerResponseKind.ManipulateModel, "CaseExaminationSimulator"))));

        Assert.Contains(ActiveLearningStandard.Evaluate(Week(
            new(InteractionFamily.ClassifyMatch, "ClassifyMatchCheck"), new(LearnerResponseKind.Classify, "ClassifyMatchCheck"))),
            f => f.Gate == ActiveLearningGate.SimulatorInteractiveModelGate);
    }

    [Fact]
    public void TheFrame_IsChrome_NotAnInteraction()
    {
        Assert.False(ActiveLearningStandard.Components["ActiveLearningFrame"].QualifiesAsInteraction);
        Assert.False(ActiveLearningStandard.Components["ActiveLearningFrame"].QualifiesAsResponse);
    }

    // ---------------------------------------------------------------- D. case examination model (stateful)

    private static CaseExaminationActivity Case() => new(
        "T", "I",
        [new("Fact", "A neutral fact."), new("Other", "Another neutral fact.")],
        [
            new("t1", "Tool One", "Question one?", [new("a", "Surface A"), new("b", "Surface B")], "a", "SECRET-FINDING-1"),
            new("t2", "Tool Two", "Question two?", [new("a", "Surface A"), new("b", "Surface B")], "b", "SECRET-FINDING-2", "SECRET-TOOL-SOURCE")
        ],
        OutcomeTitle: "Closing state",
        Outcome: "SECRET-OUTCOME",
        SourceRef: "SECRET-CASE-SOURCE");

    [Fact]
    public void CaseExamination_StartsWithAnUntouchedModel_AndNoOutcome()
    {
        CaseExaminationState state = new(Case());

        Assert.Equal(0, state.AppliedCount);
        Assert.Equal(2, state.Total);
        Assert.False(state.IsComplete);
        Assert.Null(state.Outcome);
        Assert.Null(state.ActiveToolId);
        Assert.Empty(state.Findings);
        Assert.False(state.CanCommit);
        Assert.All(state.Activity.Tools, t => Assert.Null(state.WasCorrect(t.Id)));
    }

    [Fact]
    public void CaseExamination_ATool_MustBeOpenedAndPredictedBeforeItCanBeApplied()
    {
        CaseExaminationState state = new(Case());

        Assert.False(state.Select("a"));            // nothing is open yet
        Assert.False(state.Commit());
        Assert.True(state.Open("t1"));
        Assert.False(state.CanCommit);              // opened, but no prediction yet
        Assert.False(state.Commit());
        Assert.False(state.Select("nonsense"));
        Assert.True(state.Select("a"));
        Assert.True(state.CanCommit);

        Assert.Null(state.WasCorrect("t1"));        // still no verdict before the commit
        Assert.Empty(state.Findings);

        Assert.True(state.Commit());
        Assert.True(state.WasCorrect("t1"));
        Assert.Equal(["t1"], state.Findings.Select(f => f.Id));
        Assert.Null(state.ActiveToolId);
    }

    [Fact]
    public void CaseExamination_AWrongPrediction_StillAppliesTheToolAndRevealsTheRealFinding()
    {
        CaseExaminationState state = new(Case());

        state.Open("t1");
        state.Select("b");                          // wrong: the correct option is "a"
        state.Commit();

        Assert.False(state.WasCorrect("t1"));
        Assert.Equal(0, state.CorrectPredictionCount);
        Assert.Equal("SECRET-FINDING-1", state.Findings.Single().Finding);   // the model still advances
        Assert.True(state.IsApplied("t1"));
    }

    [Fact]
    public void CaseExamination_TheOutcomeExistsOnlyAfterEveryToolHasBeenApplied()
    {
        CaseExaminationState state = new(Case());

        state.Open("t1");
        state.Select("a");
        state.Commit();
        Assert.Null(state.Outcome);                 // one tool left
        Assert.False(state.IsComplete);

        state.Open("t2");
        state.Select("b");
        state.Commit();

        Assert.True(state.IsComplete);
        Assert.Equal("SECRET-OUTCOME", state.Outcome);
        Assert.Equal(2, state.CorrectPredictionCount);
    }

    [Fact]
    public void CaseExamination_AnAppliedToolCannotBeReopened_AndClosingLeavesTheModelUnchanged()
    {
        CaseExaminationState state = new(Case());

        state.Open("t1");
        state.Select("a");
        state.Commit();
        Assert.False(state.Open("t1"));

        state.Open("t2");
        state.Select("a");
        state.Close();

        Assert.Null(state.ActiveToolId);
        Assert.Null(state.PendingSelection);
        Assert.False(state.IsApplied("t2"));
        Assert.Single(state.Findings);
    }

    [Fact]
    public void CaseExamination_ResetReturnsTheModelToItsStartingState()
    {
        CaseExaminationState state = new(Case());

        state.Open("t1");
        state.Select("a");
        state.Commit();
        state.Reset();

        Assert.Equal(0, state.AppliedCount);
        Assert.Empty(state.Findings);
        Assert.Null(state.Outcome);
        Assert.Null(state.WasCorrect("t1"));
        Assert.False(state.IsApplied("t1"));
    }

    [Theory]
    [InlineData("blank-title")]
    [InlineData("one-tool")]
    [InlineData("unknown-correct-option")]
    [InlineData("blank-finding")]
    [InlineData("blank-outcome")]
    public void CaseExamination_RefusesMalformedData(string flaw)
    {
        CaseExaminationActivity valid = Case();
        CaseExaminationActivity broken = flaw switch
        {
            "blank-title" => valid with { Title = "  " },
            "one-tool" => valid with { Tools = [valid.Tools[0]] },
            "unknown-correct-option" => valid with { Tools = [valid.Tools[0] with { CorrectOptionId = "zzz" }, valid.Tools[1]] },
            "blank-finding" => valid with { Tools = [valid.Tools[0] with { Finding = " " }, valid.Tools[1]] },
            "blank-outcome" => valid with { Outcome = "" },
            _ => throw new ArgumentOutOfRangeException(nameof(flaw))
        };

        Assert.NotEmpty(broken.Validate());
        Assert.Throws<ArgumentException>(() => new CaseExaminationState(broken));
    }
}
