import { test, expect, Page, Locator } from "@playwright/test";

// Batch 1 REMEDIATION in a real browser: the two new Weekly Mind Maps (Weeks 1 and 2) and Week 10's interactive case model.
// The rules under test are the owner's: a Mind Map exists as Preview + Review and starts collapsed; the case model genuinely
// changes state as the learner works it; and nothing is revealed before the learner commits.

const section = (page: Page, id: string): Locator => page.locator(`section[aria-labelledby="${id}-title"]`);

async function open(page: Page, route: string, width = 1280) {
  await page.setViewportSize({ width, height: 1000 });
  await page.goto(route);
  await expect(page.locator("#main-content h1")).toBeVisible();
}

const previewSection = (page: Page): Locator =>
  page.locator("#karta-sedmicata").locator("xpath=ancestor::section[contains(@class,'section-card')][1]");

const reviewDisclosure = (page: Page): Locator =>
  page.locator("details.concept-graph__retrieval-check").filter({ hasText: "Мисловна карта" }).first();

test.describe("Active Learning — Batch 1 remediation (Mind Maps + Week 10 case model)", () => {
  let consoleErrors: string[];

  test.beforeEach(async ({ page }) => {
    consoleErrors = [];
    page.on("console", (m) => m.type() === "error" && consoleErrors.push(m.text()));
    page.on("pageerror", (e) => consoleErrors.push(String(e)));
  });

  test.afterEach(() => {
    expect(consoleErrors, `console/page errors: ${consoleErrors.join(" | ")}`).toEqual([]);
  });

  // ------------------------------------------------------------------ Weeks 1 and 2 Mind Maps

  const maps: [number, string, string[]][] = [
    [1, "Как се ражда когнитивната терапия", ["Изходна позиция", "Емпирична проверка", "Какво излиза наяве", "Научно потвърждение"]],
    [2, "Бек и Елис — две когнитивни школи", ["Обща основа", "Бек — когнитивна терапия", "Елис — REBT", "Академична рамка"]],
  ];

  for (const [week, root, clusters] of maps) {
    test(`week ${week}: the Mind Map renders as Preview + Review, collapsed, from one model`, async ({ page }) => {
      await open(page, `/kurs/sedmica-${week}`);

      const preview = previewSection(page);
      await expect(preview.getByRole("heading", { level: 2, name: /00 · Карта на седмицата/ })).toBeVisible();
      await expect(preview.getByText(root, { exact: true }).first()).toBeVisible();
      for (const cluster of clusters) {
        await expect(preview.getByText(cluster, { exact: true }).first()).toBeVisible();
      }

      // branches are details/summary and start collapsed, so a child concept is not reachable until its cluster is opened
      const branch = preview.locator("details.mindmap-branch").first();
      expect(await branch.evaluate((e) => (e as HTMLDetailsElement).open)).toBe(false);
      await branch.locator("> summary").click();
      expect(await branch.evaluate((e) => (e as HTMLDetailsElement).open)).toBe(true);

      // Review: the same semantic model, inside a disclosure that is closed until the learner opens it
      const review = reviewDisclosure(page);
      expect(await review.evaluate((e) => (e as HTMLDetailsElement).open)).toBe(false);
      await review.scrollIntoViewIfNeeded();
      await review.locator("> summary").click();

      await expect(review.getByText(root, { exact: true }).first()).toBeVisible();
      for (const cluster of clusters) {
        await expect(review.getByText(cluster, { exact: true }).first()).toBeVisible();
      }
    });

    test(`week ${week}: the Mind Map's section links point at real sections of this week`, async ({ page }) => {
      await open(page, `/kurs/sedmica-${week}`);

      const hrefs = await previewSection(page)
        .locator("a[href*='#']")
        .evaluateAll((els) => els.map((e) => (e as HTMLAnchorElement).getAttribute("href")!));

      expect(hrefs.length).toBeGreaterThan(0);
      for (const href of hrefs) {
        expect(href.startsWith(`/kurs/sedmica-${week}#`), `route-unsafe anchor: ${href}`).toBe(true);
        await expect(page.locator(`#${href.split("#")[1]}`)).toHaveCount(1);
      }
    });
  }

  // ------------------------------------------------------------------ Week 10 interactive case model

  const TOOLS = ["Доказателства", "Алтернативно обяснение", "Декатастрофизиране", "Ефект от вярването", "Дистанциране", "Решаване на проблема"];

  test("week 10: the case model starts in its stated state and reveals nothing before a tool is applied", async ({ page }) => {
    await open(page, "/kurs/sedmica-10");
    const s = section(page, "week10-case-examination");
    await s.scrollIntoViewIfNeeded();

    await expect(s.getByText("Тъга (80%)")).toBeVisible();
    await expect(s.getByText(/вяра 90%/)).toBeVisible();
    await expect(s.getByText("0 от 6 приложени")).toBeVisible();
    await expect(s.getByText("Още нищо.", { exact: false })).toBeVisible();

    for (const tool of TOOLS) {
      await expect(s.getByRole("button", { name: tool, exact: true })).toBeEnabled();
    }
    await expect(s.locator(".active-learning__finding")).toHaveCount(0);
    await expect(s.locator(".active-learning__status")).toHaveCount(0);
    await expect(s.getByText(/пада от 90% на около 20%/)).toHaveCount(0);
    await expect(s.getByText(/Карен обикновено пита/)).toHaveCount(0);

    const model = await s.boundingBox();
    const submit = await page.getByRole("button", { name: "Предай теста" }).boundingBox();
    expect(model!.y).toBeLessThan(submit!.y);
  });

  test("week 10: opening a tool asks for a prediction first and can be cancelled without changing the model", async ({ page }) => {
    await open(page, "/kurs/sedmica-10");
    const s = section(page, "week10-case-examination");
    await s.scrollIntoViewIfNeeded();

    await s.getByRole("button", { name: "Доказателства", exact: true }).click();
    await expect(s.getByText("Първо предскажете какво ще разкрие този въпрос, после потвърдете.")).toBeVisible();
    await expect(s.getByRole("button", { name: "Потвърди и приложи" })).toBeDisabled();
    await expect(s.locator(".active-learning__finding")).toHaveCount(0);

    await s.getByRole("button", { name: "Откажи" }).click();
    await expect(s.getByText("0 от 6 приложени")).toBeVisible();
    await expect(s.locator("button.active-learning__tool--applied")).toHaveCount(0);
    await expect(s.getByRole("button", { name: "Доказателства", exact: true })).toBeEnabled();
  });

  test("week 10: applying tools changes the model's state; the outcome appears only when all six are applied", async ({ page }) => {
    await open(page, "/kurs/sedmica-10");
    const s = section(page, "week10-case-examination");
    await s.scrollIntoViewIfNeeded();

    // a correct prediction -> the approved finding joins the board
    await s.getByRole("button", { name: "Доказателства", exact: true }).click();
    await s.getByText("Конкретни факти за и против мисълта.").click();
    await s.getByRole("button", { name: "Потвърди и приложи" }).click();

    await expect(s.getByText("1 от 6 приложени")).toBeVisible();
    await expect(s.locator(".active-learning__finding")).toHaveCount(1);
    await expect(s.locator(".active-learning__status--correct")).toHaveCount(1);
    await expect(s.getByText(/Карен обикновено пита/)).toBeVisible();
    // an applied tool is locked; its accessible name also gains the "приложено" prefix, so match on the class
    await expect(s.locator("button.active-learning__tool--applied")).toHaveCount(1);
    await expect(s.locator("button.active-learning__tool--applied")).toBeDisabled();
    await expect(s.getByText(/пада от 90% на около 20%/)).toHaveCount(0);

    // a wrong prediction still advances the model and still shows the real finding
    await s.getByRole("button", { name: "Алтернативно обяснение", exact: true }).click();
    await s.getByText("Най-лошият, най-добрият и най-реалистичният изход.").click();
    await s.getByRole("button", { name: "Потвърди и приложи" }).click();
    await expect(s.locator(".active-learning__status--incorrect")).toHaveCount(1);
    await expect(s.getByText(/забързаността невинаги означава липса на интерес/)).toBeVisible();

    for (const tool of TOOLS.slice(2)) {
      await s.getByRole("button", { name: tool, exact: true }).click();
      // eslint-disable-next-line playwright/no-force-option
      await s.locator("input[type=radio]").first().check();
      await s.getByRole("button", { name: "Потвърди и приложи" }).click();
    }

    await expect(s.locator(".active-learning__finding")).toHaveCount(6);
    await expect(s.getByText(/Разгледано от всичките 6 страни/)).toBeVisible();
    await expect(s.getByText(/пада от 90% на около 20%/)).toBeVisible();
    await expect(s.getByText("Глава 11 — случаят на Сали и Карен (стр. 168, 171–175)")).toBeVisible();

    await s.getByRole("button", { name: "Започни отначало" }).click();
    await expect(s.getByText("0 от 6 приложени")).toBeVisible();
    await expect(s.locator(".active-learning__finding")).toHaveCount(0);
    await expect(s.getByText(/пада от 90% на около 20%/)).toHaveCount(0);
  });

  test("week 10: the case model is fully operable with the keyboard", async ({ page }) => {
    await open(page, "/kurs/sedmica-10");
    const s = section(page, "week10-case-examination");
    await s.scrollIntoViewIfNeeded();

    await s.getByRole("button", { name: "Дистанциране", exact: true }).focus();
    await page.keyboard.press("Enter");
    await expect(s.getByRole("group").first()).toBeVisible();

    const radio = s.locator("input[type=radio]").first();
    await radio.focus();
    await page.keyboard.press("Space");
    await expect(radio).toBeChecked();

    await s.getByRole("button", { name: "Потвърди и приложи" }).focus();
    await page.keyboard.press("Enter");
    await expect(s.getByText("1 от 6 приложени")).toBeVisible();
  });

  // ------------------------------------------------------------------ responsive

  for (const width of [1440, 1024, 768, 390]) {
    test(`responsive @${width}px: the new Mind Maps and case model never clip or overflow`, async ({ page }) => {
      for (const [week, expand] of [[1, true], [2, true], [10, false]] as [number, boolean][]) {
        await open(page, `/kurs/sedmica-${week}`, width);

        if (expand) {
          const review = reviewDisclosure(page);
          await review.scrollIntoViewIfNeeded();
          await review.locator("> summary").click();
          await page.waitForTimeout(150);
        }

        const offenders = await page.evaluate(() => {
          const cw = document.documentElement.clientWidth;
          return [...document.querySelectorAll(".concept-graph *, .active-learning *")]
            .filter((e) => { const r = e.getBoundingClientRect(); return r.width > 0 && (r.right > cw + 1 || r.left < -1); })
            .map((e) => e.tagName.toLowerCase() + "." + String(e.className).split(" ")[0]);
        });
        expect(offenders, `week ${week} @${width}px: elements outside the viewport`).toEqual([]);

        // the expanded review map must also fit the container it was placed in
        const clipped = await page.evaluate(() => {
          const host = document.querySelector("details[open] .concept-graph") as HTMLElement | null;
          if (!host) return 0;
          const hb = host.getBoundingClientRect();
          return [...host.querySelectorAll("*")].filter((e) => e.getBoundingClientRect().right > hb.right + 0.5).length;
        });
        expect(clipped, `week ${week} @${width}px: the map overflows its own container`).toBe(0);
      }
    });
  }
});
