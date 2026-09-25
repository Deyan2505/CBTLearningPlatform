using System.Text.RegularExpressions;
using CbtLearningPlatform.Client.Curriculum;
using CbtLearningPlatform.Client.Interactive;

namespace CbtLearningPlatform.Tests;

/// <summary>Active Learning Toolkit — what a learner (and assistive tech) actually receives on first render, produced by the real
/// Razor components through Blazor's HtmlRenderer. Interaction is covered by the state-machine tests and the Playwright E2E
/// on the harness. Fixtures are neutral placeholders — the engines contain no CBT content and neither do these tests.</summary>
public sealed class ActiveLearningToolkitRenderTests
{
    private static readonly ClassifyMatchActivity ClassifyData = new(
        "Sort things", "Pick one for each.", ClassifyMatchMode.Classify,
        [new("o1", "Option One"), new("o2", "Option Two")],
        [
            new("a", "Item Alpha", "o1", "SECRET-EXPLANATION-A", "SECRET-SOURCE-A"),
            new("b", "Item Beta", "o2", "SECRET-EXPLANATION-B")
        ]);

    private static readonly OrderingActivity OrderingData = new(
        "Order things", "Use the buttons.",
        [new("s0", "Step Zero", "SECRET-STEP-WHY-0"), new("s1", "Step One"), new("s2", "Step Two"), new("s3", "Step Three")],
        Feedback: "SECRET-FEEDBACK", SourceRef: "SECRET-SOURCE-O", ShuffleSeed: 5);

    private static readonly PredictRevealActivity PredictData = new(
        "Predict it", "A neutral scenario.", "What happens?",
        [new("y", "Outcome Y", "SECRET-FEEDBACK-Y"), new("n", "Outcome N", "SECRET-FEEDBACK-N")],
        ActualOptionId: "y", Explanation: "SECRET-EXPLANATION-P", SourceRef: "SECRET-SOURCE-P");

    private static Dictionary<string, object?> P(object activity, ActiveLearningSafetyMode? mode = null, string? notice = null, string id = "t") => new()
    {
        ["Activity"] = activity,
        ["ComponentId"] = id,
        ["SafetyMode"] = mode ?? ActiveLearningSafetyMode.NormalLearning,
        ["SafetyNotice"] = notice
    };

    private static string Classify(ActiveLearningSafetyMode? mode = null, string? notice = null) =>
        HtmlText.Decode(ComponentRender.Html<ClassifyMatchCheck>(P(ClassifyData, mode, notice)));

    private static string Ordering(ActiveLearningSafetyMode? mode = null, string? notice = null) =>
        HtmlText.Decode(ComponentRender.Html<OrderingBuilder>(P(OrderingData, mode, notice)));

    private static string Predict(ActiveLearningSafetyMode? mode = null, string? notice = null) =>
        HtmlText.Decode(ComponentRender.Html<PredictReveal>(P(PredictData, mode, notice)));

    // ---------------------------------------------------------------- A. classify / match

    [Fact]
    public void Classify_RendersOneRadioGroupPerItem_InAFieldsetWithALegend()
    {
        string html = Classify();

        Assert.Equal(2, Regex.Matches(html, "<fieldset").Count);
        Assert.Equal(2, Regex.Matches(html, "<legend").Count);
        Assert.Equal(4, Regex.Matches(html, "type=\"radio\"").Count);
        Assert.Contains("name=\"t-a\"", html);
        Assert.Contains("name=\"t-b\"", html);
        Assert.Contains("Item Alpha", html);
        Assert.Contains("Option Two", html);
    }

    [Fact]
    public void Classify_BeforeCommitment_ExposesNoFeedbackAtAll()
    {
        string html = Classify();

        Assert.DoesNotContain("SECRET-EXPLANATION-A", html);
        Assert.DoesNotContain("SECRET-EXPLANATION-B", html);
        Assert.DoesNotContain("SECRET-SOURCE-A", html);
        Assert.DoesNotContain("✓", html);
        Assert.DoesNotContain("✗", html);
        Assert.DoesNotContain("верен отговор", html);
        Assert.DoesNotContain("active-learning__explanation", html);
        Assert.Contains("0 от 2 отговорени", html);
    }

