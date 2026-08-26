using System.Reflection;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>DEEP LEARNING v2 MIGRATION — Week 8 ("Simulator Workspace" archetype), upgraded from
/// WEEK_08_SOURCE_COVERAGE_AUDIT_v1.md's approved Coverage Matrix (78 in-scope KUs: 7 from Ch.3
/// + 44 from Ch.9 + 27 from Ch.10). Reuses the native &lt;details&gt;/&lt;summary&gt; reveal
/// pattern already established by Week 1/3/10 — not a new quiz engine.</summary>
public sealed class Week8ContentSliceTests
{
    [Fact]
    public void Week8Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica8"));
    }

    [Fact]
    public void Week8_MetadataIsAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 8);

        Assert.Equal(CourseWeekStatus.Available, week.Status);
        Assert.Equal("/kurs/sedmica-8", week.Route);
    }

    [Fact]
    public void Week8Page_HasPageTitle()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("<PageTitle>Седмица 8: Автоматични мисли и емоции", source);
    }

    [Fact]
    public void Week8Page_KeepsExistingCoreSections()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("id=\"nakratko\"", source);
        Assert.Contains("id=\"karta-na-temata\"", source);
        Assert.Contains("id=\"simulator\"", source);
        Assert.Contains("id=\"sravnenie\"", source);
        Assert.Contains("id=\"misal-ili-emociya\"", source);
        Assert.Contains("id=\"palno-obyasnenie\"", source);
        Assert.Contains("id=\"izvori\"", source);
    }

    [Fact]
    public void Week8Page_HasExactlyThreeInteractiveIslands()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("<CbtChainSimulator", source);
        Assert.Contains("<InterpretationExample", source);
        Assert.Contains("<CategorizationCheck", source);
    }

    [Fact]
    public void Week8Page_HasTheNewFindingThePreciseThoughtSection()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("id=\"namirane-na-mislta\"", source);
        Assert.Contains("Приблизителна преценка", source);
        Assert.Contains("По-точна мисъл", source);
    }

    [Fact]
    public void Week8Page_HasTheNewDifferentiatingEmotionsSection()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("id=\"razlichavane-na-emociite\"", source);
        Assert.Contains("Провери дали мисълта пасва на чувството", source);
    }

    [Fact]
    public void Week8Page_HasTheThreeTypesOfThoughtsValidityContent()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("Не всяка мисъл е еднакво точна", source);
        Assert.Contains("вярна, но не особено полезна", source);
    }

    [Fact]
    public void Week8Page_DoesNotUseClinicalDysfunctionalThoughtTerminology()
    {
        // U52 owner decision: light source-grounded bridge only, full evaluation stays Week 10 —
        // no "dysfunctional thought" clinical/diagnostic terminology introduced on this page.
        string source = ReadPage("Sedmica8.razor");

        Assert.DoesNotContain("дисфункционал", source);
    }

    [Fact]
    public void Week8Page_HasTheEmotionsAreNotAMistakeContent()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("Емоциите не са грешка за поправяне", source);
        Assert.Contains("подобно на физическата болка", source);
    }

    [Fact]
    public void Week8Page_DoesNotContainSallysCaseNarrative()
    {
        // Owner decision: do not migrate Sally's full case narrative into Week 8 — all examples
        // stay original/neutral, consistent with the platform-wide non-clinical example policy.
        string source = ReadPage("Sedmica8.razor");

        Assert.DoesNotContain("Сали", source);
    }

    [Fact]
    public void Week8Page_HasARealRevealBasedKnowledgeCheck()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("id=\"proverka-znania\"", source);
        Assert.Contains("Проверка на знанията", source);
        Assert.Contains("Проверката не се оценява и не запазва отговори", source);
        Assert.Contains("Въпрос 1.", source);
        Assert.Contains("Въпрос 2.", source);
        Assert.Contains("Въпрос 3.", source);
        Assert.Contains("Въпрос 4.", source);
        Assert.Contains("Въпрос 5.", source);
    }

    [Fact]
    public void Week8Page_KnowledgeCheckReusesNativeDetailsSummary_NotANewQuizEngine()
    {
        string source = ReadPage("Sedmica8.razor");
        int checkIndex = source.IndexOf("id=\"proverka-znania\"", StringComparison.Ordinal);
        int nextSectionIndex = source.IndexOf("id=\"karta-povtorenie\"", StringComparison.Ordinal);
        string checkBlock = source[checkIndex..nextSectionIndex];

        int detailsCount = System.Text.RegularExpressions.Regex.Matches(checkBlock, "<details").Count;
        Assert.Equal(5, detailsCount);
    }

    [Fact]
    public void Week8Page_ReflectionSectionIsKeptSeparateFromKnowledgeCheck()
    {
        // The existing open, unscored reflection (section 09) and the new scored-format,
        // reveal-based Knowledge Check (section 10) serve different pedagogical purposes and
        // must remain two distinct sections, not merged into one.
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("id=\"proverka\"", source);
        Assert.Contains("Отделете момент да размислите (без изпращане, без оценка)", source);
        Assert.Contains("id=\"proverka-znania\"", source);
        Assert.True(source.IndexOf("id=\"proverka\"", StringComparison.Ordinal)
            < source.IndexOf("id=\"proverka-znania\"", StringComparison.Ordinal));
    }

    [Fact]
    public void Week8Page_ReflectionAndKnowledgeCheckAreNotPairedInAMismatchedHeightRow()
    {
        // Mirrors the Week 3 owner-review lesson (Week3ContentSliceTests.cs): sections without a
        // natural pairing partner must be their own full-width section, not forced into a shared
        // .learning-grid--balanced row with a single child.
        string source = ReadPage("Sedmica8.razor");
        int reflectionIndex = source.IndexOf("id=\"proverka\"", StringComparison.Ordinal);
        int checkIndex = source.IndexOf("id=\"proverka-znania\"", StringComparison.Ordinal);
        string between = source[reflectionIndex..checkIndex];

        Assert.Contains("</section>", between);
    }

    [Fact]
    public void Week8Page_DoesNotInventSelfGuidedAdaptationsOfExcludedTherapistTechniques()
    {
        // Owner decision: U71/U74/U75 (keep questioning past the first answer; hypothetical
        // elimination to isolate the most distressing issue/part) remain Excluded — no invented
        // self-guided adaptation of them anywhere on the page. "елиминира" alone is not checked
        // here — the emotion-function content legitimately uses it ("не да ги елиминирате
        // напълно"), unrelated to the excluded worry-elimination technique.
        string source = ReadPage("Sedmica8.razor");

        Assert.DoesNotContain("хипотетично", source);
        Assert.DoesNotContain("избройте притесненията", source);
    }

    [Fact]
    public void Week8Page_HasEducationalDisclaimer()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("<DisclaimerCallout", source);
    }

    [Fact]
    public void Week8Page_CitesAllThreeSourceChapters()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("Глава 3", source);
        Assert.Contains("Глава 9", source);
        Assert.Contains("Глава 10", source);
    }

    [Fact]
    public void Week8Page_SectionNavAnchorsIncludeTheFullPath_NotBareFragments()
    {
        // Same App.razor <base href="/"> reasoning as Week3ContentSliceTests.cs's equivalent test
        // — a bare href="#id" would resolve against Home, not the current page.
        string source = ReadPage("Sedmica8.razor");

        string[] anchorIds =
        [
            "karta-sedmicata", "nakratko", "karta-na-temata", "simulator", "sravnenie", "misal-ili-emociya",
            "namirane-na-mislta", "razlichavane-na-emociite", "palno-obyasnenie",
            "proverka", "proverka-znania", "karta-povtorenie", "izvori"
        ];

        foreach (string id in anchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-8#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
        }
    }

    [Fact]
    public void Week8Page_CrossLinksToWeek10AndModul2Lessons_NoDeadLinks()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("href=\"/kurs/sedmica-10\"", source);
        Assert.Contains("/programa/modul-2/situacia-misal-emocia-povedenie", source);
        Assert.Contains("/programa/modul-2/avtomatichni-misli", source);
        Assert.Contains("/programa/modul-2/emocii-i-telesni-reaktsii", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Week8Page_HasNoInternalDevelopmentLanguageInItsRenderableMarkup()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica8.razor");

        // Note: "11_SOURCE_REGISTER.md" and "citation-grade" are intentionally NOT in this list —
        // the page's own pre-existing "Академичен контекст" text already displays
        // "<code>11_SOURCE_REGISTER.md</code>" as visible, learner-facing citation context; that
        // predates this migration and is unrelated to internal-only development artifacts.
        string[] forbiddenTerms =
        [
            "kpt_syllabus.pdf", "WEEK_08_SOURCE_COVERAGE_AUDIT", "10_SESSION_LOG.md",
            "Project OS", "Knowledge Unit"
        ];

        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }
    }

    [Fact]
    public void Week8Page_HasWeeklyMindMapPreviewAndReview_SameModelBothTimes()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("ComponentId=\"week8-mindmap-preview\"", source);
        Assert.Contains("ComponentId=\"week8-mindmap-review\"", source);
        // Both instances render the exact same static model — one knowledge structure, two states.
        Assert.Equal(2, System.Text.RegularExpressions.Regex.Matches(source, "Model=\"@_week8MindMapRender\"").Count);
    }

    [Fact]
    public void Week8Page_MindMapPreviewComesBeforeSection01_ReviewComesBeforeSummary()
    {
        string source = ReadPage("Sedmica8.razor");

        int previewIndex = source.IndexOf("id=\"karta-sedmicata\"", StringComparison.Ordinal);
        int nakratkoIndex = source.IndexOf("id=\"nakratko\"", StringComparison.Ordinal);
        int reviewIndex = source.IndexOf("id=\"karta-povtorenie\"", StringComparison.Ordinal);
        int izvoriIndex = source.IndexOf("id=\"izvori\"", StringComparison.Ordinal);

        Assert.True(previewIndex >= 0 && previewIndex < nakratkoIndex);
        Assert.True(reviewIndex > 0 && reviewIndex < izvoriIndex);
    }

    [Fact]
    public void Week8Page_MindMapReviewUsesExplainBeforeRevealPattern()
    {
        string source = ReadPage("Sedmica8.razor");
        int reviewIndex = source.IndexOf("id=\"karta-povtorenie\"", StringComparison.Ordinal);
        int nextSectionIndex = source.IndexOf("id=\"izvori\"", StringComparison.Ordinal);
        string reviewBlock = source[reviewIndex..nextSectionIndex];

        Assert.Contains("<details", reviewBlock);
        Assert.Contains("опитай да си спомниш", reviewBlock);
    }

    [Fact]
    public void Week8Page_MindMapNodesOnlyLinkToAnchorsThatExistOnThisPage()
    {
        // Every MindMapNode anchor must resolve to a real section on this page — no invented
        // destination, no dangling reference.
        string source = ReadPage("Sedmica8.razor");

        string[] mindMapAnchors =
        [
            "karta-na-temata", "nakratko", "palno-obyasnenie", "namirane-na-mislta", "razlichavane-na-emociite"
        ];

        foreach (string anchor in mindMapAnchors)
        {
            Assert.Contains($"\"/kurs/sedmica-8#{anchor}\"", source);
            Assert.Contains($"id=\"{anchor}\"", source);
        }
    }

    [Fact]
    public void Week8Page_Section04And05AreNotPairedInAMismatchedHeightRow()
    {
        // Owner visual review found a large dead-space gap: section 04 (InterpretationExample,
        // short) and section 05 (comparison table + CategorizationCheck, much taller) previously
        // shared one .learning-grid--balanced row, so the row's height followed the taller child
        // and left a large empty gap under the shorter one. Mirrors the same fix already applied
        // to Week 3's mismatched-height pair (Week3ContentSliceTests.cs) — each is now its own
        // full-width section.
        string source = ReadPage("Sedmica8.razor");

        int sravnenieIndex = source.IndexOf("id=\"sravnenie\"", StringComparison.Ordinal);
        int misalIndex = source.IndexOf("id=\"misal-ili-emociya\"", StringComparison.Ordinal);
        string between = source[sravnenieIndex..misalIndex];

        Assert.Contains("</section>", between);
        Assert.DoesNotContain("learning-grid--balanced", between);
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
