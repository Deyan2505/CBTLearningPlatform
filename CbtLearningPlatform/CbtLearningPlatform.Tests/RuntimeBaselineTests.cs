using System.Reflection;
using System.Runtime.Versioning;

namespace CbtLearningPlatform.Tests;

public sealed class RuntimeBaselineTests
{
    [Fact]
    public void TestProject_TargetsDotNet10()
    {
        var attribute = typeof(RuntimeBaselineTests).Assembly
            .GetCustomAttribute<TargetFrameworkAttribute>();

        Assert.Equal(
            ".NETCoreApp,Version=v10.0",
            attribute?.FrameworkName);
    }

    [Fact]
    public void HostApplicationAssembly_IsAvailable()
    {
        Assembly assembly = Assembly.Load("CbtLearningPlatform");

        Assert.Equal(
            "CbtLearningPlatform",
            assembly.GetName().Name);
    }
}
