using CbtLearningPlatform.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>Semantic-model and adapter tests for the Cognitive Learning Architecture reference
/// implementation (COGNITIVE_LEARNING_ARCHITECTURE_v1.md §5/§6/§7/§8/§12) — pure C# logic, no Razor
/// rendering involved. ConceptGraph.razor's own rendering contract is covered separately in
/// Week6CognitiveMapTests.cs, matching this project's existing string-based component test style.</summary>
public sealed class ConceptGraphModelTests
{
    private static IReadOnlyList<CourseWeekDefinition> Weeks => CourseCatalog.Weeks;

    [Fact]
    public void ConceptStateResolver_ReturnsUpcoming_WhenIntroducingWeekHasNoRoute()
    {
        // Week 7 is catalogued but Route is null (still frozen).
        Assert.Equal(ConceptState.Upcoming, ConceptStateResolver.Derive(7, [], Weeks));
    }

    [Fact]
    public void ConceptStateResolver_ReturnsIntroduced_WhenNoRevisitedWeekIsRouted()
    {
        // Week 3 is routed; Week 11 (a revisit) is not.
        Assert.Equal(ConceptState.Introduced, ConceptStateResolver.Derive(3, [11], Weeks));
    }

    [Fact]
    public void ConceptStateResolver_ReturnsRevisited_WhenARoutedRevisitedWeekExists()
    {
        // Week 3 introduces, Week 6 (routed) revisits.
        Assert.Equal(ConceptState.Revisited, ConceptStateResolver.Derive(3, [6], Weeks));
    }

    [Fact]
    public void ConceptStateResolver_TreatsNullIntroducedWeek_AsUpcoming()
    {
        Assert.Equal(ConceptState.Upcoming, ConceptStateResolver.Derive(null, [], Weeks));
    }

    [Fact]
    public void MindMapAdapter_PreservesNodeCountAndParentLinks()
    {
        MindMapModel model = new("t", "s",
        [
            new("root", "Root", null, null, null, ConceptState.Introduced),
            new("child", "Child", "root", null, null, ConceptState.Introduced)
        ]);

        GraphRenderModel render = MindMapAdapter.ToRenderModel(model);

        Assert.Equal(2, render.Nodes.Count);
        Assert.Equal(ConceptGraphMode.MindMap, render.Mode);
        Assert.Null(render.Nodes.Single(n => n.Id == "root").ParentId);
        Assert.Equal("root", render.Nodes.Single(n => n.Id == "child").ParentId);
    }

    [Fact]
    public void MindMapAdapter_HasExactlyOneRoot_ForWeek6Structure()
    {
        // Guards the actual Week 6 dataset — a Mind Map with more than one parentless node isn't a
        // single hierarchy any more (COGNITIVE_LEARNING_ARCHITECTURE_v1.md §5: strictly single-parent).
        MindMapModel model = new("t", "s",
        [
            new("root", "Root", null, null, null, ConceptState.Introduced),
            new("a", "A", "root", null, null, ConceptState.Introduced),
            new("b", "B", "root", null, null, ConceptState.Introduced)
        ]);

        GraphRenderModel render = MindMapAdapter.ToRenderModel(model);

        Assert.Single(render.Nodes, n => n.ParentId is null);
    }

    [Fact]
    public void MindMapAdapter_ThrowsOnDanglingParentReference()
    {
        MindMapModel model = new("t", "s",
        [
            new("orphan", "Orphan", "missing-parent", null, null, ConceptState.Introduced)
        ]);

        Assert.Throws<InvalidOperationException>(() => MindMapAdapter.ToRenderModel(model));
    }

    [Fact]
    public void ConceptMapAdapter_AllRelationEndpointsReferenceExistingNodes()
    {
        ConceptMapModel model = new("t", "s",
        [
            new("a", "A", null, 1, [], null),
            new("b", "B", null, 1, [], null)
        ],
        [
            new("a", "b", RelationType.LeadsTo, "поражда")
        ]);

        GraphRenderModel render = ConceptMapAdapter.ToRenderModel(model, Weeks);
        var ids = render.Nodes.Select(n => n.Id).ToHashSet();

        foreach (var edge in render.Edges)
        {
            Assert.Contains(edge.FromId, ids);
            Assert.Contains(edge.ToId, ids);
        }
    }

    [Fact]
    public void ConceptMapAdapter_RealWeek6Nodes_HaveNoDuplicateIdsOrEmptyLabels()
    {
        // Exercises the actual dataset used on the live page, not a synthetic fixture.
        ConceptMapModel model = BuildRealWeek6ConceptMapForTest();
        GraphRenderModel render = ConceptMapAdapter.ToRenderModel(model, Weeks);

        Assert.Equal(render.Nodes.Count, render.Nodes.Select(n => n.Id).Distinct().Count());
        Assert.All(render.Nodes, n => Assert.False(string.IsNullOrWhiteSpace(n.Label)));
    }

