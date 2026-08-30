# Week 9 — Source-First Audit v1 ("Когнитивни изкривявания и дневник на мислите")

*Implemented — `/kurs/sedmica-9` (`Sedmica9.razor`). Not committed. Written per the Deep Learning
workflow. Owner approved all §7/§8 decisions from the original audit; final post-implementation
accounting is §0 below.*

## 0. Final accounting (post-implementation)

54 of the original 76 KUs were re-scoped once actually written, per the owner's explicit direction
to keep evaluation-question material at recap depth and not extend Сали with new biography:

- **39 Included** — every generic/structural KU, the twelve distortions (Figure 11.2, verbatim),
  the Thought Record's six-column structure, John's five-reasons-evaluation-fails section (source-
  named, previously unused on this platform), the "when true" strategies (the money-worry example
  kept but de-personalized — no name attached, matching "preserve a genuinely unique KU only if
  necessary, invent no history"), and the full Figure 12.2 worksheet demonstration (the source's own
  already-generic first-person example — no character attached at all).
- **3 Deferred** — core beliefs (Ch.14, future week), AWARE technique, relaxation/distraction
  (Ch.15) — all explicitly out of this week's two-part scope.
- **34 Excluded** — the Karen-specific evaluation-question walkthrough (7 KUs, superseded once
  evaluation questions moved to recap depth — a worked per-category walkthrough no longer fits that
  scope, independent of the case-history question), the death-fear decatastrophizing guidance (owner
  decision 3), Сали's self-labeling moment and the therapy-notes cluster (11 KUs — reviewing/reading
  notes, audio recording, her specific reminder notes — judged non-essential to the distortions +
  Thought Record core once the page was actually drafted; the Thought Record concept itself is fully
  covered by the structure map and the worksheet demo without this administrative layer), Боб's full
  narrative Thought Record walkthrough (structural-only per owner decision 4), the ACT/Hayes external
  citation, and several minor teaching-mechanics points that belong to the now-recap-depth
  evaluation-questions cluster.
- **0 Needs Review, 0 Unaccounted.**

**A build-time bug found and fixed during implementation, not a content issue**: the Thought Record
structure map's position dictionary was declared *after* the field that consumed it — the exact
static-field-ordering trap this project's own code comments already warn about (Week 3/7 precedent).
Caught only by the required real-browser QA step (the map rendered with zero nodes), not by the
project's string-based tests — fixed by reordering the declarations. A second, geometry-only issue
surfaced on the same map after that fix: an early column layout produced two long cross-column arcs
whose endpoints visually clustered together, reading as a false direct connection between two
unrelated nodes — reworked to a layout where every edge is a short same-row or same-column curve,
verified clean in a fresh screenshot.

## 1. Scope confirmed

- **CourseCatalog Week 9** (`CourseCatalog.cs`): module "Когнитивни инструменти и
  преструктуриране", title "Когнитивни изкривявания и дневник на мислите",
  `CurriculumSafetyLevel.PublicWithAdaptation`, `route: null`. Two placeholder objectives; planned
  format `Simulator` (pre-Deep-Learning placeholder, re-evaluated below).
