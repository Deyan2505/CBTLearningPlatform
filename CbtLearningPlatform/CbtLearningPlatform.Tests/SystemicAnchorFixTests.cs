namespace CbtLearningPlatform.Tests;

/// <summary>Systemic route-safe anchor fix (owner review, this session): App.razor's
/// <c>&lt;base href="/"&gt;</c> (required for Blazor asset/SignalR path resolution) means a bare
/// same-page fragment href like <c>href="#id"</c> resolves against "/" (Home), not the current
/// route — standard HTML base-URL resolution, not a Blazor-specific bug. Confirmed and fixed
/// across all three week pages plus the global skip-link, which needed a dynamic fix since its
/// target page changes on every route.</summary>
public sealed class SystemicAnchorFixTests
{
    [Theory]
    [InlineData("Sedmica1.razor", "/kurs/sedmica-1",
        new[] { "nakratko", "istoricheska-linia", "nauchen-obrat", "predi-i-sled", "avtomatichni-misli-preview", "zashto-struktura", "publikacia-1979", "proverka", "izvori" })]
    [InlineData("Sedmica3.razor", "/kurs/sedmica-3",
        new[] { "karta-sedmicata", "tri-niva", "izsledvane", "situacia-znachenie", "triada", "filtar", "sali-hierarhia", "karta", "posledovatelnost", "kaskaden-model", "obarkvaniya", "proverka", "karta-povtorenie", "izvori" })]
    [InlineData("Sedmica8.razor", "/kurs/sedmica-8",
        new[] { "nakratko", "karta-na-temata", "simulator", "sravnenie", "misal-ili-emociya", "palno-obyasnenie", "proverka", "izvori" })]
    public void WeekPage_SectionNavAnchorsAreRouteSafe_NoBareFragmentsRemain(string fileName, string routePrefix, string[] anchorIds)
    {
        string source = ReadPage(fileName);

        foreach (string id in anchorIds)
        {
            Assert.Contains($"href=\"{routePrefix}#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);

            // Every nav target must have a matching, real heading id on the same page.
            Assert.Contains($"id=\"{id}\"", source);
        }
    }

    [Theory]
    [InlineData("Sedmica1.razor")]
    [InlineData("Sedmica3.razor")]
    [InlineData("Sedmica8.razor")]
    public void WeekPage_SectionIds_HaveNoDuplicates(string fileName)
    {
        string source = ReadPage(fileName);

        List<string> ids = [];
        int index = 0;
        while ((index = source.IndexOf("id=\"", index, StringComparison.Ordinal)) != -1)
        {
            int start = index + 4;
            int end = source.IndexOf('"', start);
            ids.Add(source[start..end]);
            index = end;
        }

        // "main-content" is the shell's own landmark id (MainLayout.razor), not a page anchor.
        List<string> pageAnchorIds = [.. ids.Where(id => id != "main-content")];

        Assert.Equal(pageAnchorIds.Count, pageAnchorIds.Distinct().Count());
    }

    [Fact]
    public void MainLayout_SkipLinkIsRouteSafe_NotABareFragment()
    {
        string source = ReadLayoutComponent("MainLayout.razor");

        Assert.DoesNotContain("href=\"#main-content\"", source);
        Assert.Contains("CurrentPageFragment(\"main-content\")", source);
    }

    [Fact]
    public void MainLayout_CurrentPageFragmentHelper_UsesAbsolutePathAndQuery_NotJustHardcodedRoot()
    {
        // Static-source check (no bUnit harness in this project) — confirms the helper builds
        // the fragment from the current page's own path/query, not a fixed "/" or "#" string.
        string source = ReadLayoutComponent("MainLayout.razor");

        Assert.Contains("uri.AbsolutePath", source);
        Assert.Contains("uri.Query", source);
        Assert.Contains("Nav.Uri", source);
    }

    [Fact]
    public void NoBareFragmentAnchorsRemainAnywhereInComponents()
    {
        string componentsRoot = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components");

        foreach (string file in Directory.EnumerateFiles(componentsRoot, "*.razor", SearchOption.AllDirectories))
        {
            string source = File.ReadAllText(file);

            // A bare href="#..." resolves against <base href="/"> (Home), not the current page —
            // every same-page anchor in this app must include its own full route.
            Assert.False(
                System.Text.RegularExpressions.Regex.IsMatch(source, "href=\"#[a-zA-Z0-9-]"),
                $"{Path.GetFileName(file)} still has a bare fragment href, which resolves to Home, not the current page.");
        }
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    private static string ReadLayoutComponent(string fileName)
    {
        string layoutDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Layout");
        return File.ReadAllText(Path.Combine(layoutDirectory, fileName));
    }
}
