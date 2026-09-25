import { test, expect } from "@playwright/test";

const cases = [
  { week: 5, id: "week5-collaboration-simulator", choice: "Разгледай по-късен етап" },
  { week: 7, id: "week7-cycle-simulator", choice: "Изследвай срещите с приятели" },
  { week: 9, id: "week9-thought-record-simulator", choice: "Зареди Сценарий Б: мисълта за приятеля" },
];

for (const width of [1440, 1024, 390]) {
  for (const item of cases) {
    test(`visual QA: week ${item.week} simulator changed state @${width}px`, async ({ page }) => {
      await page.setViewportSize({ width, height: 1100 });
      await page.goto(`/kurs/sedmica-${item.week}`);
      const simulator = page.locator(`section[aria-labelledby="${item.id}-title"]`);
      await simulator.scrollIntoViewIfNeeded();
      await simulator.getByText(item.choice, { exact: true }).click();
      await simulator.getByRole("button", { name: "Промени модела" }).click();
      await expect(simulator.locator(".stateful-model__feedback")).toBeVisible();
      await simulator.screenshot({
        path: `test-results/visual-qa/week-${item.week}-${width}px-changed-state.png`,
        animations: "disabled",
      });
    });
  }
}
