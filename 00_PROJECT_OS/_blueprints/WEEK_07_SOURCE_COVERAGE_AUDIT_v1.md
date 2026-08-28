# Week 7 — Source-First Audit v1 ("Поведенческа активация" / Behavioral Activation)

*Implemented — `/kurs/sedmica-7` (`Sedmica7.razor`). Not committed. Written per the Deep Learning
workflow (`AGENTS.md` → "Approved Deep Learning workflow"): full source read → KU inventory →
Coverage Matrix → Terminology Map → Representation Fit → owner blueprint review → **owner approved
all decisions below** → implementation (this pass) → QA → owner visual/learning review pending.*

## 0. Post-approval resolution (final accounting)

The owner approved all five decisions in §10 of the original audit and required three follow-ups
before implementation: (1) normalize every KU to exactly one status, (2) verify Figure 6.3 visually
against the PDF rather than trusting OCR, (3) resolve the two unnamed-patient threads only with
facts the source itself supports, never a composite biography. All three are resolved:

- **Sally's extension (7 KUs)**: confirmed source-grounded — she is explicitly *named* in the
  friends/behavioral-experiment/exercise passage (printed pp. 84–88: "Алисън и Джо", "мотивира Сали
  да го направи"). Moved **Needs Review → Included**, retold (not verbatim) in §7.3 of the page.
- **Figure 6.3, visually inspected against PDF page 117** (not just OCR): the printed table's
  Pleasure-column row order ("10=arguing with partner / 0=football match") genuinely **contradicts**
  the chapter's own continuous, cross-page dialogue (printed pp. 95–96), which unambiguously builds
  the scale the other way and is corroborated by the patient's own follow-up line ("вечерям с брат
  ми" answering the therapist's "какво би било нещо средно"). This is a real print-layout
  discrepancy, not an OCR artifact. **Resolution**: the dialogue-verified anchors are used
  (10=football match, 5=dinner with brother, 0=arguing with partner); the printed figure table's
  order, and the Mastery-column labels (which have no corroborating dialogue at all), are excluded.
- **The two unnamed-patient threads resolved as two distinct, source-grounded micro-examples, never
  merged with each other, Сали, or a composite**:
  1. The resistant patient ("wait until I feel better", printed pp. 89–90) — energy-graduation
     reframe only.
  2. A separate unnamed patient (printed pp. 95–99, continuous dialogue) — builds the Pleasure/
     Mastery scale, rates same-day activities, and is the confirmed source of **both**
     predict-vs-actual worked examples: friends (predicted 0–3, actual 3–5) **and** a second,
     previously-unconfirmed one found during this pass — a weekend run (predicted 4/4, actual 1/1,
     surfacing new interfering thoughts) — both now used in §7.6's retrieval interaction.
- All 14 previously-`Needs Review` KUs are now **Included** — every one resolved from the real
  source text itself, none invented.

## 1. Scope confirmed

- **CourseCatalog Week 7** (`CourseCatalog.cs:107-116`): module "Как работи КПТ и терапевтичният
  процес", title "Поведенческа активация", `CurriculumSafetyLevel.PublicWithAdaptation`, `route:
  null` (not yet built). Two placeholder objectives already recorded; planned format metadata says
  `Simulator` (pre-Deep-Learning-era placeholder — re-evaluated below under Representation Fit, not
  taken as a mandate).
- **Source**: SRC-041 (Judith S. Beck, *Cognitive Behavior Therapy, 2nd ed.*), **Chapter 6
  "Поведенческа активация"**, printed pages 80–99 (PDF pages 102–121, offset +22 per
  `_source_corpus/README_EXTRACTION_METHOD.md`). Title matches Week 7's `ShortSummary` verbatim —
  unambiguous chapter match, independently corroborated by a prior (pre-Deep-Learning) planning
  note already on file: `24_IMPLEMENTATION_ROADMAP.md:523` — *"Седмица 7 — потвърден източник (Гл.
  6)"*.
- **Extraction**: full chapter, 20 printed pages, extracted via the project's established
  `pypdf` method into `_source_corpus/SRC-041_ch06_bg_extracted.txt` (55,828 characters), read in
  full this session. OCR quality good; artifacts noted below (§6).
- **Adjacent weeks inspected for continuity/architecture reuse only, never as CBT source**: Week 6
  (Deep Learning reference — component/pattern precedent), Week 8 (Simulator Workspace — component
  precedent, shares the Situation→Thought→Emotion→Behavior chain concept), Week 3 (Concept and
  Diagram — cascading-loop SVG technique precedent for the vicious-cycle diagram).
- **Governance consulted**: `07_CONTENT_GOVERNANCE.md`, `23_CLINICAL_SAFETY_BOUNDARIES.md`,
  `06_QA_STRATEGY.md` (DoD v3), `COGNITIVE_LEARNING_ROLLOUT_PLAN_v1.md` §19,
  `11_SOURCE_REGISTER.md` (SRC-041 entry, `_source_corpus` calibration), `15_GAPS_AND_CONFLICTS.md`
  / `24_IMPLEMENTATION_ROADMAP.md` (no open GAP recorded against Week 7/Chapter 6).

## 2. Knowledge Unit inventory (54 units, Ch.6 content only)

Numbered in chapter reading order. Full one-line description of each is in §3's table; grouped here
by section for orientation:

- **U01–U05** — why behavioral activation matters as an early treatment goal (withdrawal pattern,
  dysphoria-maintaining behaviors, belief that mood can't be changed, self-efficacy rationale).
- **U06–U09** — "Conceptualizing inactivity": the initiation-blocking automatic-thought pattern and
  the inactivity→low-mood→fewer-opportunities→more-negative-thinking vicious cycle.
- **U10–U15** — "Concept of lack of mastery/pleasure": the *during/after*-activity self-critical
  thought pattern, distinct from the initiation-blocking pattern; the clinical need to anticipate
  both.
- **U16–U20** — reviewing the typical schedule; life-domain categories; the mastery/pleasure balance
  concept; distinguishing inherently-dysphoric activities from activities made dysphoric by
  depressive thinking.
- **U21, U25** — the two *generic, patient-independent* technique/psychoeducation points extractable
  from the illustrative dialogue without needing case specifics (staying-in-bed Socratic reframe;
  guiding the patient to generate their own evidence-based response).
- **U22–U24, U26–U29** — the case-specific behavioral-experiment mechanics (opposite-guess
  elicitation, situation→thought→emotion→behavior construction, experiment design, pre-written
  coping response) — **all flagged `NEEDS REVIEW`**, see §4.
- **U30–U32** — self-credit technique + rationale; the "can't become active / won't help" Q&A.
- **U33** — the second, more resistant patient's case opening — **`NEEDS REVIEW`**.
- **U34–U39** — the activity-before-mood psychoeducational reframe, energy-graduation principle,
  small-step breakdown, and the Activity Schedule tool as a concept.
- **U40** — that same second patient's follow-up session — **`NEEDS REVIEW`**.
- **U41–U44** — the Pleasure/Mastery Rating Scale concept, using it on current activities, the
  depression-distorts-memory rationale, timing best-practice for filling it in.
- **U45–U49** — homework review structure, and the predict-vs-actual comparison technique with its
  two worked outcomes (better-than-predicted; worse-than-predicted) — **all flagged `NEEDS REVIEW`**
  (case-history), U49 also **cross-references a later chapter** (not itself Ch.6 content to teach).
- **U50–U52** — chapter closing summary and therapist-stance takeaways.
- **U53–U54** — the two remaining Q&A boxes (can't generate pleasant activities → forced-choice
  list technique; schedule already full/overfull → rebalance guidance).

## 3. Coverage Matrix — 100% accounted for (final, post-approval)

| Disposition | Count | KU IDs |
|---|---:|---|
| **Included** | **54** | U01–U54 (all) |
| **Needs Review** | 0 | — resolved, see §0 |
| **Deferred** | 0 (see continuity notes below — not double-counted as KUs) | — |
| **Excluded** | 0 (see citation note below — folded into U53, not a separate KU) | — |
| **Unaccounted** | **0** | — |
| **Total** | **54** | |

**Continuity references (not counted as separate KUs — they're pointers *within* Ch.6 to other
chapters, not Ch.6 content of their own):**
- The "review the typical day, pages 50–52" cross-reference points into the assessment/first-session
  material already covered by **Week 6**'s Chapter 5 source — genuine continuity link, not new
  content to build here.
- The "self-credit, pages 274–276" cross-reference points to the future Homework chapter (Ch.17,
  roughly `24_IMPLEMENTATION_ROADMAP.md`'s Week 14 territory) — Week 7 teaches the *basic* self-credit
  concept (U30–U31, Included) but not that deeper treatment.
- U49's forward-link ("evaluate the key cognition… compare to your worst point, not your best") is
  explicitly next-chapter material per the source's own words — noted, not taught here.

**Citation note (folded into U53, not a separate Excluded KU):** the chapter cites an external
"Pleasant Events Schedule" (Lewinsohn/MacPhillamy 1982; a "Фриш (2005)" reference) with a URL. The
*technique* (forced-choice from a pre-made activity list) is Included as U53; the specific external
URL itself is not verified live and is not needed to teach the technique — recommend skipping it
entirely (matches this project's established caution with unverified external links) unless the
owner wants it raised as a future `OptionalReadingSource` candidate.

## 4. Needs-Review items and reasons

All 14 stem from one root cause: **this content is Sally's established case being extended with new
history** (or, for U33/U40, a second, unnamed patient). Per `AGENTS.md` → "Source-first content
rules": *"Fictional cases may be created only within an owner-approved/source-safe exercise scope.
Never add unapproved history to an established longitudinal character."* Sally is now an established
character (Weeks 3, 6, 8), so any of her new facts here — friends named Alison/Joe, a brother she has
dinner with, a partner she argues with, a weekend-running setback, specific 0–10 rating anchors —
count as new history and need an explicit go-ahead before use, not silent inclusion.

- **U22–U24, U26–U29** (7): the friends/behavioral-experiment dialogue — new Sally facts.
- **U33, U40** (2): the second patient's case — separately flagged because the source text never
  names this patient; using them at all requires deciding whether they're a distinct unnamed
  composite or (wrongly) conflated with Sally.
- **U45, U47** (2): Sally's predict-vs-actual social-activity outcome — new Sally facts.
- **U46, U48, U49** (3): the general predict-vs-actual technique (U46, safe/generic) is bundled here
  only because the source presents it exclusively *through* the two case examples (U47 Sally,
  U48/49 second patient) — needs a decision on whether to keep the technique but re-illustrate it
  with a fresh, source-safe worked number set instead of the book's exact figures.

**One data-integrity flag, not a content-safety issue:** Figure 6.3's OCR-extracted table (pleasure
anchors "10=football game / 5=dinner with brother / 0=arguing with partner" in the prose paragraph,
but the *figure caption table itself* extracts in a row order that looks reversed —
"10=arguing-with-partner / 0=football-game"). This is the same known class of artifact the corpus
README already documents for other chapters' flattened figures. **Needs visual confirmation against
PDF page 117 before any specific numbers are used in learner-facing content** — the *concept* of a
two-scale (pleasure + mastery), patient-anchored 0–10 rating system is source-solid regardless.

## 5. Terminology Map (reuse existing project terms, don't reinvent)

| BG (this chapter) | Established elsewhere in the project | Use |
|---|---|---|
| Поведенческа активация | — (new to Week 7) | New term to introduce, define on first use |
| Автоматична мисъл | Week 1/3/6/8 core term | Reuse verbatim, no redefinition needed |
| Поведенчески експеримент | Already used conceptually (Week 1 ResearchTurnStepper, Week 8 simulator) | Reuse verbatim |
| Овладяване / Удоволствие | New pairing (mastery/pleasure) | Introduce as a pair, keep both terms every time (never "mastery" alone) |
| График на дейности | New | "Activity Schedule" — introduce as a named tool, consistent with how the project already names tools (e.g. "Скала за оценка на когнитивна терапия") |
| Даване на кредит на себе си | New but simple | Keep the source's own idiom ("давам си кредит"), not a paraphrase |
| Дисфория | Used clinically in the chapter | Consider whether to keep as-is (matches source precision) or gloss with plain-language "потиснато настроение" on first use — implementation-phase wording decision, not blocking here |

## 6. Extraction caveats (OCR quality, matches established pattern for other chapters)

Good overall; minor artifacts: "TherapisT"/"paTienT" mixed-case OCR noise throughout (readable),
running-header garbling on a few pages, and — the significant one — **the two blank/near-empty
extracted pages (PDF pages 109–110, 113–114 / printed 87–88, 91–92) are Figures 6.1 and 6.2 (the
blank and partially-filled Activity Schedule grids)**, which are tables/graphics that don't OCR as
text. Their *content* is fully reconstructable from the surrounding dialogue (hour-by-hour entries
are read out loud in the transcript), so nothing is lost, but the grids themselves aren't
positionally verbatim — same caveat class as Ch.3/Ch.9's figures.

## 7. Representation Fit — recommended, not decided

- **Weekly Mind Map**: **Yes.** Same locked pattern as Weeks 3/6/8 (root left, spine, collapsed
  top-level clusters). Natural clusters from the KU groupings above: *"Защо неактивността задълбочава
  депресията"* (the vicious cycle, U06–U09), *"Преглед на графика и баланс на дейности"* (U16–U20),
  *"Скала за удоволствие и овладяване"* (U41–U44), *"Поведенчески експерименти за проверка на
  предсказания"* (U46/U53-level generic technique), *"Даване на кредит на себе си"* (U30–U31). Five
  clusters, consistent with the established standard.
- **Process/feedback-loop visualization**: **Yes — source-grounded, not decorative.** The vicious
  cycle (U09) is explicitly cyclical in the source's own words. Recommend reusing Week 3's
  cascading-loop SVG technique (`.cascade-loop`, established in `Sedmica3.razor` §09) rather than
  inventing a new diagram mechanism.
- **Simulator**: **Partially — recommend a lighter shape than Week 8's, not "no."** The source's own
  strongest interactive moment is the *predict → reveal actual → compare* pattern (§ "Използване на
  графика на дейностите за оценка на точността на предсказанията"). That maps cleanly to a small,
  fixed-scenario "predict, then reveal" comparison (closer in weight to Week 3's
  `SchemaFilterDemonstration` toggle-reveal than to Week 8's branching `CbtChainSimulator`) — no
  personal data, no diagnosis, fixed worked example, matching every existing simulator's safety
  contract. Recommend against a full new branching engine unless the owner specifically wants one;
  the source doesn't ask for that much interactivity.
- **Case/application exercise**: **Contingent on the Needs-Review case decision (§4).** If Sally is
  extended: a short, retold (not verbatim) worked example, matching the Week 3/6/8 precedent. If the
  owner prefers not to extend Sally here: the predict-vs-actual demonstration can run with a fresh,
  explicitly-generic, unnamed scenario instead, sidestepping the case-history question entirely
  while keeping the technique.

## 8. Retrieval/application opportunities (DoD v3, four-category taxonomy)

- **Recognition**: given a thought, identify whether it's more likely to block *starting* an
  activity or undercut *enjoying* one (U06–09 vs. U10–15 — a real, source-grounded distinction).
- **Retrieval**: reconstruct the vicious-cycle sequence in order (reuses the established
  reconstruct-the-order pattern from Week 6's Review Map).
- **Application**: given a short fixed scenario, predict pleasure/mastery ratings, then compare to a
  revealed "actual" value — directly mirrors the source's own technique (§7's simulator-lite
  recommendation doubles as the retrieval-practice vehicle, not a separate build).
- **Reasoning**: explain *why* predicting-then-comparing is useful (surfaces the general principle
  that depression can distort in-the-moment perception/memory — U43 — so external tracking corrects
  for that bias).

## 9. Safety boundary

Chapter 6 is written entirely from a **therapist's** first-person perspective, instructing
clinicians how to *run* behavioral activation in session (Socratic elicitation, experiment design,
homework structuring). None of that procedural "how a therapist does this to/with a patient" framing
is appropriate as direct-to-learner instruction. Everything recommended `Included` above is scoped to
the **psychoeducational model and concepts** (why activity/mood are linked, what the tools are for,
how the logic works) — never "here is how you should run this on yourself," matching
`CurriculumSafetyLevel.PublicWithAdaptation` and the same educational/third-person framing already
established for Weeks 1/3/6/8/10/12.

## 10. Genuine owner decisions required before implementation

1. **Extend Sally into Week 7?** Her friends/exercise/schedule material (7 KUs) and her
   predict-vs-actual outcome (2 KUs) are new history for an established character. Yes (retold, not
   verbatim, matching Week 3/6/8 precedent) / No (use a fresh unnamed generic scenario instead) /
   Partial (extend for one thread, e.g. the schedule review, not the other).
2. **The unnamed second, more resistant patient (U33/U40, 2 KUs)** — use as a distinct, explicitly
   unnamed composite illustration, or drop this thread entirely and cover the "very inactive
   patient" angle through Sally alone (adapted) or through generic prose?
3. **Figure 6.3's exact anchor numbers** — needs a visual check of PDF page 117 before any specific
   pleasure/mastery values are used; the two-scale *concept* itself is not in question.
4. **Simulator weight** — confirm the recommended lightweight "predict → reveal → compare" shape
   over a full branching engine (§7), or explicitly ask for the heavier pattern.
5. **Pleasant Events Schedule external citation** — skip entirely (recommended) or raise as an
   `OptionalReadingSource` candidate pending live-URL verification.
