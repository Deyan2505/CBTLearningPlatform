# Week 10 — Deep Source + Coverage Retrofit Audit v1 ("Сократически въпроси и съвместно изследване")

*Implemented — `/kurs/sedmica-10` (`Sedmica10.razor` + `SocraticDialogueExplorer.razor`). Not
committed. Owner approved all §9 decisions from the original audit; final post-implementation
accounting is §0 below.*

## 0. Final accounting (post-implementation)

All 7 previously-`Needs Review` KUs (the Карен worked example) moved to **Included** after direct
verification against the extracted chapter text — the situation, automatic thought, belief/emotion
percentages, and all six categories' dialogue are quoted/retold from real, located passages
(printed pp. 168, 171–175), not invented. No new Сали history beyond this chapter.

| Disposition | Count | KU IDs |
|---|---:|---|
| **Included** | **23** | U01–U03, U07–U09, U10, U11, U12–U14, U16–U19, U20, U27, U28, U50–U54 |
| **Needs Review** | 0 | — resolved, see above |
| **Deferred** (Week 9's exclusive, owner-approved territory) | 30 | U04–U06, U21–U26, U29–U33, U34–U45, U46–U48 |
| **Excluded** (safety/scope) | 1 | U15 (death-fear decatastrophizing guidance) |
| **Unaccounted** | **0** | — |
| **Total** | **54** | |

## 1. What exists today (full inspection)

`Sedmica10.razor` (10 sections) + `Interactive/SocraticDialogueExplorer.razor` (1 WASM island) +
`Week10ContentSliceTests.cs` (34 tests). Built in the **pre-Deep-Learning era** ("CONTENT-DRIVEN
TEMPLATE VALIDATION, Slice 3" — the "Guided Practice" archetype). Its own dev comment is explicit:
*"map only, never verbatim text"* and cites no chapter or page range at all — just "a chapter on
guided discovery, Socratic questions" in `OptionalReadingSource`. This is the only remaining routed
week never audited against the real source with the current Deep Learning rigor.

**Content inventory of the current page:**
- 01 Explore vs. leading-question distinction (2-column comparison card)
- 02 "Four families" of questions: Evidence · Alternatives · Likely consequences · Distancing-and-
  usefulness — each with 2–3 sample questions
- 03 `SocraticDialogueExplorer` — a 5-step guided walkthrough over **one fixed, invented scenario**
  ("you send prepared material, get a curt reply, think 'it must be bad'") — not from SRC-041,
  structurally similar to but not the same as any real source example
- 04 Question-vs-disguised-advice classification exercise (4 examples)
- 05 Fact / assumption / conclusion distinction (3-item comparison)
- 06 Decatastrophizing (explains the *concept*, no worked example)
- 07 Balanced response vs. over-negative vs. forced-positive (3-item comparison)
- 08 A 6-step "practical sequence" (Stop → Name the thought → Separate facts from conclusions →
  Consider alternatives → Assess probability/consequences → Formulate a plausible response)
- 09 Final check (4 questions)
- 10 Summary/academic context/disclaimer/sources

**Not present at all**: a Weekly Mind Map (every other Deep Learning week has one), an exact chapter
citation, a Coverage Matrix, any worked example traceable to a real page number.

## 2. Source located: SRC-041 Chapter 11 (same chapter already read for Week 9)

**"Оценяване на автоматични мисли"**, printed pp. 167–186 — the exact chapter that supplies **all**
of the current page's real content: Figure 11.1's six Socratic evaluation-question categories map
directly onto the page's "four families" (compressed: decatastrophizing folded into "consequences",
"effect of believing" folded into "distancing and usefulness"); the explore-vs-leading distinction
and collaborative-empiricism framing come from the chapter's own opening rationale; decatastrophizing
and the "not forced positivity" principle are both explicitly in the source. No other chapter is
needed — Chapter 12 (Thought Record) is Week 9's exclusive territory (see §4) and contributes
nothing Week 10 needs. Already read in full this session (cached at
`_source_corpus/SRC-041_ch11_bg_extracted.txt`, reused from the Week 9 audit — no re-extraction
needed since this is the identical chapter).

## 3. Knowledge Unit inventory — reuses the Week 9 audit's own Chapter 11 numbering (U01–U54)

Deliberately the same 54 units already catalogued for Week 9's audit (`WEEK_09_SOURCE_COVERAGE_
AUDIT_v1.md`), since both pages draw on the same chapter — re-numbering would just obscure the
overlap this audit exists to resolve. Full one-line descriptions are in that file; grouped here by
Week-10 relevance:

- **U01–U03, U07–U09**: selecting which thoughts matter, why not challenge directly, grain-of-truth.
- **U10–U19**: Figure 11.1's six categories + their worked demonstration on "Карен" (a new friend
  for Сали, not previously established — same case-history class as Week 7/9's resolved items).
- **U20**: evaluating the outcome (re-rating belief/emotion afterward).
- **U21–U26**: the five reasons an evaluation attempt fails, illustrated with **Джон** — **Week 9's
  own §"9.5"**, already built and owner-approved there.
- **U27–U28**: varying/customizing the questions (an unnamed-patient example).
- **U29–U33**: identifying/self-labeling distortions — **Week 9's own §"9.3" territory**.
- **U34–U45**: the twelve distortions themselves — **Week 9's own §"9.3"**, verbatim, owner-approved.
- **U46–U49**: when a thought is true — **Week 9's own §"9.4"**, owner-approved.
- **U50–U54**: teaching patients to self-evaluate, and the "shortcut" (skip straight to an adaptive
  response) — **not used anywhere yet**, genuinely available.

## 4. Coverage Matrix — pre-implementation draft (superseded by §0's final accounting)

*This table is the original first-pass disposition, kept for the audit trail. §0 above has the
final, owner-approved, mutually-exclusive accounting actually implemented — read that one first.*

| Disposition | Count | KU IDs |
|---|---:|---|
| Included (draft) | 16 | U01–U03, U07–U09, U10, U11, U20, U27, U28, U50–U54 |
| Needs Review (draft — Карен case-history) | 7 | U12–U14, U16–U19 — resolved to Included in §0 after direct source verification |
| Deferred (unchanged) | 30 | U04–U06, U21–U26, U29–U33, U34–U45, U46–U48 |
| Excluded (unchanged) | 1 | U15 (death-fear decatastrophizing guidance) |
| Unaccounted | **0** | — |
| Total | **54** | |

`U49` (the external ACT/Hayes citation) sits inside the U46–U48 Deferred cluster and needs no
separate line — it was already Excluded at the Week 9 level and isn't Week 10's concern either.

## 5. Page-by-page diagnosis against the source

**Correct, source-consistent, should remain untouched:**
- Section 01's explore-vs-leading distinction — matches the chapter's own rationale in spirit.
- Section 04's advice-vs-question classification — safe, generic, no source conflict.
- Section 05's fact/assumption/conclusion frame — compatible with the evidence-question category,
  though not itself a named source structure.
- Section 06/07's decatastrophizing-isn't-denial and balanced-isn't-forced-positive framings — both
  genuinely consistent with the source (Ch.11 explicitly frames decatastrophizing as "what's the
  worst/best/most realistic outcome," never "there's no problem"; nothing in the chapter treats a
  balanced response as required-positive).
- Section 08's 6-step self-application sequence and Section 09's final check — generic, safe,
  no conflict; can stay as-is or be lightly re-grounded.
- The `SocraticDialogueExplorer` component **architecture** itself (fixed-scenario, step-select,
  zero input/storage) is exactly the right shape for this content — Representation Fit confirms it,
  no rebuild needed, only its **content** is in question (§7).
- All of Section 08/10's hard-won responsive CSS (`.guided-practice-sequence`, `.concept-map__flow--
  process`) — several rounds of owner visual review already went into this; a retrofit should build
  *around* it, not replace it.

**Missing, relative to the source:**
- The chapter's own three-part rationale for *why* collaborative examination beats direct challenge
  (U08) — currently absent from Week 10 (Week 9 has a version of it, but Week 10's own "how to ask
  good questions" framing would benefit from stating it too, at least as a cross-link).
- A **named, worked example** carried through the six real categories — the current page has generic
  sample questions per family, but (unlike Week 9's John, or Week 3/6/7's Sally) nothing that shows
  the technique actually working end-to-end on one real, source-traceable situation.
- Evaluating the outcome afterward (U20) — the current page ends at "formulate a plausible response,"
  never re-checks belief/emotion.
- Teaching the patient to do this themselves, and the advanced "shortcut" (U50–U54) — a good, real,
  currently-unclaimed fit for a "how you'd use this on your own" closing note.
- A Weekly Mind Map (Preview + Review) — absent, unlike every other Deep Learning week.
- An exact chapter/page citation — currently just "a chapter on guided discovery."

**Duplicated from Week 9 (risk only if migration is done carelessly)**: none currently — the two
pages don't overlap today because Week 10 predates Week 9. The risk is prospective: a naive
migration that "fills in the missing six categories in full" could accidentally re-import distortion
names or Джон's material. §4's Deferred list exists precisely to prevent that.

**Unsupported or over-generalized:**
- The "four families" compression doesn't match the source's own six-category structure — and Week
  9's own recap section (`Sedmica9.razor` §9.2) already promises the reader "пълното разгръщане... в
  Седмица 10" using the **six-category** framing. Left as four families, Week 10 would technically
  contradict what Week 9 told the reader to expect.
- `SocraticDialogueExplorer`'s scenario is invented, not source-verbatim — permitted under the old
  "map only" policy this page was built under, but the current standard requires either a real
  source-traceable example or an explicit editorial note that it's a platform-original illustration.
- The vague citation needs the exact "Глава 11, стр. 167–186" correction every other Deep Learning
  week already carries.

## 6. Terminology Map

| Term | Status |
|---|---|
| Сократически въпроси | Established, matches source (which itself notes "сократически" is sometimes a misnomer — worth a one-line mention for accuracy, currently absent) |
| Балансиран отговор (Week 10's term) vs. Адаптивен отговор (Ch.11's own wording) | **Open reconciliation** — flagged already in the Week 9 audit; Week 10 is where it would actually need resolving, since Week 10 uses "балансиран отговор" extensively |
| Изследващ / насочващ въпрос | Week 10's own terms, no direct source conflict, reasonable as-is |
| Декатастрофизиране, дистанциране, доказателства „за/против" | Should read identically to Week 9's own wording for the same underlying concepts, once both pages cite the same six categories |

## 7. Representation Fit

- **Weekly Mind Map (Preview + Review)** — Yes, clearly missing, should be added like every other
  Deep Learning week. Natural clusters: изследване срещу насочване · шестте категории въпроси (the
  real fix, replacing "four families") · факт/предположение/заключение · декатастрофизиране ·
  балансиран отговор · самостоятелно приложение (U50–54).
- **Decision/questioning process visualization** — already present twice (Section 01's conceptual
  6-step flow, Section 08's practical 6-step rail) — Representation Fit confirms both are genuinely
  useful and distinct (one is "what Socratic examination conceptually does," the other is "how you'd
  walk through it yourself") — recommend keeping both, only updating Section 01's step content if
  the six-category migration changes its shape.
- **Guided Socratic interaction** — already present (`SocraticDialogueExplorer`), architecture is
  right; only the scenario content is a genuine open question (§9, owner decision 2).
- **Case/application exercise** — Section 04 already serves this; could gain one more scenario once
  a real worked example exists, applying the full six-category structure end-to-end.

## 8. Retrieval/application opportunities (DoD v3 taxonomy)

- **Recognition**: leading vs. exploratory question (already covered, Section 04).
- **Retrieval**: recall the six question categories in order (new, once migrated from four to six).
- **Application**: given a fixed scenario, generate/select an appropriate question from a named
  category (extends the existing dialogue explorer or Section 04's pattern).
- **Reasoning**: explain why collaborative examination beats direct challenge (U08 — currently
  absent from Week 10, a genuine gap).

## 9. Genuine owner decisions required before implementation

1. **Six categories, not four.** Migrate Section 02 from the compressed "four families" to the
   source's real six (evidence, alternative explanation, decatastrophizing, effect-of-belief,
   distancing, problem-solving) — resolves the direct inconsistency with what Week 9 already
   promises the reader. Recommended; low risk; keeps all currently-correct content.
2. **Сали's friend "Карън" (7 KUs).** Same class of decision already resolved for Week 7/9: use her
   as the worked six-category example (source-grounded, retold not verbatim), or keep the page's
   existing invented scenario, or design a fresh unnamed illustration instead. This is the single
   highest-impact content decision — a real worked example is the biggest gap found.
3. **U50–U54 (teaching self-evaluation + the "shortcut").** Genuinely unclaimed, fits Week 10's own
   mandate well — confirm inclusion as a closing subsection.
4. **Terminology**: keep "балансиран отговор" as the platform's public-facing standard (recommended
   — already consistent across this page and used elsewhere), or migrate toward the source's literal
   "адаптивен отговор"?
5. **Citation correction and Weekly Mind Map addition** — both straightforward, low-risk migration
   items, not really contested, but confirming scope before implementation as requested.
6. **`SocraticDialogueExplorer`'s existing invented scenario** — retire it in favor of decision 2's
   outcome, or keep it as a second, explicitly-labeled "illustrative, not from the source" example
   alongside a new source-grounded one?
