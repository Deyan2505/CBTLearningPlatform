using System.Text.RegularExpressions;

namespace CbtLearningPlatform.Tests;

/// <summary>Guards against internal Knowledge Unit identifiers (e.g. "U04", "U22") leaking into
/// learner-facing UI. KU IDs remain valid internal metadata — a bare, standalone quoted literal such as
/// the SourceUnit/SourceRef argument passed to ScenarioSimulator/StatefulModelSimulator records — as
/// long as the component never renders that field to a visitor. This guard targets learner-visible page
/// markup and rendered-feedback source data only: it strips the page's leading dev-comment header, C#
/// line comments, and standalone quoted KU-id metadata literals before scanning, so it does not flag
/// internal code comments or data model IDs indiscriminately.</summary>
public sealed class LearnerFacingKuIdGuardTests
{
    private static readonly Regex KuId = new(@"\bU\d{2}\b", RegexOptions.Compiled);
    private static readonly Regex LineComment = new(@"^[ \t]*//.*$", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex StandaloneKuIdLiteral = new(@"""U\d{2}(?:[/–]U\d{2})*""", RegexOptions.Compiled);

    [Theory]
    [InlineData("Sedmica6.razor")]
    [InlineData("Sedmica7.razor")]
    [InlineData("Sedmica8.razor")]
    public void WeekPage_LearnerVisibleContent_NeverShowsAnInternalKnowledgeUnitId(string fileName)
    {
        string learnerVisible = ReadLearnerVisibleContent(fileName);

        Assert.DoesNotMatch(KuId, learnerVisible);
    }

    private static string ReadLearnerVisibleContent(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform.Client", "Components", "Pages");
        string source = File.ReadAllText(Path.Combine(pagesDirectory, fileName));

        int commentEnd = source.IndexOf("*@", StringComparison.Ordinal);
        string withoutHeader = commentEnd >= 0 ? source[(commentEnd + 2)..] : source;
        string withoutLineComments = LineComment.Replace(withoutHeader, "");
        return StandaloneKuIdLiteral.Replace(withoutLineComments, "\"\"");
    }
}
