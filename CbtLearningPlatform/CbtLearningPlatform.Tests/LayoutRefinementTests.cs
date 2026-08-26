namespace CbtLearningPlatform.Tests;

/// <summary>Covers the owner visual-refinement round (two-column workspace + semantic color
/// system). Deliberately avoids pixel-level assertions — checks structural presence of the
/// grid/color building blocks, not exact rendered geometry.</summary>
public sealed class LayoutRefinementTests
{
    [Fact]
    public void AppCss_DefinesTheLearningGridFoundation()
    {
        string css = ReadCss();

        Assert.Contains(".learning-grid {", css);
        Assert.Contains(".learning-grid--balanced", css);
        Assert.Contains(".learning-grid--content-visual", css);
        Assert.Contains(".learning-grid--controls-output", css);
        Assert.Contains(".learning-grid--wide-narrow", css);
    }

    [Fact]
    public void AppCss_LearningGridIsSingleColumnByDefault_TwoColumnsOnlyOnceContained()
    {
        // Session 21: switched from a viewport max-width breakpoint (blind to the sidebar
        // eating real width) to a container query on .page-container — mobile-first base
        // is 1 column, @container (min-width: 760px) opts into 2 columns once there's real room.
        string css = ReadCss();

        Assert.Contains("grid-template-columns: 1fr;", css);
        Assert.Contains("container-type: inline-size;", css);

        int containerQueryIndex = css.IndexOf("@container (min-width: 760px)", StringComparison.Ordinal);
        Assert.True(containerQueryIndex >= 0, "Expected a container query opting into two columns.");

        int cursor = css.IndexOf('{', containerQueryIndex) + 1;
        int braceDepth = 1;
        while (braceDepth > 0 && cursor < css.Length)
        {
            if (css[cursor] == '{') braceDepth++;
            if (css[cursor] == '}') braceDepth--;
            cursor++;
        }
        string containerBlock = css[containerQueryIndex..cursor];

        Assert.Contains("learning-grid--balanced", containerBlock);
        Assert.Contains("minmax(0, 1fr) minmax(0, 1fr)", containerBlock);
    }

    [Fact]
    public void AppCss_HasAMediaQueryFallbackForBrowsersWithoutContainerQuerySupport()
    {
        string css = ReadCss();

        Assert.Contains("@supports not (container-type: inline-size)", css);
    }

    [Fact]
    public void AppCss_DefinesAllSixSemanticAccentRoles()
    {
        string css = ReadCss();

        foreach (string role in new[] { "primary", "interactive", "theory", "example", "academic", "safety" })
        {
            Assert.Contains($"--accent-{role}:", css);
        }
    }

    [Fact]
    public void AppCss_SemanticAccentRolesAreDefinedForBothThemes()
    {
        string css = ReadCss();

        int lightThemeStart = css.IndexOf(":root[data-theme=\"light\"]", StringComparison.Ordinal);
        Assert.True(lightThemeStart >= 0);

        string lightBlock = css[lightThemeStart..];
        Assert.Contains("--accent-primary:", lightBlock);
        Assert.Contains("--accent-theory:", lightBlock);
        Assert.Contains("--accent-academic:", lightBlock);
    }

    [Fact]
    public void AppCss_DefinesDistinctPrimaryAndInteractiveButtonVariants()
    {
        string css = ReadCss();

        Assert.Contains(".btn-violet", css);
        Assert.Contains(".btn-secondary", css);
        // The two must resolve to different color roles, not the same token.
        Assert.Contains("background: var(--accent-primary);", css);
    }

    [Fact]
    public void DisclaimerCallout_DefaultsToTheCalmEducationalVariant_WithSafetyStillAvailable()
    {
        // Session 23: the general "this platform is educational" statement must not read as
        // an error/danger message — "educational" (calm indigo) is now the default; "safety"
        // (rose) stays defined in app.css, reserved for content that needs it, not deleted.
        string source = ReadSharedComponent("DisclaimerCallout.razor");

        Assert.Contains("callout--@Variant", source);
        Assert.Contains("public string Variant { get; set; } = \"educational\";", source);

        string css = ReadCss();
        Assert.Contains(".callout--educational", css);
        Assert.Contains(".callout--safety", css);
    }

