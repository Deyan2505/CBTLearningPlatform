# Week 1 Retrofit Audit — "Как се ражда когнитивната терапия"

Source: SRC-041 (Judith S. Beck, *Cognitive Behavior Therapy: Basics and Beyond*, 2nd ed.),
**Глава 1 "Въведение в когнитивно-поведенческата терапия"**, printed pp. 1–16 (PDF pages 23–38).
Full chapter read this session; extraction cached at
`00_PROJECT_OS/_source_corpus/SRC-041_ch01_bg_extracted.txt` (gitignored, not committed). This is
the first full read of Chapter 1 — the current page's dev comment cites it, but per its own
wording ("GAP-012 closed") only two specific facts (the 1979 citation's authors/date and the
"automatic thoughts aren't framed as invalid" reconciliation) were previously confirmed via
session-log narration, not a full chapter read.

Current page: `Sedmica1.razor` (272 lines), "Theory and History" archetype — the platform's first
week of this archetype, distinct from Week 8's "Simulator Workspace" or Week 12's "Concept and
Diagram." One interactive island (`ResearchTurnStepper`, a 4-step generic hypothesis→reformulation
stepper) plus `HistoricalTimeline` (6 generic milestones) as the visual model.

## §0 — KU accounting (Unaccounted = 0)

**Owner-approved implementation pass (this section supersedes §1's draft statuses below).**

**27 Knowledge Units total** — **13 Included / 0 Needs Review / 10 Deferred / 4 Excluded.**

Owner's decision #1 scoped the addition to a specific, closed list (dream-study hypothesis/
result, two-streams discovery paraphrased, 1977 Beck/Rush RCT vs. imipramine, 1979 milestone) —
narrower than this audit's original 15-Included draft. Per decision #9's conservative default,
two draft-Included items outside that list were reclassified to Deferred rather than added:
- **U4** (Beck first named it "cognitive therapy," now synonym with "CBT") → **Deferred** — a
  naming fact, not part of the approved origin-story list; overlaps `/kpt`'s existing "what is
  CBT" territory, already cross-linked from this page.
- **U5** (general CBT definition: structured/short-term/present-oriented) → **Deferred** — same
  reasoning as U4.
- **U7** (influences + related-therapies name lists) → **Excluded** (was Needs Review) — owner
  decision #3: no standalone name-list; no name in this set was necessary to explain the specific
  historical sequence implemented.

Final Included set (13): U1, U2, U3, U11, U14, U15, U16, U17, U18, U19, U20, U21, U22.
Final Deferred set (10): U4, U5, U6, U8, U9, U10, U13, U23, U24, U25.
Final Excluded set (4): U7, U12, U26, U27.

Implemented in `Sedmica1.razor` / `ResearchTurnStepper.razor`: U1/U2/U3 (psychoanalytic origin +
dream-study hypothesis) and U14–U16 (hypothesis, method, surprising result) enriched into
`HistoricalTimeline` milestones 1–2 and `ResearchTurnStepper` steps 1–3 (same 4 steps/6
milestones, no new components); U17/U18 (two-streams discovery, patient anecdote paraphrased,
never quoted) in `HistoricalTimeline` milestone 3, `ResearchTurnStepper` step 4, and a new §03
paragraph; U19/U20 (Rush, 1977 RCT vs. imipramine) in `HistoricalTimeline` milestone 4 and a new
§06 "1977: Изследването на Бек и Ръш" (replacing the removed unsupported section); U21 (1979
manual, already present) in §07, now with an explicit two-years-later connective sentence to §06;
U22 (anxiety-disorder extension) in `HistoricalTimeline` milestone 6. Comprehension check: old Q3
(tied to the removed §06) replaced with a dream-study question; one new Q4 added on the 1977 RCT.
"Какво да запомните" and citations updated; the Rush/Beck/Kovacs/Hollon (1977) citation added
using only the author/year form the source itself gives (no invented journal/volume/pages).

## §1 — Knowledge Unit inventory

