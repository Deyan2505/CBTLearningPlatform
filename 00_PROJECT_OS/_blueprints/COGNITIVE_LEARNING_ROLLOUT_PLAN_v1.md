# COGNITIVE LEARNING ROLLOUT PLAN v1

**Сесия 50 · 2026-08-23 · КПТ Академия Project OS**
**Обхват:** Read-only audit + migration blueprint. Никакъв `.razor`/CSS/`CourseCatalog.cs`/`KnowledgeMapCatalog.cs`/тест файл не е пипнат при написването на този документ.
**Status:** AUDIT COMPLETE — planning document, не implementation.

Owner финално одобри: Course Map (`LOCKED`), CBT Knowledge Map (`OWNER APPROVED`, project-wide reference standard за network-стил concept maps, commit `67aec83`), Week 6 Cognitive Reference Implementation (`OWNER APPROVED`), Global Cognitive Maps Phase (`PASSED`). Този документ отговаря на следващия въпрос: **какво е реалното състояние на вече routed седмици спрямо този вече доказан standard, и какъв е най-краткият надежден път към единна платформа?**

---

## 1. Executive diagnosis

Проектът реално съдържа **три различни поколения на lesson quality**, не едно последователно ниво:

1. **Generation 0 — "Content-driven template validation"** (Седмици 1, 3, 8, 10, 12). Текстово солиден, source-цитиран, safety-съобразен материал, изграден по последователни архетипи ("Theory and History", "Concept and Diagram", "Simulator Workspace", "Guided Practice", "Academic Overview"). Всяка седмица reconcile-ва конкретни syllabus твърдения срещу вече locked съдържание (документирано в собствения ѝ header коментар) — това е реална, не козметична строгост. Но: retrieval/assessment е тънък и несистемен (3–4 recognition-only въпроса, в един случай — Седмица 8 — буквално без reveal/answer check), няма Knowledge-Unit/Coverage-Matrix доказателство за source coverage, няма дедикирана terminology секция, няма case-based приложение, cognitive representation е статичен CSS chain (`.concept-map__flow`), не semantic-model-driven `ConceptGraph`.
2. **Generation 1 — "Systematic Curriculum Expansion" reference"** (Седмица 6). Numbered Knowledge Units (U01–U46+) цитирани директно в assessment feedback, 13 embedded local checks + 20-въпросен финален тест с разнообразни типове (ordering, scenario, true/false+explain, matching, error-identification), дедикирана terminology секция, Case Lab с фиктивен случай (Ирина), интерактивен ScenarioSimulator (Level A/B/C), Mind Map preview/review + Concept Map + Case Map — и трите чрез общия semantic-model-driven `ConceptGraph.razor` engine.
3. **Generation 2 — "Project-wide Cognitive Learning Architecture"** (Course Map + CBT Knowledge Map, тази сесия). Global, cross-week представяне — навигационно (Course Map) и knowledge-relational (Knowledge Map, вече с реални SVG edges) — и двете изведени изцяло от съществуващи данни, нула дублиран dataset.

Честна оценка: **Generations 0 и 1 не са "малко зад" едно от друго — те са изградени по различни стандарти, преди самият стандарт да съществува.** Седмица 6 не е "малко по-добра версия" на Седмица 3/8/10 — тя демонстрира capabilities (Knowledge Units, diverse retrieval, case-based application, semantic maps), които просто не съществуваха, когато Седмици 1/3/8/10/12 са писани. Това не е причина да ги пренапишем механично отначало — Representation Fit (§10 на заданието) означава, че не всяка седмица се нуждае от Case Lab или 20-въпросен тест. Но означава, че **source coverage за никоя от петте по-стари седмици не може да бъде потвърдено 100%** — тъй като нямат Knowledge-Unit numbering система изобщо.

---

## 2. Current routed curriculum inventory

Изведено директно от `CourseCatalog.cs` (не от памет/summary), 2026-08-23:

| # | Заглавие | Модул | Safety Level | Route (committed) | Route (working tree) | Формати |
|---|---|---|---|---|---|---|
| 1 | Как се ражда когнитивната терапия | Модул I | PublicCore | `/kurs/sedmica-1` | same | InteractiveModel |
| 2 | Когнитивна терапия на Бек и REBT на Елис | Модул I | PublicCore | `null` (Upcoming) | same | Comparison |
| 3 | Архитектура на когнитивния модел | Модул I | PublicCore | `/kurs/sedmica-3` | same | InteractiveModel |
| 4 | Клинична оценка и когнитивна концептуализация | Модул II | AcademicContextOnly | `null` | same | AcademicOnly |
| 5 | Принципи на КПТ и терапевтичен съюз | Модул II | PublicWithAdaptation | `null` | same | GuidedDemonstration |
| 6 | Структура на терапевтичната сесия | Модул II | PublicWithAdaptation | `/kurs/sedmica-6` | same | GuidedDemonstration |
| 7 | Поведенческа активация | Модул II | PublicWithAdaptation | `null` (Upcoming) | same | Simulator |
| 8 | Автоматични мисли и емоции | Модул III | PublicCore | `/kurs/sedmica-8` | same | InteractiveModel, Simulator, KnowledgeCheck |
| 9 | Когнитивни изкривявания и дневник на мислите | Модул III | PublicWithAdaptation | `null` | same | Simulator |
| 10 | Сократически въпроси и съвместно изследване | Модул III | PublicCore | `/kurs/sedmica-10` | same | GuidedDemonstration |
| 11 | Междинни вярвания | Модул III | ProfessionalReviewRequired | `null` | same | InteractiveModel |
| 12 | Основни вярвания и схеми | Модул III | AcademicContextOnly | **`null` (committed)** | **`/kurs/sedmica-12` (uncommitted!)** | AcademicOnly |
| 13–15 | (Разширени техники и академичен контекст) | Модул IV | различни | `null` | same | различни |

