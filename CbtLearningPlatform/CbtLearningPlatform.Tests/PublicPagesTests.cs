using System.Reflection;

namespace CbtLearningPlatform.Tests;

public sealed class PublicPagesTests
{
    [Fact]
    public void ProgramaPage_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Type? programaType = assembly.GetType("CbtLearningPlatform.Client.Components.Pages.Programa");

        Assert.NotNull(programaType);
    }

    [Fact]
    public void ModuleCard_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");

        Type? moduleCardType = assembly.GetType("CbtLearningPlatform.Client.Components.Shared.ModuleCard");

        Assert.NotNull(moduleCardType);
    }

    [Fact]
    public void ModuleCard_HasStablePublicParameters()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform.Client");
        Type moduleCardType = assembly.GetType("CbtLearningPlatform.Client.Components.Shared.ModuleCard")!;

        string[] expectedParameters = ["Title", "Description", "StatusLabel", "DestinationUrl", "CtaLabel"];

        foreach (string parameterName in expectedParameters)
        {
            PropertyInfo? property = moduleCardType.GetProperty(parameterName);
            Assert.NotNull(property);
        }
    }
}