| ID | KU | Locator | Status | Why |
|----|----|---------|--------|-----|
| U1 | Beck's origin: trained/practicing psychoanalyst, assistant professor of psychiatry at Univ. of Pennsylvania, sought empirical validation of psychoanalytic theory (early 1960s) | p.1 | **Included** | Direct origin context, currently only vaguely gestured at |
| U2 | His early experiments produced the *opposite* of the expected validation | p.1 | **Included** | The pivot moment — currently only abstracted generically |
| U3 | He identified distorted negative cognition as depression's core feature and developed a short-term treatment targeting the reality of depressive thinking | p.1 | **Included** | Direct throughline to "what CBT is" |
| U4 | Beck first called it "cognitive therapy" (early 1960s); "CBT" is now used as a broader synonym | p.2 | **Included** | Basic naming fact, currently absent |
| U5 | General definition: structured, short-term, present-oriented psychotherapy for depression, aimed at solving current problems and modifying dysfunctional thinking/behavior (Beck 1964) | p.2 | **Included** | Core definitional anchor, currently absent |
| U6 | Adaptation across populations/disorders while theoretical assumptions stay constant; treatment grounded in cognitive formulation + individual case conceptualization | p.2 | Deferred | Case-conceptualization is Week 3's territory |
| U7 | Intellectual-lineage name list (Epictetus, Horney, Adler, Kelly, Ellis, Lazarus, Bandura) + related CBT-family therapies list (REBT, DBT, ACT, exposure therapy, etc.) | p.2 | Needs Review | Genuine context, but a long name-list risks reading as trivia — a one-sentence mention may be the right size |
| U8 | Adapted across education/income/culture/age; used in primary care, schools, prisons; group/couple/family formats; session-length flexibility | pp.2–3 | Deferred | Practice-setting detail, not "origin" narrative |
| U9 | Theoretical model summary: dysfunctional thinking common across disorders; automatic thought → emotion/behavior chain; bounced-check example | p.3 | Deferred | Week 3's / Week 8's architecture territory — page already correctly previews-and-links instead of re-explaining |
| U10 | Deeper level: core beliefs; modifying them yields lasting change; incompetence example | p.3 | Deferred | Week 12's territory |
| U11 | Research evidence: first outcome study 1977 (Rush, Beck, Kovacs & Hollon); 500+ outcome studies by time of writing; broad efficacy | pp.4–5 | **Included** | Brief, supports "scientific transition" framing, sets up U20 |
| U12 | Table 1.1's full disorder list (3-column table) | p.4 | Excluded | Long clinical list, OCR-garbled in extraction, not narrative-essential |
| U13 | Further evidence strands: community-setting studies, computer-assisted CBT, neurobiological-change studies, depression/anxiety cognitive-model studies | p.5 | Deferred | Supporting detail, not core "birth" narrative — one line already covers it via U11 |
| U14 | **Late-50s/early-60s: Beck tests the psychoanalytic idea that depression = hostility turned inward, via a dream study expecting more hostility themes in depressed patients** | p.5 | **Included** | The chapter's actual empirical starting point — currently entirely missing |
| U15 | **Unexpected result: dreams showed *less* hostility and *more* themes of defectiveness, deprivation, and loss — matching patients' waking thought content** | p.5 | **Included** | The concrete "surprise" the current generic stepper only gestures at abstractly |
| U16 | Further studies suggested the psychoanalytic "need to suffer" idea might be inaccurate (Beck, 1967) — the "falling domino" moment prompting a search for an alternative explanation | p.5 | **Included** | Direct causal link to why Beck kept looking |
| U17 | **"Two streams of thinking" discovery (free association + rapid self-evaluative thoughts) — illustrated by the specific patient anecdote: a woman describing sexual experiences reports anxiety; Beck's incorrect interpretation ("you thought I was criticizing you") vs. her correction ("I was afraid I was boring you")** | p.5 | **Included** | A real, directly quotable historical anecdote — not invented, currently completely absent |
| U18 | Beck recognized this "automatic" negative thought stream across his depressed patients, tied closely to emotion, and began helping patients identify/evaluate/respond to it | p.5 | **Included** | The direct origin of "automatic thoughts" as a concept — ties into Week 8's link-out |
| U19 | Beck taught the method to his psychiatric residents at Penn, whose patients also responded well | p.5 | **Included** | Sets up U20 |
| U20 | **Dr. A. John Rush (then chief resident, later a leading depression authority) proposed a formal study; the 1977 RCT found cognitive therapy as effective as imipramine — one of the first times talk therapy was compared head-to-head with medication** | pp.5–6 | **Included** | Concrete, citable, and currently missing entirely — the 1979 manual (already on the page) has no lead-up without this |
| U21 | Beck, Rush, Shaw & Emery (1979) published the first CBT treatment manual, two years after the 1977 study | p.6 | **Included** | Already on the page (§07) — U20 gives it its missing lead-up |
| U22 | Late-1970s extension to anxiety disorders (Beck + postdocs at Penn): different focus (risk evaluation, internal/external resources, reduced avoidance, behavioral testing of feared predictions); refined per-disorder since; now taught in most graduate programs | p.6 | **Included** | Shows the model's growth beyond depression — part of the "birth and growth" arc, safety-neutral |
| U23 | The 10 basic principles of CBT, illustrated via Sally throughout (alliance, collaboration, goal-focus, present-focus with 2 exceptions, educational/relapse-prevention aim, time-limited, structured sessions, technique diversity, conceptualization, automatic-thought work) | pp.6–11 | Deferred | Each principle previews a *different* future week (conceptualization→Week 3, automatic thoughts→Week 8/9, session structure→Weeks 5–7, homework/relapse-prevention→later weeks) — including them here would duplicate nearly the whole course |
| U24 | Sally's formal case introduction (18-year-old, single, college, moderate MDD per DSM-IV-TR) | p.7 | Deferred | Belongs wherever Sally is first canonically introduced platform-wide (Week 3 territory), not Week 1 |
| U25 | What a therapy session looks like — general structure + a brief Sally studying-problem example | pp.11–12 | Deferred | Weeks 5–7's structured-session territory |
| U26 | Developing as a CBT therapist — 3 skill stages + a "Mike" agenda-setting role-play dialogue | pp.12–13 | Excluded | Therapist-training content — outside this platform's psychoeducation mission; also a dialogue demonstration |
| U27 | "How to use this book" — apply techniques to yourself while reading, self-monitor mood/automatic thoughts, coping-card example, chapter roadmap | pp.13–16 | Excluded | Meta content about the book itself; the self-monitoring advice is self-application instruction, inconsistent with this platform's non-self-guided theory/history weeks |

