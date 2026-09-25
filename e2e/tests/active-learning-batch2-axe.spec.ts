import { test, expect } from "@playwright/test";
import AxeBuilder from "@axe-core/playwright";

const simulatorIds: Record<number, string> = {
  5: "week5-collaboration-simulator",
  7: "week7-cycle-simulator",
  9: "week9-thought-record-simulator",
};

for (const week of [5, 7, 9]) {
  for (const theme of ["dark", "light"] as const) {
    test(`axe: week ${week} simulator has no serious/critical violations in ${theme} theme`, async ({ page }) => {
      test.setTimeout(120_000);
      await page.goto(`/kurs/sedmica-${week}`);
      await expect(page.locator("#main-content h1")).toBeVisible();
      if (theme === "light") {
        await page.getByRole("button", { name: "Светла тема" }).click();
        await expect(page.locator("html")).toHaveAttribute("data-theme", "light");
      }

      const results = await new AxeBuilder({ page })
        .include(`section[aria-labelledby="${simulatorIds[week]}-title"]`)
        .analyze();
      const blocking = results.violations.filter((v) => v.impact === "serious" || v.impact === "critical");
      expect(blocking, blocking.map((v) => `${v.id}: ${v.help}`).join("\n")).toEqual([]);
    });
  }
}
