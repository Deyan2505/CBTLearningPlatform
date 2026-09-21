using System.Reflection;
using System.Text.RegularExpressions;
using CbtLearningPlatform.Client.Components.Pages;
using CbtLearningPlatform.Client.Curriculum;
using CbtLearningPlatform.Client.Interactive;

namespace CbtLearningPlatform.Tests;

/// <summary>BATCH 1 REMEDIATION — the owner's three findings after reviewing Weeks 1, 2 and 10 in production:
///  A. Week 1 read too thin. The source/KU audit found four Included KUs that the implementation under-represented
///     (U3, U11, U16, U19 of WEEK_01_RETROFIT_AUDIT_v1 §0) — these assert the restored material is on the page.
///  B/C. Weeks 1 and 2 had no Weekly Mind Map. These assert both maps exist as Preview + Review of ONE semantic model,
///     are real single-parent knowledge hierarchies, and are built only from wording those weeks already teach.
///  D. Week 10's static six-category visual is now also manipulable: CaseExaminationSimulator drives the approved
///     Сали/Карен case, and the closing re-rating is withheld until every question has been applied.
///  E. The Weekly Mind Map is now a gate. Week 12 is the one routed week without a map and is reported, not papered over.</summary>
public sealed class ActiveLearningBatch1RemediationTests
{
    private static T Field<T>(Type page, string name) =>
        (T)(page.GetField(name, BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException($"{page.Name}.{name} not found"))
            .GetValue(null)!;

    private static MindMapModel MindMap(Type page, string builder)
    {
        MethodInfo method = page.GetMethod(builder, BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException($"{page.Name}.{builder} not found");
        return (MindMapModel)method.Invoke(null, null)!;
    }

    private static MindMapModel Week1Map => MindMap(typeof(Sedmica1), "BuildWeek1MindMap");
    private static MindMapModel Week2Map => MindMap(typeof(Sedmica2), "BuildWeek2MindMap");
    private static CaseExaminationActivity Week10Case => Field<CaseExaminationActivity>(typeof(Sedmica10), "_week10CaseExamination");

    // ================================================================ A. Week 1 source/KU depth

    /// <summary>The four Included KUs the audit approved but the implementation left out or only gestured at. Each entry is
    /// (KU, a phrase that can only be there if the KU was actually restored, the section it belongs to).</summary>
    public static IEnumerable<object[]> RestoredWeek1Kus =>
    [
        ["U16", "нужда да страдат", "nauchen-obrat"],
        ["U16", "домина", "nauchen-obrat"],
        ["U3", "изкривената, негативна когниция", "nauchen-obrat"],
        ["U3", "краткосрочно", "nauchen-obrat"],
        ["U19", "специализанти", "izsledvane-1977"],
        ["U11", "500 проучвания", "izsledvane-1977"]
    ];

    [Theory]
    [MemberData(nameof(RestoredWeek1Kus))]
    public void Week1_RestoresTheIncludedKusThatWereUnderRepresented(string ku, string phrase, string section)
    {
        string markup = WeekPageMarkup.Read(1);

        Assert.True(markup.Contains(phrase, StringComparison.Ordinal),
            $"Week 1 {ku}: approved Included KU is still missing from the page (expected '{phrase}').");

        // and it sits in the section the audit assigns it to, not dropped anywhere convenient
        int anchor = markup.IndexOf($"id=\"{section}\"", StringComparison.Ordinal);
        int next = markup.IndexOf("<h2 ", anchor, StringComparison.Ordinal);
        string sectionText = next > anchor ? markup[anchor..next] : markup[anchor..];
        Assert.True(sectionText.Contains(phrase, StringComparison.Ordinal), $"Week 1 {ku} ('{phrase}') is not in section '{section}'.");
    }

    [Fact]
    public void Week1_DoesNotPullInTheDeferredOrExcludedKus()
    {
        // The audit deferred the cognitive-model architecture (Week 3/12), Sally's case (Week 3), session structure
        // (Weeks 5–7) and the ten principles; restoring depth must not become scope creep into those weeks.
        string markup = WeekPageMarkup.Read(1);

        foreach (string forbidden in new[] { "основно вярване", "междинно вярване", "Сали", "десет принципа", "десетте принципа" })
        {
            Assert.DoesNotContain(forbidden, markup, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void Week1_StillHasItsFourApprovedAssessmentQuestions_AndTheOrderingBeforeThem()
    {
        string source = File.ReadAllText(PagePath(1));

        Assert.Equal(4, TestPaths.CountFinalAssessmentQuestions(source, "_week1FinalAssessment"));
        Assert.True(source.IndexOf("<OrderingBuilder", StringComparison.Ordinal) < source.IndexOf("<FinalAssessment", StringComparison.Ordinal));
    }

    // ================================================================ B/C. the two new Weekly Mind Maps

    public static IEnumerable<object[]> NewMindMapWeeks => [[1], [2]];

    [Theory]
    [MemberData(nameof(NewMindMapWeeks))]
    public void NewMindMaps_RenderAsPreviewAndReview_FromOneSemanticModel(int week)
    {
        string markup = WeekPageMarkup.Read(week);

        Assert.Contains($"ComponentId=\"week{week}-mindmap-preview\"", markup);
        Assert.Contains($"ComponentId=\"week{week}-mindmap-review\"", markup);

        // one model, two renderings — never two hand-maintained copies
        Assert.Equal(2, Regex.Matches(markup, $@"Model=""@_week{week}MindMapRender""").Count);

        // the review copy is the collapsed retrieval check, the preview is not inside a <details>
        int review = markup.IndexOf($"ComponentId=\"week{week}-mindmap-review\"", StringComparison.Ordinal);
        int retrieval = markup.LastIndexOf("concept-graph__retrieval-check", review, StringComparison.Ordinal);
        Assert.True(retrieval >= 0, $"Week {week}: the review map must sit inside the collapsed retrieval-check disclosure.");

        int preview = markup.IndexOf($"ComponentId=\"week{week}-mindmap-preview\"", StringComparison.Ordinal);
        Assert.True(preview < markup.IndexOf("<details", StringComparison.Ordinal), $"Week {week}: the preview map must not be collapsed.");
    }

    [Theory]
    [MemberData(nameof(NewMindMapWeeks))]
    public void NewMindMaps_AreValidSingleParentHierarchies_WithExactlyOneRoot(int week)
    {
        MindMapModel model = week == 1 ? Week1Map : Week2Map;

        Assert.Single(model.Nodes, n => n.ParentId is null);
        Assert.Equal(model.Nodes.Count, model.Nodes.Select(n => n.Id).Distinct(StringComparer.Ordinal).Count());
        Assert.All(model.Nodes.Where(n => n.ParentId is not null),
            n => Assert.Contains(model.Nodes, p => p.Id == n.ParentId));

        // the adapter is the real validator (dangling parents, cycles) — it must accept the model
        Assert.NotNull(MindMapAdapter.ToRenderModel(model));

        // a map worth having: several clusters, each with real children, and more than one level below the root
        MindMapNode root = model.Nodes.Single(n => n.ParentId is null);
        var clusters = model.Nodes.Where(n => n.ParentId == root.Id).ToList();
        Assert.True(clusters.Count >= 4, $"Week {week}: expected at least four knowledge clusters, found {clusters.Count}.");
        Assert.True(model.Nodes.Count >= 12, $"Week {week}: expected a substantive map, found {model.Nodes.Count} nodes.");
        Assert.Contains(clusters, c => model.Nodes.Any(n => n.ParentId == c.Id));
    }

    [Theory]
    [MemberData(nameof(NewMindMapWeeks))]
    public void NewMindMaps_AreKnowledgeClusters_NotARepeatOfTheSectionTitles(int week)
    {
        MindMapModel model = week == 1 ? Week1Map : Week2Map;
        string markup = WeekPageMarkup.Read(week);

        string[] headings = [.. Regex.Matches(markup, @"<h2 id=""[^""]+""[^>]*>\s*(?<t>[^<]+?)\s*</h2>")
            .Select(m => m.Groups["t"].Value)
            .Select(t => Regex.Replace(t, @"^\d+\s*·\s*", "").Trim())];

        Assert.NotEmpty(headings);

        // A concept may legitimately share its name with the section that teaches it (e.g. "ABC моделът"); what the rule
        // forbids is a map that is MERELY the section list re-drawn. So: section-named nodes must stay a small minority,
        // and the map must carry more knowledge than the page has sections.
        string[] echoed = [.. model.Nodes.Where(n => headings.Contains(n.Label, StringComparer.OrdinalIgnoreCase)).Select(n => n.Label)];
        Assert.True(echoed.Length * 4 <= model.Nodes.Count,
            $"Week {week}: {echoed.Length} of {model.Nodes.Count} nodes just restate section titles ({string.Join(", ", echoed)}).");
        Assert.True(model.Nodes.Count > headings.Length, $"Week {week}: the map is no richer than the section list.");
    }

    [Theory]
    [MemberData(nameof(NewMindMapWeeks))]
    public void NewMindMaps_AnchorsPointAtRealRouteSafeSectionsOfTheirOwnWeek(int week)
    {
        MindMapModel model = week == 1 ? Week1Map : Week2Map;
        string markup = WeekPageMarkup.Read(week);

        foreach (MindMapNode node in model.Nodes.Where(n => n.Anchor is not null))
        {
            string anchor = node.Anchor!;
            Assert.StartsWith($"/kurs/sedmica-{week}#", anchor, StringComparison.Ordinal);
            Assert.Contains($"id=\"{anchor.Split('#')[1]}\"", markup);
        }
    }

    [Fact]
    public void Week1MindMap_IsBuiltFromTheWeeksOwnApprovedVocabulary()
    {
        string approved = ApprovedProse.Normalize(WeekPageMarkup.Read(1));

        foreach (string term in new[] { "Изследване на сънищата", "Два потока на мислене", "Автоматична мисъл", "Изкривена негативна когниция" })
        {
            string head = term.Split(' ')[0];
            Assert.True(approved.Contains(head, StringComparison.OrdinalIgnoreCase), $"Week 1 map node '{term}' has no basis in the page.");
        }

        // the map tells the week's actual story, in order: assumption -> test -> discovery -> confirmation -> growth
        string[] clusterIds = [.. Week1Map.Nodes.Where(n => n.ParentId == "root").Select(n => n.Id)];
        Assert.Equal(["izhodna", "proverka", "otkritie", "nov-vapros", "potvarzhdenie", "razshiryavane"], clusterIds);
    }

    [Fact]
    public void Week2MindMap_GivesEachSchoolItsOwnBranch_AndNeverMergesTheirVocabulary()
    {
        MindMapModel model = Week2Map;

        MindMapNode beck = model.Nodes.Single(n => n.Id == "bek");
        MindMapNode ellis = model.Nodes.Single(n => n.Id == "elis");
        Assert.Equal("root", beck.ParentId);
        Assert.Equal("root", ellis.ParentId);

        // the ABC model belongs to Ellis's branch, with its three parts under it
        Assert.Equal("elis", model.Nodes.Single(n => n.Id == "abc").ParentId);
        Assert.Equal(["abc-a", "abc-b", "abc-c"], model.Nodes.Where(n => n.ParentId == "abc").Select(n => n.Id));

        // Beck's own terms stay on Beck's branch; Week 3/12's belief hierarchy is never introduced here
        Assert.Equal("bek", model.Nodes.Single(n => n.Id == "avtomatichna-misal").ParentId);
        Assert.DoesNotContain(model.Nodes, n => n.Label.Contains("основно вярване", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(model.Nodes, n => n.Label.Contains("междинно", StringComparison.OrdinalIgnoreCase));

        // and the week's framing rule is in the map, not just the prose
        Assert.Contains(model.Nodes, n => n.Id == "obshto");
        Assert.Contains(model.Nodes, n => n.Id == "ramka");
    }

    // ================================================================ D. Week 10's interactive model

    [Fact]
    public void Week10Case_IsTheApprovedSaliKarenCase_TakenFromTheExplorerThePageAlreadyShows()
    {
        CaseExaminationActivity activity = Week10Case;
        string explorer = File.ReadAllText(Path.Combine(
            TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Interactive", "SocraticDialogueExplorer.razor"));

        Assert.Empty(activity.Validate());

        // every case fact and every finding is wording the approved explorer already carries
        foreach (CaseFact fact in activity.Case)
        {
            Assert.Contains(fact.Value, explorer, StringComparison.Ordinal);
        }
        foreach (ExaminationTool tool in activity.Tools)
        {
            Assert.Contains(tool.Finding, explorer, StringComparison.Ordinal);
            Assert.Contains($"new(\"{tool.Name}\"", explorer, StringComparison.Ordinal);
        }

        // the six categories, in the source's own order (Figure 11.1), matching section 10.2
        Assert.Equal(
            ["Доказателства", "Алтернативно обяснение", "Декатастрофизиране", "Ефект от вярването", "Дистанциране", "Решаване на проблема"],
            activity.Tools.Select(t => t.Name));

        string page = WeekPageMarkup.Read(10);
        int last = -1;
        foreach (ExaminationTool tool in activity.Tools)
        {
            int index = page.IndexOf($"<h3>{tool.Name}</h3>", StringComparison.Ordinal);
            Assert.True(index > last, $"'{tool.Name}' must follow the previous category in the approved 10.2 list.");
            last = index;
        }
    }

    [Fact]
    public void Week10Case_ClosingOutcomeIsThePagesOwnReRating_NotAnInventedScore()
    {
        CaseExaminationActivity activity = Week10Case;
        string page = ApprovedProse.Normalize(WeekPageMarkup.Read(10));

        Assert.Contains("от 90% на около 20%", activity.Outcome);
        Assert.Contains(ApprovedProse.Normalize(activity.Outcome), page);

        // the model never invents per-step percentages: no tool finding claims a belief rating of its own
        Assert.All(activity.Tools, t => Assert.DoesNotMatch(@"\d+\s*%", t.Finding));
    }

    [Fact]
    public void Week10Case_EveryDistractorIsAnotherApprovedCategory_NeverAFalseClaim()
    {
        CaseExaminationActivity activity = Week10Case;

        // The six "what will this surface?" labels are one-line restatements of the six approved categories, so a wrong
        // option is only ever a different category — never an invented or incorrect statement about the case.
        string[] surfaces = [.. activity.Tools.SelectMany(t => t.Options).Select(o => o.Label).Distinct()];
        Assert.Equal(6, surfaces.Length);

        foreach (ExaminationTool tool in activity.Tools)
        {
            Assert.True(tool.Options.Count >= 3, $"'{tool.Name}' should offer a real choice.");
            Assert.All(tool.Options, o => Assert.Contains(o.Label, surfaces));
            Assert.Contains(tool.Options, o => o.Id == tool.CorrectOptionId);
        }

        // the correct answer is not always in the same slot
        int[] positions = [.. activity.Tools.Select(t => t.Options.ToList().FindIndex(o => o.Id == t.CorrectOptionId))];
        Assert.True(positions.Distinct().Count() >= 3, "The correct option sits in too few distinct positions — it would be guessable.");
    }

    [Fact]
    public void Week10Case_InitialRender_ShowsTheModelButLeaksNoFindingAndNoOutcome()
    {
        string html = HtmlText.Decode(ComponentRender.Html<CaseExaminationSimulator>(new Dictionary<string, object?>
        {
            ["Activity"] = Week10Case,
            ["ComponentId"] = "week10-case-examination",
            ["SafetyMode"] = ActiveLearningSafety.ModeFor(CourseCatalog.Weeks.Single(w => w.Number == 10).SafetyLevel)
        }));

        // the case and its tools are visible…
        Assert.Contains("Ситуация", html);
        Assert.Contains("Автоматична мисъл", html);
        foreach (ExaminationTool tool in Week10Case.Tools)
        {
            Assert.Contains(tool.Name, html);
        }
        Assert.Contains("0 от 6 приложени", html);
        Assert.Contains("Още нищо.", html);

        // …and nothing the learner has not yet earned
        foreach (ExaminationTool tool in Week10Case.Tools)
        {
            Assert.DoesNotContain(tool.Finding, html);
            Assert.DoesNotContain(tool.Question, html);
        }
        Assert.DoesNotContain(Week10Case.Outcome, html);
        Assert.DoesNotContain(Week10Case.SourceRef!, html);
        Assert.DoesNotContain("active-learning__status", html);
        Assert.DoesNotContain("✓", html);
        Assert.DoesNotContain("✗", html);
    }

    [Fact]
    public void Week10Case_IsSolvableAndSitsBeforeTheFinalAssessment_InNormalSafetyMode()
    {
        string source = File.ReadAllText(PagePath(10));
        Assert.True(source.IndexOf("<CaseExaminationSimulator", StringComparison.Ordinal) < source.IndexOf("<FinalAssessment", StringComparison.Ordinal));
        Assert.Contains("<CaseExaminationSimulator ComponentId=\"week10-case-examination\" Activity=\"_week10CaseExamination\" SafetyMode=\"_safetyMode\" />", source);

        CaseExaminationState state = new(Week10Case);
        foreach (ExaminationTool tool in Week10Case.Tools)
        {
            Assert.True(state.Open(tool.Id));
            Assert.True(state.Select(tool.CorrectOptionId));
            Assert.True(state.Commit());
        }

        Assert.True(state.IsComplete);
        Assert.Equal(6, state.CorrectPredictionCount);
        Assert.Equal(Week10Case.Outcome, state.Outcome);
    }

    [Fact]
    public void Week10_KeepsItsStaticSixCategoryVisual_TheModelComplementsItRatherThanReplacingIt()
    {
        string markup = WeekPageMarkup.Read(10);

        Assert.Contains("<SocraticDialogueExplorer", markup);
        Assert.Contains("id=\"kategorii\"", markup);
        foreach (string category in new[] { "Доказателства", "Алтернативно обяснение", "Декатастрофизиране", "Ефект от вярването", "Дистанциране", "Решаване на проблема" })
        {
            Assert.Contains($"<h3>{category}</h3>", markup);
        }
    }

    // ================================================================ E. the Weekly Mind Map gate

    [Fact]
    public void WeeklyMindMap_IsNowItsOwnGate_SeparateFromTheVisualLearningModel()
    {
        var withMap = new WeekLearningArchitecture(99, StructuralStatus.StructuralEnrichmentRequired,
            [new(VisualModelKind.Sequence, "guided-practice-sequence"), new(VisualModelKind.MindMap, "ComponentId=\"week99-mindmap-preview\"")],
            [new(InteractionFamily.Simulator, "ScenarioSimulator")], [new(LearnerResponseKind.Order, "ScenarioSimulator")], true);

        Assert.Empty(ActiveLearningStandard.Evaluate(withMap));

        // remove only the map: the visual gate still passes, the mind-map gate does not
        var withoutMap = withMap with { VisualModels = [withMap.VisualModels[0]] };
        ActiveLearningGate[] failing = [.. ActiveLearningStandard.Evaluate(withoutMap).Select(f => f.Gate)];

        Assert.Equal([ActiveLearningGate.WeeklyMindMap], failing);
    }

    [Fact]
    public void WeeklyMindMap_CannotBeFakedByRelabellingAnotherVisual()
    {
        var relabelled = new WeekLearningArchitecture(99, StructuralStatus.StructuralEnrichmentRequired,
            [new(VisualModelKind.Sequence, "guided-practice-sequence"), new(VisualModelKind.MindMap, "guided-practice-sequence")],
            [new(InteractionFamily.Simulator, "ScenarioSimulator")], [new(LearnerResponseKind.Order, "ScenarioSimulator")], true);

        Assert.Contains(ActiveLearningStandard.Evaluate(relabelled), f => f.Gate == ActiveLearningGate.WeeklyMindMap);
    }

    [Fact]
    public void EveryRoutedWeekExceptTheKnownException_DeclaresAWeeklyMindMap()
    {
        int[] without = [.. ActiveLearningCatalog.Weeks
            .Where(w => !w.VisualModels.Any(v => v.Kind == VisualModelKind.MindMap))
            .Select(w => w.WeekNumber)
            .Order()];

        // Week 12 is the single known exception, reported to the owner rather than silently satisfied.
        Assert.Equal([12], without);
        Assert.Equal(StructuralStatus.StructuralEnrichmentRequired, ActiveLearningCatalog.For(12).Status);
        Assert.Contains(ActiveLearningStandard.Evaluate(ActiveLearningCatalog.For(12)), f => f.Gate == ActiveLearningGate.WeeklyMindMap);
    }

    [Fact]
    public void NoWeekLostItsStatus_ToTheNewGate()
    {
        // Adding a gate must not silently demote anyone: the compliant set is exactly the reference weeks plus Batch 1.
        int[] compliant = [.. ActiveLearningCatalog.Weeks
            .Where(w => w.Status == StructuralStatus.Compliant)
            .Select(w => w.WeekNumber)
            .Order()];

        Assert.Equal([1, 2, 3, 6, 8, 10], compliant);
        Assert.All(compliant, w => Assert.Empty(ActiveLearningStandard.Evaluate(ActiveLearningCatalog.For(w))));
    }

    private static string PagePath(int week) =>
        Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages", $"Sedmica{week}.razor");
}
