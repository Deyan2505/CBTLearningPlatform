namespace CbtLearningPlatform.Curriculum;

/// <summary>Course Map — pure curriculum/navigation representation ("къде се намирам в курса?"),
/// COGNITIVE_LEARNING_ARCHITECTURE_v1.md Phase 5. Derives entirely from the existing CourseCatalog
/// (Modules + Weeks) at build time — no second, hand-maintained list of the 15 weeks. Rendered via
/// the locked Mind Map standard (root -> 4 modules -> weeks), reusing MindMapAdapter/ConceptGraph
/// unchanged.
///
/// ConceptState here means curriculum reachability, not learner progress: a week with a real Route
/// is "Introduced" (available/reachable in the course); a week with Route == null is "Upcoming"
/// (planned, not yet built). This mirrors CurriculumLabels.DeriveStatus()'s own routed/not-routed
/// distinction — it does not invent a new status axis.
///
/// Prerequisite relationships between weeks in different modules (e.g. Week 6 suggesting Week 3
/// first) are intentionally NOT drawn here: Mind Map mode is a strict single-parent tree, and a
/// cross-branch prerequisite line would turn it into a graph — that concern belongs to the CBT
/// Knowledge Map (KnowledgeMapCatalog.cs), not the Course Map. Each week's own page already states
/// its own prerequisite in prose.</summary>
public static class CourseMapBuilder
{
    private const string RootId = "course-map-root";

    public static MindMapModel Build()
    {
        List<MindMapNode> nodes = [new(RootId, "15-седмичен курс", null, null, null, ConceptState.Introduced)];

        foreach (CourseModule module in CourseCatalog.Modules)
        {
            string moduleId = $"course-map-module-{module.Number}";
            nodes.Add(new MindMapNode(moduleId, module.Title, RootId, null, null, ConceptState.Introduced));

            foreach (CourseWeekDefinition week in CourseCatalog.Weeks.Where(w => w.ModuleLabel == module.Title))
            {
                ConceptState state = week.Route is not null ? ConceptState.Introduced : ConceptState.Upcoming;
                nodes.Add(new MindMapNode($"course-map-week-{week.Number}", $"{week.Number}. {week.Title}", moduleId, null, week.Route, state));
            }
        }

        return new MindMapModel(
            "Карта на курса",
            "Модулите и седмиците на 15-седмичния курс — къде се намирате в програмата, кои седмици вече са налични и кои предстоят.",
            nodes);
    }
}