    [Fact]
    public void Week8Page_UsesTwoColumnLearningGridForMultipleRows()
    {
        string source = ReadPage("Sedmica8.razor");

        int gridUsageCount = CountOccurrences(source, "class=\"learning-grid");
        Assert.True(gridUsageCount >= 4, $"Expected at least 4 two-column rows, found {gridUsageCount}.");
    }

    [Fact]
    public void Week8Page_FullExplanationAndAcademicContextShareATwoColumnRow()
    {
        string source = ReadPage("Sedmica8.razor");

        int wideNarrowIndex = source.IndexOf("learning-grid--wide-narrow", StringComparison.Ordinal);
        int fullExplanationIndex = source.IndexOf("Покажи пълното обяснение", StringComparison.Ordinal);
        int academicIndex = source.IndexOf("Академичен контекст", StringComparison.Ordinal);

        Assert.True(wideNarrowIndex >= 0 && wideNarrowIndex < fullExplanationIndex && fullExplanationIndex < academicIndex);
    }

    [Fact]
    public void Week8Page_UsesSectionRoleMarkersForTheoryVisualExampleAndAcademicContent()
    {
        // Session 23: the closing "boundary" section moved from the rose "safety" role to the
        // calm indigo "academic" role (matching DisclaimerCallout's new educational default) —
        // "safety" remains a reserved role in app.css, just not used on this page anymore.
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("section-card--theory", source);
        Assert.Contains("section-card--visual", source);
        Assert.Contains("section-card--example", source);
        Assert.Contains("section-card--academic", source);
    }

    [Fact]
    public void CbtChainSimulator_SplitsControlsAndLiveOutputIntoTwoColumns()
    {
        string source = ReadClientComponent("CbtChainSimulator.razor");

        Assert.Contains("learning-grid--controls-output", source);

        int gridIndex = source.IndexOf("learning-grid--controls-output", StringComparison.Ordinal);
        int panelIndex = source.IndexOf("cbt-simulator__panel", StringComparison.Ordinal);
        int outputIndex = source.IndexOf("cbt-simulator__output", StringComparison.Ordinal);

        Assert.True(gridIndex < panelIndex && panelIndex < outputIndex,
            "Controls must appear before live output in DOM order.");
    }

    [Fact]
    public void KursPage_HasATimelineAndContextualSidePanelSplit()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("learning-grid--wide-narrow", source);
        Assert.Contains("hub-sidebar", source);
    }

    [Fact]
    public void KursPage_DisclaimerRemainsVisibleInTheSidePanel()
    {
        string source = ReadPage("Kurs.razor");

        int sidebarIndex = source.IndexOf("hub-sidebar\">", StringComparison.Ordinal);
        int disclaimerIndex = source.IndexOf("<DisclaimerCallout", StringComparison.Ordinal);

        Assert.True(sidebarIndex >= 0 && disclaimerIndex > sidebarIndex);
    }

    [Fact]
    public void ComparisonMatrix_StillGuardsAgainstHorizontalPageOverflow()
    {
        string css = ReadCss();

        Assert.Contains(".comparison-matrix-wrapper", css);
        Assert.Contains("overflow-x: auto;", css);
    }

    private static int CountOccurrences(string source, string needle)
    {
        int count = 0;
        int index = 0;

        while ((index = source.IndexOf(needle, index, StringComparison.Ordinal)) != -1)
        {
            count++;
            index += needle.Length;
        }

        return count;
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    private static string ReadSharedComponent(string fileName)
    {
        string sharedDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Shared");
        return File.ReadAllText(Path.Combine(sharedDirectory, fileName));
    }

    private static string ReadClientComponent(string fileName)
    {
        string interactiveDirectory = Path.Combine(
            TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Interactive");
        return File.ReadAllText(Path.Combine(interactiveDirectory, fileName));
    }

    private static string ReadCss()
    {
        string cssPath = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "wwwroot", "app.css");
        return File.ReadAllText(cssPath);
    }
}
