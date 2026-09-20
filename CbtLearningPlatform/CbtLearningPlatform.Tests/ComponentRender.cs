using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CbtLearningPlatform.Tests;

/// <summary>Renders a real Razor component to static HTML (Blazor's own HtmlRenderer — no browser, no bUnit) so tests can
/// assert on the actual initial markup a learner would see. Initial render only: event handling is covered by the pure
/// state-machine unit tests and by the Playwright E2E on the toolkit harness.</summary>
internal static class ComponentRender
{
    public static string Html<TComponent>(IDictionary<string, object?>? parameters = null) where TComponent : IComponent
    {
        using ServiceProvider services = new ServiceCollection().AddLogging().BuildServiceProvider();
        using HtmlRenderer renderer = new(services, services.GetRequiredService<ILoggerFactory>());

        return renderer.Dispatcher.InvokeAsync(async () =>
        {
            var root = await renderer.RenderComponentAsync<TComponent>(
                ParameterView.FromDictionary(parameters ?? new Dictionary<string, object?>()));
            return root.ToHtmlString();
        }).GetAwaiter().GetResult();
    }
}

internal static class HtmlText
{
    /// <summary>Blazor's renderer entity-encodes non-ASCII characters that come from C# expressions (literal markup text is
    /// emitted as written). Decode before asserting on visible text.</summary>
    public static string Decode(string html) => System.Net.WebUtility.HtmlDecode(html);
}
