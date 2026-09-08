# Week 15 — Deep Source + Coverage Audit v1

**Status:** `OWNER DECISIONS APPROVED — FINAL AUDIT NORMALIZED — IMPLEMENTED, TECHNICALLY READY — NOT
YET OWNER APPROVED / LOCKED`. See §14 for the full resolution log and the recalculated 76-KU tally.
Sections 0–13 below are the original turn-1 audit (read-only, source scope, KU inventory as first
drafted) and are preserved as the evidentiary record; §14 is authoritative for final status.

## 14. Owner decisions received (turn 2) and final resolution

The owner approved all nine §11 decisions with specific direction on each. Resolution of the 7
`Needs Review` KUs:

- **C1 (the "wave" framing itself):** **Included, but demoted.** Per owner direction, the learner-facing
  title drops the wave framing entirely (`Съвременни разширения на КПТ и възстановително-ориентирана
  терапия`, not "Трета вълна…"). „Трета вълна" appears exactly once, as a named secondary-literature
  label in a caveated aside stating it is not used here as a settled taxonomy — never as a section
  heading, never as the organizing structure.
- **C2, C3, C4 (wave decade ranges):** **Excluded.** No decade appears anywhere on the page — consistent
  with the original audit's own recommendation and reinforced by the owner's "do not build around a weak
  historical framing" direction.
- **C8 (MBCT):** **Excluded**, per explicit owner instruction. Week 15 names only DBT and ACT — the two
  adaptations SRC-041 itself cites with author and year. MBCT does not appear on the learner-facing page.
- **D14, D15 (CT-R belief categories):** **Included, categories only.** Defeatist performance beliefs and
  asocial beliefs are named and their function explained; their specimen statements appear only as
  paraphrased, third-person, static illustrations — never first-person, never a self-classification
  prompt, never phrased as "which one are you."

**Presentation-only adjustments to already-Included KUs** (no status change, owner direction on framing):
C5 (content→stance shift) and C6 (Buddhist origin of mindfulness/acceptance) are folded into the same
caveated aside as C1, explicitly attributed to secondary literature rather than stated as settled primary-
source fact — per the owner's "do not let Wikipedia carry core learner-facing claims." C11 (the
non-superiority efficacy caveat) is the one explicit exception the owner carved out: it is kept, in the
evidence section, clearly attributed to review-level secondary literature and deliberately juxtaposed with
CT-R's own study evidence so the page never implies newer approaches are inherently superior.

**Final, recalculated, mutually exclusive KU accounting:**

**76 KUs total = 50 Included / 9 Deferred / 17 Excluded / 0 Needs Review / 0 Unaccounted.**

Group C (third wave / later adaptations, 11 KUs) resolves item-by-item as: C1 Included (demoted/
caveated), C2/C3/C4 Excluded (decade ranges), C5/C6/C7/C9/C11 Included (already settled turn-1), C8
Excluded (MBCT), C10 Excluded (already settled turn-1) — **6 Included / 0 Deferred / 5 Excluded.**

| Group | KUs | Included | Deferred | Excluded |
|---|---|---|---|---|
| A — the CBT family | 20 | 14 | 4 | 2 |
| B — reach and limits | 6 | 5 | 1 | 0 |
| C — third wave / later adaptations | 11 | 6 | 0 | 5 |
| D — CT-R | 27 | 24 | 1 | 2 |
| E — mindfulness ownership | 2 | 1 | 1 | 0 |
| F — therapist development (Гл. 21) | 10 | 0 | 2 | 8 |
| **Total** | **76** | **50** | **9** | **17** |

**Title/objectives (final, owner-directed):**

- **Title:** „Съвременни разширения на КПТ и възстановително-ориентирана терапия"
- **Route:** `/kurs/sedmica-15` — routed, completion-eligible
- **Safety:** `AcademicContextOnly` (preserved) · **Format:** `AcademicOnly` (preserved) · resolves to
  `AcademicOverview` status (Weeks 4/12 precedent)

**Unresolved items — final confirmation (unchanged from §10):** C15-K05/Гл. 21 remains Deferred/
Unassigned; GAP-014's remaining half remains Open/Unassigned; Гл. 16/19/20/21 remain without a
curriculum owner. None annexed to Week 15.

**Implementation proceeds directly from this normalization** per owner instruction — no further planning
cycle. See `Sedmica15.razor` and `Week15ContentSliceTests.cs` for the built result.

---

**Headline finding, before anything else:** Week 15 is the **first week in the course with no SRC-041
chapter behind it.** This was verified, not assumed — see §1.2. Every other week so far mapped onto one
or two chapters of Judith Beck's textbook. Week 15's declared topic (third wave + CT-R) does not exist
in that book. That does **not** make Week 15 unsourceable — §1.3 shows it is sourceable, and better than
the roadmap currently believes — but it changes the evidentiary basis of the whole week, and three of
the nine owner decisions in §11 exist only because of it.

---

## 0. Canonical Week 15, read from the live project

| Field | Live value | Where read |
|---|---|---|
| Number / module | 15 — „Разширени техники и академичен контекст" (Module IV) | `CourseCatalog.cs:217` |
| Title | **„Трета вълна и възстановително-ориентирана терапия"** | `CourseCatalog.cs:218` |
| Short summary | „Кратък обзор на по-новите направления в КПТ традицията." | `CourseCatalog.cs:219` |
| Safety classification | **`CurriculumSafetyLevel.AcademicContextOnly`** | `CourseCatalog.cs:220` |
| Route | **`null`** — the last unrouted week in the course | `CourseCatalog.cs:220` |
| Derived status | `AcademicOverview` (unrouted) | `CurriculumEnums.cs:67` |
| Declared format | `InteractiveFormat.AcademicOnly` | `CourseCatalog.cs:226` |
| Objectives | 2 items — „Получавате кратък обзор…", „Разбирате, че полето продължава да се развива." | `CourseCatalog.cs:221–225` |
| Syllabus scope | „Разширение на КПТ: Третата вълна и Възстановително-ориентирана когнитивна терапия (CT-R)" | `kpt_syllabus.pdf`, p.7 |
| Roadmap readiness | „CONSTRAINED (вълни) / NOT READY (CT-R)"; Batch D | `24_IMPLEMENTATION_ROADMAP.md:364, 520–521, 580` |

### 0.1 Catalog vs. syllabus — flagged, not silently fixed

