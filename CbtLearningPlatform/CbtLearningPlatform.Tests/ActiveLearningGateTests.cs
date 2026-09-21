using System.Text.RegularExpressions;
using CbtLearningPlatform.Client.Curriculum;
using Xunit.Abstractions;

namespace CbtLearningPlatform.Tests;

/// <summary>ACTIVE LEARNING GATE — the cross-week, catalog-driven enforcement of the mandatory Active Learning
/// Standard (root AGENTS.md; ADR-011). Same spirit as FinalAssessmentRolloutTests: one rule, every routed week.
///
/// Every routed week declares (Curriculum/ActiveLearningCatalog.cs) a VisualLearningModel, an ActiveLearningInteraction,
/// an ActiveLearnerResponse and a FinalAssessment. This gate (1) evaluates each declaration with the pure rules in
/// ActiveLearningStandard, (2) verifies every declaration against the week's real page markup, and (3) enforces the
/// controlled migration mechanism: a week may be StructuralEnrichmentRequired while it lacks A–D, but may NEVER be
/// Compliant without them — and may not stay StructuralEnrichmentRequired once it passes.
///
/// The current course is KNOWN to fail the standard in nine weeks; the catalog says so explicitly (Weeks 3, 6 and 8 are
/// the compliant references). Nothing here fakes compliance.</summary>
public sealed class ActiveLearningGateTests
{
    private const string FinalAssessmentTag = "<FinalAssessment";

    private readonly ITestOutputHelper _output;

    public ActiveLearningGateTests(ITestOutputHelper output) => _output = output;

    public static IEnumerable<object[]> DeclaredWeeks =>
        ActiveLearningCatalog.Weeks.Select(w => new object[] { w.WeekNumber });

    // ---------------------------------------------------------------- catalog integrity

    [Fact]
    public void Catalog_DeclaresEveryRoutedWeekExactlyOnce()
    {
        int[] routed = [.. CourseCatalog.Weeks.Where(w => w.Route is not null).Select(w => w.Number).Order()];
        int[] declared = [.. ActiveLearningCatalog.Weeks.Select(w => w.WeekNumber).Order()];

        Assert.Equal(routed, declared);
        Assert.Equal(declared.Length, declared.Distinct().Count());
    }

    [Fact]
    public void StructuralStatus_HasNoExemptionState()
    {
        // Safety tiers (AcademicContextOnly / ProfessionalReviewRequired / NotEligibleForSelfGuidedSimulator) change the
        // FORM of interaction and never exempt a week. If someone adds a "NotApplicable"/"Exempt" value this fails.
        StructuralStatus[] values = Enum.GetValues<StructuralStatus>();

        Assert.Equal(
            new[] { StructuralStatus.Compliant, StructuralStatus.StructuralEnrichmentRequired },
            values.Order().ToArray());
    }

