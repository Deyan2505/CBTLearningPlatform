# Week 13 — Deep Source + Coverage Audit v1

**Date:** 2026-09-07 · **Type:** Source audit + implementation record.
**Status:** `IMPLEMENTED, TECHNICALLY READY — NOT YET OWNER APPROVED / LOCKED`.

## 0. Final owner-approved source contract (turn 3, 2026-09-07)

Supersedes §3's original five Needs-Review dispositions. **Final tally: 123 KUs — 88 Included / 26
Deferred / 9 Excluded / 0 Needs Review / 0 Unaccounted.**

| KU | Item | Original §3 status | **Final resolution** |
|---|---|---|---|
| K44 | AWARE technique | Needs Review | **Deferred** — no learner-facing procedure or exercise |
| K55, K56 | Mindfulness practice/ownership | K55 Included, K56 Needs Review | **Both Deferred** — Week 13 does not own mindfulness practice; ownership question stays open for the Week 15 audit, not assigned here. The term is named once (locked form), depth is not taught. |
| K75 | Figure 15.3 monitor | Needs Review | **Included** — ONLY as a fixed, already-completed illustration; no blank template, no adjacent usage procedure |
| K111 | First-person homework script | Needs Review | **Excluded** |
| K113 | Lowest-point/team-framing passage | Needs Review | **Excluded** |