- **Source**: SRC-041, **Chapter 11 "Оценяване на автоматични мисли"** (printed pp. 167–186) +
  **Chapter 12 "Отговаряне на автоматични мисли"** (printed pp. 187–197) — both read in full this
  session (extracted together as one file, `SRC-041_ch11_bg_extracted.txt`, since Ch.12 starts on
  printed p.187, inside the page range I requested for "Ch.11"; a second, separately-extracted file
  labeled `..._ch12_...txt` is actually **Chapter 13** — mislabeled during extraction, not read,
  irrelevant to this audit, flagged so a future session doesn't mistake it for Ch.12).
- **Why these two chapters, not one**: Week 9's own title has two parts. Ch.11 is where the
  classic 12-item cognitive-distortions list (Figure 11.2) lives. Ch.12 is where the structured
  Thought Record (Figure 12.1) and its simpler alternative (Figure 12.2) live. Together they're an
  exact match to "cognitive distortions" + "thought journal" — no other chapter covers either topic.
- **Adjacent weeks inspected for continuity/overlap only, never as CBT source**: Week 7 (Сали
  continuity — Alison/Joe already established), Week 8 (automatic-thought/emotion model, already
  routed, Ch.9/10 sourced), **Week 10 (significant thematic overlap risk — see §4)**.
- **Governance consulted**: `07_CONTENT_GOVERNANCE.md`, `23_CLINICAL_SAFETY_BOUNDARIES.md`,
  `06_QA_STRATEGY.md` (DoD v3), `11_SOURCE_REGISTER.md`, `19_MVP_SCOPE.md` /
  `01_MASTER_PLAN.md` / `18_INFORMATION_ARCHITECTURE.md` / `14_EXISTING_PROTOTYPE_AUDIT.md` (the
  **pre-Deep-Learning MVP's "Thought Record" flagship-exercise plan** — see §5, a real prior
  planning artifact, not itself a source of CBT content).

## 2. Knowledge Unit inventory (76 units)

**Chapter 11 — Evaluating Automatic Thoughts** (U01–U54): selecting which automatic thoughts are
worth evaluating (U01–U07); why therapists rarely challenge a thought directly, and the
"grain-of-truth" principle (U08–U09); Figure 11.1's six Socratic evaluation-question categories —
evidence, alternative explanation, decatastrophizing, impact-of-belief, distancing, problem-solving
(U10–U11), each demonstrated on Sally's "Karen doesn't care about me" thought (U12–U19); evaluating
the outcome (U20); five reasons an evaluation attempt fails, each with a worked example — three
using a separate, source-named patient "John" (U21–U26); varying the questions (U27–U28);
identifying and self-labeling cognitive distortions, worked on Sally's own patterns (U29–U32);
therapist self-disclosure (U33); **Figure 11.2 — the twelve classic cognitive distortions**, each
with definition + example (U34–U45); when automatic thoughts are true — problem-solving,
invalid-conclusion, and acceptance strategies (U46–U49); teaching patients to self-evaluate
(U50–U53); the "shortcut" (skip straight to an adaptive response) for advanced patients (U54).

**Chapter 12 — Responding to Automatic Thoughts** (U55–U76): recording responses to
already-evaluated thoughts, written or audio (U55–U58); reviewing/prompting a therapy-note summary
(U59–U62); Sally's own therapy notes, including a direct callback to Week 7's behavioral-activation
content and her established Alison/Joe friendship (U63–U65); practical note-keeping (U66);
**Figure 12.1 — the Thought Record**, a 6-column structured worksheet (situation / automatic thought
+ belief% / emotion + intensity% / distortion / response / outcome), fully demonstrated column-by-
column on Sally's "Bob doesn't want to come with me" thought (U67–U70); **Figure 12.2 — "Test Your
Thoughts" Worksheet**, explicitly presented in the source as a simpler, plain-language alternative
for patients who'd find the full Thought Record too complex, with a full worked example (U71–U72);
when a worksheet isn't helping (U73); recap cross-reference to Ch.11's five failure-reasons (U74);
AWARE technique for anxious/obsessive thoughts (U75); distraction/relaxation for very high emotion,
cross-referencing Ch.15 (U76).

## 3. Coverage Matrix — pre-implementation draft (superseded by §0's final accounting)

*This table is the original first-pass disposition, kept for the audit trail. §0 above has the
final, owner-approved, mutually-exclusive accounting actually implemented — read that one first.*

| Disposition | Count | Notes |
|---|---:|---|
| Included (draft) | 62 | Superseded — see §0 |
| Needs Review — Sally case-history extension (draft) | 9 | Resolved this round — Excluded (Karen walkthrough, superseded by recap-depth scoping) or Included-genericized (money-worry) or Excluded (Боб) — see §0 |
| Needs Review — safety-sensitivity (draft) | 1 | Resolved — Excluded per owner decision 3 |
| Deferred (draft) | 3 | Unchanged — see §0 |
| Excluded (draft) | 1 | Expanded — see §0 |
| Unaccounted | **0** | — |
| Total | **76** | |

## 4. Genuine scoping question: overlap with Week 10

Week 10 ("Сократически въпроси и съвместно изследване") already has sections named "Четири
семейства въпроси," "Факти, предположения и заключения," "От „ужасно" към конкретно," and
"Балансиран отговор" — these read as a looser treatment of the *same* evaluation-question territory
Ch.11's Figure 11.1 formalizes (evidence, alternative explanation, decatastrophizing, balanced/
adaptive response). Week 10 was built before the Deep Learning workflow existed, so it carries no
exact chapter citation to check against — I can't yet confirm precisely how much it overlaps.

This matters for scope, not safety: a Thought Record's "Response" column is meaningless without
*some* explanation of what goes in it, so Week 9 can't omit the evaluation-question concept
entirely — but re-teaching it in full would duplicate Week 10. My recommendation (not yet acted on):
keep Figure 11.1's six categories at recap depth in Week 9 (one line each, cross-linking to Week 10
for the full treatment), and spend Week 9's real depth on the two things Week 10 does **not** cover
at all — the distortions taxonomy and the Thought Record structure itself. This is presented as a
recommendation, not a decision — genuine owner call in §7.

## 5. The MVP Thought Record requirement — resolved

Several pre-Deep-Learning planning documents (`01_MASTER_PLAN.md`, `19_MVP_SCOPE.md`,
`18_INFORMATION_ARCHITECTURE.md`, `14_EXISTING_PROTOTYPE_AUDIT.md`) name a "simplified Thought
Record" as the platform's original flagship interactive exercise — a fillable, multi-field form
(situation / thought+belief% / emotion+intensity% / distortion / response / outcome), locally
stored, editable/deletable, exportable. That plan predates both the Deep Learning workflow and the
platform's now-consistent safety pattern (no `<input>` fields, no self-scoring, no belief/emotion
percentage sliders anywhere in Weeks 1–8/10/12 — every "exercise" is a fixed, third-person worked
demonstration, never a tool that collects the *reader's own* clinical-shaped data).

