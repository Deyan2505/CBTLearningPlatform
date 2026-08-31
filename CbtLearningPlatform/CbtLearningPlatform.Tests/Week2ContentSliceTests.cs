using System.Reflection;
using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>WEEK_02_SOURCE_AUDIT_v1 (owner-approved) — "Когнитивна терапия на Бек и REBT на Елис,"
/// a cross-source build (SRC-041 for Beck's bridge sentences + SRC-042/Albert Ellis Institute for
/// the Ellis/REBT half, closing the ABC-model source gap flagged in the prior audit turn).
/// "Comparison" archetype (CourseCatalog.cs InteractiveFormat.Comparison), zero interactivity.</summary>
public sealed class Week2ContentSliceTests
{
    [Fact]
    public void Week2Page_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Assert.NotNull(assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Sedmica2"));
    }

    [Fact]
    public void Week2_MetadataIsAvailable()
    {
        CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == 2);

        Assert.Equal(CourseWeekStatus.Available, week.Status);
        Assert.Equal("/kurs/sedmica-2", week.Route);
    }

    [Fact]
    public void Week1Week3_RemainAvailableAfterWeek2WasAdded()
    {
        int[] weeksToCheck = [1, 3];

        foreach (int number in weeksToCheck)
        {
            CourseWeekDefinition week = CourseCatalog.Weeks.Single(w => w.Number == number);
            Assert.Equal(CourseWeekStatus.Available, week.Status);
            Assert.Equal($"/kurs/sedmica-{number}", week.Route);
        }
    }

    [Fact]
    public void Week2Page_HasPageTitleAndLearningObjectives()
    {
        string source = ReadPage("Sedmica2.razor");

        Assert.Contains("<PageTitle>Седмица 2: Когнитивна терапия на Бек и REBT на Елис", source);
        Assert.Contains("<LearningObjectives", source);
    }

    [Fact]
    public void Week2Page_UsesOnlyExistingReusablePatterns_NoNewComponent()
    {
        string source = ReadPage("Sedmica2.razor");

        Assert.Contains("<LearningSection", source);
        Assert.Contains("<LearningObjectives", source);
        Assert.Contains("<ProgressiveExplanation", source);
        Assert.Contains("<DisclaimerCallout", source);
        Assert.Contains("<SourceReferences", source);
        Assert.Contains("<WeekCompletionControl", source);
        Assert.Contains("class=\"category-compare\"", source);
        Assert.Contains("class=\"comparison-matrix comparison-matrix--dual\"", source);

        string[] forbiddenNewComponents =
        [
            "<CbtChainSimulator", "<CategorizationCheck", "<InterpretationExample",
            "<ResearchTurnStepper", "<SocraticDialogueExplorer", "<SchemaFilterDemonstration",
            "<ConceptGraph", "<HistoricalTimeline"
        ];
        foreach (string component in forbiddenNewComponents)
        {
            Assert.DoesNotContain(component, source);
        }
    }

    [Fact]
    public void Week2Page_ExplainsTheAbcModel()
    {
        string source = ReadPage("Sedmica2.razor");

        Assert.Contains("A — Активиращо събитие", source);
        Assert.Contains("B — Вярвания", source);
        Assert.Contains("C — Последствия", source);

        // No formal ABCDE/disputing content in the rendered page — not in SRC-042, not invented.
        // (The dev comment above the page legitimately explains this exclusion by name — check
        // only what a visitor could see.)
        string publicMarkup = ReadPublicMarkup("Sedmica2.razor");
        Assert.DoesNotContain("ABCDE", publicMarkup);
        Assert.DoesNotContain("оспорване", publicMarkup);
    }

    [Fact]
    public void Week2Page_DistinguishesRationalAndIrrationalBeliefs()
    {
        string source = ReadPage("Sedmica2.razor");

        Assert.Contains("Ирационално вярване", source);
        Assert.Contains("Рационално вярване", source);
        Assert.Contains("функционални последствия", source);
        Assert.Contains("дисфункционални последствия", source);
    }

    [Fact]
    public void Week2Page_CoversSecondaryConsequencesAndUnconditionalSelfAcceptance()
    {
        string source = ReadPage("Sedmica2.razor");

        Assert.Contains("вторични последствия", source);
        Assert.Contains("безусловно самоприемане", source);
    }

    [Fact]
    public void Week2Page_HasTheBeckEllisBridge_FromSrc041()
    {
        string source = ReadPage("Sedmica2.razor");

        Assert.Contains("Бек изрично посочва Елис сред своите интелектуални влияния", source);
        Assert.Contains("REBT е изброена сред терапиите от", source);
    }

    [Fact]
    public void Week2Page_HasNoWinnerLoserFraming()
    {
        string publicMarkup = ReadPublicMarkup("Sedmica2.razor");

        // "остаряла" legitimately appears once, as the wrong-answer distractor for Q1 (the page
        // explicitly rejects it as the correct framing) — checked separately below, not banned
        // outright the way a page-voice assertion of it would be.
        string[] forbiddenAsPageVoice = ["по-добрата школа", "REBT технически надделява", "грешният подход"];
        foreach (string term in forbiddenAsPageVoice)
        {
            Assert.DoesNotContain(term, publicMarkup);
        }

        Assert.Contains("не спор кой е прав", publicMarkup);
        Assert.Contains("<strong>Б.</strong> И двете школи са валидни академични рамки", publicMarkup);
    }

    [Fact]
    public void Week2Page_DoesNotDuplicateWeek1HistoricalDepthOrWeek3Architecture()
    {
        string source = ReadPage("Sedmica2.razor");

        // Week 1's own dream-study/two-streams narrative — must not be retold here.
        Assert.DoesNotContain("сънища", source);
        Assert.DoesNotContain("имипрамин", source);

        // Week 3/12's own term for Beck's deepest cognitive layer — Week 2 references Beck's
        // side only at summary depth and never introduces this term.
        Assert.DoesNotContain("основно вярване", source);
        Assert.DoesNotContain("междинно вярване", source);
    }

    [Fact]
    public void Week2Page_SectionNavAnchorsAreRouteSafe()
    {
        string source = ReadPage("Sedmica2.razor");

        string[] anchorIds =
        [
            "nakratko", "bek-kratko", "elis-i-rebt", "abc-modelat", "racionalni-irracionalni",
            "posledstvia-vyarvaniya", "most-i-sravnenie", "proverka", "izvori"
        ];

        foreach (string id in anchorIds)
        {
            Assert.Contains($"href=\"/kurs/sedmica-2#{id}\"", source);
            Assert.DoesNotContain($"href=\"#{id}\"", source);
            Assert.Contains($"id=\"{id}\"", source);
        }
    }

    [Fact]
    public void Week2Page_CrossLinksToWeek1AndWeek3_NoDeadLinks()
    {
        string source = ReadPage("Sedmica2.razor");

        Assert.Contains("href=\"/kurs/sedmica-1\"", source);
        Assert.Contains("href=\"/kurs/sedmica-3\"", source);
        Assert.Contains("href=\"/kurs\"", source);
    }

    [Fact]
    public void Week2Page_HasNoDiagnosticOrSelfAssessmentContent()
    {
        string source = ReadPage("Sedmica2.razor");

        string[] forbiddenTerms = ["BDI", "BAI", "диагности", "localStorage", "HttpClient"];
        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }

        Assert.Contains("не поставя диагноза", source);
    }

    [Fact]
    public void Week2Page_MakesNoFalseAccreditationClaims()
    {
        string source = ReadPage("Sedmica2.razor");

        string[] forbiddenTerms = ["ECTS", "Катедра по Клинична психология", "акредит"];
        foreach (string term in forbiddenTerms)
        {
            Assert.DoesNotContain(term, source);
        }
    }

    [Fact]
    public void Week2Page_HasFiveComprehensionQuestions()
    {
        string source = ReadPage("Sedmica2.razor");

        for (int i = 1; i <= 5; i++)
        {
            Assert.Contains($"Въпрос {i}.", source);
        }
        Assert.DoesNotContain("Въпрос 6.", source);
    }

    [Fact]
    public void Week2Page_CitesSrc041AndSrc042ByName()
    {
        string source = ReadPage("Sedmica2.razor");

        Assert.Contains("Джудит С. Бек", source);
        Assert.Contains("Daniel David, Ph.D.", source);
        Assert.Contains("Albert Ellis Institute", source);
    }

    [Fact]
    public void Week2Page_HasNoInlineStylesOrOverflowWorkaround()
    {
        string source = ReadPage("Sedmica2.razor");

        Assert.DoesNotContain("style=", source);
        Assert.DoesNotContain("overflow-x: hidden", source);
        Assert.DoesNotContain("overflow-x:hidden", source);
    }

    [Fact]
    public void KursPage_ListsWeek2AsAvailable()
    {
        string source = ReadPage("Kurs.razor");

        Assert.Contains("/kurs/sedmica-2", source);
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
