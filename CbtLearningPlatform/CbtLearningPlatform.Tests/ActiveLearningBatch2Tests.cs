using System.Reflection;
using System.Text.RegularExpressions;
using CbtLearningPlatform.Client.Components.Pages;
using CbtLearningPlatform.Client.Curriculum;
using CbtLearningPlatform.Client.Interactive;

namespace CbtLearningPlatform.Tests;

/// <summary>ACTIVE LEARNING ENRICHMENT — Phase 2, Batch 2 (Weeks 5, 7 and 9). Proves, on the REAL page data (read from the page
/// classes by reflection, rendered by the real engines), that:
///  - every learner-facing string in an activity is drawn from the owner-approved page text (source-grounded);
///  - no feedback, outcome or answer exists in the DOM before the learner commits, and every activity is solvable to the approved key;
///  - the safety mode is derived from the week, never hand-picked;
///  - the catalog declarations pass the Active Learning Gate, and each interaction precedes the Final Assessment;
///  - each Weekly Mind Map is a valid single-parent hierarchy rendered as Preview + Review from ONE model;
///  - the approved prose is untouched: the baseline snapshot (Golden/ApprovedProse) must remain, in order, inside the current page —
///    only insertions and the named replacements below are allowed.</summary>
public sealed class ActiveLearningBatch2Tests
{
    private static readonly Type Week5Page = typeof(Sedmica5);
    private static readonly Type Week7Page = typeof(Sedmica7);
    private static readonly Type Week9Page = typeof(Sedmica9);