    [Fact]
    public void Classify_TheCheckButtonIsDisabledUntilTheLearnerHasAnswered()
    {
        string html = Classify();

        Match check = Regex.Match(html, "<button[^>]*>Провери</button>");
        Assert.True(check.Success);
        Assert.Contains("disabled", check.Value);
    }

    // ---------------------------------------------------------------- B. ordering builder

    [Fact]
    public void Ordering_OffersNonDragMoveButtonsWithDescriptiveLabels_AndNeverDraggableItems()
    {
        string html = Ordering();

        Assert.Equal(4, Regex.Matches(html, "<li class=\"active-learning__order-item").Count);
        foreach (string step in new[] { "Step Zero", "Step One", "Step Two", "Step Three" })
        {
            Assert.Contains($"aria-label=\"Премести „{step}“ нагоре\"", html);
            Assert.Contains($"aria-label=\"Премести „{step}“ надолу\"", html);
        }
        Assert.DoesNotContain("draggable", html);
        Assert.Contains("Провери подредбата", html);
    }

    [Fact]
    public void Ordering_OnlyTheEdgeButtonsAreDisabled_AndTheStartingOrderIsNotTheCorrectOne()
    {
        string html = Ordering();

        // exactly the first item's "up" and the last item's "down" are disabled
        Assert.Equal(2, Regex.Matches(html, "<button[^>]*disabled[^>]*aria-label=\"Премести|<button[^>]*aria-label=\"Премести[^>]*disabled").Count);

        string[] shown = [.. Regex.Matches(html, "active-learning__order-text\">([^<]+)<").Select(m => m.Groups[1].Value)];
        Assert.Equal(4, shown.Length);
        Assert.NotEqual(new[] { "Step Zero", "Step One", "Step Two", "Step Three" }, shown);
    }

    [Fact]
    public void Ordering_BeforeCommitment_ExposesNoVerdictNoSolutionAndNoExplanation()
    {
        string html = Ordering();

        Assert.DoesNotContain("SECRET-", html);
        Assert.DoesNotContain("На мястото си", html);
        Assert.DoesNotContain("Не е на мястото си", html);
        Assert.DoesNotContain("active-learning__solution", html);
        Assert.DoesNotContain("active-learning__reveal", html);
        Assert.DoesNotContain("Покажи верния ред", html);
    }

    // ---------------------------------------------------------------- C. committed predict -> reveal

    [Fact]
    public void Predict_IsNotADetailsReveal_AndHidesEverythingUntilTheLearnerCommits()
    {
        string html = Predict();

        Assert.DoesNotContain("<details", html);
        Assert.DoesNotContain("<summary", html);
        Assert.DoesNotContain("SECRET-", html);
        Assert.DoesNotContain("Действителният резултат", html);
        Assert.DoesNotContain("Вашето предсказание", html);
        Assert.DoesNotContain("active-learning__reveal", html);
        Assert.Contains("A neutral scenario.", html);
        Assert.Contains("What happens?", html);
    }

    [Fact]
    public void Predict_RendersAnAccessibleRadioGroup_AndADisabledCommitButton()
    {
        string html = Predict();

        Assert.Contains("<fieldset", html);
        Assert.Contains("<legend>What happens?</legend>", html);
        Assert.Equal(2, Regex.Matches(html, "type=\"radio\"").Count);
        Match commit = Regex.Match(html, "<button[^>]*>Потвърди предсказанието</button>");
        Assert.True(commit.Success);
        Assert.Contains("disabled", commit.Value);
    }

    // ---------------------------------------------------------------- shared frame + safety modes

    [Fact]
    public void EveryEngine_HasATitledSection_WhoseHeadingIdMatchesAriaLabelledby()
    {
        foreach (string html in new[] { Classify(), Ordering(), Predict() })
        {
            Match labelled = Regex.Match(html, "<section[^>]*aria-labelledby=\"([^\"]+)\"");
            Assert.True(labelled.Success);
            Assert.Contains($"<h3 id=\"{labelled.Groups[1].Value}\"", html);
        }
    }