## §2 — Current-page diagnosis

- **Correct, should remain:** the "Накратко" framing (accurate to the source's "not a
  pre-finished theory" arc); the old-vs-new research-question comparison table (§04 — accurately
  reflects the chapter's actual reframing, no invented content); the automatic-thoughts preview
  that links out to Week 8 instead of re-explaining (correct duplication discipline); the 1979
  citation (§07, accurate); the comprehension check (accurate, if thin); the "psychoanalysis
  wasn't defeated" reconciliation already baked into the page's framing (matches the chapter's
  own even-handed tone).
- **Missing (available, safe, high-value):** the chapter's actual empirical starting point — the
  dream study and its surprising result (U14–U16) — and the "two streams of thinking" discovery
  with Beck's own illustrative patient anecdote (U17–U18) are the chapter's real "birth" narrative
  and are currently **entirely absent**. Both the `HistoricalTimeline` (6 milestones) and
  `ResearchTurnStepper` (4 steps) presently only paraphrase this arc in the abstract ("hypothesis
  → test → unexpected result → reformulation") without ever citing what the hypothesis, the test,
  or the surprise actually *were*. Dr. Rush, the 1977 RCT, and the imipramine comparison (U19–U20)
  are also missing — without them, the existing 1979 manual citation has no lead-up. The anxiety-
  disorder extension (U22) is a safe, concrete growth-of-the-model fact currently unmentioned.
- **Unsupported or over-generalized:** §06 "Защо протоколите и наръчниците са важни" (manuals'
  "four roles") is **not directly traceable to Chapter 1's text** as a discrete framework — the
  chapter states the 1979 work was the *first* treatment manual and connects manualization to
  making the RCT comparison possible, but never presents a generic "four roles of manuals"
  argument. This section reads as pedagogical scaffolding invented for the page rather than a
  source-grounded claim — flagged as a genuine finding requiring an owner decision (§4).
- **Overlaps Week 3 / Week 8 / later weeks — correctly avoided today, must stay avoided:** the
  cognitive-model architecture (U9–U10, Week 3's), automatic thoughts in depth (already just
  previewed + linked — correct), the 10 principles (U23, spans nearly every future week), Sally's
  formal case introduction (U24), session structure (U25, Weeks 5–7).
- **Out of platform scope entirely:** therapist-skill-development content (U26) and "how to use
  this book" self-monitoring advice (U27) — neither fits a psychoeducation-for-learners platform;
  U27 in particular reads as self-application instruction, which this page's own established
  register avoids.

## §3 — Terminology Map

- "Когнитивна терапия" vs. "когнитивно-поведенческа терапия" — the chapter itself notes Beck's
  original term was "cognitive therapy," now used interchangeably with "CBT" by most of the field.
  Current page already uses "когнитивна терапия" for the historical narrative and "когнитивно-
  поведенческа терапия" for the modern/general term — this distinction is source-accurate and
  should be preserved, now with an explicit one-line basis (U4) instead of being implicit.
- "Автоматична мисъл" — already canonical platform-wide (Week 8); U18's origin story uses the same
  term, no drift.
- "Основно вярване" — already canonical (Week 3/12); U10 stays Deferred specifically so this term
  is never introduced here ahead of Week 3.
- No new terminology conflicts found.

## §4 — Representation Fit (evaluated, not assumed)

- **Weekly Mind Map / historical visualization:** the existing `HistoricalTimeline` component
  already *is* the right-fit representation for Week 1's content — a linear historical sequence,
  not a concept hierarchy. **Recommendation: enrich, don't replace.** Update its 6 milestone
  descriptions with the real specifics (dream study, two streams, Rush/1977/imipramine, anxiety
  extension) instead of introducing a `ConceptGraph`/Mind Map, which would misrepresent a causal
  timeline as a concept network.
- **Process/comparison representations:** `ResearchTurnStepper` (4-step causal loop) and the §04
  comparison table are both correct fits already in place. **Recommendation:** enrich
  `ResearchTurnStepper`'s 4 step explanations with the real facts (still 4 steps — no new
  component, no new interactivity) rather than adding a third visualization.
- **New interactive component:** **not recommended.** Two lightweight, already-approved
  interactive elements (stepper + timeline) are proportionate to a "Theory and History" week;
  adding a third, or upgrading either to Week 6/7/9-style multi-branch simulation, would be
  interactivity not justified by the source's own narrative shape (a simple sequence of events,
  not a decision tree).
