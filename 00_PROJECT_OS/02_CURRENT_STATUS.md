# 02 — Current Status

*Актуализира се след всяка работна сесия. Единствен източник на истина за "къде сме сега".*

## ACTIVE CONTEXT FOR CURRENT STEP

Минимален набор документи, необходими за следващата стъпка (Седмица 7 — `UNBLOCKED`, следваща):

- `02_CURRENT_STATUS.md` (този файл — винаги първи).
- Root `AGENTS.md` — стабилни operational правила, включително deployment.
- `24_IMPLEMENTATION_ROADMAP.md` → Batch A checkpoint запис (Седмица 6/12).

**Не е необходимо** за рутинна техническа стъпка: Source Register/Coverage Matrix/Gaps (11–15), PRD (17), пълен Risk Register, пълен Session Log.

## Текуща фаза

Фаза 0 — завършена. Фаза 1 — STEP-1.1–1.5 `COMPLETE`; STEP-1.6 `DEFERRED`. Фаза 2 — STEP-2.1/2.2 `COMPLETE`. Фаза 3 — STEP-3.1–3.4 `COMPLETE`. **Foundation (Сесии 17–24) — `COMPLETE`, `COMMITTED` (hash `115f5fa`). Седмица 1 — `COMPLETE`, `COMMITTED` (Сесия 27). Седмица 3 + systemic route-safe anchor contract — `COMPLETE`, `COMMITTED` (Сесия 31). Седмица 10 — `COMPLETE`, `COMMITTED` (Сесия 33) — "Guided Practice" архетипът `VALIDATED`. Седмица 6 — `COMPLETE`, `COMMITTED` (Сесия 35, Phase C, hash `ac0d82e`) — първата седмица от Systematic Curriculum Expansion. Седмица 12 — `COMPLETE`, `COMMITTED` (съдържание от Сесия 37, комитнато заедно с WASM migration commit `4135988`) — втората седмица от build order-а, `AcademicOverview` архетип (routed, но не `Available`, по дизайн).**

## Текуща стъпка

`PRE-WEEK-7 REPOSITORY CHECKPOINT` (2026-08-27). Owner-confirmed status snapshot, taken before starting
Седмица 7:

- **Седмица 3 v2** (Deep Learning rebuild, Сесии 51–56, committed `3e4ca50`) — `OWNER APPROVED / LOCKED`.
- **Седмица 6** — Deep Learning reference implementation — `LOCKED` (Сесия 46, unchanged). The
  project-wide Mind Map/visual standard. See "Locked cognitive and visual architecture" in root
  `AGENTS.md`.
- **Седмица 8 v2** — `OWNER APPROVED / LOCKED`. Final state spans more than the one commit below —
  three fixes, all shared `MindMapBranch.razor`/`app.css` so Week 3's and Week 6's Mind Maps benefit
  too: (1) `CategorizationCheck` single-button interaction fix (removed a fake 4-button category
  choice); (2) Weekly Mind Map cluster-toggle fix (removed a nested same-page goto link that hijacked
  cluster-arrow clicks into a scroll-jump instead of expand/collapse) — both commit `94314ef`; (3)
  Weekly Mind Map collapsed-load fix (`.mindmap-branch:not([open]) > .mindmap-branch__children`
  re-establishes the browser's native "hide while closed" behavior, which `display: flex` had
  silently broken — several top-level clusters rendered pre-expanded on load) — this checkpoint
  commit.
- **Minimum Stability Gate (Седмица 3 + Седмица 8)** — `ACHIEVED`: 540/540 tests, Debug+Release build
  0/0, GitHub Actions build+deploy green end to end.
- **Course Progress MVP** (local-only, `localStorage`, no backend/account/gamification;
  `WeekCompletionControl.razor` on routed weeks + `/kurs` overall progress bar) — `OWNER APPROVED /
  LOCKED`. Eligibility is `CourseWeekDefinition.Route is not null` (not `CourseWeekStatus`), so
  Week 12 (`AcademicOverview`, routed) counts correctly. Committed `7b62e45`/`95c888b`. Does not
  touch the locked Course Map/Knowledge Map engine.
- **Седмица 7** (Поведенческа активация, SRC-041 Гл. 6) — `OWNER APPROVED / LOCKED`. 54/54 KUs
  Included (0 Needs Review/Deferred/Excluded/Unaccounted) — see
  `00_PROJECT_OS/_blueprints/WEEK_07_SOURCE_COVERAGE_AUDIT_v1.md`. Final Sally Concept Map desktop
  layout (spatial Network layout, not the stacked Chain column) owner-approved. **594/594** теста,
  Debug+Release build 0/0, production deploy successful (commit `20833aa`).
