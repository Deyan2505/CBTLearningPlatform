# Week 12 Retrofit Audit — "Основни вярвания и схеми"

Source: SRC-041 (Judith S. Beck, *Cognitive Behavior Therapy: Basics and Beyond*, 2nd ed.),
**Глава 14 "Идентифициране и модифициране на основни вярвания"**, printed pp. 228–255
(PDF pages 250–277 in the local extraction). Full chapter read this session; extraction cached at
`00_PROJECT_OS/_source_corpus/SRC-041_ch14_bg_extracted.txt` (gitignored, not committed).

Current page: `Sedmica12.razor` (218 lines), `CurriculumSafetyLevel.AcademicContextOnly` →
renders as "Академичен обзор". Built in an earlier, pre-Deep-Learning batch ("Systematic
Curriculum Expansion, Phase B, Batch A" — committed `4135988` as part of the WASM migration,
originally landed earlier per `02_CURRENT_STATUS.md` line 428: "Седмица 12 — `COMPLETE`,
`COMMITTED`"). Unlike Week 10, this page was **not** built from an invented/compressed
approximation — its existing content is already narrowly and accurately source-grounded (the
dev comment even cites a verbatim Chapter 14 quote). The retrofit question here is therefore
different from Week 10's: not "replace invented content with real content," but **"is the
existing narrow scope the right scope, or is the page under-using safe, available source
material?"**

## §0 — Final KU accounting (Unaccounted = 0)

**Owner-approved implementation pass (this section supersedes §1's draft statuses below).**

**40 Knowledge Units total** — **12 Included / 0 Needs Review / 6 Deferred / 22 Excluded / 0 Unaccounted.**

Owner resolved all 6 draft "Needs Review" items:
- **U8** (abstract "screen" filtering mechanism) → **Included**, kept strictly abstract — no
  dialogue, no self-assessment, no "identify your own belief" prompt.
- **U14** (Figure 14.1's full example belief-phrase lists) → **Excluded** — kept the enriched
  thematic *descriptions* (U13) instead; explicitly rejected as a self-recognition checklist.
- **U6** (core beliefs about other people/the world) → **Excluded** — conservative default,
  avoids scope creep beyond the page's self-focused frame.
- **U30** (name the Core Belief Worksheet) → **Excluded** — conservative default.
- **U31** (name the Figure 14.2 technique catalog) → **Excluded** — conservative default.
- **U35** (Stories/Films/Metaphors general principle) → **Excluded** — not among the explicitly
  approved additions (schema/core-belief distinction, developmental origin, why a false belief
  feels true, developing/strengthening a new belief); conservative default applied.

Final Included set (12): U1, U2, U3, U4, U5, U8, U13, U15, U19, U23, U24, U26.
Final Excluded set (22): U6, U7, U12, U14, U17, U20, U21, U22, U25, U28, U29, U30, U31, U32, U33,
U34, U35, U36, U37, U38, U39, U40.
Deferred set unchanged (6): U9, U10, U11, U16, U18, U27 — not dropped, they name where they'd
belong instead (Week 11's future intermediate-belief territory, or general therapist-training
procedure that doesn't fit any student-facing course week at this safety tier).

Implemented in `Sedmica12.razor`: U1/U3/U5 (schema-vs-core-belief distinction + origin +
"known but unarticulated" nuance) in §02; U13/U15 (enriched category themes + ambiguity note) in
§03; U8/U19 (abstract screen mechanism + why a false belief can feel true) in new §04 "Как се
поддържа едно основно вярване"; U23/U24/U26 (developing/strengthening a new belief, described as
what the therapeutic process does, never as self-treatment steps) in new §05 "Развитие на
по-адаптивно вярване"; two new retrieval-check questions (Q4/Q5) testing U1 and U8/U19
conceptually, not self-diagnostically.

## §1 — Knowledge Unit inventory

| ID | KU | Locator | Status | Why |
|----|----|---------|--------|-----|
| U1 | Core belief vs. schema distinction (Beck 1964): schema = cognitive structure, core belief = its specific content | p.228 | **Included** | Definitional, safety-neutral, currently missing from the page |
| U2 | Three broad categories of *negative* core beliefs: helplessness, unlovability (Beck 1999), worthlessness (J. S. Beck 2005); patients may fall in 1, 2, or all 3 | p.228 | **Included** | Already the page's anchor claim |
| U3 | Origin: beliefs form early via genetic temperament + interactions with significant others + situations | p.228 | **Included** | One-line developmental context, safety-neutral |
| U4 | Most people hold relatively positive/realistic core beliefs most of their lives; negative ones may surface only under distress (near-continuous only in personality-disorder patients) | p.228 | **Included** | Directly supports the page's existing "not always negative" framing |
| U5 | Core beliefs are things patients "know" but can't fully articulate until questioning peels back layers of meaning | p.228 | **Included** | Short, general, explains why this differs from automatic thoughts |
| U6 | Patients can also hold negative core beliefs about *other people* and *the world*, not only the self | p.229 | Needs Review | Legitimate scope-widening point; risks scope creep beyond the page's self-focused frame — worth one sentence at most |
| U7 | Sally vignette: schema (de)activation, exaggerating negative data, missing positive data, "yes, but" discounting, as an automatic (not willful) depression symptom | pp.229 | Excluded | Clinical case narrative |
| U8 | Abstract mechanism behind U7: schemas selectively admit confirming data and filter out disconfirming data (the "screen" concept, stated without dialogue) | pp.229, 236–237 | Needs Review | The single highest-value *abstract* concept in the chapter if kept dialogue-free |
| U9 | Clinical rationale for early direct belief-modification work + when it fails (rigidity, low insight, high affect, weak alliance) | p.230 | Deferred | Therapist clinical-judgment guidance, not academic-overview content |
| U10 | Sequencing guidance (work automatic-thought/intermediate-belief tools first when needed); Axis I vs. personality-disorder difficulty | p.230 | Deferred | Clinical-practice guidance; overlaps Week 11's future territory |
| U11 | Six-step overview of the therapist's core-belief treatment process | pp.230–231 | Deferred | A therapist's clinical procedure, not a course concept |
| U12 | Three brief patient vignettes illustrating category discrimination (helplessness vs. unlovability vs. worthlessness) | pp.231–232 | Excluded | Clinical vignettes |
| U13 | Figure 14.1 category *theme descriptions* (helplessness = ineffectiveness at doing/self-protecting/achieving; unlovability = unacceptable/unwanted/defective-in-character; worthlessness = simply bad/unworthy/dangerous, indifferent to effectiveness or lovability) | p.232 | **Included** | This is the real substance behind the page's current one-line category blurbs — strengthens accuracy without adding risk |
| U14 | Figure 14.1's full example belief-*phrase* lists (~11 per category, e.g. "Аз съм некомпетентен," "Аз съм неслюбим," "Аз съм безстойностен") | p.233 | Needs Review | Highest-leverage single decision in this audit — see §4 |
| U15 | Ambiguity note: e.g. "I'm not good enough" could be helplessness *or* unlovability depending on underlying meaning | p.232 | **Included** | Useful, short, safety-neutral nuance |
| U16 | Core-belief identification techniques = same as intermediate beliefs (downward arrow, central-theme spotting, direct elicitation); explicit cross-reference to Ch.13 | p.233 | Deferred | Chapter's own cross-reference — this is Week 11's (Ch.13) territory |
| U17 | Downward-arrow dialogue eliciting a core belief from a stats-assignment automatic thought | p.234 | Excluded | Technique demonstration/dialogue |
| U18 | Presenting the hypothesized core belief to the patient (tentative sharing, pattern-spotting alternative, simplified Conceptualization Diagram, brief childhood exploration) | p.235 | Deferred | Therapist procedure |
| U19 | What patients need to understand about *any* core belief: it's an idea not necessarily truth; can feel true yet be false; is testable; may be childhood-rooted; is maintained by schema filtering; can be changed collaboratively | pp.235–236 | **Included** | Best single new addition — general, safety-neutral, reinforces the page's existing "idea not diagnosis" framing and explains *why* a false belief can feel true |
| U20 | Sally "screen" metaphor — full dialogue + homework assignment | pp.236–237 | Excluded | Dialogue/technique demonstration of U8 |
| U21 | Bibliotherapy references ("Prisoners of Belief," "Reinventing Your Life") | p.237 | Excluded | Clinical reading recommendations, not course-relevant |
| U22 | Developing a new belief: patients who had a pre-existing positive belief can often name it directly; example dialogue | p.237 | Deferred | Dialogue/technique |
| U23 | When patients can't name a prior belief, therapist devises one and guides them to it; a moderate belief is easier to accept than an extreme one | pp.237–238 | **Included** | General conceptual point, no dialogue needed |
| U24 | Old→new core belief example table (unlovable→likable; bad→OK; powerless→"control over many things"; defective→"normal, strengths and weaknesses") | p.240 | **Included** | Concrete, illustrative, safety-neutral, matches the page's plain-language register |
| U25 | Sally dialogue choosing between "I'm competent" and the gentler "I'm competent most of the time, but only human" | p.240 | Excluded | Dialogue |
| U26 | Simultaneous work to weaken the old belief / strengthen the new one, once both are identified | p.240 | **Included** | One-sentence conceptual close |
| U27 | Two general strategies for strengthening new beliefs (therapist elicits/points out data; patient adopts a self-noticing perspective) | p.240 | Deferred | Therapist-technique guidance |
| U28 | Specific Sally techniques list (credits list, intake strengths questions, achievement feedback, etc.) | pp.240–241 | Excluded | Case-specific technique catalog |
| U29 | Patient self-monitoring aids (rubber band, sticky notes, phone alarms) to notice positive data | p.241 | Excluded | Reads as self-guided instruction — barred by the page's no-self-guided-variant boundary |
| U30 | Core Belief Worksheet (CBW, Fig. 14.3) named as an organizing tool; tracking belief strength over time | p.241 | Needs Review | Could be named as "a professional tool exists," academic-fact style, without reproducing it |
| U31 | Figure 14.2 — full technique catalog table (7 "already described" + 6 "additional" named techniques) | p.242 | Needs Review | A bare *named list* (no elaboration) is a defensible academic fact; full catalog is Deferred/Excluded otherwise |
| U32 | Full CBW walkthrough dialogue + Sally's completed worksheet (Fig. 14.3) with reformulations | pp.242–244 | Excluded | Extensive technique/case demonstration |
| U33 | Four prompt-patterns for surfacing "left-side" positive data | pp.245–246 | Excluded | Therapist technique catalog |
| U34 | "Extreme Contrasts" technique, full Sally/dorm-mate dialogue | pp.246–247 | Excluded | Technique/dialogue |
| U35 | "Stories, Films, and Metaphors" — general principle (seeing an invalid belief clearly in a fictional character helps patients doubt their own) + Cinderella case example | p.247 | Needs Review | The *general principle* is a safe, interesting, non-clinical cognitive point; the case example itself stays excluded |
| U36 | "Historical Tests of the Core Belief" — full methodology + Sally's childhood-memory walkthrough | pp.247–248 | Excluded | Technique/case demonstration |
| U37 | "Restructuring Early Memories" — Gestalt-derived experiential technique, rationale for use (mainly personality-disorder patients) | pp.248–249 | Excluded | Advanced clinical technique |
| U38 | Five-step restructuring-early-memories methodology | p.249 | Excluded | Clinical procedure |
| U39 | Annie case — full extended guided-imagery/role-reversal session (two childhood memories), outcome belief percentages | pp.249–255 | Excluded | The chapter's largest content block; entirely clinical-case material |
| U40 | Chapter closing summary + further-reading pointers (J. S. Beck 2005, Beck et al. 2004, Young 1999, Riso et al. 2007) | p.255 | Excluded | Restates already-excluded material; no standalone value |

**Counts:** Included 11 (U1,U2,U3,U4,U5,U13,U15,U19,U23,U24,U26) · Needs Review 6
(U6,U8,U14,U30,U31,U35) · Deferred 6 (U9,U10,U11,U16,U18,U27) · Excluded 17 (U7,U12,U17,U20,U21,
U22,U25,U28,U29,U32,U33,U34,U36,U37,U38,U39,U40) · **Unaccounted 0**.

## §2 — Current-page diagnosis

- **Correct and should remain unchanged:** the `AcademicContextOnly`/no-self-guided-variant
  framing (Section 04, `DisclaimerCallout`, the 3-question `#proverka`); the Week 3 recap-and-link
  pattern in Section 01 (does not re-render the hierarchy diagram — correct, preserves Week 3's
  ownership); the "not every core belief is negative" framing already reconciled against Week 3;
  the three-category names and their reuse of `.category-compare`; zero new components/CSS
  discipline; the `OptionalReadingSource`/citation block.
- **Missing (available, safe, source-supported):** U1 (schema/core-belief distinction), U3–U5
  (origin + "known but unarticulated" nuance), U13 (the real category theme descriptions, vs. the
  current one-line paraphrases), U15 (categorization ambiguity note), U19 (why a false belief
  feels true — arguably the page's single best possible addition), U23/U24/U26 (developing and
  strengthening a new belief — currently entirely absent from the page, despite being safe,
  concrete, and non-technique-demonstrating).
- **Overlaps Week 3 / Week 11:** none currently mishandled — Section 01 already defers to Week 3
  correctly. U16 (identification techniques) and U9–U11/U18/U27 (therapist procedure) are Week
  11's (Ch. 13) or general-clinical-practice territory and must **not** be pulled into Week 12.
- **Unsupported or over-generalized:** none found — the existing page does not overreach the
  source.
- **Genuinely needs migration:** nothing structural. This is a *content-depth* retrofit, not an
  architecture or terminology retrofit (unlike Week 10). No invented scenario to retire, no
  compressed category set to fix, no terminology drift to reconcile.

## §3 — Terminology Map

- Canonical: **"основно вярване"** (core belief), matching the current page and (per its own dev
  comment) Week 3's established usage. The extracted Chapter 14 source itself is inconsistent —
  early/late passages say "основно вярване," the "Categorizing"/"Identifying" mid-chapter section
  headings and body text say "основно убеждение" (a second, interchangeable Bulgarian rendering,
  same pattern as Week 10's "балансиран/адаптивен" split). No page-content change needed — just
  confirm "вярване" stays canonical, "убеждение" never introduced.
- "Схема" (schema) — used precisely (Beck 1964's structure/content distinction); the current page
  says "вярвания и схемите" in its intro without ever defining the distinction. U1 fixes this.
- Category names: page uses "Безпомощност / Необичаемост / Безполезност" — matches the source's
  clearest three-way naming; source also contains looser variants ("невъзможност за обич,"
  "неслюбим") that are not adopted and should not be.
- "Работен лист за основни вярвания" (Core Belief Worksheet) — named only if U30 is approved
  Included; no Bulgarian abbreviation currently established (source uses "ЛОВ").

## §4 — Representation Fit (evaluated, not assumed)

- **Weekly Mind Map:** **not recommended.** Week 12 sits at the bottom of Week 3's own hierarchy
  diagram; a new per-week Mind Map would either duplicate Week 3's diagram or manufacture a
  hierarchy where the chapter's real structure is a flat 3-way category split. The page's own dev
  comment already commits to never re-rendering the hierarchy — a new map would break that.
- **Hierarchy/schema visualization:** **not recommended.** Nothing in Ch. 14 is hierarchical
  beyond what Week 3 already owns; `.category-compare` (already in use) is the correct fit for a
  flat 3-category split — no ConceptGraph/ConceptMap justified.
- **Global Concept Map (`/kurs/karta`) extension:** **worth owner consideration, low cost.** The
  existing `KnowledgeMapCatalog.cs` already has 10 concepts/12 relations from Weeks 3/6/8/10 and
  fully reuses the locked `ConceptGraph.razor` — adding a "core belief categories" concept node
  connected to Week 3's existing "core belief" concept is a small, architecture-safe extension if
  the owner wants Week 12 represented there at all (it currently is not).
- **Academic worked example:** **not recommended as an interactive/step-through element.** Every
  worked example in the source (Sally's downward arrow, Sally's CBW, Annie's full session) is a
  clinical technique demonstration — reproducing any of them, even read-only, risks reading as
  "here is how to do this to yourself," which is exactly what `AcademicContextOnly` + no
  self-guided variant is meant to prevent. At most, a single third-person sentence noting the book
  illustrates this work through therapy transcripts, pointing to `OptionalReadingSource` — no
  page content reproduces a dialogue.
- **Retrieval practice:** the existing 3-question `#proverka` is appropriately minimal; if U1/U13/
  U19 are added, 1–2 more questions covering the new content is a proportionate, non-structural
  extension (same component, no new representation).

## §5 — Owner decisions required before implementation

1. Approve the §1 KU accounting (11 Included / 6 Needs Review / 6 Deferred / 17 Excluded).
2. **U14** — include Figure 14.1's full example belief-phrase lists under each category card
   (concrete, recognizable — but highest self-application risk), or keep the page's current
   one-line abstract paraphrase only?
3. **U8** — include the "screen" metaphor as a purely abstract concept (schemas admit confirming
   data, filter disconfirming data), with no dialogue and no self-application prompt?
4. **U6** — one sentence noting core beliefs can also be about other people/the world, or omit for
   focus?
5. **U30/U31** — name the Core Belief Worksheet and/or the Figure 14.2 technique-name list as
   academic facts ("professionals use tools such as…"), with zero elaboration, or omit both
   entirely?
6. **U35** — include the general "seeing an invalid belief in a fictional character" principle
   (no Cinderella case), or omit?
7. Confirm the global `/kurs/karta` Concept Map extension (a "core belief categories" node linked
   to Week 3) is out of scope for this pass, or should be included alongside the page content
   changes.
8. Confirm: zero interactive/step-through worked example, zero reproduced dialogue (Sally or
   Annie), in line with `AcademicContextOnly` + no-self-guided-variant — this is the one boundary
   this audit treats as non-negotiable rather than a genuine open question.
