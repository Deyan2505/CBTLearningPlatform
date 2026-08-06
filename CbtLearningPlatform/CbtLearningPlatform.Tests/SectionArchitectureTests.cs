namespace CbtLearningPlatform.Tests;

/// <summary>Covers the owner visual-rejection-round-2 redesign: representative week must have
/// at least 8 visually distinct, anchor-navigable sections, real visual models (not just text),
/// and progressive disclosure whose "full explanation" is genuinely richer than the always-visible
/// short summary — not a repetition of it.</summary>
public sealed class SectionArchitectureTests
{
    private static readonly string[] SectionAnchors =
    [
        "nakratko", "karta-na-temata", "simulator", "sravnenie",
        "misal-ili-emociya", "palno-obyasnenie", "proverka", "izvori"
    ];

    [Fact]
    public void Week8Page_HasAllEightSectionAnchors()
    {
        string source = ReadPage("Sedmica8.razor");

        foreach (string anchor in SectionAnchors)
        {
            Assert.Contains($"id=\"{anchor}\"", source);
        }
    }

    [Fact]
    public void Week8Page_SectionNavigatorLinksToAllEightAnchors()
    {
        // Route-safe hrefs (owner review, systemic anchor fix): App.razor's <base href="/">
        // means a bare "#anchor" resolves to Home, not the current page — every section-nav
        // link must include the full "/kurs/sedmica-8" path.
        string source = ReadPage("Sedmica8.razor");

        foreach (string anchor in SectionAnchors)
        {
            Assert.Contains($"href=\"/kurs/sedmica-8#{anchor}\"", source);
            Assert.DoesNotContain($"href=\"#{anchor}\"", source);
        }
    }

    [Fact]
    public void Week8Page_SectionHeadingsAreKeyboardFocusableAfterAnchorNavigation()
    {
        string source = ReadPage("Sedmica8.razor");

        foreach (string anchor in SectionAnchors)
        {
            Assert.Contains($"id=\"{anchor}\" tabindex=\"-1\"", source);
        }
    }

    [Fact]
    public void Week8Page_DisclaimerIsNotNestedInsideProgressiveDisclosure()
    {
        string source = ReadPage("Sedmica8.razor");

        int lastDisclosureClose = source.LastIndexOf("</ProgressiveExplanation>", StringComparison.Ordinal);
        int disclaimerIndex = source.IndexOf("<DisclaimerCallout", StringComparison.Ordinal);

        Assert.True(disclaimerIndex > lastDisclosureClose,
            "DisclaimerCallout must render after both ProgressiveExplanation blocks, not inside one.");
    }

    [Fact]
    public void Week8Page_HasTwoDistinctProgressiveDisclosureBlocks()
    {
        // "Пълно обяснение" (Section 06) and "Академичен контекст" (Section 08) are separate
        // disclosure levels, not one compound block.
        string source = ReadPage("Sedmica8.razor");

        int count = CountOccurrences(source, "<ProgressiveExplanation");
        Assert.Equal(2, count);
    }

    [Fact]
    public void Week8Page_FullExplanationIsSubstantiallyRicherThanTheAlwaysVisibleShortSummary()
    {
        string source = ReadPage("Sedmica8.razor");

        int fullStart = source.IndexOf("SummaryLabel=\"Покажи пълното обяснение\"", StringComparison.Ordinal);
        int fullEnd = source.IndexOf("</ProgressiveExplanation>", fullStart, StringComparison.Ordinal);

        Assert.True(fullStart >= 0 && fullEnd > fullStart);

        string fullExplanationBody = source[fullStart..fullEnd];

        Assert.True(fullExplanationBody.Length > 800,
            $"Expected the full explanation to contain substantial additional content, found {fullExplanationBody.Length} chars.");
    }

    [Fact]
    public void Week8Page_UsesAWiderWorkspaceContainerForVisualSections()
    {
        Assert.Contains("<div class=\"workspace\">", ReadPage("Sedmica8.razor"));
    }

    [Fact]
    public void Week8Page_HasAStaticConceptMapFlowDiagram()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("concept-map", source);
        Assert.Contains("Автоматична мисъл", source);
    }

    [Fact]
    public void Week8Page_HasAComparisonMatrixWithAllFourCategories()
    {
        string source = ReadPage("Sedmica8.razor");

        Assert.Contains("comparison-matrix", source);
        foreach (string category in new[] { "Мисъл", "Емоция", "Телесна реакция", "Поведение" })
        {
            Assert.Contains($"<th scope=\"row\">{category}</th>", source);
        }
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
}
