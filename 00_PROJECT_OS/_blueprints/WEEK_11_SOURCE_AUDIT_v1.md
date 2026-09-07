# WEEK 11 — SOURCE-FIRST AUDIT (v1, superseded status below)

**Status: `OWNER-APPROVED, IMPLEMENTED`** (2026-09-07). The 5 owner decisions in §9 were approved and
applied; see §11 for the resolution log and the final, mutually exclusive KU accounting. Implementation
lives in `Sedmica11.razor` / `Week11ContentSliceTests.cs`. Not committed/pushed yet. Week 4 and Week 5
untouched throughout.

## 1. Canonical Week 11 (from `Curriculum/CourseCatalog.cs`, not memory)

| Field | Value |
|---|---|
| Number / Module | 11 / Модул 3 "Когнитивни инструменти и преструктуриране" (Седмици 8–12) |
| Title | **"Междинни вярвания"** (Intermediate Beliefs) |
| ShortSummary | "Нагласи и правила, които стоят зад повтарящи се автоматични мисли — представени образователно." |
| Learning role | Deepens the cognitive hierarchy (automatic thought → intermediate belief → core belief) already introduced in Week 3, focused specifically on the middle layer |
| SafetyLevel | `CurriculumSafetyLevel.ProfessionalReviewRequired` — one tier stricter than Week 4/12's `AcademicContextOnly` |
| Route | `null` (not yet built) |
| Objectives (as recorded) | (1) "Разбирате какво е междинно вярване (нагласа/правило)." (2) "Виждате връзката между междинни вярвания и повтарящи се автоматични мисли." |
| Format | `[InteractiveFormat.InteractiveModel]` |
| Roadmap cross-check | `24_IMPLEMENTATION_ROADMAP.md` Simulator Opportunity Matrix, row "11": *"Rules/attitudes/assumptions explorer (**без downward-arrow tool**) — INTERACTIVE MODEL — PROFESSIONAL REVIEW REQUIRED — P2 — CATALOGUED, NOT BUILT"*. Confirms both the format and the explicit safety exclusion. |
| Public status consequence | Per `CurriculumLabels.DeriveStatus`, `ProfessionalReviewRequired` always resolves to `CourseWeekStatus.ProfessionalReviewRequired` ("Изисква професионален преглед"), **never** `Available`, regardless of implementation quality — same tier as still-unrouted Week 14. This is architectural, not a defect to fix during implementation. |

No existing `WEEK_11_*` blueprint predates this file — this is the first audit for this week.

## 2. Exact source scope

