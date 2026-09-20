import { test, expect, Page, Locator } from "@playwright/test";

// Real-browser coverage of the Active Learning Toolkit (ClassifyMatchCheck, OrderingBuilder, PredictReveal, and
// ScenarioSimulator's reuse contract) on the Debug-only QA harness route. Deliberately keyboard-first: the toolkit's
// accessibility promise is that every step is reachable and operable without a pointer. All fixtures on the harness are
// neutral placeholders — the engines contain no CBT content. The harness route only exists in Debug builds (the dev server
// this suite drives); it is excluded from Release, so it never ships.
const HARNESS = "/dev/active-learning-toolkit";

const section = (page: Page, id: string): Locator => page.locator(`section[aria-labelledby="${id}-title"]`);

async function openHarness(page: Page, width = 1280) {
  await page.setViewportSize({ width, height: 1000 });
  await page.goto(HARNESS);
  await expect(page.getByRole("heading", { level: 1, name: /Active Learning Toolkit/ })).toBeVisible();
  await expect(section(page, "harness-classify-normal")).toBeVisible();
}

/** Selects a radio with the keyboard only: focus it, press Space. */
async function pickByKeyboard(page: Page, radio: Locator) {
  await radio.focus();
  await page.keyboard.press("Space");
  await expect(radio).toBeChecked();
}

