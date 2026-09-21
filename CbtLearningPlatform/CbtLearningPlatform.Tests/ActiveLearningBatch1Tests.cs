using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using CbtLearningPlatform.Client.Components.Pages;
using CbtLearningPlatform.Client.Curriculum;
using CbtLearningPlatform.Client.Interactive;

namespace CbtLearningPlatform.Tests;

/// <summary>ACTIVE LEARNING ENRICHMENT — Phase 2, Batch 1 (Weeks 1, 2 and 10). Proves, on the REAL page data (read from the
/// page classes by reflection, rendered by the real engines), that:
///  - every learner-facing string in an activity is drawn from the owner-approved page text (source-grounded);
///  - no feedback exists in the DOM before the learner commits, and the activities are solvable to the approved answer;
///  - the safety mode is derived from the week, never hand-picked (all three weeks are PublicCore → NormalLearning);
///  - the catalog declarations pass the Active Learning Gate and the interaction precedes the Final Assessment;
///  - the approved prose itself is untouched: the baseline snapshot (Golden/ApprovedProse, taken at the approved commit
///    before this batch) must remain, in order, inside the current page — only insertions (and two named replacements
///    in Week 10) are allowed.</summary>
public sealed class ActiveLearningBatch1Tests
{
    private static readonly Type Week1Page = typeof(Sedmica1);
    private static readonly Type Week2Page = typeof(Sedmica2);
    private static readonly Type Week10Page = typeof(Sedmica10);

