namespace CbtLearningPlatform.Tests;

internal static class TestPaths
{
    public static string FindSolutionRoot()
    {
        DirectoryInfo? directory = new(AppContext.BaseDirectory);

        while (directory is not null && directory.GetFiles("*.sln").Length == 0)
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new DirectoryNotFoundException("Could not locate solution root from test output directory.");
    }

    /// <summary>Counts the questions in a page's FinalAssessmentModel field by scanning its own
    /// initializer (from the field name to the closing "]);"), so weeks with more than one model
    /// field (e.g. a Weekly Mind Map) can't cross-contaminate the count.</summary>
    public static int CountFinalAssessmentQuestions(string source, string fieldName)
    {
        int start = source.IndexOf(fieldName, StringComparison.Ordinal);
        if (start < 0) throw new InvalidOperationException($"Field '{fieldName}' not found.");

        int end = source.IndexOf("\n    ]);", start, StringComparison.Ordinal);
        if (end < 0) throw new InvalidOperationException($"Closing ']);' not found for '{fieldName}'.");

        string block = source[start..end];
        int choice = System.Text.RegularExpressions.Regex.Matches(block, @"AssessmentQuestion\.Choice\(").Count;
        int trueFalse = System.Text.RegularExpressions.Regex.Matches(block, @"AssessmentQuestion\.TrueFalse\(").Count;
        return choice + trueFalse;
    }
}