    [Fact]
    public void Evaluate_TakesOnlyTheDeclaration_NeverASafetyTierOrWeekNumberExemption()
    {
        var method = typeof(ActiveLearningStandard).GetMethod(nameof(ActiveLearningStandard.Evaluate));

        Assert.NotNull(method);
        Assert.Single(method!.GetParameters());
        Assert.Equal(typeof(WeekLearningArchitecture), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void EveryInteractiveIslandFile_IsClassifiedByTheStandard()
    {
        string interactiveDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Interactive");
        string[] components = [.. Directory.GetFiles(interactiveDirectory, "*.razor").Select(f => Path.GetFileNameWithoutExtension(f))];

        Assert.NotEmpty(components);
        foreach (string component in components)
        {
            Assert.True(
                ActiveLearningStandard.Components.ContainsKey(component),
                $"Interactive component '{component}' is not classified in ActiveLearningStandard.Components — decide whether it qualifies as an interaction/response engine (by design, not by week).");
        }
    }

    [Fact]
    public void EveryClassifiedComponent_ExistsAsARazorFile()
    {
        string root = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client");

        foreach (string component in ActiveLearningStandard.Components.Keys)
        {
            bool exists =
                File.Exists(Path.Combine(root, "Interactive", component + ".razor")) ||
                File.Exists(Path.Combine(root, "Components", "Shared", component + ".razor"));
            Assert.True(exists, $"Classified component '{component}' has no .razor file.");
        }
    }

    // ---------------------------------------------------------------- the cross-week gate

    [Theory]
    [MemberData(nameof(DeclaredWeeks))]
    public void ActiveLearningGate_EveryWeek_StatusMatchesItsOwnGateEvaluation(int week)
    {
        // A–D must all pass for Compliant; a week that passes them may not stay StructuralEnrichmentRequired.
        string? problem = ActiveLearningStandard.CheckStatus(ActiveLearningCatalog.For(week));

        Assert.True(problem is null, problem);
    }

    [Theory]
    [MemberData(nameof(DeclaredWeeks))]
    public void ActiveLearningGate_EveryDeclaration_IsPresentInTheWeekPageMarkup(int week)
    {
        WeekLearningArchitecture architecture = ActiveLearningCatalog.For(week);
        string markup = WeekPageMarkup.Read(week);

        foreach (VisualModelDeclaration visual in architecture.VisualModels)
        {
            Assert.True(markup.Contains(visual.Marker, StringComparison.Ordinal),
                $"Week {week} declares visual '{visual.Marker}' ({visual.Kind}) but the page markup does not contain it.");
        }
        foreach (InteractionDeclaration interaction in architecture.Interactions)
        {
            Assert.True(markup.Contains($"<{interaction.Component}", StringComparison.Ordinal),
                $"Week {week} declares interaction <{interaction.Component}> but the page markup does not contain it.");
        }
        foreach (LearnerResponseDeclaration response in architecture.LearnerResponses)
        {
            Assert.True(markup.Contains($"<{response.Component}", StringComparison.Ordinal),
                $"Week {week} declares a learner response via <{response.Component}> but the page markup does not contain it.");
        }
    }

    [Theory]
    [MemberData(nameof(DeclaredWeeks))]
    public void ActiveLearningGate_EveryInteractionAndResponse_PrecedesTheFinalAssessmentInPageOrder(int week)
    {
        WeekLearningArchitecture architecture = ActiveLearningCatalog.For(week);
        string markup = WeekPageMarkup.Read(week);
        int finalAssessment = markup.IndexOf(FinalAssessmentTag, StringComparison.Ordinal);
        Assert.True(finalAssessment >= 0, $"Week {week} page has no {FinalAssessmentTag}.");

        IEnumerable<string> components =
            architecture.Interactions.Select(i => i.Component)
                .Concat(architecture.LearnerResponses.Select(r => r.Component))
                .Distinct();

        foreach (string component in components)
        {
            int position = markup.IndexOf($"<{component}", StringComparison.Ordinal);
            Assert.True(position >= 0 && position < finalAssessment,
                $"Week {week}: <{component}> must appear BEFORE the Final Assessment — the quiz is never the active-learning layer.");
        }
    }

    [Theory]
    [MemberData(nameof(DeclaredWeeks))]
    public void ActiveLearningGate_EveryWeek_DeclaresAndPresentsExactlyOneFinalAssessment(int week)
    {
        Assert.True(ActiveLearningCatalog.For(week).DeclaresFinalAssessment, $"Week {week} must declare a Final Assessment.");
        Assert.Single(Regex.Matches(WeekPageMarkup.Read(week), FinalAssessmentTag));
    }

    [Fact]
    public void CurrentReferenceWeeks_3_6_8_AreStructurallyCompliant()
    {
        foreach (int week in new[] { 3, 6, 8 })
        {
            WeekLearningArchitecture architecture = ActiveLearningCatalog.For(week);

            Assert.Equal(StructuralStatus.Compliant, architecture.Status);
            Assert.Empty(ActiveLearningStandard.Evaluate(architecture));
        }
    }

    [Fact]
    public void MigrationState_IsExplicit_NotFakedCompliance()
    {
        var migrating = ActiveLearningCatalog.Weeks
            .Where(w => w.Status == StructuralStatus.StructuralEnrichmentRequired)
            .OrderBy(w => w.WeekNumber)
            .ToList();

        foreach (WeekLearningArchitecture week in ActiveLearningCatalog.Weeks.OrderBy(w => w.WeekNumber))
        {
            string gates = string.Join(", ", ActiveLearningStandard.Evaluate(week).Select(f => f.Gate));
            _output.WriteLine($"Week {week.WeekNumber,2}: {week.Status.ToProjectLabel()}{(gates.Length > 0 ? $"  — failing: {gates}" : "")}");
        }

        // Migration is only ever the temporary state: it must always name the gates a week still fails.
        Assert.All(migrating, w => Assert.NotEmpty(ActiveLearningStandard.Evaluate(w)));
    }

    // ---------------------------------------------------------------- semantic rules (synthetic declarations)

    private static WeekLearningArchitecture Synthetic(
        IReadOnlyList<VisualModelDeclaration>? visuals = null,
        IReadOnlyList<InteractionDeclaration>? interactions = null,
        IReadOnlyList<LearnerResponseDeclaration>? responses = null,
        bool finalAssessment = true) =>
        new(99, StructuralStatus.StructuralEnrichmentRequired,
            visuals ?? [], interactions ?? [], responses ?? [], finalAssessment);

    private static readonly VisualModelDeclaration ValidVisual = new(VisualModelKind.Sequence, "guided-practice-sequence");
    private static readonly InteractionDeclaration ValidInteraction = new(InteractionFamily.Simulator, "ScenarioSimulator");
    private static readonly LearnerResponseDeclaration ValidResponse = new(LearnerResponseKind.Order, "ScenarioSimulator");

    [Fact]
    public void Rule_AWeekWithAllFourElementsPassesTheGate()
    {
        var week = Synthetic([ValidVisual], [ValidInteraction], [ValidResponse]);

        Assert.Empty(ActiveLearningStandard.Evaluate(week));
    }

    [Fact]
    public void Rule_FinalAssessmentAlone_CannotSatisfyTheInteractionOrResponseGates()
    {
        var week = Synthetic(
            [ValidVisual],
            [new(InteractionFamily.InteractiveModel, "FinalAssessment")],
            [new(LearnerResponseKind.Choose, "FinalAssessment")]);

        ActiveLearningGate[] failing = [.. ActiveLearningStandard.Evaluate(week).Select(f => f.Gate)];

        Assert.Contains(ActiveLearningGate.ActiveLearningInteraction, failing);
        Assert.Contains(ActiveLearningGate.ActiveLearnerResponse, failing);
    }

    [Fact]
    public void Rule_MindMapAlone_CannotSatisfyTheVisualGate_UnlessItRepresentsTheCentralStructure()
    {
        VisualModelDeclaration ordinaryMap = new(VisualModelKind.MindMap, "ComponentId=\"week4-mindmap-preview\"", RepresentsCentralStructure: false);
        VisualModelDeclaration centralMap = new(VisualModelKind.MindMap, "ComponentId=\"week4-mindmap-preview\"", RepresentsCentralStructure: true);

        Assert.Contains(ActiveLearningStandard.Evaluate(Synthetic([ordinaryMap], [ValidInteraction], [ValidResponse])),
            f => f.Gate == ActiveLearningGate.VisualLearningModel);
        Assert.DoesNotContain(ActiveLearningStandard.Evaluate(Synthetic([centralMap], [ValidInteraction], [ValidResponse])),
            f => f.Gate == ActiveLearningGate.VisualLearningModel);
    }

    [Fact]
    public void Rule_AMindMapCannotBeDisguisedAsAnotherVisualKind_NorAnotherMarkerAsAMindMap()
    {
        var disguised = new VisualModelDeclaration(VisualModelKind.Process, "ComponentId=\"week4-mindmap-preview\"");
        var wrongMarker = new VisualModelDeclaration(VisualModelKind.MindMap, "guided-practice-sequence", RepresentsCentralStructure: true);

        Assert.Contains(ActiveLearningStandard.Evaluate(Synthetic([disguised], [ValidInteraction], [ValidResponse])),
            f => f.Gate == ActiveLearningGate.VisualLearningModel);
        Assert.Contains(ActiveLearningStandard.Evaluate(Synthetic([wrongMarker], [ValidInteraction], [ValidResponse])),
            f => f.Gate == ActiveLearningGate.VisualLearningModel);
    }

    [Theory]
    [InlineData("<ul")]
    [InlineData("<ol")]
    [InlineData("<table")]
    [InlineData("<details")]
    public void Rule_PlainListsTablesAndDetails_AreNotAVisualLearningModel(string marker)
    {
        var week = Synthetic([new(VisualModelKind.Comparison, marker)], [ValidInteraction], [ValidResponse]);

        Assert.Contains(ActiveLearningStandard.Evaluate(week), f => f.Gate == ActiveLearningGate.VisualLearningModel);
    }

    [Theory]
    [InlineData("ProgressiveExplanation")]
    [InlineData("WhatIfBox")]
    [InlineData("CategorizationCheck")]
    [InlineData("InterpretationExample")]
    [InlineData("ResearchTurnStepper")]
    [InlineData("CognitiveHierarchyExplorer")]
    [InlineData("SocraticDialogueExplorer")]
    [InlineData("ConceptGraph")]
    [InlineData("MindMapBranch")]
    [InlineData("WeekCompletionControl")]
    public void Rule_RevealsAccordionsExplorersAndMaps_CannotSatisfyInteractionOrResponse(string component)
    {
        var week = Synthetic(
            [ValidVisual],
            [new(InteractionFamily.InteractiveModel, component)],
            [new(LearnerResponseKind.Choose, component)]);

        ActiveLearningGate[] failing = [.. ActiveLearningStandard.Evaluate(week).Select(f => f.Gate)];

        Assert.Contains(ActiveLearningGate.ActiveLearningInteraction, failing);
        Assert.Contains(ActiveLearningGate.ActiveLearnerResponse, failing);
    }

    [Fact]
    public void Rule_UnclassifiedComponents_AndUnsupportedResponseKinds_AreRejected()
    {
        Assert.Contains(
            ActiveLearningStandard.Evaluate(Synthetic([ValidVisual], [new(InteractionFamily.Simulator, "SomeNewIsland")], [ValidResponse])),
            f => f.Gate == ActiveLearningGate.ActiveLearningInteraction);

        // ScenarioSimulator matches/orders/branches; it does not "manipulate a model".
        Assert.Contains(
            ActiveLearningStandard.Evaluate(Synthetic([ValidVisual], [ValidInteraction], [new(LearnerResponseKind.ManipulateModel, "ScenarioSimulator")])),
            f => f.Gate == ActiveLearningGate.ActiveLearnerResponse);
    }

    [Theory]
    [InlineData(CurriculumSafetyLevel.PublicCore)]
    [InlineData(CurriculumSafetyLevel.PublicWithAdaptation)]
    [InlineData(CurriculumSafetyLevel.AcademicContextOnly)]
    [InlineData(CurriculumSafetyLevel.ProfessionalReviewRequired)]
    [InlineData(CurriculumSafetyLevel.NotEligibleForSelfGuidedSimulator)]
    public void Rule_NoSafetyTier_ExemptsAWeekWithoutInteraction(CurriculumSafetyLevel safetyLevel)
    {
        // Evaluate() is safety-blind, so a passive-only week fails identically under every tier.
        _ = safetyLevel;
        var passiveOnly = Synthetic([ValidVisual]);

        ActiveLearningGate[] failing = [.. ActiveLearningStandard.Evaluate(passiveOnly).Select(f => f.Gate)];

        Assert.Contains(ActiveLearningGate.ActiveLearningInteraction, failing);
        Assert.Contains(ActiveLearningGate.ActiveLearnerResponse, failing);
    }

    [Fact]
    public void Rule_CheckStatus_RejectsFalseCompliance_AndStaleMigration()
    {
        var falselyCompliant = Synthetic([ValidVisual]) with { Status = StructuralStatus.Compliant };
        var staleMigration = Synthetic([ValidVisual], [ValidInteraction], [ValidResponse]);

        Assert.NotNull(ActiveLearningStandard.CheckStatus(falselyCompliant));
        Assert.NotNull(ActiveLearningStandard.CheckStatus(staleMigration));
        Assert.Null(ActiveLearningStandard.CheckStatus(staleMigration with { Status = StructuralStatus.Compliant }));
    }

    // ---------------------------------------------------------------- legacy rules that encode passivity

    [Fact]
    public void LegacyPassivityAssertions_AreAllInventoried_AndTheInventoryHasNoStaleEntries()
    {
        Dictionary<(string File, string Method), SortedSet<string>> found = LegacyAssertionScanner.Scan();
        Dictionary<(string File, string Method), SortedSet<string>> inventoried = LegacyPassivityInventory.Entries
            .ToDictionary(e => (e.TestFile, e.TestMethod), e => new SortedSet<string>(e.Targets, StringComparer.Ordinal));

        string[] uninventoried = [.. found
            .Where(f => !inventoried.TryGetValue(f.Key, out SortedSet<string>? known) || !known.SetEquals(f.Value))
            .Select(f => $"{f.Key.File}::{f.Key.Method} forbids [{string.Join(", ", f.Value)}]")];
        string[] stale = [.. inventoried
            .Where(i => !found.TryGetValue(i.Key, out SortedSet<string>? actual) || !actual.SetEquals(i.Value))
            .Select(i => $"{i.Key.File}::{i.Key.Method}")];

        Assert.True(uninventoried.Length == 0,
            "Assertions that forbid a learning-layer component/format must be classified in LegacyPassivityInventory " +
            $"(they encode the retired 'interaction only where the topic allows' rule):{Environment.NewLine}{string.Join(Environment.NewLine, uninventoried)}");
        Assert.True(stale.Length == 0,
            $"Inventory entries no longer match the test sources (remove or update them):{Environment.NewLine}{string.Join(Environment.NewLine, stale)}");
    }

    [Fact]
    public void ACompliantWeek_CarriesNoReplaceOnRemediationAssertion()
    {
        foreach (LegacyPassivityAssertion entry in LegacyPassivityInventory.Entries.Where(e => e.Kind == LegacyPassivityKind.ReplaceOnRemediation))
        {
            Assert.True(
                ActiveLearningCatalog.For(entry.Week).Status == StructuralStatus.StructuralEnrichmentRequired,
                $"Week {entry.Week} is marked Compliant but {entry.TestFile}::{entry.TestMethod} still forbids learning-layer components — replace it in the same change.");
        }
    }

    [Fact]
    public void LegacyInventory_NeverTouchesTheCompliantReferenceWeeks()
    {
        Assert.DoesNotContain(LegacyPassivityInventory.Entries, e => e.Week is 3 or 6 or 8);
    }
}

/// <summary>Reads a week page's markup the way the gate needs it: Razor comments (which cite component and CSS names in
/// their headers) and the @code block removed, so a declaration can only be satisfied by real markup.</summary>
internal static class WeekPageMarkup
{
    public static string Read(int week)
    {
        string path = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages", $"Sedmica{week}.razor");
        string source = Regex.Replace(File.ReadAllText(path), @"@\*.*?\*@", string.Empty, RegexOptions.Singleline);
        int code = source.LastIndexOf("\n@code", StringComparison.Ordinal);
        return code >= 0 ? source[..code] : source;
    }
}

/// <summary>Finds every test assertion that forbids a learning-layer component (DoesNotContain "&lt;Tag", or a
/// forbiddenNewComponents array) or an interactive format label.</summary>
internal static class LegacyAssertionScanner
{
    private static readonly Regex MethodName = new(@"public\s+(?:async\s+)?(?:void|Task)\s+(\w+)\s*\(", RegexOptions.Compiled);
    private static readonly Regex DoesNotContainTag = new(@"DoesNotContain\(""<(\w+)""", RegexOptions.Compiled);
    private static readonly Regex QuotedTag = new(@"""<(\w+)""", RegexOptions.Compiled);
    private static readonly Regex DoesNotContainFormat = new(@"DoesNotContain\(InteractiveFormat\.(Simulator|InteractiveModel)\b", RegexOptions.Compiled);

    // Assessment, completion, site chrome and plain disclosure are not part of the interactive/visual learning layer;
    // a test may legitimately keep FinalAssessment off the hub pages, for example.
    private static readonly HashSet<string> NotLearningLayer =
        ["FinalAssessment", "WeekCompletionControl", "ThemeToggle", "ProgressiveExplanation"];

    private static readonly HashSet<string> LearningLayerTags =
    [
        .. ActiveLearningStandard.Components.Keys.Where(k => !NotLearningLayer.Contains(k)),
        .. ActiveLearningStandard.VisualConstructs.Keys.Where(k => k.StartsWith('<')).Select(k => k[1..]),
        "SourceArtifact"
    ];

    // The gate's own files mention these names in synthetic rule tests and the inventory itself.
    private static readonly HashSet<string> Excluded = ["ActiveLearningGateTests.cs", "LegacyPassivityInventory.cs"];

    public static Dictionary<(string File, string Method), SortedSet<string>> Scan()
    {
        string testsDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Tests");
        Dictionary<(string, string), SortedSet<string>> result = [];

        foreach (string path in Directory.GetFiles(testsDirectory, "*.cs", SearchOption.TopDirectoryOnly))
        {
            string file = Path.GetFileName(path);
            if (Excluded.Contains(file)) continue;

            string method = "?";
            bool inForbiddenArray = false;
            foreach (string line in File.ReadAllLines(path))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("//", StringComparison.Ordinal)) continue;

                Match m = MethodName.Match(line);
                if (m.Success) method = m.Groups[1].Value;
                if (trimmed.Contains("forbiddenNewComponents =", StringComparison.Ordinal)) inForbiddenArray = true;

                void Record(string target)
                {
                    if (!result.TryGetValue((file, method), out SortedSet<string>? set))
                    {
                        result[(file, method)] = set = new SortedSet<string>(StringComparer.Ordinal);
                    }
                    set.Add(target);
                }

                Match tag = DoesNotContainTag.Match(line);
                if (tag.Success && LearningLayerTags.Contains(tag.Groups[1].Value)) Record("<" + tag.Groups[1].Value);

                if (inForbiddenArray)
                {
                    foreach (Match q in QuotedTag.Matches(line))
                    {
                        if (LearningLayerTags.Contains(q.Groups[1].Value)) Record("<" + q.Groups[1].Value);
                    }
                    if (trimmed.Contains("];", StringComparison.Ordinal)) inForbiddenArray = false;
                }

                Match format = DoesNotContainFormat.Match(line);
                if (format.Success) Record("InteractiveFormat." + format.Groups[1].Value);
            }
        }

        return result;
    }
}
