import { test, expect, Page, Locator } from "@playwright/test";

// Phase 2, Batch 1 — Active Learning enrichment of Weeks 1, 2 and 10 in a real browser (Chromium).
// The engines are the shared Active Learning Toolkit; these tests drive the real lesson pages the way a learner does:
// keyboard first, nothing judged before the learner commits, feedback only afterwards, retry/reset, and no layout damage
// at 1440 / 1024 / 768 / 390. Wording asserted here is wording that was already approved on the pages.

const section = (page: Page, id: string): Locator => page.locator(`section[aria-labelledby="${id}-title"]`);

async function open(page: Page, route: string, width = 1280) {
  await page.setViewportSize({ width, height: 1000 });
  await page.goto(route);
  await expect(page.locator("#main-content h1")).toBeVisible();
}

const texts = (s: Locator) => s.locator(".active-learning__order-text").allTextContents();
const up = (s: Locator, text: string) => s.getByRole("button", { name: `Премести „${text}“ нагоре` });
const down = (s: Locator, text: string) => s.getByRole("button", { name: `Премести „${text}“ надолу` });

async function solve(s: Locator, correct: string[]) {
  for (let target = 0; target < correct.length; target++) {
    let at = (await texts(s)).indexOf(correct[target]);
    while (at > target) {
      await up(s, correct[target]).click();
      at--;
    }
  }
}

/** Selects a radio with the keyboard only: focus it, press Space. */
async function pickByKeyboard(page: Page, radio: Locator) {
  await radio.focus();
  await page.keyboard.press("Space");
  await expect(radio).toBeChecked();
}