    [Fact]
    public void ConceptMapAdapter_CrossReferenceFlagSurvivesAdaptation()
    {
        ConceptMapModel model = new("t", "s",
        [
            new("a", "A", null, 1, [], null),
            new("b", "B (elsewhere)", null, 8, [], "/kurs/sedmica-8", IsCrossReference: true)
        ],
        []);

        GraphRenderModel render = ConceptMapAdapter.ToRenderModel(model, Weeks);

        Assert.False(render.Nodes.Single(n => n.Id == "a").IsCrossReference);
        Assert.True(render.Nodes.Single(n => n.Id == "b").IsCrossReference);
    }

    [Fact]
    public void CaseConceptualizationAdapter_OnlyEmitsFieldsThatWereProvided()
    {
        CaseCharacter character = new("test-case", "Тест", CaseLevel.Basic, 6);
        CaseObservation observation = new(
            character.Id, WeekNumber: 6,
            Situation: "Some situation",
            Thought: null, Emotion: null, Body: null,
            Behavior: "Some behavior",
            Distortion: null, IntermediateBelief: null, CoreBelief: null,
            InterventionLink: null);

        GraphRenderModel render = CaseConceptualizationAdapter.ToRenderModel(character, observation);

        Assert.Equal(2, render.Nodes.Count);
        Assert.Contains(render.Nodes, n => n.ShortDefinition == "Some situation");
        Assert.Contains(render.Nodes, n => n.ShortDefinition == "Some behavior");
        Assert.DoesNotContain(render.Nodes, n => n.Label is "Емоция" or "Телесна реакция" or "Когнитивно изкривяване" or "Междинно вярване" or "Основно вярване");
    }

    [Fact]
    public void CaseConceptualizationAdapter_IrinaWeek6_OmitsEveryFieldWeek6DidNotEstablish()
    {
        CaseObservation observation = CaseCatalog.IrinaObservations.Single(o => o.WeekNumber == 6);
        GraphRenderModel render = CaseConceptualizationAdapter.ToRenderModel(CaseCatalog.Irina, observation);

        var labels = render.Nodes.Select(n => n.Label).ToList();

        Assert.Contains("Ситуация", labels);
        Assert.Contains("Поведение", labels);
        Assert.Contains("Терапевтична интервенция", labels);
        Assert.DoesNotContain("Автоматична мисъл", labels);
        Assert.DoesNotContain("Емоция", labels);
        Assert.DoesNotContain("Телесна реакция", labels);
        Assert.DoesNotContain("Когнитивно изкривяване", labels);
        Assert.DoesNotContain("Междинно вярване", labels);
        Assert.DoesNotContain("Основно вярване", labels);
    }

    [Fact]
    public void CaseCatalog_Irina_HasNoInventedFutureHistory()
    {
        // Guards the explicit owner constraint (blueprint §4/§7): no future belief/distortion/
        // treatment-history is invented ahead of a week that actually teaches it.
        Assert.Equal(6, CaseCatalog.Irina.FirstAppearedWeek);
        Assert.All(CaseCatalog.IrinaObservations, o => Assert.Equal(6, o.WeekNumber));
    }

    private static ConceptMapModel BuildRealWeek6ConceptMapForTest() => new(
        "Ситуация → Мисъл → Реакция (пример: обяд с колеги)",
        "s",
        [
            new("situation", "Ситуация", "Обяд със съученици.", 3, [6], null),
            new("thought", "Автоматична мисъл", "тест", 3, [6], null),
            new("emotion", "Емоция", "Тъга.", 3, [6], null),
            new("body", "Телесна реакция", "тест", 8, [], "/kurs/sedmica-8#karta-na-temata", IsCrossReference: true),
            new("behavior", "Поведение", "тест", 8, [], "/kurs/sedmica-8#karta-na-temata", IsCrossReference: true),
            new("beliefs", "Междинни и основни вярвания", "тест", 3, [11, 12], "/kurs/sedmica-3", IsCrossReference: true)
        ],
        [
            new("situation", "thought", RelationType.LeadsTo, "поражда"),
            new("thought", "emotion", RelationType.LeadsTo, "поражда"),
            new("emotion", "body", RelationType.Precedes, "продължава в пълния модел на"),
            new("body", "behavior", RelationType.LeadsTo, "влияе върху"),
            new("thought", "beliefs", RelationType.Precedes, "задълбочено в")
        ]);
}
