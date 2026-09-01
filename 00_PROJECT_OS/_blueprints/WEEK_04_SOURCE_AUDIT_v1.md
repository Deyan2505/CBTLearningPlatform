# Week 4 Source Audit — "Клинична оценка и когнитивна концептуализация"

No existing page (`CourseCatalog.cs`: `route: null`, metadata-only). Build-from-scratch, not a
retrofit. `CurriculumSafetyLevel.AcademicContextOnly` (already set in `CourseCatalog.cs`) —
`DeriveStatus` resolves this to `AcademicOverview` if routed, same pattern as Week 12.
`24_IMPLEMENTATION_ROADMAP.md`'s Simulator Opportunity Matrix (Седмица 4 row) had already
catalogued this territory as `ACADEMIC ONLY` / `ACADEMIC CONTEXT ONLY`, explicitly "без лична
история" (no personal-history input) — confirmed, not revised, by this audit.

## §0 — Source scope

**SRC-041 (Judith S. Beck), Chapter 4 — "Сесия по оценка" ("The Assessment Session")**, printed
pp. 46–58 (PDF pp. 68–80). Read in full this session; extraction cached at
`00_PROJECT_OS/_source_corpus/SRC-041_ch04_bg_extracted.txt` (gitignored, not committed).

This is the correct, sole primary-source chapter for Week 4's "clinical assessment" half. The
"cognitive conceptualization" half of the title does **not** point to new source material — Chapter
3 ("Когнитивна концептуализация," printed pp. 29–45) is Week 3's chapter, already read, already
implemented (`Sedmica3.razor`, `OWNER APPROVED / LOCKED`, hierarchy diagram + Sally's populated
Concept Map). Chapter 4 itself treats conceptualization only as the **output** the assessment
process feeds into — its closing section ("Разработване на първоначална когнитивна концепция")
shows how assessment data is synthesized into the model Week 3 already teaches. No second reading
of Chapter 3 was performed; Week 4's conceptualization content is limited to this bridge, per the
owner's own boundary framing in the task (§7: "Week 3 owns the general cognitive architecture").

No source gap here — Chapter 4 is self-contained and sufficient for the assessment half.

## §1 — Knowledge Unit inventory (final, after owner decisions)

**38 KUs total — 27 Included / 0 Needs Review / 4 Deferred / 7 Excluded / 0 Unaccounted.**

Owner decisions closing the 3 `Needs Review` items from the prior audit turn (§5 below, now
resolved): RISK1 → **Included**, as a single high-level academic safety sentence only (no
screening questions, no scales, no decision rules, no procedure — framed as trained-professional
responsibility). TECH3 → **Excluded** outright — its only safe value is already covered, more
safely, by CONCEPT1. DIAG1 → **Included**, as a single general sentence — diagnostic frameworks
such as DSM may be considered by qualified professionals, no criteria/checklist/self-diagnosis.