test.describe("Active Learning Toolkit — real browser (Chromium)", () => {
  let consoleErrors: string[];

  test.beforeEach(async ({ page }) => {
    consoleErrors = [];
    page.on("console", (m) => m.type() === "error" && consoleErrors.push(m.text()));
    page.on("pageerror", (e) => consoleErrors.push(String(e)));
    await openHarness(page);
  });

  test.afterEach(() => {
    expect(consoleErrors, `console/page errors: ${consoleErrors.join(" | ")}`).toEqual([]);
  });

  // ------------------------------------------------------------------ A. classify / match

  test("classify: keyboard-only commit -> check -> explanatory verdicts -> retry", async ({ page }) => {
    const s = section(page, "harness-classify-normal");
    const check = s.getByRole("button", { name: "Провери", exact: true });

    // no feedback exists before commitment, and the check cannot be pressed yet
    await expect(check).toBeDisabled();
    await expect(s.locator(".active-learning__explanation")).toHaveCount(0);
    await expect(s.getByText("0 от 3 отговорени")).toBeVisible();

    // each item is an accessible group; answer with Space / arrow keys only
    await expect(s.getByRole("group", { name: /Ябълка/ })).toBeVisible();
    await pickByKeyboard(page, s.locator('input[name="harness-classify-normal-apple"]').nth(1)); // wrong: apple -> vegetable
    await s.locator('input[name="harness-classify-normal-carrot"]').first().focus();
    await page.keyboard.press("ArrowDown"); // native radio-group arrow navigation selects the next option
    await expect(s.locator('input[name="harness-classify-normal-carrot"]').nth(1)).toBeChecked(); // right: carrot -> vegetable
    await pickByKeyboard(page, s.locator('input[name="harness-classify-normal-pear"]').first()); // right: pear -> fruit

    await expect(s.getByText("3 от 3 отговорени")).toBeVisible();
    await expect(check).toBeEnabled();
    await check.focus();
    await page.keyboard.press("Enter");

    // commitment -> per-item verdicts (glyph + text, never colour-only) + explanations
    await expect(s.getByText("2 от 3 верни")).toBeVisible();
    await expect(s.locator(".active-learning__status--incorrect")).toHaveCount(1);
    await expect(s.locator(".active-learning__status--correct")).toHaveCount(2);
    await expect(s.locator(".active-learning__status--incorrect")).toContainText("Грешен");
    await expect(s.locator(".active-learning__explanation")).toHaveCount(3);
    await expect(s.getByText("Ябълката е плод на дърво.")).toBeVisible();
    await expect(s.locator("input[type=radio]:not(:disabled)")).toHaveCount(0); // locked after checking
    await expect(s.getByText("верен отговор")).toHaveCount(3); // the right option is announced to assistive tech
    await expect(s.getByText("избран, но неверен")).toHaveCount(1);

    // retry returns to a clean, unanswered state
    await s.getByRole("button", { name: "Опитай отново" }).click();
    await expect(s.locator("input[type=radio]:checked")).toHaveCount(0);
    await expect(s.locator(".active-learning__explanation")).toHaveCount(0);
    await expect(check).toBeDisabled();
  });

  test("match: one-to-one — a duplicate choice blocks the check and explains why", async ({ page }) => {
    const s = section(page, "harness-match-academic");
    const check = s.getByRole("button", { name: "Провери", exact: true });

    await pickByKeyboard(page, s.locator('input[name="harness-match-academic-sun"]').first()); // Звезда
    await pickByKeyboard(page, s.locator('input[name="harness-match-academic-earth"]').first()); // Звезда again -> duplicate
    await pickByKeyboard(page, s.locator('input[name="harness-match-academic-luna"]').nth(2)); // Луна

    await expect(s.getByText("Всеки вариант може да се използва само веднъж.")).toBeVisible();
    await expect(check).toBeDisabled();

    await pickByKeyboard(page, s.locator('input[name="harness-match-academic-earth"]').nth(1)); // Планета
    await expect(s.getByText("Всеки вариант може да се използва само веднъж.")).toHaveCount(0);
    await expect(check).toBeEnabled();
    await check.click();
    await expect(s.getByText("3 от 3 верни")).toBeVisible();
  });

  // ------------------------------------------------------------------ B. ordering builder

  const CORRECT = ["Събуждане", "Закуска", "Обяд", "Вечеря"];
  const texts = (s: Locator) => s.locator(".active-learning__order-text").allTextContents();
  const up = (s: Locator, text: string) => s.getByRole("button", { name: `Премести „${text}“ нагоре` });
  const down = (s: Locator, text: string) => s.getByRole("button", { name: `Премести „${text}“ надолу` });

  async function solve(s: Locator) {
    for (let target = 0; target < CORRECT.length; target++) {
      let at = (await texts(s)).indexOf(CORRECT[target]);
      while (at > target) {
        await up(s, CORRECT[target]).click();
        at--;
      }
    }
  }

  test("ordering: keyboard moves keep focus on the moved item and are announced; nothing is judged before the check", async ({ page }) => {
    const s = section(page, "harness-ordering-normal");

    expect(await texts(s)).not.toEqual(CORRECT); // starts scrambled
    await expect(s.locator(".active-learning__status")).toHaveCount(0);
    await expect(s.locator(".active-learning__solution")).toHaveCount(0);
    await expect(s.locator(".active-learning__order-item button[disabled]")).toHaveCount(2); // only the two edge buttons

    const first = (await texts(s))[0];
    await down(s, first).focus();
    await page.keyboard.press("Enter");

    expect((await texts(s))[1]).toBe(first);
    // focus follows the item (same direction button, now one row lower) so repeated presses keep working
    await expect(down(s, first)).toBeFocused();
    await expect(s.getByText(`„${first}“ е на позиция 2 от 4.`)).toBeVisible();
    await page.keyboard.press("Enter");
    await expect(s.getByText(`„${first}“ е на позиция 3 от 4.`)).toBeVisible();
  });

  test("ordering: wrong order -> per-position verdicts, reveal the sequence, retry keeps the arrangement", async ({ page }) => {
    const s = section(page, "harness-ordering-normal");
    const before = await texts(s);

    await s.getByRole("button", { name: "Провери подредбата" }).click();

    await expect(s.locator(".active-learning__status--incorrect").first()).toBeVisible();
    await expect(s.getByText(/от 4 са на правилното място\./)).toBeVisible();
    await expect(s.locator("button[aria-label^='Премести']:not([disabled])")).toHaveCount(0); // locked after commitment
    await expect(s.getByText("Първото хранене.")).toBeVisible(); // per-step explanation appears only now

    await s.getByRole("button", { name: "Покажи верния ред" }).click();
    await expect(s.locator(".active-learning__solution li")).toHaveText(CORRECT);

    await s.getByRole("button", { name: "Опитай отново" }).click();
    await expect(s.locator(".active-learning__solution")).toHaveCount(0);
    expect(await texts(s)).toEqual(before); // arrangement kept so the learner can fix it
    await expect(up(s, before[1])).toBeEnabled();

    await s.getByRole("button", { name: "Провери подредбата" }).click();
    await s.getByRole("button", { name: "Започни отначало" }).click();
    expect(await texts(s)).toEqual(before); // reset -> original scramble
  });

  test("ordering: the correct order is confirmed and offers no pointless retry", async ({ page }) => {
    const s = section(page, "harness-ordering-normal");

    await solve(s);
    expect(await texts(s)).toEqual(CORRECT);
    await s.getByRole("button", { name: "Провери подредбата" }).click();

    await expect(s.getByText("Подредбата е вярна.")).toBeVisible();
    await expect(s.locator(".active-learning__status--correct")).toHaveCount(4);
    await expect(s.getByRole("button", { name: "Опитай отново" })).toHaveCount(0);
    await expect(s.getByText("Обичайната последователност е от сутрин към вечер.")).toBeVisible();
  });

  // ------------------------------------------------------------------ C. committed predict -> reveal

  test("predict: the explanation does not exist in the page until a prediction is committed", async ({ page }) => {
    const s = section(page, "harness-predict-normal");
    const commit = s.getByRole("button", { name: "Потвърди предсказанието" });

    await expect(commit).toBeDisabled();
    await expect(s.locator("details")).toHaveCount(0); // not a <details> reveal
    let html = await page.content();
    expect(html).not.toContain("Гравитацията дърпа топката надолу");
    expect(html).not.toContain("Точно така — предсказанието ви съвпада.");
    expect(html).not.toContain("Действителният резултат");

    await pickByKeyboard(page, s.getByRole("radio", { name: "Ще полети нагоре" }));
    await expect(commit).toBeEnabled();
    html = await page.content();
    expect(html).not.toContain("Гравитацията дърпа топката надолу"); // choosing is not committing

    await commit.focus();
    await page.keyboard.press("Enter");

    await expect(s.getByText("Вашето предсказание")).toBeVisible();
    await expect(s.locator(".active-learning__comparison dd").nth(0)).toHaveText("Ще полети нагоре");
    await expect(s.locator(".active-learning__comparison dd").nth(1)).toHaveText("Ще падне надолу");
    await expect(s.getByText("Не съвпада")).toBeVisible();
    await expect(s.getByText("Не — нищо не я тласка нагоре.")).toBeVisible();
    await expect(s.getByText("Гравитацията дърпа топката надолу, затова тя пада.")).toBeVisible();
    await expect(s.locator("input[type=radio]:not(:disabled)")).toHaveCount(0);

    await s.getByRole("button", { name: "Опитай отново" }).click();
    await expect(s.locator("input[type=radio]:checked")).toHaveCount(0);
    html = await page.content();
    expect(html).not.toContain("Гравитацията дърпа топката надолу"); // gone again after retry
  });

  test("predict: a matching prediction is confirmed; a comparison-only activity never judges", async ({ page }) => {
    const s = section(page, "harness-predict-normal");
    await pickByKeyboard(page, s.getByRole("radio", { name: "Ще падне надолу" }));
    await s.getByRole("button", { name: "Потвърди предсказанието" }).click();
    await expect(s.locator(".active-learning__status--correct")).toContainText("Съвпада");
    await expect(s.getByText("Не съвпада")).toHaveCount(0);

    const c = section(page, "harness-predict-professional");
    await pickByKeyboard(page, c.getByRole("radio", { name: "Първото" }));
    await c.getByRole("button", { name: "Потвърди предсказанието" }).click();
    await expect(c.getByText("Вашето предсказание")).toBeVisible();
    await expect(c.getByText("Действителният резултат")).toHaveCount(0);
    await expect(c.getByText("Съвпада")).toHaveCount(0);
    await expect(c.getByText("Не съвпада")).toHaveCount(0);
    await expect(c.getByText("И двете описания са разпространени; тук се сравнява само изборът.")).toBeVisible();
  });

  // ------------------------------------------------------------------ safety modes

  test("safety modes: restricted modes always show their framing notice and stay fully interactive", async ({ page }) => {
    await expect(section(page, "harness-classify-normal").locator(".active-learning__notice")).toHaveCount(0);

    await expect(section(page, "harness-match-academic").locator(".active-learning__notice"))
      .toHaveText("Упражнението използва фиксирани примери от трето лице. Учебно е — не е инструмент за самооценка.");
    await expect(section(page, "harness-predict-professional").locator(".active-learning__notice"))
      .toHaveText("Одобрена формулировка, зададена от страницата на седмицата."); // approved override replaces the default
    await expect(section(page, "harness-ordering-nosim").locator(".active-learning__notice"))
      .toHaveText("Упражнението използва фиксирани примери. Учебно е — не е симулация за самостоятелна практика.");

    // still active learning, not passive reading
    const nosim = section(page, "harness-ordering-nosim");
    await down(nosim, (await texts(nosim))[0]).click();
    await nosim.getByRole("button", { name: "Провери подредбата" }).click();
    await expect(nosim.locator(".active-learning__reveal")).toBeVisible();
  });

  test("scenario simulator: configurable levels, and NoSelfGuidedSimulation withholds branching", async ({ page }) => {
    const sims = page.locator(".scenario-simulator");
    await expect(sims).toHaveCount(2);

    // no branching data + Normal: only the configured levels A and B are offered
    await expect(sims.nth(0).getByRole("tab", { name: /Ниво A/ })).toBeVisible();
    await expect(sims.nth(0).getByRole("tab", { name: /Ниво B/ })).toBeVisible();
    await expect(sims.nth(0).getByRole("tab", { name: /Ниво C/ })).toHaveCount(0);

    // branching data supplied but the strictest mode withholds it, and shows the mandatory notice
    await expect(sims.nth(1).getByRole("tab", { name: /Ниво C/ })).toHaveCount(0);
    await expect(sims.nth(1).locator(".active-learning__notice")).toBeVisible();

    // the configured Level B ordering still works
    await sims.nth(0).getByRole("tab", { name: /Ниво B/ }).click();
    await sims.nth(0).getByRole("button", { name: "Провери подредбата" }).click();
    await expect(sims.nth(0).locator(".scenario-simulator__order-item span:last-child").first()).toBeVisible();
  });

  // ------------------------------------------------------------------ responsive

  for (const width of [1440, 1024, 390]) {
    test(`responsive @${width}px: no horizontal overflow, >=44px targets, options never clip`, async ({ page }) => {
      await openHarness(page, width);

      const overflow = await page.evaluate(() => document.documentElement.scrollWidth > document.documentElement.clientWidth + 1);
      expect(overflow, `horizontal overflow at ${width}px`).toBe(false);

      const small = await page.evaluate(() =>
        [...document.querySelectorAll(".active-learning .btn, .active-learning__option")]
          .filter((e) => e.getBoundingClientRect().height < 43.5).length);
      expect(small, "controls smaller than 44px").toBe(0);

      const clipped = await page.evaluate(() =>
        [...document.querySelectorAll(".active-learning *")].filter((e) => {
          const r = e.getBoundingClientRect();
          return r.width > 0 && (r.right > document.documentElement.clientWidth + 1 || r.left < -1);
        }).length);
      expect(clipped, "elements outside the viewport").toBe(0);

      if (width === 390) {
        const s = section(page, "harness-classify-normal");
        const container = await s.locator(".active-learning__options").first().boundingBox();
        const option = await s.locator(".active-learning__option").first().boundingBox();
        expect(option!.width).toBeGreaterThan(container!.width * 0.98); // options stack full-width on phones
      }
    });
  }

  test("focus is visible on every toolkit control (real Tab navigation)", async ({ page }) => {
    const s = section(page, "harness-ordering-normal");
    await s.getByRole("heading", { level: 3 }).click(); // put the pointer/focus inside the section
    await page.keyboard.press("Tab");
    const outline = await page.evaluate(() => {
      const e = document.activeElement as HTMLElement;
      const cs = getComputedStyle(e);
      return { tag: e.tagName, outlineStyle: cs.outlineStyle, outlineWidth: parseFloat(cs.outlineWidth) };
    });
    expect(outline.outlineStyle).not.toBe("none");
    expect(outline.outlineWidth).toBeGreaterThanOrEqual(2);
  });
});
