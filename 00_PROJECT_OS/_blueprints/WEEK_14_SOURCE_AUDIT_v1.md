# Week 14 — Deep Source + Coverage Audit v1

**Status:** `IMPLEMENTED, TECHNICALLY READY — NOT YET OWNER APPROVED / LOCKED`.
**Date:** 2026-09-07 (audit) / 2026-09-07 (owner decisions + implementation, same session).
**Scope:** SRC-041 Гл. 17 (printed 294–315) + Гл. 18 (printed 316–331).

## §12 — Owner decisions (turn 3) and final resolution

Owner approved all nine §9 recommendations as written. The two `Needs Review` KUs are resolved:

- **C18-K50 (Figure 18.3 — self-therapy session guide):** **Included, DESCRIPTIVE ONLY.** The page
  explains what such a plan reviews (the past week, homework, current problems, predicted future
  problems, new homework, the next self-therapy date) as academic description of a clinician-taught
  concept — never reproduced as a usable form, checklist, or first-person guide. Paraphrased, not
  transcribed from the copyrighted worksheet.
- **C18-K65 (Figure 18.5 — booster session guide):** **Included, DESCRIPTIVE ONLY.** Same treatment —
  explains what a booster session covers (what went well, what problems arose and how they were
  handled, anticipated future problems, CBT work done/planned, additional goals) academically, never
  as a fillable preparation checklist.