Now that the actual source is read in full, the resolution is clear and a genuine improvement on the
old plan rather than a rejection of it: **Figure 12.2 ("Test Your Thoughts" Worksheet) is explicitly
presented in SRC-041 itself as the simpler, patient-facing alternative** to the full clinical
Thought Record — plain language, no percentage ratings, easier to complete. That is a *source-
provided* answer to exactly the question the old MVP plan was trying to solve on its own. Recommendation: build Week 9's representation around a **fixed, worked demonstration** of this
simpler worksheet (the source's own Joan example, or a Sally-continuity example if approved) —
teaching the *structure* and *why* it helps, not a live form inviting the reader to type their own
automatic thoughts into a browser. This keeps the platform's established third-person/observational
framing intact and matches `PublicWithAdaptation` exactly as Week 6/7 already established it.

## 6. Terminology Map (reuse existing terms, flag reconciliation points)

| Term | Status |
|---|---|
| Автоматична мисъл | Established (Weeks 1/3/6/7/8/10) — reuse verbatim |
| Когнитивно изкривяване / грешка в мисленето | New — first formal 12-item taxonomy on the platform |
| Запис на мислите (Thought Record) | New |
| Работен лист „Тест на мислите ви" | New — the recommended learner-facing representation (§5) |
| Балансиран отговор (Week 10) vs. Адаптивен отговор (Ch.11/12's own wording) | **Reconciliation needed** — same concept, different label; pick one at implementation time |
| Декатастрофизиране, дистанциране, доказателства „за/против" | Likely already used loosely in Week 10 — verify exact wording before Week 9 reuses it, so the two weeks read as one consistent vocabulary, not two dialects |

## 7. Representation Fit

- **Weekly Mind Map** — Yes, standard pattern. Natural clusters: защо не оспорваме директно
  (indirect-challenge rationale) · оценъчни въпроси (brief, cross-linked to Week 10) · 12-те
  когнитивни изкривявания · запис на мислите · по-опростен работен лист · кога мислите са верни.
- **Cognitive-distortion recognition representation** — Yes, strongly justified, not decorative:
  Figure 11.2's 12 distortions (definition + example each) map directly onto Week 9's own stated
  objective ("recognize a few typical distortions") and are completely unclaimed by any other week.
  Recommend a recognition-practice grid reusing the existing `.learning-grid`/`.progressive-
  explanation` card pattern (match-the-thought-to-its-distortion), zero new component — same
  technique Week 6 already uses for its terminology cards.
- **Thought Record interaction** — Contingent on §5's resolution: a fixed, worked, non-fillable
  demonstration (predict/reveal-style disclosure, reusing Week 7's lightweight pattern), not a live
  form. Recommend showing the simpler Figure 12.2 worksheet as the primary demonstration, with the
  full Figure 12.1 Thought Record shown as "what a therapist-guided version also looks like" —
  observational, not something the reader is invited to fill in about themselves.
- **Case/application exercise** — Yes, reusing Week 7's established "Приложение" pattern (short
  fixed scenarios, apply a distortion label / identify a Thought Record column) — contingent on the
  Sally-extension decision in §8; John's examples are available as a Sally-independent fallback.

## 8. Retrieval/application opportunities (DoD v3 taxonomy)

- **Recognition**: match a stated automatic thought to its distortion category.
- **Retrieval**: recall the Thought Record's six columns in order.
- **Application**: given a fixed scenario, identify the likely distortion and what a "Response"
  entry might address.
- **Reasoning**: explain why therapists rarely challenge a thought directly (the three-part
  rationale, U08), and why the "grain of truth" principle matters even for a distorted thought.

## 9. Genuine owner decisions required before implementation

1. **Week 9 / Week 10 overlap (§4)** — keep the evaluation-question categories at recap depth in
   Week 9, cross-linking to Week 10 for the full treatment (recommended), or something else?
2. **Sally extension (§3, 9 KUs)** — approve using her two new source-grounded facts (a friend
   "Карън," running short on money) and one new person ("Боб," from the full Thought Record
   walkthrough) — same disposition class already approved for Week 7 — or keep Week 9 entirely on
   John's examples instead (source-named, unclaimed, zero case-history risk)?
3. **Death-fear decatastrophizing guidance (U15)** — include with explicit safety framing (matching
   Week 6's precedent for risk-adjacent content), or exclude as unnecessary for this week's scope?
4. **Thought Record representation (§5, §7)** — confirm the fixed-worked-demonstration approach
   (recommended, resolves the old MVP plan safely) over building any form of fillable input.
5. **AWARE technique / relaxation cross-references (U75–U76)** — confirm Deferred (out of this
   week's two-part scope) rather than included as a "beyond the basics" section.
