using System.Reflection;

namespace CbtLearningPlatform.Tests;

public sealed class ErrorHandlingTests
{
    [Fact]
    public void ErrorPage_ExistsInHostAssembly()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform");

        Type? errorPageType = assembly.GetType("CbtLearningPlatform.Components.Pages.Error");

        Assert.NotNull(errorPageType);
    }

    [Fact]
    public void ErrorPage_HasNoExceptionMember()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform");
        Type errorPageType = assembly.GetType("CbtLearningPlatform.Components.Pages.Error")!;

        bool hasExceptionMember = errorPageType
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Any(property => typeof(Exception).IsAssignableFrom(property.PropertyType));

        Assert.False(hasExceptionMember);
    }
}
