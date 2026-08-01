using System.Reflection;

namespace CbtLearningPlatform.Tests;

public sealed class DesignSystemTests
{
    [Fact]
    public void DisclaimerCallout_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform");

        Type? disclaimerType = assembly.GetType("CbtLearningPlatform.Components.Shared.DisclaimerCallout");

        Assert.NotNull(disclaimerType);
    }

    [Fact]
    public void AppCss_DefinesCoreDesignTokens()
    {
        string cssPath = Path.Combine(TestPaths.FindSolutionRoot(), "CbtLearningPlatform", "wwwroot", "app.css");

        string css = File.ReadAllText(cssPath);

        Assert.Contains("--color-primary:", css);
        Assert.Contains("--color-background:", css);
        Assert.Contains("--color-focus:", css);
        Assert.Contains("--space-4:", css);
    }
}
