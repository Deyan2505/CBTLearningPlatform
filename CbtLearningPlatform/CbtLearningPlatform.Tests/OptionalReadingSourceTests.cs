namespace CbtLearningPlatform.Tests;

/// <summary>OPTIONAL READING SOURCE COMPONENT — a small reusable "Допълнително четене" block
/// for voluntary deeper reading of the original academic source (SRC-041). Supplemental only:
/// lessons stay fully self-contained, no Library phase, no embedded book, no textbook
/// dependency. Chapter numbers may appear only where the source registry / session log has
/// actually confirmed them (Chapter 1 and Chapter 3); URL renders only when a verified public
/// location is explicitly provided — none exists yet, so no page passes one.</summary>
public sealed class OptionalReadingSourceTests
{
    private static readonly string[] WeekPagesWithOptionalReading =
    [
        "Sedmica1.razor", "Sedmica3.razor", "Sedmica6.razor", "Sedmica8.razor", "Sedmica10.razor"
    ];

    [Fact]
    public void OptionalReadingSource_ComponentExists()
    {
        string source = ReadSharedComponent("OptionalReadingSource.razor");

        Assert.Contains("class=\"optional-reading\"", source);
    }

    [Theory]
    [InlineData("Sedmica1.razor")]
    [InlineData("Sedmica3.razor")]
    [InlineData("Sedmica6.razor")]
    [InlineData("Sedmica8.razor")]
    [InlineData("Sedmica10.razor")]
    public void EveryValidatedWeek_UsesTheOptionalReadingSource(string fileName)
    {
        string source = ReadPage(fileName);

        Assert.Contains("<OptionalReadingSource", source);
        Assert.Contains("Джудит С. Бек", source);
        Assert.Contains("Когнитивно-поведенческа терапия: Основи и отвъд", source);
    }

    [Fact]
    public void Component_TitleIsOptionalReading_AndSaysTheSourceIsNotRequired()
    {
        string source = ReadSharedComponent("OptionalReadingSource.razor");

        Assert.Contains(">Допълнително четене</h3>", source);
        Assert.Contains("не е необходим за преминаването на урока", source);
        Assert.Contains("по-задълбочено в оригиналния академичен контекст", source);
    }

    [Fact]
    public void Component_NeverUsesTextbookDependentLanguage()
    {
        string source = ReadSharedComponent("OptionalReadingSource.razor");

        string[] forbiddenPhrases =
        [
            "За да разберете урока", "Задължително четене", "Продължете обучението в учебника",
            "Урокът продължава в книгата", "Необходимо е да прочетете"
        ];

        foreach (string phrase in forbiddenPhrases)
        {
            Assert.DoesNotContain(phrase, source);
        }
    }

    [Fact]
    public void ChapterNumbers_AppearOnlyWhereSourceConfirmed()
    {
        // Session log confirms Chapter 1 (Въведение) and Chapter 3 (Когнитивна концептуализация)
        // were actually read from SRC-041. Week 8 and Week 10 themes have no confirmed chapter
        // number, so they must use a thematic RelevantSection instead of an invented "Глава N".
        Assert.Contains("Глава 1 — Въведение в когнитивно-поведенческата терапия", ReadPage("Sedmica1.razor"));
        Assert.Contains("Глава 3 — Когнитивна концептуализация", ReadPage("Sedmica3.razor"));
        Assert.Contains("Глава 5 — Структура на първата терапевтична сесия", ReadPage("Sedmica6.razor"));

        Assert.Contains("RelevantSection=\"Разпознаване на автоматичните мисли\"", ReadPage("Sedmica8.razor"));
        Assert.Contains("RelevantSection=\"Насочено откриване и сократически въпроси\"", ReadPage("Sedmica10.razor"));

        // No invented chapter numbers on the two weeks without a confirmed chapter.
        Assert.DoesNotContain("RelevantSection=\"Глава", ReadPage("Sedmica8.razor"));
        Assert.DoesNotContain("RelevantSection=\"Глава", ReadPage("Sedmica10.razor"));
    }

    [Fact]
    public void Url_RendersOnlyWhenProvided_AndNoPagePassesOneYet()
    {
        string component = ReadSharedComponent("OptionalReadingSource.razor");

        // Conditional link rendering — the component must work without a URL.
        Assert.Contains("@if (!string.IsNullOrEmpty(Url))", component);
        Assert.Contains("Отвори източника", component);

        // Source governance: no verified public URL is registered yet (SRC-041 was provided as
        // an owner-supplied full text), so no page may pass one.
        foreach (string fileName in WeekPagesWithOptionalReading)
        {
            string source = ReadPage(fileName);
            int usageStart = source.IndexOf("<OptionalReadingSource", StringComparison.Ordinal);
            int usageEnd = source.IndexOf("/>", usageStart, StringComparison.Ordinal);
            string usage = source[usageStart..usageEnd];

            Assert.DoesNotContain("Url=", usage);
        }
    }

