namespace CbtLearningPlatform.Tests;

/// <summary>How a legacy assertion that forbids a learning-layer component is to be handled.</summary>
internal enum LegacyPassivityKind
{
    /// <summary>Encodes "this week must NOT have a reusable interactive/visual component". Contradicts the Active
    /// Learning Standard. Replace — never merely delete — during the week's controlled remediation with an assertion
    /// that the week's ActiveLearningCatalog declarations are present and verified.</summary>
    ReplaceOnRemediation,

    /// <summary>Forbids a format LABEL that contradicts the week's safety tier (self-guided "Simulator" on a week that is
    /// not eligible for self-guided simulation). Compatible with the standard — safety changes the FORM of interaction —
    /// so it stays until the format vocabulary itself is revisited.</summary>
    KeepSafetyLabelGuard,

    /// <summary>Asserts that ANOTHER week's locked page did not absorb this week's component. Compatible; keep.</summary>
    KeepCrossWeekScopeGuard
}

internal sealed record LegacyPassivityAssertion(
    int Week,
    string TestFile,
    string TestMethod,
    IReadOnlySet<string> Targets,
    LegacyPassivityKind Kind,
    string Note);

/// <summary>Migration inventory of every existing test assertion that forbids a reusable learning-layer component (or an
/// interactive format) in a week page. They were written under the older rule "interaction only where the topic allows"
/// and are NOT removed blindly: each is classified here and replaced during that week's remediation. The gate
/// (ActiveLearningGateTests) fails on any such assertion that is not inventoried, on any inventory entry that no longer
/// exists, and on a Compliant week that still carries a ReplaceOnRemediation entry.</summary>
internal static class LegacyPassivityInventory
{
    private static readonly string[] Interactives =
        ["<CbtChainSimulator", "<CategorizationCheck", "<InterpretationExample"];

    private static LegacyPassivityAssertion Replace(int week, string file, string method, string note, params string[] targets) =>
        new(week, file, method, new HashSet<string>(targets), LegacyPassivityKind.ReplaceOnRemediation, note);

    public static IReadOnlyList<LegacyPassivityAssertion> Entries { get; } =
    [
        Replace(1, "Week1ContentSliceTests.cs", "Week1Page_HasTheResearchTurnStepperAsItsOnlyInteractiveIsland",
            "Encodes 'exactly one interactive island'. Replace with the Week 1 catalog gate.", Interactives),

        Replace(2, "Week2ContentSliceTests.cs", "Week2Page_UsesOnlyExistingReusablePatterns_NoNewComponent",
            "Encodes 'zero interactivity' and forbids ConceptGraph/HistoricalTimeline. Replace with the Week 2 catalog gate.",
            "<CbtChainSimulator", "<CategorizationCheck", "<InterpretationExample", "<ResearchTurnStepper",
            "<SocraticDialogueExplorer", "<SchemaFilterDemonstration", "<ConceptGraph", "<HistoricalTimeline"),

        Replace(4, "Week4ContentSliceTests.cs", "Week4Page_UsesOnlyExistingReusablePatterns_NoNewComponent",
            "Encodes AcademicContextOnly = zero interaction. Replace with the Week 4 catalog gate; keep the MindMapBranch (internal component) guard.",
            "<CbtChainSimulator", "<CategorizationCheck", "<InterpretationExample", "<ResearchTurnStepper",
            "<SocraticDialogueExplorer", "<SchemaFilterDemonstration", "<HistoricalTimeline", "<MindMapBranch"),

        Replace(5, "Week5ContentSliceTests.cs", "Week5Page_UsesEstablishedReusablePatterns_ZeroNewComponents",
            "Forbids the generic ScenarioSimulator. Replace with the Week 5 catalog gate.",
            "<ScenarioSimulator", "<SourceArtifact", "<CbtChainSimulator"),

        Replace(7, "Week7ContentSliceTests.cs", "Week7Page_UsesEstablishedReusablePatterns_ZeroNewComponents",
            "Forbids the generic ScenarioSimulator. Replace with the Week 7 catalog gate.",
            "<ScenarioSimulator", "<SourceArtifact", "<CbtChainSimulator"),

        Replace(9, "Week9ContentSliceTests.cs", "Week9Page_UsesEstablishedReusablePatterns_ZeroNewComponents",
            "Forbids ScenarioSimulator and WhatIfBox. Replace with the Week 9 catalog gate; the fixed (non-fillable) Thought Record decision is separate and stays.",
            "<ScenarioSimulator", "<SourceArtifact", "<CbtChainSimulator", "<WhatIfBox"),

        Replace(10, "Week10ContentSliceTests.cs", "Week10Page_HasExactlyOneInteractiveIsland",
            "Encodes 'exactly one island' and forbids reuse of the other islands. Replace with the Week 10 catalog gate.",
            "<CbtChainSimulator", "<CognitiveHierarchyExplorer", "<SchemaFilterDemonstration", "<ResearchTurnStepper"),

        new(10, "Week10ContentSliceTests.cs", "Week8Page_CrossLinksToWeek10_WithoutDuplicatingTheSimulator",
            new HashSet<string> { "<SocraticDialogueExplorer" }, LegacyPassivityKind.KeepCrossWeekScopeGuard,
            "Asserts the LOCKED Week 8 page did not absorb Week 10's explorer. Compatible."),

        new(13, "Week13ContentSliceTests.cs", "Week13_FormatIsNeverSimulator",
            new HashSet<string> { "InteractiveFormat.Simulator" }, LegacyPassivityKind.KeepSafetyLabelGuard,
            "NotEligibleForSelfGuidedSimulator must not carry a self-guided 'Simulator' label. Interaction is still required in a safety-adapted form."),

        Replace(13, "Week13ContentSliceTests.cs", "Week13Page_UsesOnlyExistingReusablePatterns",
            "Comment says CategorizationCheck-style checks 'use the native <details> reveal instead' — a plain reveal does not satisfy the standard. Replace with the Week 13 catalog gate.",
            Interactives),

        new(14, "Week14ContentSliceTests.cs", "Week14_FormatIsInteractiveModelNeverStaticVisualization",
            new HashSet<string> { "InteractiveFormat.Simulator" }, LegacyPassivityKind.KeepSafetyLabelGuard,
            "ProfessionalReviewRequired must not carry a self-guided 'Simulator' label. The InteractiveModel label is currently unearned until Week 14 is enriched."),

        Replace(14, "Week14ContentSliceTests.cs", "Week14Page_UsesOnlyExistingReusablePatterns",
            "Replace with the Week 14 catalog gate.", Interactives),

        Replace(15, "Week15ContentSliceTests.cs", "Week15_FormatIsAcademicOnlyNeverInteractiveOrSimulator",
            "Encodes 'AcademicOnly means never interactive'. Replace with the Week 15 catalog gate; keep the no-self-guided-Simulator label rule.",
            "InteractiveFormat.InteractiveModel", "InteractiveFormat.Simulator"),

        Replace(15, "Week15ContentSliceTests.cs", "Week15Page_UsesOnlyExistingReusablePatterns",
            "Replace with the Week 15 catalog gate. The SourceArtifact ban has a content reason (no reproducible figure) that must be re-judged against the visual-model requirement.",
            "<CbtChainSimulator", "<InterpretationExample", "<CategorizationCheck", "<SourceArtifact")
    ];
}