The **title matches** the syllabus. The **summary and objectives do not.** The syllabus asks for five
specific things (the content→stance shift; ACT/DBT/MBCT; an introduction to CT-R; CT-R methodology —
values, empowerment, adaptive mode; strengths-based living rather than deficit removal). The catalog
carries a two-line placeholder that promises only "a short overview" and "the field keeps developing."

This is the same placeholder shape Weeks 11, 13 and 14 each carried before implementation, and each had
its summary and objectives rewritten at implementation time. **Recommendation: treat the current
summary/objectives as stale placeholders to be rewritten from the approved KU set, not as a scope
constraint.** They are the thinnest in the catalog and would under-describe any honest implementation.

The **declared format (`AcademicOnly`) and safety level (`AcademicContextOnly`) are correct and should
be preserved** — they match the Weeks 4 and 12 precedent exactly (both are `AcademicContextOnly` +
`AcademicOnly` + routed, resolving to `AcademicOverview`). Unlike Week 13, there is no format/safety
contradiction here to resolve.

---

## 1. Source scope — verified, not assumed

### 1.1 What the roadmap expected

`24_IMPLEMENTATION_ROADMAP.md:521` records: *"'възстановително-ориентирана терапия'/CT-R терминологията
— не потвърдена от НИКОЙ прегледан източник; SRC-041 (2011, 2-ро изд.) хронологично малко вероятно да
покрива тази по-късна терминология."* GAP-011 repeats this and adds that the three-wave framing is
confirmed by three independent reviewed sources (SRC-014, SRC-023, SRC-025).

**Half of that is right and half is materially stale.** Both halves are corrected below.

### 1.2 SRC-041 is genuinely silent — verified by full-text scan

I scanned all 414 pages of the Bulgarian SRC-041 PDF for the week's vocabulary. Results:

| Term searched | Pages hit |
|---|---|
| „трета вълна" / „третата вълна" / "third wave" | **0** |
| "CT-R" / „възстановително-ориентира" / "recovery-oriented" / „адаптивен режим" | **0** |
| "MBCT" | **0** |
| „диалектическа" (DBT) | 2 — printed **p.2**, and p.170 (unrelated: Socratic method) |
| „приемане и ангажиране" (ACT) | 1 — printed **p.2** |
| „осъзнатост" / „внимателност" (mindfulness) | 7 — of which only printed **p.264** is substantive |

The roadmap's chronological reasoning is confirmed: **the 2011 textbook contains no third-wave framing
and no CT-R.** The 21-chapter table of contents ends at „Напредване като терапевт" (printed p.358); no
chapter covers this week's topic. `23_CLINICAL_SAFETY_BOUNDARIES.md:58` explicitly permits secondary web
sources to be cited directly *"освен когато SRC-041 мълчи по темата"* — that exception is exactly what
Week 15 needs, and it is the same mechanism that closed Week 2's ABC gap via SRC-042.

### 1.3 But SRC-041 does supply a primary-source anchor — printed p.2

The scan surfaced something the roadmap did not anticipate. SRC-041 Chapter 1, **printed p.2 / PDF p.24**,
names the CBT family with author-and-year citations:

> „Има редица форми на когнитивно-поведенческа терапия, които споделят характеристиките на терапията на
> Бек, но чиито концептуализации и акценти в лечението варират до известна степен. Те включват рационална
> емоционална поведенческа терапия (Ellis, 1962), **диалектическа поведенческа терапия (Linehan, 1993)**,
> терапия за решаване на проблеми (D'Zurilla & Nezu, 2006), **терапия за приемане и ангажиране (Hayes,
> Follette, & Linehan, 2004)**, експозиционна терапия (Foa & Rothbaum, 1998), терапия с когнитивни процеси
> (Resick & Schnicke, 1993), когнитивно-поведенческа анализна система на психотерапията (McCullough, 1999),
> поведенческа активация (Lewinsohn, Sullivan, & Grosscup, 1980; Martell, Addis, & Jacobson, 2001),
> модификация на когнитивното поведение (Meichenbaum, 1977) и други."

**DBT and ACT — two of the syllabus's three named third-wave therapies — are cited in the primary source
itself, with founders and years.** The same page states that adaptations *"промениха фокуса, техниките и
продължителността на лечението, но самите теоретични предположения останаха постоянни"*, and that Beck's
CBT *"често включва техники от всички тези терапии… в когнитивна рамка."*

**This passage is unclaimed.** It is Week 1's KU **U7**, which owner decision #3 moved to **Excluded**
(`WEEK_01_RETROFIT_AUDIT_v1.md:32`) on relevance grounds — *"no standalone name-list; no name in this set
was necessary to explain the specific historical sequence implemented"* — not on source-validity grounds.
In Week 15 the same list is not trivia; it is the subject. It is available.

### 1.4 The third-wave sources have degraded since they were reviewed

I re-fetched the sources GAP-011 relies on. Two no longer say what the register records:

| Source | Register/GAP-011 says | **What the live page says today** |
|---|---|---|
| **SRC-014** (3rdwavetherapy.com) | „потвърждава 3-вълновия модел… списък ACT/DBT/EFT/EMDR/ERP/IFS" | **Does not name DBT. Does not name MBCT. Does not describe the first or second wave. Gives no dates. Names no founders.** Only ACT survives, plus the content→relationship shift and "psychological flexibility". |
| **SRC-023** (drjohngkuna.com) | „различна времева рамка за 'вълните'" | **Does not use the wave framing at all** — no first/second/third wave, no ACT/DBT/MBCT/mindfulness. Only Ellis (1950s) and Beck (1960s). |
| **SRC-025** (Wikipedia, CBT) | „потвърждава… 3-вълновия модел с различни дати" | **Confirmed and intact** — see below. |

**Net effect: the "three waves" framing now rests on one live reviewed source, and it is Wikipedia.**
GAP-011's stated basis of "three independent reviewed sources" no longer holds. This is the single most
consequential finding for scoping the week, and it drives owner decisions #2–#4.

SRC-025 does carry the framing cleanly, including the sentence that matches the syllabus almost word for
word: *"A change of orienting assumptions constituted the 'third wave' of CBT, in which the focus shifted
from the content of one's cognitions to one's stance toward one's cognitions."* It names DBT, MBCT, ACT
and compassion-focused therapy; gives wave ranges (1st 1920s–1960s, 2nd 1950s–1970s, 3rd 1980s–1990s
onward); attributes mindfulness and acceptance to Buddhist principles; and carries an efficacy caveat.