- **Retrieval practice:** the existing 3-question check is thin relative to the richer content
  becoming available; 1 new question testing the dream-study/two-streams narrative would be a
  proportionate, non-structural extension (same component, no new representation) — matching the
  precedent set in Weeks 10 and 12.

## §5 — Owner decisions required before implementation

1. Approve the §1 KU accounting (15 Included / 1 Needs Review / 8 Deferred / 3 Excluded).
2. **U7** — include the influences/related-therapies name lists as one short sentence, or omit
   entirely?
3. **§06 finding** — the existing "four roles of manuals" section is not directly source-grounded.
   Re-ground it using the concrete 1979-manual-enabled-the-RCT-comparison fact (U20/U21), or leave
   the existing generic framing as reasonable general academic context, or cut the section?
4. Approve enriching `HistoricalTimeline`'s 6 milestones with the real dream-study/two-streams/
   Rush-1977-imipramine/anxiety-extension specifics (same 6-milestone structure, richer text)?
5. Approve enriching `ResearchTurnStepper`'s 4 step explanations the same way (same 4 steps,
   richer text — no new component)?
6. Approve one new comprehension question testing the dream-study/two-streams narrative?
7. Confirm the 8 Deferred KUs stay out of Week 1 even though the retrofit is otherwise expanding
   the page (no scope creep into Week 3/8/later-week territory).
8. Confirm the 3 Excluded KUs (Table 1.1's full list, therapist-development stages, "how to use
   this book") stay fully out.