test.describe("Active Learning — Batch 1 (Weeks 1, 2, 10)", () => {
  let consoleErrors: string[];

  test.beforeEach(async ({ page }) => {
    consoleErrors = [];
    page.on("console", (m) => m.type() === "error" && consoleErrors.push(m.text()));
    page.on("pageerror", (e) => consoleErrors.push(String(e)));
  });

  test.afterEach(() => {
    expect(consoleErrors, `console/page errors: ${consoleErrors.join(" | ")}`).toEqual([]);
  });

  // ------------------------------------------------------------------ Week 1 — ordering the historical sequence

  const W1 = ["Психоаналитично начало", "Неочакван резултат", "Два потока на мислене", "1977 г.", "1979 г."];

  test("week 1: the timeline stays; the ordering precedes the quiz, judges nothing before the check, then explains", async ({ page }) => {
    await open(page, "/kurs/sedmica-1");
    const s = section(page, "week1-history-order");

    // the visual model is preserved (six approved milestones) and the old stepper is still there
    await expect(page.locator(".week-timeline__item")).toHaveCount(6);
    await expect(page.locator(".research-turn-stepper")).toBeVisible();

    // placement: inside the check section, before the scored quiz
    const ordering = await s.boundingBox();
    const submit = await page.getByRole("button", { name: "Предай теста" }).boundingBox();
    expect(ordering!.y).toBeLessThan(submit!.y);

    // starts scrambled, all five approved milestones present, and NOTHING is judged or explained yet
    const start = await texts(s);
    expect([...start].sort()).toEqual([...W1].sort());
    expect(start).not.toEqual(W1);
    await expect(s.locator(".active-learning__status")).toHaveCount(0);
    await expect(s.locator(".active-learning__order-explanation")).toHaveCount(0);
    await expect(s.locator(".active-learning__solution, .active-learning__reveal")).toHaveCount(0);
    await expect(s.getByText("научният подход изисква тя да бъде преразгледана")).toHaveCount(0);
    await expect(s.getByText("рандомизирано контролирано изследване")).toHaveCount(0);

    // the sixth milestone is deliberately not part of the exercise
    await expect(s.getByText("Разширяване към тревожността")).toHaveCount(0);

    // keyboard: move an item, focus follows it, the move is announced
    const first = start[0];
    await down(s, first).focus();
    await page.keyboard.press("Enter");
    await expect(down(s, first)).toBeFocused();
    await expect(s.getByText(`„${first}“ е на позиция 2 от 5.`)).toBeVisible();

    // wrong order -> verdicts + the approved milestone text as the explanation + the approved key message
    await s.getByRole("button", { name: "Провери подредбата" }).click();
    await expect(s.locator(".active-learning__status--incorrect").first()).toBeVisible();
    await expect(s.locator(".active-learning__order-explanation")).toHaveCount(5);
    await expect(s.getByText("научният подход изисква тя да бъде преразгледана")).toBeVisible();
    await expect(s.locator("button[aria-label^='Премести']:not([disabled])")).toHaveCount(0); // locked after commitment

    await s.getByRole("button", { name: "Покажи верния ред" }).click();
    await expect(s.locator(".active-learning__solution li")).toHaveText(W1);

    // retry keeps the arrangement so the learner can fix it; then solve it
    await s.getByRole("button", { name: "Опитай отново" }).click();
    await expect(s.locator(".active-learning__solution")).toHaveCount(0);
    await solve(s, W1);
    expect(await texts(s)).toEqual(W1);
    await s.getByRole("button", { name: "Провери подредбата" }).click();
    await expect(s.getByText("Подредбата е вярна.")).toBeVisible();
    await expect(s.locator(".active-learning__status--correct")).toHaveCount(5);
    await expect(s.getByRole("button", { name: "Опитай отново" })).toHaveCount(0);
  });

  test("week 1: evidence application commits either source-grounded model change and reset opens the other path", async ({ page }) => {
    await open(page, "/kurs/sedmica-1");
    const simulator = page.locator(".stateful-model");

    await expect(simulator).toHaveAttribute("data-state", "initial");
    await expect(simulator.getByText("Изходна хипотеза", { exact: true })).toBeVisible();
    await expect(simulator.getByText("Повече теми на враждебност в сънищата на депресираните пациенти.", { exact: true })).toBeVisible();
    await expect(simulator.getByText("Приложи тези данни към модела: изследването на сънищата", { exact: true })).toBeVisible();
    await expect(simulator.getByText("Приложи тези данни към модела: наблюдението на втория поток от самооценъчни мисли", { exact: true })).toBeVisible();
    await expect(simulator.locator(".stateful-model__feedback")).toHaveCount(0);
    await expect(simulator).not.toContainText("дефектност, лишение и загуба");
    await expect(simulator).not.toContainText("SRC-041");
    await expect(simulator).not.toContainText(/Какво избира Бек|ако Бек беше|променят историята/);

    const dream = simulator.getByRole("radio", { name: "Приложи тези данни към модела: изследването на сънищата" });
    await pickByKeyboard(page, dream);
    await expect(simulator).toHaveAttribute("data-state", "initial");
    await expect(simulator.locator(".stateful-model__feedback")).toHaveCount(0);
    await simulator.getByRole("button", { name: "Промени модела" }).click();
    await expect(simulator).toHaveAttribute("data-state", "dream-evidence");
    await expect(simulator.locator('[data-field="observation"] dd')).toContainText("По-малко теми на враждебност");
    await expect(simulator.locator('[data-field="status"] dd')).toHaveText("Не е подкрепено от резултатите.");
    await expect(simulator.locator('[data-field="next-focus"] dd')).toContainText("нужда да страдат");
    await expect(simulator.getByText("SRC-041 · Гл. 1, стр. 5 · U1/U2/U14/U15/U16")).toBeVisible();

    await simulator.getByRole("button", { name: "Изследвай друг път" }).click();
    await expect(simulator).toHaveAttribute("data-state", "initial");
    await expect(simulator.locator(".stateful-model__feedback")).toHaveCount(0);
    await expect(simulator.locator(".stateful-model__history li")).toHaveCount(0);

    const thoughtStream = simulator.getByRole("radio", { name: "Приложи тези данни към модела: наблюдението на втория поток от самооценъчни мисли" });
    await pickByKeyboard(page, thoughtStream);
    await simulator.getByRole("button", { name: "Промени модела" }).click();
    await expect(simulator).toHaveAttribute("data-state", "thought-stream-evidence");
    await expect(simulator.locator('[data-field="streams"] dd')).toContainText("втори, много по-бърз поток от самооценъчни мисли");
    await expect(simulator.locator('[data-field="emotion"] dd')).toContainText("тясно свързани с емоционалните реакции");
    await expect(simulator.locator('[data-field="replication"] dd')).toContainText("повтаря и с други пациенти");
    await expect(simulator.locator('[data-field="focus"] dd')).toContainText("Идентифициране и оценяване на автоматичните мисли");
    await expect(simulator.getByText("SRC-041 · Гл. 1, стр. 5 · U17/U18/U3")).toBeVisible();

    const simulatorBox = await simulator.boundingBox();
    const section04 = await page.locator("#predi-i-sled").boundingBox();
    const assessment = await page.getByRole("button", { name: "Предай теста" }).boundingBox();
    expect(simulatorBox!.y).toBeLessThan(section04!.y);
    expect(simulatorBox!.y).toBeLessThan(assessment!.y);

    await page.setViewportSize({ width: 390, height: 844 });
    await expect(simulator).toBeVisible();
    expect(await page.locator("html").evaluate((element) => element.scrollWidth)).toBeLessThanOrEqual(390);
  });

  test("week 1: the scored Final Assessment is untouched and still works after the exercise", async ({ page }) => {
    await open(page, "/kurs/sedmica-1");
    await expect(page.getByText("0 от 4 отговорени")).toBeVisible();
    await expect(page.getByRole("button", { name: "Предай теста" })).toBeDisabled();
  });

  // ------------------------------------------------------------------ Week 2 — chains + committed attribution

  test("week 2: two process chains sit side by side on desktop and stack on phones", async ({ page }) => {
    await open(page, "/kurs/sedmica-2", 1440);
    const chains = page.locator(".process-chain");
    await expect(chains).toHaveCount(2);
    await expect(chains.nth(0).locator(".concept-map__node")).toHaveText(["Ситуация", "Автоматична мисъл", "Настроение и поведение"]);
    await expect(chains.nth(1).locator(".concept-map__node")).toHaveText(["A — Активиращо събитие", "B — Вярвания", "C — Последствия"]);
    await expect(chains.nth(0).locator(".concept-map__node--highlight")).toHaveText("Автоматична мисъл");
    await expect(chains.nth(1).locator(".concept-map__node--highlight")).toHaveText("B — Вярвания");
    // each chain is a list with only its three nodes exposed (the arrows are decorative)
    await expect(chains.nth(0).getByRole("listitem")).toHaveCount(3);

    const a = await chains.nth(0).boundingBox();
    const b = await chains.nth(1).boundingBox();
    expect(Math.abs(a!.y - b!.y)).toBeLessThan(4); // same row = side by side
    expect(a!.x + a!.width).toBeLessThanOrEqual(b!.x + 1);

    await open(page, "/kurs/sedmica-2", 390);
    const a2 = await page.locator(".process-chain").nth(0).boundingBox();
    const b2 = await page.locator(".process-chain").nth(1).boundingBox();
    expect(b2!.y).toBeGreaterThan(a2!.y + a2!.height - 1); // stacked
  });

  test("week 2: attribution — keyboard commit, nothing explained before the check, the table row afterwards, retry", async ({ page }) => {
    await open(page, "/kurs/sedmica-2");
    const s = section(page, "week2-school-attribution");
    const check = s.getByRole("button", { name: "Провери", exact: true });

    // placed after the approved comparison table and before the quiz
    const table = await page.locator(".comparison-matrix", { hasText: "Академично сравнение" }).boundingBox();
    const box = await s.boundingBox();
    const submit = await page.getByRole("button", { name: "Предай теста" }).boundingBox();
    expect(box!.y).toBeGreaterThan(table!.y);
    expect(box!.y).toBeLessThan(submit!.y);

    await expect(check).toBeDisabled();
    await expect(s.getByText("0 от 6 отговорени")).toBeVisible();
    await expect(s.locator(".active-learning__explanation, .active-learning__status")).toHaveCount(0);
    await expect(s.getByText("В сравнението: Бек")).toHaveCount(0);

    // beck = 0, ellis = 1; row 4 of the table (the shared idea) is deliberately not offered
    const answers: [string, number][] = [
      ["r1-ellis", 1], ["r2-beck", 1], ["r3-ellis", 1], ["r1-beck", 0], ["r2-ellis", 1], ["r3-beck", 0]
    ]; // r2-beck is answered wrongly (Ellis) on purpose
    for (const [id, index] of answers) {
      await pickByKeyboard(page, s.locator(`input[name="week2-school-attribution-${id}"]`).nth(index));
    }
    await expect(s.getByText("6 от 6 отговорени")).toBeVisible();
    await expect(check).toBeEnabled();
    await check.focus();
    await page.keyboard.press("Enter");

    await expect(s.getByText("5 от 6 верни")).toBeVisible();
    await expect(s.locator(".active-learning__status--incorrect")).toHaveCount(1);
    await expect(s.locator(".active-learning__status--correct")).toHaveCount(5);
    await expect(s.locator(".active-learning__explanation")).toHaveCount(6);
    await expect(s.getByText("В сравнението: Бек — „Тръгва от конкретната автоматична мисъл в дадена ситуация.“; Елис — „Тръгва от общите вярвания (рационални/ирационални) зад реакцията към събитието.“").first()).toBeVisible();
    await expect(s.getByText("Сравнението по-горе, ред 2").first()).toBeVisible();
    await expect(s.locator("input[type=radio]:not(:disabled)")).toHaveCount(0);

    await s.getByRole("button", { name: "Опитай отново" }).click();
    await expect(s.locator("input[type=radio]:checked")).toHaveCount(0);
    await expect(s.locator(".active-learning__explanation")).toHaveCount(0);
    await expect(check).toBeDisabled();
  });

  // ------------------------------------------------------------------ Week 10 — classification + retrieval ordering

  test("week 10: 10.4 is a committed classification, no plain reveals remain, feedback only after the check", async ({ page }) => {
    await open(page, "/kurs/sedmica-10");
    const s = section(page, "week10-question-types");
    const check = s.getByRole("button", { name: "Провери", exact: true });

    await expect(page.getByText("Покажи класификацията")).toHaveCount(0);
    await expect(page.getByRole("heading", { level: 2, name: /10\.4 · Въпрос или прикрит съвет/ })).toBeVisible();

    const box = await s.boundingBox();
    const submit = await page.getByRole("button", { name: "Предай теста" }).boundingBox();
    expect(box!.y).toBeLessThan(submit!.y);

    await expect(check).toBeDisabled();
    await expect(s.getByText("0 от 4 отговорени")).toBeVisible();
    await expect(s.locator(".active-learning__explanation, .active-learning__status")).toHaveCount(0);
    await expect(s.getByText("предполага предварително заключение")).toHaveCount(0);

    // directive(0) exploratory(1) advice(2) alternative(3); q2 answered wrongly on purpose
    const answers: [string, number][] = [["q1", 0], ["q2", 0], ["q3", 2], ["q4", 3]];
    for (const [id, index] of answers) {
      await pickByKeyboard(page, s.locator(`input[name="week10-question-types-${id}"]`).nth(index));
    }
    await check.focus();
    await page.keyboard.press("Enter");

    await expect(s.getByText("3 от 4 верни")).toBeVisible();
    await expect(s.getByText("Насочващ въпрос — предполага предварително заключение.")).toBeVisible();
    await expect(s.getByText("Изследващ въпрос — търси конкретната основа на заключението.")).toBeVisible();
    await expect(s.locator(".active-learning__status--incorrect")).toHaveCount(1);
    await expect(s.getByText("Виж 10.2 — категория „Алтернативно обяснение“")).toBeVisible();

    await s.getByRole("button", { name: "Опитай отново" }).click();
    await expect(s.locator("input[type=radio]:checked")).toHaveCount(0);
    await expect(s.locator(".active-learning__explanation")).toHaveCount(0);
  });

  const W10 = ["Доказателства", "Алтернативно обяснение", "Декатастрофизиране", "Дистанциране"];

  test("week 10: the 10.11 retrieval reveal is now a committed ordering (keyboard, verdicts, retry, solve)", async ({ page }) => {
    await open(page, "/kurs/sedmica-10");
    await page.locator("details.concept-graph__retrieval-check", { hasText: "Retrieval practice" }).locator("summary").click();
    const s = section(page, "week10-category-order");
    await expect(s).toBeVisible();
    await expect(page.getByText("Покажи верния ред")).toHaveCount(0); // the plain reveal is gone; the engine offers it only after a check

    const start = await texts(s);
    expect([...start].sort()).toEqual([...W10].sort());
    expect(start).not.toEqual(W10);
    await expect(s.locator(".active-learning__status, .active-learning__solution, .active-learning__reveal")).toHaveCount(0);

    await s.getByRole("button", { name: "Провери подредбата" }).click();
    await expect(s.getByText(/от 4 са на правилното място\./)).toBeVisible();
    await expect(s.getByText("Фигура 11.1 — виж пълния списък в 10.2")).toBeVisible();

    await s.getByRole("button", { name: "Покажи верния ред" }).click();
    await expect(s.locator(".active-learning__solution li")).toHaveText(W10);
    await s.getByRole("button", { name: "Опитай отново" }).click();
    await solve(s, W10);
    await s.getByRole("button", { name: "Провери подредбата" }).click();
    await expect(s.getByText("Подредбата е вярна.")).toBeVisible();
  });

  // ------------------------------------------------------------------ state is local only

  test("nothing is persisted: reloading returns every activity to its untouched state", async ({ page }) => {
    await open(page, "/kurs/sedmica-2");
    const s = section(page, "week2-school-attribution");
    await pickByKeyboard(page, s.locator('input[name="week2-school-attribution-r1-ellis"]').nth(1));
    await expect(s.getByText("1 от 6 отговорени")).toBeVisible();

    await page.reload();
    await expect(page.locator("#main-content h1")).toBeVisible();
    await expect(section(page, "week2-school-attribution").getByText("0 от 6 отговорени")).toBeVisible();

    const stored = await page.evaluate(() => JSON.stringify({ ...localStorage }) + JSON.stringify({ ...sessionStorage }));
    expect(stored).not.toContain("week2-school-attribution");
  });

  // ------------------------------------------------------------------ responsive: 1440 / 1024 / 768 / 390

  const pages: [string, string][] = [["/kurs/sedmica-1", "week 1"], ["/kurs/sedmica-2", "week 2"], ["/kurs/sedmica-10", "week 10"]];
  for (const width of [1440, 1024, 768, 390]) {
    for (const [route, label] of pages) {
      test(`responsive @${width}px ${label}: no horizontal overflow, >=44px toolkit targets, nothing clipped`, async ({ page }) => {
        await open(page, route, width);
        if (route.endsWith("sedmica-10")) {
          await page.locator("details.concept-graph__retrieval-check", { hasText: "Retrieval practice" }).locator("summary").click();
        }

        // Page-level overflow: any offender other than the pre-existing Week 1 ResearchTurnStepper fails. That island overflows
        // by 5px around 1024px on production too (verified on the pre-enrichment page at commit 0231c8f); it is owner-approved,
        // locked and unrelated to this batch, so it is reported, not changed here.
        const offenders = await page.evaluate(() => {
          const cw = document.documentElement.clientWidth;
          return [...document.querySelectorAll("body *")]
            .filter((e) => { const r = e.getBoundingClientRect(); return r.width > 0 && r.right > cw + 1; })
            .filter((e) => !e.closest(".research-turn-stepper"))
            .map((e) => e.tagName.toLowerCase() + "." + String(e.className).split(" ")[0]);
        });
        expect(offenders, `elements overflowing the ${width}px viewport (excluding the known ResearchTurnStepper defect)`).toEqual([]);
        if (route.endsWith("sedmica-1")) {
          const stepperOverflows = await page.evaluate(() => document.documentElement.scrollWidth > document.documentElement.clientWidth + 1);
          if (stepperOverflows) {
            test.info().annotations.push({ type: "known-preexisting", description: `Week 1 ResearchTurnStepper overflows the page at ${width}px (also on production before this batch).` });
          }
        }

        const small = await page.evaluate(() =>
          [...document.querySelectorAll(".active-learning .btn, .active-learning__option")]
            .filter((e) => e.getBoundingClientRect().height < 43.5).length);
        expect(small, "toolkit controls smaller than 44px").toBe(0);

        const clipped = await page.evaluate(() =>
          [...document.querySelectorAll(".active-learning *, .process-chain *")].filter((e) => {
            const r = e.getBoundingClientRect();
            return r.width > 0 && (r.right > document.documentElement.clientWidth + 1 || r.left < -1);
          }).length);
        expect(clipped, "elements outside the viewport").toBe(0);
      });
    }
  }
});
