using System.Reflection;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>WEEK 9 — DEEP LEARNING MODULE. Source: SRC-041 (Judith Beck), Глава 11 "Оценяване на
/// автоматични мисли" (printed стр. 167–186) + Глава 12 "Отговаряне на автоматични мисли" (printed
/// стр. 187–197), both read in full. See 00_PROJECT_OS/_blueprints/WEEK_09_SOURCE_COVERAGE_AUDIT_v1.md
/// for the final KU accounting. Owner scoping decisions applied: evaluation-question categories stay
/// at recap depth (cross-linked to Week 10, not re-taught); Сали is not extended with new Week 9
/// biography (Джон, source-named and previously unused, carries the "why evaluation fails" section;
/// the simpler worksheet demonstration reuses the source's own already-generic first-person example);
/// the death-fear decatastrophizing guidance is excluded entirely; the Thought Record is a fixed
/// worked demonstration only — no input, no scoring, no live form, no new browser storage, and this
/// lesson demonstration is explicitly a separate scope from the still-open product-level MVP Thought
/// Record requirement (which this page does not implement, complete, or remove).</summary>
public sealed class Week9ContentSliceTests
{
    private static readonly string[] AnchorIds =
    [
        "karta", "zashto-otsenka", "vaprosi-pregled", "izkrivyavaniya", "koga-verni",
        "zashto-ne-pomaga", "zapis-na-mislite", "raboten-list", "case-lab", "proverki",
        "assessment", "review-map", "izvori"
    ];