**Final KU accounting: 176 = 126 Included / 31 Deferred / 19 Excluded / 0 Needs Review / 0
Unaccounted.** (§2's 124/31/19/2/0 + the two resolved KUs → 126 Included.)

Other approved decisions: catalog objectives rewritten (§9 #3); title `прекратяване` locked (§9 #4);
format → `InteractiveModel` (§9 #5); route `/kurs/sedmica-14` assigned (§9 #6); `поддържаща сесия`
locked (§9 #7); `списък на заслугите` keeps Week 13's lock (§9 #8); GAP-014 homework half closed
against Week 14 (§9 #9, §10) — see `15_GAPS_AND_CONFLICTS.md`. C15-K05 (therapist development)
explicitly **not** assigned here — remains Deferred/Unassigned pending the Week 15 audit.

Implementation follows in `Sedmica14.razor` per §6/§7/§8 of this audit. See that file's own header
comment for the section-by-section resolution log.

## §13 — Owner production-review fix (same session, after §12)

Browser QA at 390px found the §04 "Диагностика на неизпълнение" table (3 columns: Тип / Как се
разпознава / Отговор на терапевта) still overflowing its wrapper — 468px measured table width in a
289px container — despite the scoped `white-space: normal` fix already applied in `app.css`. Owner
declined to accept this as a mobile tolerance and required a real fix.

**Root cause:** not the shared `nowrap` (already fixed) — CSS auto table-layout floors each column at
its own min-content width (the widest unbreakable Cyrillic word in that column), and the sum across
three prose columns exceeds 289px even with wrapping enabled. `table-layout: fixed` was considered and
rejected — this codebase already tried and reverted it for Week 11 §07 (a 2-column table) because
narrow fixed columns break long Bulgarian words mid-word; three columns leaves even less room per
column than two.

**Fix:** below 480px only, the diagnosis table (same markup, same content, only `data-label`
attributes added per cell) reflows into stacked label/value cards via scoped CSS, reusing the
project's existing `.visually-hidden` clip technique to keep the header row accessible while hidden
visually. Desktop/tablet keep the real, untouched table. Verified 0 overflow at 1440px/1024px/390px
after the fix (was 468×289 at 390px before). No other Week 14 table touched; no other week touched;
KU accounting/content/wording unchanged.

---

## 0. Canonical Week 14 (read from the live project, not from memory)

Source of truth: `CbtLearningPlatform.Client/Curriculum/CourseCatalog.cs:202-211`.

| Field | Live value |
|---|---|
| Number | 14 |
| Module | `Разширени техники и академичен контекст` |
| Title | `Домашна работа, приключване и превенция на рецидив` |
| Short summary | „Как наученото се поддържа след края на структурирана работа." |
| Safety level | `CurriculumSafetyLevel.ProfessionalReviewRequired` |
| Derived public status | `Изисква професионален преглед` (`CurriculumEnums.cs:70-71`) |
| Route | `null` — **not routed** |
| Declared format | `InteractiveFormat.StaticVisualization` |
| Objectives | 2 — „Разбирате защо поддържането на наученото е отделна стъпка."; „Запознавате се с идеята за план за трудни периоди." |

Roadmap status before this audit: `24_IMPLEMENTATION_ROADMAP.md` §F — **`NOT READY`**, „нищо конкретно потвърдено… не се измисля глава номер". **This audit closes that gap.**

### 0.1 Catalog mismatches found (flagged, not silently fixed)

1. **Objectives cover only half the week.** Both objectives describe Гл. 18 (maintenance, plan for
   difficult periods). Гл. 17 — *домашна работа*, the **first word of the week's own title** and the
   larger of the two chapters (102 of 176 KUs) — has **no objective at all**. The catalog wording is
   syllabus-derived and predates any reading of the source.
2. **Terminology.** The catalog says `приключване`; SRC-041's own chapter title and every in-chapter
   running header say **`прекратяване`**. Recommend locking the source term learner-facing.
3. **Declared format.** `StaticVisualization` was assigned when the week was `NOT READY` and
   unsourced. If routed, the platform standard requires a Weekly Mind Map (Preview + Review), which
   `StaticVisualization` does not describe. Same class of contradiction as Week 13's
   `Simulator`-vs-safety-tier defect. Owner decision — see §9.
4. **Route is `null`.** Routing is required for completion eligibility
   (`CourseWeekDefinition.Route is not null`).

**Safety level is NOT in question and is preserved exactly:** `ProfessionalReviewRequired`.

---

## 1. Source scope — verified, not assumed

Extraction method and page offset per `_source_corpus/README_EXTRACTION_METHOD.md`
(`PDF page = printed page + 22`). Table of contents (PDF pp. 17–21) verified directly.

| Chapter | Title (BG, as printed) | Printed pp. | PDF pp. | Extract | Chars |
|---|---|---|---|---|---|
| 17 | Домашна работа | 294–315 | 316–337 | `SRC-041_ch17_bg_extracted.txt` | 83,371 |
| 18 | Прекратяване и превенция на рецидив | 316–331 | 338–353 | `SRC-041_ch18_bg_extracted.txt` | 31,527 |

Boundaries verified: Гл. 17 opens „Глава 17 / Домашна работа" on printed 294 and closes with its own
summary + the Kazantzis/Tompkins resource note on printed 315. Гл. 18 opens „Глава 18 / ПРЕКРАТЯВАНЕ И
ПРЕВЕНЦИЯ НА РЕЦИДИВ" on printed 316 and closes with its own summary on printed 331. Гл. 19
(„Планиране на лечението", printed 332) is **outside scope**.

Both chapters read in full for this audit. OCR quality good; the usual artifacts (`паТиент`,
mixed-case running headers, one garbled header on printed 330 reading „КОНТРОЛ НА ПОВЕДЕНИЕ") do not
affect content. Figures 17.1, 18.1–18.5 extract as flattened text — same positional caveat as every
prior week's figures.

**Chapter 16 („Образи", printed 277–293) is deliberately NOT in scope.** It sits between the two
chapters and has no curriculum owner. Imagery is trauma-sensitive; it is not annexed here.

---

## 2. Knowledge Unit inventory — 176 KUs, 100% accounted

**Total 176 = 124 Included / 31 Deferred / 19 Excluded / 2 Needs Review / 0 Unaccounted.**

| Chapter | KUs | Included | Deferred | Excluded | Needs Review |
|---|---|---|---|---|---|
| Гл. 17 | 102 | 72 | 23 | 7 | 0 |
| Гл. 18 | 74 | 52 | 8 | 12 | 2 |
| **Total** | **176** | **124** | **31** | **19** | **2** |

### 2.1 Chapter 17 — Домашна работа (C17-K01 … C17-K102)

**Included — the assignment spine (K01–K17).** Homework is integral, not optional (Бек и др., 1979)
· research support (Казанцис/Уитингтън/Датилио 2010; Неймайер и Фейкас 1990; Пърсънс/Бърнс/Перлоф
1988) · purpose = extend cognitive and behavioral change through the patient's week · preparation
begins in the first session · the name is negotiated with the patient, and it is explicitly *not*
school homework — designed together, only for this person · six opportunities good homework creates
(further self-education, gathering data, testing thoughts and beliefs, modifying thinking,
practising tools, experimenting with new behaviour) · it maximizes what was learned in session and
raises self-efficacy · even expert therapists find most patients rarely do *written* assignments ·
default assumption is that any patient (bar very low functioning) will comply if it is assigned
correctly · four compliance levers (tailor, rationale, uncover obstacles, modify beliefs) · the
chapter's own four-part structure · no formula — tailoring is the method · what to suggest follows
from the session, the treatment plan and the patient's goals · individual characteristics to weigh
(literacy, motivation, current stress and functioning, practical limits) · therapist leads early,
then hands the design over · **patients who routinely set their own homework by the end of therapy
are more likely to keep doing it after treatment ends.**

**Deferred — the eight typical assignment types (K18–K25).** Behavioural activation → **Week 7**;
monitoring automatic thoughts → **Week 8**; evaluating and responding to them → **Weeks 9/10**;
problem solving → no single owner; behavioural skills (relaxation, assertiveness, organisation) →
**Week 13**; behavioural experiments → **Weeks 7/9**; bibliotherapy → no owner; preparation for the
next session / Therapy Preparation Worksheet (printed p. 102) → **Гл. 7 territory, unassigned**.
Week 14 owns the *catalog and its logic* — that homework has recurring types and which type fits
when — not a second teaching of each technique.

**Deferred — Sally's session-by-session homework (K26–K29).** Sessions 1–12. Week 6 already
reproduces Sally's Session-1 homework list (Figure 5.1, printed p. 67) as a `SourceArtifact`.
Re-running the same case week by week would duplicate Week 6 and add no new mechanism. The one
forward-pointing item — Session 12's „Прегледайте бележките за провеждане на сесия за само-терапия" —
is carried by Гл. 18's own material instead.

**Included — adherence (K30–K50, minus deferrals).** The eight guidelines · prefer tasks that are
too easy over too hard · personalization through three contrasted, *anonymized* patients (one who
could not yet identify thoughts at all; one who arrived already understanding them; and the fact
that *amount* differs as much as type) · breaking assignments into manageable steps · anticipating
difficulty from diagnosis and personality style (severely depressed → behavioural before cognitive;
avoidant patients withdraw from assignments they read as too challenging; anxious and overwhelmed
patients freeze if given too many) · failure to complete breeds self-criticism and hopelessness ·
rationale first, later elicited from the patient · setting homework collaboratively · **the no-lose
frame and its own stated limit** — useful data even when it isn't done, *but* if a patient misses
significant homework two weeks running, find the obstacle and emphasize importance instead of
continuing to make it risk-free · start it in session, because continuation is far easier than
initiation and the hardest moment is the one just before beginning · write it down every week from
session one; remembering strategies (pair with a daily activity, notes on the fridge or mirror,
phone or calendar, another person) and the transfer question „how do you remember your medication?"
· the five anticipation questions · **the single most important question: „Колко вероятно е да го
направите, 0–100%?"** · below 90–100% confidence, three named fallbacks.

**Deferred/Excluded — the three fallbacks' procedures.** *Covert rehearsal* (K51–K53) uses induced
imagery — **Deferred**, imagery belongs to unassigned Гл. 16; the Sally office-hours dialogue (K52)
is **Excluded** as reproduced dialogue. *Intellectual–emotional role play* (K55–K57) is
**Deferred**: Week 13 already excluded role-play mechanics (C15-K82) and the same boundary holds;
its transcript (K56) is **Excluded**. *Changing the task* (K54) is **Included** — it is a decision
rule, not a procedure. Therapist self-disclosure to normalize procrastination (K45) is
**Deferred** (Week 13 precedent C15-K16).

**Included — preparing for a negative outcome (K58, K59, K61).** Design a scenario likely to
succeed, but still have the patient predict what they would think if it failed; advance discussion
protects against dysphoria when an experiment goes badly. The Sally/Ross transcript (K60) is
**Excluded** as reproduced dialogue.

**Included — conceptualizing difficulty (K62–K89), the chapter's strongest unique asset.** The
four-way diagnostic: *practical* · *psychological* · *psychological masked as practical* ·
*therapist's own cognitions*. Practical problems and their fixes: last-minute homework (and the
avoidance beliefs behind it — „Ако се съсредоточа върху проблема, вместо да се разсейвам, ще се
чувствам само по-зле"; „Не мога да се променя, така че защо изобщо да опитвам?") · forgetting the
rationale (write it beside the task) · disorganisation (Figure 17.1's daily checklist; a calendar;
or a message left at the office) · a task that was too hard or unclear — where **the therapist takes
responsibility**, which simultaneously models fallibility, builds rapport, shows willingness to
adapt, and hands the patient an alternative explanation for their own failure. Psychological
problems: negative predictions elicited by recalling one concrete moment; running the experiment in
session; **if the experiment fails, make the task more basic** · exaggerating a task's demands, in
time (the time-limited-discomfort analogy) and in energy (a distorted *image* of the task — the
patient who pictures trudging shop to shop when the assignment was ten minutes; the parent for whom
the problem was leaving the house, not the park) · framing the assignment as an experiment that
records the prediction · perfectionism, met with „this is a skill, like learning to type" ·
the masked-practical test: „нека да се преструваме, че този проблем магически изчезва — сега колко
вероятно е?". The therapist's-own-cognitions category (K89) is **Included as a named category
only**; its seven specimen assumptions (K90) and the supervision remedies (K91) are **Excluded** as
therapist-only material, as is the deliberately-imperfect Thought Record prescription (K87) and the
brochure transcript (K76).

**Included — reviewing homework (K92, K96–K102).** Prepare before the session · **how much time to
spend is „част от изкуството на терапията"** · spend more when it covers a live problem, when the
patient did not complete it, or when there is something to learn · too little review starves
reinforcement, too much starves new problems · early on, reinforce the therapy notes by asking how
much the patient now believes what they wrote · jointly decide what continues and what changes ·
the chapter's own summary and its further-reading pointers. Agenda placement and crisis exceptions
(K93–K95) are **Deferred → Week 6**, which owns session structure and already renders „Прегледай
домашната работа" as a step in its session sequence.

### 2.2 Chapter 18 — Прекратяване и превенция на рецидив (C18-K01 … C18-K74)

**Included — the frame (K01–K09, K12–K17, K19, K21).** The goal of CBT is to facilitate remission
and teach skills for life — **explicitly not to solve all the patient's problems**; a therapist who
takes responsibility for every problem risks building dependency and denies the patient the chance
to test their own skills. Weekly sessions by default; more frequent for severe symptoms; then a
**jointly agreed, trial-basis taper**: weekly → every 2 weeks → every 3–4 weeks, with booster
sessions at roughly 3, 6 and 12 months. Preparation begins in the *initial* session, by saying the
aim is to make treatment as time-limited as possible so that patients **become their own
therapists**. Once they feel better, hold an explicit discussion of the *shape* of recovery.
Attributing progress to the patient: ask why they think they feel better and reinforce that they
produced it; when they credit the therapist, circumstances or medication, acknowledge the external
factor and still ask what *they* changed — the alternative attribution is what builds self-efficacy.

**Included — Figure 18.1 as a fixed illustration (K10, K13).** Improvement interrupted by plateaus,
fluctuations and setbacks; setbacks become fewer, shorter and less severe over time; and the reason
the picture matters — without it, a setback reads as „терапията не работи, никога няма да се
подобря". **K11 Excluded:** the caption's joke (the curve resembling the southern border of the
United States, setbacks as „Texas" and „Florida") is untranslatable local humour with no learner
value in Bulgarian.

**Included — tools as lifelong aids (K23, K24, K25, K27).** The tools are not disorder-specific;
they apply whenever a person notices they are reacting more strongly than the situation warrants.
**Figure-free ten-item consolidation list** — break problems into components · generate solutions ·
identify, test and respond to automatic thoughts and beliefs · Thought Records · monitoring and
scheduling activities · relaxation exercises · distraction and refocusing · hierarchies of avoided
tasks or situations · credit lists · advantages and disadvantages. This list is Week 14's single
best *integrative* asset: it is the only place in the source where the whole toolkit is named at
once, as a maintenance inventory. Two constraints ride on it — see §5. And the source's own explicit
limit is Included with it: **„не казвам, че трябва да се опитвате да се отървете от всички негативни
емоции — само когато мислите, че може да реагирате прекалено."**

**Included — setbacks during therapy (K29, K30, K32).** As soon as patients improve, ask what would
go through their mind if they got worse; the nine specimen thoughts the source lists; coping cards
and Figure 18.1 as the counters. The imagined future self-image (K31) is **Deferred** (imagery,
Гл. 16). The full rehearsal transcript (K33) is **Excluded**.

**Included — tapering and termination (K34–K38, K41–K46).** Taper framed and offered **as an
experiment** · when patients see no advantage, elicit the disadvantages first, use guided discovery
for the advantages, then reframe the disadvantages · **Figure 18.2 as a fixed worked example**
(Sally's four advantages, and her three disadvantages each with its reframe — including „ако ще
рецидивирам, по-добре е да се случи докато все още съм в терапия") · at each spaced session, jointly
decide whether to keep spacing or go back · elicit automatic thoughts about ending: some patients
are hopeful, some frightened or angry, most mixed — pleased with progress, worried about relapse,
sorry the relationship is ending · acknowledge the feeling **and** address the distortion · the
therapist may honestly express their own regret, pride and confidence · review and organise the
notes, with a written summary of what was learned as the natural assignment. Transcripts K39/K40 are
**Excluded** as reproduced dialogue; K22 (eliciting the core belief behind refusing credit) is
**Deferred → Week 12**.

**Included — self-therapy as a concept (K47, K48, K49, K53, K54).** Many patients never hold formal
self-therapy sessions, yet discussing a plan is still worthwhile; trying it *while sessions are
still tapering* both makes post-termination use far likelier and surfaces the obstacles early (no
time, not knowing what to do, and the interfering thoughts „Това е твърде много работа"; „Наистина
не ми трябва да го правя"; „Не мога да го направя сам"). Its stated advantages: therapy continues at
a convenient time and free of charge, tools stay fresh, difficulties get solved before they grow,
relapse becomes less likely, and the skills transfer into ordinary life. Post-termination setbacks
are planned for in advance, and patients are encouraged to try solving a difficulty themselves
before calling — with the follow-up framed as *what got in the way*, not as failure.
**The two worksheets themselves are the open decisions — see §5.**

**Included — booster sessions (K56–K64, K67, K71, K73).** All eight source reasons: reviewing how
difficulties were handled, anticipating the next months and planning for them, the motivating effect
of knowing you will be asked, detecting **reactivated** dysfunctional beliefs, watching for the
return of dysfunctional strategies such as avoidance, surfacing new or unmet goals, re-evaluating
the self-therapy programme, and the plain fact that a scheduled booster relieves anxiety about
coping alone. Plus the booster's own shape — check well-being, plan continued maintenance — and its
closing observation: **this patient had already learned the tools; he needed the session to remind
and motivate him to use them.** The transcript (K68–K70) is **Excluded**, including its **BDI
(„Инвентар за депресия на Бек") score reference** — clinical instrument, same boundary Weeks 4 and 6
already hold. Conditional clinical escalation (K72) and belief restructuring in session (K60) are
**Deferred** (clinician judgment / Week 12). Avoidance as a dysfunctional strategy (K61) is
**Deferred → Week 13**.

**Included — the closing claim (K74):** relapse prevention is not an end-of-therapy module; it runs
throughout, and problems in tapering and termination are handled like any other problem — problem
solving plus responding to dysfunctional thoughts and beliefs.

---

## 3. Terminology Map (locks proposed)

| Concept | Forms found in Гл. 17/18 | Proposed learner-facing lock |
|---|---|---|
| Homework | „домашна работа", „домашно задание", „задача", „план за действие" | **„домашна работа"** (task-level: „домашно задание") |
| Termination | „прекратяване" (chapter title + all headers), „приключване" (catalog only) | **„прекратяване"** — source term; catalog wording corrected |
| Tapering | „намаляване на сесиите", „разпределяне на сесиите", „разстояние между сесиите" | **„намаляване на честотата на сесиите"** |
| Setback | „неуспех", „спад", „рецидив" (distinct: relapse ≠ setback) | **„спад"** for setback, **„рецидив"** reserved for relapse |
| Booster session | „бустер сесия", „сесия за усилване", „сесия за подсилване", „сесия за повишаване на мотивацията" | **„поддържаща сесия"** (4 competing OCR forms — lock required) |
| Self-therapy session | „сесия за самотерапия", „сесия за само-терапия" | **„сесия за самотерапия"** |
| Covert rehearsal | „тайна репетиция", „скрито репетиране" | n/a — Deferred, not taught |
| Credit list | „списък с кредити" (Гл. 17/18) vs Week 13's locked „списък на заслугите" | **„списък на заслугите"** — Week 13's lock wins; Гл. 17/18's „кредити" is a literal OCR calque |
| No-lose homework | „безрискова", „без загуби" | **„безрискова домашна работа"** |

Two locks matter beyond style: **„поддържаща сесия"** (four competing translations inside one
chapter) and **„списък на заслугите"** (Week 13 already locked it; Гл. 17/18 would otherwise
introduce a second name for the same thing on a later page of the same course).

---

## 4. Overlap audit against every implemented week

| Week | Status | Genuine overlap | Boundary for Week 14 |
|---|---|---|---|
| **3** — cognitive architecture | LOCKED | none material | — |
| **4** — assessment/conceptualization | Implemented | Гл. 17 conceptualizing *homework difficulty* reuses the conceptualization stance | Different object: Week 4 conceptualizes the *patient*; Week 14 conceptualizes *a failed assignment*. No re-teaching. |
| **5** — principles/alliance | Implemented | Гл. 18's therapist self-disclosure at termination touches the alliance | Cross-link only; Week 5 owns alliance. |
| **6** — session structure | LOCKED | **Substantial.** Week 6 already teaches: homework as a session slot („Прегледай домашната работа"), the written dated homework list, Sally's Figure 5.1 list, optional framing to prevent overwhelm, saying task duration aloud, where to keep the sheet | **Hard boundary.** Week 14 must not re-teach the homework list as a session artifact, the optional framing, or Sally's Session-1 list. Week 14 owns *design, adherence, failure diagnosis and review depth* — the mechanics Гл. 5 only gestures at. K93–K95 explicitly Deferred back to Week 6. |
| **7** — behavioral activation | LOCKED | Гл. 17 assignment type 1; Гл. 18 tool 5 | Named in the catalog/tool list, cross-linked, not re-taught. |
| **8** — automatic thoughts/emotions | LOCKED | Гл. 17 assignment type 2 | Same. |
| **9** — distortions/Thought Record | LOCKED | Гл. 17 type 3; Гл. 18 tool 4 | Same. Thought Record procedure stays Week 9's. |
| **10** — Socratic/collaborative | LOCKED | Гл. 17 K79 standard Socratic questioning; Гл. 18 K36 guided discovery | Cross-link only. |
| **11** — intermediate beliefs | LOCKED | Гл. 17 K81 („техниките от глави 13 и 14") | Explicit cross-link, no re-teaching. |
| **12** — core beliefs/schemas | LOCKED | Гл. 17 K80 homework-activated beliefs; Гл. 18 K22, K60 | Cross-link. Belief modification stays Week 12's. |
| **13** — additional techniques | **LOCKED** | **Largest overlap.** Гл. 18's ten-tool list names, in one breath, Week 13's refocusing/distraction, credit lists, advantages/disadvantages, relaxation, **and avoided-task hierarchies** | The list is Included **as a consolidation inventory**, each item named only, every one cross-linked to its owning week. Two live constraints — §5. |

### 4.1 Unique Week 14 ownership (what no other week can claim)

1. **Homework as a designed object** — the six opportunities, the four compliance levers, and the
   fact that there is no formula, only tailoring.
2. **The adherence architecture** — the eight guidelines, the 0–100% likelihood question, and the
   escalation ladder when confidence is below 90%.
3. **The no-lose frame *and its stated limit*** — the only place the source says when to stop making
   homework risk-free.
4. **The four-way diagnosis of homework failure** — practical / psychological / masked / therapist's
   own cognitions. Nothing else in the course diagnoses a *non-event*.
5. **Review depth as a judgment** — how much session time homework deserves, and what is lost at
   each extreme.
6. **The therapy-arc claim** — treatment is designed from session one to end, and the patient is
   built into their own therapist.
7. **The shape of recovery** — plateaus and setbacks as normal, and setbacks shrinking over time.
8. **Attribution of progress to the patient** as an explicit, repeated intervention.
9. **Tapering and termination as experiments**, with their own advantages/disadvantages work.
10. **Booster sessions** — the eight reasons and the maintenance cadence.
11. **The maintenance inventory** — the single place the whole CBT toolkit is named as a set.

---

## 5. Safety-sensitive material

Live classification `ProfessionalReviewRequired` **preserved unchanged**. Public status stays
`Изисква професионален преглед`, never `Налично`.

**Excluded outright (no owner decision needed — settled precedent):**

- **All reproduced therapist–patient dialogue** (K17-52/56/60/76; K18-18/20/33/39/40/68/69/70) —
  every prior week's boundary.
- **BDI / „Инвентар за депресия на Бек"** (K18-68) — clinical scoring instrument; Weeks 4 and 6 hold
  the same line.
- **Therapist-only material** — the seven specimen therapist assumptions and supervision remedies
  (K17-90/91).
- **Prescribed corrective exercises** — the deliberately-imperfect Thought Record for perfectionism
  (K17-87).
- **Induced-imagery procedures** — covert rehearsal (K17-51/52/53) and the imagined future self
  (K18-31). Гл. 16 („Образи") has no curriculum owner and is not annexed here.
- **Exposure hierarchies** — Гл. 18's tool 8 („създаване на йерархии на избягвани задачи или
  ситуации") is named in the inventory only. Week 13 excluded the hierarchy *procedure* under the
  strictest tier in the platform; **that exclusion is inherited verbatim**, no procedure, no planner,
  no worked hierarchy.

**Constraint inherited from Week 13:** tool 6 („упражнения за релаксация") must carry Week 13's
**K53 paradoxical-arousal caution** wherever relaxation is named, or cross-link to it. Not optional.

**Two genuine owner decisions — the only `Needs Review` KUs:**

| KU | Item | The question |
|---|---|---|
| **C18-K50** | **Figure 18.3 — Ръководство за сесии за самотерапия** (six-step guide: review the week, review homework, current problems, predict future problems, set new homework, schedule the next self-therapy session) | This is a **complete self-administered treatment protocol**. Reproducing it as an actionable tool on a `ProfessionalReviewRequired` page would hand a lay reader a structured self-therapy procedure — the exact boundary `23_CLINICAL_SAFETY_BOUNDARIES.md` §2/§6 draws. **Options:** (a) exclude entirely; (b) **describe that such a plan exists and what it reviews, as fixed academic illustration, without reproducing it as a usable form** (Week 13's Fig. 15.3 precedent); (c) reproduce in full. Recommendation: **(b)**. Note the source itself is a copyrighted Beck Institute worksheet — (c) also carries a licensing question. |
| **C18-K65** | **Figure 18.5 — Ръководство за поддържащи сесии** (patient-facing preparation questions a–e) | Same class, lower intensity — a preparation checklist for a session *with a clinician*, not a substitute for one. **Options:** (a) exclude; (b) present descriptively as what a booster session covers; (c) reproduce as a checklist. Recommendation: **(b)**. Same worksheet-provenance note. |

**Explicitly NOT built, whatever is decided above:** no self-input form, no relapse-risk predictor,
no personal maintenance-plan builder, no self-scored questionnaire, no exposure or hierarchy planner,
no first-person coping-card generator. **C18-K55 (Figure 18.4, Sally's first-person setback coping
card) is Excluded** — identical class to Week 13's excluded first-person homework script (C15-K111).

---

## 6. Representation Fit

Judged against the source, not against Week 6's layout.

| Representation | Verdict | Justification |
|---|---|---|
| **Weekly Mind Map (Preview + Review)** | **Required if routed** | Platform standard. The material is genuinely two-lobed (design/adherence/diagnosis · maintenance/termination/boosters) and clusters cleanly at ~7–8 single-parent branches. |
| **Concept Map** | **No** | The relations here are sequential and procedural, not a multi-parent network. Forcing one would be decorative. Same call Weeks 11 and 13 made. |
| **Process visualization** | **Yes — one** | The taper ladder (weekly → 2 weeks → 3–4 weeks → termination → boosters at 3/6/12 months) is a genuine ordered sequence with source-exact intervals. Reuses `.guided-practice-sequence`. |
| **Fixed illustration — Figure 18.1** | **Yes** | The recovery curve is the single most transferable idea in Гл. 18 and is *explicitly* meant to be looked at. Own design, not a facsimile; caption humour excluded. |
| **Comparison** | **Yes — two** | (a) Figure 18.2's advantages/disadvantages-with-reframe table is already a comparison in the source. (b) The four-way homework-failure diagnosis is a genuine discriminating table. Both reuse `.comparison-matrix` — **and must ship with the scoped `white-space: normal` override from the start** (root cause fixed in Weeks 1/2/7/11/13; do not re-introduce it). |
| **Fixed worked example** | **Yes — Figure 18.2 only** | Sally, already established, already tapering in the source. **No new biography** — Figure 18.2's own content and nothing beyond it. |
| **Interactive element** | **Reveal-based formative checks only** | The chapter's decision points (is this failure practical or psychological? does this belong before or after termination?) suit `ProgressiveExplanation` reveals. **No simulator, no self-input.** |
| **Shared `FinalAssessment`** | **Yes — 8 questions** | Platform standard, unchanged. |
| **`WeekCompletionControl`** | **Yes** | Standard, independent of assessment result. Requires a route. |

---

## 7. Retrieval and application opportunities (source-grounded)

- **Discriminate homework failure type** — practical vs psychological vs masked vs therapist-side.
  The source supplies the masked-practical test verbatim („нека да се преструваме, че този проблем
  магически изчезва").
- **The 0–100% question** as a single retrievable rule, with its 90% threshold and three fallbacks.
- **Recognise the no-lose limit** — two consecutive missed weeks flips the strategy.
- **Order the taper ladder** with its real intervals.
- **Attribute progress correctly** — distinguish acknowledging an external factor from conceding the
  attribution.
- **Read a setback against Figure 18.1** rather than as evidence therapy failed.
- **Map a tool to its owning week** using the ten-item maintenance inventory — a natural
  course-wide retrieval exercise that also does integrative work Week 14 is uniquely placed to do.

---

## 8. Platform standards for implementation (when authorized)

Weekly Mind Map Preview + Review from one semantic model, collapsed by default · shared
`FinalAssessment` (8 questions), **no new local quiz implementation** · `WeekCompletionControl`,
independent of the assessment result · Course Progress via the existing route-based eligibility ·
`SourceReferences` + `OptionalReadingSource` with the verified chapter numbers (Гл. 17, Гл. 18) ·
scoped `white-space: normal` on every `.comparison-matrix` from the first commit.

---

## 9. Owner decisions required before implementation

| # | Decision | Recommendation |
|---|---|---|
| 1 | **Figure 18.3 (self-therapy guide)** — exclude / describe descriptively / reproduce | **Describe descriptively**, no usable form |
| 2 | **Figure 18.5 (booster guide)** — exclude / describe descriptively / reproduce | **Describe descriptively** |
| 3 | **Catalog objectives** — 2 stale, Гл. 17 unrepresented. Rewrite to actual ownership? | **Yes — rewrite to ~6**, matching §4.1 |
| 4 | **Title terminology** — `приключване` → `прекратяване`? | **Yes**, source term |
| 5 | **Declared format** — `StaticVisualization` contradicts a routed Mind Map week | **Change to `InteractiveModel`**, safety tier untouched (Week 13 precedent) |
| 6 | **Route** — assign `/kurs/sedmica-14`? | **Yes**, required for completion eligibility |
| 7 | **`поддържаща сесия`** terminology lock (4 competing forms) | **Lock** |
| 8 | **`списък на заслугите`** — keep Week 13's lock over Гл. 17/18's „кредити"? | **Yes**, Week 13 wins |
| 9 | **GAP-014** — Гл. 17 now has a confirmed owner; close it? | **Close for the homework thread**; see §10 |

**Safety level is not among these decisions.** `ProfessionalReviewRequired` is preserved.

---

## 10. Previously deferred/unassigned content that genuinely fits Week 14

Assessed against real source fit, not availability.

| Origin | Item | Verdict |
|---|---|---|
| **GAP-014** (`15_GAPS_AND_CONFLICTS.md`) | Гл. 2's „улесняване на когнитивна/поведенческа промяна между сесиите" / homework thread (C2-K26). GAP-014 states Гл. 17 is one „за която в текущия 15-седмичен куррикулум изобщо не съществува седмица" | **GENUINE FIT — and GAP-014's premise is factually wrong.** Week 14's title *begins* with „Домашна работа" and Гл. 17 is its primary source. The gap was written during the Week 5 audit without checking Week 14's catalog entry. **Recommend closing the homework half of GAP-014** against Week 14. |
| **GAP-014** | Гл. 2's „подчертаване на положителното" (C2-K25, printed 26–27) | **PARTIAL — do not annex.** Гл. 18's „приписване на напредъка на пациента" is adjacent but is a *different* mechanism (attribution for change vs. weekly extraction of positive data), and the credit-list half already belongs to Week 13. Leave unassigned. |
| **Week 13** (`WEEK_13_SOURCE_AUDIT_v1.md`) | C15-K115 (rationale explained first), C15-K119 (list started in session), C15-K121 (recording timing) — all three Deferred → „GAP-014 homework thread (no week owner)" | **GENUINE FIT.** All three are independently covered by Гл. 17's own text (K38–K39 rationale; K44 start-in-session; K46–K47 remembering) — Week 14 owns them **from its own source**, not by reassignment. |
| **Week 13** | C15-K05 — „therapists develop their own techniques as they become more skilled", Deferred → „Week 14/15" | **DOES NOT FIT.** Neither Гл. 17 nor Гл. 18 covers therapist development; that is Гл. 21 („Напредване като терапевт"), which is unassigned. **Re-route to Week 15 or leave unassigned** — do not absorb it here. |
| **Roadmap** §F | „предварителна safety рамка (без relapse-risk predictor, без personal plan builder)" | **STILL IN FORCE** — carried into §5 unchanged. |

---

## 11. Result (superseded by §12 — kept for audit trail)

**`SOURCE-READY PENDING OWNER DECISIONS`** was this audit's status at the end of the read-only pass.
176 KUs, 100% accounted, 0 Unaccounted. Chapters 17 and 18 verified against the printed table of
contents and read in full. Week 14's `NOT READY` roadmap status is closed. Nine owner decisions in §9
(two of them the `Needs Review` KUs) were required before implementation; the safety classification
was never one of them.

At this point in the session: no code touched, no commit, Week 4/5 untouched, locked weeks unopened.
**§12 records the owner's decisions and the implementation that followed in the same session — see
that section for the current status.**
