using System.Reflection;

namespace CbtLearningPlatform.Tests;

public sealed class ContentSliceTests
{
    [Theory]
    [InlineData("CbtLearningPlatform.Components.Pages.Kpt")]
    [InlineData("CbtLearningPlatform.Components.Pages.Modul2")]
    [InlineData("CbtLearningPlatform.Components.Pages.Modul2Lesson1")]
    [InlineData("CbtLearningPlatform.Components.Shared.LearningObjectives")]
    [InlineData("CbtLearningPlatform.Components.Shared.SourceReferences")]
    public void ContentSliceType_ExistsInHostAssembly(string typeName)
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform");

        Type? type = assembly.GetType(typeName);

        Assert.NotNull(type);
    }

    [Fact]
    public void LearningObjectives_HasStableObjectivesParameter()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform");
        Type type = assembly.GetType("CbtLearningPlatform.Components.Shared.LearningObjectives")!;

        Assert.NotNull(type.GetProperty("Objectives"));
    }

    [Fact]
    public void SourceReferences_HasStableCitationsParameter()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform");
        Type type = assembly.GetType("CbtLearningPlatform.Components.Shared.SourceReferences")!;

        Assert.NotNull(type.GetProperty("Citations"));
    }

    [Fact]
    public void Modul2Overview_LinksToTheRealFirstLessonRoute()
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Pages");

        string lessonSource = File.ReadAllText(Path.Combine(pagesDirectory, "Modul2Lesson1.razor"));
        string overviewSource = File.ReadAllText(Path.Combine(pagesDirectory, "Modul2.razor"));

        string lessonRouteLine = lessonSource
            .Split('\n')
            .Single(line => line.TrimStart().StartsWith("@page "));

        string lessonRoute = lessonRouteLine.Split('"')[1];

        Assert.Contains(lessonRoute, overviewSource);
    }

    [Theory]
    [InlineData("Kpt.razor")]
    [InlineData("Modul2.razor")]
    [InlineData("Modul2Lesson1.razor")]
    public void PsychologicalContentPage_IncludesDisclaimerCallout(string fileName)
    {
        // 23_CLINICAL_SAFETY_BOUNDARIES.md: visible disclaimer required on every page with
        // psychological content, not only the homepage.
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Pages");
        string content = File.ReadAllText(Path.Combine(pagesDirectory, fileName));

        Assert.Contains("<DisclaimerCallout", content);
    }

    [Fact]
    public void ContentSlice_DoesNotContainTheUnsupportedDistortionCategorization()
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Pages");

        string[] contentFiles = ["Kpt.razor", "Modul2.razor", "Modul2Lesson1.razor"];

        // ADR-006/ADR-008: this categorization has no confirmed source and must never appear in published content.
        string[] forbiddenTerms = ["Оценка/Прогнозиране/Филтриране/Правила", "Прогнозиране/Филтриране"];

        foreach (string fileName in contentFiles)
        {
            string content = File.ReadAllText(Path.Combine(pagesDirectory, fileName));

            foreach (string forbiddenTerm in forbiddenTerms)
            {
                Assert.DoesNotContain(forbiddenTerm, content);
            }
        }
    }
}
