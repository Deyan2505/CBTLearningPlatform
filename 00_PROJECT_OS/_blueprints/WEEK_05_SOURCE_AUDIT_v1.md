# Week 5 Source Audit — "Принципи на КПТ и терапевтичен съюз"

Read-only Deep Source + Coverage Audit, three-turn, **`OWNER APPROVED — FINAL` (2026-09-01)**.
Turn 1 audited Chapter 1 only (provisional); turn 2 authorized and read Chapter 2 in full; **turn 3
records final owner sign-off on the merged 52-KU accounting, terminology, structure, and
Representation Fit — see §6.** No code touched, no build/tests run this session. Week 4 remains
`IMPLEMENTED / TECHNICALLY READY — NOT YET OWNER APPROVED / LOCKED` (Section 08 pending), untouched
by any turn of this audit.

**`SOURCE-READY FOR IMPLEMENTATION: YES`** — this audit is the authoritative source/KU basis for a
future Week 5 implementation session. Implementation itself was explicitly out of scope for all
three turns and did not happen here.

## §0 — Source scope (final)

**SRC-041 (Judith S. Beck), Chapter 1** — "КАКВИ СА ОСНОВНИТЕ ПРИНЦИПИ НА ЛЕЧЕНИЕТО?" subsection,
printed pp. 6–11 (PDF pp. 28–33). Read turn 1, from the pre-existing Chapter 1 extraction
(`SRC-041_ch01_bg_extracted.txt`, originally cached for Week 1).

**SRC-041 (Judith S. Beck), Chapter 2 — "ОБЗОР НА ЛЕЧЕНИЕТО"** ("Overview of Treatment"), printed
pp. 17–28 (PDF pp. 39–50), the entire chapter. Read turn 2, in full, boundary-verified: extraction
starts exactly at "Глава 2 ОБЗОР НА ЛЕЧЕНИЕТО" (p.17) and ends exactly where "Глава 3 Когнитивна
концептуализация" begins (p.29, outside the extracted range) — confirmed against
`README_EXTRACTION_METHOD.md`'s table of contents. Newly extracted this turn using the same method
(`pypdf`, same source PDF, same +22 page-offset formula) and cached at
`00_PROJECT_OS/_source_corpus/SRC-041_ch02_bg_extracted.txt` (gitignored, not committed). Source PDF
located on this machine at the equivalent per-user OneDrive path (414 pages total, matching the
documented page count).

**Why Chapter 2 was necessary (confirmed, not assumed):** Chapter 1's Principle 2 explicitly defers
alliance depth to Chapter 2 ("Вижте глава 2 за по-дълго описание на терапевтичната връзка"). Chapter
2 turned out to be a 5-thread "overview" chapter (alliance; treatment planning/session structuring;
identifying/responding to dysfunctional cognitions; emphasizing the positive; between-session
change/homework) that itself points forward to Chapters 5/6/7/8/9/11/17 for each thread's full
depth. Only the **first thread** (developing the therapeutic relationship, pp. 17–21) is Week 5's
territory; the other four are each other weeks' — confirmed below, not assumed.

## §1 — Final merged Knowledge Unit inventory

**52 KUs total — 30 Included / 10 Deferred / 12 Excluded / 0 Needs Review / 0 Unaccounted.**

### Chapter 1 KUs (K01–K25, unchanged from turn 1 except K15)

Per owner decision, **K15 is now Excluded** (was `Needs Review`): the suicide/severity parenthetical
in Principle 7 does not carry into Week 5's learner-facing content. Chapter 1's KU set is otherwise
unchanged from the turn-1 audit (see prior table below); Chapter 1 revised counts: **13 Included / 6
Deferred / 7 Excluded / 0 Needs Review.**

