using CbtLearningPlatform.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>Phase 5 of COGNITIVE_LEARNING_ARCHITECTURE_v1.md — Course Map and CBT Knowledge Map on
/// `/kurs/karta`. Course Map tests cover derivation from CourseCatalog (no second, hardcoded
/// dataset); Knowledge Map tests cover source-grounding (every concept/relation traceable to a
/// routed week's real content); route tests follow this project's existing string-based style (no
/// bUnit/HTTP integration harness in this test project).</summary>
public sealed class GlobalMapsTests
{
    // ---- Course Map ----

    [Fact]
    public void CourseMap_HasExactlyFourModuleBranches()
    {
        MindMapModel model = CourseMapBuilder.Build();
        var root = model.Nodes.Single(n => n.ParentId is null);
        var moduleBranches = model.Nodes.Where(n => n.ParentId == root.Id).ToList();

        Assert.Equal(CourseCatalog.Modules.Count, moduleBranches.Count);
        Assert.Equal(4, moduleBranches.Count);
    }

    [Fact]
    public void CourseMap_WeekCountMatchesCourseCatalogExactly_NoSecondDataset()
    {
        // If this ever drifts from CourseCatalog.Weeks.Count, the builder has stopped deriving live
        // and started hardcoding a second list.
        MindMapModel model = CourseMapBuilder.Build();
        var root = model.Nodes.Single(n => n.ParentId is null);
        var moduleBranches = model.Nodes.Where(n => n.ParentId == root.Id).Select(n => n.Id).ToHashSet();
        var weekNodes = model.Nodes.Where(n => n.ParentId is not null && moduleBranches.Contains(n.ParentId!)).ToList();

        Assert.Equal(CourseCatalog.Weeks.Count, weekNodes.Count);
        Assert.Equal(15, weekNodes.Count);
    }

    [Fact]
    public void CourseMap_EveryWeekNodeHasCorrectRouteAndCurriculumState()
    {
        MindMapModel model = CourseMapBuilder.Build();

        foreach (CourseWeekDefinition week in CourseCatalog.Weeks)
        {
            MindMapNode node = model.Nodes.Single(n => n.Label.StartsWith($"{week.Number}. ", StringComparison.Ordinal));

            Assert.Equal(week.Route, node.Anchor);
            Assert.Equal(week.Route is not null ? ConceptState.Introduced : ConceptState.Upcoming, node.State);
        }
    }

    [Fact]
    public void CourseMap_WeeksAreGroupedUnderTheirOwnModule()
    {
        MindMapModel model = CourseMapBuilder.Build();
        var byId = model.Nodes.ToDictionary(n => n.Id);

        foreach (CourseWeekDefinition week in CourseCatalog.Weeks)
        {
            MindMapNode weekNode = model.Nodes.Single(n => n.Label.StartsWith($"{week.Number}. ", StringComparison.Ordinal));
            MindMapNode parentModule = byId[weekNode.ParentId!];

            Assert.Equal(week.ModuleLabel, parentModule.Label);
        }
    }

    [Fact]
    public void CourseMap_ProducesAValidHierarchy_NoDanglingReferencesOrCycles()
    {
        // MindMapAdapter throws on a dangling ParentId or a cycle — this alone would fail if it did.
        GraphRenderModel render = MindMapAdapter.ToRenderModel(CourseMapBuilder.Build());

        Assert.True(render.Nodes.Count > 0);
    }

    // ---- CBT Knowledge Map ----

    [Fact]
    public void KnowledgeMap_HasNoDuplicateConceptIds()
    {
        ConceptMapModel model = KnowledgeMapCatalog.Build();

        Assert.Equal(model.Nodes.Count, model.Nodes.Select(n => n.Id).Distinct().Count());
    }

    [Fact]
    public void KnowledgeMap_AllRelationEndpointsReferenceExistingConcepts()
    {
        ConceptMapModel model = KnowledgeMapCatalog.Build();
        var ids = model.Nodes.Select(n => n.Id).ToHashSet();

        foreach (ConceptRelation relation in model.Relations)
        {
            Assert.Contains(relation.FromId, ids);
            Assert.Contains(relation.ToId, ids);
        }
    }

