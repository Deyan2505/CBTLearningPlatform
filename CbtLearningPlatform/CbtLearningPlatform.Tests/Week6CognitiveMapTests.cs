namespace CbtLearningPlatform.Tests;

/// <summary>Cognitive Learning Architecture v1.1 reference implementation on Week 6
/// (COGNITIVE_LEARNING_ARCHITECTURE_v1.md, Phases 1-3) — Weekly Mind Map (Preview + Review), the
/// upgraded Concept Map, the Ирина Case Conceptualization Map, and the retrieval-practice
/// enhancements. String-based source checks, matching this project's existing test style (no bUnit
/// in this project — CbtLearningPlatform.Tests.csproj has no such reference).</summary>
public sealed class Week6CognitiveMapTests
{
    [Fact]
    public void Week6Page_HasPreviewMindMap()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("ComponentId=\"week6-mindmap-preview\"", source);
    }

    [Fact]
    public void Week6Page_HasReviewMindMap_GatedByAttemptBeforeReveal()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("ComponentId=\"week6-mindmap-review\"", source);

        int summaryIndex = source.IndexOf("опитай да си спомниш, преди да разгънеш", StringComparison.OrdinalIgnoreCase);
        int mapIndex = source.IndexOf("ComponentId=\"week6-mindmap-review\"", StringComparison.Ordinal);

        Assert.True(summaryIndex >= 0 && summaryIndex < mapIndex,
            "Review Mind Map must be preceded by an attempt-before-reveal prompt (architecture v1.1 §26).");
    }

    [Fact]
    public void Week6Page_PreviewAndReviewMindMap_ShareTheSameUnderlyingModel()
    {
        // Architecture v1.1 §4/§10: Preview and Review are two states of ONE knowledge structure,
        // not two independently-maintained maps.
        string source = ReadPage("Sedmica6.razor");

        int bindingCount = CountOccurrences(source, "Model=\"@_week6MindMapRender\"");

        Assert.Equal(2, bindingCount);
    }

    [Fact]
    public void Week6Page_ConceptMapContainsFullBeckModelAsCrossLinkedNodes()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("ComponentId=\"week6-concept-map\"", source);
        Assert.DoesNotContain("class=\"concept-map__flow\" aria-label=\"Пример: обяд с колеги\"", source);

        // Primary nodes actually elaborated in this week's own worked example.
        foreach (string label in new[] { "\"situation\"", "\"thought\"", "\"emotion\"" })
        {
            Assert.Contains(label, source);
        }

        // Cross-reference nodes: pointers to where the fuller model already lives, not invented content.
        Assert.Contains("/kurs/sedmica-8#karta-na-temata", source);
        Assert.Contains("/kurs/sedmica-3", source);
    }

    [Fact]
    public void Week6Page_HasIrinaCaseConceptualizationMap()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("ComponentId=\"irina-case-map\"", source);
        Assert.Contains("_irinaCaseMapRender", source);
    }

    [Fact]
    public void Week6Page_HasAtLeastTwoRetrievalPracticeOpportunities()
    {
        string source = ReadPage("Sedmica6.razor");

        int count = CountOccurrences(source, "Retrieval practice");
        Assert.True(count >= 2, $"Expected at least 2 retrieval-practice prompts, found {count}.");
        Assert.Contains("подреди без да гледаш назад", source);
    }

    [Fact]
    public void Week6Page_SourceCoverageClaimIsUnchanged()
    {
        // Regression guard: the cognitive-map layer must not touch the 47/47 source-coverage claim.
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("100% accounted for", source);
        Assert.Contains("Included: 47 / Deferred: 0 / Excluded: 0 /", source);
    }

    [Fact]
    public void Week6Page_AssessmentAndSimulatorRemainIntact()
    {
        string source = ReadPage("Sedmica6.razor");

        for (int i = 1; i <= 20; i++)
        {
            Assert.Contains($"<strong>Q{i:D2}", source);
        }

        Assert.Contains("<ScenarioSimulator", source);
        Assert.Contains("6.9 · Интерактивен симулатор", source);
    }

    [Fact]
    public void Week6Page_HasNoBareFragmentAnchors()
    {
        string source = ReadPage("Sedmica6.razor");

        Assert.DoesNotContain("href=\"#", source);
    }

    [Fact]
    public void ConceptGraphComponent_IsStaticSsrByDefault()
    {
        // Strip the leading @* ... *@ dev comment first — it explains the design decision using the
        // literal token "@rendermode" in prose, which must not be confused with an actual directive.
        string source = ReadPublicMarkup("ConceptGraph.razor");

        Assert.DoesNotContain("@rendermode", source);
    }

    [Fact]
    public void ConceptGraphComponent_RequiresModelAndComponentIdParameters()
    {
        string source = ReadComponent("ConceptGraph.razor");

        Assert.Equal(2, CountOccurrences(source, "[Parameter, EditorRequired]"));
        Assert.Contains("public GraphRenderModel Model", source);
        Assert.Contains("public string ComponentId", source);
    }

    [Fact]
    public void ConceptGraphComponent_HasAccessibleFallback_DerivedFromTheSameModel()
    {
        string source = ReadComponent("ConceptGraph.razor");

        Assert.Contains("class=\"concept-graph__fallback\"", source);
        Assert.Contains("Текстово описание на картата", source);

        // Both the visual canvas and the fallback list read Model.Nodes/Model.Edges directly —
        // no second, hand-authored copy of the content exists to drift out of sync (§13/§19).
        Assert.True(CountOccurrences(source, "Model.Nodes") >= 2);
        Assert.True(CountOccurrences(source, "Model.Edges") >= 2);
    }

    [Fact]
    public void ConceptGraphComponent_DecorativeConnectorsAreAriaHidden_ButEdgeLabelsAreVisibleText()
    {
        string source = ReadComponent("ConceptGraph.razor");

        Assert.Contains("aria-hidden=\"true\"", source);
        Assert.Contains("concept-graph__chain-edge-label", source);
        Assert.Contains("concept-graph__edge-label", source);
    }

    [Fact]
    public void ConceptGraphComponent_UsesAriaLabelledBy_OnTheSectionRoot()
    {
        string source = ReadComponent("ConceptGraph.razor");

        Assert.Contains("aria-labelledby=\"@_titleId\"", source);
    }

    [Fact]
    public void ConceptGraphComponent_SupportsAllThreeReferenceModes()
    {
        string source = ReadComponent("ConceptGraph.razor");

        Assert.Contains("ConceptGraphMode.MindMap", source);
        Assert.Contains("concept-graph__chain", source); // shared ConceptMap/CaseMap rendering path
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
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    private static string ReadComponent(string fileName)
    {
        string sharedDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Shared");
        return File.ReadAllText(Path.Combine(sharedDirectory, fileName));
    }

    /// <summary>Strips the leading @* ... *@ dev comment — same convention as the existing
    /// Week6ContentSliceTests.ReadPublicMarkup — so prose that happens to mention a directive name
    /// isn't mistaken for the directive itself.</summary>
    private static string ReadPublicMarkup(string fileName)
    {
        string source = ReadComponent(fileName);
        int commentEnd = source.IndexOf("*@", StringComparison.Ordinal);
        return commentEnd >= 0 ? source[(commentEnd + 2)..] : source;
    }
}