    [Fact]
    public void NoEmbeddedBook_NoIframe_NoDownloadEndpoint_NoLibraryRoute()
    {
        string component = ReadSharedComponent("OptionalReadingSource.razor");
        Assert.DoesNotContain("<iframe", component);
        Assert.DoesNotContain("<embed", component);
        Assert.DoesNotContain(".pdf", component);
        Assert.DoesNotContain("localStorage", component);
        Assert.DoesNotContain("sessionStorage", component);

        // No PDF/EPUB was added to the served static assets.
        string wwwroot = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "wwwroot");
        Assert.Empty(Directory.EnumerateFiles(wwwroot, "*.pdf", SearchOption.AllDirectories));
        Assert.Empty(Directory.EnumerateFiles(wwwroot, "*.epub", SearchOption.AllDirectories));

        // No new Library route appeared anywhere in the app.
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Pages");
        foreach (string file in Directory.EnumerateFiles(pagesDirectory, "*.razor"))
        {
            string source = File.ReadAllText(file);
            Assert.DoesNotContain("@page \"/biblioteka", source);
            Assert.DoesNotContain("@page \"/library", source);
        }
    }

    [Fact]
    public void Component_IconsArePresentationOnly_AndBlockIsAnAsideWithItsOwnHeading()
    {
        string source = ReadSharedComponent("OptionalReadingSource.razor");

        Assert.Contains("<aside class=\"optional-reading\" aria-labelledby=\"optional-reading-heading\">", source);
        Assert.Contains("<h3 id=\"optional-reading-heading\">", source);

        int svgCount = source.Split("<svg").Length - 1;
        int hiddenSvgCount = source.Split("aria-hidden=\"true\"").Length - 1;
        Assert.True(hiddenSvgCount >= svgCount, "Every decorative SVG icon must be aria-hidden.");
    }

    [Fact]
    public void AppCss_OptionalReadingUsesTheCalmAcademicRole_NotWarningOrPrimaryCta()
    {
        string css = ReadCss();

        int ruleIndex = css.IndexOf(".optional-reading {", StringComparison.Ordinal);
        Assert.True(ruleIndex >= 0);

        int blockEnd = css.IndexOf(".optional-reading__link-icon", ruleIndex, StringComparison.Ordinal);
        string block = css[ruleIndex..blockEnd];

        Assert.DoesNotContain("--accent-safety", block);
        Assert.DoesNotContain("--color-warning", block);
        Assert.DoesNotContain("--color-error", block);
        Assert.DoesNotContain("--accent-interactive", block);
        // The link is a quiet secondary action, not the violet primary CTA treatment.
        Assert.DoesNotContain("--accent-primary", block);
    }

    [Fact]
    public void AppCss_OptionalReadingUsesThePageCompatibleSurface_NotAFilledAccentBackground()
    {
        // Owner review: the original solid --accent-academic-surface fill read as a violet
        // featured card. Only the border stays academic-indigo; the background now matches the
        // same page-compatible surface every ordinary lesson card (.card/.section-card) uses.
        string css = ReadCss();

        int ruleIndex = css.IndexOf(".optional-reading {", StringComparison.Ordinal);
        int ruleEnd = css.IndexOf('}', ruleIndex);
        string rule = css[ruleIndex..ruleEnd];

        Assert.Contains("border: 1px solid var(--accent-academic-border);", rule);
        Assert.Contains("background: var(--color-surface);", rule);
        Assert.DoesNotContain("--accent-academic-surface", rule);
        // No arbitrary narrow max-width — it aligns with .source-references above it.
        Assert.DoesNotContain("max-width", rule);
    }

    [Fact]
    public void SourceReferences_HeadingIsJustSources_NoLongerDuplicatesOptionalReading()
    {
        // Owner review: "Източници и допълнително четене" duplicated the OptionalReadingSource
        // block's own "Допълнително четене" heading immediately below it. The two blocks now
        // have clearly distinct roles and headings.
        string source = ReadSharedComponent("SourceReferences.razor");

        Assert.Contains("<h2 id=\"source-references-heading\">Източници</h2>", source);
        Assert.DoesNotContain("Източници и допълнително четене", source);
    }

    [Fact]
    public void OptionalReadingSource_NoLongerRendersASourceRoleBadge()
    {
        string component = ReadSharedComponent("OptionalReadingSource.razor");

        Assert.DoesNotContain("SourceRole", component);
        Assert.DoesNotContain("optional-reading__role", component);

        foreach (string fileName in WeekPagesWithOptionalReading)
        {
            Assert.DoesNotContain("SourceRole=\"Учебник\"", ReadPage(fileName));
        }
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    private static string ReadSharedComponent(string fileName)
    {
        string sharedDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Shared");
        return File.ReadAllText(Path.Combine(sharedDirectory, fileName));
    }

    private static string ReadCss()
    {
        string cssPath = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "wwwroot", "app.css");
        return File.ReadAllText(cssPath);
    }
}