### 1.5 The CT-R half is well sourced — GAP-011 is wrong about this

**SRC-013 — Beck Institute, "What is Recovery-Oriented Cognitive Therapy (CT-R)?" — is already registered
`FULLY REVIEWED`** (`11_SOURCE_REGISTER.md`, SRC-013), with the register's own summary confirming *"CT-R
за тежки психични състояния, 'достъп/развитие/актуализиране на адаптивния режим' модел."*

The claim that CT-R is *"не потвърдена от НИКОЙ прегледан източник"* is therefore **factually wrong** and
should be corrected. It most likely originates from SRC-032 (Kaiser Permanente CT-R basics), which *is*
dead (HTTP 404) — but SRC-013 is alive, official, and detailed.

SRC-013 stands in the same relation to CT-R that SRC-042 (Albert Ellis Institute) stands to REBT and that
SRC-007…SRC-010 stand to Beck: an **official institutional source, the highest available tier for a topic
on which SRC-041 is silent.** It is authored by Feldman, Best, **Aaron T. Beck**, Inverso and Grant, and
supplies a four-stage model, two named belief categories, a clean CT-R/CBTp distinction, and six cited
studies including a 2012 RCT and a 2020 Guilford Press book.

**The CT-R half of Week 15 is better sourced than the third-wave half.** That inverts the roadmap's
assumption, which treated the waves as CONSTRAINED-but-workable and CT-R as NOT READY.

### 1.6 Final source scope for Week 15

| ID | Source | Locator | Tier | Role |
|---|---|---|---|---|
| **S1** | SRC-041, Гл. 1 | printed **p.2** / PDF p.24 | Primary (citation-grade) | The CBT family incl. DBT + ACT with citations; constancy of theoretical assumptions |
| **S2** | SRC-041, Гл. 1 | printed **p.3** / PDF p.25 | Primary | Reach of CBT; schizophrenia/full-session limit — the bridge to CT-R's population |
| **S3** | SRC-013 | beckinstitute.org — CT-R | Official institutional | The entire CT-R half |
| **S4** | SRC-025 | Wikipedia, CBT — third wave | Secondary (weak) | The wave framing, MBCT, the content→stance shift, the efficacy caveat |
| **S5** | SRC-041, Гл. 15 | printed **p.264** | Primary | Mindfulness definition — **already Included in Week 13** (C15-K55); cross-link only |
| — | SRC-014, SRC-023 | — | — | **Not used.** Degraded; no longer support what the register records (§1.4) |
| — | SRC-041, Гл. 21 | printed pp.358–360 | Primary | **Not used.** Read in full; see §5.3 — topic mismatch + safety |

---

## 2. Knowledge Unit inventory — 76 KUs, 100% accounted

**Total 76 = 47 Included / 9 Deferred / 13 Excluded / 7 Needs Review / 0 Unaccounted.**

| Group | Source | KUs | Incl. | Def. | Excl. | NR |
|---|---|---|---|---|---|---|
| **A** — the CBT family | S1 (Гл.1 p.2) | 20 | 14 | 4 | 2 | 0 |
| **B** — reach and limits | S2 (Гл.1 p.3) | 6 | 5 | 1 | 0 | 0 |
| **C** — third wave | S4 (SRC-025) | 11 | 5 | 0 | 1 | 5 |
| **D** — CT-R | S3 (SRC-013) | 27 | 22 | 1 | 2 | 2 |
| **E** — mindfulness ownership | S5 / Week 13 | 2 | 1 | 1 | 0 | 0 |
| **F** — therapist development | Гл. 21 | 10 | 0 | 2 | 8 | 0 |
| **Total** | | **76** | **47** | **9** | **13** | **7** |

### 2.1 Group A — the CBT family (A1–A20), SRC-041 printed p.2

