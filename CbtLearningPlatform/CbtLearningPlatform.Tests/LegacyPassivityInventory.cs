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
        // Weeks 1, 2 and 10 (Phase 2, Batch 1), Weeks 5, 7 and 9 (Phase 2, Batch 2), Week 4, Week 11, Week 12,
        // Week 13, Week 14 and Week 15 were remediated: their Replace entries were removed together with the
        // assertions themselves, which now assert the catalog gate instead
        // (Week1/2/4/5/7/9/10/11/12/13/14/15ContentSliceTests + ActiveLearningBatch1Tests / ActiveLearningBatch2Tests).
        // No ReplaceOnRemediation entries remain — every routed week is Compliant.

        new(10, "Week10ContentSliceTests.cs", "Week8Page_CrossLinksToWeek10_WithoutDuplicatingTheSimulator",
            new HashSet<string> { "<SocraticDialogueExplorer" }, LegacyPassivityKind.KeepCrossWeekScopeGuard,
            "Asserts the LOCKED Week 8 page did not absorb Week 10's explorer. Compatible."),

        new(13, "Week13ContentSliceTests.cs", "Week13_FormatIsNeverSimulator",
            new HashSet<string> { "InteractiveFormat.Simulator" }, LegacyPassivityKind.KeepSafetyLabelGuard,
            "NotEligibleForSelfGuidedSimulator must not carry a self-guided 'Simulator' label. Interaction is still required in a safety-adapted form."),

        new(14, "Week14ContentSliceTests.cs", "Week14_FormatIsInteractiveModelNeverStaticVisualization",
            new HashSet<string> { "InteractiveFormat.Simulator" }, LegacyPassivityKind.KeepSafetyLabelGuard,
            "ProfessionalReviewRequired must not carry a self-guided 'Simulator' label. Interaction is still required in a safety-adapted form."),

        new(15, "Week15ContentSliceTests.cs", "Week15_FormatIsAcademicOnlyNeverInteractiveOrSimulator",
            new HashSet<string> { "InteractiveFormat.Simulator" }, LegacyPassivityKind.KeepSafetyLabelGuard,
            "AcademicContextOnly must not carry a self-guided 'Simulator' label. AcademicOnly stays the course format; interaction is still required in a safety-adapted (AcademicThirdPerson) form.")
    ];
}
