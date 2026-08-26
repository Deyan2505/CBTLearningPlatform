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
    public void Week6Page_MindMapHasExpectedPrimaryBranches()
    {
        // Mind Map Visual Standard correction pass: a real hierarchy, not a flat root+children list.
        string source = ReadPage("Sedmica6.razor");

        foreach (string branchId in new[] { "\"goals\"", "\"nachalo\"", "\"sreda\"", "\"kray\"", "\"gavkavost\"" })
        {
            Assert.Contains(branchId, source);
        }

        // Each non-leaf primary branch has at least one child pointing back to it as ParentId.
        foreach (string parentId in new[] { "\"nachalo\"", "\"sreda\"", "\"kray\"", "\"gavkavost\"" })
        {
            Assert.Contains($", {parentId},", source);
        }
    }

    [Fact]
    public void Week6Page_GavkavostBranchMirrorsExistingDecisionBranchLeaves()
    {
        // Reuses the already-approved .decision-branch content as Mind Map children instead of
        // duplicating new text — same three reasons, same wording basis.
        string source = ReadPage("Sedmica6.razor");

        Assert.Contains("Риск за пациента или други", source);
        Assert.Contains("Силно емоционално претоварване", source);
        Assert.Contains("Риск за терапевтичния алианс", source);
    }

    [Fact]
    public void Week6Page_MindMapHasNoSectionNumberNodeLabels()
    {
        string source = ReadPage("Sedmica6.razor");
        int mindMapStart = source.IndexOf("BuildWeek6MindMap()", StringComparison.Ordinal);
        int mindMapEnd = source.IndexOf("private static ConceptMapModel", StringComparison.Ordinal);
        string mindMapSection = source[mindMapStart..mindMapEnd];

        Assert.DoesNotContain("\"6.0\"", mindMapSection);
        Assert.DoesNotContain("\"6.1\"", mindMapSection);
        Assert.DoesNotContain("\"6.12\"", mindMapSection);
    }

    [Fact]
    public void ConceptGraphComponent_MindMapIsARealTree_NotACardGrid()
    {
        string source = ReadPublicMarkup("ConceptGraph.razor");

        Assert.DoesNotContain("concept-graph__cluster-grid", source);
        Assert.Contains("<MindMapBranch", source);
        Assert.Contains("mindmap-tree", source);
        Assert.Contains("mindmap-root", source);
    }

    [Fact]
    public void MindMapBranchComponent_UsesNativeDisclosure_CollapsedByDefault()
    {
        string source = ReadPublicMarkup("MindMapBranch.razor");

        Assert.Contains("<details class=\"mindmap-branch\">", source); // no `open` attribute — collapsed by default
        Assert.DoesNotContain("open=\"true\"", source);
        Assert.DoesNotContain("open=\"@true\"", source);
        Assert.Contains("<summary", source);
        Assert.Contains("mindmap-branch__chevron", source);
    }

    [Fact]
    public void MindMapBranchComponent_SeparatesToggleFromNavigation()
    {
        // Visual correction pass 2 (§9-§12): the label+chevron toggle expand/collapse; the compact
        // "→" goto affordance is a distinct, separately-classed control (.mindmap-branch__goto),
        // not merged into the label itself — clicking it still navigates correctly even though it
        // sits inside <summary>, since the browser is about to leave the page regardless of the
        // incidental toggle. A leaf node (nothing to expand) is unambiguous: the whole node IS the
        // link, with no separate toggle to confuse it with.
        string source = ReadPublicMarkup("MindMapBranch.razor");

        int summaryStart = source.IndexOf("<summary", StringComparison.Ordinal);
        int summaryEnd = source.IndexOf("</summary>", StringComparison.Ordinal);
        string summaryBlock = source[summaryStart..summaryEnd];

        Assert.Contains("mindmap-branch__chevron", summaryBlock);
        Assert.Contains("concept-graph__node-label", summaryBlock);
        Assert.Contains("mindmap-branch__goto", summaryBlock);

        int labelIndex = summaryBlock.IndexOf("concept-graph__node-label", StringComparison.Ordinal);
        int gotoIndex = summaryBlock.IndexOf("mindmap-branch__goto", StringComparison.Ordinal);
        Assert.True(labelIndex < gotoIndex, "The goto link must be a distinct element after the label, not merged into it.");

        Assert.Contains("<a class=\"mindmap-node", source); // leaf-with-anchor: the whole node is the link
    }

    [Fact]
    public void MindMapBranchComponent_IsDomainIgnorant()
    {
        string source = ReadPublicMarkup("MindMapBranch.razor");

        foreach (string forbidden in new[] { "Week6", "Седмица 6", "Ирина", "автоматична мисъл", "core belief" })
        {
            Assert.DoesNotContain(forbidden, source, StringComparison.OrdinalIgnoreCase);
        }
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

    [Fact]
    public void MindMap_HasDesktopSpatialLayout_DrivenByTheSameMarkup()
    {
        // Owner visual correction pass 2: a wide-container rule reflows the identical
        // <details>/<ul> tree from a vertical outline into left-to-right spatial branching — same
        // GraphRenderModel, same MindMapBranch markup, no second component/dataset (§3 of the
        // correction). Container-query-first with a @supports fallback, matching the
        // .guided-practice-sequence convention already established on this project.
        string css = ReadCss();

        Assert.Contains("@container (min-width: 700px)", css);
        Assert.Contains("@supports not (container-type: inline-size)", css);

        int containerRuleStart = css.IndexOf("@container (min-width: 700px)", StringComparison.Ordinal);
        int containerRuleRegionEnd = Math.Min(containerRuleStart + 2500, css.Length);
        string containerRuleBody = css[containerRuleStart..containerRuleRegionEnd];

        Assert.Contains(".mindmap-tree", containerRuleBody);
        Assert.Contains("flex-direction: row", containerRuleBody);
        Assert.Contains(".mindmap-branch {", containerRuleBody);
    }

    [Fact]
    public void MindMapNode_HasNoInlineSubDescription_CompactLabelOnly()
    {
        // §9-§11 of the correction pass: nodes are compact — label only, no repeated inline
        // definition text or full-sentence "Виж секцията →" CTA duplicated in every node.
        string source = ReadPublicMarkup("MindMapBranch.razor");

        Assert.DoesNotContain("concept-graph__node-definition", source);
        Assert.DoesNotContain("Виж секцията", source);
        Assert.DoesNotContain("ShortDefinition", source);
    }

    [Fact]
    public void MindMap_NavigationAffordancesAreSecondaryUntilInteraction()
    {
        // Owner polish pass 3 (§9-§10): the concept label reads first; the link chrome (persistent
        // underline, full opacity) only appears on hover/focus — never removed entirely, so
        // keyboard/focus users can still always tell a node is navigable (§10, §18).
        string css = ReadCss();

        Assert.Contains(".mindmap-branch__goto {", css);
        Assert.Contains("text-decoration: none;", css);
        Assert.Contains(".mindmap-branch__goto:hover,\n.mindmap-branch__goto:focus-visible {", css);
        Assert.Contains("a.mindmap-node:hover .concept-graph__node-label,\na.mindmap-node:focus-visible .concept-graph__node-label {", css);
    }

    [Fact]
    public void MindMap_PrimaryBranchesHaveDistinctSurface_FromChildConcepts()
    {
        // §7: one consistent, calm surface/border treatment for the whole depth-1 tier (Цели/
        // Начало/Среда/Край/Гъвкавост) — not a different color per branch — so a primary branch
        // reads as its own territory before a learner looks at its children.
        string css = ReadCss();

        int depth1Start = css.IndexOf(".mindmap-node--depth-1.mindmap-node,", StringComparison.Ordinal);
        int depth1End = css.IndexOf(".mindmap-node--depth-2.mindmap-node,", StringComparison.Ordinal);
        string depth1Rule = css[depth1Start..depth1End];

        Assert.Contains("border-width: 2px;", depth1Rule);
        Assert.Contains("background: var(--color-surface);", depth1Rule);
    }

    [Fact]
    public void MindMap_TopLevelTrunkIsThinnerThanEachBranchsOwnElbow()
    {
        // §4/§6: the shared root->branches trunk is deliberately de-emphasized (thin, quiet guide),
        // while each branch's own curved connector is bolder — so branches read as radiating their
        // own line from root, not as entries pinned to one dominant vertical rail.
        string css = ReadCss();

        Assert.Contains("border-left: 1px solid var(--color-border);", css); // the trunk
        Assert.Contains("border-bottom-left-radius: 24px;", css); // each branch's own, gentler elbow
    }

    [Fact]
    public void Week6Page_GoalsClusterRemainsALeaf_SourceFidelityDocumented()
    {
        // §11 of the polish pass: the source states "six goals" but only ever enumerates them as
        // one continuous, semicolon-separated sentence (one clause bundles multiple sub-aims) — not
        // as six independently-labeled concepts. Splitting it into 6 child nodes would require
        // inventing short labels the approved text never gives them, so "Цели на сесията" stays
        // Option A (a leaf), not Option B (a branch) — documented here as a regression guard.
        string source = ReadPage("Sedmica6.razor");

        int goalsNodeIndex = source.IndexOf("new(\"goals\",", StringComparison.Ordinal);
        Assert.True(goalsNodeIndex >= 0, "The 'goals' MindMap node should still exist.");

        Assert.DoesNotContain(", \"goals\",", source); // no node declares ParentId == "goals"
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

    private static string ReadComponent(string fileName)
    {
        string sharedDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Shared");
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

    private static string ReadCss()
    {
        string wwwrootDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "wwwroot");
        return File.ReadAllText(Path.Combine(wwwrootDirectory, "app.css"));
    }
}