    [Fact]
    public void KnowledgeMap_NoEmptyLabelsOrRelationLabels()
    {
        ConceptMapModel model = KnowledgeMapCatalog.Build();

        Assert.All(model.Nodes, n => Assert.False(string.IsNullOrWhiteSpace(n.Label)));
        Assert.All(model.Relations, r => Assert.False(string.IsNullOrWhiteSpace(r.RelationLabel)));
    }

    [Fact]
    public void KnowledgeMap_IntroducedAndRevisitedWeeksReferenceRealRoutedCurriculumWeeks()
    {
        // §9/§10 of the Phase 5 authorization: every concept must trace to real, already-routed
        // curriculum — never a week number that doesn't exist, and never left unverified against
        // the actual CourseCatalog.
        ConceptMapModel model = KnowledgeMapCatalog.Build();
        var routedWeekNumbers = CourseCatalog.Weeks.Where(w => w.Route is not null).Select(w => w.Number).ToHashSet();

        foreach (ConceptNode node in model.Nodes)
        {
            Assert.NotNull(node.IntroducedWeek);
            Assert.Contains(node.IntroducedWeek!.Value, routedWeekNumbers);

            foreach (int revisited in node.RevisitedWeeks)
            {
                Assert.Contains(revisited, routedWeekNumbers);
                Assert.True(revisited > node.IntroducedWeek.Value, $"'{node.Label}' lists a revisit week ({revisited}) that isn't after its own IntroducedWeek ({node.IntroducedWeek}).");
            }
        }
    }

    [Fact]
    public void KnowledgeMap_HasNoUpcomingConcepts_V1IsFullyGroundedInRoutedContent()
    {
        // §11: not required to have Upcoming nodes in v1 — this map deliberately doesn't, since
        // every concept here is drawn from already-routed, already-approved content.
        GraphRenderModel render = ConceptMapAdapter.ToRenderModel(KnowledgeMapCatalog.Build(), CourseCatalog.Weeks);

        Assert.DoesNotContain(render.Nodes, n => n.DisplayState == ConceptState.Upcoming);
    }

    [Fact]
    public void KnowledgeMap_AnchorsAreRouteSafe_NoBareFragments()
    {
        ConceptMapModel model = KnowledgeMapCatalog.Build();

        Assert.All(model.Nodes, n => Assert.False(n.Anchor is not null && n.Anchor.StartsWith('#'), $"'{n.Label}' has a bare fragment anchor: {n.Anchor}"));
    }

    [Fact]
    public void KnowledgeMap_UsesOnlyCoveredRoutedWeeks_ThreeSixEightTen()
    {
        // Documents the exact, intentionally small v1 scope (§10) — Week 1 contributes no concept
        // here (purely historical/narrative content, nothing reusable as a cross-week concept);
        // Week 12 only ever appears as a Revisited week (core belief), never as an IntroducedWeek.
        ConceptMapModel model = KnowledgeMapCatalog.Build();
        var introducedWeeks = model.Nodes.Select(n => n.IntroducedWeek!.Value).Distinct().OrderBy(w => w).ToList();

        Assert.Equal([3, 6, 8, 10], introducedWeeks);
    }

    // ---- Route / page ----

    [Fact]
    public void KartaPage_IsStaticSsr_NoRenderModeDirective()
    {
        string source = ReadPublicMarkup("Karta.razor");

        Assert.DoesNotContain("@rendermode", source);
    }

    [Fact]
    public void KartaPage_HasTheRoute()
    {
        string source = ReadPage("Karta.razor");

        Assert.Contains("@page \"/kurs/karta\"", source);
    }

    [Fact]
    public void KartaPage_DefaultsToCourseMap_KnowledgeModeIsExplicitOptIn()
    {
        string source = ReadPublicMarkup("Karta.razor");

        Assert.Contains("IsKnowledgeMode => string.Equals(Mode, \"knowledge\"", source);
        Assert.Contains("@if (IsKnowledgeMode)", source);
    }

    [Fact]
    public void KartaPage_HasBothDistinctModeViews()
    {
        string source = ReadPage("Karta.razor");

        Assert.Contains("CBT Knowledge Map", source);
        Assert.Contains("ComponentId=\"cbt-knowledge-map\"", source);
        Assert.Contains("ComponentId=\"course-map\"", source);
    }

    [Fact]
    public void KartaPage_ModeSwitchIsTwoRealLinks_NotAJsToggle()
    {
        string source = ReadPublicMarkup("Karta.razor");

        Assert.Contains("href=\"/kurs/karta?mode=course\"", source);
        Assert.Contains("href=\"/kurs/karta?mode=knowledge\"", source);
        Assert.Contains("aria-current=", source);
    }