**Реално routed/implemented седмици за този audit: 1, 3, 6, 8, 10, 12.** Седмица 12 съществува само в **некомитнатото работно дърво** — `git diff` на `CourseCatalog.cs`/`Kurs.razor` показва, че редът `route: "/kurs/sedmica-12"` (заедно с `Sedmica12.razor` и `Week12ContentSliceTests.cs`, и двата untracked) все още не е commit-нат. Прочетена е read-only за пълнота на audit-а (владелецът поиска изрично това в §7), но **не е редактирана**, и всяка препоръка за нея по-долу изрично флагва тази зависимост.

---

## 3. Reference standard

**Week 6 ("Структура на терапевтичната сесия") + Global Cognitive Maps (`/kurs/karta`).** Пълни детайли в `10_SESSION_LOG.md` Сесии 35/42-47 и в самия `Sedmica6.razor`. Ключови, verified факти (не от памет — grep-нати директно от файла тази сесия): 14 секции (6.0–6.13), Knowledge Units U01–U46+ цитирани в assessment feedback, 13 embedded local checks + 20-въпросен финален тест (diverse типове), Mind Map preview (6.0) и review (6.12, explain-before-reveal), Concept Map (6.5), Case Lab с Ирина + Case Map (6.8), ScenarioSimulator Level A/B/C (6.9), дедикирана terminology секция (6.3). **Week 6 е REFERENCE, не TEMPLATE — не всяка седмица трябва да копира точния ѝ формат** (Representation Fit, §10 на заданието).

---

## 4. Audit method

Прочетени изцяло, директно от диска, тази сесия (не от Project OS summary, не от тестове, не от `CourseCatalog.ShortSummary`): `Sedmica1.razor` (270 реда), `Sedmica3.razor` (352 реда), `Sedmica8.razor` (229 реда), `Sedmica10.razor` (413 реда), `Sedmica12.razor` (215 реда, uncommitted). `Sedmica6.razor` (1036 реда) — вече дълбоко познат от Сесии 35/42-49 на този проект; тази сесия допълнително verified чрез targeted `grep` на всичките ѝ `<h2 id=`/interactive-island/assessment маркери, за да се потвърдят конкретните числа (13+20 checks, 4-те `ConceptGraph` инстанции) вместо да се разчита на спомен. `CourseCatalog.cs` прочетен целият за §2. `git diff` изпълнен за Седмица 12 verification. `KnowledgeMapCatalog.cs`'s собствен source-grounding коментар (написан в Сесия 47, source-verified тогава) преизползван за §15's "already confirmed" редове, не пре-verified от нулата тук.

---

## 5. Per-week audit

### Week 1 — Как се ражда когнитивната терапия

