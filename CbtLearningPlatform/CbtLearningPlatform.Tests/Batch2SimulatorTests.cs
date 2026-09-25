using System.Net;
using System.Reflection;
using CbtLearningPlatform.Client.Components.Pages;
using CbtLearningPlatform.Client.Curriculum;
using CbtLearningPlatform.Client.Interactive;

namespace CbtLearningPlatform.Tests;

/// <summary>Owner remediation: Weeks 5/7/9 must carry a real stateful simulator in addition to
/// their retained retrieval activities.</summary>
public sealed class Batch2SimulatorTests
{
    public static IEnumerable<object[]> Activities()
    {
        yield return [5, Field<Sedmica5>("_week5CollaborationSimulator")];
        yield return [7, Field<Sedmica7>("_week7CycleSimulator")];
        yield return [9, Field<Sedmica9>("_week9ThoughtRecordSimulator")];
    }

    [Theory]
    [MemberData(nameof(Activities))]
    public void EverySimulator_IsValidMultiPathAndSourceTraced(int week, StatefulModelActivity activity)
    {
        Assert.Empty(activity.Validate());
        StatefulModelNode initial = activity.States.Single(s => s.Id == activity.InitialStateId);
        Assert.True(initial.Choices.Count >= 2);
        Assert.True(initial.Choices.Select(c => c.NextStateId).Distinct().Count() >= 2);
        Assert.True(activity.States.Count >= 5);
        Assert.All(activity.States.SelectMany(s => s.Choices), choice =>
        {
            Assert.False(string.IsNullOrWhiteSpace(choice.SourceRef));
            Assert.Contains(week.ToString(), choice.SourceRef!);
            Assert.Contains("SRC-041", choice.SourceRef!);
            Assert.Matches(@"\b(?:K|U)\d+", choice.SourceRef!);
        });
    }

    [Theory]
    [MemberData(nameof(Activities))]
    public void EveryInitialPath_ChangesVisibleStateAndCanReachATerminalState(int _, StatefulModelActivity activity)
    {
        foreach (StatefulModelChoice firstChoice in activity.States.Single(s => s.Id == activity.InitialStateId).Choices)
        {
            StatefulModelState state = new(activity);
            string initialId = state.CurrentStateId;
            Assert.Null(state.LatestTransition);
            Assert.False(state.Commit());

            Assert.True(state.Select(firstChoice.Id));
            Assert.Null(state.LatestTransition); // no consequence before commitment
            Assert.True(state.Commit());
            Assert.NotEqual(initialId, state.CurrentStateId);
            Assert.NotNull(state.LatestTransition);

            int guard = 0;
            while (!state.IsTerminal && guard++ < activity.States.Count)
            {
                StatefulModelChoice next = state.Current.Choices[0];
                Assert.True(state.Select(next.Id));
                Assert.True(state.Commit());
            }
            Assert.True(state.IsTerminal);
            Assert.True(state.History.Count >= 2);
        }
    }

    [Theory]
    [MemberData(nameof(Activities))]
    public void SimulatorFeedbackAndConsequences_DoNotExistInInitialMarkup(int week, StatefulModelActivity activity)
    {
        string html = WebUtility.HtmlDecode(ComponentRender.Html<StatefulModelSimulator>(new Dictionary<string, object?>
        {
            ["Activity"] = activity,
            ["ComponentId"] = $"week{week}-test",
            ["SafetyMode"] = ActiveLearningSafetyMode.NormalLearning
        }));

        Assert.Contains($"data-state=\"{activity.InitialStateId}\"", html);
        Assert.DoesNotContain("stateful-model__feedback", html);
        foreach (StatefulModelChoice choice in activity.States.SelectMany(s => s.Choices))
        {
            Assert.DoesNotContain(choice.Consequence, html);
        }
    }

    [Theory]
    [InlineData(5, "week5-collaboration-simulator")]
    [InlineData(7, "week7-cycle-simulator")]
    [InlineData(9, "week9-thought-record-simulator")]
    public void Simulator_IsBeforeFinalAssessment_AndRetrievalLayerRemains(int week, string componentId)
    {
        string markup = WeekPageMarkup.Read(week);
        int simulator = markup.IndexOf($"ComponentId=\"{componentId}\"", StringComparison.Ordinal);
        int assessment = markup.IndexOf("<FinalAssessment", StringComparison.Ordinal);
        Assert.True(simulator >= 0 && simulator < assessment);
        Assert.Contains(week == 7 ? "<PredictReveal" : "<ClassifyMatchCheck", markup);
    }

    [Fact]
    public void Week9Simulator_IsClosedChoiceThirdPerson_NoClinicalFreeTextOrPersistence()
    {
        string component = File.ReadAllText(Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Interactive", "StatefulModelSimulator.razor"));
        StatefulModelActivity activity = Field<Sedmica9>("_week9ThoughtRecordSimulator");

        Assert.DoesNotContain("<textarea", component, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("type=\"text\"", component, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("localStorage", component);
        Assert.DoesNotContain("sessionStorage", component);
        Assert.Contains("трето лице", activity.Instruction);
    }

    [Fact]
    public void SimulatorCoreValues_AreAlreadyPresentInTheApprovedWeekMaterial()
    {
        string w5 = ApprovedProse.Baseline(5);
        Assert.Contains("Терапевтът е по-активен", w5);
        Assert.Contains("пациентът поема все по-активна роля", w5);

        string w7 = ApprovedProse.Baseline(7);
        Assert.Contains("основно между 0 и 3", w7);
        Assert.Contains("Действителни оценки: 3 до 5", w7);
        Assert.Contains("Действителни оценки: по 1", w7);

        string w9 = ApprovedProse.Baseline(9);
        Assert.Contains("Значи цялостно се представих ужасно", w9);
        Assert.Contains("Четене на мисли", w9);
        Assert.Contains("Какви са доказателствата за тази идея", w9);
    }

    private static StatefulModelActivity Field<TPage>(string name) =>
        (StatefulModelActivity)(typeof(TPage).GetField(name, BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null)
            ?? throw new InvalidOperationException($"Missing {typeof(TPage).Name}.{name}"));
}