    private static T Field<T>(Type page, string name) =>
        (T)(page.GetField(name, BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException($"{page.Name}.{name} not found"))
            .GetValue(null)!;

    private static MindMapModel MindMap(Type page, string builder) =>
        (MindMapModel)(page.GetMethod(builder, BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException($"{page.Name}.{builder} not found")).Invoke(null, null)!;

    private static ClassifyMatchActivity Week5Stage => Field<ClassifyMatchActivity>(Week5Page, "_week5CollaborationStage");
    private static PredictRevealActivity Week7Friends => Field<PredictRevealActivity>(Week7Page, "_week7PredictFriends");
    private static PredictRevealActivity Week7Run => Field<PredictRevealActivity>(Week7Page, "_week7PredictRun");
    private static OrderingActivity Week7Cycle => Field<OrderingActivity>(Week7Page, "_week7CycleOrder");
    private static ClassifyMatchActivity Week9Match => Field<ClassifyMatchActivity>(Week9Page, "_week9DistortionMatch");

    public static IEnumerable<object[]> BatchWeeks => [[5], [7], [9]];

    private static bool Approved(int week, string text, StringComparison comparison = StringComparison.Ordinal) =>
        ApprovedProse.NormalizedBaseline(week).Contains(ApprovedProse.Normalize(text), comparison);

    // ---------------------------------------------------------------- data is well-formed

    [Fact]
    public void EveryBatch2Activity_IsWellFormed()
    {
        Assert.Empty(Week5Stage.Validate());
        Assert.Empty(Week7Friends.Validate());
        Assert.Empty(Week7Run.Validate());
        Assert.Empty(Week7Cycle.Validate());
        Assert.Empty(Week9Match.Validate());
    }

    // ---------------------------------------------------------------- Week 5: source-grounded

    [Fact]
    public void Week5_StatementsAreThePhrasesOfTheApprovedTwoStageComparison_AndFeedbackIsItsOwnText()
    {
        ClassifyMatchActivity activity = Week5Stage;
        string page = ApprovedProse.CurrentPage(5);

        Assert.Equal(["В началото на лечението", "По-нататък в лечението", "И в двата етапа"], activity.Options.Select(o => o.Label));
        Assert.Contains("В началото на лечението", ApprovedProse.NormalizedBaseline(5));
        Assert.Contains("По-нататък в лечението", ApprovedProse.NormalizedBaseline(5));
        Assert.True(Approved(5, "и в двата етапа", StringComparison.OrdinalIgnoreCase));

        const string earlyLabel = "В началото на лечението: ";
        const string laterLabel = "По-нататък в лечението: ";

        foreach (ClassifyMatchItem item in activity.Items)
        {
            string predicate = item.CorrectOptionId switch
            {
                "early" => StripActor(item.Prompt, "Терапевтът"),
                "later" => StripActor(item.Prompt, "Пациентът"),
                _ => item.Prompt
            };
            string body = item.CorrectOptionId switch
            {
                "early" => StripPrefix(item.Explanation, earlyLabel),
                "later" => StripPrefix(item.Explanation, laterLabel),
                _ => item.Explanation
            };

            Assert.True(Approved(5, predicate, StringComparison.OrdinalIgnoreCase), $"'{predicate}' is not a phrase of the approved comparison.");
            Assert.True(Approved(5, body), $"Feedback for '{item.Id}' is not approved text.");
            Assert.Contains(predicate, body, StringComparison.OrdinalIgnoreCase);   // the phrase lives in the very paragraph that names its stage
            Assert.Contains("5.5 · Сътрудничество: как се променя балансът", page);
            Assert.Equal("5.5 — Сътрудничество: как се променя балансът", item.SourceRef);
        }

        // The same skill (summarising) moves from therapist to patient — the shift itself, not just an actor pattern.
        Assert.Contains(activity.Items, i => i.Prompt == "Терапевтът обобщава наученото" && i.CorrectOptionId == "early");
        Assert.Contains(activity.Items, i => i.Prompt == "Пациентът обобщава важните моменти" && i.CorrectOptionId == "later");
        Assert.Equal(["both", "early", "later"], activity.Items.Select(i => i.CorrectOptionId).Distinct().Order());
    }

    // ---------------------------------------------------------------- Week 7: source-grounded

    [Fact]
    public void Week7_PredictionsUseTheApprovedScenariosOutcomesAndConclusions_Verbatim()
    {
        foreach ((PredictRevealActivity activity, string actual) in new[] { (Week7Friends, "higher"), (Week7Run, "lower") })
        {
            Assert.True(Approved(7, activity.Title), activity.Title);
            Assert.True(Approved(7, activity.Scenario), activity.Scenario);
            Assert.True(Approved(7, activity.Explanation), "Explanation must be one contiguous run of approved text.");
            Assert.Equal(actual, activity.ActualOptionId);
        }

        // Options describe a direction only; the two directions the page names are its own words, "same" is a neutral distractor.
        Assert.True(Approved(7, "по-високи от предсказаните", StringComparison.OrdinalIgnoreCase));
        Assert.True(Approved(7, "по-ниски от предсказаните", StringComparison.OrdinalIgnoreCase));
        Assert.Equal(["higher", "same", "lower"], Week7Friends.Options.Select(o => o.Id));
        Assert.All(new[] { Week7Friends, Week7Run }, a => Assert.NotEqual("same", a.ActualOptionId));

        // Opposite real outcomes, so one constant guess can never score twice.
        Assert.NotEqual(Week7Friends.ActualOptionId, Week7Run.ActualOptionId);

        // The real numbers the approved page reports.
        Assert.Contains("0 и 3", Week7Friends.Scenario);
        Assert.Contains("Действителни оценки: 3 до 5", Week7Friends.Explanation);
        Assert.Contains("по 4 за овладяване", Week7Run.Scenario);
        Assert.Contains("Действителни оценки: по 1", Week7Run.Explanation);
    }

    [Fact]
    public void Week7_TheClosingSentenceThatStatesBothDirections_LivesOnlyAfterTheSecondPredictionIsCommitted()
    {
        const string synthesis = "Двата сценария показват двете посоки";

        Assert.DoesNotContain(synthesis, WeekPageMarkup.Read(7));
        Assert.DoesNotContain(synthesis, Week7Friends.Explanation);
        Assert.Contains(synthesis, Week7Run.Explanation);
    }

    [Fact]
    public void Week7_OrderingIsTheApprovedCycle_InTheApprovedOrder_WithThePagesOwnSentenceAsFeedback()
    {
        OrderingActivity activity = Week7Cycle;
        string baseline = ApprovedProse.Baseline(7);

        Assert.Equal(["Бездействие", "По-ниско настроение", "По-негативно мислене", "Още по-силно бездействие"], activity.CorrectSequence.Select(i => i.Text));

        // The order the old reveal listed, and the order of the cycle drawn in 7.2.
        int last = -1;
        foreach (OrderingItem item in activity.CorrectSequence)
        {
            int index = baseline.IndexOf($"<li>{item.Text}</li>", StringComparison.Ordinal);
            Assert.True(index > last, $"'{item.Text}' must follow the previous step in the approved reveal.");
            last = index;
        }

        Assert.StartsWith("Подредете тези четири стъпки от порочния кръг на бездействието в правилния ред", activity.Instruction);
        Assert.True(Approved(7, "Подредете тези четири стъпки от порочния кръг на бездействието в правилния ред"));
        Assert.True(Approved(7, activity.Feedback!, StringComparison.OrdinalIgnoreCase));
    }

    // ---------------------------------------------------------------- Week 9: source-grounded

    [Fact]
    public void Week9_EveryExampleIsA93CardVerbatim_TheOptionIsThatCardsNameAndTheFeedbackItsDefinition()
    {
        string baseline = ApprovedProse.Baseline(9);
        string region = baseline[baseline.IndexOf("id=\"izkrivyavaniya\"", StringComparison.Ordinal)..baseline.IndexOf("id=\"koga-verni\"", StringComparison.Ordinal)];

        List<(string Quote, string Name, string Definition)> cards =
        [
            .. Regex.Matches(region, "<summary>(?<q>.*?)</summary>\\s*<div class=\"progressive-explanation__body\"><p><strong>(?<n>.*?)</strong>(?<rest>.*?)</p></div>", RegexOptions.Singleline)
                .Select(m => (ApprovedProse.Normalize(m.Groups["q"].Value),
                              ApprovedProse.Normalize(m.Groups["n"].Value),
                              ApprovedProse.Normalize(m.Groups["n"].Value + m.Groups["rest"].Value)))
        ];
        Assert.Equal(12, cards.Count);

        ClassifyMatchActivity activity = Week9Match;
        Assert.Equal(6, activity.Items.Count);
        foreach (ClassifyMatchItem item in activity.Items)
        {
            var card = cards.Single(c => c.Quote == item.Prompt);
            Assert.Equal(card.Name, activity.Options.Single(o => o.Id == item.CorrectOptionId).Label);
            Assert.Equal(card.Definition, item.Explanation);
            Assert.Equal("9.3 — Фигура 11.2", item.SourceRef);
        }

        // The near-neighbours (easily confused with one another) stay in 9.3 for reading and are not offered as options.
        string[] excluded = ["Етикетиране", "Увеличаване/намаляване", "Ментален филтър", "Преувеличение", "Тунелно виждане", "Дисквалифициране или отхвърляне на позитивното"];
        Assert.DoesNotContain(activity.Options, o => excluded.Contains(o.Label));
        Assert.Equal(ClassifyMatchMode.Match, activity.Mode);
    }

    [Fact]
    public void Week9_ItemsAreListedOutOfTheOptionsOrder_SoTheDiagonalIsNotTheAnswer()
    {
        ClassifyMatchActivity activity = Week9Match;
        string[] optionOrder = [.. activity.Options.Select(o => o.Id)];

        for (int i = 0; i < activity.Items.Count; i++)
        {
            Assert.NotEqual(optionOrder[i], activity.Items[i].CorrectOptionId);
        }
    }

    // ---------------------------------------------------------------- nothing exists before the learner commits

    [Fact]
    public void ClassifyActivities_ExposeNoFeedbackAndCannotBeCheckedBeforeCommitment()
    {
        foreach ((ClassifyMatchActivity activity, int week, string id) in new[] { (Week5Stage, 5, "week5-collaboration-stage"), (Week9Match, 9, "week9-distortion-match") })
        {
            string html = Render<ClassifyMatchCheck>(activity, week, id);

            Assert.Contains(activity.Items[0].Prompt, html);
            Assert.Contains("0 от " + activity.Items.Count + " отговорени", html);
            Assert.Matches(@"<button[^>]*disabled[^>]*>Провери</button>", html);
            foreach (ClassifyMatchItem item in activity.Items)
            {
                Assert.DoesNotContain(item.Explanation, html);
                Assert.DoesNotContain(item.SourceRef!, html);
            }
            Assert.DoesNotContain("active-learning__explanation", html);
            Assert.DoesNotContain("✓", html);
            Assert.DoesNotContain("✗", html);
        }
    }

    [Fact]
    public void PredictActivities_ExposeNoExplanationNoOutcomeAndNoVerdictBeforeTheCommit()
    {
        foreach ((PredictRevealActivity activity, string id) in new[] { (Week7Friends, "week7-predict-friends"), (Week7Run, "week7-predict-run") })
        {
            string html = Render<PredictReveal>(activity, 7, id);

            Assert.Contains(activity.Scenario, html);
            Assert.Matches(@"<button[^>]*disabled[^>]*>Потвърди предсказанието</button>", html);
            Assert.DoesNotContain(activity.Explanation, html);
            Assert.DoesNotContain("Действителни оценки", html);
            Assert.DoesNotContain("Действителният резултат", html);
            Assert.DoesNotContain("Вашето предсказание", html);
            Assert.DoesNotContain("Съвпада", html);
            Assert.DoesNotContain("active-learning__reveal", html);
            Assert.DoesNotContain("Двата сценария показват", html);
        }
    }

    [Fact]
    public void PredictActivities_RevealOnlyAfterCommit_AndAWrongPredictionStillGetsTheRealOutcome()
    {
        foreach (PredictRevealActivity activity in new[] { Week7Friends, Week7Run })
        {
            PredictRevealState state = new(activity);
            Assert.Null(state.Explanation);
            Assert.Null(state.Actual);
            Assert.False(state.Commit());

            string wrong = activity.Options.First(o => o.Id != activity.ActualOptionId).Id;
            state.Select(wrong);
            Assert.True(state.Commit());

            Assert.Equal(activity.Explanation, state.Explanation);
            Assert.Equal(activity.ActualOptionId, state.Actual!.Id);
            Assert.False(state.MatchesActual);

            Assert.True(state.Retry());
            state.Select(activity.ActualOptionId!);
            state.Commit();
            Assert.True(state.MatchesActual);
        }
    }

    [Fact]
    public void OrderingActivity_ExposesNoFeedbackNoVerdictAndNoSolutionBeforeTheCheck_StartsScrambled_AndSolves()
    {
        OrderingActivity activity = Week7Cycle;
        string html = Render<OrderingBuilder>(activity, 7, "week7-cycle-order");

        Assert.DoesNotContain(activity.Feedback!, html);
        Assert.DoesNotContain("active-learning__solution", html);
        Assert.DoesNotContain("На мястото си", html);
        Assert.Contains("Провери подредбата", html);
        Assert.Contains("Премести „", html);

        OrderingBuilderState state = new(activity);
        string[] correct = [.. activity.CorrectSequence.Select(i => i.Id)];
        Assert.NotEqual(correct, state.CurrentOrder.Select(i => i.Id));

        for (int target = 0; target < correct.Length; target++)
        {
            int at = state.CurrentOrder.ToList().FindIndex(i => i.Id == correct[target]);
            while (at > target) { Assert.True(state.MoveUp(at)); at--; }
        }
        Assert.Null(state.IsPlacedCorrectly(0));
        Assert.True(state.Check());
        Assert.True(state.IsFullyCorrect);
    }

    [Fact]
    public void ClassifyActivities_AreSolvableToTheApprovedKey_AndAWrongChoiceIsCaught()
    {
        foreach (ClassifyMatchActivity activity in new[] { Week5Stage, Week9Match })
        {
            ClassifyMatchState state = new(activity);
            Assert.False(state.CanCheck);
            Assert.All(activity.Items, i => Assert.Null(state.IsCorrect(i.Id)));

            foreach (ClassifyMatchItem item in activity.Items) state.Select(item.Id, item.CorrectOptionId);
            Assert.True(state.CanCheck);
            Assert.Null(state.IsCorrect(activity.Items[0].Id));
            Assert.True(state.Check());
            Assert.Equal(activity.Items.Count, state.CorrectCount);

            state.Reset();
            ClassifyMatchItem first = activity.Items[0];
            foreach (ClassifyMatchItem item in activity.Items) state.Select(item.Id, item.CorrectOptionId);
            state.Select(first.Id, activity.Options.First(o => o.Id != first.CorrectOptionId).Id);

            if (activity.Mode == ClassifyMatchMode.Match)
            {
                Assert.False(state.CanCheck);   // one-to-one: reusing an option blocks the check
                state.Select(first.Id, first.CorrectOptionId);
                continue;
            }
            state.Check();
            Assert.False(state.IsCorrect(first.Id));
            Assert.Equal(activity.Items.Count - 1, state.CorrectCount);
        }
    }

    // ---------------------------------------------------------------- page structure

    [Fact]
    public void Week5_TheClassificationSitsUnderTheTwoStageComparison_BeforeTheLocalCheck()
    {
        string markup = WeekPageMarkup.Read(5);

        int compare = markup.IndexOf("category-compare", StringComparison.Ordinal);
        int activity = markup.IndexOf("<ClassifyMatchCheck", StringComparison.Ordinal);
        int check6 = markup.IndexOf("<strong>Проверка 6.</strong>", StringComparison.Ordinal);

        Assert.True(compare >= 0 && compare < activity && activity < check6);
        Assert.Single(Regex.Matches(markup, "<ClassifyMatchCheck"));
    }

    [Fact]
    public void Week7_TheReplacedRevealsAreGone_TwoPredictionsPrecedeTheAssessment_AndTheOrderingFollowsItUndeclared()
    {
        string markup = WeekPageMarkup.Read(7);

        Assert.DoesNotContain("Разкрий действителния резултат", markup);
        Assert.DoesNotContain("Покажи верния ред", markup);
        Assert.DoesNotContain("разгънете всяка карта", markup);

        int assessment = markup.IndexOf("<FinalAssessment", StringComparison.Ordinal);
        Assert.Equal(2, Regex.Matches(markup, "<PredictReveal").Count);
        Assert.True(markup.LastIndexOf("<PredictReveal", StringComparison.Ordinal) < assessment);
        Assert.True(markup.IndexOf("<OrderingBuilder", StringComparison.Ordinal) > assessment);
        Assert.DoesNotContain(ActiveLearningCatalog.For(7).Interactions, i => i.Component == "OrderingBuilder");
        Assert.Contains("Виж пълната диаграма в 7.2 →", markup);
    }

    [Fact]
    public void Week9_TheTwelveReadingCardsAreUntouched_AndTheMatchingFollowsThemInTheApplicationSection()
    {
        string markup = WeekPageMarkup.Read(9);

        int cards = markup.IndexOf("id=\"izkrivyavaniya\"", StringComparison.Ordinal);
        int application = markup.IndexOf("id=\"case-lab\"", StringComparison.Ordinal);
        int activity = markup.IndexOf("<ClassifyMatchCheck", StringComparison.Ordinal);
        int summary = markup.IndexOf("id=\"proverki\"", StringComparison.Ordinal);

        Assert.True(cards < application && application < activity && activity < summary);
        Assert.Equal(12, Regex.Matches(markup[cards..markup.IndexOf("id=\"koga-verni\"", StringComparison.Ordinal)], "<details class=\"progressive-explanation\"><summary>").Count);

        // The fixed Thought Record demonstration and its safety boundary are exactly as approved.
        Assert.Contains("Разкрий пълния попълнен работен лист", markup);
        Assert.DoesNotContain("<input", markup);
        Assert.DoesNotContain("<textarea", markup);
    }

    // ---------------------------------------------------------------- Weekly Mind Map: Preview + Review from ONE model

    [Theory]
    [InlineData(5, "BuildWeek5MindMap", "_week5MindMapRender")]
    [InlineData(7, "BuildWeek7MindMap", "_week7MindMapRender")]
    [InlineData(9, "BuildWeek9MindMap", "_week9MindMapRender")]
    public void EveryBatch2Week_HasAValidSingleParentMindMap_RenderedAsPreviewAndReviewFromOneModel(int week, string builder, string render)
    {
        Type page = week switch { 5 => Week5Page, 7 => Week7Page, _ => Week9Page };
        MindMapModel model = MindMap(page, builder);

        Assert.Single(model.Nodes, n => n.ParentId is null);
        Assert.Equal(model.Nodes.Count, model.Nodes.Select(n => n.Id).Distinct(StringComparer.Ordinal).Count());
        Assert.All(model.Nodes.Where(n => n.ParentId is not null), n => Assert.Contains(model.Nodes, p => p.Id == n.ParentId));
        foreach (MindMapNode node in model.Nodes)   // every node reaches the root: no cycles
        {
            HashSet<string> seen = [];
            for (MindMapNode? at = node; at?.ParentId is not null; at = model.Nodes.Single(n => n.Id == at.ParentId))
            {
                Assert.True(seen.Add(at.Id), $"Cycle at {at.Id}");
            }
        }

        string markup = WeekPageMarkup.Read(week);
        Assert.Contains($"ComponentId=\"week{week}-mindmap-preview\" Model=\"@{render}\"", markup);
        Assert.Contains($"ComponentId=\"week{week}-mindmap-review\" Model=\"@{render}\"", markup);
        Assert.Contains(ActiveLearningCatalog.For(week).VisualModels, v => v.Kind == VisualModelKind.MindMap);
    }

    // ---------------------------------------------------------------- safety mode

    [Theory]
    [MemberData(nameof(BatchWeeks))]
    public void Batch2Weeks_ArePublicWithAdaptation_AndTheirActivitiesRunInNormalLearningMode_DerivedNotHandPicked(int week)
    {
        CourseWeekDefinition definition = CourseCatalog.Weeks.Single(w => w.Number == week);
        string source = ApprovedProse.CurrentPage(week);

        Assert.Equal(CurriculumSafetyLevel.PublicWithAdaptation, definition.SafetyLevel);
        Assert.Equal(ActiveLearningSafetyMode.NormalLearning, ActiveLearningSafety.ModeFor(definition.SafetyLevel));

        Assert.Contains("ActiveLearningSafety.ModeFor(_week.SafetyLevel)", source);
        MatchCollection tags = Regex.Matches(source, @"<(ClassifyMatchCheck|OrderingBuilder|PredictReveal)\b[^>]*/>");
        Assert.NotEmpty(tags);
        foreach (Match tag in tags)
        {
            Assert.Contains("SafetyMode=\"_safetyMode\"", tag.Value);
        }
        Assert.DoesNotContain("SafetyMode=\"ActiveLearningSafetyMode.", source);
    }

    // ---------------------------------------------------------------- the Active Learning Gate

    [Theory]
    [MemberData(nameof(BatchWeeks))]
    public void Batch2Weeks_PassTheActiveLearningGate_AndArePromotedToCompliant(int week)
    {
        WeekLearningArchitecture architecture = ActiveLearningCatalog.For(week);

        Assert.Equal(StructuralStatus.Compliant, architecture.Status);
        Assert.Empty(ActiveLearningStandard.Evaluate(architecture));
        Assert.Null(ActiveLearningStandard.CheckStatus(architecture));
    }

    [Fact]
    public void Batch2_DeclaresExactlyTheToolkitElementsThePagesContain_AndKeepsEachWeeksOwnVisualModels()
    {
        Assert.Contains(ActiveLearningCatalog.For(5).Interactions, i => i is { Family: InteractionFamily.ClassifyMatch, Component: "ClassifyMatchCheck" });
        Assert.Contains(ActiveLearningCatalog.For(5).LearnerResponses, r => r is { Response: LearnerResponseKind.Classify, Component: "ClassifyMatchCheck" });
        Assert.Contains(ActiveLearningCatalog.For(5).VisualModels, v => v is { Kind: VisualModelKind.Comparison, Marker: "category-compare" });
        Assert.Contains(ActiveLearningCatalog.For(5).VisualModels, v => v is { Kind: VisualModelKind.Sequence, Marker: "guided-practice-sequence" });

        Assert.Contains(ActiveLearningCatalog.For(7).Interactions, i => i is { Family: InteractionFamily.PredictCommit, Component: "PredictReveal" });
        Assert.Contains(ActiveLearningCatalog.For(7).LearnerResponses, r => r is { Response: LearnerResponseKind.Predict, Component: "PredictReveal" });
        Assert.Contains(ActiveLearningCatalog.For(7).VisualModels, v => v is { Kind: VisualModelKind.Cycle, Marker: "cascade-loop" });
        Assert.Contains(ActiveLearningCatalog.For(7).VisualModels, v => v is { Kind: VisualModelKind.ConceptNetwork });

        Assert.Contains(ActiveLearningCatalog.For(9).Interactions, i => i is { Family: InteractionFamily.ClassifyMatch, Component: "ClassifyMatchCheck" });
        Assert.Contains(ActiveLearningCatalog.For(9).LearnerResponses, r => r is { Response: LearnerResponseKind.Classify, Component: "ClassifyMatchCheck" });
        Assert.Contains(ActiveLearningCatalog.For(9).VisualModels, v => v is { Kind: VisualModelKind.Process, Marker: "ComponentId=\"week9-thought-record-structure\"" });
    }

    [Fact]
    public void WeeksOutsideBatches1And2_AreNotPromoted()
    {
        foreach (int week in new[] { 4, 11, 12, 13, 14, 15 })
        {
            Assert.Equal(StructuralStatus.StructuralEnrichmentRequired, ActiveLearningCatalog.For(week).Status);
        }
        foreach (int week in new[] { 1, 2, 3, 5, 6, 7, 8, 9, 10 })
        {
            Assert.Equal(StructuralStatus.Compliant, ActiveLearningCatalog.For(week).Status);
        }
    }

    // ---------------------------------------------------------------- the approved prose is preserved

    [Theory]
    [MemberData(nameof(BatchWeeks))]
    public void ApprovedProse_IsPreservedInOrder_OnlyInsertionsAndTheNamedReplacementsAreAllowed(int week)
    {
        string baseline = ApprovedProse.Baseline(week);

        if (week == 7)
        {
            // The only approved text that legitimately leaves the markup, each moved verbatim into the committed activities
            // (see the grounding tests above): (1) the instruction that told the learner to expand the old cards; (2) the two
            // reveal cards of 7.6 and the closing sentence that states both directions; (3) 7.11's retrieval reveal.
            baseline = ApprovedProse.RemoveBetween(baseline,
                "опитайте да предскажете резултата преди да разгънете всяка карта по-долу.", "</p>");
            baseline = ApprovedProse.RemoveBetween(baseline,
                "<strong>Сценарий 1: среща с приятели.</strong>", "</LearningSection>");
            baseline = ApprovedProse.RemoveBetween(baseline,
                "Подредете тези четири стъпки от", "<p><a href=\"/kurs/sedmica-7#porochen-krag\">Виж пълната диаграма в 7.2 →</a></p>");
        }

        string? missing = ApprovedProse.FirstMissingWord(baseline, ApprovedProse.CurrentPage(week));

        Assert.True(missing is null, $"Week {week}: approved prose changed or was removed near: {missing}");
    }

    // ---------------------------------------------------------------- helpers

    private static string StripActor(string prompt, string actor)
    {
        Assert.StartsWith(actor + " ", prompt);
        return prompt[(actor.Length + 1)..];
    }

    private static string StripPrefix(string text, string prefix)
    {
        Assert.StartsWith(prefix, text);
        return text[prefix.Length..];
    }

    private static string Render<TComponent>(object activity, int week, string id) where TComponent : Microsoft.AspNetCore.Components.IComponent
    {
        CurriculumSafetyLevel level = CourseCatalog.Weeks.Single(w => w.Number == week).SafetyLevel;
        return HtmlText.Decode(ComponentRender.Html<TComponent>(new Dictionary<string, object?>
        {
            ["Activity"] = activity,
            ["ComponentId"] = id,
            ["SafetyMode"] = ActiveLearningSafety.ModeFor(level)
        }));
    }
}
