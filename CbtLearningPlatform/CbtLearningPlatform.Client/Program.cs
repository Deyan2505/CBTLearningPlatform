using CbtLearningPlatform.Client.Components;
using CbtLearningPlatform.Client.Curriculum;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CourseProgressService>();

await builder.Build().RunAsync();