**SRC-041** (Judith S. Beck, *Cognitive Behavior Therapy: Basics and Beyond*, 2nd ed., Guilford Press 2011 — the project's Primary Source per `11_SOURCE_REGISTER.md`), **Chapter 13**, printed pages **198–227** (PDF pages 220–249 in the local Bulgarian-translation PDF, per the calibrated +22 offset in `_source_corpus/README_EXTRACTION_METHOD.md`).

- Chapter title (per the book's own printed table of contents, PDF pp. 17–21): **"Идентифициране и модифициране на междинни вярвания"**. The chapter's own OCR'd title-page heading misreads as "...ПРИМЕРНИ ВЯРВАНИЯ" (stylized display font) — a scanning artifact, not a real title discrepancy; every in-body running header and the TOC agree on "междинни."
- Newly extracted this session: `_source_corpus/SRC-041_ch13_bg_extracted.txt` (62,792 characters, same `pypdf` method as Chapters 1–6/9/10/12/14), read in full (all ~1,138 extracted lines).
- Two in-chapter figures (13.1's blank Cognitive Conceptualization Diagram template, and its labels) extract as flattened, partially garbled OCR text — same positional caveat already on record for Chapters 3/9/10's figures. Figures 13.2–13.5 (populated with Sally's data / the strategy list / the belief-pair table) are legible.
- Confirmed against the extraction README's chapter table: Ch.12 ends at printed p.197, Ch.14 starts at printed p.228 — page boundaries are exact, no bleed from neighboring chapters.

## 3. Full KU inventory + Coverage Matrix (100% accounted, Unaccounted = 0)

40 Knowledge Units, all statused. None invented — every KU below traces to a specific passage in the chapter.

| # | KU (compressed) | Source locator | Status | Destination / reason |
|---|---|---|---|---|
| U01 | Two-category recap: intermediate beliefs (rules/attitudes/assumptions) vs. core beliefs (rigid/global), originally from Ch.3 | p.198 | Deferred | Already Included on Week 3 (locked) |
| U02 | Intermediate beliefs are more flexible than core beliefs, less easily modified than automatic thoughts | p.198 | Included | New nuance, not on Week 3 |
| U03 | Cognitive Conceptualization Diagram — purpose: links automatic thoughts to intermediate/core beliefs, organizes case data | p.199 | Included | New — the diagram-as-tool, distinct from Week 3's already-populated resulting hierarchy |
| U04 | When/how to start the diagram — after session 1 if data exists; provisional entries marked "?"; refined over 3–4 sessions | p.199 | Included | New (methodology) |
| U05 | Usually not shown as a literal worksheet to the patient; shared verbally/simplified; full picture timed for patient readiness | p.199 | Included | New |
| U06 | Bottom-half procedure: 3 situations × automatic thought/meaning/emotion/behavior, linked to core belief | pp.199–201 | Deferred | Week 3's flow diagram + Sally concept map already demonstrate this exact chain |
| U07 | Top-half procedure: childhood-origin question categories (family conflict, divorce, critical/devaluing interactions, illness, loss, abuse, adverse conditions, subtler comparisons/favoritism) | pp.199–200 | **Excluded** (resolved) | Owner-approved scope narrowing (§11, Decision #1): a clinician-training checklist of trauma-adjacent categories beyond the narrowed net-new objectives; Sally's own version stays Included on Week 3 only |
| U08 | Sally's Fig.13.2 diagram data (critical mother, older-brother/peer comparison, core belief, assumption pair, 5 coping strategies) | pp.201–202 | Deferred | Developmental origin + core belief + assumption already Included on Week 3; only the coping-strategy list is net-new (→ U11) |
| U09 | Fig.13.3 — hierarchy diagram of Sally's belief structure | p.203 | Deferred | Week 3's flow diagram/concept map already render this exact hierarchy |
| U10 | "Positive" vs. "negative" side of one conditional assumption; most Axis-I patients act on the positive side until stressed; "positive" ≠ adaptive | pp.202–203 | Included | New nuance |
| U11 | Coping-strategies concept + Fig.13.4's paired-opposite-strategy list (9 pairs) | pp.203–205 | Included | Genuinely new concept, not covered by any other week |
| U12 | Coping-strategy "if/then" formula (engage strategy → belief may not come true; don't engage → belief likely confirmed) | p.203 | Included | New, compact |
| U13 | Conceptualization is a standing hypothesis — continuously reassessed, "complete" only at termination, shared as hypothesis ("does this sound right?") | pp.204–205 | Deferred | Already Included/locked on Week 4 (its closing statement: "conceptualization is revised throughout therapy") |
| U14 | Overview list — 7 ways to identify intermediate/core beliefs | pp.205–206 | Included | Academic list, no self-tool |
| U15 | Method 1 — belief voiced directly as an automatic thought | p.205 | Included | Brief |
| U16 | Method 2 — therapist supplies first half of a conditional, patient completes it | pp.205–206 | Included | Brief, paraphrased |
| U17 | Method 3 — direct elicitation of a rule/attitude via importance/standards questions | p.206 | Included | Brief |
| U18 | Method 4 — Downward Arrow Technique (Burns, 1980): named + cited only, no procedure/rephrasing prompts/stop condition | pp.206–208 | **Included** (resolved, narrowed) | Owner-approved (§11, Decision #2): name/attribution only, folded into the U14 method list — all procedural depth dropped |
| U19 | Method 5 — scanning automatic thoughts across situations for a common theme | pp.208–209 | Included | Brief |
| U20 | Method 6 — asking the patient directly | p.209 | Included | Brief |
| U21 | Method 7 — standardized belief questionnaires (Dysfunctional Attitude Scale — Weissman & Beck 1978; Personality Belief Questionnaire — A.T. Beck & Beck 1991) | p.209 | Excluded | Named clinical instruments; no educational value at this course's depth, safety-sensitive if implied self-assessable |
| U22 | Central-vs-peripheral judgment — focus on important/strongly-held beliefs (Safran, Vallis, Segal & Shaw 1986) | pp.209–210 | Included | Academic principle |
| U23 | Judgment checklist (what/how strong/how broad/timing/session time) | pp.209–210 | Included | Academic checklist |
| U24 | Sequencing principle — modify intermediate beliefs before core beliefs; very rigid beliefs may need automatic-thought practice first | p.210 | Included | Own connective content; forward-links to Ch.14/Week 12 |
| U25 | Educating patients that beliefs are learned ideas (not innate truths), illustrated via a differing-belief comparison (Sally/cousin "Emily") | pp.210–211 | Included (flag) | Paraphrased, not verbatim; introduces a new named secondary figure — Owner Decision #3 |
| U26 | Reframing a rule/attitude into assumption form before testing it (uses downward arrow) | p.211 | **Excluded** (resolved) | Owner-approved (§11, Decision #2): the mechanism is inseparable from the barred downward-arrow procedure — not viable to teach without it |
| U27 | Advantages/disadvantages examination of a belief (mirrors automatic-thought usefulness technique) | pp.211–212 | Included | Brief, paraphrased |
| U28 | Formulating a new, more adaptive belief — clinician-prepared, collaboratively co-constructed | pp.212–213 | Included | — |
| U29 | Fig.13.5 — Sally's 7 old-belief → more-functional-belief pairs | p.213 | Included | Strong comparison-table candidate, fully source-grounded |
| U30 | Belief-strength rating practice (0–100%, "intellectual" vs. "gut" level; ~30% = "weakened enough") | pp.213–214 | Included | Academic |
| U31 | Recommended patient-notes format (old belief / new belief / % each) | p.214 | Included | Descriptive only, not built as a tracking tool (safety) |
| U32 | Named list of 7 modification techniques | p.214 | Included | Natural backbone for the page's "InteractiveModel" format |
| U33 | Technique 1 — Socratic Questioning for beliefs (same question repertoire as automatic thoughts, applied concretely) | pp.215–217 | Included | Cross-references Week 10, does not re-teach its 6 categories |
| U34 | Technique 2 — Behavioral Experiments to test a belief | pp.217–218 | Included | Paraphrased, no verbatim transcript |
| U35 | Technique 3 — Cognitive Continuum (numeric scale against all-or-nothing beliefs) | pp.218–220 | Included | Good visual candidate (labeled number line) |
| U36 | Technique 4 — Intellectual-Emotional Role Play / "point-counterpoint" (Young, 1999) | pp.220–222 | Included | Description only, no verbatim script |
| U37 | Technique 5 — Using Others as a Reference Point (4 approaches: comparison figure, shared-belief friend, persuade-a-third-party role play, hypothetical child) | pp.222–226 | Included (flag) | Introduces 2 more new named secondary figures ("Emily" reused, "Rebecca") — same Owner Decision #3 |
| U38 | Technique 6 — Acting "As If" (belief/behavior positive-spiral) | pp.226–227 | Included | — |
| U39 | Technique 7 — Self-Disclosure (genuine, relevant therapist disclosure) | p.227 | Included | Generic academic mention only, no fabricated anecdote |
| U40 | Chapter closing summary (recaps both lists; notes techniques double for core beliefs — forward link to Ch.14) | p.227 | Included | Natural page-closing recap |

**Pre-resolution totals (as first audited): 40 KUs — 31 Included / 5 Deferred / 1 Excluded / 3 Needs
Review / 0 Unaccounted.** See §11 for the final, mutually exclusive accounting after the owner resolved
the 3 Needs Review items: **40 KUs — 32 Included / 5 Deferred / 3 Excluded / 0 Needs Review / 0
Unaccounted.**

## 4. Overlap audit against the declared ownership map

| Week | Declared ownership | Result |
|---|---|---|
| **Week 3** (LOCKED) | Foundational cognitive architecture | **Confirmed owns**, and already contains more Sally-specific Ch.13 material than expected: the 3-level hierarchy, the Attitude/Rule/Assumption 3-way split (with the book's own "Reader E" examples), Sally's core belief ("некомпетентна"), Sally's *specific* assumption ("Ако работя усилено, мога да преодолея недостатъците си" — the same assumption as Fig.13.2/13.3), and Sally's developmental origin (critical mother, comparison to older brother). Verified directly in `Sedmica3.razor` (lines 183–184, 213–227, 511, 580–583). This is why U01/U06/U08/U09/U13 Defer there — building them again on Week 11 would be redundant, not new coverage. |
| **Week 8** | Automatic thoughts/emotions | No overlap found — Ch.13 only references automatic thoughts as the *starting point* for belief work, never re-teaches identification/emotion content. |
| **Week 9** | Distortions + Thought Record depth | No overlap — Ch.13's advantages/disadvantages technique (U27) cross-references an *earlier* chapter's automatic-thought usefulness discussion (pp.174–175, outside this chapter's scope and outside Week 9's own Ch.11–12 scope too), not Week 9's distortion/Thought Record material. |
| **Week 10** | Collaborative/Socratic evaluation | Confirmed owns the question-category teaching; Ch.13's U33 explicitly says it reuses "the same kinds of questions" from automatic-thought evaluation — Week 11 should *cite* that connection, not re-teach the six categories. |
| **Week 12** | Core beliefs/schemas academic depth | Confirmed owns Ch.14; Ch.13 itself explicitly defers core-belief-specific technique depth to "the next chapter" (U24, U40) — clean chapter boundary, no content bleed either direction. |

**Net finding:** Week 11's two stated objectives are *largely already satisfied* by Week 3's existing locked content. The genuinely new material this chapter offers is narrower than a first read suggests — see Owner Decision #1.

## 5. Terminology Map

**Carried forward, locked — do not redefine:** "междинно вярване", "нагласа", "правило", "предположение", "основно вярване", "автоматична мисъл", "схема" (Week 3); "сократически въпроси" (Week 10).

**New to Week 11 (needs a single locked Bulgarian rendering if Included):** "диаграма за когнитивна концептуализация" (Cognitive Conceptualization Diagram); "стратегия за справяне" (coping strategy); "когнитивен континуум" (Cognitive Continuum); "интелектуално-емоционална ролева игра" / "точка-контрапункт" (Young, 1999); "действие „като че ли"" (Acting As If); "саморазкритие" (Self-Disclosure). The extraction shows two inconsistent OCR renderings of the same technique — "техника на низходящата стрела" and "техника на стрелката надолу" — if U18/U26 are Included at all, one must be picked and locked (recommend "техника на низходящата стрела", matching the more common published Bulgarian rendering of Burns' "downward arrow").

## 6. Representation Fit (no component forced)

| Representation | Recommendation | Why |
|---|---|---|
| Weekly Mind Map | **No** | The hierarchy it would show already lives on Week 3; a second Mind Map would duplicate, not add — a simple cross-link back to Week 3 instead |
| Concept Map | **No** | No new multi-parent relation set distinct from Week 3's existing Sally map |
| Process visualization | **Yes** | A 3-step "identify → decide whether to modify → modify" sequence (reusing an existing numbered-sequence pattern) |
| Comparison | **Yes, twice** | Fig.13.4 coping-strategy pairs; Fig.13.5 old-belief vs. new-belief pairs (reusing `.comparison-matrix`/`.category-compare`) |
| Worked example | **Yes, Sally continued** | Limited to the genuinely new facts (coping strategies, before/after belief pairs) — explicitly not re-deriving the hierarchy/origin Week 3 already owns |
| Final assessment | **Yes** | Per the now-`OWNER APPROVED` Weekly Final Assessment Standard — a `FinalAssessment` block drawn from the Included KUs |

## 7. Retrieval/application opportunities (safety-bounded)

- "Which identification method does this short excerpt illustrate?" — receptive classification over methods 1/2/3/5/6 (never the downward arrow as something to *try*).
- Match each of the 7 modification-technique names to its one-line definition.
- Reconstruct the 3-step identify→decide→modify sequence (same pattern as Week 6 Review's reconstruct-the-order check).
- **Explicitly excluded by design:** any exercise asking the learner to identify or evaluate their *own* beliefs — that would cross the `ProfessionalReviewRequired` boundary the catalog already drew.

## 8. Source-grounded case/example opportunities

Continue Sally (established character) using only the chapter's own confirmed facts not already used elsewhere: the 5 coping strategies (Fig.13.4) and the 7 old→new belief pairs (Fig.13.5). Three secondary figures appear in the book's own worked examples adjacent to Sally (cousin "Emily", friend "Rebecca", an unnamed roommate/hypothetical child) — see Owner Decision #3 below before adopting any of them.

## 9. Genuine owner decisions required before implementation

1. **Scope confirmation.** 5 of 40 KUs (the hierarchy, Sally's specific assumption, and her developmental origin) already Defer to Week 3 — Week 11's real net-new content is the Cognitive Conceptualization Diagram as a tool, coping strategies, the 7 identification methods (minus the arrow), the modify-or-not judgment framework, the 7 modification techniques, and Fig.13.5. Proceed on that narrower, still-legitimate scope (31 Included KUs), or revise the CourseCatalog objectives first?
2. **Downward Arrow Technique (U18/U26).** The roadmap explicitly excludes it as a self-guided tool. Academic mention only (name/attribution, never an interactive "try it on yourself" widget), or omit entirely?
3. **New secondary figures (U25/U37).** The source's own worked examples introduce "Emily" and "Rebecca" alongside Sally. Adopt them (paraphrased, not verbatim) as part of Sally's world, or generalize/anonymize instead, per the "never add unapproved history to an established character" rule?
4. **U21 (belief questionnaires).** Recommended Excluded as named clinical instruments — confirm.
5. **Public status.** Even a fully-built Week 11 will show "Изисква професионален преглед" (never "Налично") per the existing `DeriveStatus` architecture — confirm this is the intended launch state, not a defect to work around.

## 10. Proposed structure (as implemented)

01 Накратко (orientation + Mind Map preview) · 02 Връзка със Седмица 3 (recap + cross-link only) ·
03 Диаграмата за когнитивна концептуализация · 04 Стратегии за справяне (comparison table) ·
05 Методи за идентифициране (+ 3-step process visual; arrow named only) · 06 Трябва ли вярването да
се модифицира? · 07 Методи за модифициране на вярвания (7-technique table) · 08 Сали — преди/след
вярване (Fig.13.5 comparison table) · 09 Седмичен преглед (Mind Map review, collapsed) · 10 Финална
проверка (shared `FinalAssessment`, 8 questions) · 11 Извори.

## 11. Owner decisions — resolved (2026-09-07) and final accounting

The owner approved all 5 decisions from §9 and authorized implementation:

1. **Scope** — proceed on the narrower net-new scope. Confirmed; the page covers only the Cognitive
   Conceptualization Diagram as a tool, coping strategies, 6 non-arrow identification methods (+ the
   arrow named only), the modify-or-not judgment framework, the 7 modification techniques, and
   Fig.13.5 — nothing Week 3 already owns is re-taught.
2. **Downward Arrow Technique (U18/U26)** — academic mention only. U18 resolved to **Included**, but
   narrowed to name + citation (Burns, 1980) inside the U14 method list, with no procedure, no sample
   questions, no exercise, no simulation. U26 (reframing a rule/attitude into assumption form via the
   arrow) resolved to **Excluded** — its mechanism is inseparable from the barred procedure.
3. **Secondary figures (U25/U37)** — generalized/anonymized. Every "compare to another person"
   mention on the page is a fully generic third-party reference; "Emily" and "Rebecca" are not
   introduced as named recurring figures, and no fact beyond Chapter 13 was added.
4. **U21 (belief questionnaires)** — confirmed **Excluded**; named instruments (Dysfunctional Attitude
   Scale, Personality Belief Questionnaire) do not appear anywhere in learner-facing content.
5. **Public status** — confirmed intended. `CourseCatalog.cs` keeps
   `CurriculumSafetyLevel.ProfessionalReviewRequired`; the routed page resolves to
   `CourseWeekStatus.ProfessionalReviewRequired` ("Изисква професионален преглед"), never `Available`.

U07 (childhood-origin question-category checklist) was additionally resolved to **Excluded** as part
of Decision #1's scope narrowing — a clinician-training checklist beyond the narrowed objectives, with
Sally's own version of it staying Included on Week 3 only.

**Final, mutually exclusive accounting: 40 KUs — 32 Included / 5 Deferred / 3 Excluded / 0 Needs
Review / 0 Unaccounted.**

| Status | Count | KU IDs |
|---|---|---|
| Included | 32 | U02, U03, U04, U05, U10, U11, U12, U14, U15, U16, U17, U18, U19, U20, U22, U23, U24, U25, U27, U28, U29, U30, U31, U32, U33, U34, U35, U36, U37, U38, U39, U40 |
| Deferred | 5 | U01, U06, U08, U09, U13 |
| Excluded | 3 | U07, U21, U26 |
| Needs Review | 0 | — |

Implementation: `Sedmica11.razor` (new), `Week11ContentSliceTests.cs` (new), `CourseCatalog.cs`
(Route + objectives updated), `FinalAssessmentRolloutTests.cs` / `CurriculumHubTests.cs` /
`Week1/3/6/10/12ContentSliceTests.cs` (routing-invariant updates — no LOCKED week content changed).
Week 4 and Week 5 untouched; not committed/pushed. **STOP for owner production review.**
