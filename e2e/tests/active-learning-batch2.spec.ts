import { test, expect, Page, Locator } from "@playwright/test";

// Active Learning Phase 2, Batch 2 (Weeks 5, 7, 9) in a real browser: each new activity starts with no feedback in the DOM,
// requires a committed response, and only then shows the approved explanation. Also: keyboard, no overflow, no console errors.

const section = (page: Page, id: string): Locator => page.locator(`section[aria-labelledby="${id}-title"]`);

async function open(page: Page, week: number, width = 1280) {
  await page.setViewportSize({ width, height: 1000 });
  await page.goto(`/kurs/sedmica-${week}`);
  await expect(page.locator("#main-content h1")).toBeVisible();
}

test.describe("Active Learning — Batch 2 (Weeks 5, 7, 9)", () => {
  let errors: string[];
  test.beforeEach(({ page }) => {
    errors = [];
    page.on("console", (m) => m.type() === "error" && errors.push(m.text()));
    page.on("pageerror", (e) => errors.push(String(e)));
  });
  test.afterEach(() => expect(errors, errors.join(" | ")).toEqual([]));

  test("week 5 simulator: stage and contribution commits visibly change the model", async ({ page }) => {
    await open(page, 5);
    const s = section(page, "week5-collaboration-simulator");
    await s.scrollIntoViewIfNeeded();
    await expect(s.locator(".stateful-model")).toHaveAttribute("data-state", "start");
    await expect(s.locator(".stateful-model__feedback")).toHaveCount(0);

    await s.getByText("Разгледай началото на лечението", { exact: true }).click();
    await s.getByRole("button", { name: "Промени модела" }).click();
    await expect(s.locator(".stateful-model")).toHaveAttribute("data-state", "early");
    await expect(s.getByText("По-активен", { exact: true })).toBeVisible();

    await s.getByText("Терапевтът предлага посока за сесиите", { exact: true }).click();
    await s.getByRole("button", { name: "Промени модела" }).click();
    await expect(s.locator(".stateful-model")).toHaveAttribute("data-state", "early-direction");
    await expect(s.getByText("Видим принос", { exact: true })).toBeVisible();

    await s.getByRole("button", { name: "Изследвай друг път" }).click();
    await expect(s.locator(".stateful-model")).toHaveAttribute("data-state", "start");
  });

  test("week 5: the collaboration classification withholds feedback until Провери, then explains from the approved text", async ({ page }) => {
    await open(page, 5);
    const s = section(page, "week5-collaboration-stage");
    await s.scrollIntoViewIfNeeded();

    await expect(s.getByText("0 от 6 отговорени")).toBeVisible();
    await expect(s.getByRole("button", { name: "Провери" })).toBeDisabled();
    await expect(s.locator(".active-learning__explanation")).toHaveCount(0);
    await expect(s.getByText("Терапевтът е по-активен")).toHaveCount(0);

    // answer all six correctly: early, early, both, later x3 by matching each prompt to its stage
    const key: Record<string, string> = {
      "Пациентът сам избира за какво да говори": "По-нататък в лечението",
      "Терапевтът предлага посока за сесиите": "В началото на лечението",
      "Терапията остава екипна работа": "И в двата етапа",
      "Пациентът обобщава важните моменти": "По-нататък в лечението",
      "Терапевтът обобщава наученото": "В началото на лечението",
      "Пациентът идентифицира изкривявания в собственото си мислене": "По-нататък в лечението",
    };
    const items = s.locator("ol > li");
    for (let i = 0; i < 6; i++) {
      const li = items.nth(i);
      const text = (await li.locator("legend").innerText()).replace(/^\d+\.\s*/, "");
      const prompt = Object.keys(key).find((k) => text.startsWith(k))!;
      await li.getByText(key[prompt], { exact: true }).click();
    }
    await expect(s.getByText("6 от 6 отговорени")).toBeVisible();
    await s.getByRole("button", { name: "Провери" }).click();
    await expect(s.getByText("6 от 6 верни")).toBeVisible();
    await expect(s.getByText("Терапевтът е по-активен", { exact: false }).first()).toBeVisible();
  });

  test("week 7: a prediction must be committed before the real outcome exists; the closing sentence appears only after scenario 2", async ({ page }) => {
    await open(page, 7);
    const one = section(page, "week7-predict-friends");
    const two = section(page, "week7-predict-run");

    await two.scrollIntoViewIfNeeded();
    await expect(page.getByText("Двата сценария показват двете посоки")).toHaveCount(0);
    await expect(one.getByText("Действителни оценки")).toHaveCount(0);
    await expect(one.getByRole("button", { name: "Потвърди предсказанието" })).toBeDisabled();

    await one.getByText("По-високи от предсказаните", { exact: true }).click();
    await one.getByRole("button", { name: "Потвърди предсказанието" }).click();
    await expect(one.getByText("✓ Съвпада")).toBeVisible();
    await expect(one.getByText("Действителни оценки: 3 до 5")).toBeVisible();
    await expect(page.getByText("Двата сценария показват двете посоки")).toHaveCount(0);

    await two.getByText("По-високи от предсказаните", { exact: true }).click();     // wrong on purpose
    await two.getByRole("button", { name: "Потвърди предсказанието" }).click();
    await expect(two.getByText("Не съвпада")).toBeVisible();
    await expect(two.getByText("Действителни оценки: по 1")).toBeVisible();
    await expect(page.getByText("Двата сценария показват двете посоки")).toBeVisible();

    await two.getByRole("button", { name: "Опитай отново" }).click();
    await expect(two.getByText("Действителни оценки")).toHaveCount(0);
  });

  test("week 7 simulator: prediction → action → observed outcome updates the cycle model", async ({ page }) => {
    await open(page, 7);
    const s = section(page, "week7-cycle-simulator");
    await s.scrollIntoViewIfNeeded();

    await s.getByText("Изследвай срещите с приятели", { exact: true }).click();
    await s.getByRole("button", { name: "Промени модела" }).click();
    await expect(s.locator(".stateful-model")).toHaveAttribute("data-state", "friends-prediction");
    await expect(s.getByText("Удоволствие: 0–3", { exact: true })).toBeVisible();

    await s.getByText("Проведи трите планирани срещи и запиши оценките", { exact: true }).click();
    await s.getByRole("button", { name: "Промени модела" }).click();
    await expect(s.locator(".stateful-model")).toHaveAttribute("data-state", "friends-action");

    await s.getByText("Добави наблюдавания резултат", { exact: true }).click();
    await s.getByRole("button", { name: "Промени модела" }).click();
    await expect(s.locator(".stateful-model")).toHaveAttribute("data-state", "friends-outcome");
    await expect(s.getByText("3–5", { exact: true })).toBeVisible();
    await expect(s.getByText("Действителността е по-добра от предсказаното", { exact: true })).toBeVisible();
  });

  test("week 7: the review ordering of the cycle is committed (no solution until Провери подредбата)", async ({ page }) => {
    await open(page, 7);
    const review = page.locator("details.concept-graph__retrieval-check").filter({ hasText: "Retrieval practice" });
    await review.scrollIntoViewIfNeeded();
    await review.locator("> summary").click();
    const s = section(page, "week7-cycle-order");

    await expect(s.getByText("На мястото си")).toHaveCount(0);
    await expect(s.getByText("Порочен кръг: бездействието води")).toHaveCount(0);
    await s.getByRole("button", { name: "Провери подредбата" }).click();
    await expect(s.getByText("Порочен кръг: бездействието води")).toBeVisible();
  });

  test("week 9: the distortion matching is one-to-one, blocks a repeated option, and explains from 9.3 after the check", async ({ page }) => {
    await open(page, 9);
    const s = section(page, "week9-distortion-match");
    await s.scrollIntoViewIfNeeded();

    await expect(s.getByRole("button", { name: "Провери" })).toBeDisabled();
    await expect(s.locator(".active-learning__explanation")).toHaveCount(0);
    await expect(s.getByText("Персонализация — вярвате, че другите")).toHaveCount(0);

    const key: Record<string, string> = {
      "Ремонтникът беше груб": "Персонализация",
      "Ако не съм напълно успешен": "Мислене в черно и бяло",
      "Ужасно е, че направих грешка": "Изказвания „трябва“ и „моля“",
      "Ще бъда толкова разстроен": "Катастрофизиране",
      "Той мисли, че не знам": "Четене на мисли",
      "Знам, че правя много неща": "Емоционално мислене",
    };
    const items = s.locator("ol > li");
    for (let i = 0; i < 6; i++) {
      const li = items.nth(i);
      const text = await li.locator("legend").innerText();
      const prompt = Object.keys(key).find((k) => text.includes(k))!;
      await li.getByText(key[prompt], { exact: true }).click();
    }
    await expect(s.getByRole("button", { name: "Провери" })).toBeEnabled();
    await s.getByRole("button", { name: "Провери" }).click();
    await expect(s.getByText("6 от 6 верни")).toBeVisible();
    await expect(s.getByText("Персонализация — вярвате, че другите")).toBeVisible();
  });

  test("week 9: reusing a distortion blocks the check", async ({ page }) => {
    await open(page, 9);
    const s = section(page, "week9-distortion-match");
    await s.scrollIntoViewIfNeeded();
    const items = s.locator("ol > li");
    for (let i = 0; i < 6; i++) await items.nth(i).locator("input[type=radio]").first().check();
    await expect(s.getByText("Всеки вариант може да се използва само веднъж.")).toBeVisible();
    await expect(s.getByRole("button", { name: "Провери" })).toBeDisabled();
  });

  test("week 9 simulator: fixed third-person choices fill the structure without free text", async ({ page }) => {
    await open(page, 9);
    const s = section(page, "week9-thought-record-simulator");
    await s.scrollIntoViewIfNeeded();
    await expect(s.locator("textarea, input[type=text]")).toHaveCount(0);
    await expect(s.locator("[data-field=distortion] dd")).toHaveText("Не е дадено във фиксирания сценарий");

    await s.getByText("Зареди Сценарий Б: мисълта за приятеля", { exact: true }).click();
    await s.getByRole("button", { name: "Промени модела" }).click();
    await expect(s.locator(".stateful-model")).toHaveAttribute("data-state", "case-b");
    await s.getByText("Добави одобреното изкривяване", { exact: true }).click();
    await s.getByRole("button", { name: "Промени модела" }).click();
    await expect(s.locator("[data-field=distortion] dd")).toHaveText("Четене на мисли");
    await s.getByText("Добави първия оценъчен въпрос", { exact: true }).click();
    await s.getByRole("button", { name: "Промени модела" }).click();
    await expect(s.locator("[data-field=response] dd")).toContainText("Какви са доказателствата");
  });

  test("keyboard: a Week 7 prediction can be chosen and committed without a mouse", async ({ page }) => {
    await open(page, 7);
    const s = section(page, "week7-predict-friends");
    await s.scrollIntoViewIfNeeded();
    await s.locator("input[type=radio]").first().focus();
    await page.keyboard.press("Space");
    await s.getByRole("button", { name: "Потвърди предсказанието" }).focus();
    await page.keyboard.press("Enter");
    await expect(s.getByText("Действителни оценки: 3 до 5")).toBeVisible();
  });

  for (const width of [1440, 1024, 768, 390]) {
    test(`responsive @${width}px: the new activities never overflow the viewport`, async ({ page }) => {
      for (const week of [5, 7, 9]) {
        await open(page, week, width);
        const offenders = await page.evaluate(() => {
          const cw = document.documentElement.clientWidth;
          return [...document.querySelectorAll(".active-learning, .active-learning *")]
            .filter((e) => { const r = e.getBoundingClientRect(); return r.width > 0 && (r.right > cw + 1 || r.left < -1); })
            .map((e) => e.tagName.toLowerCase() + "." + String(e.className).split(" ")[0]);
        });
        expect(offenders, `week ${week} @${width}px`).toEqual([]);
      }
    });
  }
});