    [Fact]
    public void Week9Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica9"));
    }

    [Fact]
    public void Week9_IsNowRoutedAndAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 9);

        Assert.Equal("/kurs/sedmica-9", week.Route);
        Assert.Equal(CourseWeekStatus.Available, week.Status);
    }

    [Fact]
    public void PreviouslyRoutedWeeks_RemainAvailableAfterWeek9Routing()
    {
        int[] weeksToCheck = [1, 3, 6, 7, 8, 10];

        foreach (int number in weeksToCheck)
        {
            CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == number);
            Assert.Equal(CourseWeekStatus.Available, week.Status);
            Assert.Equal($"/kurs/sedmica-{number}", week.Route);
        }
    }

    [Fact]
    public void Week9Page_HasPageTitleAndDeepLearningBadge()
    {
        string source = ReadPage("Sedmica9.razor");

        Assert.Contains("<PageTitle>Седмица 9: Когнитивни изкривявания и дневник на мислите", source);
        Assert.Contains("Дълбочинен модул", source);
    }

    [Fact]
    public void Week9Page_HasAllThirteenSections()
    {
        string source = ReadPage("Sedmica9.razor");

        foreach (string id in AnchorIds)
        {
            Assert.Contains($"id=\"{id}\"", source);
        }

        string[] sectionNumbers = ["9.0", "9.1", "9.2", "9.3", "9.4", "9.5", "9.6", "9.7", "9.8", "9.9", "9.10", "9.11", "9.13"];
        foreach (string number in sectionNumbers)
        {
            Assert.Contains($"{number} ", source);
        }
    }

    [Fact]
    public void Week9Page_SectionNavAnchorsAreRouteSafe()
    {
        string source = ReadPage("Sedmica9.razor");

        foreach (string id in AnchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-9#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
        }
    }

    [Fact]
    public void Week9Page_UsesEstablishedReusablePatterns_ZeroNewComponents()
    {
        string source = ReadPage("Sedmica9.razor");

        Assert.Contains("<LearningSection", source);
        Assert.Contains("<LearningObjectives", source);
        Assert.Contains("<ProgressiveExplanation", source);
        Assert.Contains("<DisclaimerCallout", source);
        Assert.Contains("<SourceReferences", source);
        Assert.Contains("<OptionalReadingSource", source);
        Assert.Contains("<ConceptGraph", source);
        Assert.Contains("<WeekCompletionControl WeekNumber=\"@_week.Number\" />", source);

        Assert.DoesNotContain("<ScenarioSimulator", source);
        Assert.DoesNotContain("<SourceArtifact", source);
        Assert.DoesNotContain("<CbtChainSimulator", source);
        Assert.DoesNotContain("<WhatIfBox", source);
    }

    [Fact]
    public void Week9Page_CrossLinksWeekTenForEvaluationQuestions_InsteadOfReteaching()
    {
        string source = ReadPage("Sedmica9.razor");

        Assert.Contains("/kurs/sedmica-10", source);
        Assert.Contains("/kurs/sedmica-10#semeystva", source);
        Assert.Contains("пълното разгръщане", source);
    }

    [Fact]
    public void Week9Page_HasAllTwelveDistortions_WithSourceExamples()
    {
        string source = ReadPage("Sedmica9.razor");

        string[] distortions =
        [
            "Мислене в черно и бяло", "Катастрофизиране", "Дисквалифициране или отхвърляне на позитивното",
            "Емоционално мислене", "Етикетиране", "Увеличаване/намаляване", "Ментален филтър",
            "Четене на мисли", "Преувеличение", "Персонализация", "Изказвания „трябва“ и „моля“",
            "Тунелно виждане"
        ];

        foreach (string distortion in distortions)
        {
            Assert.Contains(distortion, source);
        }

        // Spot-check a few exact source examples, not paraphrased.
        Assert.Contains("Ако не съм напълно успешен, значи съм провал", source);
        Assert.Contains("Ремонтникът беше груб с мен, защото направих нещо грешно", source);
        Assert.Contains("Учителят на сина ми не може да направи нищо правилно", source);
    }

    [Fact]
    public void Week9Page_UsesJohnNotSali_ForWhyEvaluationFails()
    {
        // Checked against public markup only — the dev comment at the top of the file legitimately
        // names "Сали" once, explaining why she is deliberately NOT used this week (AGENTS.md-style
        // rationale comment), which is never rendered to a visitor.
        string publicMarkup = ReadPublicMarkup("Sedmica9.razor");

        Assert.Contains("Джон", publicMarkup);
        Assert.Contains("баскетболния отбор", publicMarkup);
        Assert.DoesNotContain("Сали", publicMarkup);
        Assert.DoesNotContain("Боб", publicMarkup);
        Assert.DoesNotContain("Карън", publicMarkup);
    }

    [Fact]
    public void Week9Page_DeathFearGuidance_IsExcluded()
    {
        string source = ReadPage("Sedmica9.razor");

        Assert.DoesNotContain("умира", source);
        Assert.DoesNotContain("умре", source);
        Assert.DoesNotContain("смъртта", source);
    }

    [Fact]
    public void Week9Page_ThoughtRecordDemonstration_HasNoInputScoringOrLiveForm()
    {
        // Public markup only — the dev comment legitimately discusses "no <input>" as design
        // rationale, which is never rendered to a visitor.
        string publicMarkup = ReadPublicMarkup("Sedmica9.razor");

        Assert.DoesNotContain("<input", publicMarkup);
        Assert.DoesNotContain("localStorage", publicMarkup);
        Assert.DoesNotContain("твоят резултат", publicMarkup);
        Assert.DoesNotContain("вашият резултат", publicMarkup);
    }

    [Fact]
    public void Week9Page_SimplerWorksheet_UsesTheSourcesOwnGenericExample()
    {
        string source = ReadPage("Sedmica9.razor");

        Assert.Contains("Джоан ми извика", source);
        Assert.Contains("Разкрий пълния попълнен работен лист", source);
    }

    [Fact]
    public void Week9Page_HasTheThoughtRecordStructuralMap_SixColumns_NoNarrativeWalkthrough()
    {
        string source = ReadPage("Sedmica9.razor");

        Assert.Contains("ComponentId=\"week9-thought-record-structure\"", source);
        string[] columns = ["Ситуация", "Автоматична мисъл (+вяра %)", "Емоция (+интензивност %)", "Когнитивно изкривяване", "Отговор", "Резултат"];
        foreach (string column in columns)
        {
            Assert.Contains(column, source);
        }

        // No Bob-specific narrative walkthrough — the full Thought Record is structural only.
        Assert.DoesNotContain("Боб", source);
    }

    [Fact]
    public void Week9Page_FinalAssessmentHasFourteenQuestions()
    {
        string source = ReadPage("Sedmica9.razor");

        for (int i = 1; i <= 14; i++)
        {
            Assert.Contains($"Q{i:D2}", source);
        }
    }

    [Fact]
    public void Week9Page_AssessmentQuestionsHaveSourceCitations()
    {
        string source = ReadPage("Sedmica9.razor");

        int sourceCitationCount = CountOccurrences(source, "Source: 9.");
        Assert.True(sourceCitationCount >= 12, $"Expected at least 12 source-cited assessment answers, found {sourceCitationCount}.");
    }

    [Fact]
    public void Week9Page_HasNoSelfAssessmentInputOrScoring()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica9.razor");

        Assert.DoesNotContain("<input", publicMarkup);
        Assert.DoesNotContain("процент от отговорите", publicMarkup);
    }

    [Fact]
    public void Week9Page_DistinguishesOverviewFromSelfTherapyInstruction()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica9.razor");

        Assert.Contains("не заменя професионална психологическа или медицинска помощ", publicMarkup);
        Assert.Contains("не обучава за самостоятелно провеждане на терапия", publicMarkup);
    }

    [Fact]
    public void Week9Page_ReviewMapHasCrossWeekConnections()
    {
        string source = ReadPage("Sedmica9.razor");

        Assert.Contains("/kurs/sedmica-8", source);
        Assert.Contains("/kurs/sedmica-10", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Week9Page_DoesNotLinkToUnroutedWeeksAsIfAvailable()
    {
        string source = ReadPage("Sedmica9.razor");

        Assert.DoesNotContain("/kurs/sedmica-2", source);
        Assert.DoesNotContain("/kurs/sedmica-4", source);
        Assert.DoesNotContain("/kurs/sedmica-5", source);
        Assert.DoesNotContain("/kurs/sedmica-11", source);
    }

    [Fact]
    public void Week9Page_HasNoInternalDevelopmentLanguageInItsRenderableMarkup()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica9.razor");

        string[] forbiddenTerms =
        [
            "11_SOURCE_REGISTER.md", "kpt_syllabus.pdf", "citation-grade",
            "ACADEMIC/CLINICAL REVIEW PENDING", "10_SESSION_LOG.md", "Project OS",
            "code_artifact.html", "14_EXISTING_PROTOTYPE_AUDIT.md", "_source_corpus", "_blueprints"
        ];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }
    }

    [Fact]
    public void Week9Page_MakesNoFalseAccreditationClaims()
    {
        string source = ReadPage("Sedmica9.razor");

        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит"];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week9Page_HasNoInlineStylesOrAbsolutePositioning()
    {
        string source = ReadPage("Sedmica9.razor");

        Assert.DoesNotContain("style=", source);
        Assert.DoesNotContain("position: absolute", source);
        Assert.DoesNotContain("position:absolute", source);
    }

    [Fact]
    public void Week9Page_HasPreviewMindMap()
    {
        string source = ReadPage("Sedmica9.razor");

        Assert.Contains("ComponentId=\"week9-mindmap-preview\"", source);
    }

    [Fact]
    public void Week9Page_HasReviewMindMap_GatedByAttemptBeforeReveal()
    {
        string source = ReadPage("Sedmica9.razor");

        Assert.Contains("ComponentId=\"week9-mindmap-review\"", source);

        int summaryIndex = source.IndexOf("опитай да си спомниш, преди да разгънеш", StringComparison.OrdinalIgnoreCase);
        int mapIndex = source.IndexOf("ComponentId=\"week9-mindmap-review\"", StringComparison.Ordinal);

        Assert.True(summaryIndex >= 0 && summaryIndex < mapIndex,
            "Review Mind Map must be preceded by an attempt-before-reveal prompt.");
    }

    [Fact]
    public void Week9Page_PreviewAndReviewMindMap_ShareTheSameUnderlyingModel()
    {
        string source = ReadPage("Sedmica9.razor");

        int bindingCount = CountOccurrences(source, "Model=\"@_week9MindMapRender\"");

        Assert.Equal(2, bindingCount);
    }

    [Fact]
    public void Week9MindMap_ProducesAValidHierarchy_NoDanglingReferencesOrCycles()
    {
        string source = ReadPage("Sedmica9.razor");

        Assert.Contains("MindMapAdapter.ToRenderModel(BuildWeek9MindMap())", source);
    }

    [Fact]
    public void KursPage_ListsWeekNineAsAvailable()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("/kurs/sedmica-9", source);
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

    /// <summary>Strips the leading @* ... *@ dev comment — Razor comments never reach the
    /// browser, so only the text after them reflects what a visitor could actually see.</summary>
    private static string ReadPublicMarkup(string fileName)
    {
        string source = ReadPage(fileName);
        int commentEnd = source.IndexOf("*@", StringComparison.Ordinal);
        return commentEnd >= 0 ? source[(commentEnd + 2)..] : source;
    }
}
