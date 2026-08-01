using System.Reflection;

namespace CbtLearningPlatform.Tests;

public sealed class ContentSliceTests
{
    [Theory]
    [InlineData("CbtLearningPlatform.Components.Pages.Kpt")]
    [InlineData("CbtLearningPlatform.Components.Pages.Modul2")]
    [InlineData("CbtLearningPlatform.Components.Pages.Modul2Lesson1")]
    [InlineData("CbtLearningPlatform.Components.Pages.Modul2Lesson2")]
    [InlineData("CbtLearningPlatform.Components.Pages.Modul2Lesson3")]
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
    public void Modul2Overview_LinksToAllThreeRealLessonRoutes()
    {
        string overviewSource = ReadPage("Modul2.razor");

        Assert.Contains(RouteOf("Modul2Lesson1.razor"), overviewSource);
        Assert.Contains(RouteOf("Modul2Lesson2.razor"), overviewSource);
        Assert.Contains(RouteOf("Modul2Lesson3.razor"), overviewSource);
    }

    [Fact]
    public void Lesson1_NextLessonLink_PointsToTheRealLesson2Route_NotADeadLink()
    {
        string lesson1Source = ReadPage("Modul2Lesson1.razor");

        Assert.Contains(RouteOf("Modul2Lesson2.razor"), lesson1Source);
        Assert.DoesNotContain("предстои", lesson1Source);
    }

    [Fact]
    public void Lesson2_NextLessonLink_PointsToTheRealLesson3Route_NotADeadLink()
    {
        string lesson2Source = ReadPage("Modul2Lesson2.razor");

        Assert.Contains(RouteOf("Modul2Lesson3.razor"), lesson2Source);
        Assert.DoesNotContain("предстои", lesson2Source);
    }

    [Fact]
    public void Lesson2_BackLinks_PointToRealExistingRoutes()
    {
        string lesson2Source = ReadPage("Modul2Lesson2.razor");

        Assert.Contains(RouteOf("Modul2Lesson1.razor"), lesson2Source);
        Assert.Contains(RouteOf("Modul2.razor"), lesson2Source);
    }

    [Fact]
    public void Lesson3_BackLinks_PointToRealExistingRoutes()
    {
        string lesson3Source = ReadPage("Modul2Lesson3.razor");

        Assert.Contains(RouteOf("Modul2Lesson2.razor"), lesson3Source);
        Assert.Contains(RouteOf("Modul2.razor"), lesson3Source);
    }

    [Fact]
    public void Lesson3_HonestlyMarksTheNextLessonAsNotYetAvailable()
    {
        // No Lesson 4 exists — Lesson 3 must not link to a route that doesn't exist.
        string lesson3Source = ReadPage("Modul2Lesson3.razor");

        Assert.Contains("предстои", lesson3Source);
    }

    [Theory]
    [InlineData("Kpt.razor")]
    [InlineData("Modul2.razor")]
    [InlineData("Modul2Lesson1.razor")]
    [InlineData("Modul2Lesson2.razor")]
    [InlineData("Modul2Lesson3.razor")]
    public void PsychologicalContentPage_IncludesDisclaimerCallout(string fileName)
    {
        // 23_CLINICAL_SAFETY_BOUNDARIES.md: visible disclaimer required on every page with
        // psychological content, not only the homepage.
        Assert.Contains("<DisclaimerCallout", ReadPage(fileName));
    }

    [Fact]
    public void ContentSlice_DoesNotContainTheUnsupportedDistortionCategorization()
    {
        string[] contentFiles = ["Kpt.razor", "Modul2.razor", "Modul2Lesson1.razor", "Modul2Lesson2.razor", "Modul2Lesson3.razor"];

        // ADR-006/ADR-008: this categorization has no confirmed source and must never appear in published content.
        string[] forbiddenTerms = ["Оценка/Прогнозиране/Филтриране/Правила", "Прогнозиране/Филтриране"];

        foreach (string fileName in contentFiles)
        {
            string content = ReadPage(fileName);

            foreach (string forbiddenTerm in forbiddenTerms)
            {
                Assert.DoesNotContain(forbiddenTerm, content);
            }
        }
    }

    private static string ReadPage(string fileName)
    {
        string pagesDirectory = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "Components", "Pages");
        return File.ReadAllText(Path.Combine(pagesDirectory, fileName));
    }

    private static string RouteOf(string fileName)
    {
        string routeLine = ReadPage(fileName)
            .Split('\n')
            .Single(line => line.TrimStart().StartsWith("@page "));

        return routeLine.Split('"')[1];
    }
}