| ID | KU | Status | Why |
|---|---|---|---|
| A1 | Beck developed CBT in the early 1960s, first called "cognitive therapy"; now synonymous with CBT | Deferred → Week 1 | Week 1's U4; origin naming is Week 1's territory |
| A2 | Beck and others adapted the therapy to a surprisingly diverse range of populations and disorders | **Included** | Opens the week: the field expanded |
| A3 | **Adaptations changed focus, techniques and duration — but the theoretical assumptions stayed constant** | **Included** | The week's spine. The one claim that makes "new directions" coherent rather than a list |
| A4 | Treatment is based on a cognitive formulation of a specific disorder (Alford & Beck, 1997) | Deferred → Week 3/4 | Conceptualization is owned |
| A5 | Treatment is based on conceptualization of the individual patient | Deferred → Week 3/4 | Same |
| A6 | The therapist seeks cognitive change to produce lasting emotional/behavioural change | Deferred → Week 3 | Core model, already taught |
| A7 | Intellectual lineage: Epictetus, Horney, Adler, Kelly, Ellis, Lazarus, Bandura | Excluded | Backward lineage; owner decision #3 already excluded this name-list from Week 1, and Week 15 looks forward. Re-including it would revive a settled exclusion |
| A8 | Beck's work has been extended by current researchers and theorists, in the US and abroad | **Included** | The only primary-source support for the catalog's own objective „полето продължава да се развива" |
| A9 | A number of CBT forms share Beck's characteristics but vary in conceptualization and emphasis | **Included** | The framing sentence for the inventory |
| A10 | REBT (Ellis, 1962) | **Included — name only** | Week 2 owns REBT; cross-link, do not re-teach |
| A11 | **DBT (Linehan, 1993)** | **Included** | Third-wave member, primary-source cited |
| A12 | Problem-solving therapy (D'Zurilla & Nezu, 2006) | **Included — name only** | Unowned; no week teaches it |
| A13 | **ACT (Hayes, Follette, & Linehan, 2004)** | **Included** | Third-wave member, primary-source cited |
| A14 | Exposure therapy (Foa & Rothbaum, 1998) | **Included — name only** | Week 13 owns exposure boundaries; cross-link only |
| A15 | Cognitive processing therapy (Resick & Schnicke, 1993) | **Included — name only** | Unowned |
| A16 | CBASP (McCullough, 1999) | **Included — name only** | Unowned |
| A17 | Behavioral activation (Lewinsohn et al., 1980; Martell et al., 2001) | **Included — name only** | Week 7 owns BA; cross-link only |
| A18 | Cognitive behavior modification (Meichenbaum, 1977) | **Included — name only** | Unowned |
| A19 | Beck's CBT often incorporates techniques from all these therapies within a cognitive framework | **Included** | The integrative claim — genuine, source-grounded course integration (see §9) |
| A20 | Bibliography of historical reviews (Arnkoff & Glass 1992; A. Beck 2005; etc.) | Excluded | Reference apparatus, not learner-facing |

**Representation note for A10–A18:** this is the same shape as Week 14's ten-tool consolidation inventory —
each item named only, each cross-linked to its owning week where one exists. It must not become nine
mini-lessons.

### 2.2 Group B — reach and limits (B1–B6), SRC-041 printed p.3

| ID | KU | Status | Why |
|---|---|---|---|
| B1 | Adapted across education, income, culture and age — from young children to older adults | **Included** | Breadth of the field |
| B2 | Used in primary care, medical offices, schools, vocational programs, prisons | **Included** | Breadth of settings; sets up CT-R's settings |
| B3 | Used in group, couples and family formats | **Included** | Breadth of formats |
| B4 | The book's treatment is individual 45-minute sessions; treatment can be shorter | Deferred → Week 6 | Session structure is owned |
| B5 | **Some patients, such as those with schizophrenia, often cannot tolerate a full session** | **Included** | The genuine primary-source bridge to CT-R's population. Safety-sensitive — §5 |
| B6 | Some practitioners use CT techniques without a full session — in a medical/rehab encounter or medication review | **Included** | Bridges to CT-R's non-office delivery |

### 2.3 Group C — the third wave (C1–C11), SRC-025

| ID | KU | Status | Why |
|---|---|---|---|
| C1 | The "wave" framing itself | **Needs Review** | Owner decision #2 — rests on one weak source (§1.4) |
| C2 | First wave 1920s–1960s (behaviorism) | **Needs Review** | Owner decision #3 — GAP-011 constrains precise years |
| C3 | Second wave 1950s–1970s (cognitive therapy, classical CBT) | **Needs Review** | Owner decision #3 |
| C4 | Third wave 1980s–1990s onward | **Needs Review** | Owner decision #3 |
| C5 | **The shift: from the *content* of one's cognitions to one's *stance toward* one's cognitions** | **Included** | Matches the syllabus almost verbatim; the intellectual core of the week |
| C6 | Mindfulness and acceptance derive from Buddhist principles and shaped new CBT forms | **Included** | Academic, safety-neutral, and the honest origin of the third wave |
| C7 | DBT named as third wave | **Included** | Cross-validated by A11 (primary source) |
| C8 | **MBCT named as third wave** | **Needs Review** | Owner decision #4 — MBCT is named *only* by SRC-025. SRC-041: 0 hits. SRC-014 no longer names it. The weakest of the syllabus's three therapies |
| C9 | ACT named as third wave | **Included** | Cross-validated by A13 (primary source) |
| C10 | Compassion-focused therapy named as third wave | Excluded | Not in the syllabus; a fourth name without curriculum need |
| C11 | **Reviews reveal there may be no difference in effectiveness vs. non-third-wave CBT for depression** | **Included** | Strongly recommended. Without it the week reads as promotion; with it, it reads as academic |

### 2.4 Group D — CT-R (D1–D27), SRC-013

| ID | KU | Status | Why |
|---|---|---|---|
| D1 | CT-R promotes empowerment, recovery and resiliency in people with serious mental health conditions | **Included** | Definition |
| D2 | Based on Aaron Beck's cognitive model; embodies recovery-movement principles | **Included** | Anchors CT-R to the course's own model |
| D3 | Expands from CBT for psychosis (CBTp) | **Included** | Lineage |
| D4 | A strengths-based approach focused on activating adaptive modes of living | **Included** | The syllabus's „силните страни… изграждане на пълноценен живот" |
| D5 | Initially developed by Paul Grant and Aaron T. Beck | **Included** | Authorship |
| D6 | For people with serious mental health conditions, incl. mistrust, chronic institutionalization, limited motivation | **Included — reduced** | Keep at population level; safety §5 |
| D7 | Presentation checklist: distressing voices, aggression, self-injury, trauma impact, disagreement about diagnosis | **Excluded** | Reads as a symptom inventory; no learner-facing purpose; §5 |
| D8 | **The adaptive mode = the positive beliefs, emotion and actions that appear when a person is at their best** | **Included** | The week's key new concept |
| D9 | Stage 1 — Accessing and energizing the adaptive mode through shared activity; builds trust while avoiding activating discouraging beliefs | **Included** | Process step 1 |
| D10 | Stage 2 — Developing it through aspirations: a vivid image of the desired future; the meaning underneath the aspiration | **Included** | Process step 2 |
| D11 | Aspirations spur hope, elicit values, and orient daily action toward purpose | **Included** | The syllabus's „фокус върху ценностите" |
| D12 | Stage 3 — Actualizing it through positive action; strengthens beliefs about being capable | **Included** | Process step 3 |
| D13 | Stage 4 — Strengthening it: empowerment and resiliency; the adaptive mode becomes dominant | **Included** | Process step 4 |
| D14 | **Defeatist performance beliefs** as a named category linked to motivation difficulty | **Needs Review** | Owner decision #5 — category yes; the two verbatim specimen beliefs are the question |
| D15 | **Asocial beliefs** as a named category linked to social participation | **Needs Review** | Owner decision #5 |
| D16 | **CBTp is primarily focused on symptom reduction; CT-R is focused on identifying and attaining the person's desired life** | **Included** | The sharpest "how is this different" content in the week |
| D17 | Delivered in community teams, hospital, residential and outpatient settings, individually or in groups | **Included — reduced** | Keep the categories; drop the US agency names |
| D18 | Named US states and specific agency types (ACT teams, jail diversion, VA…) | Excluded | US service-system detail; not transferable to a Bulgarian academic audience |
| D19 | Grant & Beck (2009): defeatist beliefs mediate cognitive impairment, negative symptoms and functioning | **Included** | Evidence base |
| D20 | Grant & Beck (2010): asocial beliefs predict asocial behaviour in schizophrenia | **Included** | Evidence base |
| D21 | **Grant et al. (2012): RCT — CT-R improved community participation, motivation and positive symptoms more than treatment as usual** | **Included** | The strongest single evidence claim |
| D22 | Thomas et al. (2017): beliefs predict motivation and community participation | Deferred | Redundant with D19/D20; keeps the evidence strip readable |
| D23 | Grant et al. (2017): improvements maintained 6 months after treatment ended | **Included** | Durability |
| D24 | Grant et al. (2020): *Recovery-Oriented Cognitive Therapy for Serious Mental Health Conditions*, Guilford Press | **Included** | Also supports „полето продължава да се развива" with a real date |
| D25 | Gains were experienced regardless of duration of illness | **Included** | Directly supports D26 |
| D26 | "Recovery is possible for all, no matter how long… or how difficult these challenges appear" | **Included** | The recovery-movement claim, source-attributed |
| D27 | By focusing on personal vision rather than symptom reduction, CT-R empowers people to flourish | **Included** | Closing claim |

### 2.5 Group E — mindfulness ownership (E1–E2)

| ID | KU | Status | Why |
|---|---|---|---|
| E1 | Mindfulness techniques help patients nonjudgmentally observe and accept internal experience without evaluating or changing it (SRC-041 p.264) | Deferred → Week 13 | **Already Included there as C15-K55.** Cross-link only; do not re-teach |
| E2 | **Mindfulness/acceptance as an organizing principle of the newer approaches** (Week 13's C15-K56, left `Needs Review` with *"Week 15 has a competing ownership claim"*) | **Included** | **This genuinely resolves here** — see §10 |

### 2.6 Group F — therapist development (F1–F10), SRC-041 Гл. 21

Read in full (printed pp.358–360). **Recommended: none of it lands in Week 15.**

| ID | KU | Status | Why |
|---|---|---|---|
| F1 | Гл. 21 outlines the steps for initiating standard CBT practice | Excluded | Topic mismatch — clinician training, not new therapeutic directions |
| F2 | You should practise the basic techniques **on yourself** before using them with patients | Excluded | **Self-application protocol — §5.3** |
| F3 | Trying techniques yourself lets you correct application difficulties and identify obstacles from the patient role | Excluded | Same |
| F4 | Steps 1–8: monitor your own mood; record your own automatic thoughts; do a daily Thought Record; complete the Cognitive Conceptualization Diagram and a Core Belief Worksheet **on yourself** | Excluded | **The clearest excluded class on this platform — §5.3** |
| F5 | CBT therapists do not try to eliminate negative emotion; they reduce dysfunctional degrees of it | Deferred → Week 14 | Week 14 already Included the equivalent limit |
| F6 | Step 9: choose a simple first patient — unipolar depression or adjustment disorder, no Axis II | Excluded | Clinician-only; patient selection |
| F7 | Therapists trained in another modality often revert to prior skills that interfere with CBT | Excluded | Clinician-only |
| F8 | Step 10: written consent to record sessions; supervisor review is essential; Cognitive Therapy Rating Scale | Excluded | Clinician-only; supervision |
| F9 | Steps 11–15: keep reading; bibliotherapy; Wright/Basco/Thase (2006); watch master therapists; seek Beck Institute training; join the Academy of Cognitive Therapy | Excluded | Clinician-only; professional-body guidance |
| F10 | **C15-K05** — therapists develop their own techniques as they become more skilled (deferred from Week 13 → "Week 14/15") | **Deferred — remains unassigned** | **Does not fit. See §10** |

---

## 3. Terminology Map (locks proposed)

| Concept | Variants seen | Proposed lock |
|---|---|---|
| Third wave | „трета вълна", „третата вълна" | **„трета вълна"** — lowercase, in quotes on first use, signalling a field label rather than a formal taxonomy |
| CT-R | „възстановително-ориентирана когнитивна терапия" (syllabus), „възстановително-ориентирана терапия" (catalog title) | **„възстановително-ориентирана когнитивна терапия (CT-R)"** in body; the catalog's shorter title stays as-is |
| Adaptive mode | „адаптивен режим" (syllabus: „Адаптивния режим") | **„адаптивен режим"** |
| Aspirations | „стремежи", „аспирации" | **„стремежи"** — „аспирации" is a calque |
| ACT | „терапия за приемане и ангажиране" (SRC-041 p.2) | **„терапия на приемане и ангажираност (ACT)"** — but see note |
| DBT | „диалектическа поведенческа терапия" (SRC-041 p.2) | **„диалектическа поведенческа терапия (DBT)"** |
| MBCT | — (no Bulgarian form in any reviewed source) | **Pending owner decision #4**; if included, „когнитивна терапия, базирана на осъзнатост (MBCT)" needs an owner-approved Bulgarian form |
| Mindfulness | „осъзнатост" (SRC-041 p.256), „внимателност" (p.264) | **„осъзнатост"** — already the Week 13 lock; keep it identical |
| Recovery | „възстановяване" | **„възстановяване"** — note Week 14 already uses „възстановяване" for the *recovery curve* (Figure 18.1); §4 flags the collision |
| Serious mental health conditions | „тежки психиатрични състояния" (syllabus), „тежки психични състояния" (register) | **„тежки психични състояния"** — „психични" is the current standard term |

**Terminology conflict to resolve (owner decision #6):** the syllabus writes ACT as „Терапия на приемане и
ангажираност"; SRC-041 p.2 writes „терапия за приемане и ангажиране". The primary source and the syllabus
disagree. Recommend following the **syllabus** form for the learner-facing name (it is the more current
Bulgarian rendering) while citing SRC-041's own wording in the source reference — the same treatment Week 5
gave to „колаборативен емпиризъм" vs. Гл. 1's „съвместен емпиризъм".

---

## 4. Overlap audit against every implemented week

| Week | Status | Overlap with Week 15 | Boundary |
|---|---|---|---|
| **1** — history | LOCKED | **Direct.** Week 15's S1/S2 are Гл. 1 pp.2–3, the same chapter Week 1 owns | **Hard boundary.** Week 1 owns the *origin narrative* (dream study, 1977 RCT, 1979 manual). Week 15 takes only U7's family list (owner-Excluded from Week 1) plus U6/U8's reach material (Deferred there). Week 15 must not retell Beck's origin — recap + cross-link only |
| **2** — Beck vs. Ellis | LOCKED | **Moderate.** A10 names REBT (Ellis, 1962) | Name only, cross-linked. Week 15 must not re-run the Beck/Ellis comparison |
| **3** — cognitive architecture | LOCKED | Low. A4–A6 touch conceptualization | All Deferred back to Week 3. Week 15 does not re-teach the hierarchy |
| **4** — assessment/conceptualization | NOT LOCKED | Low. A4/A5 | Deferred. **Week 4 must not be touched** |
| **5** — principles/alliance | NOT LOCKED | Low | None material. **Week 5 must not be touched** |
| **6** — session structure | LOCKED | Low. B4 (45-minute sessions) | Deferred → Week 6 |
| **7** — behavioral activation | LOCKED | **Moderate.** A17 names behavioral activation with its own citations | Name only, cross-linked. Week 15 must not re-teach BA |
| **8** — automatic thoughts/emotions | LOCKED | Low | None material |
| **9** — distortions/Thought Record | LOCKED | Low | None material. Note F4 would have re-introduced the Thought Record as a self-tool — Excluded |
| **10** — Socratic evaluation | LOCKED | **Conceptual.** C5's "change your stance toward thoughts, not their content" is a genuine *contrast* to Week 10's evaluate-the-thought method | **Opportunity, not a conflict** — but it must be framed as *a different orientation*, never as "Week 10's method was wrong". See §5 |
| **11** — intermediate beliefs | LOCKED | Low. D14/D15 are belief categories | Different population and construct; cross-link at most |
| **12** — core beliefs/schemas | LOCKED | **Moderate.** D14 (defeatist performance beliefs) and D15 (asocial beliefs) are core-belief-shaped | Cross-link to Week 12's three categories; Week 15 must not re-teach core-belief modification |
| **13** — additional techniques | LOCKED | **Direct.** C15-K55/K56 (mindfulness), A14 (exposure) | Week 13 keeps the *technique-level* mindfulness definition (E1). Week 15 takes only the *principle-level* claim (E2). Exposure: name only |
| **14** — homework/termination | LOCKED | **Terminology collision.** Week 14 uses „възстановяване" for Figure 18.1's recovery curve; Week 15 uses it for the recovery movement | Not a content overlap, but the word carries two different meanings across adjacent weeks. Week 15 must make the sense explicit on first use („възстановяване" като движение/ориентация, не кривата на подобрението от Седмица 14) |

### 4.1 Unique Week 15 ownership — what no other week can claim

1. **That CBT is a family of therapies, not one method** — A9/A19, with the primary source's own citation list.
2. **That the theoretical assumptions stayed constant while everything else adapted** — A3. No other week states this.
3. **The content→stance shift** — C5. The single idea that distinguishes the newer approaches.
4. **CT-R in full** — D1–D27. Entirely unclaimed by any week; an entire treatment approach the course otherwise never mentions.
5. **The adaptive mode** — D8. A concept with no counterpart anywhere in the course.
6. **Severe mental illness as a CBT population** — B5 + D6. Weeks 1–14 treat unipolar depression and anxiety; psychosis appears nowhere else as a learner-facing topic.
7. **The honest efficacy caveat** — C11. The only place the course says a newer approach may not outperform what came before.

---

## 5. Safety-sensitive material

Week 15's live classification is **`AcademicContextOnly`**, and **§1's evidence supports keeping it
exactly as it is.** Recommendation: **preserve, do not change.** Routed, it resolves to `AcademicOverview`
— the Weeks 4 and 12 treatment.

### 5.1 Genuinely sensitive content in the Included set

| Item | Why sensitive | Required handling |
|---|---|---|
| B5, D6 | **Severe mental illness / psychosis** — a population the course has never addressed | Third-person, descriptive, professional-context only. Never "if you experience…". No prevalence, no diagnostic criteria, no self-recognition framing |
| D8–D13 | The four CT-R stages are a **treatment process** | Descriptive process of what a clinician and team do — never a path the learner could follow. Same posture as Week 14's §08 taper ladder |
| D14, D15 | Named belief categories for a psychotic-spectrum population | **Owner decision #5.** Recommend: name the categories and their function; exclude the four first-person specimen beliefs, which invite self-application |
| C6 | Buddhist origin of mindfulness/acceptance | Academic attribution only. No practice, no instruction, no exercise |
| C11 | An efficacy caveat about real treatments | State it as the source states it — a review-level finding about depression, not a verdict on any therapy |

### 5.2 Excluded outright — settled precedent, no owner decision needed

- **No mindfulness practice, exercise, script or induction** — Week 13's precedent (C15-K44 AWARE excluded) holds unchanged.
- **No self-assessment, self-rating or self-input of any kind** — platform-wide.
- **No clinical instrument reproduced** — the Cognitive Therapy Rating Scale (F8) is excluded with the rest of Гл. 21.
- **No Sally.** Sally is an 18-year-old with moderate unipolar depression and no Axis II diagnosis. Extending her into psychosis or CT-R would be **inventing case history** — forbidden by `AGENTS.md`. Week 15 has no case material in any source and must have none.
- **No reproduced dialogue** — none exists in these sources anyway.

### 5.3 The Гл. 21 safety finding

This is worth stating plainly because it settles a deferred item that has been drifting since Week 13.

**SRC-041 Chapter 21 is a self-application protocol.** Its steps 1–8 instruct the reader to monitor their
own mood, record their own automatic thoughts, complete **one Thought Record a day**, fill in both halves
of the **Cognitive Conceptualization Diagram on themselves**, and complete a **Core Belief Worksheet on
themselves** (printed pp.358–359). Steps 9–15 then cover selecting a first patient, recording sessions,
and supervision.

That is written for a clinician in training. On this platform — whose learners are psychology students,
not supervised clinicians, and whose every week so far has excluded self-input tools — importing Гл. 21
would hand the learner the most complete "do CBT on yourself" instruction set in the entire book. It is
the clearest excluded class the project has.

**Conclusion: Гл. 21 has no owner and should not acquire one by default.** C15-K05 must not be routed to
Week 15 merely because Week 15 is last.

---

## 6. Representation Fit

Evaluated against the source, not against the Week 6 template.

| Representation | Verdict | Reasoning |
|---|---|---|
| **Weekly Mind Map** | **YES — if routed** | Required by the platform standard for any routed week. The content has a clean single-parent shape: 6 branches (Семейството на КПТ · Обхват и граници · „Трета вълна" · Три представителя · CT-R и адаптивният режим · Какво показват изследванията). Same semantic model in Preview and Review, collapsed by default |
| **Concept Map** | **NO** | Do not force one. Week 15 is two parallel inventories plus one linear process. There is no multi-parent relation network with source-grounded labels — inventing edges between ACT, DBT and CT-R would be exactly the "infer a relation because it seems plausible" failure `AGENTS.md` forbids |
| **Process visualization** | **YES** | D9–D13's four CT-R stages are a genuinely ordered process with source-named steps. Reuse `.guided-practice-sequence` (Weeks 5/14 precedent), descriptive and third-person |
| **Comparison** | **YES — two** | (1) **CT-R vs. CBTp** (D16) — the source states both sides explicitly. (2) **Content of thoughts vs. stance toward thoughts** (C5) — a two-column contrast that also cross-links Week 10. Reuse `.category-compare`/`.comparison-matrix`; apply Week 14's §13 mobile stacking from the start |
| **Fixed worked example** | **NO** | No case material exists in any Week 15 source. Sally is excluded (§5.2). Do not invent one |
| **Interactive element / simulator** | **NO** | `AcademicOnly` format, `AcademicContextOnly` safety. Weeks 4 and 12 set the precedent: routed academic weeks carry zero interactivity beyond the shared components |
| **FinalAssessment** | **YES** | Shared component, 8 questions, platform standard. Every routed week has it |
| **Evidence strip** | **YES** | D19–D25 is a compact, dated citation list — the right representation for an academic week, and the course's only one |

---

## 7. Retrieval, application and example opportunities

**Retrieval (source-grounded, no new facts):**
- Why did the theoretical assumptions stay constant while techniques changed? (A3)
- What exactly shifted in the third wave — the thoughts, or the person's relation to them? (C5)
- What is the adaptive mode, and when is it visible? (D8)
- What separates CT-R from CBTp in one sentence? (D16)
- What did the 2012 RCT actually show? (D21)

**Application-level (recognition, not practice):** given a short descriptive vignette of a clinician's
*orientation* — "evaluating the evidence for a thought" vs. "helping someone notice a thought without
acting on it" — identify which wave's assumption is at work. This is recognition of a stated distinction
(C5), not clinical judgment, and requires no new content.

**Source-grounded examples available:** SRC-013's aspiration examples (the nursing aspiration whose
underlying meaning is "to help people and make the world a better place"; the partnership aspiration
meaning "to share good experiences and have mutual support") are the **only** worked examples in the
entire Week 15 source set. They are non-clinical, positively valenced, and safe. Recommend using them —
and nothing else — to illustrate D10/D11.

---

## 8. Proposed structure (14 sections)

| § | Section | KUs |
|---|---|---|
| 01 | Накратко — Mind Map preview | — |
| 02 | Едно семейство, не един метод | A2, A3, A8, A9, A19 + A10–A18 inventory |
| 03 | Докъде стигна КПТ | B1, B2, B3, B5, B6 |
| 04 | „Трета вълна": от съдържанието към отношението | C1–C6 *(pending decisions #2–#3)* |
| 05 | Три представителя | C7, C8 *(pending #4)*, C9 + cross-refs to A11/A13 |
| 06 | Какво показват изследванията за третата вълна | C11 |
| 07 | CT-R: възстановително-ориентирана когнитивна терапия | D1–D6, D16 |
| 08 | Адаптивният режим | D8 + D9–D13 as a four-step process |
| 09 | Стремежи, ценности и значение | D10, D11 + the two source aspiration examples |
| 10 | CT-R срещу CBTp | D16, D17, D26, D27 comparison |
| 11 | Доказателствената база | D19–D21, D23, D24, D25 |
| 12 | Границите на този курс | B5, D6 + the `AcademicContextOnly` boundary |
| 13 | Седмичен преглед — Mind Map review | — |
| 14 | Финална проверка + Източници | shared `FinalAssessment` (8 q.) |

---

## 9. Course closure — what the source actually supports

Instruction: *do not invent a "course finale" narrative if the source does not support it.* Assessed
honestly, item by item:

| Asked | Supported? | Detail |
|---|---|---|
| **Course consolidation** | **NO** | No Week 15 source consolidates the course. **Week 14 already did this** — its Гл. 18 ten-tool inventory is the course's real consolidation moment. Duplicating it here would be invention *and* a Week 14 overlap |
| **Professional-development framing** | **NO — not safely** | Supported only by Гл. 21, which is topic-mismatched and is a self-application protocol (§5.3). Do not build this |
| **Integration of prior CBT skills** | **YES — genuinely** | A19: Beck's CBT *"често включва техники от всички тези терапии… в когнитивна рамка"*, plus the cross-linked family inventory (A10/A14/A17 point back at Weeks 2/13/7). This is real, source-grounded integration by reference — the only kind available |
| **Boundaries of learner competence** | **YES** | B5 (some patients cannot tolerate a full session), D6 (serious mental health conditions), and the `AcademicContextOnly` tier itself. §12 of the structure is built on these and invents nothing |
| **Next-step / continuing-learning framing** | **YES — but thin** | A8 (Beck's work extended by current researchers, in the US and abroad) and D24 (a 2020 book) support "the field continues" with real citations. Enough for one honest closing paragraph; **not** enough for a "your journey continues" section |

**Recommendation: no finale.** Close on §12's competence boundary plus one sourced sentence from A8/D24.
Anything warmer would be written by us, not by the sources.

---

## 10. Previously deferred / unassigned items — what genuinely resolves here

| Item | Origin | Verdict |
|---|---|---|
| **Mindfulness ownership** (C15-K56) | Week 13, `Needs Review` — *"Week 15 (third wave) has a competing ownership claim"* | **RESOLVES HERE — genuinely.** Not because Week 15 is last, but because C6/C8/C9 make mindfulness and acceptance the *organizing principle* of the third wave. **Split by level:** Week 13 keeps the technique-level definition (C15-K55/E1, already Included, LOCKED, untouched); Week 15 takes only the principle-level claim (E2). No Week 13 change required |
| **C15-K05** — therapists develop their own techniques as they become more skilled | Week 13 → "Week 14/15"; Week 14 declined it (`WEEK_14_SOURCE_AUDIT_v1.md:468`) | **DOES NOT RESOLVE HERE. Remains Deferred/Unassigned.** Its real home is Гл. 21, which is topic-mismatched and safety-problematic (§5.3). Гл. 21 has no owner. Assigning it to Week 15 would be exactly the "final week absorbs the leftovers" move the audit instruction forbids |
| **GAP-014** — Гл. 2's „подчертаване на положителното" | Week 5 → open; Week 14 declined the remaining half | **DOES NOT RESOLVE HERE.** No Week 15 source touches it. Its natural home is Гл. 2 (Week 5's chapter) or Week 13's credit-list territory. **Stays Open/Unassigned** |
| **GAP-011** — three-wave dates + CT-R sourcing | Roadmap/gaps, `Constrained / Open-needs-new-source` | **PARTIALLY RESOLVES.** The **CT-R half should be closed** against SRC-013 (§1.5) — the "no reviewed source" claim is wrong. The **wave half gets worse, not better** (§1.4): its evidentiary base has fallen from three sources to one. Requires owner decisions #2–#4 |
| **Гл. 16 (Образи), Гл. 19 (Планиране на лечението), Гл. 20 (Проблеми в терапията), Гл. 21** | Various weeks deferred imagery and treatment planning to "unassigned Гл. 16/19/20/21" | **NONE resolve here** — none is Week 15's topic. Four SRC-041 chapters end the course with **no curriculum owner**. Flagged for owner awareness; not a Week 15 problem to solve |

---

## 11. Owner decisions required before implementation

Genuine decisions only — everything settled by existing precedent is already applied above.

1. **Route Week 15, or leave it unrouted?** Routing it makes it the 12th routed week, resolving to
   `AcademicOverview` (the Weeks 4/12 treatment), and triggers the Weekly Mind Map + shared
   `FinalAssessment` requirements. Leaving it unrouted keeps the course at 11 routed weeks with a
   catalogued-only finish. *Recommendation: route it — the source scope (76 KUs, 47 Included) is
   substantial enough to justify a real page.*

2. **The "wave" framing (C1).** It now rests on **one** live reviewed source, and it is Wikipedia (§1.4).
   Options: (a) use it, attributed as a field label rather than a formal taxonomy; (b) drop the wave
   vocabulary and present „по-нови направления в КПТ" using SRC-041's own family framing (A9), which is
   citation-grade. *Recommendation: (a), with the framing explicitly attributed and hedged* — the syllabus
   names it, and C5's shift is the real content either way.

3. **The wave decade ranges (C2–C4).** GAP-011's standing rule is: never cite the prototype's exact years;
   use ranges compatible with reviewed sources, or omit disputed precise years entirely. SRC-025's ranges
   (1920s–1960s / 1950s–1970s / 1980s–1990s onward) *are* from a reviewed source and are overlapping rather
   than crisp. *Recommendation: omit the decades.* They add no learning value, and the overlap between the
   first two ranges will read as an error to a student.

4. **MBCT (C8).** Named only by SRC-025. SRC-041: zero hits. SRC-014 no longer names it. The syllabus asks
   for ACT/DBT/MBCT. *Recommendation: include MBCT by name only, explicitly on the weaker source, or drop
   to ACT + DBT which SRC-041 itself cites with founders and years.* Owner's call — this is the one place
   the syllabus and the available sourcing genuinely diverge.

5. **CT-R belief categories (D14, D15).** Name the two categories and their function (recommended), or also
   reproduce the four first-person specimen beliefs from SRC-013. *Recommendation: categories only* —
   consistent with Week 11's exclusion of the Dysfunctional Attitude Scale and Personality Belief
   Questionnaire items.

6. **ACT's Bulgarian name (§3).** The syllabus („терапия на приемане и ангажираност") and SRC-041 p.2
   („терапия за приемане и ангажиране") disagree. *Recommendation: follow the syllabus in body text, cite
   SRC-041's wording in the source reference* — the Week 5 „колаборативен емпиризъм" precedent.

7. **Mindfulness ownership split (E2).** Confirm Week 15 takes the principle-level claim while Week 13
   (LOCKED) keeps the technique-level definition untouched. *Recommendation: approve as described in §10.*

8. **C15-K05 and Гл. 21.** Confirm C15-K05 stays **Deferred/Unassigned** and that Гл. 21 is not imported
   into Week 15. Separately: decide whether the course ever covers therapist training — noting §5.3's
   finding that Гл. 21 is a self-application protocol and would need heavy constraint in any week.
   *Recommendation: confirm unassigned; treat therapist training as a separate future scope question.*

9. **Register and roadmap corrections (documentation only, no content effect).** Four records are now
   demonstrably stale: SRC-014's and SRC-023's register rows (§1.4); GAP-011's CT-R half (§1.5); and
   `_source_corpus/README_EXTRACTION_METHOD.md`'s source path, which points at `C:\Users\deian\…` while the
   file actually lives under `C:\Users\deianarie306\…`. *Recommendation: authorize these corrections as part
   of the Week 15 implementation cycle rather than as a separate housekeeping pass.*

---

## 12. Return summary

- **Canonical Week 15:** „Трета вълна и възстановително-ориентирана терапия", Module IV („Разширени
  техники и академичен контекст").
- **Safety classification:** `AcademicContextOnly` — **preserve unchanged**; resolves to `AcademicOverview`
  if routed (Weeks 4/12 precedent). Declared format `AcademicOnly` is correct — no contradiction to fix.
- **Exact source scope:** SRC-041 Гл. 1, printed **pp.2–3** (PDF pp.24–25) · **SRC-013** (Beck Institute,
  CT-R) · **SRC-025** (Wikipedia, CBT third-wave section) · SRC-041 Гл. 15 p.264 cross-link only.
  **No SRC-041 chapter covers this week** — verified by full-text scan of all 414 pages.
- **KU accounting:** **76 total = 47 Included / 9 Deferred / 13 Excluded / 7 Needs Review / 0 Unaccounted.**
- **Unique ownership:** CBT as a family rather than a method; constancy of theoretical assumptions;
  the content→stance shift; CT-R in full; the adaptive mode; severe mental illness as a CBT population;
  the honest efficacy caveat.
- **Proposed structure:** 14 sections (§8).
- **Recommended representations:** Weekly Mind Map (6 branches, Preview + Review) · four-step CT-R process
  visualization · two comparisons · evidence strip · shared `FinalAssessment` (8 q.). **No Concept Map, no
  simulator, no worked case, no Sally.**
- **Resolves here:** mindfulness ownership (C15-K56), at principle level only, with Week 13 untouched.
- **Stays unassigned:** C15-K05 / Гл. 21 · GAP-014's remaining half · Гл. 16, 19, 20, 21 (four chapters
  end the course with no curriculum owner).
- **Safety-sensitive:** psychosis/severe mental illness as a population (B5, D6) · the CT-R treatment
  process (D8–D13) · CT-R belief categories (D14, D15) · Buddhist origins (C6) · the efficacy caveat (C11).
  Гл. 21 excluded as a self-application protocol.
- **Owner decisions required:** nine (§11) — three of them created by the source situation in §1.

**STOP — awaiting owner review. No implementation, no commit, no push.**
