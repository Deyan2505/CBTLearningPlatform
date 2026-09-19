import { test, expect, Page } from "@playwright/test";

// Real-browser E2E coverage of the full learner journey, wired against the ACTUAL Blazor WASM UI
// (served by the standalone dev server — there is no server project, see AGENTS.md). Complements,
// never replaces, CbtLearningPlatform.Tests/StudentJourneyStressTests.cs, which covers the same
// journey's pure business logic (FinalAssessmentState, CourseProgressStore, CertificateEligibility)
// directly, without a browser. Here we only click the real controls a learner would click and read
// the real rendered DOM — no source mutation, no bUnit, no reaching into CourseProgress internals.
//
// The 20-question course final exam's correct-answer key (FinalExamCatalog.cs, items f01..f20) is
// duplicated here deliberately, as plain QA-only data — never imported from production code — so this
// file can drive the real exam UI to a real, predictable score.
const FINAL_EXAM_ANSWER_KEY = [0, 3, 2, 1, 3, 3, 1, 2, 0, 0, 2, 1, 2, 1, 3, 3, 2, 0, 0, 1];
const TOTAL_WEEKS = 15;
const MINIMUM_EXAM_SCORE = 75;

function wrongOptionIndex(correctIndex: number): number {
  return (correctIndex + 1) % 4; // every final-exam item has exactly 4 options (regression-tested in FinalExamTests.cs)
}

async function completeWeekThroughRealUi(page: Page, weekNumber: number) {
  await page.goto(`/kurs/sedmica-${weekNumber}`);
  const toggle = page.locator(".week-completion__button");
  await expect(toggle).toHaveText(/Отбележи като завършена/);
  await toggle.click();
  await expect(toggle).toHaveText(/Седмицата е завършена/);
}

async function assertCourseProgress(page: Page, completed: number) {
  await page.goto("/kurs");
  await expect(page.locator(".course-progress__bar")).toHaveAttribute("value", String(completed));
  await expect(page.locator(".course-progress__bar")).toHaveAttribute("max", String(TOTAL_WEEKS));
  await expect(page.locator(".course-progress__header")).toContainText(`${completed} / ${TOTAL_WEEKS} седмици`);
}

/** Selects one option per final-exam question by radio `name` (`izpit-q{i}`) — never by option label
 * text, since several real options are intentionally near-identical ("(а)", "(б)", "(в)", "(г)"). */
async function answerFinalExam(page: Page, correctCount: number) {
  for (let i = 0; i < FINAL_EXAM_ANSWER_KEY.length; i++) {
    const correct = FINAL_EXAM_ANSWER_KEY[i];
    const optionIndex = i < correctCount ? correct : wrongOptionIndex(correct);
    await page.locator(`input[name="izpit-q${i}"]`).nth(optionIndex).check();
  }
}

