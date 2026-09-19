import { defineConfig, devices } from "@playwright/test";

// E2E coverage for the real Blazor WASM UI, separate from the 925 xUnit unit/integration tests in
// CbtLearningPlatform.Tests. Drives the actual dev server (Microsoft.AspNetCore.Components.WebAssembly.DevServer)
// in Chromium — no bUnit, no fake DOM.
//
// Port is overridable via E2E_PORT (defaults to 5131, the app's own launchSettings.json port) so a
// run can target a freshly started server on another port without touching one already running
// elsewhere — e.g. `E2E_PORT=5132 npx playwright test`.
const port = process.env.E2E_PORT ?? "5131";
const baseURL = `http://localhost:${port}`;

export default defineConfig({
  testDir: "./tests",
  timeout: 60_000,
  // Blazor WASM's first page load in a fresh browser profile/dev-server pairs downloads and boots the
  // whole .NET runtime before any DOM renders — the default 5s per-assertion timeout can be too tight
  // for that first hit (subsequent navigations reuse the cached framework files and are fast).
  expect: { timeout: 15_000 },
  fullyParallel: false, // shared localStorage-key semantics per test; each test opens its own context anyway
  reporter: [["list"]],
  use: {
    baseURL,
    trace: "retain-on-failure",
    screenshot: "only-on-failure",
  },
  projects: [{ name: "chromium", use: { ...devices["Desktop Chrome"] } }],
  webServer: {
    command: `dotnet run --no-launch-profile --urls ${baseURL}`,
    cwd: "../CbtLearningPlatform/CbtLearningPlatform.Client",
    url: baseURL,
    reuseExistingServer: true,
    timeout: 120_000,
  },
});