    [Fact]
    public void EveryEngine_UsesOnlyClosedChoiceControls_NoFreeTextAnywhere()
    {
        foreach (string html in new[] { Classify(), Ordering(), Predict() })
        {
            Assert.DoesNotContain("<textarea", html);
            Assert.DoesNotContain("<select", html);
            foreach (Match input in Regex.Matches(html, "<input[^>]*>"))
            {
                Assert.Contains("type=\"radio\"", input.Value);
            }
        }
    }

    [Fact]
    public void GeneratedIds_AreUniquePerInstance_SoSeveralEnginesCanShareAPage()
    {
        string Id(string html) => Regex.Match(html, "aria-labelledby=\"([^\"]+)\"").Groups[1].Value;

        var noId = new Dictionary<string, object?> { ["Activity"] = ClassifyData };
        string first = Id(ComponentRender.Html<ClassifyMatchCheck>(noId));
        string second = Id(ComponentRender.Html<ClassifyMatchCheck>(noId));

        Assert.NotEqual(first, second);
    }

    [Theory]
    [InlineData(ActiveLearningSafetyMode.AcademicThirdPerson)]
    [InlineData(ActiveLearningSafetyMode.ProfessionalContext)]
    [InlineData(ActiveLearningSafetyMode.NoSelfGuidedSimulation)]
    public void EveryEngine_ShowsTheMandatoryNoticeInRestrictedModes_ButStillFullyInteractive(ActiveLearningSafetyMode mode)
    {
        string notice = ActiveLearningSafety.Profile(mode).DefaultNotice;

        string classify = Classify(mode);
        string ordering = Ordering(mode);
        string predict = Predict(mode);

        foreach (string html in new[] { classify, ordering, predict })
        {
            Assert.Contains("role=\"note\"", html);
            Assert.Contains(notice, html);
            Assert.Contains($"data-safety-mode=\"{mode}\"", html);
        }

        // safety framing never turns the activity into passive reading
        Assert.Equal(4, Regex.Matches(classify, "type=\"radio\"").Count);
        Assert.Contains("Провери подредбата", ordering);
        Assert.Contains("Потвърди предсказанието", predict);
    }

    [Fact]
    public void NormalLearning_AddsNoNotice()
    {
        foreach (string html in new[] { Classify(), Ordering(), Predict() })
        {
            Assert.DoesNotContain("active-learning__notice", html);
        }
    }

    [Fact]
    public void ANoticeOverride_ReplacesTheDefault_ButABlankOneCannotRemoveIt()
    {
        Assert.Contains("Approved wording.", Classify(ActiveLearningSafetyMode.ProfessionalContext, "Approved wording."));

        string blank = Classify(ActiveLearningSafetyMode.ProfessionalContext, "   ");
        Assert.Contains(ActiveLearningSafety.Profile(ActiveLearningSafetyMode.ProfessionalContext).DefaultNotice, blank);
    }

    [Fact]
    public void InvalidData_RendersAnUnavailableMessage_InsteadOfThrowingOrShowingABrokenExercise()
    {
        var bad = new ClassifyMatchActivity("", "", ClassifyMatchMode.Classify, [], []);
        string html = ComponentRender.Html<ClassifyMatchCheck>(P(bad));

        Assert.Contains("data-config-error=\"classify-match\"", html);
        Assert.DoesNotContain("type=\"radio\"", html);

        Assert.Contains("data-config-error=\"ordering\"",
            ComponentRender.Html<OrderingBuilder>(P(new OrderingActivity("T", "I", []))));
        Assert.Contains("data-config-error=\"predict-reveal\"",
            ComponentRender.Html<PredictReveal>(P(new PredictRevealActivity("T", "", "", [], null, ""))));
    }
}