| ID | KU | Locator | Status | Why |
|----|----|----|--------|-----|
| K01 | Opening frame: therapy is individualized, but principles underlie CBT for all patients | p.6 | **Included** | Section's own framing sentence |
| K02 | Meta-note (Sally illustrates principles) + external citation list | p.6 | Excluded | Owner decision #7 — bibliographic name-drop, no learner-facing value |
| K03 | Sally's formal case-intro facts (age/status/semester/DSM-IV-TR dx) | pp.6–7 | Deferred → Week 3 | Week 1's U24 precedent — Sally's canonical intro is Week 3's |
| K04 | Principle 1: ongoing individual cognitive conceptualization | p.7 | **Included** | One-sentence principle claim |
| K05 | Principle 1's Sally 3-timeframe illustration | p.7 | Deferred → Week 3/4 | Re-teaches LOCKED Week 3 model / duplicates Week 4 bridge |
| K06 | Principle 2: stable therapeutic alliance, core conditions (warmth/empathy/care/respect/competence) | pp.7–8 | **Included** | Title-anchor content |
| K07 | Principle 2 cont'd: success-highlighting, realistic optimism, end-of-session feedback | p.8 | **Included** | Concrete alliance behaviors |
| K08 | Principle 3: collaboration/active participation, teamwork, shifting activity balance | p.8 | **Included** | Second title-anchor |
| K09 | Principle 4: goal-oriented, problem-focused; concrete first-session goal-setting | p.8 | **Included** | One-sentence principle claim |
| K10 | Principle 4's full Sally worked example | pp.8–9 | Deferred → Weeks 8/9/10 | Automatic-thought/behavioral-experiment mechanics |
| K11 | Principle 5: present-focus + 2 named exceptions | pp.8–9 | **Included** | Principle + source-explicit exceptions |
| K12a | Principle 5's Sally core-belief illustration | p.9 | Deferred → Week 12 | Core-belief content ahead of LOCKED Week 12 |
| K12b | Principle 5's personality-disorder aside | p.9 | Excluded | Diagnostic-category-specific |
| K13 | Principle 6: educational aim, "be your own therapist," relapse prevention | p.9 | **Included** | Distinct, no overlap |
| K14 | Principle 7: time-limited, session count/tapering/booster pattern | pp.9–10 | **Included** | No other week owns treatment length |
| K15 | Principle 7's suicide/severity parenthetical | p.9 | **Excluded** *(was Needs Review)* | **Owner decision #2, turn 2** |
| K16 | Principle 8: structured sessions, one-sentence mention only | pp.9–10 | **Included (1 sentence only)** | Names, doesn't render breakdown |
| K17 | Principle 8's full structure breakdown | pp.9–10 | Deferred → Week 6 | LOCKED Chapter-5 territory |
| K18 | Principle 9: identify/evaluate/respond to dysfunctional thoughts; guided discovery; "collaborative empiricism" named | pp.9–10 | **Included (principle + term only)** | Term aligned per owner decision #4 |
| K19 | Principle 9's deeper mechanics + Sally example | p.10 | Deferred → Weeks 8/9/10 | LOCKED territory |
| K20 | Principle 10: technique pluralism | pp.10–11 | **Included** | Distinct, standalone |
| K21 | Principle 10's Gestalt/psychodynamic/Axis-II examples | p.11 | Excluded | Diagnostic-label-adjacent specifics |
| K22 | Closing: principles apply to all, therapy varies by individual (variability factors) | p.11 | **Included** | Safe closing frame |
| K23 | Disorder-specific technique examples (panic/anorexia/substance abuse) | p.11 | Excluded | Disorder-specific clinical content |
| K24 | "What does a session look like" section (heading + content) | pp.11–12 | Excluded from Week 5 | Week 6's Chapter-5 territory |
| K25 | "Developing as a CBT therapist" section | p.12+ | Excluded | Therapist-training content, matches Week 1's U26 |

### Chapter 2 KUs (C2-K01–C2-K26, new this turn)