| ID | KU | Locator | Status | Why |
|----|----|----|--------|-----|
| P1 | Assessment's central purpose: enables case formulation + an initial cognitive conceptualization tied to the patient's diagnosis | p.46 | **Included** | The chapter's own framing sentence; direct bridge to Week 3 |
| P2 | Assessment determines the diagnosis | p.46 | **Included** | Stated as a goal only, no diagnostic criteria reproduced |
| P3 | Assessment determines therapist fit, appropriate treatment "dose," and need for additional treatments/services | p.47 | **Included** | Shows what professional judgment weighs — informational, not a checklist for self-use |
| P4 | Assessment begins the therapeutic alliance and socializes the patient into the therapy process | p.47 | **Included** | Forward cross-link to Week 5 (alliance) |
| P5 | Assessment identifies important problems and sets broad goals | p.47 | **Included** | |
| P6 | Assessment continues throughout treatment, not limited to the first meeting | p.46 | **Included** | Corrects a natural misconception (assessment = one-time event) |
| PREP1 | Gathering prior records/self-report forms before the first meeting | p.47 | **Included** | |
| PREP2 | Importance of a recent medical checkup — ruling out organic causes (e.g. hypothyroidism mimicking depression) | p.47/49 | **Included** | Concrete, safe, illustrates differential clinical reasoning without diagnosing anyone |
| PREP3 | A family member may attend, by mutual, consent-based decision; patient controls what is shared | p.47/53 | **Included** | Models a collaborative, consent-based ethic |
| STRUCT1 | Six-step session structure: greet → decide re: family presence → set agenda → conduct assessment → set broad goals → get feedback | p.48 | **Included** | Descriptive overview, not an instruction for the learner to run |
| STRUCT2 | Patient is explicitly told this session is assessment, not yet treatment | p.48 | **Included** | Paraphrased concept only — see STRUCT3 |
| STRUCT3 | The verbatim opening script (therapist/Sally dialogue) | p.48 | Excluded | Reproduced patient dialogue — same boundary Week 12 already established: "no reproduced dialogue (Sally or Annie)" |
| AREA1 | Broad domains surveyed: demographics, presenting problem, history of present illness, coping strategies, psychiatric/substance/medical/family/developmental/social/educational/occupational/religious history, strengths and values | pp.49–50 | **Included** | Category list only (parallel to Week 9's distortion-category pattern), not a self-assessment form |
| AREA2 | Note that specific assessment instruments/procedures are outside this book's own scope, pointing to further specialized literature | p.50 | Deferred | Out of our platform's scope too — nothing to teach from an unreviewed citation list |
| RISK1 | Suicide risk assessment is a mandatory, explicit part of assessment (cites Wenzel, Brown & Beck, 2008) | p.50 | **Included** | Owner-approved: single high-level academic sentence only — no screening questions, no scales, no decision rules, no procedure; framed as trained-professional responsibility |
| DAY1 | "Typical day" interview as a general technique — reveals mood variation, functioning, isolation, coping | p.50 | **Included** | Concept only, no transcript |
| DAY2 | The verbatim Sally daily-routine dialogue and the clinician's resulting inference list (sleep, isolation, concentration, etc.) | pp.50–52 | Excluded | Reproduced dialogue — same STRUCT3 boundary |
| DAY3 | Also asking about positive experiences/adaptive coping, not just problems | p.52 | **Included** | Balances DAY1, easy academic point |
| TECH1 | Structuring patient responses with explicit instructions ("answer yes/no") | p.53 | Excluded | Therapist micro-technique, not conceptually necessary at academic-overview depth |
| TECH2 | Gently redirecting a tangential patient | p.53 | Excluded | Same reason as TECH1 |
| TECH3 | Noticing patient ambivalence/automatic thoughts about treatment during assessment, gently linking to the cognitive model | pp.52–53 | Excluded | Owner-approved exclusion: its only safe value (linking assessment to the cognitive model) is already covered, more safely, by CONCEPT1 — no separate automatic-thought assessment technique added |
| END1 | Closing question ("is there anything else important...") with its specific scripted phrasing | p.53 | Excluded | Therapist script |
| FAM1 | Including a family member near the end — what is asked, sharing impressions/plan with consent | p.53 | Deferred | Procedural detail, secondary to the core teaching goals |
| DIAG1 | Communicating initial diagnostic impressions to the patient, referencing DSM as the shared diagnostic framework | p.53 | **Included** | Owner-approved: single general sentence only — diagnostic frameworks such as DSM may be considered by qualified professionals where relevant; no diagnostic criteria, no symptom checklist, no self-diagnosis |
| DIAG2 | Judgment call: naming a formal diagnosis vs. summarizing symptoms, depending on patient reaction | p.53 | Deferred | Nuanced clinical judgment, secondary |
| GOAL1 | Treatment goals framed as the "flip side" of problems | p.54 | **Included** | Clean, general, safe reframe |
| GOAL2 | Linking the treatment plan to how CBT addresses the problems (identify/evaluate/modify thoughts + behavioral steps + problem-solving) | pp.54–55 | **Included** | Ties directly into Week 3's model, stated generally |
| GOAL3 | The verbatim Sally goal-setting dialogue | pp.54–55 | Excluded | Reproduced dialogue |
| SKEP1 | Addressing patient skepticism about the plan — validate it, then gather more data about its source | pp.55–56 | **Included** | Short, general, models healthy collaboration; forward cross-link to Week 5 |
| SKEP2 | Checklist for comparing to a prior unsuccessful therapy (did the prior therapist set an agenda, teach specific skills, etc.) | pp.55–56 | Deferred | Fairly technical, secondary |
| EXP1 | Typical treatment length: ~2–4 months for many depression cases; longer for chronic/personality-related work; more intensive + booster sessions for severe mental illness | p.56 | **Included** | Concrete, sourced, factual expectation-setting — no personal-data risk |
| EXP2 | Typical session frequency/tapering (weekly → biweekly → monthly) and an estimate of 8–14 sessions | p.56 | **Included** | Same as EXP1 |
| POST1 | Between assessment and first session: therapist writes an assessment report and initial treatment plan | p.57 | **Included** | Brief professional-workflow fact |
| POST2 | With consent, therapist contacts previous providers to coordinate care | p.57 | **Included** | Brief, reinforces the consent/collaboration theme |
| CONCEPT1 | **The bridge:** assessment data is synthesized into the initial cognitive conceptualization (core beliefs + behavioral patterns tied to the diagnosis) | p.57 | **Included** | The chapter's own explicit link back to Week 3's model — highest-priority KU in this audit |
| CONCEPT2 | The 5 hypothesis-generating questions a clinician asks when building an initial conceptualization (early-life origin of core beliefs; what the core beliefs are; what triggered the disorder; adverse interpretation of triggering events; how thinking/behavior maintain the disorder) | p.57 | **Included** | Generalizable professional reasoning questions about a hypothetical case — safe, not self-directed |
| CONCEPT3 | Sally's case-synthesis narrative (paraphrased): hypothesized incompetence belief, origin in family/school interactions → activated at college → generalized incompetence → automatic thoughts → mood/behavioral effects → misattribution to innate deficiency rather than depression → resulting treatment-plan hypothesis | pp.57–58 | **Included** | Beck's own narrated synthesis, not a dialogue transcript — paraphrase-not-quote, same precedent Week 1 already used; extends Week 3's Sally case with the assessment→conceptualization angle, doesn't duplicate Week 3's diagram |
| CONCEPT4 | The treatment plan is refined throughout therapy as the clinician learns more | p.58 | **Included** | Brief, honest closing point — conceptualization is provisional, not a one-time verdict |

## §2 — Terminology Map

- "Сесия по оценка" (assessment session) — kept distinct from "терапевтична сесия" (therapy
  session); Week 4's own central distinction (STRUCT2).
- "Начална/първоначална когнитивна концептуализация" (initial cognitive conceptualization) — the
  same term Week 3 already uses for its model; Week 4 does not rename or duplicate it, only shows
  how it gets built.
- "Основни вярвания" (core beliefs) — Week 3/12's existing term, reused verbatim, not
  reintroduced as new vocabulary.
- No new clinical-technique vocabulary introduced (no "downward arrow," no thought-record terms —
  those stay Week 9/11/12's territory).

## §3 — Representation Fit (evaluated, not assumed)

- **Domain-survey list (AREA1):** a plain categorized list/grid, same visual weight as Week 9's
  distortion-category list — no new component.
- **The 5 conceptualization questions (CONCEPT2):** a simple ordered list, optionally in an
  existing callout-style card — no new component.
- **Sally's case synthesis (CONCEPT3):** prose paragraph(s), same pattern as Week 1's paraphrased
  dream-study anecdote — no dialogue markup, no new component. Optionally closes with a link back
  to Week 3's Concept Map ("виж пълния модел в Седмица 3") rather than re-rendering any diagram.
- **Weekly Mind Map / interactive stepper / worked-example walkthrough:** **not recommended**,
  same reasoning as Week 12's audit — this is exactly the "here is how to do this to yourself"
  risk `AcademicContextOnly` + no self-guided variant exists to prevent. No step-through UI, no
  reproduced dialogue, no case-history input field of any kind.
- **Retrieval practice:** standard non-scored comprehension check, same pattern as every other
  week.

## §4 — Boundary check against neighboring weeks

- **Week 3** (`OWNER APPROVED / LOCKED`) owns the cognitive-model architecture and Sally's
  conceptualization diagram — Week 4 references it (CONCEPT1) and extends it narratively (CONCEPT3)
  but does not re-teach or re-render it.
- **Week 5** ("Принципи на КПТ и терапевтичен съюз," not started) owns the therapeutic-alliance
  depth — Week 4 only cross-links (P4, SKEP1), doesn't develop alliance theory itself.
- **Week 6** ("Структура на терапевтичната сесия," `LOCKED`) covers session structure for sessions
  *after* the assessment session (SRC-041 Ch. 5/7 territory) — no overlap; Week 4's STRUCT1/STRUCT2
  are specific to the assessment session only, a chapter Week 6 doesn't touch.
- **Week 8/9/11** (automatic thoughts, distortions, intermediate beliefs) — TECH3's exclusion
  candidacy exists specifically to avoid pulling their depth in early; Week 4 stays at "this
  happens during assessment too," not "here's how to do it."

## §5 — Owner decisions (resolved, this turn)

1. **RISK1:** Included — single high-level academic safety sentence only (no screening questions,
   no scales, no decision rules, no procedure), framed explicitly as trained-professional
   responsibility.
2. **TECH3:** Excluded — no separate automatic-thought assessment technique added; the safer
   conceptualization bridge (CONCEPT1) covers the value this would have added.
3. **DIAG1:** Included — one general academic DSM reference (diagnostic frameworks such as DSM may
   be considered by qualified professionals where relevant); no diagnostic criteria, no symptom
   checklist, no self-diagnosis.
4. **Route:** Week 4 gets a real route, `/kurs/sedmica-4`, matching Week 12's precedent.
   `CurriculumSafetyLevel.AcademicContextOnly` unchanged — resolves to `AcademicOverview`, not
   `Available`.
5. **CONCEPT3:** approved — paraphrase only facts directly supported by SRC-041 Ch.4, no new
   history added, used to demonstrate assessment → conceptualization, links back to Week 3 rather
   than re-teaching/re-rendering it.

## §6 — Implemented structure

01 Накратко → 02 Защо е нужна оценка (P1–P6) → 03 Подготовка преди първата среща (PREP1–3) →
04 Структура на сесията по оценка (STRUCT1–2) → 05 Области на оценката (AREA1 + RISK1, as a
`DisclaimerCallout Variant="safety"`) → 06 Ежедневието на пациента (DAY1/DAY3) → 07 Цели и план за
лечение (GOAL1–2, SKEP1, EXP1–2, DIAG1) → 08 От оценка към когнитивна концептуализация (POST1–2,
CONCEPT1–4 — the chapter's bridge to Week 3, the page's centerpiece) → 09 Проверка → 10 Извори.

Implemented in `Sedmica4.razor` (`/kurs/sedmica-4`), integrated into `CourseCatalog.cs`/`Kurs.razor`
with `WeekCompletionControl`. Zero new components, zero new CSS — reuses `DisclaimerCallout`'s
pre-existing, previously-unused `Variant="safety"` for RISK1.

**Implementation complete this turn. Not committed. Not OWNER APPROVED / LOCKED — awaiting owner
visual/learning review.**