    [Fact]
    public void KursHub_LinksToTheGlobalMaps()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("href=\"/kurs/karta\"", source);
    }

    [Fact]
    public void KartaPage_DoesNotDuplicateWeekTitles_DerivesFromCourseCatalogOnly()
    {
        // A second, hardcoded copy of a week's title in Karta.razor itself would indicate the page
        // stopped deriving from CourseMapBuilder/CourseCatalog.
        string source = ReadPage("Karta.razor");

        Assert.DoesNotContain(CourseCatalog.Weeks[0].Title, source);
        Assert.DoesNotContain(CourseCatalog.Weeks[5].Title, source);
    }

    // ---- Knowledge Map Network layout (spatial correction pass) ----

    [Fact]
    public void KnowledgeMap_StillHasExactlyTenConceptsAndTwelveRelations()
    {
        // The correction pass changes only how the map renders — not what it says. No concept or
        // relation may be added "to enrich the map" or removed without new source-backed reason.
        ConceptMapModel model = KnowledgeMapCatalog.Build();

        Assert.Equal(10, model.Nodes.Count);
        Assert.Equal(12, model.Relations.Count);
    }

    [Fact]
    public void KnowledgeMap_EveryConceptHasAPresentationCluster_NoOrphans()
    {
        ConceptMapModel model = KnowledgeMapCatalog.Build();

        foreach (ConceptNode node in model.Nodes)
        {
            Assert.True(KnowledgeMapCatalog.Clusters.ContainsKey(node.Id), $"'{node.Label}' has no presentation cluster assigned.");
        }
    }

    [Fact]
    public void KnowledgeMap_DenseClustersAreGroundedInMutualRelations()
    {
        // "Когнитивна верига" and "Вярвания" are grouped because their members are actually connected
        // to each other by real relations above — not an invented taxonomy layered on top.
        ConceptMapModel model = KnowledgeMapCatalog.Build();
        var byCluster = KnowledgeMapCatalog.Clusters.GroupBy(kv => kv.Value).ToDictionary(g => g.Key, g => g.Select(kv => kv.Key).ToHashSet());

        foreach (string denseCluster in new[] { "Когнитивна верига", "Вярвания" })
        {
            var memberIds = byCluster[denseCluster];
            bool anyMemberConnectsToAnother = model.Relations.Any(r => memberIds.Contains(r.FromId) && memberIds.Contains(r.ToId));
            Assert.True(anyMemberConnectsToAnother, $"Cluster '{denseCluster}' has {memberIds.Count} members but none are connected to each other by a real relation.");
        }
    }

    [Fact]
    public void KnowledgeMap_ProcessClusterMembers_HaveAtMostOneRelationEach()
    {
        // "Терапевтичен процес" groups the two concepts that sit outside the dense chain/beliefs
        // sub-graphs, not because they connect to each other (they don't) but because each has at
        // most one relation in the whole map — exactly why they read as loosely-connected outliers
        // rather than belonging in either dense cluster.
        ConceptMapModel model = KnowledgeMapCatalog.Build();
        var processMembers = KnowledgeMapCatalog.Clusters.Where(kv => kv.Value == "Терапевтичен процес").Select(kv => kv.Key).ToHashSet();

        foreach (string memberId in processMembers)
        {
            int relationCount = model.Relations.Count(r => r.FromId == memberId || r.ToId == memberId);
            Assert.True(relationCount <= 1, $"'{memberId}' has {relationCount} relations — too connected to be a loose outlier.");
        }
    }

    [Fact]
    public void KnowledgeMapRender_UsesNetworkLayout_WithClustersPropagatedOntoRenderNodes()
    {
        GraphRenderModel render = ConceptMapAdapter.ToRenderModel(KnowledgeMapCatalog.Build(), CourseCatalog.Weeks, ConceptGraphLayout.Network, KnowledgeMapCatalog.Clusters);

        Assert.Equal(ConceptGraphLayout.Network, render.Layout);
        Assert.All(render.Nodes, n => Assert.False(string.IsNullOrWhiteSpace(n.Cluster)));
    }

    [Fact]
    public void Week6ConceptMapRender_StillDefaultsToChainLayout_NoClusterAssigned()
    {
        // Week 6's own call site only ever passes (model, weeks) — the two new parameters are
        // additive with defaults specifically so this keeps compiling and behaving unchanged.
        var week6Model = new ConceptMapModel("t", "s", [new ConceptNode("a", "A", null, 1, [], null)], []);
        GraphRenderModel render = ConceptMapAdapter.ToRenderModel(week6Model, CourseCatalog.Weeks);

        Assert.Equal(ConceptGraphLayout.Chain, render.Layout);
        Assert.All(render.Nodes, n => Assert.Null(n.Cluster));
    }

    [Fact]
    public void ConceptGraphComponent_HasADistinctNetworkLayoutBranch_ChainBranchUntouched()
    {
        string source = ReadComponent("ConceptGraph.razor");

        Assert.Contains("Model.Layout == ConceptGraphLayout.Network", source);
        Assert.Contains("concept-graph__network", source);
        // The original chain+cross-ref branch (Week 6's rendering path) must still be reachable and unchanged.
        Assert.Contains("concept-graph__chain-connector", source);
        Assert.Contains("concept-graph__cross-refs", source);
    }

    [Fact]
    public void ConceptGraphComponent_NetworkNodesShowEveryOutgoingEdge_NotJustOne()
    {
        // The rejected layout used FirstOrDefault, capping every node at one visible connection —
        // the fix must enumerate ALL outgoing edges for a node, not look up a single one.
        string source = ReadComponent("ConceptGraph.razor");
        int networkBranchStart = source.IndexOf("Model.Layout == ConceptGraphLayout.Network", StringComparison.Ordinal);
        int chainBranchStart = source.IndexOf("var primary = Model.Nodes.Where(n => !n.IsCrossReference)", StringComparison.Ordinal);
        string networkBranch = source[networkBranchStart..chainBranchStart];

        Assert.Contains("Model.Edges.Where(e => e.FromId == node.Id)", networkBranch);
        Assert.DoesNotContain("Model.Edges.FirstOrDefault", networkBranch);
    }

    [Fact]
    public void ConceptGraphComponent_NetworkNodesAreCompact_NoPersistentGotoCta()
    {
        // Same compactness principle as the approved Mind Map standard: the concept label itself is
        // the link, not an additional persistent "Виж секцията →" call to action.
        string source = ReadComponent("ConceptGraph.razor");
        int networkBranchStart = source.IndexOf("Model.Layout == ConceptGraphLayout.Network", StringComparison.Ordinal);
        int chainBranchStart = source.IndexOf("var primary = Model.Nodes.Where(n => !n.IsCrossReference)", StringComparison.Ordinal);
        string networkBranch = source[networkBranchStart..chainBranchStart];

        Assert.DoesNotContain("Виж секцията", networkBranch);
    }

    [Fact]
    public void KartaPage_KnowledgeMapUsesNetworkLayoutAndPassesClusters()
    {
        string source = ReadPage("Karta.razor");

        Assert.Contains("ConceptGraphLayout.Network", source);
        Assert.Contains("KnowledgeMapCatalog.Clusters", source);
    }

    [Fact]
    public void KartaPage_KnowledgeMapHeading_IsBulgarianPrimary_EnglishOnlyAsSecondary()
    {
        string source = ReadPublicMarkup("Karta.razor");
        int headingStart = source.IndexOf("<h2 id=\"znanie\"", StringComparison.Ordinal);
        int headingEnd = source.IndexOf("</h2>", headingStart, StringComparison.Ordinal);
        string heading = source[headingStart..headingEnd];

        int bulgarianIndex = heading.IndexOf("Карта на знанието по КПТ", StringComparison.Ordinal);
        int englishIndex = heading.IndexOf("CBT Knowledge Map", StringComparison.Ordinal);

        Assert.True(bulgarianIndex >= 0, "Heading must contain the Bulgarian title.");
        Assert.True(englishIndex > bulgarianIndex, "English label must come after the Bulgarian title, not before it.");
        Assert.Contains("concept-graph__title-secondary", heading);
    }

    private static string ReadComponent(string fileName)
    {
        string sharedDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Shared");
        return File.ReadAllText(Path.Combine(sharedDirectory, fileName));
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    private static string ReadPublicMarkup(string fileName)
    {
        string source = ReadPage(fileName);
        int commentEnd = source.IndexOf("*@", StringComparison.Ordinal);
        return commentEnd >= 0 ? source[(commentEnd + 2)..] : source;
    }
}