    private static T Field<T>(Type page, string name) =>
        (T)(page.GetField(name, BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException($"{page.Name}.{name} not found"))
            .GetValue(null)!;

    private static ClassifyMatchActivity Week2Attribution => Field<ClassifyMatchActivity>(Week2Page, "_week2SchoolAttribution");
    private static ClassifyMatchActivity Week10Types => Field<ClassifyMatchActivity>(Week10Page, "_week10QuestionTypes");
    private static OrderingActivity Week1Order => Field<OrderingActivity>(Week1Page, "_week1HistoryOrder");
    private static OrderingActivity Week10Order => Field<OrderingActivity>(Week10Page, "_week10CategoryOrder");

    public static IEnumerable<object[]> BatchWeeks => [[1], [2], [10]];

    // ---------------------------------------------------------------- data is well-formed

    [Fact]
    public void EveryBatch1Activity_IsWellFormed()
    {
        Assert.Empty(Week1Order.Validate());
        Assert.Empty(Week2Attribution.Validate());
        Assert.Empty(Week10Types.Validate());
        Assert.Empty(Week10Order.Validate());
    }

    // ---------------------------------------------------------------- source-grounded activity data

    [Fact]
    public void Week1_OrderingItemsAreTheFirstFiveApprovedTimelineMilestones_AndTheAmbiguousSixthIsLeftOut()
    {
        string approved = ApprovedProse.NormalizedBaseline(1);
        OrderingActivity activity = Week1Order;

        Assert.Equal(
            ["Психоаналитично начало", "Неочакван резултат", "Два потока на мислене", "1977 г.", "1979 г."],
            activity.CorrectSequence.Select(i => i.Text));

        // Title and description come from the approved milestone itself (verbatim), so the exercise can never drift from it.
        foreach (OrderingItem item in activity.CorrectSequence)
        {
            Assert.Contains(ApprovedProse.Normalize(item.Text), approved);
            Assert.Contains(ApprovedProse.Normalize(item.Explanation!), approved);
        }
        Assert.Contains(ApprovedProse.Normalize(activity.Feedback!), approved);

        // "Late 1970s" overlaps 1977 and 1979 in the source, so that milestone has no single defensible position.
        Assert.DoesNotContain(activity.CorrectSequence, i => i.Text.Contains("тревожността", StringComparison.Ordinal));
    }

    [Fact]
    public void Week2_EveryStatementIsATableCellVerbatim_AndFeedbackShowsThatSameRow()
    {
        string[][] rows = ApprovedProse.ComparisonRows(ApprovedProse.CurrentPage(2));
        string[][] baselineRows = ApprovedProse.ComparisonRows(ApprovedProse.Baseline(2));

        // The approved table itself was not edited to fit the exercise.
        Assert.Equal(4, rows.Length);
        Assert.Equal(baselineRows, rows);

        ClassifyMatchActivity activity = Week2Attribution;
        Assert.Equal(["beck", "ellis"], activity.Options.Select(o => o.Id));
        Assert.Equal(["Бек — когнитивна терапия", "Елис — REBT"], activity.Options.Select(o => o.Label));

        HashSet<(int Row, int Column)> used = [];
        foreach (ClassifyMatchItem item in activity.Items)
        {
            (int row, int column) = Locate(rows, item.Prompt);

            Assert.Equal(column == 0 ? "beck" : "ellis", item.CorrectOptionId);
            Assert.Equal($"В сравнението: Бек — „{rows[row][0]}“; Елис — „{rows[row][1]}“", item.Explanation);
            Assert.Equal($"Сравнението по-горе, ред {row + 1}", item.SourceRef);
            Assert.True(used.Add((row, column)), $"'{item.Prompt}' is used twice.");
        }

        // Rows 1–3, both sides. Row 4 ("change of thinking improves mood and behavior") is the idea the page itself says both
        // schools share (section 01), so it has no single correct school and must not be used.
        Assert.Equal(6, used.Count);
        Assert.DoesNotContain(used, u => u.Row == 3);
        Assert.Contains("обща по същество идея", ApprovedProse.NormalizedBaseline(2));
    }

    [Fact]
    public void Week10_ClassificationReusesTheApprovedExamplesLabelsAndReasons()
    {
        string approved = ApprovedProse.NormalizedBaseline(10);
        ClassifyMatchActivity activity = Week10Types;

        Assert.Equal(4, activity.Items.Count);
        foreach (ClassifyMatchOption option in activity.Options)
        {
            Assert.Contains(ApprovedProse.Normalize(option.Label), approved);
        }
        foreach (ClassifyMatchItem item in activity.Items)
        {
            Assert.Contains(ApprovedProse.Normalize(item.Prompt), approved);
            Assert.Contains(ApprovedProse.Normalize(item.Explanation), approved);
        }

        // The four approved reveals each named a different category; the key is exactly what they revealed.
        Assert.Equal(
            ["Насочващ въпрос", "Изследващ въпрос", "Съвет, представен като въпрос", "Въпрос за алтернативно обяснение"],
            activity.Items.Select(i => activity.Options.Single(o => o.Id == i.CorrectOptionId).Label));

        // The source pointers name sections that exist on the page.
        string page = ApprovedProse.CurrentPage(10);
        Assert.Contains("10.1 · Какво е сократическо изследване", page);
        Assert.Contains("10.2 · Шестте категории оценъчни въпроси", page);
    }

    [Fact]
    public void Week10_OrderingUsesTheApprovedCategoryLabels_InTheSourcesOwnOrder()
    {
        string baseline = ApprovedProse.Baseline(10);
        OrderingActivity activity = Week10Order;

        Assert.Equal(
            ["Доказателства", "Алтернативно обяснение", "Декатастрофизиране", "Дистанциране"],
            activity.CorrectSequence.Select(i => i.Text));

        // Same order as the six-category list in 10.2 (Figure 11.1) and as the old reveal's answer.
        int last = -1;
        foreach (OrderingItem item in activity.CorrectSequence)
        {
            int index = baseline.IndexOf($"<h3>{item.Text}</h3>", StringComparison.Ordinal);
            Assert.True(index > last, $"'{item.Text}' must follow the previous category in the approved 10.2 list.");
            last = index;
        }
    }

    // ---------------------------------------------------------------- feedback is withheld until the learner commits

    [Fact]
    public void ClassifyActivities_ExposeNoFeedbackAndCannotBeCheckedBeforeCommitment()
    {
        foreach ((ClassifyMatchActivity activity, string id) in new[] { (Week2Attribution, "week2-school-attribution"), (Week10Types, "week10-question-types") })
        {
            string html = Render<ClassifyMatchCheck>(activity, 1, id);

            Assert.Contains(activity.Items[0].Prompt, html);
            Assert.Contains(activity.Options[0].Label, html);
            Assert.Contains("0 от " + activity.Items.Count + " отговорени", html);
            Assert.Matches(@"<button[^>]*disabled[^>]*>Провери</button>|<button[^>]*>Провери</button>", html);
            Assert.Contains("disabled", html);

            foreach (ClassifyMatchItem item in activity.Items)
            {
                Assert.DoesNotContain(item.Explanation, html);
                if (item.SourceRef is not null) Assert.DoesNotContain(item.SourceRef, html);
            }
            Assert.DoesNotContain("active-learning__explanation", html);
            Assert.DoesNotContain("active-learning__status", html);
            Assert.DoesNotContain("✓", html);
            Assert.DoesNotContain("✗", html);
            Assert.DoesNotContain("верен отговор", html);
        }
    }

    [Fact]
    public void OrderingActivities_ExposeNoFeedbackNoVerdictAndNoSolutionBeforeTheCheck()
    {
        foreach ((OrderingActivity activity, string id) in new[] { (Week1Order, "week1-history-order"), (Week10Order, "week10-category-order") })
        {
            string html = Render<OrderingBuilder>(activity, 1, id);

            foreach (OrderingItem item in activity.CorrectSequence)
            {
                Assert.Contains(item.Text, html);
                if (item.Explanation is not null) Assert.DoesNotContain(item.Explanation, html);
            }
            if (activity.Feedback is not null) Assert.DoesNotContain(activity.Feedback, html);
            if (activity.SourceRef is not null) Assert.DoesNotContain(activity.SourceRef, html);
            Assert.DoesNotContain("active-learning__solution", html);
            Assert.DoesNotContain("active-learning__reveal", html);
            Assert.DoesNotContain("На мястото си", html);
            Assert.DoesNotContain("Не е на мястото си", html);
            Assert.Contains("Провери подредбата", html);
        }
    }

    [Fact]
    public void OrderingActivities_StartScrambled_HaveNonDragControls_AndSolveToTheApprovedOrder()
    {
        foreach ((OrderingActivity activity, string id) in new[] { (Week1Order, "week1-history-order"), (Week10Order, "week10-category-order") })
        {
            string html = Render<OrderingBuilder>(activity, 1, id);
            Assert.Contains("Премести „", html);

            OrderingBuilderState state = new(activity);
            string[] correct = [.. activity.CorrectSequence.Select(i => i.Id)];
            Assert.NotEqual(correct, state.CurrentOrder.Select(i => i.Id));

            // Selection-sort with the same Move-up buttons a learner uses.
            for (int target = 0; target < correct.Length; target++)
            {
                int at = state.CurrentOrder.ToList().FindIndex(i => i.Id == correct[target]);
                while (at > target) { Assert.True(state.MoveUp(at)); at--; }
            }

            Assert.Null(state.IsPlacedCorrectly(0));
            Assert.True(state.Check());
            Assert.True(state.IsFullyCorrect);
        }
    }

    [Fact]
    public void ClassifyActivities_AreSolvableToTheApprovedKey_AndAWrongChoiceIsCaught()
    {
        foreach (ClassifyMatchActivity activity in new[] { Week2Attribution, Week10Types })
        {
            ClassifyMatchState state = new(activity);
            Assert.False(state.CanCheck);
            Assert.All(activity.Items, i => Assert.Null(state.IsCorrect(i.Id)));

            foreach (ClassifyMatchItem item in activity.Items) state.Select(item.Id, item.CorrectOptionId);
            Assert.True(state.CanCheck);
            Assert.Null(state.IsCorrect(activity.Items[0].Id));   // still no verdict until the check

            Assert.True(state.Check());
            Assert.Equal(activity.Items.Count, state.CorrectCount);

            state.Reset();
            ClassifyMatchItem first = activity.Items[0];
            foreach (ClassifyMatchItem item in activity.Items) state.Select(item.Id, item.CorrectOptionId);
            state.Select(first.Id, activity.Options.First(o => o.Id != first.CorrectOptionId).Id);
            state.Check();
            Assert.False(state.IsCorrect(first.Id));
            Assert.Equal(activity.Items.Count - 1, state.CorrectCount);
        }
    }

    [Fact]
    public void ClassifyKeys_UseEveryCategory_SoNoSingleChoiceScoresFullMarks()
    {
        foreach (ClassifyMatchActivity activity in new[] { Week2Attribution, Week10Types })
        {
            string[] keys = [.. activity.Items.Select(i => i.CorrectOptionId).Distinct()];
            Assert.True(keys.Length >= 2, "A key that always names the same option would be guessable.");
        }
    }

    // ---------------------------------------------------------------- safety mode

    [Theory]
    [MemberData(nameof(BatchWeeks))]
    public void Batch1Weeks_ArePublicCore_AndTheirActivitiesRunInNormalLearningMode_DerivedNotHandPicked(int week)
    {
        CourseWeekDefinition definition = CourseCatalog.Weeks.Single(w => w.Number == week);
        string source = ApprovedProse.CurrentPage(week);

        Assert.Equal(CurriculumSafetyLevel.PublicCore, definition.SafetyLevel);
        Assert.Equal(ActiveLearningSafetyMode.NormalLearning, ActiveLearningSafety.ModeFor(definition.SafetyLevel));

        // The page derives the mode from its own safety level and hands exactly that to every toolkit engine.
        Assert.Contains("ActiveLearningSafety.ModeFor(_week.SafetyLevel)", source);
        MatchCollection tags = Regex.Matches(source, @"<(ClassifyMatchCheck|OrderingBuilder|PredictReveal)\b[^>]*/>");
        Assert.NotEmpty(tags);
        foreach (Match tag in tags)
        {
            Assert.Contains("SafetyMode=\"_safetyMode\"", tag.Value);
        }
        Assert.DoesNotContain("SafetyMode=\"ActiveLearningSafetyMode.", source);
    }

    [Fact]
    public void RenderedInNormalMode_TheActivitiesCarryNoRestrictedFramingNotice()
    {
        string classify = Render<ClassifyMatchCheck>(Week2Attribution, 2, "week2-school-attribution");
        string ordering = Render<OrderingBuilder>(Week1Order, 1, "week1-history-order");

        foreach (string html in new[] { classify, ordering })
        {
            Assert.Contains("data-safety-mode=\"NormalLearning\"", html);
            Assert.DoesNotContain("active-learning__notice", html);
        }
    }

    // ---------------------------------------------------------------- the Active Learning Gate

    [Theory]
    [MemberData(nameof(BatchWeeks))]
    public void Batch1Weeks_PassTheActiveLearningGate_AndArePromotedToCompliant(int week)
    {
        WeekLearningArchitecture architecture = ActiveLearningCatalog.For(week);

        Assert.Equal(StructuralStatus.Compliant, architecture.Status);
        Assert.Empty(ActiveLearningStandard.Evaluate(architecture));
        Assert.Null(ActiveLearningStandard.CheckStatus(architecture));
    }

    [Fact]
    public void Batch1_DeclaresExactlyTheToolkitElementsThePagesContain()
    {
        Assert.Contains(ActiveLearningCatalog.For(1).Interactions, i => i is { Family: InteractionFamily.OrderingBuilder, Component: "OrderingBuilder" });
        Assert.Contains(ActiveLearningCatalog.For(1).LearnerResponses, r => r is { Response: LearnerResponseKind.Order, Component: "OrderingBuilder" });
        Assert.Contains(ActiveLearningCatalog.For(1).VisualModels, v => v is { Kind: VisualModelKind.Timeline });

        Assert.Contains(ActiveLearningCatalog.For(2).Interactions, i => i is { Family: InteractionFamily.ClassifyMatch, Component: "ClassifyMatchCheck" });
        Assert.Contains(ActiveLearningCatalog.For(2).LearnerResponses, r => r is { Response: LearnerResponseKind.Classify, Component: "ClassifyMatchCheck" });
        Assert.Contains(ActiveLearningCatalog.For(2).VisualModels, v => v is { Kind: VisualModelKind.Process, Marker: "concept-map__flow" });

        Assert.Contains(ActiveLearningCatalog.For(10).Interactions, i => i is { Family: InteractionFamily.ClassifyMatch, Component: "ClassifyMatchCheck" });
        Assert.Contains(ActiveLearningCatalog.For(10).LearnerResponses, r => r is { Response: LearnerResponseKind.Classify, Component: "ClassifyMatchCheck" });
    }

    [Fact]
    public void Week2_TheProcessChainsAreRealMarkup_TwoChainsSideBySide_EachWithItsApprovedStartingPoint()
    {
        string source = ApprovedProse.CurrentPage(2);

        Assert.Equal(2, Regex.Matches(source, @"class=""card process-chain""").Count);
        Assert.Equal(2, Regex.Matches(source, @"<ol class=""concept-map__flow""").Count);
        Assert.Equal(2, Regex.Matches(source, "concept-map__node--highlight").Count);
        // Each chain's caption is the comparison table's own "starts from" row, verbatim.
        Assert.Contains("<p class=\"process-chain__start\">Тръгва от конкретната автоматична мисъл в дадена ситуация.</p>", source);
        Assert.Contains("<p class=\"process-chain__start\">Тръгва от общите вярвания (рационални/ирационални) зад реакцията към събитието.</p>", source);
        // Sits in the balanced two-column grid so it is side by side wherever there is room and stacked on phones.
        Assert.Contains("learning-grid learning-grid--balanced", source[source.IndexOf("Двете вериги едно до друго", StringComparison.Ordinal)..]);
    }

    [Fact]
    public void Week10_TheReplacedRevealsAreGone_AndTheRetrievalOrderingSitsAfterTheAssessmentUndeclared()
    {
        // Rendered markup only (Razor comments and the @code block, which cites the old reveals, are excluded).
        string source = WeekPageMarkup.Read(10);

        Assert.DoesNotContain("Покажи класификацията", source);
        Assert.DoesNotContain("Покажи верния ред", source);
        Assert.DoesNotContain("после разгънете обяснението", source);

        // 10.11's ordering follows the assessment, so it can never be what satisfies the gate: it is not declared.
        int assessment = source.IndexOf("<FinalAssessment", StringComparison.Ordinal);
        Assert.True(source.IndexOf("<OrderingBuilder", StringComparison.Ordinal) > assessment);
        Assert.DoesNotContain(ActiveLearningCatalog.For(10).Interactions, i => i.Component == "OrderingBuilder");
    }

    [Fact]
    public void WeeksOutsideBatch1_AreNotPromoted()
    {
        // Weeks 5, 7 and 9 were promoted by Phase 2, Batch 2 (ActiveLearningBatch2Tests).
        foreach (int week in new[] { 4, 11, 12, 13, 14, 15 })
        {
            Assert.Equal(StructuralStatus.StructuralEnrichmentRequired, ActiveLearningCatalog.For(week).Status);
        }
        foreach (int week in new[] { 3, 6, 8 })
        {
            Assert.Equal(StructuralStatus.Compliant, ActiveLearningCatalog.For(week).Status);
        }
    }

    // ---------------------------------------------------------------- the approved prose is preserved

    [Theory]
    [MemberData(nameof(BatchWeeks))]
    public void ApprovedProse_IsPreservedInOrder_OnlyInsertionsAreAllowed(int week)
    {
        string baseline = ApprovedProse.Baseline(week);

        if (week == 10)
        {
            // The only approved text that legitimately leaves the markup: the four reveal cards of 10.4 (and their instruction,
            // which described the old reveal) and the 10.11 retrieval reveal — replaced by the committed activities, whose data
            // reuses that wording (see the grounding tests above).
            baseline = ApprovedProse.RemoveBetween(baseline,
                "<p>Изберете как бихте класифицирали всеки пример, после разгънете обяснението.</p>", "</LearningSection>");
            baseline = ApprovedProse.RemoveBetween(baseline,
                "Подредете тези четири от шестте категории", "<p><a href=\"/kurs/sedmica-10#kategorii\">Виж пълния списък в 10.2 →</a></p>");
        }

        string? missing = ApprovedProse.FirstMissingWord(baseline, ApprovedProse.CurrentPage(week));

        Assert.True(missing is null, $"Week {week}: approved prose changed or was removed near: {missing}");
    }

    [Fact]
    public void TheProseGuard_ActuallyDetectsRemovedAndRewrittenText()
    {
        string baseline = ApprovedProse.Baseline(1);

        Assert.Null(ApprovedProse.FirstMissingWord(baseline, baseline + " extra inserted words"));

        string removed = baseline.Replace("Когнитивната терапия се ражда от научна проверка, а не от предварително готова теория.", "");
        string rewritten = baseline.Replace("научна проверка", "клинично впечатление");

        Assert.NotEqual(baseline, removed);
        Assert.NotNull(ApprovedProse.FirstMissingWord(baseline, removed));
        Assert.NotEqual(baseline, rewritten);
        Assert.NotNull(ApprovedProse.FirstMissingWord(baseline, rewritten));
    }

    // ---------------------------------------------------------------- helpers

    private static (int Row, int Column) Locate(string[][] rows, string prompt)
    {
        string wanted = ApprovedProse.Normalize(prompt);
        for (int row = 0; row < rows.Length; row++)
        {
            for (int column = 0; column < 2; column++)
            {
                if (rows[row][column] == wanted) return (row, column);
            }
        }
        throw new InvalidOperationException($"'{prompt}' is not a cell of the approved comparison table.");
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

/// <summary>The approved-prose baseline (a snapshot of each page taken at the approved commit, before Batch 1) and the helpers
/// that compare against it. Text is normalised the way a learner would read it: Razor comments dropped, C# string escapes
/// resolved, entities decoded, tags stripped, soft hyphens removed, whitespace collapsed.</summary>
internal static class ApprovedProse
{
    private static string Root => TestPaths.FindSolutionRoot();

    public static string Baseline(int week) =>
        File.ReadAllText(Path.Combine(Root, "CbtLearningPlatform.Tests", "Golden", "ApprovedProse", $"Sedmica{week}.approved.txt"));

    public static string CurrentPage(int week) =>
        File.ReadAllText(Path.Combine(Root, "CbtLearningPlatform.Client", "Components", "Pages", $"Sedmica{week}.razor"));

    public static string NormalizedBaseline(int week) => Normalize(Baseline(week));

    public static string Normalize(string raw)
    {
        string text = Regex.Replace(raw, @"@\*.*?\*@", " ", RegexOptions.Singleline);
        text = text.Replace("\\\"", "\"");
        text = WebUtility.HtmlDecode(text);
        text = Regex.Replace(text, "<[^>]+>", " ");
        text = text.Replace("­", "");
        return Regex.Replace(text, @"\s+", " ").Trim();
    }

    /// <summary>The cells of the "Академично сравнение" table, row by row (Бек column, Елис column), normalised.</summary>
    public static string[][] ComparisonRows(string page)
    {
        int start = page.IndexOf("<caption>Академично сравнение", StringComparison.Ordinal);
        int end = page.IndexOf("</table>", start, StringComparison.Ordinal);
        string table = page[start..end];

        return [.. Regex.Matches(table, @"<tr>\s*<td>(?<a>.*?)</td>\s*<td>(?<b>.*?)</td>\s*</tr>", RegexOptions.Singleline)
            .Select(m => new[] { Normalize(m.Groups["a"].Value), Normalize(m.Groups["b"].Value) })];
    }

    public static string RemoveBetween(string raw, string startMarker, string endMarker)
    {
        int start = raw.IndexOf(startMarker, StringComparison.Ordinal);
        Assert.True(start >= 0, $"Baseline marker not found: {startMarker}");
        int end = raw.IndexOf(endMarker, start, StringComparison.Ordinal);
        Assert.True(end > start, $"Baseline end marker not found: {endMarker}");
        return raw.Remove(start, end - start);
    }

    /// <summary>Null when every word of the baseline appears, in the same order, in the current page (insertions are fine);
    /// otherwise a short excerpt of the first baseline text that is missing or changed.</summary>
    public static string? FirstMissingWord(string baselineRaw, string currentRaw)
    {
        string[] baseline = Normalize(baselineRaw).Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string[] current = Normalize(currentRaw).Split(' ', StringSplitOptions.RemoveEmptyEntries);

        int b = 0;
        foreach (string word in current)
        {
            if (b < baseline.Length && word == baseline[b]) b++;
        }

        return b == baseline.Length
            ? null
            : string.Join(' ', baseline.Skip(Math.Max(0, b - 4)).Take(12));
    }
}