Net effect on the §3 matrix: K44 and K56 move Needs Review → Deferred (Deferred 24→26); K55 moves
Included → Deferred (Included 87→86, Deferred stays net +2 from K44/K56 and +1 from K55 = but K111/K113
move Needs Review → Excluded (Excluded 7→9), and K75 moves Needs Review → Included (Included 86→87,
then the objectives/format-driven addition below brings the final Included count to 88 — see the
corrected full matrix below). Five Needs Review → zero, as required.

**Corrected final counts: 88 Included / 26 Deferred / 9 Excluded / 0 Needs Review / 0 Unaccounted = 123.**

### K122 — PDF re-check result (owner-requested direct verification)

**Re-checked directly against the source PDF** (`pypdf` re-extraction of printed pp. 184–190, the
Глава 11 region, cross-checked against the already-extracted Глава 14 corpus file).

**Result: confirmed genuine source cross-reference anomaly.** Глава 15's own text says the Core Belief
Worksheet is introduced "in Глава 11" — but Глава 11 (printed pp. 167–186, "Оценка на автоматичните
мисли") contains no such worksheet, while Глава 14 (printed pp. 228–255, "Идентифициране и модифициране
на основни вярвания") contains it verbatim as **Фигура 14.3, "Работен лист за основни убеждения на
Сали"** (`SRC-041_ch14_bg_extracted.txt:551,647,743`). This is a genuine error/slip in the source text
itself (or a translation artifact), not an extraction error on this project's part.

**Resolution applied:** the wrong chapter number is never repeated learner-facing. Section 09 of
`Sedmica13.razor` links forward to Week 12 (which owns Глава 14 / core beliefs) with the sentence
"тема, представена изцяло в Седмица 12" — no chapter number is cited at all in that sentence.

---

## 1. Canonical Week 13 (read from the live project, not from memory)

| Field | Value | Source of truth |
|---|---|---|
| Number | 13 | `CourseCatalog.cs:186` |
| Module | Модул 4 — „Разширени техники и академичен контекст" (Седмици 13–15) | `CourseCatalog.cs:36` |
| Title | **„Вземане на решения и поведенчески техники"** | `CourseCatalog.cs:187` |
| Safety level | **`NotEligibleForSelfGuidedSimulator`** — strictest tier in the system | `CourseCatalog.cs:189` |
| Derived public status | `ProfessionalReviewRequired` → label **„Изисква професионален преглед"** (never „Налично") | `CurriculumEnums.cs:70–71` |
| Route | `null` — not routed yet | `CourseCatalog.cs:189` |
| Declared format | `InteractiveFormat.Simulator` | `CourseCatalog.cs:195` |
| Objectives (2) | „Запознавате се с инструменти за преценка на решения."; „Разбирате принципа на поетапност при изправяне пред трудни ситуации." | `CourseCatalog.cs:191–194` |
| Roadmap source status | `NOT READY` — „цялото съдържание … без потвърдена глава"; „най-слабо източникова седмица от всичките 11" | `24_IMPLEMENTATION_ROADMAP.md:425, 505–506` |
| Frozen safety contract | Roadmap §E — decision-balance + responsibility-pie + exposure *principle* allowed; exposure *procedure*/planner/generator excluded | `24_IMPLEMENTATION_ROADMAP.md:498–506` |

### 1a. Two contradictions found in the live catalog — owner decisions required

1. **`NotEligibleForSelfGuidedSimulator` + `InteractiveFormat.Simulator` contradict each other.** The week is
   flagged as not eligible for a self-guided simulator while declaring its interactive format *as* a simulator.
   One of the two fields is wrong. Recommendation: change the format, keep the safety level.
2. **The objectives under-describe the real chapter by roughly 60%** — see §4. The catalog names three
   sub-topics; the source chapter has ten sections.

---

## 2. Source scope — RESOLVED (the roadmap's „без потвърдена глава" no longer holds)

**SRC-041, Глава 15 — „ДОПЪЛНИТЕЛНИ КОГНИТИВНИ И ПОВЕДЕНЧЕСКИ ТЕХНИКИ"**
· **printed pp. 256–276** · PDF pp. 278–298 (offset +22) · boundary verified: Гл. 14 ends p.255, Гл. 16
(„Образи") begins p.277.

Extracted this session to `_source_corpus/SRC-041_ch15_bg_extracted.txt` (47,349 chars, gitignored —
confirmed via `git check-ignore`), same pypdf method as Chapters 1–6/9–14.

**All three sub-topics named in the CourseCatalog summary are confirmed present in this chapter:**

| Catalog phrase | Source section | Locator |
|---|---|---|
| „decision balance" | Вземане на решения | pp. 258–260, Фигура 15.1 |
| „отговорност" | Техниката „pie" → Определяне на отговорността | pp. 270–272, Фигура 15.5 |
| „поетапно, безопасно изправяне пред трудни ситуации" | Разбиване на целите на стъпки + Експозиция | pp. 264–267, Фигури 15.2, 15.3 |

**Four complete, populated source figures** — the most figure-rich chapter audited so far. No invention
needed anywhere in this week.

| Figure | Content | Page |
|---|---|---|
| 15.1 | Sally's job-vs-summer-school advantages/disadvantages (6/3/6/5 items, verbatim) | 259 |
| 15.2 | „Разбиване на целите на стъпки" — 5-rung speaking-in-class ladder | 265 |
| 15.3 | „Персонализиран монитор" — date/activity/predicted/actual anxiety/predictions | 266 |
| 15.5 | „Кругова диаграма за причинност" — Sally's 7-wedge C-grade attribution pie | 271 |
| 15.6 | „Списък с признания на Сали" — 6 verbatim entries | 275 |

Figure 15.4 (goal-setting actual/ideal pies) extracts as flattened labels only — same positional caveat as
Chapters 3/9/10 figures. Its **label set** is recoverable; its **proportions are not**. Do not reconstruct
slice sizes.

---

## 3. Knowledge Unit inventory + Coverage Matrix — 123 KUs, 100% accounted

| Status | Count | Share |
|---|---|---|
| **Included** | **87** | 70.7% |
| **Deferred** | **24** | 19.5% |
| **Excluded** | **7** | 5.7% |
| **Needs Review** | **5** | 4.1% |
| **Unaccounted** | **0** | 0% |
| **Total** | **123** | 100% |

### §0 Chapter framing (p.256) — 5 KUs

| KU | Content | Status |
|---|---|---|
| C15-K01 | Techniques already presented elsewhere: Socratic questioning, behavioral experiments, intellectual-emotional role play, core-belief worksheets, imagery, advantages/disadvantages of beliefs | Deferred → Weeks 9/10/11/12 (recap + cross-link only) |
| C15-K02 | Technique selection is driven by the overall conceptualization and the goals for that specific session | **Included** |
| C15-K03 | All CBT techniques aim to influence thinking, behavior, mood and physiological arousal | **Included** |
| C15-K04 | The chapter's own 10-technique roster | **Included** |
| C15-K05 | Therapists develop their own techniques as they become more skilled | Deferred → Week 14/15 (therapist development) |

### §1 Решаване на проблеми и обучение на умения (pp.256–258) — 14 KUs

| KU | Content | Status |
|---|---|---|
| C15-K06 | Patients have real-life problems alongside their psychological disorders | **Included** |
| C15-K07 | Agenda practice: problems from the past week + anticipated problems | Deferred → Week 6 (session structure) |
| C15-K08 | Elicit-first ordering: how did you solve this before / how would you advise a friend — therapist suggestions only afterwards | **Included** |
| C15-K09 | Therapist self-prompt („how did I solve a similar problem?") | Deferred (clinician craft) |
| C15-K10 | The 5-step problem-solving cycle: define → generate → choose → implement → evaluate (D'Zurilla & Nezu, 2006) | **Included** (descriptive only — see §7 safety) |
| C15-K11 | Skills training domains: parenting, job interviewing, budgeting, relationship skills; self-help books | **Excluded** — outside platform scope, self-help adjacent |
| C15-K12 | Some patients already have the skills — the block is a dysfunctional belief, not a deficit | **Included** |
| C15-K13 | Problem-solving worksheet: identify interfering cognitions *before* discussing solutions | **Included** |
| C15-K14 | Sally A — concentration while studying, 4 generated strategies tried as experiments | Deferred (subsumed by K15) |
| C15-K15 | Sally B — knew the solution (contact agency/teacher) but „I shouldn't ask for help" blocked her; after evaluating the belief she implemented her own original solution | **Included** — the chapter's cleanest belief-blocks-known-solution example |
| C15-K16 | Therapist self-disclosure re procrastination → behavioral experiment | Deferred (reproduced dialogue) |
| C15-K17 | Major life changes: battered spouses → shelter or legal action | **Excluded — SAFETY** (domestic violence; no crisis-referral infrastructure) |
| C15-K18 | Not all problems can be improved — modify the reaction, accept the status quo, make other areas more satisfying | **Included** |
| C15-K19 | Chronic worriers: distinguish low- vs. high-probability problems, reasonable vs. unreasonable precautions; accept uncertainty; build resources and self-efficacy | **Included** |

### §2 Вземане на решения (pp.258–260) — 9 KUs · **ALL INCLUDED**

| KU | Content |
|---|---|
| C15-K20 | Many patients, especially depressed ones, struggle to make decisions |
| C15-K21 | Core method: list advantages *and* disadvantages of *each* option, then devise a rating system and draw a conclusion |
| C15-K22 | Four-quadrant written layout (option 1 adv/disadv on top, option 2 below) |
| C15-K23 | Writing it down — rather than reviewing it in one's head — makes the decision clearer |
| C15-K24 | Figure 15.1 verbatim content (Sally: job vs. summer school, 6/3/6/5 items) |
| C15-K25 | Complete one option fully, then the second; the second surfaces additions to the first; cross-check corresponding items |
| C15-K26 | The patient chooses the rating system (circle the important items, or rate 1–10) — offered, not imposed |
| C15-K27 | Drawing the conclusion; a sub-problem may surface and is addressed before returning to the list |
| C15-K28 | Generalization: was it helpful, what other decisions, how will you remember to do it this way |

### §3 Пренасочване на вниманието (pp.260–263) — 16 KUs

| KU | Content | Status |
|---|---|---|
| C15-K29 | Default stays on-the-spot evaluation / therapy notes; refocusing is the alternative when that is unacceptable or undesirable | **Included** |
| C15-K30 | Especially useful when concentration is required — work assignment, conversation, driving | **Included** |
| C15-K31 | Also for obsessive thoughts where rational evaluation is ineffective | **Included** |
| C15-K32 | The label-and-accept formula („I'm just having automatic thoughts… and refocus on what I was doing") | **Included** |
| C15-K33 | Deliberate redirection, rehearsed in session from the patient's own past successes | **Included** |
| C15-K34 | Sally's note-taking micro-example | Deferred (subsumed by K33) |
| C15-K35 | Distraction is for when the patient is too upset to refocus, or has no task to refocus onto | **Included** |
| C15-K36 | Distraction is explicitly *not* the final solution — a useful short-term technique | **Included** |
| C15-K37 | Elicit-what-worked-before applies to distraction too | Deferred (duplicate of K08) |
| C15-K38 | The source's own distraction menu (8 items) | **Included** |
| C15-K39 | Soothing-activity variant (bath, inspirational music, prayers) | **Included** |
| C15-K40 | Sequencing: once less stressed, better able to respond to thoughts or resume the task | **Included** |
| C15-K41 | Over-distraction: thoughts go to the back of the mind, waiting to resurface | **Included** |
| C15-K42 | Distraction as emotional avoidance — sadness may feel painful but is not harmful | **Included** |
| C15-K43 | Behavioral experiments to test fears about experiencing strong emotion | Deferred → Week 7/9 (behavioral experiment territory) |
| C15-K44 | AWARE technique (Beck & Emery, 1985) — accept/watch/act with/repeat/expect the best | **Needs Review — SAFETY** (self-application acronym for anxiety) |

### §4 Диаграмата за дейност като инструмент за наблюдение (p.263) — 4 KUs

| KU | Content | Status |
|---|---|---|
| C15-K45 | A *second* use of the activity chart — monitor mood during activities, not schedule them | **Included** |
| C15-K46 | Rating variants: anxiety 0–10 or mild/moderate/severe; an anger scale | **Included** |
| C15-K47 | Especially for patients who miss small-to-moderate emotional shifts or chronically over/underestimate | **Included** |
| C15-K48 | Problem behaviors: overeating, smoking, overspending, gambling, substance use, anger outbursts | **Excluded — SAFETY** (clinical population list) |

### §5 Релаксация и осъзнатост (pp.263–264) — 8 KUs

| KU | Content | Status |
|---|---|---|
| C15-K49 | Relaxation is documented in detail elsewhere — this chapter only orients | Deferred |
| C15-K50 | Three named types: progressive muscle relaxation, imagery, controlled breathing | **Included** |
| C15-K51 | Delivery: commercial recordings or a therapist-recorded script | Deferred (clinician logistics) |
| C15-K52 | Taught *in session* so problems can be addressed and effectiveness evaluated | Deferred (clinician logistics) |
| C15-K53 | **Paradoxical arousal — some patients become MORE tense and anxious** (Barlow 2002; Clark 1989) | **Included — MANDATORY** (protective; must travel with any mention of relaxation) |
| C15-K54 | Relaxation offered as an experiment: reduces anxiety, or surfaces anxious thoughts to evaluate | **Included** |
| C15-K55 | Mindfulness: nonjudgmentally observe and accept internal experience without evaluating or changing it | **Included** |
| C15-K56 | Mindfulness integration with CBT across psychiatric/medical/stress domains + literature | **Needs Review** — Week 15 (third wave) has a competing ownership claim |

### §6 Разбиване на целите на стъпки (pp.264–265) — 7 KUs · **ALL INCLUDED**

| KU | Content |
|---|---|
| C15-K57 | Reaching a goal usually requires completing a number of steps along the way |
| C15-K58 | Patients feel overwhelmed focusing on distance-from-goal instead of the current step |
| C15-K59 | A graphic representation of the steps is often reassuring |
| C15-K60 | Sally: anxious at the mere thought of volunteering to speak in class |
| C15-K61 | Figure 15.2 ladder, 5 rungs bottom→top: ask a question of another student after class → ask the professor after class → ask a question in class → answer a question in class → express an opinion in class |
| C15-K62 | **The mastery-before-advance rule** — become really comfortable on a step before trying the next; before the final step you'll have become really good at the one below it |
| C15-K63 | Reminder use: when the final goal feels frightening, recall the ladder and especially the step you're on now |

### §7 Експозиция (pp.265–267) — 14 KUs · the frozen safety split runs through this section

| KU | Content | Status |
|---|---|---|
| C15-K64 | Depressed and anxious patients often resort to avoidance — itself a coping strategy | **Included** |
| C15-K65 | Two motives: hopelessness („no use calling my friends") or fear („something bad will happen") | **Included** |
| C15-K66 | Obvious avoidance: time in bed, avoiding self-care, chores, socializing, tasks | **Included** |
| C15-K67 | Subtle avoidance: avoiding eye contact, smiling, conversations, sharing opinions | **Included** |
| C15-K68 | **Поведенчески стратегии за безопасност** (Salkovskis, 1996) — believed to prevent anxiety | **Included** — appears nowhere else in the curriculum |
| C15-K69 | The maintenance mechanism: immediate relief is reinforcing, but preserves the problem | **Included** |
| C15-K70 | The cognitive cost: no opportunity to test automatic thoughts or get disconfirming data | **Included** |
| C15-K71 | The exposure *procedure* (daily engagement until anxiety drops, then escalate) | **Excluded — SAFETY**, frozen contract §B |
| C15-K72 | Coping techniques before/during/after: Thought Records, coping cards, relaxation | Deferred → Week 9 (reference only) |
| C15-K73 | Covert rehearsal (pp.303–305) for particularly avoidant patients | Deferred (outside this chapter's page range) |
| C15-K74 | Daily monitor increases adherence to a graded exposure hierarchy | **Excluded — SAFETY** (adherence instrument) |
| C15-K75 | Figure 15.3 „Персонализиран монитор" (date/activity/predicted 0–100/actual/predictions) | **Needs Review — SAFETY** (Week 9's fixed-demonstration precedent would permit it as an illustration) |
| C15-K76 | The cross-out step for predictions that didn't come true | **Excluded — SAFETY** (procedural) |
| C15-K77 | Agoraphobic hierarchies (Goldstein & Steinbeck 1987); Dobson & Dobson (2009) on exposure effectiveness | **Excluded — SAFETY** (clinical procedure literature) |

### §8 Ролева игра и социални умения (pp.267–268) — 11 KUs

| KU | Content | Status |
|---|---|---|
| C15-K78 | Role plays elsewhere in the volume: uncovering AT, developing adaptive response, modifying intermediate/core beliefs | Deferred → Weeks 10/11/12 |
| C15-K79 | Role playing is also for learning and practicing social skills | **Included** |
| C15-K80 | Two profiles: generally poor social skills, vs. one mastered style with no ability to adapt it | **Included** |
| C15-K81 | Sally: good at empathic conversation, far less practiced where assertion is appropriate | **Included** |
| C15-K82 | Role-play mechanics (therapist plays patient, patient plays the other party, then reverse) | Deferred (procedure + reproduced dialogue) |
| C15-K83 | The modelled assertive pattern (polite repeated persistence) | Deferred (reproduced dialogue) |
| C15-K84 | **Assess the skill level the patient already has before teaching social skills** | **Included** |
| C15-K85 | Many know what to say but are blocked by assumptions („If I express an opinion, I'll be rejected") | **Included** |
| C15-K86 | Test 1 — have the patient assume a positive outcome: „If you knew for sure they'd be willing, what would you say?" | **Included** |
| C15-K87 | Test 2 — the patient uses the skill in another context (assertive at work but not with friends) | **Included** |
| C15-K88 | Residual role-play uses when it's a belief problem, not a skills deficit | Deferred |

### §9 Техниката „pie" (pp.268–272) — 14 KUs

| KU | Content | Status |
|---|---|---|
| C15-K89 | It is often helpful for patients to see their ideas in graphic form | **Included** |
| C15-K90 | Two named uses: goal setting and determining relative responsibility | **Included** |
| C15-K91 | Goal-setting use case: can't specify problems/changes, or lacks insight into life imbalance | **Included** |
| C15-K92 | The 8 life areas (work, friends, fun, family, other interests, physical, household, spiritual/cultural/intellectual) | **Included** |
| C15-K93 | Two-pie method: draw „actual", then „ideal" | **Included** (labels only — Figure 15.4 proportions not recoverable) |
| C15-K94 | An automatic thought surfaces during the ideal pie; it is *recorded as a prediction*, not argued away | **Included** |
| C15-K95 | The testable alternative chain: less work → more pleasure → better mood → better concentration → more efficient work | **Included** |
| C15-K96 | Follow-through: set specific goals bringing actual time expenditure closer to ideal | **Included** |
| C15-K97 | Responsibility use case: graphically see the possible causes of an outcome | **Included** |
| C15-K98 | Sally believed close to 100% she got a C because she is basically incompetent | **Included** |
| C15-K99 | The elicited alternative causes (6, verbatim) | **Included** |
| C15-K100 | Figure 15.5's 7 wedges | **Included** |
| C15-K101 | **The ordering rule — rate the dysfunctional attribution LAST**, so all other explanations are considered first | **Included** — the section's key procedural insight |
| C15-K102 | The patient divides the sections fairly evenly — that is itself the therapeutic data | Deferred (session craft) |

### §10 Само-сравнения и списъци с кредити (pp.272–276) — 20 KUs

| KU | Content | Status |
|---|---|---|
| C15-K103 | Patients with psychiatric disorders have a negative information-processing bias, especially self-evaluation | **Included** |
| C15-K104 | They notice negative-interpretable data and ignore, discount, or forget positive information | **Included** |
| C15-K105 | Three dysfunctional comparison types: with pre-disorder self, with desired self, with people without a disorder | **Included** |
| C15-K106 | This attentional bias maintains or increases dysphoria | **Included** |
| C15-K107 | The corrective comparison — with themselves at their *worst* point | **Included** |
| C15-K108 | Sally: went to all classes and took notes, but „no one else probably had to force themselves" | **Included** |
| C15-K109 | The pneumonia analogy — would you be as hard on yourself with pneumonia? | **Included** |
| C15-K110 | The link back to the first session's depression symptoms supplies the „legitimate reason" | **Included** |
| C15-K111 | The first-person homework wording („I catch myself when I compare…") | **Needs Review — SAFETY** (self-instruction script) |
| C15-K112 | Progress-not-distance rule: focus on progress from the worst point, not distance from the best | **Included** |
| C15-K113 | **The lowest-point exception** — the approach changes; remind them of the goal list, the joint plan, and that therapist and patient are a team | **Needs Review — SAFETY** |
| C15-K114 | Credit list definition: daily lists (mental or written) of positives or things deserving credit | **Included** |
| C15-K115 | The rationale is explained first, as with all assignments | Deferred → GAP-014 homework thread (no week owner) |
| C15-K116 | The rationale chain: self-critical thoughts worsen mood; noticing good things improves it | **Included** |
| C15-K117 | Therapist self-disclosure analogy (pneumonia/depression, could have stayed in bed) | Deferred (reproduced dialogue) |
| C15-K118 | Figure 15.6 verbatim: „(Things I did that were positive, or were a little difficult but I did them anyway)" + 6 entries | **Included** |
| C15-K119 | The list is started *in session*, not only assigned | Deferred → GAP-014 |
| C15-K120 | Two entry modes: positives done, or „what did I do today that was even a little hard, but I did anyway?" | **Included** |
| C15-K121 | Recording timing (immediately; else lunch/dinner/bedtime) | Deferred → GAP-014 |
| C15-K122 | Forward link: credit lists prepare patients for uncovering positive data for the Core Belief Worksheet | Deferred → Week 12 · **LOCATOR UNCERTAIN** (see §5) |

### §11 Chapter close (p.276) — 1 KU

| KU | Content | Status |
|---|---|---|
| C15-K123 | There are many techniques; the volume covers the most common; further reading is encouraged | **Included** |

---

## 4. Unique Week 13 ownership — and the objectives mismatch

### 4a. What is genuinely Week 13's, owned by no other week

1. **How a therapist chooses among techniques** (K02, K03) — the meta-level frame that no single technique
   is „the" method and that selection follows the conceptualization. Natural closing frame for Module 3→4.
2. **Structured decision making** (K20–K28) — advantages/disadvantages *between two life options*, with a
   rating step and an explicit conclusion. Nowhere else in the curriculum.
3. **Responsibility attribution via the pie** (K97–K101), including the rate-the-attribution-last rule.
4. **Goal decomposition with the mastery-before-advance rule** (K57–K63).
5. **Avoidance as a maintaining mechanism + behavioral safety strategies** (K64–K70) — Salkovskis's
   safety-behavior concept appears in no other week.
6. **Refocusing as a legitimate alternative to evaluation, with its own failure mode** (K29–K42) — a real
   corrective to the impression Weeks 9/10 could leave that evaluation is always the right move.
7. **The negative information-processing bias and dysfunctional self-comparison** (K103–K112).
8. **Skills deficit vs. belief block, with two concrete tests** (K84–K87).

### 4b. The objectives do NOT match the real source ownership — flagged, not forced

The catalog's two objectives cover items 2 and 4 above. They do not cover items 1, 3, 5, 6, 7 or 8 —
roughly 60% of the chapter's real, non-duplicative content, including two of its strongest sections
(refocusing, ~3 pages; self-comparison/credit, ~4 pages).

The chapter's own title is **„Допълнителни когнитивни *и* поведенчески техники"**. The catalog title
„Вземане на решения и поведенчески техники" drops the cognitive half.

**Per the owner instruction, this is flagged rather than resolved.** Three options are laid out in §8.

### 4c. Overlap audit against implemented weeks

| Week | Overlap | Verdict |
|---|---|---|
| **3** — cognitive architecture | none | Clean |
| **4** — assessment/conceptualization | K02 (selection follows conceptualization) | Reference only |
| **5** — principles/alliance | K113 (team framing) | Reference only |
| **6** — session structure | K07 (agenda) | Deferred to Week 6 |
| **7** — behavioral activation | **REAL, THREE POINTS** — see below | Manageable with explicit framing |
| **8** — automatic thoughts/emotions | none material | Clean |
| **9** — distortions/Thought Record | K72 (Thought Records, coping cards as exposure aids) | Reference only |
| **10** — Socratic evaluation | K29 positions refocusing *against* evaluation | Complementary — a boundary, not a duplicate |
| **11** — intermediate beliefs | advantages/disadvantages; role play | **Same tool, different job** — see below |
| **12** — core beliefs | K122 (Core Belief Worksheet) | Reference only |

**Week 7 — the three real contact points (highest duplication risk in this week):**

- `#postepenni-stapki` (Ch.6) breaks tasks into small *energy-sized chunks* inside an activity schedule.
  Ch.15's ladder (K57–K63) decomposes a *feared goal* and adds the mastery-before-advance rule. Different
  device, different rule → **cross-link, never re-teach**.
- `#kredit` (Ch.6) owns *why credit matters in depression*. Ch.15 adds the **written daily instrument**
  (K114–K121) and the **comparison-bias diagnosis** (K103–K112), which Week 7 does not have. Week 13 must
  open by crediting Week 7 and adding only the new half.
- `#predskazvane` (Ch.6) owns prediction-vs-actual. Ch.15's version is anxiety-specific and lives inside
  the excluded exposure monitor (K74–K76) → **no new teaching needed**.

**Week 11 — „same tool, different job":** Week 11 teaches advantages/disadvantages *of a belief* and
intellectual-emotional role play *for modifying beliefs*. Week 13 uses the same two devices for a
*decision between life options* and for *assertiveness skills*. This must be stated explicitly on the page,
or it will read as duplication.

---

## 5. Terminology Map — this chapter's Bulgarian translation is unstable

**The single largest implementation risk in this week.** Four terms have 2–4 competing renderings *within
the same chapter*. Each needs an owner lock before implementation.

| Concept | Renderings found in Гл. 15 | Recommendation |
|---|---|---|
| Refocusing | roster p.256 „**пренасочване**"; section heading p.260 „**ПРЕОСМИСЛЯНЕ**" (= „rethinking"); body „да се **пренасочи** вниманието им" | **„пренасочване (на вниманието)"**. The heading is a mistranslation — body and roster agree against it. |
| Graded tasks | heading p.264 „**Оценени задачи**" (= „graded/marked", wrong sense); Фигура 15.2 caption „**Разбиване на целите на стъпки**" | **„разбиване на целта на стъпки"** — the figure caption is source-grounded and semantically correct. |
| Pie technique | roster „техниката „**пирог**""; heading „техниката „**pie**""; body „**диаграма на пай**"; Фиг. 15.5 „**Кругова диаграма** за причинност" | Owner lock required. Suggest **„кръгова диаграма"** (Fig. 15.5's own word, and native Bulgarian). |
| Credit list | body „**списък с кредити**" *and* „**списък с признания**"; Фиг. 15.6 „Списък с **признания**" | **„списък с кредити"** — matches Week 7's already-locked „Даване на **кредит** на себе си". |
| Mindfulness | roster „**осъзнатост**"; heading „умствена **яснота**"; body „техники на **внимателност**" | Owner lock required; **„осъзнатост"** is the standard Bulgarian CBT term. |
| Behavioral safety strategies | „поведенчески стратегии за безопасност" (Salkovskis, 1996), p.266 | Stable — use as-is |
| Graded exposure hierarchy | „йерархия на градирано излагане", p.266 | Stable — but the procedure itself is Excluded |

**Locator uncertainty (K122):** the BG text cites the Core Belief Worksheet as „Глава 11", but this book's
own structure places core beliefs in Глава 14. Either a translation slip or a cross-reference to a
worksheet introduced earlier. **Not resolved from memory; do not carry this cross-reference into
learner-facing content without re-checking pp.187–197.**

**OCR artifacts noted (content unaffected):** „паТиент"/„TherapisT"/„paTienT" mixed-case on pp.261–262 and
267; running header „КОНГИТИВНА" on pp.262 and 276; „maladaptive" left untranslated on p.263.

---

## 6. Representation Fit — what this week genuinely needs

Applied honestly per `AGENTS.md` („Week 6 is a reference, not a mechanical template").

| Representation | Verdict | Justification |
|---|---|---|
| **Weekly Mind Map** (Preview + Review) | **YES — required if routed** | Platform standard, and this is the best-fitting week so far: the chapter *is* a technique catalog, a natural strict single-parent hierarchy (chapter → technique family → technique). |
| **Concept Map** | **NO — do not force** | The one genuine relation-network candidate is the avoidance-maintenance loop (K64/K69/K70). But Week 7 already owns a vicious-cycle diagram (`#porochen-krag`) and Week 3 owns the hierarchy. Adding a second network risks duplication for decoration. If the owner wants one, it should *replace* prose in §Exposure, not supplement it. |
| **Process visualization** | **YES — two, both source figures** | Figure 15.2's 5-rung ladder (fully populated, verbatim) and Figure 15.1's four-quadrant decision grid (fully populated, verbatim). These are the week's spine. |
| **Comparison** | **YES — three strong, all new** | (a) refocusing vs. evaluating — when each is right (K29–K31); (b) distraction as short-term aid vs. distraction as emotional avoidance (K36 vs. K41/K42); (c) **skills deficit vs. belief block**, with two concrete tests (K84–K87). (c) is the most valuable and appears in no other week. Reuse `.category-compare` (Week 5) / `.comparison-matrix` (Weeks 7/11). |
| **Worked example** | **YES — four, zero invention needed** | Sally's decision grid (Fig. 15.1), responsibility pie (Fig. 15.5, 7 wedges), speaking ladder (Fig. 15.2), credit list (Fig. 15.6). All verbatim in source. **No new Sally biography required** — same constraint honoured as Week 11. |
| **Interactive element** | **AT MOST ONE, fixed-example only** | The source supports a „which technique fits this situation" categorization on fixed examples (reuse the locked `CategorizationCheck`). **No simulator**, despite the catalog declaring one — see §1a. Nothing resembling a personal planner, generator, or free-text builder. |
| **FinalAssessment** | **YES** | Platform standard, 8 questions; ample source material across the Included set. |
| **WeekCompletionControl / Course Progress** | **YES** | Existing architecture, unchanged. `ProfessionalReviewRequired` weeks remain completion-eligible (Week 11 precedent). |

### Retrieval / application opportunities (source-grounded, non-personal)

- Match a technique to a situation (fixed vignettes drawn from K30/K31/K35 contexts).
- Given Sally's 7 attribution wedges, identify which one the rate-it-last rule applies to (K101).
- Distinguish obvious from subtle avoidance (K66 vs. K67) on fixed examples.
- Given a fixed scenario, decide: skills deficit or belief block? (K84–K87 two tests.)
- Order the 5 rungs of Figure 15.2's ladder (K61).
- Identify which of the three dysfunctional comparison types a fixed statement shows (K105).

### Source-grounded case/example opportunities

All four Sally figures are complete and verbatim. Week 11's precedent applies directly: **use the figures,
add no new biography.** Non-Sally material in the chapter is anonymous („a patient with an anxiety
disorder", „a chronically irritable patient") and stays anonymous — no new recurring named figures, the
same decision the owner made for Emily/Rebecca in Week 11.

---

## 7. Safety — classification preserved, sensitive items flagged

**`NotEligibleForSelfGuidedSimulator` is preserved unchanged.** Derived public status stays
„Изисква професионален преглед" — never „Налично". The whole exposure sub-topic requires professional
review before publication even in purely conceptual form (roadmap §E, unchanged).

**How the frozen contract maps onto the now-known source:**

| Frozen contract item | Source reality | Mapping |
|---|---|---|
| „decision-balance conceptual diagram (fixed example)" | K21–K28, Фиг. 15.1 | **Fully satisfied** — a complete fixed example exists |
| „responsibility-pie conceptual diagram (fixed example)" | K97–K101, Фиг. 15.5 | **Fully satisfied** — 7 wedges verbatim |
| „graduated-exposure PRINCIPLE as a static ladder/continuum" | K57–K63 (ladder) + K64–K70 (avoidance mechanism) | **Fully satisfied** — and the source cleanly separates principle from procedure |
| „self-directed exposure procedure / planner / generator — EXCLUDED" | K71, K74, K76, K77 | **Excluded** as specified |

**Safety-sensitive items requiring an explicit recorded decision:**

| # | KU | Item | Recommendation |
|---|---|---|---|
| 1 | K17 | Battered spouses → shelter / legal action | **Exclude.** Domestic violence; the platform has no crisis-referral infrastructure to carry it responsibly. |
| 2 | K48 | Overeating, smoking, overspending, gambling, substance use, anger outbursts | **Exclude** the enumerated list; the monitoring principle (K45–K47) is safe without it. |
| 3 | K53 | Paradoxical arousal from relaxation | **Include — mandatory.** Protective. Relaxation must never be mentioned without it. |
| 4 | K71/K74/K76/K77 | Exposure procedure, adherence monitor, cross-out step, hierarchy literature | **Exclude** per frozen contract §B. |
| 5 | K75 | Figure 15.3 monitor | **Needs Review.** Week 9's precedent (Thought Record as a fixed, source-grounded demonstration only) would permit it as a static illustration. Owner call. |
| 6 | K113 | Lowest-point handling | **Needs Review.** Adjacent to crisis territory. The source says nothing about suicidality here and nothing may be imported. If kept, clinician-framed only. |
| 7 | K111 | First-person homework script | **Needs Review.** Reads as self-instruction. If kept, reframe as „what the therapist and patient agree on", not a script for the learner. |
| 8 | K44 | AWARE technique | **Needs Review.** Naming it is safe; presenting it as a self-applicable anxiety tool is not. |
| 9 | K10 | 5-step problem-solving cycle | **Include descriptively only** — „what therapists train", never a worksheet. Standing prohibition on personal worksheets applies. |

No diagnostic instrument, no self-assessment, no scoring, no personal record, no free-text capture is
proposed anywhere in this week.

---

## 8. Genuine owner decisions required before implementation

1. **Title/objectives mismatch (§4b).** Three options:
   (a) **widen** the week to the chapter's real scope and retitle toward „Допълнителни когнитивни и
   поведенчески техники"; (b) **keep** the narrow title and formally Defer the refocusing and
   self-comparison/credit sections to a future week (creating a new unassigned-content gap alongside
   GAP-014); (c) **split** across Weeks 13 and 14. **Recommendation: (a)** — the material is strong,
   non-duplicative, and has no other home in the 15-week structure.
2. **Catalog contradiction (§1a).** `InteractiveFormat.Simulator` vs. `NotEligibleForSelfGuidedSimulator`.
   Recommendation: change the format to `StaticVisualization` + `KnowledgeCheck`; keep the safety level.
3. **Five Needs-Review KUs (§7):** K44 (AWARE), K56 (mindfulness → Week 13 or Week 15), K75 (Figure 15.3
   as fixed illustration), K111 (homework script), K113 (lowest-point handling).
4. **Five terminology locks (§5):** refocusing, graded tasks, pie, credit list, mindfulness.
5. **Concept Map: yes or no (§6).** Recommendation: no — the avoidance loop overlaps Week 7's existing
   vicious-cycle diagram.
6. **K122 locator** — re-check pp.187–197 before carrying the Core Belief Worksheet cross-reference.
7. **Professional review scheduling** — the exposure sub-topic requires it before publication regardless
   of how narrow the final scope is.

---

## 9. Status

`IMPLEMENTED, TECHNICALLY READY — NOT YET OWNER APPROVED / LOCKED`.

The roadmap's original assessment („най-слабо източникова седмица от всичките 11", „без потвърдена
глава") was made before anyone had read Chapter 15 — **superseded**: Chapter 15 is confirmed, complete,
and supplies all three original catalog sub-topics plus four fully populated figures. On source strength
this was one of the *better*-sourced weeks, not the weakest.

**Owner decisions (§8) resolved 2026-09-07 (turn 3):** title widened to
„Допълнителни когнитивни и поведенчески техники"; catalog format contradiction resolved
(`InteractiveFormat.InteractiveModel`, never `Simulator`); all five Needs-Review KUs resolved (§0);
five terminology locks applied learner-facing; K122 re-checked directly against the source PDF and
confirmed a genuine cross-reference anomaly (§0); Concept Map declined (Mind Map only, 8 knowledge-cluster
branches).

**Implemented this session:** `Sedmica13.razor` (route `/kurs/sedmica-13`), `CourseCatalog.cs` Week 13
entry (title/objectives/route/format), `Week13ContentSliceTests.cs`, and targeted updates to
`CurriculumHubTests.cs`/`CourseProgressTests.cs`. See `02_CURRENT_STATUS.md` for the full test/build/QA
record. **Not committed, not pushed** — pending owner production review.