| ID | KU | Locator | Status | Why |
|----|----|----|--------|-----|
| C2-K01 | Chapter's own framing: 5 threads run through every session (alliance; planning/structuring; identifying/responding to dysfunctional cognitions; emphasizing the positive; between-session change/homework) | p.17 | **Included** | Orients the reader: only thread 1 is Week 5's |
| C2-K02 | Trust/rapport-building starts at first contact; positive alliances correlate with positive outcomes | p.17 | **Included (finding only, no named citation)** | Owner decision (turn 3): the source's own author-name rendering is OCR-uncertain — never used in learner-facing copy; only the general, source-supported finding is kept; a named citation may only be added later after direct visual verification against the source PDF, not the OCR extraction |
| C2-K03 | Caveat: harder with severe mental illness / Axis-II pathology | p.18 | Excluded | Diagnostic-category-specific, matches K12b |
| C2-K04 | 6-item concrete alliance-building action list (counseling skills; share conceptualization+plan; joint decisions; seek feedback; vary style; help solve problems) | p.18 | **Included** | Orienting sub-list for the sub-sections that follow |
| C2-K05 | "Demonstrating good counseling skills": continuous empathic engagement, "be a good person in the room," treat patients as you'd want to be treated | p.18 | **Included** | Core therapist-stance content |
| C2-K06 | Implicit/explicit supportive-messages list (only when genuinely felt): CBT will help; not overwhelmed by your problems; helped others like you; I care/value you; want to understand your experience; confident we can work well together | pp.18–19 | **Included** | Paraphrased content, not a verbatim script |
| C2-K07 | Caveat: if you can't honestly support these messages, seek supervisory help re: your own reactions | p.19 | Excluded | Therapist-training/supervision content, matches Week 1's U26 |
| C2-K08 | Alliance-outcome feelings list: likeable (warmth), less alone (teamwork), optimistic (realistic hope), self-efficacious (credited progress) | p.19 | **Included** | Why the alliance helps, safe |
| C2-K09 | Myth-busting: CBT is not cold/mechanical — the first CBT manual (Beck et al., 1979) itself stresses the relationship | p.19 | **Included** | Corrects a common public misconception, fits `PublicWithAdaptation` |
| C2-K10 | Sharing conceptualization aloud + checking "does this ring true" (principle) | p.19 | **Included** | General principle, safe |
| C2-K11 | Worked dialogue illustrating conceptualization-sharing (generic patient, mother-phone-call scenario) | p.19 | Excluded | Reproduced-dialogue-style illustration — same boundary as Week 4/12 |
| C2-K12 | Eliciting feedback on conceptualization accuracy strengthens alliance + improves accuracy | p.19 | **Included** | Safe general principle |
| C2-K13 | Joint/collaborative decision-making: prioritizing problems together; providing rationale + eliciting buy-in | pp.19–20 | **Included** | Deepens Chapter 1's Principle 3 with a concrete mechanic |
| C2-K14 | Continuous monitoring of patient's emotional reactions; addressing rising distress in the moment | p.20 | **Included** | Deepens Principle 2's feedback content |
| C2-K15 | Handling patients' negative reactions to therapy/therapist: validate first, then conceptualize/plan; unaddressed reactions risk drop-out | p.20 | **Included** | Alliance difficulty/repair content — directly requested |
| C2-K16 | End-of-session feedback-seeking even when alliance seems strong; may be first professional ever to ask; patients feel honored | p.20 | **Included** | Deepens Principle 2 further |
| C2-K17 | "Varying your style": some patients react negatively to warmth (e.g. "too touchy"); adapt self-presentation | p.20 | **Included** | Source-grounded personalization principle — directly requested |
| C2-K18 | Effective/competent therapy itself strengthens the alliance; alliance strengthens as symptoms improve (DeRubeis & Feeley 1990; Feeley, DeRubeis & Gelfand 1999) | pp.20–21 | **Included** | Alliance is dynamic, not static — safe, citable |
| C2-K19 | Alliance used to help patients test core-belief validity; if stable, avoid over-spending time on it — maximize time on actual problem-solving | p.21 | **Included (1 boundary sentence)** | Directly requested "boundary between alliance and technique"; not elaborated with core-belief mechanics (Week 12's) |
| C2-K20 | Personality-disorder patients need more alliance-focused attention + advanced strategies | p.21 | Excluded | Diagnostic-category-specific, matches C2-K03/K12b |
| C2-K21 | Treatment planning & session-structuring thread in full (3-part session breakdown, in-session homework-setting, interrupting technique) | pp.21–23 | Deferred → Week 6 | Chapter itself cross-refers to Ch.5/7/8; **duplicate-risk: HIGH if pulled into Week 5** |
| C2-K22 | Integrative sentence: structure is applied flexibly, never mechanically/impersonally, even with a standard format | pp.21–22 | **Included** | Alliance↔structure boundary statement; does not re-teach Week 6's mechanics |
| C2-K23 | Identifying/responding to dysfunctional cognitions thread (guided-discovery questions, behavioral experiments, core-belief/assumption preview) | pp.22–26 | Deferred → Weeks 8/9/10 | Chapter cross-refers to Ch.9/11; **duplicate-risk: HIGH** |
| C2-K24 | Verbatim therapy transcript embedded in the cognitions thread (bookstore job-search dialogue) + coping-card example | pp.23–26 | Excluded | Reproduced dialogue, same boundary as C2-K11/Week 4/Week 12 |
| C2-K25 | "Emphasizing the positive" thread (strengths at assessment, weekly positive-data elicitation, positive-adaptation callouts, pleasure/accomplishment homework) | pp.26–27 | Deferred, **no current dedicated week** | Genuine open curriculum gap — not Week 5's under its "principles + alliance" title (see §5) |
| C2-K26 | Between-session change/homework thread | pp.27–28 | Deferred, **no current dedicated week** | Chapter cross-refers to Ch.6 (partially Week 7's) and Ch.17 (no week exists at all in the 15-week curriculum) — flagged as an open gap, not resolved here |

## §2 — Terminology Map (`OWNER LOCKED`, final)

- **"Терапевтичен съюз" — OWNER-LOCKED canonical learner-facing term** (decision #4, turn 3). Week 5
  is this term's first substantive definition site; now backed by both chapters (depth from Ch.2
  §1). Future weeks should cross-link, not redefine.
- **"Колаборативен емпиризъм"** — OWNER-LOCKED canonical learner-facing term (decision #4, turn 2,
  reaffirmed turn 3), aligned with LOCKED Weeks 9/10. Chapter 1's own wording ("съвместен
  емпиризъм," K18) is preserved only in this audit's source notes, never in learner-facing copy.
- **"Съюз" vs. "алианс" — OWNER-LOCKED (decision #4, turn 3):** Chapter 2's extraction alternates
  between "терапевтичен съюз" (Ch.1, and most of Ch.2) and "работна алианс"/"терапевтичния алианс"
  (a transliteration, appearing twice in Ch.2, e.g. p.21). **Resolved:** learner-facing copy uses
  only "терапевтичен съюз"; "алианс" remains solely as a source-wording note in this audit, never
  surfaces on the page.
- **"Ръководено откритие" (Ch.1) vs. "управлявано откритие" (Ch.2, p.23)** — two different Bulgarian
  renderings of "guided discovery" within the same source book. Not blocking for Week 5 (the content
  using this term, C2-K23, is Deferred to Weeks 8/9/10), but flagged for whoever eventually reconciles
  Week 9/10's own Chapter 11 sourcing against this chapter's wording.
- **"Основно вярване"** — Week 5 must not introduce this term ahead of Week 12; K12a/C2-K19 stay
  scoped to avoid this (C2-K19 names "core belief" only as the *target* of the alliance-boundary
  sentence, without teaching how core beliefs are identified or modified).
- **OCR/citation handling — OWNER-LOCKED (decision #3, turn 3):** C2-K02's source-text author
  rendering ("Рауе и Голдфрид, 1994") is OCR-uncertain and **must never appear in learner-facing
  copy**. Only the general, source-supported finding survives (positive alliance quality correlates
  with positive treatment outcomes) — stated with no named citation. If a named citation is ever
  required later, it requires direct visual verification against the source PDF itself first, not
  re-reliance on this OCR extraction.

## §3 — Representation Fit (`OWNER LOCKED`, decision #5, turn 3)

- **Principle card grid** (Chapter 1's 10 principles): unchanged from turn 1 — reuse
  `.category-compare`, no new component, no Mind Map/Concept Map (flat list, not a hierarchy).
- **Alliance section** (now substantially deepened by Chapter 2): fits the same reusable pattern
  family — `LearningSection`/`ProgressiveExplanation` prose for the "why it matters" and "6 actions"
  content, an existing card/list pattern for the 6-item action list (C2-K04) and the 4-item
  outcome-feelings list (C2-K08), consistent with Week 9/10/12's category-list precedent. No new
  component needed even with the added depth.
- **Collaboration before/after comparison** (Principle 3 + C2-K13): **approved, source-supported** —
  Chapter 2 adds the concrete "provide rationale + elicit buy-in" mechanic on top of Chapter 1's
  directive-early/active-later framing, strengthening (not just permitting) this representation.
  Reuse Week 10's comparison-card pattern.
- **Session-tapering sequence** (Principle 7, K14): **approved on Chapter 1 alone** — Chapter 2 adds
  nothing to this specific content, so no change from turn 1. Must render as descriptive ("a typical
  pattern, e.g. Sally's course") not a rigid universal protocol, per owner decision #6 — e.g. avoid
  imperative phrasing like "sessions follow this schedule," prefer "sessions often follow a pattern
  such as...".
- **Alliance-repair content** (C2-K15, validate → conceptualize → plan): fits a simple 3-step
  process mention (prose or a compact numbered list), not a new interactive stepper — proportionate
  to a `PublicWithAdaptation`, `GuidedDemonstration`-format week.
- **New interactive component: still not recommended.** Nothing in the merged scope has
  decision-branching or simulation shape.
- **Retrieval practice additions from Chapter 2:** distinguish "alliance-building action" (C2-K04's
  6 items) from "principle" (Chapter 1's 10) as two related but separate lists; a repair-scenario
  recognition item ("patient looks upset mid-session — what does the source say to do first?" →
  validate, matching C2-K15).

## §4 — Retrieval/application opportunities (final)

Unchanged from turn 1, plus:
- Recognition: match each of the 6 alliance-building actions (C2-K04) to its one-line description.
- Application: given a short, generic (non-Sally, non-clinical-diagnosis) scenario where a patient
  reacts negatively mid-session, identify the correct first step (validate, not immediately explain
  or defend) — grounded in C2-K15.
- Comparison retained from turn 1: alliance (Principle 2/C2 §1) vs. collaboration (Principle 3) as a
  common learner confusion point, now richer — alliance is the relational foundation (trust,
  warmth), collaboration is the working method (joint agenda-setting, shifting activity balance).

## §5 — Boundary check against neighboring weeks (final)

**Week 5 uniquely owns (expanded by Chapter 2):**
- The 10 principles at principle-level (Chapter 1, unchanged).
- **Now substantially owns real alliance depth**, not just Principle 2's short paragraph: why the
  alliance matters, the 6 concrete building actions, therapist-stance behaviors, personalization
  (varying style), monitoring/repairing alliance ruptures, and the boundary between alliance-time
  and technique-time (C2-K19/K22).

**Must cross-link only, never re-teach (all `LOCKED`):**
- **Week 3** — Sally's case, conceptualization model (K03, K05).
- **Week 6** — full session-structure breakdown (K17, **C2-K21 confirmed duplicate-risk: HIGH**).
- **Week 9/10** — automatic-thought/distortion/Socratic mechanics (K10, K19, **C2-K23/C2-K24
  confirmed duplicate-risk: HIGH**, plus a reproduced-dialogue exclusion).
- **Week 12** — core beliefs/schemas (K12a; C2-K19 stays at boundary-sentence depth only).

**Cross-link with a "not yet final" caveat:**
- **Week 4** — same caveat as turn 1 (Section 08 pending, not `LOCKED`).
- **Week 8** — automatic thoughts/emotions, same as turn 1.

**Confirmed unassigned — recorded, not absorbed (decision #2, turn 3):**
- **C2-K25 ("emphasizing the positive")** and **C2-K26 (homework/between-session change)** are two
  of the chapter's own 5 named threads, each with real source-grounded content, but **neither maps
  to any existing or planned week title** in the current 15-week curriculum. Owner-confirmed: kept
  formally **Deferred / unassigned future scope** — not forced into Week 5 or any other week.
  Recorded platform-wide as **`GAP-014`** in `00_PROJECT_OS/15_GAPS_AND_CONFLICTS.md` and cross-noted
  in `00_PROJECT_OS/24_IMPLEMENTATION_ROADMAP.md` §B (Week 5 source status), so the finding survives
  beyond this single audit file. Does not block Week 5's `SOURCE-READY` status.

## §6 — Owner decisions — `RESOLVED` (turn 3, 2026-09-01)

All open questions from turns 1–2 are now resolved. No genuine owner decision remains outstanding
for the *source/audit* layer; the next open decisions belong to an actual implementation session
(exact component wiring, CSS reuse specifics, route activation), not to this audit.

1. **Final KU accounting — OWNER APPROVED for source scope:** 52 total, 30 Included / 10 Deferred /
   12 Excluded / 0 Needs Review / 0 Unaccounted. Explicitly **not** the final learner-facing content
   count (Included ≠ number of on-page sentences) — it is the source-coverage denominator.
2. **C2-K25/C2-K26 curriculum-gap:** kept **Deferred / unassigned future scope**. Not forced into
   Week 5 or any other week. Recorded as `GAP-014` (`15_GAPS_AND_CONFLICTS.md`) and cross-noted in
   `24_IMPLEMENTATION_ROADMAP.md` §B.
3. **C2-K02's citation — resolved:** no author name used in learner-facing copy; only the general,
   source-supported finding retained; a named citation requires direct visual PDF verification
   first, not this OCR extraction.
4. **Terminology — locked:** `терапевтичен съюз` is the canonical learner-facing term; `алианс`
   stays in source/terminology notes only. `колаборативен емпиризъм` stays aligned with Weeks 9/10.
5. **Structure and Representation Fit — locked** (§3): 10-principle card grid; substantial
   therapeutic-alliance section; collaboration before/after comparison; descriptive (non-protocol)
   Principle 7 tapering sequence; no Mind Map; no Concept Map; no new component.
6. K15 excluded, Chapter 2 authorized/read, Sally-vignette scope, and all turn-1/turn-2 decisions
   remain as resolved in §1–§5 above.

**Not implemented (any turn). Week 4 untouched. No build/tests run.**
**`SOURCE-READY FOR IMPLEMENTATION: YES.`**