/// <summary>Structural guards on the toolkit's own source: presentation/interaction engines only.</summary>
public sealed class ActiveLearningToolkitStructureTests
{
    private static readonly string[] EngineFiles =
    [
        Path.Combine("Interactive", "ClassifyMatchCheck.razor"),
        Path.Combine("Interactive", "OrderingBuilder.razor"),
        Path.Combine("Interactive", "PredictReveal.razor"),
        Path.Combine("Components", "Shared", "ActiveLearningFrame.razor"),
        Path.Combine("Curriculum", "ActiveLearningModels.cs"),
        Path.Combine("Curriculum", "ActiveLearningState.cs"),
        Path.Combine("Curriculum", "ActiveLearningSafety.cs")
    ];

    private static string Read(string relative) =>
        File.ReadAllText(Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", relative));

    public static IEnumerable<object[]> Files => EngineFiles.Select(f => new object[] { f });

    [Theory]
    [MemberData(nameof(Files))]
    public void NoCbtContentIsHardCodedInSharedEngines(string file)
    {
        // Shared engines are presentation/interaction only; weeks supply approved, source-grounded data.
        string source = Read(file);
        // stems match at the start of a word; exact tokens must stand alone (so the "Cbt" in a namespace is not a hit)
        string[] stems = ["когнитив", "автоматичн", "вярван", "терапев", "депрес", "тревог", "изкривяван"];
        string[] tokens = ["КПТ", "CBT", "Сали", "Ирина", "Мартин", "Радо", "Бек", "Елис", "поведенческа активация"];

        foreach (string stem in stems)
        {
            Assert.False(Regex.IsMatch(source, @"(?<!\p{L})" + Regex.Escape(stem), RegexOptions.IgnoreCase), $"{file} contains CBT vocabulary '{stem}'.");
        }
        foreach (string token in tokens)
        {
            Assert.False(Regex.IsMatch(source, @"(?<!\p{L})" + Regex.Escape(token) + @"(?!\p{L})", RegexOptions.IgnoreCase), $"{file} contains CBT vocabulary '{token}'.");
        }
    }

    [Fact]
    public void TheEnginesUseNoFreeTextNoStorageNoNetworkAndNoJsInterop()
    {
        string[] files = ["Interactive/ClassifyMatchCheck.razor", "Interactive/OrderingBuilder.razor", "Interactive/PredictReveal.razor", "Interactive/StatefulModelSimulator.razor", "Components/Shared/ActiveLearningFrame.razor"];

        foreach (string file in files)
        {
            string markup = Read(file.Replace('/', Path.DirectorySeparatorChar));

            Assert.DoesNotContain("<textarea", markup);
            Assert.DoesNotContain("type=\"text\"", markup);
            Assert.DoesNotContain("<select", markup);
            Assert.DoesNotContain("@bind", markup);
            Assert.DoesNotContain("localStorage", markup);
            Assert.DoesNotContain("sessionStorage", markup);
            Assert.DoesNotContain("HttpClient", markup);
            Assert.DoesNotContain("IJSRuntime", markup);
            Assert.DoesNotContain("@inject", markup);
        }
    }

    [Fact]
    public void ToolkitStyles_AreAdditiveAndNeverTouchTheLockedSimulatorsSelectors()
    {
        string css = File.ReadAllText(Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "wwwroot", "app.css"));
        int start = css.IndexOf("Active Learning Toolkit (ClassifyMatchCheck", StringComparison.Ordinal);
        Assert.True(start > 0);
        string toolkit = css[start..css.IndexOf("Blazor framework hooks", start, StringComparison.Ordinal)];

        // toolkit rules plus the shared stateful-model engine; locked simulator selectors remain untouched
        foreach (Match selector in Regex.Matches(toolkit, @"^\.([a-z][a-z0-9_-]*)", RegexOptions.Multiline))
        {
            string name = selector.Groups[1].Value;
            Assert.True(name.StartsWith("active-learning") || name.StartsWith("stateful-model") || name == "scenario-simulator__config-error",
                $"Toolkit CSS defines unexpected selector .{name}");
        }
    }
}