**Current generation:** 0 (Content-driven template validation, "Theory and History" archetype).
**Source status:** SRC-041 + примарен цитат (Beck, Rush, Shaw & Emery, 1979), и двете дати независимо потвърдени (GAP-012 closed, документирано в собствения header коментар).
**Coverage status:** `UNKNOWN — REQUIRES DEEP SOURCE PASS`. Няма Knowledge-Unit numbering или Coverage Matrix за тази седмица — не може да се докаже 100% source coverage.
**Depth:** Реален исторически наратив с нюанс — explicit reconciliation на 2 syllabus твърдения (автоматичните мисли не са "невалидни"; психоанализата не е "победена", а change на изследователски въпрос). Не superficial, но и не задълбочена terminology-ниво трактовка — една седмица, тесен обхват по дизайн.
**Terminology:** Няма дедикирана terminology секция. Термини ("автоматична мисъл") обяснени inline, с 1 worked пример.
**Cognitive representation:** `HistoricalTimeline` (компонент, 6 milestones) + 1 comparison-matrix таблица (стар vs нов изследователски въпрос). Няма Concept Map, Process Diagram, Decision Tree.
**Retrieval:** Само 3-въпросен end-of-page MCQ quiz с reveal. Няма embedded checks през текста, няма explain-before-reveal, няма ordering/reconstruct механизъм.
**Application:** Един малък worked пример (SMS сценарий). Няма Case Lab, няма simulator за практика (`ResearchTurnStepper` е explainer/animation, не decision practice).
**Reasoning:** Минимален — stepper-ът показва Бек-овото разсъждение, но не изисква learner-ът да разсъждава сам.
**Assessment:** 3 recognition-only MCQ, единствен тип, обяснения без source-unit citations.
**Knowledge Map readiness:** `Not applicable` — съзнателно изключена от global map (чисто наративно съдържание, документирано в `KnowledgeMapCatalog.cs`'s собствен коментар).
**Migration class:** **C, граничещо с B** (retrieval/assessment/terminology-та имат нужда от cognitive-layer работа; source coverage е формално `UNKNOWN`, което тегли към лек B-елемент, но самото съдържание не изисква пълен rebuild).

**Missing:** Knowledge-Unit/Coverage-Matrix доказателство; дедикирана terminology секция; embedded retrieval; case/scenario reasoning practice; повече от 1 assessment тип.

**Recommended migration:** Не пълен rebuild. (1) Coverage Matrix pass срещу SRC-041 + 1979 primary source — потвърждава или коригира твърдение за пълнота. (2) Малка terminology card секция (5-6 термина). (3) 1-2 нови retrieval механизма (виж §22 по-долу). (4) НЕ добавяй Case Lab/simulator — не пасва на "Theory and History" archetype-а (Representation Fit).

**Source actions required:** Пълно извличане на relevant SRC-041 глава(и) за историческия контекст + 1979 primary source в `_source_corpus/` (gitignored), после Knowledge-Unit номериране, после Coverage Matrix спрямо съществуващия текст.

**Candidate cognitive representations:** Няма нужда от нов Concept/Process/Decision/Comparison тип отвъд вече съществуващата comparison-matrix — тя вече е подходяща форма за "стар vs нов изследователски въпрос".

**Retrieval opportunities:** (1) Ordering/reconstruct exercise, преизползващ съществуващите `HistoricalTimeline` milestones (learner подрежда стъпките преди да разгъне). (2) Explain-before-reveal за "защо се променя изследователският въпрос" (огледало на Седмица 6's review-map pattern).

**Knowledge Map impact:** `Not applicable` (потвърдено, не преразглеждано).

---

### Week 3 — Архитектура на когнитивния модел

**Current generation:** 0, но с най-силната interactivity сред петте (2 WASM острова: `CognitiveHierarchyExplorer`, `SchemaFilterDemonstration`).
**Source status:** SRC-041 Ch. 3 + класическата Beck triad (потвърдена срещу множество registered sources, SRC-025/035/037).
**Coverage status:** `UNKNOWN — REQUIRES DEEP SOURCE PASS`. Без Knowledge-Unit numbering.
**Depth:** Силна — 3-нивова йерархия, triad, automatic vs. reflexive processing сравнение, "чести обърквания" класификация (3 примера с reveal), explicit reconciliation на 3 syllabus твърдения (core beliefs не са задължително негативни; "схема като филтър" изрично маркирана като метафора, не буквален механизъм; "automatic" ≠ "irrational").
**Terminology:** Няма дедикирана секция (термините се обясняват inline, добре, но не са консолидирани).
**Cognitive representation:** 3 отделни `.concept-map__flow` статични CSS вериги (различни от `ConceptGraph`/semantic model system) + `learning-path-diagram` 3-стъпков модел + 2 WASM interactive explorers. Визуално richer от Седмица 1, но архитектурно различен механизъм от Седмица 6/global maps (виж §6).
**Retrieval:** 4-въпросен end-quiz (recognition-only) + отделно 3-примерна класификационна проверка (реален, макар и малък retrieval елемент). Няма embedded checks през основния текст.
**Application:** Няма Case Lab. `SchemaFilterDemonstration` е fixed neutral scenario, не learner-specific practice.
**Reasoning:** Умерен — класификационното упражнение изисква реално разграничаване (мисъл vs. вярване vs. основно вярване), но е само 3 примера.
**Assessment:** 4 recognition-only MCQ, без source-unit citations.
**Knowledge Map readiness:** **Вече силно интегрирана** — situation/automatic-thought/behavior/cognitive-model/intermediate-belief/core-belief всичките сочат тук (`#situacia-znachenie`/`#tri-niva`).
**Migration class:** **B** (Substantial Deep Learning Upgrade) — не защото съдържанието е слабо, а защото това е **prerequisite-critical** седмица (виж §9/§25) и текущият retrieval/coverage-gap тук се пропагира във всяка следваща седмица, която реферира тази йерархия.

**Missing:** Coverage Matrix; дедикирана terminology секция; embedded retrieval низ основния текст; ConceptGraph-базирана (не `.concept-map__flow`) визуализация, съгласувана с global map стандарта.

**Recommended migration:** (1) Coverage Matrix + Knowledge Units — най-висок приоритет сред петте (виж §25). (2) Terminology card секция. (3) Разшири класификационното упражнение с повече retrieval разнообразие (виж §22). (4) Обмисли upgrade на 3-те `.concept-map__flow` диаграми към реален `ConceptMapModel`/`ConceptGraph` (архитектурна консистентност с global map — owner decision, виж §24).

**Source actions required:** Пълно извличане на SRC-041 Ch. 3 в `_source_corpus/`, Knowledge Units, Coverage Matrix.

**Candidate cognitive representations:** Real Concept Map (upgrade от `.concept-map__flow`) за 3-нивовата йерархия + triad, за консистентност с global standard. Comparison Matrix вече съществува (automatic vs reflexive) — добра форма, запази я.

**Retrieval opportunities:** (1) Blank-hierarchy/label-the-model — покрий labels на съществуващата 3-стъпкова диаграма, learner ги възстановява. (2) Разшири "чести обърквания" в explain-before-reveal с learner-generated примери. (3) Compare-from-memory за automatic vs reflexive таблицата.

**Knowledge Map impact:** `Confirmed` за 6-те вече свързани concepts. `Candidate — needs source verification`: Cognitive Triad (Аз/Светът/Бъдещето) — реално покрита тук в дълбочина, но страницата я рамкира като исторически, depression-специфичен модел; needs owner decision дали заслужава собствени map nodes или остава prose-only (виж §24).

---

### Week 8 — Автоматични мисли и емоции

**Current generation:** 0, с най-много interactive islands (3: `CbtChainSimulator`, `InterpretationExample`, `CategorizationCheck`) — реално отговаря на `InteractiveFormat.Simulator` label-а си от `CourseCatalog`.
**Source status:** SRC-041 Ch. 3/9/10 (модел, идентифициране на мисли, идентифициране на емоции) — три отделни глави, изрично цитирани.
**Coverage status:** `UNKNOWN — REQUIRES DEEP SOURCE PASS`.
**Depth:** Добра — конкретен работен пример (закъснение за среща, две интерпретации), factи vs. интерпретация разграничение, comparison-matrix за 4-те категории (мисъл/емоция/телесна реакция/поведение).
**Terminology:** Няма дедикирана секция; comparison-matrix-ата частично служи тази роля.
**Cognitive representation:** `.concept-map__flow` (Ситуация→Мисъл→[Емоция↔Телесна реакция]→Поведение) + comparison matrix + 2 допълнителни WASM demos.
**Retrieval:** **Най-слабото звено сред петте** — секция 07 "Проверка на разбирането" е **само 4 open reflection въпроса, БЕЗ answer reveal, без scoring, без структура**. Единственият реален structured check е `CategorizationCheck` (WASM компонент) — недостатъчно за седмица с този обем съдържание.
**Application:** `CbtChainSimulator` + `InterpretationExample` дават реална guided practice, но не case-based (без фиктивен герой).
**Reasoning:** Добър — двете интерпретации/два пътя демонстрация изисква реално сравняване на последствия.
**Assessment:** Практически `MISSING` — 4-те reflection въпроса нямат нито един reveal/answer механизъм.
**Knowledge Map readiness:** **Най-силно интегрирана седмица** — Emotion/Body-reaction get their `IntroducedWeek=8` именно оттук (Седмица 3's собствена верига ги обединява в едно, документирано в `KnowledgeMapCatalog.cs`).
**Migration class:** **B** — retrieval/assessment gap-ът тук е най-острият сред петте (буквално нула structured check), а централността на седмицата за global map-а прави това с висок приоритет.

**Missing:** Всякакъв structured knowledge-check с reveal; Coverage Matrix; дедикирана terminology секция; case-based приложение.

**Recommended migration:** (1) **Спешно** — добави реален structured check (reveal-based, не само reflection) към секция 07. (2) Coverage Matrix + Knowledge Units. (3) Terminology консолидация.

**Source actions required:** Извличане на SRC-041 Ch. 3/9/10 в `_source_corpus/`, Knowledge Units, Coverage Matrix.

**Candidate cognitive representations:** `.concept-map__flow`-ът тук вече е добра форма — upgrade към `ConceptGraph` само ако цялостна архитектурна консистентност се реши positively (виж owner decision §24), не сам по себе си спешно.

**Retrieval opportunities:** (1) **Приоритет:** добави reveal-базиран check към reflection въпросите (explain-before-reveal формат). (2) Reconstruct-the-chain — разбъркана Ситуация→Мисъл→Емоция→Поведение верига, learner подрежда. (3) Compare-from-memory за 4-категорийната таблица.

**Knowledge Map impact:** `Confirmed` за situation/automatic-thought/emotion/body-reaction/behavior/cognitive-model (всички вече сочат тук). `Candidate — needs source verification`: "Факт срещу интерпретация" и "Мисъл срещу чувство" — реални, но е възможно да са pedagogical device, не самостоятелен CBT concept; owner решение нужно.

---

### Week 10 — Сократически въпроси и съвместно изследване

**Current generation:** 0, с най-структурирания end-quiz сред петте (4 MCQ, всичките distinguishing/comparing типове, не чисто recognition).
**Source status:** SRC-041 (guided discovery, Socratic questioning, collaborative empiricism).
**Coverage status:** `UNKNOWN — REQUIRES DEEP SOURCE PASS`.
**Depth:** Добра — 4 семейства въпроси, "въпрос vs прикрит съвет" класификация (4 примера с reveal), факт/предположение/заключение триада, декатастрофизиране като собствена подсекция, "балансиран отговор" сравнение (3 варианта).
**Terminology:** Няма дедикирана секция, но 4-те "семейства въпроси" карти частично служат тази роля.
**Cognitive representation:** `.concept-map__flow` (6-стъпков процес) + `guided-practice-sequence` (**вече готов Process Diagram candidate** — номерирана 6-стъпкова последователност) + `SocraticDialogueExplorer` (1 WASM остров).
**Retrieval:** 4 класификационни примера (04) + 4-въпросен финален MCQ, всичките с reveal. Най-добрата структура сред петте, но все още само end-of-page + едно вградено упражнение, не Седмица-6-ниво разпръснато.
**Application:** `SocraticDialogueExplorer` дава guided practice; няма case-based фиктивен герой.
**Reasoning:** Добър — декатастрофизиране и "балансиран отговор" секциите изискват реално разграничаване на nuance (не просто позитивно мислене).
**Assessment:** 4 MCQ, разнообразие в типа въпроса (не само recognition), но без source-unit citations.
**Knowledge Map readiness:** `Confirmed` за socratic-question (`#izsledvane`).
**Migration class:** **C** — най-близо до готовия standard сред петте; основно нужен formalization pass, не rebuild.

**Missing:** Coverage Matrix; дедикирана terminology секция; `guided-practice-sequence`-ът не е формализиран като recognized "Process Diagram" pattern другаде.

**Recommended migration:** (1) Coverage Matrix + Knowledge Units. (2) Formalize `guided-practice-sequence` като Process Diagram pattern, преизползваем за бъдещи седмици със стъпкови техники. (3) Дребна terminology консолидация.

**Source actions required:** Извличане на SRC-041's guided-discovery/Socratic глава, Knowledge Units, Coverage Matrix.

**Candidate cognitive representations:** `guided-practice-sequence`-ът вече Е Process Diagram — не нов тип, а potvarждение/formalization на съществуващ pattern.

**Retrieval opportunities:** (1) Ordering exercise, преизползващ `guided-practice-sequence`-а (разбъркан, learner подрежда). (2) Explain-before-reveal разширение на "въпрос vs прикрит съвет" с learner-generated примери.

**Knowledge Map impact:** `Confirmed` за socratic-question. `Candidate — needs source verification`: Декатастрофизиране, Балансиран отговор — реални, именувани техники/concepts, потенциално заслужаващи собствени map nodes.

---

### Week 12 — Основни вярвания и схеми (⚠ UNCOMMITTED WORKING TREE)

**⚠ Внимание:** Route-ът, файлът `Sedmica12.razor`, и тестовете `Week12ContentSliceTests.cs` съществуват само в **некомитнатото работно дърво**. Всичко по-долу reflect-ва актуалното локално състояние, но остава unverified от git история — owner решение е нужно преди retrofit да докосне тези файлове (виж §24).

**Current generation:** 0, деliberately минимална по дизайн (`AcademicContextOnly`, "zero interaction, strictly third-person" по собствения си header коментар).
**Source status:** SRC-041 Глава 14, потвърдена срещу session log (Сесия 3). Explicit reconciliation срещу Седмица 3's вече публикувано твърдение ("core beliefs не са задължително негативни") — добра дисциплина, не противоречие.
**Coverage status:** `UNKNOWN — REQUIRES DEEP SOURCE PASS`.
**Depth:** Тясна, но целенасочена — 3-те категории (безпомощност/необичаемост/безполезност), explicit boundary защо остава "академичен обзор, не самооценка". Не дублира Седмица 3's диаграма (добра discipline, документирана в header коментара).
**Terminology:** Няма дедикирана секция; 3-те категории вече са представени като comparison-style карти.
**Cognitive representation:** Само 1 `category-compare` визуал (3-way). Умишлено минимална — safety level забранява интерактивност.
**Retrieval:** 3 recognition-only MCQ.
**Application:** `Not applicable` по дизайн (AcademicContextOnly забранява self-guided practice тук).
**Reasoning:** Минимален, съзнателно (темата изисква professional guidance, не self-directed reasoning).
**Assessment:** 3 MCQ, recognition-only, но подходящо тесен за scope-а на седмицата.
**Knowledge Map readiness:** `Confirmed` — core-belief вече `Revisited` тук (`#osnovno-vyarvane`).
**Migration class:** **D** (Light standardization) — почти отговаря на собствения си (умишлено тесен) стандарт; основната липса е Coverage Matrix, не съдържание/interactivity.

**Missing:** Coverage Matrix; committed git status (blocker преди retrofit).

**Recommended migration:** (1) **Първо: owner решение за uncommitted status** (commit as-is / revise-then-commit / discard) — нищо друго не трябва да зависи от некомитнато съдържание. (2) След commit: лек Coverage Matrix pass. (3) НЕ добавяй simulator/case/interactivity — противоречи на съзнателния AcademicContextOnly design boundary.

**Source actions required:** (след commit решение) Извличане на SRC-041 Ch. 14, Knowledge Units, Coverage Matrix.

**Candidate cognitive representations:** Няма нужда от нов тип — `category-compare` вече е подходяща форма.

**Retrieval opportunities:** (1) Лек matching/classification exercise (твърдение → категория), отвъд 3-те текущи MCQ. Не повече — представлението вече е representation-fit-appropriate за scope-а.

**Knowledge Map impact:** `Confirmed` за core-belief (revisit). `Candidate — needs source verification`: 3-те категории (безпомощност/необичаемост/безполезност) като собствени sub-nodes на core-belief — препоръка: **не добавяй** без owner authorization (§11 на оригиналната Phase 5 оторизация: "по-добре малка вярна карта" — тази гранулярност рискува да наруши тази дисциплина без ясна relational стойност).

---

### Week 6 — Структура на терапевтичната сесия (REFERENCE — виж §3)

**Migration class: E.** Не се променя.

---

## 6. Cross-week consistency issues

Идентифицирани, **не поправени**:

1. **Два паралелни concept-visualization механизма съществуват едновременно**: `.concept-map__flow` (статичен CSS chain, нула semantic model зад него — Седмици 3/8/10/12) срещу `ConceptGraph.razor` + `ConceptMapModel` (semantic-model-driven, Седмица 6 + global maps). И двата работят коректно, но представляват архитектурен duplication — retrofit-ът е естествена възможност да консолидира (owner decision, §24), не задължение.
2. **Retrieval/assessment дълбочина варира драстично без дефиниран минимум**: 0 structured checks (Седмица 8) → 3–4 recognition MCQ (Седмици 1/3/10/12) → 13 embedded + 20 diverse (Седмица 6). Няма documented "minimum acceptable retrieval bar" преди Седмица 6 да установи стандарта de facto.
3. **Knowledge-Unit/Coverage-Matrix numbering съществува само в Седмица 6.** Нито една от петте по-стари седмици може честно да claim-не "100% source coverage" — това е реален, не козметичен gap.
4. **Terminology treatment е inconsistent**: само Седмица 6 има дедикирана "Ключови понятия" карта секция; останалите обясняват термини inline, без консолидация.
5. **Emotion/Body-reaction naming нюанс — вече резолвнат, не нов проблем**: Седмица 3's собствена верига ги обединява в "Емоционална и телесна реакция", докато Седмица 8 ги разделя — това вече е документирано и целенасочено обработено в `KnowledgeMapCatalog.cs`'s собствен коментар (Emotion/Body-reaction `IntroducedWeek=8`, не 3). Споменато тук за пълнота, не като нов finding.
6. **"Основно вярване"/"Схема" naming — потвърдено consistent**, не проблем: Седмица 3 и Седмица 12 използват еднакви термини без drift.
7. **Simulator format metadata леко подценява реалната сложност**: `CourseCatalog.cs` label-ва Седмица 6 като `GuidedDemonstration`, но реално построеният `ScenarioSimulator` (Level A/B/C, case-based) е по-близо до `Simulator` категорията, отколкото до другите `GuidedDemonstration` седмици (5, 10). Дребна metadata-vs-реалност несъответствие, не блокираща.
8. **GAP-013** (Седмица 6, "шест цели" vs 5 клаузи) остава `Open`, недокоснат тук — вече tracked отделно в `15_GAPS_AND_CONFLICTS.md`.

---

## 7. Migration classification summary

| Седмица | Клас | Severity | Source readiness | Основен gap | Dependency |
|---|---|---|---|---|---|
| 3 | B | Висока (prerequisite-critical) | SRC-041 Ch.3 достъпен | Coverage Matrix + retrieval depth | Няма upstream; downstream: 6\*, 8, 10, 12, бъдеща 7 |
| 8 | B | Висока (retrieval почти липсва) | SRC-041 Ch.3/9/10 достъпен | Нула structured assessment | Depends on 3 |
| 10 | C | Средна | SRC-041 достъпен | Coverage Matrix + formalization | Depends on 3, 8 |
| 12 | D (⚠ uncommitted) | Ниска, но blocked от git статус | SRC-041 Ch.14 достъпен | Owner commit решение първо | Depends on 3 |
| 1 | C/B граница | Ниска (без downstream dependents) | SRC-041 + 1979 primary достъпни | Retrieval depth + Coverage Matrix | Няма dependents |
| 6 | E (reference) | — | — | — | — |

\* Седмица 6 вече е locked/complete — "downstream" тук означава конкретно **терминологична consistency**, не буквална зависимост (Седмица 6 е построена независимо и по-рано).

---

## 8. Recommended migration order

**3 → 8 → 10 → 12 (след owner git решение) → 1.**

Обосновка (§25 критерии, не week-number ред):
1. **Учебна зависимост / curriculum prerequisite:** Седмица 3 преподава йерархията, върху която 6, 8, 10, 12 (и бъдещата 7) directly реферират. Retrofit-вайки я първа намалява риска most downstream седмиците да наследят unknown-coverage несигурност.
2. **Severity of gap:** Седмица 8 има буквално нулев structured retrieval — по-остър gap от Седмица 3's "само тънък end-quiz".
3. **Source readiness:** И трите (3, 8, 10) използват вече добре познатия SRC-041 — нула нов source acquisition риск.
4. **Reusable component value:** Retrofit-вайки 3 и 8 заедно установява patterns (Coverage Matrix формат, terminology card, embedded retrieval) директно преизползваеми за 10, 12, 1 и за бъдещата Седмица 7.
5. **Risk of building бъдещи седмици върху лоша основа:** Седмица 7 (Поведенческа активация) ще реферира directly "Ситуация→Мисъл→Емоция→Поведение" веригата (3/8) и вече съществуващия "behavior" knowledge-map concept — retrofit-вайки 3/8 first директно de-risk-ва Седмица 7.
6. Седмица 12 е генерично нископриоритетна по съдържание, но **не може да чака безкрайно** заради git статуса — позиционирана след 10, преди 1, с explicit git gate.
7. Седмица 1 последна — няма downstream dependents, най-нисък риск ако се забави.

---

## 9. Migration batches

- **Batch A — Prerequisite-critical (retrofit преди Седмица 7):** Седмица 3, Седмица 8. Coverage Matrix + Knowledge Units минимум; пълен retrieval/terminology upgrade когато удобно.
- **Batch B — Central, но не gating:** Седмица 10. Formalization pass (Coverage Matrix, Process Diagram recognition), може да се случи паралелно с/след Седмица 7.
- **Batch C — Light standardization, git-gated:** Седмица 12. Изисква owner git решение първо; после лек Coverage Matrix pass.
- **Batch D — Standalone, lowest urgency:** Седмица 1. Може да изчака без риск за друга работа.

---

## 10. Minimum Stability Gate before Week 7

**Не** пълна retrofit на всичките 5 седмици. Конкретен, обоснован минимум:

1. Global standard стабилен — **вече постигнато** (Course Map/Knowledge Map owner-approved, тази сесия).
2. Source-extraction workflow доказан стабилен — **вече постигнато** (Седмица 6's `_source_corpus/` → Knowledge Units pattern работи).
3. **Седмица 3 И Седмица 8 имат завършен Coverage Matrix + Knowledge-Unit numbering pass** — конкретно защото Седмица 7 (Поведенческа активация) ще реферира точно тяхната терминология ("Ситуация", "Мисъл", "Поведение") и вероятно ще се свърже с вече съществуващия "behavior" Knowledge Map node. Пълен retrieval/assessment/terminology upgrade за 3/8 **не е задължителен преди Седмица 7** — само source-coverage доказателството.
4. Терминологична проверка: потвърдено тук (§6) — няма drift между Седмица 3's йерархия и другите седмици, използващи я. Няма допълнително действие нужно.

Това е **умишлено модест gate** — избягва "безкраен retrofit преди всякаква бъдеща работа" (§27/§37 на заданието), докато все пак адресира конкретния, доказан риск (unknown source coverage propagating forward в нова седмица).

---

## 11. When Week 7 may resume

**Препоръка: след Batch A (Седмица 3 + Седмица 8 Coverage Matrix pass), не след всичките 5 седмици.** Явна причина: Седмица 7 structurally депендва само на 3/8's core model, не на 10/12/1's съдържание. Изчакването на всичките 5 би било прекомерно консервативно спрямо реалната зависимост.

---

## 12. Knowledge Map growth strategy

За всяка seudmica — вижте съответния "Knowledge Map impact" ред в §5. Обобщено:

- **Confirmed, вече интегрирани:** всичките 10 текущи concepts (без промяна нужна).
- **Candidate — needs source verification** (owner решение изисква се преди добавяне, виж §24): Cognitive Triad (Седмица 3), Факт-срещу-интерпретация / Мисъл-срещу-чувство (Седмица 8), Декатастрофизиране / Балансиран отговор (Седмица 10), core-belief's 3 подкатегории (Седмица 12 — препоръка против добавяне).
- **Not applicable:** Седмица 1 (потвърдено, не преразглеждано).

**Метод:** никакво ново concept/relation не се добавя в код по време на самия retrofit без отделна source-verification стъпка, огледало на Phase 5's собствена дисциплина ("не измисляй relationships").

---

## 13. Course Map impact

**Минимален.** Course Map чете `CourseCatalog.Route`/`ModuleLabel` живо — веднъж щом Седмица 12's route бъде commit-нат, Course Map автоматично ще я покаже като `Introduced`, без никаква code промяна (`CourseMapBuilder.Build()` вече е чиста функция върху `CourseCatalog`). Retrofit на съдържание (Coverage Matrix, retrieval, terminology) не засяга Course Map изобщо — тя не отчита content depth.

---

## 14. Longitudinal case strategy

Ирина остава единственият одобрен pilot:

- **Седмица 8 — `potentially suitable after source review`.** Ситуация→Мисъл→Емоция→Поведение веригата вече точно съвпада с `CaseConceptualizationModel`'s data shape; седмицата в момента няма никакво case-based приложение — органично разширение.
- **Седмица 10 — `potentially suitable after source review`.** Сократическо изследване на една от Ирина's вече установени автоматични мисли би било естествено narrative продължение от Седмица 6's Case Lab.
- **Седмица 3 — не трябва да се използва.** Темата е абстрактният 3-нивов модел/triad; case би конкурирал за пространство с вече съществуващите interactive demos, не ги допълва.
- **Седмица 1 — не трябва да се използва.** Чисто исторически наратив.
- **Седмица 12 — не трябва да се използва.** AcademicContextOnly изрично забранява self-assessment framing; case-based "коя категория си ти" би противоречило на страницата's собствена decларирана граница.

Никаква бъдеща история не е измислена — само suitability флагове, per §23 на заданието.

---

## 15. Retrieval rollout strategy

Виж "Retrieval opportunities" във всеки week-specific audit (§5). Общ принцип: **1–3 механизма на седмица, представлението determined by existing content shape** (§22 на заданието — не всичко е quiz):

- Reconstruct/ordering exercises, преизползващи вече съществуващи sequence визуали (Timeline за Седмица 1, guided-practice-sequence за Седмица 10, concept-map__flow chains за Седмица 3/8).
- Explain-before-reveal разширения на вече съществуващи класификационни упражнения (Седмица 1/3/10).
- **Приоритет:** Седмица 8's секция 07 се нуждае от **първи** реален reveal-базиран check — в момента е единствената седмица с буквално нула структуриран retrieval.

---

## 16. Assessment rollout strategy

Не фиксиран брой въпроси за всички седмици (§24 на заданието). Минимален bar за всяка от петте: (1) поне 1 non-recognition тип въпрос (scenario/ordering/matching/true-false — Седмица 8 вече не покрива дори recognition); (2) explanatory feedback с source traceability (глава/секция, дори без пълна Knowledge-Unit номерация); (3) **Седмица 8 конкретно** се нуждае от базов reveal механизъм преди всичко друго. Седмица 6's 20-въпросен диапазон **не** е targeting — Representation Fit решава обхвата за всяка седмица индивидуално.

---

## 17. Visual/cognitive representation rollout strategy

Не добавяй Concept/Process/Decision/Comparison диаграма "защото standard-ът я има" (§17 на заданието — explicit забрана). Конкретно:

- Седмица 3/8/10/12's съществуващи `.concept-map__flow`/`category-compare`/`guided-practice-sequence` визуали са вече **legitimate representations**, не "styled cards act­ing as visuals" — не се нуждаят от замяна само за симетрия.
- Единствената реална архитектурна промяна с истинска стойност: **owner decision дали да се upgrade-нат `.concept-map__flow` инстанциите (3/8/10) към `ConceptGraph`/`ConceptMapModel`** за консистентност с global map стандарта (§24, owner решение — реален effort trade-off, не автоматично "да").
- Седмица 10's `guided-practice-sequence` вече е готов Process Diagram — needs formalization (documentation/naming), не rebuild.
- Никъде не е идентифициран нужда от нов Decision Tree измежду петте (нито една реално описва if/then clinical logic отвъд вече наличния съдържание).

---

## 18. Source extraction workflow

Доказаният Week 6 workflow (`_source_corpus/` → gitignored → Knowledge-Unit extraction) се прилага по следния план **за планиране само, не за изпълнение сега**:

1. За всяка от Batch A седмиците (3, 8) — пълно извличане на релевантните SRC-041 глави (Ch. 3 за Седмица 3; Ch. 3/9/10 за Седмица 8 — вече три различни глави, потенциално най-голямото extraction усилие сред петте) в локален `_source_corpus/week-03/` / `_source_corpus/week-08/`, gitignored.
2. Knowledge-Unit номерация (U01, U02...) directly срещу извлечения текст, огледало на Седмица 6's U01-U46 конвенция.
3. Coverage Matrix, съпоставящ всеки Knowledge Unit срещу конкретен `<h2 id=...>` секция в съответната `.razor` страница — доказва (или разкрива gap в) 100% source representation.
4. Повторение за Batch B (10) и Batch C (12, след git решение) и Batch D (1, с добавка на 1979 primary source).

**Не се извлича нищо в тази сесия** — това е план, не изпълнение.

---

## 19. Future Deep Learning Week workflow

Финален, приет workflow за всяка НОВА седмица от Седмица 7 нататък (уточнена версия на заданието's §32):

1. Real source extraction / full read → `_source_corpus/` (gitignored).
2. Knowledge Units номерация.
3. Coverage Matrix (Knowledge Unit ↔ page section).
4. Terminology Map (дедикирана "Ключови понятия" секция).
5. Cognitive Representation Analysis (кои concepts се нуждаят от hierarchy/relationship/sequence/conditional-logic визуализация).
6. Weekly Mind Map (preview state).
7. Concept / Process / Decision / Comparison визуали, само където content-ът реално ги оправдава (Representation Fit).
8. Case application (Ирина, ако е "potentially suitable", или нов case, само с owner authorization).
9. Retrieval Practice план (1-3 механизма, не автоматично quiz).
10. Simulator/application, само където Interactive Format-ът от `CourseCatalog` го оправдава.
11. Assessment blueprint (тип разнообразие + source traceability, обхват determined by седмицата, не fixed count).
12. Owner blueprint review.
13. Implementation.
14. Technical QA (build 0/0, tests green, route 200).
15. Source QA (Coverage Matrix verified срещу реалния краен текст).
16. Accessibility QA.
17. Visual QA (browser, ако достъпен; иначе explicit "Structural QA only" disclosure).
18. Owner learning review.
19. Knowledge Map metadata integration (само Confirmed concepts, source-verified).
20. `COMPLETE`.

---

## 20. Deprecated workflow

Изрично маркиран deprecated, **не приемлив за нова работа**:

> source note → short lesson → 2–3 questions → tests → `COMPLETE`

Тази последователност произвежда точно Generation-0 резултата, документиран в §1/§5 по-горе — текстово коректен, но без source-coverage доказателство, без embedded retrieval, без cognitive representation отвъд статичен CSS chain.

---

## 21. Risk register

- **Content drift** — retrofit трябва да ДОБАВЯ (Coverage Matrix, terminology, retrieval), не тихо да пренаписва вече source-verified, owner-approved текст.
- **Visual overproduction** — добавяне на maps/диаграми "за симетрия" без реална multi-relation съдържателна база (explicit забранено, §17).
- **Source gaps** — Coverage Matrix работата може да разкрие реални липси, изискващи допълнително четене отвъд текущия обхват — buffer нужен в планирането.
- **Duplicate/divergent metadata** — ако Седмица 3/8/10 upgrade-нат локалните си `.concept-map__flow` към `ConceptMapModel`, трябва да преизползват точно същите node IDs/labels като `KnowledgeMapCatalog.cs`, не паралелни копия.
- **Excessive retrofit** — превръщане на Седмица 1 или 12 в Седмица-6-клонинг противоречи на Representation Fit.
- **Week 12 working-tree risk** — некомитнато съдържание може да бъде изгубено от невнимателна git операция; retrofit не трябва да го докосва преди explicit owner git решение.
- **Terminology drift risk** — нови terminology секции в старите седмици трябва да се cross-check-ват срещу вече locked Седмица 6/global map дефиниции, не да ги противоречат случайно.
- **Test-suite regression risk** — всеки retrofit трябва да поддържа текущия 510/510 baseline; нови тестове се добавят, не replace-ват.
- **Scope creep в Седмица 7** — начало на Седмица 7 преди Batch A gate (§10) пропагира unknown-coverage риск напред.

---

## 22. Implementation sequence (след owner approval)

1. Owner review на този blueprint (текущата стъпка).
2. Owner git решение за Седмица 12 (§24, точка 3).
3. Owner решения за Knowledge Map candidate concepts (§24, точка 4) и `.concept-map__flow` upgrade обхват (§24, точка 5).
4. Отделна, нова оторизирана задача: **Седмица 3 retrofit** (Coverage Matrix + Knowledge Units минимум; retrieval/terminology upgrade ако owner избере пълен Class B scope).
5. Отделна задача: **Седмица 8 retrofit** (същия формат, приоритет върху reveal-based assessment).
6. Minimum Stability Gate проверка (§10) → Седмица 7 може да започне паралелно с/след това.
7. Batch B (Седмица 10), после Batch C (Седмица 12, ако git решен), после Batch D (Седмица 1) — не блокиращи Седмица 7.

---

## 23. Project OS updates needed later

След всеки бъдещ retrofit: нов `10_SESSION_LOG.md` запис (по вече установената конвенция), `02_CURRENT_STATUS.md` checkpoint update, евентуален нов ред в `11_SOURCE_REGISTER.md`/Coverage Matrix документ (ако не съществува, ще се нуждае от нов файл при първия Coverage Matrix pass — **не** създаден тук, само отбелязан като бъдеща нужда).

---

## 24. Owner decisions required

1. **Одобри/отхвърли Minimum Stability Gate дефиницията** (§10 — само Седмица 3+8 Coverage Matrix, не пълен Class B, преди Седмица 7).
2. **Одобри/отхвърли предложения migration order** (§8 — 3→8→10→12→1).
3. **Реши съдбата на Седмица 12's uncommitted working tree**: commit as-is / revise-then-commit / discard — owner решение, не мое.
4. **Потвърди/отхвърли Knowledge Map candidate concepts** (§12/§14 във всеки week audit): Cognitive Triad, Факт-vs-интерпретация, Мисъл-vs-чувство, Декатастрофизиране, Балансиран отговор, core-belief's 3 подкатегории.
5. **Одобри/отхвърли upgrade на `.concept-map__flow` (Седмици 3/8/10/12) към `ConceptGraph`/`ConceptMapModel`** — архитектурна консистентност срещу реален допълнителен effort.
6. **Реши дали Ирина да се разшири в Седмица 8 и/или 10** (и двете флагнати "potentially suitable after source review") — или остава single-week pilot.

---

## 25. Recommendation

`READY FOR OWNER ROLLOUT REVIEW: YES`