test.describe("Student journey — real browser (Chromium)", () => {
  test("full journey: 0/15 -> all weeks -> exam retry -> certificate -> responsive/print -> reload reset", async ({
    page,
  }) => {
    test.setTimeout(300_000);
    const consoleErrors: string[] = [];
    page.on("console", (msg) => {
      if (msg.type() === "error") consoleErrors.push(msg.text());
    });
    page.on("pageerror", (err) => consoleErrors.push(String(err)));

    // ---- Step 1-2: clean start, 0/15 ----
    await page.goto("/kurs");
    await expect(page.locator(".course-progress__bar")).toHaveAttribute("value", "0");
    await expect(page.locator(".course-progress__header")).toContainText(`0 / ${TOTAL_WEEKS} седмици`);

    // ---- Step 3: complete all 15 weeks through the real WeekCompletionControl, with
    // persistence checks (reload) at weeks 1, 5, 10, 15 ----
    const milestones = new Set([1, 5, 10, 15]);
    for (let week = 1; week <= TOTAL_WEEKS; week++) {
      await completeWeekThroughRealUi(page, week);

      if (milestones.has(week)) {
        await page.reload();
        await expect(page.locator(".week-completion__button")).toHaveText(/Седмицата е завършена/);
        await assertCourseProgress(page, week);
      }
    }
    await assertCourseProgress(page, TOTAL_WEEKS);

    // ---- Step 4-5: final exam page, certificate locked pre-submission ----
    await page.goto("/kurs/finalen-izpit");
    await expect(page.locator("#udostoverenie-section")).toContainText(
      `Удостоверението се отключва след завършени 15/15 седмици`
    );
    await expect(page.locator("#udostoverenie-section")).toContainText("Крайният изпит все още не е предаден.");
    await expect(page.getByRole("button", { name: "Създай удостоверение" })).toHaveCount(0);

    // ---- Step 6: first attempt scores below 75 (all 20 wrong -> 0) ----
    await answerFinalExam(page, /* correctCount */ 0);
    await page.getByRole("button", { name: "Предай теста" }).click();
    await expect(page.locator(".final-assessment__score")).toHaveText("Резултат: 0 / 100");
    await expect(page.locator("#udostoverenie-section")).toContainText("Текущ резултат от крайния изпит: 0/100");
    await expect(page.locator("#udostoverenie-section")).toContainText(
      `Завършени седмици: ${TOTAL_WEEKS}/${TOTAL_WEEKS}`
    );
    await expect(page.getByRole("button", { name: "Създай удостоверение" })).toHaveCount(0);

    // ---- Step 7: real Retry control clears answers/result ----
    await page.getByRole("button", { name: "Опитай отново" }).click();
    await expect(page.locator(".final-assessment__result")).toHaveCount(0);
    for (let i = 0; i < FINAL_EXAM_ANSWER_KEY.length; i++) {
      for (let opt = 0; opt < 4; opt++) {
        await expect(page.locator(`input[name="izpit-q${i}"]`).nth(opt)).not.toBeChecked();
      }
    }
    await expect(page.getByRole("button", { name: "Предай теста" })).toBeDisabled();

    // ---- Step 8: second attempt, exactly 15/20 -> 75/100 ----
    await answerFinalExam(page, /* correctCount */ 15);
    await page.getByRole("button", { name: "Предай теста" }).click();
    await expect(page.locator(".final-assessment__score")).toHaveText(`Резултат: ${MINIMUM_EXAM_SCORE} / 100`);
    await expect(page.locator("#udostoverenie-section")).toContainText("Курсът е успешно завършен");
    await expect(page.getByRole("button", { name: "Създай удостоверение" })).toBeVisible();

    // ---- Step 9: certificate creation ----
    await page.getByRole("button", { name: "Създай удостоверение" }).click();
    const nameInput = page.getByPlaceholder("Въведете вашето име");
    await expect(nameInput).toBeVisible();
    await expect(page.locator(".course-certificate-preview")).toBeVisible(); // preview stays visible alongside the real one
    await nameInput.fill("Тестов Обучаем");

    const issuedCertificate = page.locator("#udostoverenie-section .course-certificate").first();
    await expect(issuedCertificate.locator(".course-certificate__name")).toHaveText("Тестов Обучаем");
    await expect(issuedCertificate.locator(".course-certificate__issuer")).toHaveText("CBT Learning Platform");
    await expect(issuedCertificate.locator(".course-certificate__course")).toContainText(
      "15-седмичен курс по когнитивно-поведенческа терапия"
    );
    await expect(issuedCertificate.locator(".course-certificate__statement")).toHaveText(
      "Курсът е успешно завършен."
    );
    await expect(issuedCertificate.locator(".course-certificate__date")).toContainText("Дата на издаване:");
    await expect(issuedCertificate.locator(".course-certificate__disclaimer")).toContainText(
      "Не представлява"
    );
    await expect(issuedCertificate.locator(".course-certificate__disclaimer")).toContainText(
      "право за упражняване на"
    );

    // Fields the certificate must NOT show: exam score, a separate start/completion date, a
    // certificate/reference ID. (The disclaimer legitimately contains the words "лиценз"/"акредитация"
    // — it exists specifically to DENY them; that is the mandatory disclaimer itself, not a defect.)
    const certificateText = (await issuedCertificate.innerText()).toLowerCase();
    expect(certificateText).not.toContain("75");
    expect(certificateText).not.toContain("резултат");
    expect(certificateText).not.toContain("№");
    expect(certificateText).not.toMatch(/id[:\s]/);
    // Exactly one date field (issue date) — no separate start-date/completion-date line.
    expect((certificateText.match(/дата/g) ?? []).length).toBe(1);

    // ---- Step 12 (run here, while the certificate/eligible state is still live — a reload at this
    // point would intentionally clear it, see step 10) : responsive smoke at 1440 / 1024 / 390 ----
    for (const width of [1440, 1024, 390]) {
      await page.setViewportSize({ width, height: 1000 });
      const overflow = await page.evaluate(
        () => document.documentElement.scrollWidth > document.documentElement.clientWidth + 1
      );
      expect(overflow, `horizontal overflow at ${width}px`).toBe(false);

      const columns = await page
        .locator(".learning-grid--certificate")
        .evaluate((el) => getComputedStyle(el).gridTemplateColumns.trim().split(/\s+/).length);
      if (width === 390) {
        expect(columns, "390px must stack to one column").toBe(1);
      } else {
        expect(columns, `${width}px must stay side-by-side`).toBe(2);
      }
    }
    await page.setViewportSize({ width: 1280, height: 1000 });
    expect(consoleErrors, `console/page errors: ${consoleErrors.join(" | ")}`).toEqual([]);

    // ---- Step 13: print — emulate print media, then generate a real PDF with the real print CSS ----
    await page.emulateMedia({ media: "print" });
    await expect(page.locator(".course-certificate").first()).toBeVisible();
    await expect(page.locator(".course-certificate-preview")).toBeHidden();
    await expect(page.locator(".app-sidebar")).toBeHidden();
    await expect(page.locator(".app-topbar")).toBeHidden();
    await expect(page.locator(".site-footer")).toBeHidden();
    await expect(page.locator("nav.section-nav")).toBeHidden();
    await expect(page.locator(".course-certificate__print-action")).toBeHidden();
    await expect(issuedCertificate.locator(".course-certificate__disclaimer")).toBeVisible();
    await page.screenshot({ path: "test-results/print-preview.png", fullPage: true });

    const pdfPath = "test-results/certificate.pdf";
    await page.pdf({ path: pdfPath, printBackground: true, format: "A4" });
    const fs = await import("fs");
    const pdfBytes = fs.readFileSync(pdfPath);
    const pageObjectCount = (pdfBytes.toString("latin1").match(/\/Type\s*\/Page[^s]/g) ?? []).length;
    expect(pageObjectCount, "printed certificate must be exactly one page").toBe(1);
    expect(pdfBytes.length).toBeGreaterThan(1000);

    await page.emulateMedia({ media: null });

    // ---- Step 10: reload resets the session-only exam/certificate state, keeps 15/15 progress ----
    await page.reload(); // reloads /kurs/finalen-izpit itself, per the required scenario
    await expect(page.locator("#udostoverenie-section")).toContainText("Крайният изпит все още не е предаден.");
    await expect(page.getByRole("button", { name: "Създай удостоверение" })).toHaveCount(0);
    await expect(page.getByPlaceholder("Въведете вашето име")).toHaveCount(0);
    await expect(page.locator(".course-certificate__name")).toHaveCount(1); // only the blank preview remains
    await expect(page.locator(".course-certificate__name")).toHaveText("________________________");
    await assertCourseProgress(page, TOTAL_WEEKS); // course progress (localStorage) survives
  });

  test("corrupted localStorage never inflates progress or unlocks the certificate", async ({ page, context }) => {
    // Corrupt the REAL storage key before the app's first script runs, in a fresh, isolated context.
    await context.addInitScript(() => {
      window.localStorage.setItem("cbt-course-progress", "{not valid json, definitely not an int array");
    });

    const pageErrors: string[] = [];
    page.on("pageerror", (err) => pageErrors.push(String(err)));

    await page.goto("/kurs");
    await expect(page.locator(".course-progress__bar")).toHaveAttribute("value", "0");
    await expect(page.locator(".course-progress__header")).toContainText(`0 / ${TOTAL_WEEKS} седмици`);
    expect(pageErrors, `uncaught page errors: ${pageErrors.join(" | ")}`).toEqual([]);

    await page.goto("/kurs/finalen-izpit");
    await expect(page.locator("#udostoverenie-section")).toContainText(`Завършени седмици: 0/${TOTAL_WEEKS}`);
    await expect(page.getByRole("button", { name: "Създай удостоверение" })).toHaveCount(0);
    expect(pageErrors, `uncaught page errors: ${pageErrors.join(" | ")}`).toEqual([]);
  });
});