- **Седмица 9** (Когнитивни изкривявания и дневник на мислите, SRC-041 Гл. 11–12) —
  `OWNER APPROVED / LOCKED`. 76 KUs — 39 Included / 3 Deferred / 34 Excluded / 0 Needs Review /
  0 Unaccounted — see `00_PROJECT_OS/_blueprints/WEEK_09_SOURCE_COVERAGE_AUDIT_v1.md`. Evaluation
  questions kept at recap depth (cross-linked to Седмица 10); Сали not extended with new biography —
  Джон (source-named) carries "why evaluation fails"; Thought Record is a fixed, source-grounded
  demonstration only (Figure 12.2's own generic worksheet example), separate scope from the
  still-open product-level MVP Thought Record requirement. Includes the Thought Record map's
  edge-label micro-fix (scoped CSS only, same technique as Week 3's Sally-map precedent).
  **623/623** теста, Debug+Release build 0/0, production deploy successful (commit `1cd2171`).
- **Седмица 10** (Сократически въпроси и съвместно изследване, SRC-041 Гл. 11 — retrofit of a
  pre-Deep-Learning page) — `OWNER APPROVED / LOCKED`. 54 KUs (same chapter as Седмица 9) — 23
  Included / 30 Deferred (Седмица 9's territory) / 1 Excluded / 0 Needs Review / 0 Unaccounted —
  see `00_PROJECT_OS/_blueprints/WEEK_10_RETROFIT_AUDIT_v1.md`. Four-category compression replaced
  by the source's real six categories; invented dialogue scenario retired for a source-grounded
  Сали/Карен worked example; terminology migrated to "адаптивен отговор" (page-scoped only).
  **627/627** теста, Debug+Release build 0/0, production deploy successful (commit `fc5b9f4`).
- **Седмица 12** (Основни вярвания и схеми, SRC-041 Гл. 14 — conservative retrofit of an already
  clean-source page) — `OWNER APPROVED / LOCKED`. 40 KUs — 12 Included / 6 Deferred / 22 Excluded /
  0 Needs Review / 0 Unaccounted — see `00_PROJECT_OS/_blueprints/WEEK_12_RETROFIT_AUDIT_v1.md`.
  Added schema-vs-core-belief distinction, developmental origin, the abstract "screen" filtering
  mechanism, and a descriptive account of developing/strengthening a new belief; full clinical-
  technique catalog (Sally/Annie dialogues, CBW walkthrough, etc.) stays Excluded — AcademicContextOnly
  boundary preserved, zero new interactivity. **633/633** теста, Debug+Release build 0/0, production
  deploy successful (commit `c72593b`).
- **Седмица 1** (Как се ражда когнитивната терапия, SRC-041 Гл. 1 — conservative retrofit of an
  already clean-source page) — `OWNER APPROVED / LOCKED`. 27 KUs — 13 Included / 10 Deferred /
  4 Excluded / 0 Needs Review / 0 Unaccounted — see `00_PROJECT_OS/_blueprints/WEEK_01_RETROFIT_AUDIT_v1.md`.
  Enriched HistoricalTimeline/ResearchTurnStepper with the real dream-study/two-streams/1977
  Beck-Rush RCT narrative (patient anecdote paraphrased, never quoted); removed an unsupported
  "why manuals matter" section; later visual fix un-nested Sections 04–05 from a shared grid and
  corrected a shared-CSS `white-space: nowrap` root cause behind the Section 04 table's clipping.
  **639/639** теста, Debug+Release build 0/0, production deploy successful (commit `6b7f352`).
- **Седмица 2** (Когнитивна терапия на Бек и REBT на Елис, SRC-041 Гл. 1 бридж-изречения + нов
  SRC-042 — Albert Ellis Institute, build-from-scratch академично сравнение) — `OWNER APPROVED /
  LOCKED`. 20 KUs — 14 Included / 2 Deferred / 4 Excluded / 0 Needs Review / 0 Unaccounted — виж
  `00_PROJECT_OS/_blueprints/WEEK_02_SOURCE_AUDIT_v1.md`. Затворен source gap за ABC модела чрез
  ново регистриран SRC-042 (официална Albert Ellis Institute статия); честно сравнение Бек/Елис
  без "победител", без дублиране на Седмица 1/3. **658/658** теста, Debug+Release build 0/0,
  production deploy successful (commit `90acb56`).
- **Седмица 4** (Клинична оценка и когнитивна концептуализация, SRC-041 Гл. 4 — build-from-scratch,
  `AcademicContextOnly`) — **`IMPLEMENTED, TECHNICALLY READY — NOT YET OWNER APPROVED / LOCKED`.**
  38 KUs — 27 Included / 4 Deferred / 7 Excluded / 0 Needs Review / 0 Unaccounted — виж
  `00_PROJECT_OS/_blueprints/WEEK_04_SOURCE_AUDIT_v1.md`. Мостът "Сесия по оценка → начална
  когнитивна концептуализация" (връзка към Седмица 3) е централната секция; три owner-resolved
  safety-sensitive KUs (суициден риск, DSM, automatic-thought bridge), нула възпроизведен
  терапевт-пациент диалог. **677/677** теста, Debug+Release build 0/0. Routed
  (`/kurs/sedmica-4`), resolves to `AcademicOverview` (same treatment as Седмица 12).
  **PENDING OWNER-REVIEW ITEM (before LOCK):** Section 08 visual/safety framing refinement —
  keep all five conceptualization questions, but reframe as professional-clinician reasoning, not
  learner self-assessment; move the five questions into an existing academic/info card pattern;
  split the Sally synthesis into two readable paragraphs without changing facts; preserve the
  final statement that conceptualization is revised throughout therapy. No KU accounting change.
  After the fix: desktop/mobile QA → production review → explicit `OWNER APPROVAL` → `LOCK`.
- **Седмица 5** (Принципи на КПТ и терапевтичен съюз, SRC-041 Гл. 1 стр. 6–11 + Гл. 2 стр. 17–21,
  `PublicWithAdaptation`) — **`IMPLEMENTED, TECHNICALLY READY — NOT YET OWNER APPROVED / LOCKED`.**
  52 KUs (two-chapter, owner-approved final audit) — 30 Included / 10 Deferred / 12 Excluded /
  0 Needs Review / 0 Unaccounted — виж `00_PROJECT_OS/_blueprints/WEEK_05_SOURCE_AUDIT_v1.md`.
  Десетте принципа на ниво принцип (Гл. 1) + задълбочена секция за терапевтичния съюз (Гл. 2,
  нишка 1: доверие/резултати, шест конкретни действия, терапевтична позиция, мониторинг/поправка
  на алианса, персонализация, alliance-is-dynamic) + collaboration before/after сравнение
  (`.category-compare`) + описателна, изрично не-протоколна времева последователност за Принцип 7
  (`.guided-practice-sequence`). Терминология заключена: `терапевтичен съюз` (никога „алианс"),
  `колаборативен емпиризъм` (не Гл.1's „съвместен емпиризъм"); C2-K02's OCR-несигурна citation без
  име на автор. Нула нов компонент, нула Mind Map/Concept Map, нула възпроизведен диалог. Реален
  browser smoke test (Playwright, headless Chromium) потвърди: 13/13 секции, 10/10 принципа,
  category-compare + guided-practice-sequence рендерирани коректно, 0 console/page грешки, `/kurs`
  hub-ът коректно линква към новата седмица („Девет седмици" вместо „Осем"). **691/691** теста,
  Debug+Release build 0/0, `git diff --check` чист (само LF/CRLF предупреждения). Routed
  (`/kurs/sedmica-5`), resolves to `Available` (PublicWithAdaptation, като Седмица 6/7/9). **GAP-014**
  (Гл. 2's "подчертаване на положителното"/"домашна работа" нишки нямат curriculum owner) остава
  съзнателно извън тази страница — виж `15_GAPS_AND_CONFLICTS.md`. Некомитнато — предстои owner
  visual/learning review преди `OWNER APPROVAL`/`LOCK`.
- **Седмица 11** — `UNBLOCKED` / `NEXT` (след Седмица 5's build order). Not started.
- **App architecture** — standalone **.NET 10 Blazor WebAssembly**. The hosted Blazor Web App/server
  project was replaced by a standalone `CbtLearningPlatform.Client` WASM app so the site can deploy to
  Netlify (static-only host). Commit `4135988`.
- **GitHub Actions CI → Netlify production deployment** — `OPERATIONAL`. Remote:
  `https://github.com/Deyan2505/CBTLearningPlatform` (public). `.github/workflows/ci.yml` gained a
  `deploy` job (publish + `nwtgck/actions-netlify`) alongside the existing build+test job. First
  push-triggered run on `main` (commit `94314ef`) completed: **Build success, Deploy to Netlify
  success.** The long-standing "no remote / CI never run on GitHub" blocker recorded below (line
  "Push/deployment блокер") is resolved.

Full technical detail lives in the session's own commits (`4135988`, `94314ef`) and conversation
record — not duplicated here. No architecture change, no new plan; this entry is a status checkpoint
only.

Предходен checkpoint — `WEEK 3 — FINAL GEOMETRY CORRECTION` (Сесия 56, 2026-08-25). Owner откри, че Section 07/09 остават
вертикални въпреки Сесия 55. **Root cause:** `.concept-map__flow--horizontal` никога не redeclare-ваше
`flex-direction` — базовият клас го фиксира на `column` безусловно; `display:flex` само по себе си не
значи row. Добавен липсващият `flex-direction: row` (единична поправка, коригира и двете секции).
Cascade loop connector-ът смален до плитка дъга (70% ширина) с label до нея, не центриран под целия
ред. Sally map: `sali-situation` преместена в СЪЩАТА колона като `sali-thought` (не същия ред) —
превръща ѝ входа в distinct top-anchor vertical entry, различен от трите belief edges' left-middle
entry — потвърдено чрез rendered path координати. Canvas 536→460px. 9 nodes/10 relations непроменени.
**518/518** теста, build 0/0. **Git anomaly:** pre-existing Седмица 12 работа (6 файла) открита STAGED
в index-а (не от тази сесия) — commit направен с изричен pathspec (`3e4ca50`), комитвайки само двата
Week 3 файла; Седмица 12 остава staged точно както е била, недокосната от тази сесия. Резултат:
`STRUCTURAL QA COMPLETE — OWNER PIXEL REVIEW REQUIRED`. Server: `http://localhost:5131/kurs/sedmica-3`.

Предходен checkpoint — `WEEK 3 — TARGETED DIAGRAM GEOMETRY FIX` (Сесия 55, 2026-08-24). Geometry-only pass след Сесия 54 —
content/architecture остава PASS, Mind Map/coverage/semantic relations недокоснати. **Section 09:**
нов `.trigger-inputs` bordered box отделя trigger-примерите от каскадата; каскадата вече хоризонтална
на 900px+ (реизползва Section 07's `--horizontal` модификатор) с нова widescreen loop-back SVG (дипва
надолу и се връща от последния към първия node), label вече постоянно хоризонтален текст (премахнат
`vertical-rl`). **Sally map:** прередизайнирана, не rescale-ната — `sali-situation` вече на СЪЩИЯ ред
като `sali-thought` (паралелна двойка, не подниво на вярванията); canvas 840px→**536px** (verified).
9 nodes/10 relations непроменени. **Section 03 ROOT CAUSE fix:** базовият `.concept-map__node` (inline
-flex, shrink-to-fit) нямаше собствено centering — fix-нат на ниво базов `.concept-map__flow` клас
(`flex column + align-items:center`), подобрява ВСЯКА употреба сайт-wide, не само Section 03. Заглавие
"От ситуация до значение" → "От ситуация до автоматична мисъл" (терминологична яснота). §4 verification:
Section 07/Triad/Section 13/Sources/no-English-leakage — всичките потвърдени непокътнати. **518/518**
теста (0 нови), build 0/0 (Debug+Release). Резултат: `STRUCTURAL QA COMPLETE — OWNER PIXEL REVIEW
REQUIRED`. Server: `http://localhost:5131/kurs/sedmica-3`. **Седмица 7/8/12 — недокоснати.**

Предходен checkpoint — `WEEK 3 v2 — FINAL UI + TERMINOLOGY POLISH` (Сесия 54, 2026-08-24). Content/architecture вече PASS
(Сесия 53) — тази сесия е чист presentation polish по owner screenshot findings, Mind Map/43-43
coverage/Sally архитектура недокоснати. **Terminology:** "Автоматично значение" (секция 03) уеднаквено
на "Автоматична мисъл" — проверено директно срещу source extraction-а, главата никога не използва
"значение" като отделен термин. Секция 07's "Междинно правило или предположение" коригирано на
"Междинно вярване (нагласа, правило или предположение)" — вече не противоречи на 3-подтиповата
йерархия. **Localization:** 5 English-leakage instances ("worked example"/"populated пример")
заменени с естествен български. **Sally map:** row spacing утроен (presentation-only данни на тази
страница, не на споделения `ConceptGraph` engine) — canvas 1048×384→1048×840px, verified чрез decoded
HTML, 9 nodes/10 relations непроменени. **Section 07/triad/cascading model:** три нови, additive CSS
modifier класа (`--horizontal`, `--triad`, `.cascade-loop`), всеки scoped само към своята инстанция,
base класовете (`.concept-map__flow`, `.concept-map__side-notes`) недокоснати за всички други weeks.
**§8 ROOT CAUSE намерен:** секция 13 седеше сама в 2-колонен `.learning-grid--balanced` — единствено
дете заема само 1-вата колонна писта (bug пренесен от v1, сега поправен систематично, не с hardcoded
width). **518/518** теста (0 нови/премахнати), build 0/0 (Debug+Release). Резултат: `STRUCTURAL QA
COMPLETE — OWNER PIXEL REVIEW REQUIRED`. Server: `http://localhost:5131/kurs/sedmica-3`. **Седмица
7/8/12 — недокоснати.**

Предходен checkpoint — `WEEK 3 v2 — DEEP LEARNING IMPLEMENTATION` (Сесия 53, 2026-08-24). Реален код по
`WEEK_03_V2_MIGRATION_BLUEPRINT.md`: `Sedmica3.razor` разширен (не пренаписан) — запазени 01-04/07 и
двата load-bearing anchor-а (`#situacia-znachenie`/`#tri-niva`, реферирани от `KnowledgeMapCatalog.cs`);
добавени Weekly Mind Map preview/review (locked Week 6 pattern), "Случаят на Сали" (retold, не
verbatim, populated 9-node/10-edge `ConceptGraphLayout.Network` map, изцяло локален — нищо ново в
`KnowledgeMapCatalog.cs`), Attitude/Rule/Assumption таблица, treatment-sequencing rationale,
cascading-model секция, 2 нови assessment въпроса (общо 6). **SOURCE UNRESOLVED резолюция:**
"automatic vs. reflexive processing" — 0 съвпадения в `11_SOURCE_REGISTER.md`, премахнато (не
измислен source). Самокоригиран бъг по време на runtime QA: static field declaration order (fixed).
**518/518** теста, build 0/0 (Debug + Release). Runtime QA срещу decoded HTML потвърди точна SVG
геометрия (1048×384px canvas, 9 nodes, 10 edges) — по-силна от предишен "structural only" QA, но
все още без browser/screenshot инструмент. Резултат: `IMPLEMENTED — AWAITING OWNER LEARNING REVIEW`.
Server: `http://localhost:5131/kurs/sedmica-3`. **Седмица 7/8/12 — недокоснати.**

Предходен checkpoint — `WEEK 3 AUDIT QA FIX + V2 MIGRATION BLUEPRINT` (Сесия 52, 2026-08-24). Owner откри QA blocker в
Сесия 51's Coverage Matrix (43 units декларирани, но статусите сумираха само 41 — U01-U04 никога не
получиха ред, U33 беше двойно броен). Коригирано: **10 Included / 23 Missing / 9 Deferred / 1
Excluded = 43, Unaccounted 0** — корекцията прави Class B заключението по-силно (Missing нарасна от
17 на 23). Owner потвърди: Сали остава worked case (преразказана, не verbatim), Ирина остава
отделен transfer-exercise case. Нов `00_PROJECT_OS/_blueprints/WEEK_03_V2_MIGRATION_BLUEPRINT.md` —
пълна migration архитектура (Sally case, populated hierarchy Concept Map, intermediate-belief
подтипове, treatment rationale, cascading model diagram, retrieval/assessment upgrade). Едно open
item: "automatic vs. reflexive processing" маркирано `SOURCE UNRESOLVED` — owner решение нужно преди
implementation. `READY FOR WEEK 3 IMPLEMENTATION: YES`. **Никакъв код не е пипнат.** Седмица
7/8/12 — недокоснати.

Предходен checkpoint — `WEEK 3 — DEEP SOURCE + COVERAGE AUDIT` (Сесия 51, 2026-08-23). Read-only audit, `Sedmica3.razor`
недокоснат. Глава 3 на SRC-041 ("Когнитивна концептуализация", printed стр. 29-45) извлечена по
Week 6 метода → `_source_corpus/SRC-041_ch03_bg_extracted.txt` (gitignored). **43 Knowledge Units**,
Coverage Matrix: 18 Included / 17 Missing / 3 Deferred / 3 Excluded — **не 100% coverage**.
Най-голямата липса: ~40% от главата е case example ("Сали"), нула еквивалент на текущата страница.
**Migration class потвърден: B** (не граничен случай). Owner decision surfaced: адаптирай Сали
директно или разшири Ирина с еквивалентен worked example — не решено тук. Резултат: нов
`00_PROJECT_OS/_blueprints/WEEK_03_SOURCE_COVERAGE_AUDIT_v1.md`, `READY FOR OWNER REVIEW: YES`.
Следваща стъпка: owner review, после Week 8 audit (същия формат), после реален implementation.
**Никакъв код не е пипнат.** Седмица 7/8/12 — недокоснати.

Предходен checkpoint — `PROJECT-WIDE COGNITIVE LEARNING ROLLOUT — AUDIT & MIGRATION PLAN` (Сесия 50, 2026-08-23). Owner
финално одобри: **Course Map — `OWNER APPROVED / LOCKED`**; **CBT Knowledge Map — `OWNER APPROVED`,
project-wide reference standard** за network-стил concept maps (real drawn SVG relations, не chain/
chips); **Week 6 Cognitive Reference Implementation — `OWNER APPROVED`** (непроменено от Сесия 46);
**Global Cognitive Maps Phase (Phase 5) — `PASSED`**. Одобрен финален Knowledge Map commit: `67aec83`
(предшестван от корекционен commit `2498036` — виж Сесии 48-49 в `10_SESSION_LOG.md`: първата
корекция премести relations от единичен `FirstOrDefault` chain към chips, но owner откри, че chips
все още са текст вътре в картите; финалната версия рисува всяка от 12-те relations като реална SVG
крива между node карти, deterministic SSR геометрия, нула DOM measurement, нула WASM). **510/510**
теста, build 0/0. Текущата задача е **read-only audit + planning**, не implementation: пълен одит на
реално routed седмици (1, 3, 6, 8, 10 — committed; 12 — само в uncommitted working tree, read-only
анализирана) срещу вече доказания Week 6 + global-maps standard, с migration classification (A-E) и
конкретен blueprint. Резултат: нов `00_PROJECT_OS/_blueprints/COGNITIVE_LEARNING_ROLLOUT_PLAN_v1.md`.
**Никакъв код не е пипнат тази сесия.** GAP-013 остава `Open`, недокоснат. Седмица 7 — не е започната.

Предходен checkpoint — `GLOBAL COURSE + KNOWLEDGE MAPS — IMPLEMENTED, AWAITING OWNER VISUAL + LEARNING REVIEW` (Сесия 47,
2026-08-22). Phase 5 на `COGNITIVE_LEARNING_ARCHITECTURE_v1.md` — нов route `/kurs/karta`, два
различни режима (`?mode=course` по подразбиране / `?mode=knowledge`), избирани чрез два реални
server-rendered линка (`[SupplyParameterFromQuery]`, нула JS/WASM за самия switch). **Course Map:**
root ("15-седмичен курс") → 4 модула → 15 седмици, изведени изцяло от `CourseCatalog` (нов
`Curriculum/CourseMapBuilder.cs`, без втори hardcoded списък) — routed седмици = `Introduced`
(достъпни), unrouted = `Upcoming` (приглушени, curriculum reachability, не learner progress).
**CBT Knowledge Map:** нов `Curriculum/KnowledgeMapCatalog.cs` — 10 concepts / 12 relations,
**всеки грундиран в реално прочетено съдържание** на вече routed страници (Седмици 3/6/8/10,
верифицирани real anchors: `#situacia-znachenie`/`#tri-niva`/`#karta-na-temata`/`#poniatiya`/
`#izsledvane`) — Седмица 1 съзнателно изключена (чисто наративно съдържание, няма преизползваем
concept), Седмица 12 се появява само като `Revisited`, никога като `IntroducedWeek`. И двете карти
преизползват **напълно непроменен** `ConceptGraph.razor`/`MindMapBranch.razor` (LOCKED Week 6
standard) — нула нов rendering код, нула промяна по Week 6. **495/495 теста** (476 + 19 нови).
Build 0/0. 10/10 routes `200` на прясна инстанция, включително двата `/kurs/karta` режима. Week 6
regression потвърдена непокътната. **Structural QA only** — собственически visual review остава
задължителен. Некомитнато — предстои единствен commit, изолиран от Седмица 12. GAP-013 не е
адресиран (извън обхват). Седмица 7/12/retrofit — не са пипнати.

Предходен checkpoint — `WEEK 6 COGNITIVE REFERENCE IMPLEMENTATION — OWNER APPROVED` / `MIND MAP VISUAL STANDARD —
LOCKED` (Сесия 46, 2026-08-22, documentation closeout). Собственикът прегледа `/kurs/sedmica-6` в
реален browser и одобри финално. **LOCKED като project-wide reference standard** (не се polish-ва
повече без нова собственическа забележка): desktop spatial parent→child Mind Map; mobile vertical
expandable tree; compact memory nodes; primary vs. secondary visual hierarchy; branch
expand/collapse; navigation secondary спрямо learning representation; semantic-model-driven
rendering; SSR-first architecture; Mind Map ≠ Concept Map разграничение; Case Map като domain-specific
semantic representation. Последен одобрен commit: `4ea9619`. **Project-wide Cognitive Learning
Architecture (`COGNITIVE_LEARNING_ARCHITECTURE_v1.md` v1.1) reference validation: `PASSED`** —
Phase 4 gate (owner visual+learning review на reference implementation-а) е премината.
**Content QA note (не блокира одобрението, GAP-013 в `15_GAPS_AND_CONFLICTS.md`):** Седмица 6
текстът твърди "шест цели", но реално изброява само 5 ясно отделими клаузи — не е поправено с
предположение, оставено за директна сверка със SRC-041 Глава 5 при бъдещ source-fidelity pass.
**Следваща оторизирана фаза:** Phase 5 — `COURSE MAP + CBT KNOWLEDGE MAP` на нов route `/kurs/karta`,
с ДВА различни режима (Course Map: "къде съм в курса" — модули/седмици/prerequisites/статус; CBT
Knowledge Map: "как са свързани знанията" — concepts/labeled relationships/introduced-revisited
metadata/cross-week връзки) — **не е започната в тази сесия**, изисква отделно ново извикване.
Никакъв код не е пипнат тази сесия (само документация). Не е започната Седмица 7, не е пипната
Седмица 12, няма retrofit.

Предходен checkpoint — `MIND MAP FINAL VISUAL POLISH — IMPLEMENTED, OWNER PIXEL APPROVAL REQUIRED` (Сесия 45, 2026-08-22).
Owner potвърди, че Сесия 44 вече Е mind map (не card grid, не outline), но posочи polish проблеми:
(1) primary branches все още се закачат за една обща вертикална линия — усещане за navigation
spine; (2) connector-ите твърде ортогонални/технически; (3) node label + `→` изглеждат като меню;
(4) Level-1 branches нямат достатъчно собствена visual identity; (5) "Цели на сесията" — need semantic
check дали leaf или branch. Полирано, чисто CSS, без нов компонент/WASM/данни: (1) споделеният root→
branches trunk е направен по-тънък и приглушен (`1px`), докато собствената extend на всеки branch
(elbow-ът) е удебелен (`3px`) и много по-заоблен (`24px` радиус) — всеки branch вече чете като "своя
собствена извита линия от root", не "запис на обща релса"; gap между branch редовете увеличен
(`var(--space-5)`) за по-ясна territory separation; (2) elbow геометрията вече по-organic
(по-голям радиус), не технически 90°; (3) navigation вече secondary — премахнат persistent
underline от `.mindmap-branch__goto` и leaf-линковете, `text-decoration`/пълна opacity се появяват
само на hover/focus (never напълно скрити — keyboard/focus users винаги виждат affordance-а); (4)
Level-1 (depth-1) вече получава по-силен, но calm treatment — `border-width: 2px`, `background:
var(--color-surface)` (различна от decht-2's `--color-info-bg` "chip" вид) — едно консистентно
третиране за целия tier, не различен цвят per branch; (5) **"Цели на сесията" остава leaf (Option
A)** — документирано решение: source текстът твърди "шест цели", но реално изброява само 5
semicolon-разделени клаузи (една клауза съдържа множество под-цели) — разбиване на 6 деца би
изисквало да се измислят кратки labels, които одобреният текст никога не дава — source fidelity
диктува да остане leaf, не branch. Mobile напълно непипнат (regression-проверено). **476/476 теста**
(472 + 4 нови). Build 0/0. ConceptMap/CaseMap/данни/labels потвърдено идентични на Сесия 44 (нула
съдържателна регресия). 8/8 routes `200`. **Structural QA only** — CSS polish не може да бъде
визуално потвърден без browser; статус изрично `IMPLEMENTED — OWNER PIXEL APPROVAL REQUIRED`.
Некомитнато — предстои единствен commit, изолиран от Седмица 12.

Предходен checkpoint — `DESKTOP SPATIAL MIND MAP — IMPLEMENTED, OWNER PIXEL REVIEW REQUIRED` (Сесия 44, 2026-08-22). Owner
pixel-review на Сесия 43 потвърди: резултатът е "GOOD HIERARCHICAL OUTLINE TREE", но не "FINISHED
SPATIAL MIND MAP" — всички primary branches вертикално един под друг, child nodes indented list,
геометрията не кодира spatial hierarchy, node-овете приличат на content cards, "Виж секцията →"
дублирано визуално навсякъде. Коригирано **без нов компонент, без WASM, без дублирани данни**: **същата**
`MindMapBranch.razor` markup вече рефлоу-ва респонсивно през чист CSS — тесни контейнери (mobile,
непроменено) остават vertical outline tree; широки контейнери (`@container (min-width: 700px)` плюс `@supports` fallback, точно като
`.guided-practice-sequence`-ия прецедент) превключват `<details>`
от block (summary над body) на `flex-direction:row` (summary | children-column вдясно) — root вляво,
primary branches central column, secondary concepts дясна колона, чрез рекурсивно същия механизъм
(root→branches идентичен на branch→children). Curved elbow connector техниката (border+border-radius)
остава непроменена геометрично, само gap-ът между summary/children е направен равен на elbow-ото
хоризонтално разстояние (`1.5rem`), за да прилепва визуално без нужда от допълнителен bridge елемент.
Node-овете вече са компактни — премахнат `ShortDefinition` inline текст и повтарящото се "Виж
секцията →" изречение; заменени с малка, отделна `.mindmap-branch__goto` "→" връзка (branch) или
целият leaf node като линк (leaf) — toggle (label+chevron) и navigate (goto/leaf link) остават
различими controls. **472/472 теста** (470 + 2 нови — desktop spatial CSS hook, compact-node
guard). Build 0/0. ConceptMap/CaseMap потвърдено непокътнати. 8/8 routes `200`. **Structural QA
only** — технически статус изрично `IMPLEMENTED — OWNER PIXEL REVIEW REQUIRED`, не "visual pass".
Некомитнато — предстои единствен commit, изолиран от Седмица 12.

Предходен checkpoint — `MIND MAP VISUAL STANDARD — IMPLEMENTED, AWAITING OWNER VISUAL APPROVAL` (Сесия 43, 2026-08-22).
Owner visual review на Сесия 42 откри конкретен проблем: MindMap семантиката беше правилна, но
визуалното представяне (responsive card grid) не отговаряше на "истинска mind map" — приложен
reference screenshot като cognitive/visual benchmark (не за копиране 1:1). Коригирано: MindMap
режимът на `ConceptGraph.razor` вече рендерира **истинско пространствено дърво**, не card grid —
нов рекурсивен `Components/Shared/MindMapBranch.razor` (root → primary branches → secondary
concepts, arbitrary depth, native `<details>`/`<summary>` disclosure, collapsed by default). Curved
branch connectors чрез CSS border+border-radius "elbow" техника (не SVG paths) — изрично решение,
обосновано в кода: SVG координати биха изисквали JS recompute при всяко expand/collapse, докато CSS
техниката е винаги коректна (произлиза от live box model), без нужда от WASM. Depth изразен чрез
size/weight/padding, не само цвят. `MindMapAdapter` разширен с cycle detection (защита срещу
infinite recursion в рекурсивния renderer). Седмица 6 MindMap данните преструктурирани в реална
3-нивова йерархия (root → Цели/Начало/Среда/Край/Гъвкавост → техните concepts) — същото вече
одобрено съдържание, само реорганизирано; "Гъвкавост" клонът преизползва точните 3 leaves на
съществуващия `.decision-branch` (не дублирано съдържание). ConceptMap/CaseMap режимите — напълно
непроменени, потвърдено чрез regression QA. **470/470 теста** (458 + 12 нови — cycle detection,
arbitrary depth, deterministic child order, domain-ignorance на `MindMapBranch`, toggle/navigation
separation, collapsed-by-default). Build 0/0. 8/8 routes `200`. Structural QA потвърди: 2 mindmap
инстанции (Preview/Review) с еднакъв root label, 8 collapsed `<details class="mindmap-branch">`
(4 branches × 2 инстанции), 8 aria-hidden chevrons, всички labels присъстват. **Structural QA only**
— собственически pixel-level visual review остава задължителен. Некомитнато — предстои единствен
commit, изолиран от Седмица 12.

Предходен checkpoint — `WEEK 6 COGNITIVE REFERENCE IMPLEMENTATION — AWAITING OWNER REVIEW` /
`PROJECT-WIDE COGNITIVE ARCHITECTURE — REFERENCE VALIDATION IN PROGRESS` (Сесия 42, 2026-08-22).
Owner-authorized implementation на `COGNITIVE_LEARNING_ARCHITECTURE_v1.md` v1.1, строго Phases 1–3
(semantic models, `ConceptGraph` rendering engine, Седмица 6 reference implementation) — **без**
глобалната `/kurs/karta`, **без** Course Map/Knowledge Map, **без** retrofit на други седмици,
**без** Седмица 7. Нови файлове: `Curriculum/ConceptGraphModels.cs` (MindMap/ConceptMap domain
модели + `ConceptState`/`ConceptStateResolver`), `Curriculum/CaseConceptualizationModels.cs`
(`CaseCatalog` с Ирина, само Week-6-потвърдени полета), `Curriculum/ConceptGraphAdapters.cs`
(domain → чисто презентационен `GraphRenderModel`, adapter слой), `Components/Shared/ConceptGraph.razor`
(domain-ignorant renderer, Static SSR по подразбиране, MindMap/ConceptMap/CaseMap режими,
accessible fallback генериран от същите данни като визуала). Седмица 6: Weekly Mind Map
Preview (6.0) + Review (6.12) — **един и същ** `_week6MindMapRender`, Review гейтнат зад
attempt-before-reveal `<details>`; Concept Map в 6.5 надградена от плоска 3-node верига до
ConceptGraph с реални cross-links към вече съществуващия пълен модел на Седмица 8 и вярванията на
Седмица 3 (нищо ново измислено); Case Conceptualization Map за Ирина в 6.8 (само Situation/
Behavior/InterventionLink — единствените Week-6-потвърдени полета); 2 retrieval-practice добавки
(explain-before-reveal при терминологията, reconstruct-the-order при Review Map, чист SSR/
`<details>`, не extracted WASM widget). Deep Learning DoD v2 → **v3** приложен в
`06_QA_STRATEGY.md` (механичното "минимум 3 визуализации" заменено с Cognitive Representation
Coverage + нов Retrieval Practice Coverage gate). **458/458 теста** (430 baseline + 28 нови), build
0/0 (0 warnings), 8/8 routes `200` на прясна инстанция (`/`, `/kpt`, `/kurs`, Седмици 1/3/6/8/10),
structural HTML QA потвърди коректно рендериране на всичките 4 ConceptGraph инстанции — **structural
visual QA only**, собственически визуален/learning review остава задължителен преди Phase 4.
Некомитнато в момента на писане — предстои единствен implementation commit, изолиран от все още
некомитнатата Седмица 12. Пълни детайли в `10_SESSION_LOG.md` (Сесия 42).

Предходен checkpoint — `PROJECT-WIDE COGNITIVE LEARNING ARCHITECTURE v1.1 — REVISION PASS COMPLETE, READY FOR
IMPLEMENTATION PROMPT: YES` (Сесия 41, 2026-08-21). Собственикът прегледа v1 (Сесия 40) —
`APPROVED IN PRINCIPLE — REVISION REQUIRED BEFORE IMPLEMENTATION`. Фундаментална корекция: **Course
structure ≠ Knowledge structure** — v1's единствена "Global CBT Master Mind Map" (седмица-като-node)
разделена на **Course Map** (навигационна: Modules→Weeks, работи директно с вече съществуващите
`CourseWeekDefinition`/`CourseModule`) и **CBT Knowledge Map** (концептуална: concepts→relationships,
седмица/модул стават метаданни върху concept node, не равностоен node). `ConceptGraph.razor`
потвърден като rendering engine, но НЕ като единствен семантичен модел — три отделни domain модела
(`MindMapModel`/`ConceptMapModel`/`CaseConceptualizationModel`) захранват renderer-а през adapter
слой. Weekly Mind Map коригиран — nodes са knowledge clusters/concepts (agenda/mood-check/diagnosis
discussion), не визуален Table of Contents от секции; Preview/Review вече са две състояния на една
структура, не два модела. Curriculum-state semantics преименувани (`Locked`/`Available`/`Reinforced`
→ `Upcoming`/`Introduced`/`Revisited`) — платформата няма persistent learner-progress engine и не
намеква, че има. Retrieval Practice издигнат в самостоятелна architecture секция, с нова
Recognition/Retrieval/Application/Reasoning таксономия, илюстрирана с реален (не измислен) coverage
одит на Седмица 6 (retrieval систематично отсъства в теоретичните секции 6.2/6.3/6.5, но не е нула
цялостно благодарение на симулатора). DoD v3 вече с ДВЕ gates (Cognitive Representation Coverage +
нов Retrieval Practice Coverage), механичните количества премахнати (memory anchor "maximum ~1"
заменено с "само при ясна mnemonic function"). Owner decisions затворени: `/kurs/karta` permanent
navigation; Static SSR default + progressive WASM; build order Phase 1–7 (Week 6 reference преди
global rollout, Week 7 остава frozen); retroactive metadata само след Week 6 reference approval,
source-confirmed. Blueprint актуализиран на място:
`00_PROJECT_OS/_blueprints/COGNITIVE_LEARNING_ARCHITECTURE_v1.md` (v1.1). Никакъв `.razor`/CSS/тест/
`CourseCatalog.cs` файл не е пипан; Week 6 v2, Week 7, Week 12 остават точно както бяха; няма commit.

Предходен checkpoint — `PROJECT-WIDE COGNITIVE LEARNING ARCHITECTURE v1 — ARCHITECTURE PROPOSAL, AWAITING OWNER REVIEW`
(Сесия 40, 2026-08-21). Owner review на Week 6 v2 установи фундаментален проблем — платформата е
систематично text-first въпреки Deep Learning DoD v2's "минимум 3 визуализации" правило; проблемът е
project-wide, не Week-6-специфичен. Собственикът изрично забрани Visual Enhancement само за Week 6,
начало на Седмица 7, и всякаква implementation в тази сесия — само architecture/specification.
Пълен диагностичен прочит на `Sedmica6.razor` (928 реда), `CourseCatalog.cs`, CSS pattern inventory
(120 съвпадения concept-map/category-compare/decision-branch/guided-practice-sequence и др.), целия
Interactive component каталог, плюс `06_QA_STRATEGY.md`/`18_INFORMATION_ARCHITECTURE.md`/
`21_CONTENT_AND_DATA_MODEL.md`/`24_IMPLEMENTATION_ROADMAP.md`/`20_TECHNOLOGY_DECISION.md`/
`09_BACKLOG.md`. Находка: дефицитът не е брой визуали (Седмица 6 v2 вече изпълнява DoD-а формално) —
той е (а) липса на един generic, преизползваем graph-rendering компонент (шест bespoke Interactive
компонента съществуват, нула споделена "map" абстракция), и (б) липса на course-level ниво над
отделната седмица, на което mind map/concept map/case map да живеят. Написан пълен 25-секционен
blueprint: `00_PROJECT_OS/_blueprints/COGNITIVE_LEARNING_ARCHITECTURE_v1.md` — Master Mind Map
architecture, Weekly Mind Map standard, Concept Map system (архитектурно разграничен от Mind Map —
tree срещу graph layout), Visual Representation Taxonomy, Memory Anchor/Process/Decision/Comparison
system, Guided Discovery/Retrieval Practice/Cumulative Review архитектура, Longitudinal Case system
(Ирина) + Case Conceptualization Map, Component architecture proposal (1 нов generic `ConceptGraph`
компонент + 2 малки metadata catalog-а + разширение на `.decision-branch`/`.concept-map__flow` —
изрично **без** нова JS graph библиотека), Week 6 gap analysis (не приложена — отделен бъдещ
prompt), Deep Learning DoD v3 proposal (заменя само механичното "минимум 3 визуализации" правило с
Cognitive Representation Coverage checklist), Project OS impact карта, implementation sequence, и
отворени собственически решения. `READY FOR OWNER REVIEW: YES`. Никакъв `.razor`/CSS/тест/
`CourseCatalog.cs` файл не е пипан; Week 6 v2 и Week 12 остават точно както бяха; няма git commit.

Предходен checkpoint — `WEEK 6 v2 — DEEP LEARNING REBUILD — IMPLEMENTED, AWAITING OWNER LEARNING REVIEW` (Сесия 39,
2026-08-14). **Собственикът обяви фундаментална промяна на посоката** (Сесия 38) — замразеният
curriculum build order е поставен на PAUSE: досегашните седмици (6/8/10/12) са преценени като
прекалено кратки/повърхностни за реалната цел на платформата (дълбоко, систематично, source-faithful
учене на КПТ). Нов стандарт: "Deep Learning Week" — пълен прочит на реалната глава от източника (не
резюме от паметта на проекта), Chapter Coverage Matrix, 100% accounted-for покритие, реални визуали,
практика, сериозен финален тест, собственически learning review като последна gate — виж
`06_QA_STRATEGY.md` → "Deep Learning Week — Definition of Done v2" за пълния стандарт.

**Blueprint (Сесия 38):** `00_PROJECT_OS/_blueprints/WEEK6_v2_DEEP_LEARNING_BLUEPRINT.md`
написан и ревизиран до v1.1 (owner-approved, `READY FOR IMPLEMENTATION: YES`). Критична находка:
SRC-041 никога не е бил запазен като файл — само поставен веднъж в чат сесия (Сесия 3,
2026-07-29). Собственикът предостави локалния PDF път; пълният текст на Глава 5 (~41 600 символа)
е извлечен и кеширан в `00_PROJECT_OS/_source_corpus/` (gitignored, никога не се commit-ва). 47
knowledge units идентифицирани, 100% accounted for.

**Implementation (Сесия 39):** `Sedmica6.razor` изцяло преизграден — 14 секции (6.0–6.13), всичките
47 units представени, first-session прецизност навсякъде (v1's "стандартна сесия" генерализация
беше реален, сега поправен source-fidelity дефект). Три нови, индивидуално обосновани компонента:
`WhatIfBox`, `SourceArtifact`, `ScenarioSimulator` (Interactive WebAssembly, multi-level A/B/C
branching симулатор). Нов `.decision-branch` CSS pattern (истинско branching дърво, замества
плосък `.category-compare` grid). Case Lab: Мартин/Ирина (одобрен pilot longitudinal case)/Радо.
U08/U22 включени по owner-approved `OBSERVATIONAL SAFETY BOUNDARY` договор. 13 локални проверки +
20-въпросен финален тест (Q19 пренаписан от platform-policy към source-grounded съдържание).
Нов `Week6ContentSliceTests.cs` (пълен rewrite, 33 факта) + нов `Week6NewComponentTests.cs` (17
факта). **430/430 passing** (396 baseline + 34 нови), build 0/0, 9/9 routes 200. **Некомитнато,
изолирано от все още некомитнатата Седмица 12** (тя не е изоставена — само с по-нисък приоритет,
докато Deep Learning моделът се потвърди). Собственически learning review остава последната gate
преди `COMPLETE` — виж пълни детайли в `10_SESSION_LOG.md` (Сесии 38–39).

Предходен checkpoint — `WEEK 12 — COMPLETE` (Сесия 37, 2026-08-14). Втората седмица от замразения Systematic Curriculum Expansion build order (6→12→7→4→5→2→9→11→15→13→14) е реализирана върху съществуващата архитектура — нула нови reusable компонента, нула нов CSS. Source contract: SRC-041, Глава 14 (confirmed чрез session-log наратив, Сесия 3 продължение 2) — трите широки категории при **негативните** основни вярвания (безпомощност/необичаемост/безполезност, Beck 1999 + J.S. Beck 2005), не core beliefs изобщо; формулировката изрично реконсилирана със Седмица 3's установена позиция, че основните вярвания не са задължително отрицателни. Съдържание: кратък recap на Седмица 3's трипластова йерархия (само линк, без повторно рендериране на диаграмата), дефиниция на основно вярване/схема, трите категории чрез `.category-compare`, изричен academic-only/no-self-assessment boundary, non-scored проверка на разбирането (native `<details>`), обобщение+академичен контекст+източници+`OptionalReadingSource` (Глава 14, без URL). Reuse: `LearningSection`/`LearningObjectives`/`ProgressiveExplanation`/`DisclaimerCallout`/`SourceReferences`/`OptionalReadingSource`, `.category-compare`, `.learning-grid--balanced` (коригиран вариант — grid-ът има директни `<div>` card деца, не Седмица-3-стил single-child anti-pattern). **Curriculum nuance:** Седмица 12 е `CurriculumSafetyLevel.AcademicContextOnly`, затова `DeriveStatus()` я резолва на `CourseWeekStatus.AcademicOverview`, не `Available`, въпреки зададения route — потвърдено structural QA на прясна инстанция; `Kurs.razor` start-panel остава непроменен (същите пет налични седмици), Седмица 12 се появява само в timeline-а с "Академичен обзор" badge и работещ линк. `CourseCatalog.cs`/`Kurs.razor` route + doc-коментари актуализирани. Нов `Week12ContentSliceTests.cs` (21 факта) + минимални актуализации в 6 съществуващи test файла. **396/396 passing** (374 baseline + 22 нови), build 0/0, `git diff --check` чист, 9/9 routes `200` на прясна инстанция (порт 5131), structural QA (1×`<h1>`, коректна h1→h2→h3, 0 bare fragment anchors, всички decorative icon-и aria-hidden). Source QA: всяко твърдение проследено до потвърдения Глава-14 обхват или до вече публикувано Седмица-3 съдържание. Safety QA: explicit anti-diagnostic disclaimer, explicit no-self-assessment boundary, нула интерактивност, нула лични данни. Промените са изолирани — единствено Week-12-related файлове в working tree, все още **некомитнати**, чакат собственическо одобрение преди единствен commit. Следваща стъпка от замразения ред: **Седмица 7** (не е започната автоматично). `10_SESSION_LOG.md` беше открит без запис за предходната Сесия 36 (Optional Reading Visual Refinement Closure) — попълнен ретроактивно в тази сесия, за да не се изгуби историята.

Предходен checkpoint — `OPTIONAL READING VISUAL REFINEMENT — CLOSED, COMMITTED` (Сесия 36, 2026-08-09). Audit на некомитнатия diff от Сесия 34 (продължение) не намери реални дефекти — heading дублирането е премахнато (`SourceReferences.razor`: "Източници и допълнително четене" → "Източници"), визуалната тежест на `.optional-reading` е намалена (`--color-surface` вместо плътен `--accent-academic-surface`, премахнат "Учебник" badge, премахнат `max-width`), 0 dead CSS, 0 orphaned селектори (`.optional-reading__role`/`SourceRole` изцяло премахнати навсякъде), source съдържанието (заглавия/автор/издание/глави/описания) непроменено на всичките 4 засегнати страници. Fresh-server QA потвърди консистентност на всичките 5 седмици (1/3/6/8/10, включително Седмица 6 като regression check). 372/372 теста (непроменен брой — трите нови теста вече бяха включени в baseline-а от Сесия 35). Build 0/0. Затворено в собствен, изолиран commit `70d56cb` ("style: refine optional reading presentation") — Седмица-6 commit-ът `ac0d82e` остава недокоснат, отделен исторически commit. Working tree чист.

**Push/deployment блокер — RESOLVED (2026-08-27, виж "Текуща стъпка" по-горе):** GitHub remote вече съществува (`https://github.com/Deyan2505/CBTLearningPlatform`, public), приложението е конвертирано в standalone Blazor WebAssembly, и `.github/workflows/ci.yml` вече има `deploy` job (publish + Netlify) в допълнение на build+test. Първият push-triggered run на `main` (commit `94314ef`) завърши: Build success, Deploy to Netlify success. Таблицата "Repository" по-долу остава исторически snapshot от 2026-07-30 (дата в заглавието ѝ) — не отразява текущия remote/CI статус.

Предходен checkpoint — `WEEK 6 — COMPLETE` (Сесия 35, Phase C). Първата задача от замразения build order (6→12→7→4→5→2→9→11→15→13→14) е реализирана върху съществуващата архитектура — **нула нови reusable компонента, нула нов CSS**. Source contract: SRC-041, Глава 5 (confirmed чрез session-log наратив + затворения GAP-010). Съдържание: защо сесиите имат структура (общо, безопасно), трипластова форма начало/среда/край (обща характеристика, не дословен списък от прототипа — прототипният мууд-чек "BDI/BAI" изрично изключен, заменен с неклинична формулировка), информационен преглед срещу инструкция за самотерапия (explicit boundary), проверка на разбирането, обобщение+академичен контекст+източници+OptionalReadingSource (Глава 5, без URL). Reuse: `LearningSection`/`LearningObjectives`/`ProgressiveExplanation`/`DisclaimerCallout`/`SourceReferences`/`OptionalReadingSource`, `.category-compare`, `.concept-map__side-notes`. `CourseCatalog.cs` Week 6 route зададен; `Kurs.razor` start-panel актуализиран за петте налични седмици. Нов `Week6ContentSliceTests.cs` (18 факта) + минимални актуализации в 6 съществуващи test файла (available-weeks broят 4→5, remaining 11→10). **372/372 passing**, build 0/0, `git diff --check` чист, 15/15 routes `200` на прясна инстанция, source QA потвърждава точния Глава-5 обхват без надхвърляне. Промените са изолирани от некомитнатата `Optional Reading Visual Refinement` (отделен, все още неодобрен diff) — предстои изолиран Week-6-only commit. Следваща стъпка от замразения ред: **Седмица 12** (не е започната автоматично).

Предходен checkpoint — `SOURCE GAPS REVIEWED — IMPLEMENTATION ORDER READY FOR OWNER APPROVAL` (Сесия 35, Phase B). Върху Phase A архитектурната карта, собственикът поиска source-gap затваряне/ограничаване и замразен build order преди coding. GAP-008 (Седмица 2, REBT-vs-CBT рамка) и GAP-011 (Седмица 15, "три вълни" дати) актуализирани от `Open` на `CONSTRAINED` в `15_GAPS_AND_CONFLICTS.md` — и двете safe да продължат напред с изрично ограничен, cross-source-потвърден allowed-claims обхват (не blocking). Пълни content contracts изведени за Седмица 2/5/9/11/13/14/15; Седмица 4/6/7/12 потвърдени `SOURCE READY: YES`. Нов, преизведен (не механично запазен) build order: **6 → 12 → 7 → 4 → 5 → 2 → 9 → 11 → 15 → 13 → 14**. Batches преработени в 4 нови групи по source-readiness/review-gate профил (A: Clean Source Zero Synthesis — 6/12/7; B: Clinical Care + Source Synthesis — 4/5/2; C: Belief Hierarchy Deepening High Care — 9/11; D: Restricted/Weakest-Sourced/Final — 15/13/14). **First implementation candidate: Седмица 6** (не имплементирана). Седмица 13 и 14 остават `NOT READY` — изискват source-reading pass преди coding. Optional Reading mapping изведен за всичките 11 (confirmed chapter / confirmed theme / pending / not needed), без измислени номера на глави. Планиране само — **не е добавен нов route/component/page, не е направен commit**. Работен tree все още НЕ е чист (виж чекпойнта по-долу).

Предходен checkpoint — `SYSTEMATIC CURRICULUM EXPANSION — PLANNING` (Сесия 35, Phase A). Собственикът стартира Phase A — архитектурна карта за оставащите 11 седмици (2/4/5/6/7/9/11/12/13/14/15), без implementation. Пълна 11-week matrix (primary/secondary архетип, interaction Y/N, визуализации, source status, safety profile, сложност, зависимости), duplication audit (Седмица 8/9/10/11/12 — заключение: без реално дублиране при спазен обхват), clinical safety map (Седмица 4–7 + 9/11/13/14), препоръчан build order (1→11: 2,5,6,4,7,9,11,12,15,13,14) и 4-batch план (A: Foundation Continuity; B: Clinical-Adjacent Process; C: Beliefs & Distortions Deepening; D: Final Integration & Restricted Topics). Нови archetype кандидати: `NONE` — всичките 11 се map-ват на четирите вече валидирани архетипа. Source gaps консолидирани за 7 седмици (2/5/9/11/13/14/15), маркирани `CONTENT DETAIL — NEEDS SOURCE VERIFICATION`, не допълнени по памет. Пълна карта в `24_IMPLEMENTATION_ROADMAP.md` → "Systematic Curriculum Expansion — Architecture Map". Планиране само — **не е добавен нов route/component/page, не е направен commit**. Забележка: working tree в момента НЕ е чист — съдържа некомитнатата "Optional Reading Visual Refinement" (виж чекпойнта по-долу), която тази планинг стъпка не пипа.

Предходен checkpoint (некомитнат) — `OPTIONAL READING VISUAL REFINEMENT READY — OWNER REVIEW REQUIRED` (визуален refinement след Сесия 34 commit-а): source heading опростен на "Източници" (премахнато дублирането с "Допълнително четене"); визуалната тежест на `.optional-reading` намалена (page-compatible `--color-surface` вместо плътен `--accent-academic-surface` фон, премахнат "Учебник" badge, премахнат произволен `max-width`). 351/351 теста. Чака собственическо одобрение преди следващия commit.

Предходен checkpoint — `OPTIONAL READING SOURCE COMPONENT — COMPLETE` (Сесия 34). Собственикът одобри визуално компонента на четирите седмици; изпълнен пълен pre-flight (restore/build 0/0, test 348/348, `git diff --check` чист, 14-route smoke test, четирите week routes с точно по един блок и без рендериран линк) и създаден единствен commit `feat: add optional reading source component`. Нов reusable компонент `OptionalReadingSource.razor` — compact „Допълнително четене" блок за доброволно по-задълбочено четене на оригиналния академичен източник (SRC-041, Джудит С. Бек), добавен на четирите валидирани седмици (1/3/8/10) след SourceReferences. Ключови принципи (закотвени и в тестове): optional reading is supplemental only; platform lessons remain self-contained; no Library phase created; no textbook dependency; public URL is optional and must be verified before use. Няма регистриран публичен URL за SRC-041 → блокът рендерира без бутон навсякъде (**PUBLIC SOURCE URL — PENDING VERIFICATION**, вътрешен статус). Номера на глави само където са потвърдени от реалния прочит (Глава 1 → Седмица 1; Глава 3 → Седмица 3); Седмица 8/10 — тематичен RelevantSection без номер. 12 нови теста (общо 348/348). Пълни детайли в `10_SESSION_LOG.md` (Сесия 34).

Предходен checkpoint — `WEEK 10 COMMITTED` (Сесия 33, финал). Собственикът одобри окончателно Седмица 10, включително опростения Section 08 process design (два responsive режима). Изпълнен пълен pre-flight: restore/build (0/0), test (336/336), `git diff --check` чист, 14/14 routes `200` + приятелски български 404 на прясна инстанция, Week 10 content pre-flight (заглавие/badge/реконсилирани формулировки/без диагностика/без клинично-обучителен език/learner-facing academic context/educational disclaimer), interactive pre-flight (SocraticDialogueExplorer — 5 стъпки, фиксиран сценарий, без input/persistence/scoring), Section 01/08/10 layout contracts, route-safe anchor contract (0 bare fragments, 11 route-safe anchors, динамичен skip-link). Създаден единствен commit `feat: add week 10 guided practice learning slice` (без Co-Authored-By, без AI attribution, без push). Пълни детайли в `10_SESSION_LOG.md` (Сесия 33, финал).

Предходен checkpoint в същата сесия — `WEEK 10 SECTION 08 SIMPLIFIED PROCESS READY — OWNER REVIEW REQUIRED` (Сесия 33, продължение 4). Собственикът отхвърли целия 3×2 layout на Section 08 — не отделна позиция, а самата структура: номер/label не групирани визуално, линии изглеждат отделени, 03→04 неинтуитивен, 06 изолиран. Решено с пълен redesign: точно два responsive режима (wide хоризонтален единичен ред `01→02→03→04→05→06`, narrow вертикална колона), нищо междинно — премахнат целият 3×2/nth-child dead CSS. Нов `.guided-practice-sequence__unit` физически групира номер+label. Connector-и (5, presentation-only) свързват цели units чрез flexbox. Container query (breakpoint 1100px, преизползващ съществуващия `.page-container` ambient контекст) вместо viewport media query. 2 нови теста (общо 336/336). Изрична собственическа инструкция: само Section 08, без съдържание/тема промени, без commit.

Предходен checkpoint в същата сесия — `WEEK 10 VISUAL CORRECTIONS APPLIED — OWNER VISUAL REVIEW REQUIRED` (Сесия 33, продължение 3). Нов собственически визуален преглед докладва 2 забележки: (1) Section 01's turn connector изглеждаше сякаш тръгва от грешна стъпка — root cause: центриран през целия ред вместо подравнен под колоната, в която реално стоят стъпка 3 (край на ред 1) и стъпка 4 (начало на ред 2); поправено с `grid-column: 5`. (2) Section 08's стъпка 3 (диагностицирана чрез structural CSS reasoning, без screenshot инструмент) имаше центриран marker, несъответстващ на съседните ѝ ляво-подравнени стъпки — четеше се като прекъснат/полу-счупен rail; поправено чрез премахване на центрирането (остава само за истински финалната стъпка 6). И двете корекции са чисто CSS grid-placement промени в `app.css` — без DOM, съдържание или reading-order промяна. 334/334 непроменено (нови тестове не бяха нужни).

Предходен checkpoint в същата сесия — `SECTION 08 SEMANTICS FIXED — OWNER VISUAL REVIEW REQUIRED` (Сесия 33, продължение 2). Предходният handoff призна собствен accessibility/semantic дефект: Section 08's `<ol>` съдържаше 11 `<li>` (6 markers + 5 connector-и като отделни sibling list items) вместо точно 6 — connector-ите не са съдържателни стъпки. Поправено: `<ol>` вече съдържа точно 6 `<li>` (по един на стъпка), connector-ите станаха presentation-only `<span aria-hidden="true">`, вложени в предходния `<li>`, без `role="listitem"`. Visual layout едновременно преработен заради директна двусмисленост, докладвана от собственика — предходният 4+2 boustrophedon (ред 2 визуално обърнат) заменен с по-прост 3×2 grid, изцяло без reversal (01→02→03 отгоре, 04→05→06 отдолу, и двата отляво надясно), point positioning чрез explicit CSS Grid placement, не CSS `order`. Съдържанието — непроменено. 3 нови теста (общо 334/334). Изрична собственическа инструкция: без промяна на визуалния process-rail дизайн отвъд disambiguation-а, без промяна на съдържанието, без commit.

Предходен checkpoint в същата сесия — `WEEK 10 SECTION 08 PROCESS RAIL READY — OWNER APPROVAL REQUIRED` (Сесия 33, продължение). Върху трите вече изградени layout корекции (`<h1>` фокус, Section 01, Section 10) собственическият преглед отхвърли визуално Section 08 ("Практическа последователност") — пълноширочинна секция, но шестте стъпки оставаха тесен вертикален stack, огромно неизползвано пространство отдясно. Поправено с нов, изрично scoped `.guided-practice-sequence` pattern (номерирани markers на свързваща хоризонтална релса, **не** reuse на `.concept-map__flow`) — mobile вертикална релса, desktop (900px+) 4+2 converging layout през пълната ширина. Съдържанието (шестте стъпки + educational note) запазено дословно. 3 нови теста (общо 331/331). Изрична собственическа инструкция: само Section 08, без ново съдържание, без нова теория, без промяна на останалите секции, без commit.

Предходен checkpoint в същата сесия — `WEEK 10 FINAL LAYOUT CORRECTIONS READY — OWNER APPROVAL REQUIRED` (Сесия 33, начало). Върху некомитнатата Седмица 10 (Сесия 32) собственическият визуален преглед одобри "Guided Practice" архетипа като цяло и поиска 3 точкови корекции: (1) `<h1>` фокус рамка при навигация — root-cause пре-диагноза установи, че Сесия-23-ото предположение за `tabindex="-1"` семантиката беше фактически невярно; поправено с едно слято, безусловно `outline: none` CSS правило, ретроактивно валидно и за Седмица 1/3/8; (2) Section 01 процесна диаграма — нов scoped `.concept-map__flow--process` responsive модификатор (boustrophedon 6-стъпков grid layout), всяка друга `.concept-map__flow` употреба недокосната; (3) Section 10 празна дясна половина — премахнат single-child `.learning-grid--balanced` wrapper (същият defect клас, недокоснат в Седмица 3), заменен с пълноширочинен `LearningSection` + вътрешна 4-блокова семантична подялба, реализирана чрез естественото CSS Grid auto-placement (0 нов CSS, без `order`). Изрична собственическа инструкция: само тези 3 корекции, без глобален redesign, без ново съдържание, без нова седмица, без промяна на Course Hub, без commit. Validation: build (0/0)/test (328/328, +7 нови спрямо Сесия 32) чисти; `git diff --check` чист; fresh-server structural QA (curl + decoded HTML) потвърждава и трите корекции и липсата на регресия. **Ограничение, отбелязано изрично: няма headless browser/screenshot инструмент в тази среда — QA-то е структурно, не pixel-level визуално; собственикът трябва сам да потвърди визуално на `http://localhost:5055`.** **Ясно разграничение на текущия обхват:**

- **Visual/UX foundation** — `COMPLETE`, `COMMITTED` (hash `115f5fa`).
- **Weekly Course Hub foundation** — `COMPLETE`, `COMMITTED` (`/kurs` route, `CourseCatalog.cs`, `DeriveStatus()` — ADR-010).
- **Седмица 1 (Theory and History архетип)** — `COMPLETE`, `COMMITTED` (`/kurs/sedmica-1`) — архетипът `VALIDATED`.
- **Седмица 3 (Concept and Diagram архетип)** — `COMPLETE`, `COMMITTED` (`/kurs/sedmica-3`) — архетипът `VALIDATED`.
- **Седмица 8 (Simulator Workspace архетип)** — `COMPLETE`, `COMMITTED` (`/kurs/sedmica-8`).
- **Systemic route-safe anchor contract** — `COMPLETE`, `COMMITTED`.
- **Седмица 10 (Guided Practice архетип)** — `COMPLETE`, `COMMITTED` (`/kurs/sedmica-10`) — архетипът `VALIDATED`.
- **Седмица 6 (Guided Practice/Concept and Diagram hybrid)** — `COMPLETE`, `COMMITTED` (`/kurs/sedmica-6`, hash `ac0d82e`) — първата седмица от Systematic Curriculum Expansion build order.
- **Седмица 12 (Concept and Diagram, AcademicOverview архетип)** — `COMPLETE`, `COMMITTED` (`4135988`) (`/kurs/sedmica-12`).
- **Седмица 6 v2 (Deep Learning module)** — `OWNER APPROVED / LOCKED` (Сесия 46, потвърдено отново в чекпойнта от 2026-08-27) — пълен rebuild, 47/47 knowledge units, 14 секции, нов симулатор и 2 нови static компонента.
- **Замразеният curriculum build order** — не е вече на PAUSE: виж "Текуща стъпка" по-горе (Minimum Stability Gate постигнат, Седмица 7 `UNBLOCKED`/`NEXT`).
- **Останалите 9 седмици** — `NOT STARTED` (само метаданни в `CourseCatalog.cs`).
- **Независим академичен/клиничен review на съдържанието** — `PENDING` (RISK-010 — няма щатен рецензент; съдържанието не е публикувано за реални потребители извън локалната разработка).
- **Optional Reading Source компонент** — `COMPLETE`, `COMMITTED` (`OptionalReadingSource.razor`, използван на Седмица 1/3/6/8/10/12).
- **Следваща стъпка** — собственически learning review на Седмица 6 v2. Не Седмица 7, не Седмица 12 продължение, не redesign на друга седмица.

## Последна завършена задача

Global Course + Knowledge Maps Implementation — Phase 5 (Сесия 47, 2026-08-22): нов `/kurs/karta`
route с Course Map (изведена от `CourseCatalog`) и CBT Knowledge Map (нов, source-грундиран
`KnowledgeMapCatalog.cs`, 10 concepts/12 relations от Седмици 3/6/8/10). Реюзва напълно непроменения
LOCKED `ConceptGraph`/`MindMapBranch`. 495/495 теста, build 0/0. `AWAITING OWNER VISUAL + LEARNING
REVIEW`. Week 6 недокоснат (regression потвърдена). Некомитнато. Детайли в `10_SESSION_LOG.md`
(Сесия 47).

Documentation Closeout — Week 6 Cognitive Reference Implementation Owner Approved (Сесия 46,
2026-08-22): собственически финален browser review на `/kurs/sedmica-6` — одобрено. Mind Map Visual
Standard **LOCKED** като project-wide reference (10-точков списък, виж "Текуща стъпка"). Project-wide
Cognitive Learning Architecture reference validation `PASSED`. GAP-013 добавен в
`15_GAPS_AND_CONFLICTS.md` (Седмица 6 "шест цели" срещу 5 намерени клаузи — не блокира, за бъдещ
source-fidelity pass). Следваща оторизирана фаза: Course Map + CBT Knowledge Map на `/kurs/karta` —
не започната тази сесия. Само документация пипната, никакъв код. Детайли в `10_SESSION_LOG.md`
(Сесия 46).

Mind Map Final Visual Polish (Сесия 45, 2026-08-22): чисто CSS полиране — тънък/приглушен shared
trunk срещу удебелен/по-заоблен собствен elbow на всеки branch (премахва "vertical highway"
усещането), navigation вече secondary (без persistent underline), depth-1 branches с калм но по-
силна surface identity, "Цели на сесията" останал leaf (документирана source-fidelity причина).
Нула нови компоненти/данни/съдържание. 476/476 теста, build 0/0. `OWNER PIXEL APPROVAL REQUIRED`.
Некомитнато. Детайли в `10_SESSION_LOG.md` (Сесия 45).

Desktop Spatial Mind Map — Correction Pass 2 (Сесия 44, 2026-08-22): същата `MindMapBranch.razor`
markup вече рефлоу-ва през `@container`/`@supports` CSS в истинска left-to-right spatial branching
на широки контейнери (root ляво, primary branches средна колона, secondary concepts дясна колона),
запазвайки vertical outline tree на mobile — нула нови компоненти, нула WASM, нула дублирани данни.
Node-овете компактни (label + малка отделна `→` navigate връзка, вместо повтарящо се "Виж
секцията →" изречение и inline definition текст). 472/472 теста, build 0/0. `OWNER PIXEL REVIEW
REQUIRED`. Некомитнато. Детайли в `10_SESSION_LOG.md` (Сесия 44).

Mind Map Visual Standard — Correction Pass (Сесия 43, 2026-08-22): MindMap режимът на
`ConceptGraph.razor` пренаписан от responsive card grid на истинско пространствено дърво
(`MindMapBranch.razor`, рекурсивен, native `<details>` disclosure, CSS elbow connectors, depth чрез
typography). Седмица 6 MindMap данните — реална 3-нивова йерархия. ConceptMap/CaseMap непроменени.
470/470 теста, build 0/0. `AWAITING OWNER VISUAL APPROVAL`. Некомитнато. Детайли в
`10_SESSION_LOG.md` (Сесия 43).

Week 6 Cognitive Reference Implementation — Phases 1-3 (Сесия 42, 2026-08-22): семантични модели
(MindMap/ConceptMap/CaseConceptualization) + domain-ignorant `ConceptGraph` renderer + Седмица 6
reference use cases (Preview/Review Mind Map, надградена Concept Map с реални cross-links, Ирина
Case Map, 2 retrieval-practice добавки). DoD v2→v3. 458/458 теста, build 0/0. `AWAITING OWNER
REVIEW` — не `COMPLETE`. Некомитнато. Пълни детайли в `10_SESSION_LOG.md` (Сесия 42).

Project-Wide Cognitive Learning Architecture v1.1 — Revision Pass (Сесия 41, 2026-08-21):
собственическа ревизия на v1 приложена изцяло — Course Map/Knowledge Map разделени, три domain
модела заменят генеричния Node/Edge риск, Weekly Mind Map коригиран (concepts, не section TOC),
curriculum-state semantics преименувани, Retrieval Practice издигнат в самостоятелна архитектура с
нова 4-категорийна таксономия, DoD v3 с две gates, всички собственически решения затворени.
`READY FOR IMPLEMENTATION PROMPT: YES`. Никакъв код пипнат, няма commit. Пълни детайли в
`10_SESSION_LOG.md` (Сесия 41).

Project-Wide Cognitive Learning Architecture v1 (Сесия 40, 2026-08-21): собственически поискана
architecture-only задача след Week 6 v2 owner review — платформата е диагностицирана като
систематично text-first, не Week-6-специфичен проблем. Пълен 25-секционен blueprint написан
(`00_PROJECT_OS/_blueprints/COGNITIVE_LEARNING_ARCHITECTURE_v1.md`), изведен от директен прочит на
реалния код, не от примерна таксономия. `READY FOR OWNER REVIEW: YES`. Никакъв код/CSS/тест пипнат,
Week 6 v2 и Week 12 недокоснати, няма commit. Пълни детайли в `10_SESSION_LOG.md` (Сесия 40).

Week 6 v2 Deep Learning Rebuild — Implementation (Сесия 39, 2026-08-14): пълен rebuild на Седмица 6
по owner-approved blueprint v1.1 — 14 секции, 47/47 knowledge units, нов multi-level branching
симулатор, 2 нови static компонента, 20-въпросен финален тест. 430/430 теста. **Некомитнато** —
чака собственически learning review (последна DoD gate, не автоматична). Пълни детайли в
`10_SESSION_LOG.md` (Сесии 38–39).

Week 12 Implementation — Systematic Curriculum Expansion, Batch A, Second Candidate (Сесия 37, 2026-08-14): реализирана втората седмица от замразения build order — нула нови компонента, нула нов CSS, изцяло reuse на съществуващи patterns. Source contract: SRC-041, Глава 14 (confirmed, scoped към негативните основни вярвания). `AcademicContextOnly` safety level → `AcademicOverview` статус, не `Available`, въпреки зададения route — потвърдено чрез `DeriveStatus()` и structural QA. 396/396 теста. **Некомитнато** — чака собствен Week-12-only commit. Пълни детайли в `10_SESSION_LOG.md` (Сесия 37).

Optional Reading Visual Refinement: Closure + Deployment Check (Сесия 36, 2026-08-09): audit на некомитнатия diff от Сесия 34 (продължение) не намери реални дефекти; затворено в изолиран commit `70d56cb`. Deployment проверка потвърди липсата на GitHub remote/deployment pipeline — докладвано, не заобиколено. Project OS commit `3bd527e`. Пълни детайли в `10_SESSION_LOG.md` (Сесия 36).

Week 6 Implementation — Phase C, First Implementation Candidate (Сесия 35, 2026-08-09): реализирана първата седмица от замразения Systematic Curriculum Expansion build order — нула нови компонента, нула нов CSS, изцяло reuse на съществуващи patterns (`.category-compare`, `.concept-map__side-notes`, shared components). Source contract: SRC-041, Глава 5 (confirmed). Прототипната диагностична референция (BDI/BAI) изрично изключена. 372/372 теста. **Некомитнато** — изолирано от отделната некомитната `Optional Reading Visual Refinement`, чака собствен Week-6-only commit. Пълни детайли в `10_SESSION_LOG.md` (Сесия 35).

Optional Reading Source Component — Final Pre-Flight and Commit (Сесия 34, 2026-08-08): собственикът одобри визуално компонента „Допълнително четене" на четирите валидирани седмици; изпълнен пълен pre-flight (restore/build/test 348/348, 14-route smoke test, four-week block QA) и създаден единствен commit `feat: add optional reading source component`. Компонентът остава supplemental-only, self-contained lessons, без Library phase, без textbook dependency, без публичен URL (pending verification). Виж `10_SESSION_LOG.md` (Сесия 34) за пълен commit hash и diffstat.

Week 10 Final Pre-Flight and Commit (Сесия 33, финал, 2026-08-07): собственикът одобри окончателно Седмица 10 (включително Section 08 simplified process); изпълнен пълен pre-flight (restore/build/test 336/336, 14-route smoke test + 404, Week 10 content pre-flight, interactive pre-flight, Section 01/08/10 layout contracts, route-safe anchor contract) и създаден единствен commit `feat: add week 10 guided practice learning slice`. "Guided Practice" архетипът е `VALIDATED` — четвъртият representative-week архетип на Weekly Course Hub-а (ADR-010). Виж `10_SESSION_LOG.md` (Сесия 33, финал) за пълен commit hash и diffstat.

Section 08 Final Redesign: Simple, Clear, Intuitive Six-Step Process (Сесия 33, продължение 4, 2026-08-06): пълен redesign на Section 08 след собственическо отхвърляне на 3×2 layout подхода — точно два responsive режима (wide хоризонтален единичен ред, narrow вертикална колона), нов `.unit` елемент групиращ номер+label физически, connector-и (5, presentation-only) свързващи цели units чрез flexbox, container query (1100px breakpoint) вместо viewport media query, целият стар 3×2 dead CSS премахнат. 2 нови теста (общо 336/336 passing). **Некомитнато** — чака собственическо одобрение. Пълни детайли в `10_SESSION_LOG.md` (Сесия 33, продължение 4).

Week 10 Visual Corrections: Section 01 Arrow Placement + Section 08 Broken-Flow Look (Сесия 33, продължение 3, 2026-08-06): Section 01's turn connector преместен от центрирано на `grid-column: 5`, точно под/над стъпка 3/стъпка 4, отстранявайки визуалната двусмисленост "тръгва от грешна стъпка". Section 08's стъпка 3 вече не получава несъответстващо центриран marker (само истински финалната стъпка 6 остава центрирана), отстранявайки "полу-счупен" вид. И двете — чисто CSS, без DOM/съдържание промяна. 334/334 непроменено. **Некомитнато** — чака собственическо одобрение. Пълни детайли в `10_SESSION_LOG.md` (Сесия 33, продължение 3).

Section 08 Semantic Correction: Six Steps Must Mean Six List Items (Сесия 33, продължение 2, 2026-08-06): поправен accessibility/semantic дефект — Section 08's `<ol>` вече съдържа точно 6 `<li>` (не 11); connector-ите станаха presentation-only `<span aria-hidden="true">`, вложени във всеки `<li>`, без `role="listitem"`. Visual layout преработен от двусмислен 4+2 boustrophedon на еднозначен 3×2 grid (без reversal, точно като пренасяне на текстов ред), точно по собственическа препоръка при риск от визуална двусмисленост. Съдържанието непроменено. 3 нови теста (общо 334/334 passing). **Некомитнато** — чака собственическо одобрение. Пълни детайли в `10_SESSION_LOG.md` (Сесия 33, продължение 2).

WEEK 10 Redesign Section 08 Only: Practical Sequence as a Real Process Visual (Сесия 33, продължение, 2026-08-06): собственически визуален преглед на трите вече изградени корекции отхвърли Section 08 — пълноширочинна секция с шестте стъпки все още подредени като тесен вертикален stack, огромно неизползвано пространство отдясно. Нов, изрично scoped `.guided-practice-sequence` pattern (номерирани markers на свързваща хоризонтална релса) заменя reuse-а на `.concept-map__flow`; desktop 4+2 converging layout през пълната ширина, mobile вертикална релса. Съдържанието запазено дословно. 3 нови теста (общо 331/331 passing). **Некомитнато** — чака собственическо одобрение. Пълни детайли в `10_SESSION_LOG.md` (Сесия 33, продължение).

WEEK 10 Owner Visual Review — Final Layout Corrections (Сесия 33, 2026-08-06): собственически визуален преглед на некомитнатата Седмица 10 доведе до 3 точкови layout корекции — `<h1>` фокус рамка (глобален CSS root-cause fix, ретроактивно валиден и за Седмица 1/3/8, без техни промени), Section 01 responsive 6-стъпков process layout (нов scoped `.concept-map__flow--process` модификатор, всяка друга употреба недокосната), Section 10 пълноширочинна семантична подялба (премахнат single-child `.learning-grid--balanced` anti-pattern, 0 нов CSS чрез естествения CSS Grid auto-placement). 7 нови теста + 1 коригиран (общо 328/328 passing). **Некомитнато** — чака собственическо одобрение. Пълни детайли в `10_SESSION_LOG.md` (Сесия 33).

CONTENT-DRIVEN TEMPLATE VALIDATION — Slice 3: Седмица 10 (Сесия 32, 2026-08-06): изграден нов route `/kurs/sedmica-10` — четвъртият representative-week архетип ("Guided Practice"), валидиращ че Weekly Course Hub архитектурата (ADR-010) реално мащабира и към насочени, non-scored въпросни упражнения. Един нов interactive компонент (`SocraticDialogueExplorer` — wholesale reuse на `.cbt-diagram`, 0 нови CSS класа). Реконсилирани syllabus твърдения (сократическите въпроси не са разпит/прикрит съвет/риторика; балансираният отговор не е принудителен позитивизъм; декатастрофизирането не омаловажава реален проблем; алтернативното обяснение не отменя първото само защото е по-приятно). 24 нови/актуализирани теста (общо 321/321 passing). **Некомитнато** — чака собственически преглед. Пълни детайли в `10_SESSION_LOG.md` (Сесия 32).

Week 3 Final Pre-Flight and Commit, включващ Systemic Anchor Fix (Сесия 31, 2026-08-06): собственикът одобри Седмица 3, anchor навигацията на Седмици 1/3/8, глобалния skip-link и поправения section flow; изпълнен пълен pre-flight (build/test, 13-route smoke test, anchor navigation pre-flight на трите седмици + skip-link на 6 routes, Week 3 content pre-flight, Week 3 layout pre-flight) и създаден единствен commit `feat: add week 3 cognitive model slice and route-safe anchors`. Виж `10_SESSION_LOG.md` (Сесия 31) за пълен commit hash и diffstat.

Systemic Route-Safe Anchor Fix (Сесия 30, 2026-08-06): систематичен одит и поправка на bare-fragment anchor риска, докладван в Сесия 29 — `Sedmica1.razor` (9 anchors), `Sedmica8.razor` (8 anchors) вече route-safe (`Sedmica3.razor` вече беше поправен). Site-wide skip-link в `MainLayout.razor` поправен динамично чрез `NavigationManager` (`{uri.AbsolutePath}{uri.Query}#main-content`), тъй като целевата страница се променя на всеки route. `<base href="/">` в `App.razor` — недокоснат (задължителен). Нов `SystemicAnchorFixTests.cs` с whole-tree regression тест, потвърждаващ 0 bare fragment hrefs остават никъде в `Components/`. 9 нови/актуализирани теста (общо 296/296 passing). **Некомитнато.** Пълни детайли в `10_SESSION_LOG.md` (Сесия 30).

WEEK 3 Owner Review — Anchor Navigation and Grid Gap Fix (Сесия 29, 2026-08-06): собственически преглед на живо на `/kurs/sedmica-3` докладва 2 blocking дефекта. Root cause #1: `App.razor`-овият `<base href="/">` кара bare `href="#id"` да резолва спрямо "/" (Home), не спрямо текущата страница — поправено само в Седмица 3 чрез пълни пътища; идентичен риск докладван, но **не поправен**, в Седмица 1/Седмица 8 и site-wide skip-link-а в `MainLayout`. Root cause #2: секции 08/09 бяха сдвоени в общ двуколонен ред с несъответстващи височини, оставяйки празна зона под по-късата — поправено чрез разделяне в собствени пълноширочинни секции. 2 нови regression теста (общо 287/287 passing). **Некомитнато.** Пълни детайли в `10_SESSION_LOG.md` (Сесия 29).

CONTENT-DRIVEN TEMPLATE VALIDATION — Slice 2: Седмица 3 (Сесия 28, 2026-08-06): изграден нов route `/kurs/sedmica-3` — третият representative-week архетип ("Concept and Diagram"), валидиращ че Weekly Course Hub архитектурата (ADR-010) реално мащабира към йерархично/диаграмно съдържание, различно и от двата предишни архетипа. Два нови interactive компонента (`CognitiveHierarchyExplorer` — reuse на `.cbt-diagram`; `SchemaFilterDemonstration` — нов малък `.schema-filter` CSS блок за toggle+list). Реконсилирани syllabus твърдения (основните вярвания не са задължително отрицателни; "схема като филтър" изрично обозначена като метафора; "автоматично" не се приравнява на "ирационално"; рефлексивната обработка не гарантира безгрешен резултат). 26 нови/актуализирани теста (общо 285/285 passing). **Некомитнато** — чака собственически преглед. Пълни детайли в `10_SESSION_LOG.md` (Сесия 28).

Week 1 Final Pre-Flight and Commit (Сесия 27, 2026-08-06): собственикът одобри съдържанието и визуалната реализация на Седмица 1; изпълнен пълен pre-flight (restore/build/test, 12-route smoke test, content pre-flight — заглавие/badge/reconciled формулировки/1979 цитат/learner-facing academic context/български review статус/educational disclaimer) и създаден единствен commit `feat: add week 1 CBT history learning slice`. Виж `10_SESSION_LOG.md` (Сесия 27) за пълен commit hash и diffstat.

CONTENT-DRIVEN TEMPLATE VALIDATION — Owner review correction (Сесия 26, 2026-08-06): собственически визуален преглед на Седмица 1 доведе до 9 точкови content-driven корекции — премахнат вътрешен development език от публичния HTML; нов scoped `.research-turn-stepper` responsive CSS Grid (споделеният `.cbt-diagram` остава непроменен); нов `HistoricalTimeline.Compact` density вариант (Course Hub timeline непроменена); нов sidebar "weak context" слой в `MainLayout`; скъсено заглавие; content-format badge вместо availability pill; преформулирана knowledge-check инструкция; Section 09 разбита на 3 подблока; Section 01 сведена до същината си. 10 нови теста (общо 258/258 passing). Пълни детайли в `10_SESSION_LOG.md` (Сесия 26).

CONTENT-DRIVEN TEMPLATE VALIDATION — Slice 1: Седмица 1 (Сесия 25, 2026-08-05): изграден нов route `/kurs/sedmica-1` — вторият representative-week архетип ("Theory and History"), валидиращ че Weekly Course Hub архитектурата (ADR-010) реално мащабира отвъд Седмица 8. Два нови reusable компонента с нула нови CSS класове (`HistoricalTimeline` reuse на `.week-timeline`; `ResearchTurnStepper` reuse на `.cbt-diagram`, адаптиран от `CbtModelDiagram`); `comparison-matrix--dual` (дефиниран, но неизползван преди тази сесия) приложен за перви път; knowledge check чрез native `<details>/<summary>`, не нов quiz engine; `DisclaimerCallout` разширен с optional `Text` параметър (backward-compatible). Source governance: `kpt_syllabus.pdf` използван само като curriculum map; 2 syllabus конфликта реконсилирани (виж `10_SESSION_LOG.md`, Сесия 25). 21 нови/актуализирани теста (общо 248/248 passing). Пълни детайли в `10_SESSION_LOG.md` (Сесия 25).

Final Pre-Flight and Foundation Commit (Сесия 24, 2026-08-05): собственикът одобри натрупаната visual/UX основа от Сесии 17–23 без допълнителни промени; изпълнен пълен pre-flight (validation + 11-route smoke test) и създаден един-единствен foundation commit (`feat: add interactive CBT learning portal foundation`, hash `115f5fa`), обхващащ всичко от предходните седем checkpoint-а. Виж `10_SESSION_LOG.md` (Сесия 24) за пълен commit hash и diffstat.

Final Layout Defect Correction (Сесия 23, 2026-08-04): собственикът извърши реален визуален преглед и докладва 12 конкретни, визуално потвърдени проблема. **Двата blocking проблема бяха реални root-cause bugs, не козметика:** (1) **хоризонтален overflow** — grid items по подразбиране имат `min-width: auto` (не `0`), затова широко/непрекъсваемо съдържание в която и да е `.learning-grid` колона можеше да разшири цялата страница вместо да задейства локалния `overflow-x: auto` на `.comparison-matrix-wrapper` — коригирано с `.learning-grid > * { min-width: 0; align-self: start; }` (втората половина на same fix едновременно реши и #2); (2) **опънати колони с празно пространство** — `align-items: start` на самия grid контейнер не е достатъчен, ако конкретен grid item няма собствен `align-self: start`; същото правило по-горе го гарантира за всеки item; (3) **дебела синя рамка около `<h1>`** — Blazor `<FocusOnNavigate Selector="h1">` реално добавя `tabindex="-1"` и фокусира h1 при всяка навигация (за screen reader announcement, не истинска клавиатурна фокусировка) — генеричното `[tabindex]:focus-visible` правило го третираше като бутон; добавено отделно, по-меко правило само за `h1/h2/h3[tabindex="-1"]`. Допълнително: nested "карта в карта" chrome премахнат (ModuleCard вече без собствена пълна рамка — divider pattern); `DisclaimerCallout` вече поддържа `Variant` ("educational" спокоен индиго по подразбиране, "safety" остава запазен за силни ограничения) — вече не изглежда като error съобщение; Модул 1/Модул 2 дясната колона преработена в реален `.module-path` (номерирани link nodes + connector линия + ясно разграничен status node) + нова concept map секция; 18 duplicate role-label/heading двойки коригирани в цялото приложение (напр. "Проверка"/"Проверка на разбирането" → "Рефлексия"/"Проверка на разбирането"); "Наличен" статус текст премахнат за наличните уроци (CTA вече го подразбира); sidebar разширен на 276px с по-плътен padding (без неудобно пренасяне на "Обучителна програма"); Home hero компресиран (по-малко top padding, по-стегнат `line-height` на h1); top bar получи малка platform икона. 42 нови теста (общо 227/227 passing). Пълни детайли в `10_SESSION_LOG.md` (Сесия 23).

## Repository — статус (2026-07-30)

| Параметър | Стойност |
|---|---|
| Repository root | project root (съдържа `00_PROJECT_OS/`, `code_artifact.html`, `CbtLearningPlatform/`) — потвърдено чрез `git rev-parse --show-toplevel` |
| Branch | `main` |
| `.gitignore` | официален `dotnet new gitignore` темплейт, `.vscode/` селективно (не изцяло игнорирана) |
| Git identity | зададена **локално** (`--local`, само това repository); global конфигурация непроменена |
| Baseline commit | **CREATED** — hash виж `10_SESSION_LOG.md` (не се записва email в Project OS) |
| Remote | **STALE (2026-07-30) — виж "Текуща стъпка" по-горе:** вече съществува, `https://github.com/Deyan2505/CBTLearningPlatform` (public) |
| CI workflow | **STALE (2026-07-30):** `.github/workflows/ci.yml` вече има и `deploy` job (build+test+publish+Netlify); изпълнен на GitHub — Build success, Deploy to Netlify success (commit `94314ef`) |
| Test project | `CbtLearningPlatform.Tests` (xUnit, `net10.0`) — 4 теста, всички passing |
| Error handling | `ErrorBoundary` (Routes.razor) + българска `Error.razor` + преведен `#blazor-error-ui` банер; server-side `UseExceptionHandler` от темплейта, проверен реално в Production среда |
| Design system | `app.css` — tokens (color/typography/spacing/shape/layout) + компоненти (бутони/карти/callout/nav/форми); `DisclaimerCallout` shared component; `MainLayout`/`Home`/`NotFound` реализирани с реално съдържание |
| Public pages | `/`, `/programa`, `/kpt`, `/programa/modul-1` + 1 урок, `/programa/modul-2` + 3 урока — реални, честни, с `ModuleCard`/`LearningObjectives`/`SourceReferences` компоненти; nav active state (`NavLink`); Google Fonts CDN — временно решение (Variant B), не окончателно production |
| Learning content | Модул 1 (1 урок) + Модул 2 (3 урока, горна граница на REQ-CONT-002) — `REQUIRES PROFESSIONAL REVIEW`, не публикувани за реални потребители (RISK-010, няма щатен клиничен рецензент) |
| Content architecture | `KEEP RAZOR FOR MVP` (Content Pipeline Decision, STEP-3.3) — Markdown/JSON pipeline не се изгражда; преразглежда се при съдържание извън капацитета на флагманския Модул 2 |

## Environment — актуален статус (2026-07-30)

| Компонент | Статус |
|---|---|
| .NET SDK | `10.0.302` инсталиран и потвърден (`C:\Program Files\dotnet\sdk\10.0.302`) |
| .NET Runtimes | 6.0.35, 8.0.17 (запазени непроменени), 10.0.10 (нови) |
| VS Code / Claude Code / Git | Работят |
| Visual Studio (пълен IDE) | Все още не е инсталирана — `OPTIONAL`, не блокира |
| Blazor Web App solution | Съществува — `CbtLearningPlatform/` (2 проекта + `.sln` + `global.json`), build чист след Git init (0/0) |
| Git repository в проекта | **Съществува** (project root), 4 commits (baseline + CI workflow + test foundation + error handling), няма remote |

## Какво е проверено

- `git rev-parse --show-toplevel` → сочи към project root, не към `CbtLearningPlatform/`.
- `git check-ignore -v` → `bin/`/`obj/` пътища игнорирани; `02_CURRENT_STATUS.md`, `code_artifact.html`, `global.json`, `.sln` — НЕ игнорирани.
- `git status --short` след `git add .` → точно 48 файла (A), без `bin/`/`obj/`/`.vs/`.
- `git diff --cached --stat` → `48 files changed, 4538 insertions(+)`.
- Secret scan (`.env`, `*.pfx`/`*.p12`/`*.pem`/`*.key`, `appsettings*` съдържание) → без открити тайни.
- `dotnet restore` + `dotnet build` след Git init → 0 Warning(s), 0 Error(s) (без регресия).
- `code_artifact.html` — непроменен (122142 bytes, timestamp непроменен).

## Активни проблеми

- Visual Studio (пълен IDE) все още не е инсталирана — `RECOMMENDED`, не блокира.
- Клиничен рецензент все още липсва — не блокира техническа работа.
- ~~Anchor-navigation риск (bare `href="#id"` резолва спрямо `App.razor`-овия `<base href="/">`)~~ — **RESOLVED и committed (Сесия 31).** Виж `10_SESSION_LOG.md` (Сесии 29–31) за пълната диагноза и поправка.
- **Повтарящ се environment проблем:** `dotnet run` фонови процеси в тази Windows/Git-Bash среда понякога надживяват bash `kill` (не се виждат от `ps aux`), задържайки порт 5055 за следваща сесия/стъпка — потвърдено седем пъти (Сесия 17–23). Обратен случай, открит в Сесия 23: `nohup ... &` фонов процес умря сам (без crash в лога) между стъпки, оставяйки `http://localhost:5055` недостъпен за собственика — коригирано с `disown` след `&`, не гарантирано решение за тази среда. Друга находка (Сесия 23): `Get-NetTCPConnection -LocalPort 5055` понякога показва "призрачни" `TimeWait` записи с `OwningProcess 0` (System Idle Process) от вече затворени HTTP заявки — тези НЕ блокират нов `LISTEN`; проверявай `State` колоната, не само присъствие на запис, преди да заключиш, че портът е зает. Винаги проверявай `Get-NetTCPConnection -LocalPort 5055` + `Get-Process -Name "CbtLearningPlatform"` през PowerShell преди нов smoke test, не разчитай само на предходно "процесът спрян чисто" съобщение от bash.

## Блокиращи проблеми

Няма.

## Следваща препоръчана задача

**Седмица 7** — `UNBLOCKED`, следваща (виж "Текуща стъпка" по-горе: Minimum Stability Gate постигнат,
Седмица 3 v2/Седмица 6/Седмица 8 v2 всичките `LOCKED`/`OWNER APPROVED`, deploy pipeline `OPERATIONAL`).
Не се пипа Седмица 6 (LOCKED). Не се прави retrofit на вече завършени седмици извън изрична
собственическа заявка. GAP-013 остава отворен (не адресиран) за бъдещ source-fidelity pass.
`KEEP RAZOR FOR MVP` в сила.

## Последна актуализация

2026-08-27 — Pre-Week-7 repository checkpoint (standalone WASM migration + Week 8 v2 owner approval +
GitHub CI/Netlify deployment operational).

## Общ приблизителен прогрес

Фаза 0: 100%. Фаза 1: ~92% (5.5 от 6 STEP-а). Фаза 2: ~40% (design system + 2 публични страници). Фаза 3: ~30% (7 страници, 4 реални урока — Модул 1 + флагманският Модул 2 съдържателно завършени за MVP). Общ проект (Фази 0–9): ~34%.
